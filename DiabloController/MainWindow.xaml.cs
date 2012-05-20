using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Xna.Framework.Input;
using System.Windows.Threading;
using System.Windows.Interop;
using System.Runtime.InteropServices;
using System.Diagnostics;


namespace DiabloController
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private GamePadState gamePadState = new GamePadState();
        private JoyAPI.JOYINFOEX infoEx = new JoyAPI.JOYINFOEX();
        private DispatcherTimer timer = new DispatcherTimer();
        private DispatcherTimer timerQ = new DispatcherTimer();

        private int controllerType = 0;
        private string ver = "0.0.10b";

        private bool left = false;
        private bool right = false;
        private bool stand = false;

        private bool key1 = false;
        private bool key2 = false;
        private bool key3 = false;
        private bool key4 = false;
        private bool key5 = false;
        private bool key6 = false;
        private bool key7 = false;
        private bool key8 = false;
        private bool key9 = false;
        private bool key10 = false;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Hyperlink_Click(object sender, RequestNavigateEventArgs e) 
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
            verlink.Inlines.Add("VER:" + ver);

            infoEx.dwSize = Marshal.SizeOf(typeof(JoyAPI.JOYINFOEX));
            infoEx.dwFlags = 0x0080;


            timer.Interval = TimeSpan.FromMilliseconds(5);
            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();


            timerQ.Interval = TimeSpan.FromMilliseconds(10);
            timerQ.Tick += new EventHandler(timerQ_Tick);
            timerQ.Start();

            textBox1.Text = bloodLeft + ",-" + bloodHeight;


            gfxDisplay = System.Drawing.Graphics.FromHdc(hdlDisplay);
            bmp = new System.Drawing.Bitmap(1, bloodHeight, gfxDisplay);
            gfxBmp = System.Drawing.Graphics.FromImage(bmp);

            slider3.Value = 30;
        }


        int bloodLeft = 430;
        int bloodHeight = 160;
        IntPtr hdlDisplay = CreateDC("DISPLAY", null, null, IntPtr.Zero);
        System.Drawing.Graphics gfxDisplay;
        System.Drawing.Bitmap bmp;
        System.Drawing.Graphics gfxBmp;
        string temp = "";
        int mark = 0;
        IntPtr hdlScreen;
        IntPtr hdlBmp;

        void timerQ_Tick(object sender, EventArgs e)
        {
            if (checkBox1.IsChecked.Value)
            {
                hdlScreen = gfxDisplay.GetHdc();
                hdlBmp = gfxBmp.GetHdc();

                BitBlt(hdlBmp, 0, 0, 1, bloodHeight, hdlScreen, bloodLeft, (int)SystemParameters.PrimaryScreenHeight - bloodHeight, 13369376);
                gfxDisplay.ReleaseHdc(hdlScreen);
                gfxBmp.ReleaseHdc(hdlBmp);

                temp = "";
                for (int i = 0; i < bloodHeight; i++)
                {
                    byte red = bmp.GetPixel(0, i).R;
                    if (red > 0xff - slider3.Value)
                    //if (red > 0xf0)
                    {
                        temp += "1";
                    }
                    else
                    {
                        temp += "0";
                    }

                }
                //textBox2.Text = temp;

                if (mark + 3 < temp.LastIndexOf('1'))
                {
                    vibration = new System.Threading.Thread(new System.Threading.ThreadStart(Vibration));
                    vibration.Start();
                }
                mark = temp.LastIndexOf('1');
            }
        }
        System.Threading.Thread vibration;
       

        void Vibration() 
        {
            if (controllerType == 1)
            {
                Microsoft.Xna.Framework.Input.GamePad.SetVibration(0, (float)0.5, (float)0.5);
                System.Threading.Thread.Sleep(500);
                Microsoft.Xna.Framework.Input.GamePad.SetVibration(0, (float)0.0, (float)0.0);
            }
        }

        void timer_Tick(object sender, EventArgs e)
        {
            TestController();
        }

        private void TestController()
        {
            gamePadState = GamePad.GetState(0);
            // 优先判断360控制器
            if (gamePadState.IsConnected)
            {
                if (controllerType != 1)
                {
                    image1.Source = new BitmapImage(new Uri(@"xbox.png", UriKind.RelativeOrAbsolute));
                    controllerType = 1;
                    this.Title = "暗黑破坏神3控制器（已连接XBOX游戏控制器）";
                    checkBox1.IsEnabled = true;
                }
                Xbox(gamePadState);
            }
            else 
            {
                // 判断其他控制器
                int result =JoyAPI.joyGetPosEx(0, ref infoEx);

                if (result == 0)
                {
                    if (controllerType != 2)
                    {
                        image1.Source = new BitmapImage(new Uri(@"other.png", UriKind.RelativeOrAbsolute));
                        controllerType = 2;
                        this.Title = "暗黑破坏神3控制器（已连接普通游戏控制器）";
                        checkBox1.IsEnabled = false;
                        checkBox1.IsChecked = false;
                    }


                    // 移动
                    Diablo.Move(
                        leftH.IsChecked.Value ? -newXY(infoEx.dwXpos) : newXY(infoEx.dwXpos),
                        leftZ.IsChecked.Value ? -newXY(infoEx.dwYpos) : newXY(infoEx.dwYpos), 
                        stand, 
                        (int)slider1.Value, 
                        (int)slider2.Value);
                    Diablo.MouseMove(
                        rightH.IsChecked.Value ? -newXY(infoEx.dwRpos) : newXY(infoEx.dwRpos),
                        rightZ.IsChecked.Value ? -newXY(infoEx.dwZpos) : newXY(infoEx.dwZpos));

                    if ((infoEx.dwButtons & JoyAPI.JOY_BUTTON3) == JoyAPI.JOY_BUTTON3)
                    {
                        Diablo.MouseLeftDown(ref left);
                    }
                    else
                    {
                        Diablo.MouseLeftUp(ref left);
                    }

                    if ((infoEx.dwButtons & JoyAPI.JOY_BUTTON2) == JoyAPI.JOY_BUTTON2)
                    {
                        Diablo.MouseRightDown(ref right);
                    }
                    else
                    {
                        Diablo.MouseRightUp(ref right);
                    }


                    if ((infoEx.dwButtons & JoyAPI.JOY_BUTTON4) == JoyAPI.JOY_BUTTON4)
                    {
                        Diablo.Key1(ref key1);
                    }
                    else
                    {
                        key1 = false;
                    }
                    if ((infoEx.dwButtons & JoyAPI.JOY_BUTTON1) == JoyAPI.JOY_BUTTON1)
                    {
                        Diablo.Key2(ref key2);
                    }
                    else
                    {
                        key2 = false;
                    }

                    // 站立攻击
                    if ((infoEx.dwButtons & JoyAPI.JOY_BUTTON7) == JoyAPI.JOY_BUTTON7)
                    {
                        Diablo.StandDown(ref stand);
                    }
                    else
                    {
                        Diablo.StandUp(ref stand);
                    }

                    if ((infoEx.dwButtons & JoyAPI.JOY_BUTTON6) == JoyAPI.JOY_BUTTON6)
                    {
                        Diablo.Key4(ref key4);
                    }
                    else
                    {
                        key4 = false;
                    }
                    if ((infoEx.dwButtons & JoyAPI.JOY_BUTTON8) == JoyAPI.JOY_BUTTON8)
                    {
                        Diablo.Key3(ref key3);
                    }
                    else
                    {
                        key3 = false;
                    }

                    if ((infoEx.dwButtons & JoyAPI.JOY_BUTTON5) == JoyAPI.JOY_BUTTON5)
                    {
                        Diablo.KeyQ(ref key5);
                    }
                    else
                    {
                        key5 = false;
                    }

                    // 重置血量监视点
                    if ((infoEx.dwButtons & JoyAPI.JOY_BUTTON9) == JoyAPI.JOY_BUTTON9)
                    {
                        bloodLeft = System.Windows.Forms.Control.MousePosition.X;
                        bloodHeight = (int)SystemParameters.PrimaryScreenHeight - System.Windows.Forms.Control.MousePosition.Y;

                        textBox1.Text = bloodLeft + ",-" + bloodHeight;
                    }
                   

                    if (infoEx.dwPOV == JoyAPI.JOY_BUTTONUP)
                    {
                        // 上
                        Diablo.KeyM(ref key7);
                    }
                    else
                    {
                        key7 = false;
                    }
                    
                    if (infoEx.dwPOV == JoyAPI.JOY_BUTTONDOWN)
                    {
                        // 下
                        Diablo.KeyX(ref key8);
                    }
                    else
                    {
                        key8 = false;
                    }
                    
                    if (infoEx.dwPOV == JoyAPI.JOY_BUTTONLEFT)
                    {
                        // 左
                        Diablo.KeyT(ref key9);
                    }
                    else
                    {
                        key9 = false;
                    }
                    
                    if (infoEx.dwPOV == JoyAPI.JOY_BUTTONRIGHT)
                    {
                        // 右
                        Diablo.KeyI(ref key10);
                    }
                    else
                    {
                        key10 = false;
                    }
                }
                else 
                {
                    image1.Source = new BitmapImage(new Uri(@"null.png", UriKind.RelativeOrAbsolute));
                    controllerType = 0;
                    this.Title = "暗黑破坏神3控制器（未连接游戏控制器）";
                    checkBox1.IsEnabled = false;
                    checkBox1.IsChecked = false;
                }
            }
        }

        private float newXY(int num)
        {
            return (float)((num - 32767.5) / 32767.5);
        }

        private void Xbox(GamePadState gamePadState)
        {
            // 移动
            Diablo.Move(
                leftH.IsChecked.Value ? -gamePadState.ThumbSticks.Left.X : gamePadState.ThumbSticks.Left.X,
                leftZ.IsChecked.Value ? gamePadState.ThumbSticks.Left.Y : -gamePadState.ThumbSticks.Left.Y, 
                stand, 
                (int)slider1.Value, 
                (int)slider2.Value);
            Diablo.MouseMove(
                rightH.IsChecked.Value ? -gamePadState.ThumbSticks.Right.X : gamePadState.ThumbSticks.Right.X,
                rightZ.IsChecked.Value ? gamePadState.ThumbSticks.Right.Y : -gamePadState.ThumbSticks.Right.Y);

            // 普通攻击
            if (gamePadState.Buttons.A == ButtonState.Pressed)
            {
                Diablo.MouseLeftDown(ref left);
            }
            else 
            {
                Diablo.MouseLeftUp(ref left);
            }


            // 特殊攻击
            if (gamePadState.Buttons.B == ButtonState.Pressed)
            {
                Diablo.MouseRightDown(ref right);
            }
            else
            {
                Diablo.MouseRightUp(ref right);
            }

            // 1234
            if (gamePadState.Buttons.X == ButtonState.Pressed)
            {
                Diablo.Key1(ref key1);
            }
            else 
            {
                key1 = false;
            }

            if (gamePadState.Buttons.Y == ButtonState.Pressed)
            {
                Diablo.Key2(ref key2);
            }
            else
            {
                key2 = false;
            }

            // 站立攻击
            if (gamePadState.Triggers.Left > 0.5)
            {
                Diablo.StandDown(ref stand);
            }
            else
            {
                Diablo.StandUp(ref stand);
            }

            if (gamePadState.Buttons.RightShoulder == ButtonState.Pressed)
            {
                Diablo.Key3(ref key3);
            }
            else
            {
                key3 = false;
            }
            if (gamePadState.Triggers.Right > 0.5)
            {
                Diablo.Key4(ref key4);
            }
            else
            {
                key4 = false;
            }

            // 喝血瓶
            if (gamePadState.Buttons.LeftShoulder == ButtonState.Pressed)
            {
                Diablo.KeyQ(ref key6);
            }
            else
            {
                key6 = false;
            }


            if (gamePadState.DPad.Up == ButtonState.Pressed)
            {
                Diablo.KeyM(ref key7);
            }
            else
            {
                key7 = false;
            }

            if (gamePadState.DPad.Down == ButtonState.Pressed)
            {
                Diablo.KeyX(ref key8);
            }
            else
            {
                key8 = false;
            }

            if (gamePadState.DPad.Left == ButtonState.Pressed)
            {
                Diablo.KeyT(ref key9);
            }
            else
            {
                key9 = false;
            }

            if (gamePadState.DPad.Right == ButtonState.Pressed)
            {
                Diablo.KeyI(ref key10);
            }
            else
            {
                key10 = false;
            }

            // 重置血量监视点
            if (gamePadState.Buttons.Back == ButtonState.Pressed)
            {
                bloodLeft = System.Windows.Forms.Control.MousePosition.X;
                bloodHeight = (int)SystemParameters.PrimaryScreenHeight - System.Windows.Forms.Control.MousePosition.Y;

                textBox1.Text = bloodLeft + ",-" + bloodHeight;
            }
        }



        [DllImport("gdi32.dll")]
        private static extern bool BitBlt(
                    IntPtr hdcDest,  //  目标设备的句柄  
                    int nXDest,  //  目标对象的左上角的X坐标  
                    int nYDest,  //  目标对象的左上角的X坐标  
                    int nWidth,  //  目标对象的矩形的宽度  
                    int nHeight,  //  目标对象的矩形的长度  
                    IntPtr hdcSrc,  //  源设备的句柄  
                    int nXSrc,  //  源对象的左上角的X坐标  
                    int nYSrc,  //  源对象的左上角的X坐标  
                    int dwRop  //  光栅的操作值  
        );

        [DllImport("gdi32.dll ")]
        static public extern uint GetPixel(IntPtr hDC, int XPos, int YPos); 

        [DllImport("gdi32.dll")]
        private static extern IntPtr CreateDC(
                    string lpszDriver,  //  驱动名称  
                    string lpszDevice,  //  设备名称  
                    string lpszOutput,  //  无用，可以设定位"NULL"  
                    IntPtr lpInitData  //  任意的打印机数据  
        );

        [DllImport("gdi32.dll ")]
        static public extern bool DeleteDC(IntPtr DC);

        private void slider3_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            textBox3.Text = slider3.Value.ToString();
        } 
    }
}

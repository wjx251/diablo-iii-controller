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
        private DispatcherTimer timer = new DispatcherTimer();

        private int selectedPlayer = 0;
        private int controllerType = 0;
        private string ver = "0.0.5";

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

            timer.Interval = TimeSpan.FromMilliseconds(20);
            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();
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
                }
                Xbox(gamePadState);
            }
            else 
            {
                // 判断其他控制器
                JoyAPI.JOYINFOEX infoEx = new JoyAPI.JOYINFOEX();
                infoEx.dwSize = Marshal.SizeOf(typeof(JoyAPI.JOYINFOEX));
                infoEx.dwFlags = 0x0080;
                int result =JoyAPI.joyGetPosEx(0, ref infoEx);

                if (result == 0)
                {
                    if (controllerType != 2)
                    {
                        image1.Source = new BitmapImage(new Uri(@"other.png", UriKind.RelativeOrAbsolute));
                        controllerType = 2;
                        this.Title = "暗黑破坏神3控制器（已连接普通游戏控制器）";
                    }





                    if ((infoEx.dwButtons & JoyAPI.JOY_BUTTON1) == JoyAPI.JOY_BUTTON1)
                    {
                        Diablo.Key2();
                    }
                    if ((infoEx.dwButtons & JoyAPI.JOY_BUTTON2) == JoyAPI.JOY_BUTTON2)
                    {
                        Diablo.MouseRightClick();
                    }
                    if ((infoEx.dwButtons & JoyAPI.JOY_BUTTON3) == JoyAPI.JOY_BUTTON3)
                    {
                        Diablo.MouseLeftClick();
                    }
                    if ((infoEx.dwButtons & JoyAPI.JOY_BUTTON4) == JoyAPI.JOY_BUTTON4)
                    {
                        Diablo.Key1();
                    }

                    if ((infoEx.dwButtons & JoyAPI.JOY_BUTTON5) == JoyAPI.JOY_BUTTON5)
                    {
                        Diablo.KeyQ();
                    }
                    if ((infoEx.dwButtons & JoyAPI.JOY_BUTTON6) == JoyAPI.JOY_BUTTON5)
                    {
                        Diablo.KeyQ();
                    }

                    if ((infoEx.dwButtons & JoyAPI.JOY_BUTTON7) == JoyAPI.JOY_BUTTON7)
                    {
                        Diablo.Key3();
                    }
                    if ((infoEx.dwButtons & JoyAPI.JOY_BUTTON8) == JoyAPI.JOY_BUTTON8)
                    {
                        Diablo.Key4();
                    }

                    if (infoEx.dwPOV == JoyAPI.JOY_BUTTONUP)
                    {
                        // 上
                        Diablo.KeyM();
                    }
                    else if (infoEx.dwPOV == JoyAPI.JOY_BUTTONDOWN)
                    {
                        // 下
                        Diablo.KeyX();
                    }
                    else if (infoEx.dwPOV == JoyAPI.JOY_BUTTONLEFT)
                    {
                        // 左
                        Diablo.KeyT();
                    }
                    else if (infoEx.dwPOV == JoyAPI.JOY_BUTTONRIGHT)
                    {
                        // 右
                        Diablo.KeyM();
                    }

                    // 移动
                    Diablo.Move(newXY(infoEx.dwXpos), newXY(infoEx.dwYpos));
                    Diablo.MouseMove(newXY(infoEx.dwRpos), newXY(infoEx.dwZpos));


                    this.Title = Math.Cos(Math.Atan(newXY(infoEx.dwYpos) / newXY(infoEx.dwXpos))).ToString();
                }
                else 
                {
                    image1.Source = new BitmapImage(new Uri(@"null.png", UriKind.RelativeOrAbsolute));
                    controllerType = 0;
                    this.Title = "暗黑破坏神3控制器（未连接游戏控制器）";
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
            Diablo.Move(gamePadState.ThumbSticks.Left.X, -gamePadState.ThumbSticks.Left.Y);
            Diablo.MouseMove(gamePadState.ThumbSticks.Right.X, -gamePadState.ThumbSticks.Right.Y);

            // 普通攻击
            if (gamePadState.Buttons.A == ButtonState.Pressed)
            {
                Diablo.MouseLeftClick();
            }


            // 特殊攻击
            if (gamePadState.Buttons.B == ButtonState.Pressed)
            {
                Diablo.MouseRightClick();
            }

            // 1234
            if (gamePadState.Buttons.X == ButtonState.Pressed)
            {
                Diablo.Key1();
            }
            if (gamePadState.Buttons.Y == ButtonState.Pressed)
            {
                Diablo.Key2();
            }
            if (gamePadState.Triggers.Left > 0.5)
            {
                Diablo.Key3();
            }
            if (gamePadState.Triggers.Right > 0.5)
            {
                Diablo.Key4();
            }

            // 喝血瓶
            if (gamePadState.Buttons.LeftShoulder == ButtonState.Pressed)
            {
                Diablo.KeyQ();
            }
            if (gamePadState.Buttons.RightShoulder == ButtonState.Pressed)
            {
                Diablo.KeyQ();
            }
        }
    }
}

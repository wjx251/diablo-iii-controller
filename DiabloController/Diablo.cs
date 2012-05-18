using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace DiabloController
{
    class Diablo
    {
        static bool LEFTDOWN = false;
        static bool RIGHTDOWN = false;

        static int ScreenCenterY = (int)SystemParameters.PrimaryScreenHeight / 2;
        static int ScreenCenterX = (int)SystemParameters.PrimaryScreenWidth / 2;

        // 方向控制
        static public void Move(float x, float y)
        {
            

            if ((System.Math.Abs(x) > 0.1) || (System.Math.Abs(y) > 0.1))
            {
                // 作用范围
                if (x * x + y * y > 1)
                {
                    if (x > 0)
                    {
                        x = (float)Math.Cos(Math.Atan(y / x));
                        y = (float)Math.Sin(Math.Atan(y / x));
                    }
                    else
                    {
                        x = -(float)Math.Cos(Math.Atan(y / x));
                        y = -(float)Math.Sin(Math.Atan(y / x));
                    }
                }

                SetCursorPos((int)(x * 200) + ScreenCenterX, (int)(y * 200) + ScreenCenterY);
            }
        }

        // 鼠标移动
        static public void MouseMove(float x, float y)
        {
            if ((System.Math.Abs(x) > 0.1) || (System.Math.Abs(y) > 0.1))
            {
                mouse_event(MOUSEEVENTF_MOVE, (int)(x * 20), (int)(y * 20), 0, 0);
            }
        }

        // 普通攻击
        static public void MouseLeftClick()
        {
            MouseLeftDown();
            MouseLeftUp();
        }
        static public void MouseLeftDown()
        {
            if (!LEFTDOWN)
            {
                System.Windows.Forms.SendKeys.SendWait("+");
                mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
                LEFTDOWN = true;
            }
        }
        static public void MouseLeftUp()
        {
            if (LEFTDOWN)
            {
                System.Windows.Forms.SendKeys.SendWait("+");
                mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
                LEFTDOWN = false;
            }
        }

        // 右键攻击
        static public void MouseRightClick()
        {
            MouseRightDown();
            MouseRightUp();
        }
        static public void MouseRightDown()
        {
            if (!RIGHTDOWN)
            {
                mouse_event(MOUSEEVENTF_RIGHTDOWN, 0, 0, 0, 0);
                RIGHTDOWN = true;
            }
        }
        static public void MouseRightUp()
        {
            if (RIGHTDOWN)
            {
                mouse_event(MOUSEEVENTF_RIGHTUP, 0, 0, 0, 0);
                RIGHTDOWN = false;
            }
        }

        // 快捷键1
        static public void Key1()
        {
            System.Windows.Forms.SendKeys.SendWait("1");
        }
        // 快捷键2
        static public void Key2()
        {
            System.Windows.Forms.SendKeys.SendWait("2");
        }
        // 快捷键3
        static public void Key3()
        {
            System.Windows.Forms.SendKeys.SendWait("3");
        }
        // 快捷键4
        static public void Key4()
        {
            System.Windows.Forms.SendKeys.SendWait("4");
        }
        // 快捷键血瓶
        static public void KeyQ()
        {
            System.Windows.Forms.SendKeys.SendWait("q");
        }

        // 地图
        static public void KeyM()
        {
            System.Windows.Forms.SendKeys.SendWait("m");
        }
        // 退出
        static public void KeyX()
        {
            System.Windows.Forms.SendKeys.SendWait("{ESC}");
        }
        // 传送门
        static public void KeyT()
        {
            System.Windows.Forms.SendKeys.SendWait("t");
        }

        // 通过API控制鼠标
        [System.Runtime.InteropServices.DllImport("user32.dll", EntryPoint = "SetCursorPos")]
        private static extern int SetCursorPos(int x, int y);

        [System.Runtime.InteropServices.DllImport("user32")]
        private static extern int mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);
        const int MOUSEEVENTF_MOVE = 0x0001;
        const int MOUSEEVENTF_LEFTDOWN = 0x0002;
        const int MOUSEEVENTF_LEFTUP = 0x0004;
        const int MOUSEEVENTF_RIGHTDOWN = 0x0008;
        const int MOUSEEVENTF_RIGHTUP = 0x0010;
        const int MOUSEEVENTF_MIDDLEDOWN = 0x0020;
        const int MOUSEEVENTF_MIDDLEUP = 0x0040;
        const int MOUSEEVENTF_ABSOLUTE = 0x8000;
    }
}

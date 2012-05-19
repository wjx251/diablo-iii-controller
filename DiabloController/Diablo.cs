using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace DiabloController
{
    class Diablo
    {
        static int ScreenCenterY = (int)SystemParameters.PrimaryScreenHeight / 2;
        static int ScreenCenterX = (int)SystemParameters.PrimaryScreenWidth / 2;

        static uint KEYEVENTF_KEYUP = 0x2;

        // 方向控制
        static public void Move(float x, float y, bool stand)
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
                SetCursorPos((int)(x * 200) + ScreenCenterX, (int)(y * 200) + ScreenCenterY - 40);

                if (!stand)
                {
                    keybd_event(0x58, 0, 0, 0);
                }
                else
                {
                    keybd_event(0x58, 0, KEYEVENTF_KEYUP, 0);
                }
            }
            else 
            {
                keybd_event(0x58, 0, KEYEVENTF_KEYUP, 0);
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


        // 点击
        static public void MouseLeftClick()
        {
            mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
        }

        // 站立
        static public void StandDown(ref bool stand)
        {
            if (!stand)
            {
                keybd_event(0x10, 0, 0, 0);
                stand = true;
            }
        }
        static public void StandUp(ref bool stand)
        {
            if (stand)
            {
                keybd_event(0x10, 0, KEYEVENTF_KEYUP, 0);
                stand = false;
            }
        }

        // 普通攻击
        static public void MouseLeftDown(ref bool left)
        {
            if (!left)
            {
                mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
                left = true;
            }
        }
        static public void MouseLeftUp(ref bool left)
        {
            if (left)
            {
                mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
                left = false;
            }
        }

        // 右键攻击
        static public void MouseRightDown(ref bool right)
        {
            if (!right)
            {
                mouse_event(MOUSEEVENTF_RIGHTDOWN, 0, 0, 0, 0);
                right = true;
            }
        }
        static public void MouseRightUp(ref bool right)
        {
            if (right)
            {
                mouse_event(MOUSEEVENTF_RIGHTUP, 0, 0, 0, 0);
                right = false;
            }
        }

        // 快捷键1
        static public void Key1(ref bool key)
        {
            if (!key)
            {
                System.Windows.Forms.SendKeys.SendWait("1");
                key = true;
            }
        }
        // 快捷键2
        static public void Key2(ref bool key)
        {
            if (!key)
            {
                System.Windows.Forms.SendKeys.SendWait("2");
                key = true;
            }
        }
        // 快捷键3
        static public void Key3(ref bool key)
        {
            if (!key)
            {
                System.Windows.Forms.SendKeys.SendWait("3");
                key = true;
            }
        }
        // 快捷键4
        static public void Key4(ref bool key)
        {
            if (!key)
            {
                System.Windows.Forms.SendKeys.SendWait("4");
                key = true;
            }
        }
        // 快捷键血瓶
        static public void KeyQ(ref bool key)
        {
            if (!key)
            {
                System.Windows.Forms.SendKeys.SendWait("q");
                key = true;
            }
        }

        // 地图
        static public void KeyM(ref bool key)
        {
            if (!key)
            {
                System.Windows.Forms.SendKeys.SendWait("m");
                key = true;
            }
        }
        // 退出
        static public void KeyX(ref bool key)
        {
            if (!key)
            {
                System.Windows.Forms.SendKeys.SendWait("{ESC}");
                key = true;
            }
        }
        // 传送门
        static public void KeyT(ref bool key)
        {
            if (!key)
            {
                System.Windows.Forms.SendKeys.SendWait("t");
                key = true;
            }
        }
        // 背包
        static public void KeyI(ref bool key)
        {
            if (!key)
            {
                System.Windows.Forms.SendKeys.SendWait("i");
                key = true;
            }
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

        // 通过API控制键盘
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, uint dwExtraInfo);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern byte MapVirtualKey(byte wCode, int wMap);
    }
}

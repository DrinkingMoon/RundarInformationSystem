using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;

namespace UniversalControlLibrary
{
    internal class NativeMethods
    {
        public const int IDANI_CAPTION = 0x3;

        public const int WM_SYSCOMMAND = 0x0112;

        public const uint SWP_NOZORDER = 0x0004;
        public const uint SWP_NOREDRAW = 0x0008;

        public const int SW_HIDE = 0x0;
        public const int SW_NORMAL = 0x1;

        public enum SystemCommands
        {
            SC_SIZE = 0xF000,
            SC_MOVE = 0xF010,
            SC_MINIMIZE = 0xF020,
            SC_MAXIMIZE = 0xF030,
            SC_MAXIMIZE2 = 0xF032,	// fired from double-click on caption
            SC_NEXTWINDOW = 0xF040,
            SC_PREVWINDOW = 0xF050,
            SC_CLOSE = 0xF060,
            SC_VSCROLL = 0xF070,
            SC_HSCROLL = 0xF080,
            SC_MOUSEMENU = 0xF090,
            SC_KEYMENU = 0xF100,
            SC_ARRANGE = 0xF110,
            SC_RESTORE = 0xF120,
            SC_RESTORE2 = 0xF122,	// fired from double-click on caption
            SC_TASKLIST = 0xF130,
            SC_SCREENSAVE = 0xF140,
            SC_HOTKEY = 0xF150,

            SC_DEFAULT = 0xF160,
            SC_MONITORPOWER = 0xF170,
            SC_CONTEXTHELP = 0xF180,
            SC_SEPARATOR = 0xF00F
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;

            public RECT(int left, int top, int right, int bottom)
            {
                this.Left = left;
                this.Top = top;
                this.Right = right;
                this.Bottom = bottom;
            }

            public Rectangle Rect
            { 
                get 
                { 
                    return new Rectangle(
                        this.Left, this.Top, this.Right - this.Left, this.Bottom - this.Top); 
                } 
            }

            public static RECT FromXYWH(int x, int y, int width, int height)
            {
                return new RECT(x,
                                y,
                                x + width,
                                y + height);
            }

            public static RECT FromRectangle(Rectangle rect)
            {
                return new RECT(rect.Left,
                                rect.Top,
                                rect.Right,
                                rect.Bottom);
            }
        }

        [DllImport("user32.dll")]
        public static extern IntPtr FindWindowEx(
            IntPtr hwndParent,
            IntPtr hwndChildAfter,
            string lpszClass,
            string lpszWindow);

        [DllImport("user32.dll")]
        public static extern int GetWindowRect(IntPtr hwnd, ref RECT lpRect);

        [DllImport("user32.dll")]
        public static extern bool DrawAnimatedRects(
            IntPtr hwnd,
            int ani,
            ref RECT from,
            ref RECT to);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetWindowPos(
            IntPtr hWnd, 
            IntPtr hWndAfter, 
            int x,
            int y, 
            int cx, 
            int cy, 
            uint flags);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
    }
}

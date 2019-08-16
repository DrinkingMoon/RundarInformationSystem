using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Reflection;
using System.IO;

namespace UniversalControlLibrary
{
    /// <summary>
    /// 鼠标控制
    /// </summary>
    public class CursorControl
    {

        [DllImport("user32.dll")]
        public static extern IntPtr LoadCursorFromFile(string fileName);
        [DllImport("user32.dll")]
        public static extern IntPtr SetCursor(IntPtr cursorHandle);
        [DllImport("user32.dll")]
        public static extern uint DestroyCursor(IntPtr cursorHandle);

        /// <summary>
        /// 支持ico,cur,ani格式
        /// </summary>
        /// <param name="form"></param>
        /// <param name="path"></param>
        public static void SetCursor(Form form, string path)
        {
            Cursor myCursor = new Cursor(Cursor.Current.Handle);
            IntPtr colorCursorHandle = LoadCursorFromFile(path);//鼠标图标路径
            myCursor.GetType().InvokeMember("handle", BindingFlags.Public
                | BindingFlags.NonPublic | BindingFlags.Instance
                | BindingFlags.SetField, null, myCursor, new object[] { colorCursorHandle });
            form.Cursor = myCursor;
        }

        public static void SetWaitCursor(Form form)
        {
            SetCursor(form, UniversalControlLibrary.Properties.Resources.wait);
        }

        /// <summary>
        /// 支持ico,cur,ani格式
        /// </summary>
        /// <param name="form"></param>
        /// <param name="bt"></param>
        public static void SetCursor(Form form, byte[] bt)
        {
            string filePath = Application.StartupPath + "\\Cursor.ico";

            if (!File.Exists(filePath))
            {
                File.WriteAllBytes(filePath, bt);
            }

            Cursor myCursor = new Cursor(Cursor.Current.Handle);

            IntPtr colorCursorHandle = LoadCursorFromFile(filePath);//图标路径
            myCursor.GetType().InvokeMember("handle", BindingFlags.Public
                | BindingFlags.NonPublic | BindingFlags.Instance
                | BindingFlags.SetField, null, myCursor, new object[] { colorCursorHandle });

            form.Cursor = myCursor;
        }

        /// <summary>
        /// 支持png,gif格式
        /// </summary>
        /// <param name="form"></param>
        /// <param name="path"></param>
        public static void SetCursor(Form form, Bitmap cursor, Point hotPoint)
        {
            int hotX = hotPoint.X;
            int hotY = hotPoint.Y;
            Bitmap myNewCursor = new Bitmap(cursor.Width * 2 - hotX, cursor.Height * 2 - hotY);
            Graphics g = Graphics.FromImage(myNewCursor);
            g.Clear(Color.FromArgb(0, 0, 0, 0));
            g.DrawImage(cursor, cursor.Width - hotX, cursor.Height - hotY, cursor.Width,
            cursor.Height);

            form.Cursor = new Cursor(myNewCursor.GetHicon());

            g.Dispose();
            myNewCursor.Dispose();
        }
    }
}

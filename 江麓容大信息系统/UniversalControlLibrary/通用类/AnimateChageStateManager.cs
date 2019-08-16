using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.ComponentModel;
using System.Windows.Forms;

namespace UniversalControlLibrary
{

    public class AnimateChangeStateManager
    {
        public static void Animate(
            IntPtr handle,
            Rectangle fromRect,
            Rectangle toRect)
        {
            NativeMethods.RECT from = NativeMethods.RECT.FromRectangle(fromRect);
            NativeMethods.RECT to = NativeMethods.RECT.FromRectangle(toRect);
            NativeMethods.DrawAnimatedRects(
                handle,
                NativeMethods.IDANI_CAPTION,
                ref from,
                ref to);
        }

        public static void Animate(
            Form animateForm,
            Control owner)
        {
            Animate(
                animateForm.Handle,
                animateForm.Bounds,
                owner.RectangleToScreen(owner.ClientRectangle));
        }

        public static void Animate(
            Form animateForm,
            bool minimized)
        {
            if (minimized)
            {
                Animate(
                    animateForm.Handle,
                    GetNotificationRect(),
                    animateForm.Bounds);
            }
            else
            {
                Animate(
                    animateForm.Handle,
                    animateForm.Bounds,
                    GetNotificationRect());
            }
        }

        private static Rectangle GetNotificationRect()
        {
            NativeMethods.RECT rect = new NativeMethods.RECT();
            IntPtr hwnd = NativeMethods.FindWindowEx(
                IntPtr.Zero,
                IntPtr.Zero,
                "Shell_TrayWnd",
                null);
            if (hwnd == IntPtr.Zero)
            {
                throw new Win32Exception();
            }

            hwnd = NativeMethods.FindWindowEx(
                hwnd,
                IntPtr.Zero,
                "TrayNotifyWnd",
                null);
            if (hwnd == IntPtr.Zero)
            {
                throw new Win32Exception();
            }

            hwnd = NativeMethods.FindWindowEx(
                hwnd,
                IntPtr.Zero,
                "SysPager",
                null);
            if (hwnd == IntPtr.Zero)
            {
                throw new Win32Exception();
            }

            hwnd = NativeMethods.FindWindowEx(
                hwnd,
                IntPtr.Zero,
                "ToolbarWindow32",
                null);
            if (hwnd == IntPtr.Zero)
            {
                throw new Win32Exception();
            }

            NativeMethods.GetWindowRect(hwnd, ref rect);

            return rect.Rect;
        }
    }
}
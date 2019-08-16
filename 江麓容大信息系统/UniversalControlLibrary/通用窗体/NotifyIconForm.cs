using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.InteropServices;

namespace UniversalControlLibrary
{
    public class NotifyIconForm : Form
    {
        private NotifyIcon _notifyIcon;
        private Rectangle _bounds;

        public NotifyIconForm()
            : base()
        {
            Init();
        }

        public NotifyIcon NotifyIcon
        {
            get { return _notifyIcon; }
        }

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case NativeMethods.WM_SYSCOMMAND:
                    WmSyscommand(ref m);
                    break;
            }
            base.WndProc(ref m);
        }

        private void Init()
        {
            ShowInTaskbar = false;

            _notifyIcon = new NotifyIcon();
            _notifyIcon.Text = "NotifyIconForm";
            _notifyIcon.Icon = Icon;
            _notifyIcon.DoubleClick += delegate(object sender, EventArgs e)
            {
                if (WindowState == FormWindowState.Minimized)
                {
                    NativeMethods.SetWindowPos(
                        Handle,
                        new IntPtr(0),
                        _bounds.X,
                        _bounds.Y,
                        _bounds.Width,
                        _bounds.Height,
                        NativeMethods.SWP_NOREDRAW | NativeMethods.SWP_NOZORDER);

                    AnimateChangeStateManager.Animate(this, true);
                    NativeMethods.ShowWindow(Handle, NativeMethods.SW_NORMAL);
                    Opacity = 1;
                }
            };
        }

        private void WmSyscommand(ref Message m)
        {
            int wparam = m.WParam.ToInt32();
            switch (wparam)
            {
                case (int)NativeMethods.SystemCommands.SC_MINIMIZE:
                    AnimateChangeStateManager.Animate(this, false);
                    _bounds = Bounds;
                    Hide();
                    Opacity = 0;
                    break;
            }
        }
    }
}

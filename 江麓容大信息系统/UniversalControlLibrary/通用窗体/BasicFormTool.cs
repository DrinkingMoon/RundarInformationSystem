using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using GlobalObject;

namespace UniversalControlLibrary
{
    public partial class BasicFormTool : Form
    {
        ToolStripForm tsf;

        object _BusinessList_Object;

        public object BusinessList_Object
        {
            get { return _BusinessList_Object; }
            set { _BusinessList_Object = value; }
        }

        List<object> _BusinessList = new List<object>();

        public List<object> BusinessList
        {
            get { return _BusinessList; }
            set { _BusinessList = value; }
        }

        DataGridViewRow _BusinessView_Row = null;

        public DataGridViewRow BusinessView_Row
        {
            get { return _BusinessView_Row; }
            set { _BusinessView_Row = value; }
        }

        DataGridView _BusinessView = new DataGridView();

        public DataGridView BusinessView
        {
            get { return _BusinessView; }
            set { _BusinessView = value; }
        }

        private const int WM_SYSCOMMAND = 0x112;
        private const int MF_STRING = 0x0;
        private const int MF_SEPARATOR = 0x800;

        /// <summary>
        /// 返回包含了指定点的窗口的句柄。忽略屏蔽、隐藏以及透明窗口
        /// </summary>
        /// <param name="Point">指定的鼠标坐标</param>
        /// <returns>鼠标坐标处的窗口句柄，如果没有，返回</returns>
        [DllImport("user32.dll")]
        internal static extern IntPtr WindowFromPoint(Point Point);

        /// <summary>
        /// 获取鼠标处的坐标
        /// </summary>
        /// <param name="lpPoint">随同指针在屏幕像素坐标中的位置载入的一个结构</param>
        /// <returns>非零表示成功，零表示失败</returns>
        [DllImport("user32.dll")]
        internal static extern bool GetCursorPos(out Point lpPoint);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool AppendMenu(IntPtr hMenu, int uFlags, int uIDNewItem, string lpNewItem);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool InsertMenuItem(IntPtr hMenu, int uPosition, int uFlags, int uIDNewItem, String ipNewItem);

        private int SYSMENU_ToolStrip_ID = 0x1;

        public BasicFormTool()
        {
            InitializeComponent();
        }

        public virtual void LoadFormInfo()
        {
            if (!DesignMode)
            {
                if (tsf == null || tsf.IsDisposed)
                {
                    if ((_BusinessView != null && _BusinessView.Rows.Count > 0)
                        || (_BusinessList != null && _BusinessList.Count > 0))
                    {
                        tsf = new ToolStripForm(this);
                        tsf.Show();
                    }
                }
            }
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if ((m.Msg == WM_SYSCOMMAND) && ((int)m.WParam == SYSMENU_ToolStrip_ID))
            {
                if (tsf.IsDisposed)
                {
                    if ((_BusinessView != null && _BusinessView.Rows.Count > 0)
                        || (_BusinessList != null && _BusinessList.Count > 0))
                    {
                        tsf = new ToolStripForm(this);
                        tsf.Show();
                    }
                }
            }
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);

            IntPtr hSysMenu = GetSystemMenu(this.Handle, false);
            AppendMenu(hSysMenu, MF_SEPARATOR, 0, String.Empty);
            AppendMenu(hSysMenu, MF_STRING, SYSMENU_ToolStrip_ID, "显示工具条");
        }

        private void BasicFormTool_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.tsf != null && !tsf.IsDisposed)
            {
                tsf.Close();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Point p;
            GetCursorPos(out p);//获取鼠标坐标

            if (tsf != null && !tsf.IsDisposed)
            {
                if (tsf.Handle == WindowFromPoint(p))
                {
                    tsf.Activate();
                    return;
                }

                foreach (Control cl in tsf.Controls)
                {
                    if (cl.Handle == WindowFromPoint(p))
                    {
                        tsf.Activate();
                        cl.Focus();
                        return;
                    }
                }
            }
        }
    }
}

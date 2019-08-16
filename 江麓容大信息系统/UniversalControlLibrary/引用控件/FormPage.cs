using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace UniversalControlLibrary
{
    public partial class WinFormPage : UserControl
    {

        private GlobalObject.DelegateCollection.NonArgumentHandle _refresh;

        //页显示数
        private int _PageSize = 25;

        //页总数
        private int _PageCount = 0;

        //页码
        private int _PageIndex = 1;

        //数据条数
        private int _Count = 0;

        //跳转页码
        private int _GoIndex = 0;

        /// <summary>
        /// 获取或设置页显示数量
        /// </summary>
        public int PageSize
        {
            get { return _PageSize; }

            set {
                _PageSize = value;

                if (Count > 0)
                {
                    PageCount = Count / PageSize;

                    if (Count % PageSize != 0)
                        PageCount += 1;
                }
            }
        }

        /// <summary>
        /// 获取或设置页数量
        /// </summary>
        public int PageCount
        {
            get { return _PageCount; }
            set
            {
                _PageCount = value;
                labpcount.Text = _PageCount.ToString();
            }
        }

        /// <summary>
        /// 获取或设置页码
        /// </summary>
        public int PageIndex
        {
            get { return Convert.ToInt32(labindex.Text); }
            set { _PageIndex = value; }
        }

        /// <summary>
        /// 获取或设置数据总数量
        /// </summary>
        public int Count
        {
            get { return _Count; }
            set {
                _Count = value;

                if (PageSize > 0 && Count > 0)
                {
                    PageCount = Count / PageSize;

                    if (Count % PageSize != 0)
                        PageCount += 1;
                }
            }
        }
        /// <summary>
        /// 获取或设置跳转页面
        /// </summary>
        public int GoIndex
        {
            get { return _GoIndex; }
            set { _GoIndex = value; }
        }
        
        /// <summary>
        /// 刷新数据
        /// </summary>
        public GlobalObject.DelegateCollection.NonArgumentHandle RefreshData
        {
            set { _refresh = value; }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public WinFormPage()
        {
            InitializeComponent();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //第一页
            _PageIndex = 1;
            labindex.Text = _PageIndex.ToString();
            _refresh();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //上一页
            int tmp = Convert.ToInt32(labindex.Text);
            tmp = tmp - 1;

            if (tmp <= 0)
            {
                tmp = 1;
            }

            _PageIndex = tmp;
            labindex.Text = _PageIndex.ToString();
            _refresh();
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //下一页
            int tmp = Convert.ToInt32(labpcount.Text);
            int tmp2 = Convert.ToInt32(labindex.Text);
            tmp2 = tmp2 + 1;

            if (tmp2 > tmp)
            {
                _PageIndex = tmp;
                labindex.Text = tmp.ToString();
            }
            else
            {
                _PageIndex = tmp2;
                labindex.Text = tmp2.ToString();
            }
            _refresh();
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //最后页
            _PageIndex = Convert.ToInt32(labpcount.Text);
            labindex.Text = labpcount.Text;
            _refresh();
        }

        private void linkLabel5_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //跳转
            int tmp = 0;

            try
            {
                tmp = Convert.ToInt32(tbxGo.Text);
            }
            catch
            {
                tmp = 1;
                tbxGo.Text = "1";
            }

            if (tmp <= 0)
            {
                tmp = 1;
            }

            int tmp2 = Convert.ToInt32(labpcount.Text);

            if (tmp > tmp2)
            {
                _PageIndex = tmp2;
            }
            else
            {
                _PageIndex = tmp;
            }

            labindex.Text = _PageIndex.ToString();
            tbxGo.Text = _PageIndex.ToString();
            _refresh();
        }
    }
}
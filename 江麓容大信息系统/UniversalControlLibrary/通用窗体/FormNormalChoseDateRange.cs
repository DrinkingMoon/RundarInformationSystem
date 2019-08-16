/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  FormNormalChoseDateRange.cs
 * 作者    :  夏石友    版本: v1.00    日期: 2010/10/25
 * 开发平台:  vs2005(c#)
 * 用于    :  生产线管理信息系统
 *----------------------------------------------------------------------------
 * 描述 : 关于界面
 * 其它 :
 *----------------------------------------------------------------------------
 * 历史记录:
 *     1. 日期时间: 2009/07/03 08:02:08 作者: 夏石友 当前版本: V1.00
 *        修改说明: 创建
 ******************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace UniversalControlLibrary
{
    /// <summary>
    /// 通用的时间范围选择界面
    /// </summary>
    public partial class FormNormalChoseDateRange : Form
    {
        /// <summary>
        /// 开始时间
        /// </summary>
        DateTime m_beginTime;

        /// <summary>
        /// 结束时间
        /// </summary>
        DateTime m_endTime;

        /// <summary>
        /// 获取开始时间
        /// </summary>
        public DateTime BeginTime
        {
            get { return m_beginTime; }
        }

        /// <summary>
        /// 获取终止时间
        /// </summary>
        public DateTime EndTime
        {
            get { return m_endTime; }
        }

        public FormNormalChoseDateRange()
        {
            InitializeComponent();
        }

        private void FormNormalChoseDateRange_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 使日期控件中的数据生效
            if (dateTimePicker1.Focused)
                dateTimePicker2.Focus();
            else if (dateTimePicker2.Focused)
                dateTimePicker1.Focus();

                if (dateTimePicker1.Value > dateTimePicker2.Value)
            {
                e.Cancel = true;
                MessageBox.Show("起止日期的顺序颠倒，请重新选择！");
                return;
            }

            m_beginTime = Convert.ToDateTime(dateTimePicker1.Value.ToShortDateString());
            m_endTime = Convert.ToDateTime(string.Format("{0}", dateTimePicker2.Value.ToShortDateString()));
        }

    }
}
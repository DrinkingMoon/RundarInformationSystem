/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  FormChoseDateRange.cs
 * 作者    :  夏石友    版本: v1.00    日期: 2009/06/15
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

namespace Expression
{
    /// <summary>
    /// 电子档案时间查询范围界面类
    /// </summary>
    public partial class FormChoseDateRange : Form
    {
        /// <summary>
        /// 电子档案窗体
        /// </summary>
        UserControlElectronFile m_electronFile;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="form">电子档案组件</param>
        public FormChoseDateRange(UserControlElectronFile form)
        {
            InitializeComponent();

            if (form != null)
            {
                m_electronFile = form;
            }
        }

        /// <summary>
        /// 确定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOk_Click(object sender, EventArgs e)
        {
            if (dateTimePicker1.Value.Year.ToString() == dateTimePicker2.Value.Year.ToString() 
                && dateTimePicker1.Value.Month.ToString() == dateTimePicker2.Value.Month.ToString() 
                && dateTimePicker1.Value.Date.ToString() == dateTimePicker2.Value.Date.ToString())
            {

            }

            int y1 = Convert.ToInt32(dateTimePicker1.Value.Year.ToString());
            int y2 = Convert.ToInt32(dateTimePicker2.Value.Year.ToString());
            int m1 = Convert.ToInt32(dateTimePicker1.Value.Month.ToString());
            int m2 = Convert.ToInt32(dateTimePicker2.Value.Month.ToString());
            int d1 = Convert.ToInt32(dateTimePicker1.Value.Day.ToString());
            int d2 = Convert.ToInt32(dateTimePicker2.Value.Day.ToString());

            if (y1 > y2)
            {
                MessageBox.Show("起止日期的顺序颠倒，请重新选择！");
            }
            else if (m1 > m2)
            {
                MessageBox.Show("起止日期的顺序颠倒，请重新选择！");
            }
            else if (d1 > d2)
            {
                MessageBox.Show("起止日期的顺序颠倒，请重新选择！");
            }
            else
            {
                string year1 = dateTimePicker1.Value.Year.ToString();
                string month1;
                string day1;
                string year2 = dateTimePicker1.Value.Year.ToString();
                string month2;
                string day2;

                if (dateTimePicker1.Value.Month.ToString().Length == 1)
                {
                    month1 = "0" + dateTimePicker1.Value.Month.ToString();
                }
                else
                {
                    month1 = dateTimePicker1.Value.Month.ToString();
                }

                if (dateTimePicker1.Value.Day.ToString().Length == 1)
                {
                    day1 = "0" + dateTimePicker1.Value.Day.ToString();
                }
                else
                {
                    day1 = dateTimePicker1.Value.Day.ToString();
                }

                string date1 = year1 + "-" + month1 + "-" + day1;

                if (dateTimePicker2.Value.Month.ToString().Length == 1)
                {
                    month2 = "0" + dateTimePicker2.Value.Month.ToString();
                }
                else
                {
                    month2 = dateTimePicker2.Value.Month.ToString();
                }

                if (dateTimePicker2.Value.Day.ToString().Length == 1)
                {
                    day2 = "0" + dateTimePicker2.Value.Day.ToString();
                }
                else
                {
                    day2 = dateTimePicker2.Value.Day.ToString();
                }

                string date2 = year2 + "-" + month2 + "-" + day2;

                m_electronFile.SetDate(date1, date2);

                this.Close();
            }
        }

    }
}
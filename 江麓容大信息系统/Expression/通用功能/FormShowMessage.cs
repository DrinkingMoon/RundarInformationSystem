using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PlatformManagement;
using UniversalControlLibrary;
using System.Runtime.InteropServices;

namespace Expression
{
    public partial class FormShowMessage : Form
    {
        [DllImport("user32.dll")]
        public static extern bool IsIconic(IntPtr hWnd);
        Form tempForm = new Form();

        public FormShowMessage(int messageCount, Form frm)
        {
            InitializeComponent();
            tempForm = frm;
            Rectangle rect = Screen.GetWorkingArea(this);
            Point pt1 = new Point(rect.Location.X + rect.Width - this.Width, rect.Location.Y + rect.Height - this.Height);
            this.Location = pt1;

            lbMessageShow.Text = "您有" + messageCount + "条新消息需要处理";
        }

        private void lbMessageShow_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //if (m_lnqBillMessage.附加信息1 != null && m_lnqBillMessage.附加信息1.ToString().Trim().Length != 0)
            //{
            //    List<string> lstData = new List<string>();

            //    List<string> lstColumns = GlobalObject.GeneralFunction.GetItemPropertyName(m_lnqBillMessage);

            //    foreach (string columnName in lstColumns)
            //    {
            //        if (columnName.Contains("附加信息"))
            //        {
            //            lstData.Add(GlobalObject.GeneralFunction.GetItemValue<Flow_BillFlowMessage>(m_lnqBillMessage, columnName).ToString());
            //        }
            //    }

            //    ((FormMain)StapleInfo.MainForm).ShowBillForm(m_lnqBillMessage.附加信息1,
            //        m_lnqBillMessage.附加信息2, lstData);
            //}
            //else
            //{
            //    ((FormMain)StapleInfo.MainForm).ShowBillForm(m_lnqBillMessage.单据类型,
            //        m_lnqBillMessage.单据流水号);
            //}

            ((FormMain)StapleInfo.MainForm).ShowForm();
            this.Close();
        }
    }
}

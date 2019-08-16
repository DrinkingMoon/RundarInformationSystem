using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ServerModule;
using PlatformManagement;
using GlobalObject;

namespace Expression
{
    /// <summary>
    /// 产品隔离处置单质管主管确认界面
    /// </summary>
    public partial class 不合格品隔离处置单质管主管确认窗体 : Form
    {
        /// <summary>
        /// 确认标志
        /// </summary>
        private bool m_blFlag = true;

        public bool BlFlag
        {
            get { return m_blFlag; }
            set { m_blFlag = value; }
        }

        /// <summary>
        /// 备注信息
        /// </summary>
        private string m_strRemark = "";

        public string StrRemark
        {
            get { return m_strRemark; }
            set { m_strRemark = value; }
        }

        public 不合格品隔离处置单质管主管确认窗体(string strtext)
        {
            
            InitializeComponent();
            label1.Text = "请对" + strtext + "\n\n进行判定是否解除隔离？";
        }

        private void btnYes_Click(object sender, EventArgs e)
        {

            m_blFlag = true;
            m_strRemark = textBox1.Text.Trim();

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnNo_Click(object sender, EventArgs e)
        {

            m_blFlag = false;
            m_strRemark = textBox1.Text.Trim();

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}

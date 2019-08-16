using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Expression;
using Service_Quality_File;
using GlobalObject;

namespace Form_Quality_File
{
    public partial class 操作方式 : Form
    {
        /// <summary>
        /// 操作方式 0为退出，1为在线阅读 2为下载 3为在线编辑
        /// </summary>
        private CE_FileOperatorType m_OperatorFlag = CE_FileOperatorType.无操作;

        public CE_FileOperatorType OperatorFlag
        {
            get { return m_OperatorFlag; }
            set { m_OperatorFlag = value; }
        }

        public 操作方式(ProcessType mode)
        {
            InitializeComponent();

            if (mode == ProcessType.发布)
            {
                button3.Visible = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            m_OperatorFlag = CE_FileOperatorType.在线阅读;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            m_OperatorFlag = CE_FileOperatorType.下载;
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            m_OperatorFlag = CE_FileOperatorType.在线编辑;
            this.Close();
        }
    }
}

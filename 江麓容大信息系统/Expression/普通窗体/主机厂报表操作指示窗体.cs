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
using UniversalControlLibrary;


namespace Expression
{
    /// <summary>
    /// 主机厂报表操作指示界面
    /// </summary>
    public partial class 主机厂报表操作指示窗体 : Form
    {
        /// <summary>
        /// 客户信息服务组件
        /// </summary>
        IClientServer m_serverClient = ServerModuleFactory.GetServerModule<IClientServer>();

        /// <summary>
        /// 主机厂
        /// </summary>
        private string m_strCommunicate = "";

        public string StrCommunicate
        {
            get { return m_strCommunicate; }
            set { m_strCommunicate = value; }
        }

        /// <summary>
        /// 年月
        /// </summary>
        private string m_strNy = "";

        public string StrNy
        {
            get { return m_strNy; }
            set { m_strNy = value; }
        }

        /// <summary>
        /// 保存标志
        /// </summary>
        private bool m_blFlag = false;

        public bool BlFlag
        {
            get { return m_blFlag; }
            set { m_blFlag = value; }
        }

        public 主机厂报表操作指示窗体()
        {
            InitializeComponent();

            for (int i = 2010; i < 2050; i++)
            {
                cmbYear.Items.Add(i);
            }

            for (int f = 1; f <= 12; f++)
            {
                cmbMonth.Items.Add(f.ToString("D2"));
            }

            cmbYear.Text = ServerTime.Time.Year.ToString();
            cmbMonth.Text = ServerTime.Time.Month.ToString("D2");
        }

        private void btnSure_Click(object sender, EventArgs e)
        {
            if (txtClient.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("请选择主机厂");
                return;
            }

            m_blFlag = true;

            m_strCommunicate = txtClient.Tag.ToString();

            m_strNy = cmbYear.Text.ToString() + cmbMonth.Text.ToString();

            this.Close();

        }

        private void btnQX_Click(object sender, EventArgs e)
        {
            m_blFlag = false;

            this.Close();
        }

        private void txtClient_OnCompleteSearch()
        {
            txtClient.Tag = txtClient.DataResult["客户编码"].ToString();
            txtClient.Text = txtClient.DataResult["客户名称"].ToString();
        }
    }
}

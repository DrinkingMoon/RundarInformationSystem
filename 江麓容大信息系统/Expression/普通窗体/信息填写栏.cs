using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ServerModule;
using PlatformManagement;
using GlobalObject;


namespace Expression
{
    /// <summary>
    /// 不合格品信息填写界面
    /// </summary>
    public partial class 信息填写栏 : Form
    {
        /// <summary>
        /// 单据号
        /// </summary>
        string m_strDJH;

        /// <summary>
        /// 信息Row
        /// </summary>
        private DataRow m_drMasssage;

        public DataRow DrMasssage
        {
            get { return m_drMasssage; }
            set { m_drMasssage = value; }
        }

        /// <summary>
        /// 标志
        /// </summary>
        private bool m_blFlag = false;

        public bool BlFlag
        {
            get { return m_blFlag; }
            set { m_blFlag = value; }
        }

        /// <summary>
        /// 服务类
        /// </summary>
        /// <param name="nodeInfo"></param>
        ICheckReturnRepair m_serverCheckReturnRepair = ServerModuleFactory.GetServerModule<ICheckReturnRepair>();

        public 信息填写栏(string strBill_ID)
        {
            InitializeComponent();
            m_strDJH = strBill_ID;
            m_drMasssage = m_serverCheckReturnRepair.GetData(m_strDJH);

            if (m_drMasssage != null)
            {
                txtReason.Text = m_drMasssage["挑返原因"].ToString();
                txtMeansAndAsk.Text = m_drMasssage["挑返方法及要求"].ToString();
                txtManHour.Text = m_drMasssage["挑返损耗及工时"].ToString();
            }

            if (!BasicInfo.ListRoles.Contains(CE_RoleEnum.SQE组员.ToString()))
            {
                btnSave.Enabled = false;
            }
            
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            m_drMasssage["挑返原因"] = txtReason.Text;
            m_drMasssage["挑返方法及要求"] = txtMeansAndAsk.Text;
            m_drMasssage["挑返损耗及工时"] = txtManHour.Text;
            m_blFlag = true;

            this.Close();
        }

        private void btnOut_Click(object sender, EventArgs e)
        {
            m_blFlag = false;
            this.Close();
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ServerModule;
using UniversalControlLibrary;

namespace CommonBusinessModule
{
    /// <summary>
    /// 针对于各种入库业务的不合格品的供应商质量信息反馈单的提交界面
    /// </summary>
    public partial class 不合格品信息 : Form
    {
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
        /// 供货状态
        /// </summary>
        string m_strForGoods;

        /// <summary>
        /// 信息来源
        /// </summary>
        string m_strMessageFrom;

        /// <summary>
        /// 错误信息
        /// </summary>
        string m_strErr;

        /// <summary>
        /// 单据号
        /// </summary>
        string m_strDJH;

        /// <summary>
        /// 质量信息反馈单LINQ数据集
        /// </summary>
        S_MessMessageFeedback m_lnqMessMessage = new S_MessMessageFeedback();

        /// <summary>
        /// 服务类
        /// </summary>
        IMessMessageFeedback m_serverMessMessage = ServerModuleFactory.GetServerModule<IMessMessageFeedback>();

        public 不合格品信息(string strBillID)
        {
            InitializeComponent();

            m_strDJH = strBillID;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            CheckForGoods();
            CheckMessageFrom();

            if (m_strForGoods == "" || m_strMessageFrom == "")
            {
                MessageDialog.ShowPromptMessage("请将状态填写完整");
                return;
            }

            m_lnqMessMessage.DJH = txtDJH.Text;
            m_lnqMessMessage.ForGoodsStatus = m_strForGoods;
            m_lnqMessMessage.MessageFromStatus = m_strMessageFrom;
            m_lnqMessMessage.DisqualificationDepict = txtBHGPMS.Text;
            m_lnqMessMessage.InDepotBillID = m_strDJH;

            if (!m_serverMessMessage.AddData(m_lnqMessMessage, out m_strErr))
            {
                MessageDialog.ShowErrorMessage(m_strErr);
                return;
            }

            m_blFlag = true;
            this.Close();
        }

        private void 不合格品信息_Load(object sender, EventArgs e)
        {
            txtDJH.Text = m_serverMessMessage.GetBillID();
        }

        /// <summary>
        /// 获得供货状态
        /// </summary>
        private void CheckForGoods()
        {
            if (rbGHZTPL.Checked)
            {
                m_strForGoods = rbGHZTPL.Text;
                return;
            }

            if (rbGHZTXPL.Checked)
            {
                m_strForGoods = rbGHZTXPL.Text;
                return;
            }

            if (rbGHZTYJ.Checked)
            {
                m_strForGoods = rbGHZTYJ.Text;
                return;
            }
        }

        /// <summary>
        /// 获得信息来源
        /// </summary>
        private void CheckMessageFrom()
        {
            if (rbXXLYGK.Checked)
            {
                m_strMessageFrom = rbGHZTPL.Text;
                return;
            }

            if (rbXXLYJHJY.Checked)
            {
                m_strMessageFrom = rbXXLYJHJY.Text;
                return;
            }

            if (rbXXLYSCGC.Checked)
            {
                m_strMessageFrom = rbXXLYSCGC.Text;
                return;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

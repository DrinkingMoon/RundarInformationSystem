using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PlatformManagement;
using ServerModule;
using Expression;
using UniversalControlLibrary;

namespace Form_Peripheral_HR
{
    public partial class 回退单据 : Form
    {
        /// <summary>
        /// 出差申请单据状态字符串数组
        /// </summary>
        string[] m_strOnBusiness = { "等待随行人员部门确认", "等待主管审核", "等待分管领导审批", "等待总经理批准", "等待销差人确认" };

        /// <summary>
        /// 单据号
        /// </summary>
        private string m_strBillID = "";

        public string StrBillID
        {
            get { return m_strBillID; }
            set { m_strBillID = value; }
        }

        /// <summary>
        /// 单据状态
        /// </summary>
        private string m_strBillStatus = "";

        public string StrBillStatus
        {
            get { return m_strBillStatus; }
            set { m_strBillStatus = value; }
        }

        /// <summary>
        /// 是否确认回退标志
        /// </summary>
        private bool m_blFlag = false;

        public bool BlFlag
        {
            get { return m_blFlag; }
            set { m_blFlag = value; }
        }

        /// <summary>
        /// 单据类型
        /// </summary>
        private string m_strType = "";

        /// <summary>
        /// 获取单据流消息操作接口
        /// </summary>
        IBillFlowMessage m_billFlowMsg = PlatformFactory.GetObject<IBillFlowMessage>();

        /// <summary>
        /// 窗体消息发布器
        /// </summary>
        WndMsgSender m_wndMsgSender = new WndMsgSender();

        /// <summary>
        /// 回退原因
        /// </summary>
        public string Reason
        {
            get { return txtReason.Text; }
        }

        public 回退单据(string strBillType, string strBill_ID, string strBillStatus)
        {
            InitializeComponent();

            lbDJZT.Text = strBillStatus;
            m_strType = strBillType;
            txtDJH.Text = strBill_ID;
            m_strBillID = strBill_ID;
            GetBillType();
        }

        /// <summary>
        /// 插入ComBox
        /// </summary>
        private void GetBillType()
        {
            switch (m_strType)
            {
                case "出差申请单":
                    InsertComBox(m_strOnBusiness);
                    break;
                default:
                    break;
            }

        }

        private void InsertComBox(string[] str)
        {
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i].ToString() == lbDJZT.Text)
                {
                    return;
                }

                cmbDJZT.Items.Add(str[i]);
            }

            cmbDJZT.SelectedIndex = -1;
        }

        #region 按钮事件

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (cmbDJZT.Text == "")
            {
                cmbDJZT.Focus();
                MessageDialog.ShowPromptMessage("请选择回退状态");
                return;
            }

            if (txtReason.Text.Trim() == "")
            {
                txtReason.Focus();
                MessageDialog.ShowPromptMessage("请填写回退原因");
                return;
            }

            m_strBillStatus = cmbDJZT.Text;
            m_blFlag = true;
            this.Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            m_strBillStatus = "";
            this.Close();
        }

        #endregion
    }
}

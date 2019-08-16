using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PlatformManagement;
using ServerModule;
using GlobalObject;
using UniversalControlLibrary;

namespace Expression
{
    /// <summary>
    /// 退货处理建议界面
    /// </summary>
    public partial class 退货处理建议 : Form
    {
        /// <summary>
        /// 报检入库单服务组件
        /// </summary>
        ICheckOutInDepotServer m_billServer = ServerModuleFactory.GetServerModule<ICheckOutInDepotServer>();

        /// <summary>
        /// 单据号
        /// </summary>
        string m_strDJH;

        /// <summary>
        /// 退货建议
        /// </summary>
        string m_strRejectMode;

        /// <summary>
        /// 错误信息
        /// </summary>
        string m_err;

        public 退货处理建议(string strInDJH)
        {
            InitializeComponent();

            m_strDJH = strInDJH;

            m_strRejectMode = m_billServer.GetRejectMode(m_strDJH);

            if (m_strRejectMode == radioButton1.Text)
            {
                radioButton1.Checked = true;
            }

            if (m_strRejectMode == radioButton2.Text)
            {
                radioButton2.Checked = true;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                m_strRejectMode = radioButton1.Text;
            }

            if (radioButton2.Checked)
            {
                m_strRejectMode = radioButton2.Text;
            }

            if (m_billServer.UpdateRejectMode(m_strDJH, m_strRejectMode, out m_err))
            {
                MessageBox.Show("提交成功！", "提示");
            }
            else
            {
                MessageDialog.ShowErrorMessage(m_err);
                return;
            }

            this.Close();
        }

        private void btnOut_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

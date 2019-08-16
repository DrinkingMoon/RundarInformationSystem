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
using UniversalControlLibrary;


namespace Expression
{
    /// <summary>
    /// 工装编号录入界面
    /// </summary>
    public partial class 工装编号录入窗体 : Form
    {
        CE_BusinessBillType m_businessType;

        /// <summary>
        /// 错误信息
        /// </summary>
        string m_strErr;

        /// <summary>
        /// 工装台帐服务组件
        /// </summary>
        IFrockStandingBook m_serverFrockStandingBook = PMS_ServerFactory.GetServerModule<IFrockStandingBook>();

        /// <summary>
        /// 单据号
        /// </summary>
        string m_strBillNo = "";

        /// <summary>
        /// 物品ID
        /// </summary>
        int m_intGoodsID;

        public 工装编号录入窗体(string billno, int goodsID, CE_BusinessBillType businessType, bool isShow)
        {
            InitializeComponent();
            m_strBillNo = billno;
            m_intGoodsID = goodsID;

            m_businessType = businessType;

            if (isShow)
            {
                btnAdd.Visible = true;
                btnUpdate.Visible = true;
                btnDelete.Visible = true;
            }

            dataGridView1.DataSource = m_serverFrockStandingBook.GetFrockNumberFromBillNo(billno, goodsID);
        }

        private void txtFrockNumber_Enter(object sender, EventArgs e)
        {
            string strSql = " and 物品ID = " + m_intGoodsID;

            if (m_businessType == CE_BusinessBillType.入库)
            {
                txtFrockNumber.ShowResultForm = false;
            }
            else
            {
                txtFrockNumber.ShowResultForm = true;

                if (!Convert.ToBoolean((int)m_businessType))
                {
                    strSql += " and 是否在库 = 1 ";
                }
                else
                {
                    strSql += " and 是否在库 = 0 ";
                }
            }

            txtFrockNumber.StrEndSql = strSql;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!m_serverFrockStandingBook.IsIntactSatelliteInformation(txtFrockNumber.Text,m_intGoodsID))
            {
                MessageDialog.ShowPromptMessage("工装信息未填写完成或者数据不唯一");
                return;
            }

            DataTable dt = (DataTable)dataGridView1.DataSource;

            DataRow dr = dt.NewRow();

            dr["FrockNumber"] = txtFrockNumber.Text;

            dt.Rows.Add(dr);

            dataGridView1.DataSource = dt;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)dataGridView1.DataSource;

            dt.Rows.RemoveAt(dataGridView1.CurrentRow.Index);

            dataGridView1.DataSource = dt;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count > 0 )
            {
                if (!m_serverFrockStandingBook.UpdateFrockOperation(m_strBillNo,m_intGoodsID,
                    (DataTable)dataGridView1.DataSource, m_businessType, out m_strErr))
                {
                    MessageDialog.ShowPromptMessage(m_strErr);
                    return;
                }
                else
                {
                    MessageDialog.ShowPromptMessage("提交成功");
                    this.Close();
                }
            }
        }
    }
}

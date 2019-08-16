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
    /// 量检具编号录入界面
    /// </summary>
    public partial class 量检具编号录入窗体 : Form
    {
        /// <summary>
        /// 工装台帐服务组件
        /// </summary>
        IGaugeManage m_serverGaugeManage = ServerModuleFactory.GetServerModule<IGaugeManage>();

        /// <summary>
        /// 单据号
        /// </summary>
        string m_strBillID = "";

        /// <summary>
        /// 物品ID
        /// </summary>
        int m_intGoodsID;

        decimal _GoodsCount = 0;

        public 量检具编号录入窗体(string billID, int goodsID, decimal goodsCount, CE_BusinessBillType businessType, bool isControlVisible)
        {
            InitializeComponent();

            m_strBillID = billID;
            m_intGoodsID = goodsID;
            _GoodsCount = goodsCount;

            string strSql = " and 物品ID = " + goodsID;

            if (businessType == CE_BusinessBillType.入库)
            {
                txtGaugeCoding.ShowResultForm = false;
            }
            else
            {
                txtGaugeCoding.ShowResultForm = true;

                if (!Convert.ToBoolean((int)businessType))
                {
                    strSql += " and 在库 = 1 ";
                }
                else
                {
                    strSql += " and 在库 = 0 ";
                }
            }

            txtGaugeCoding.StrEndSql = strSql;

            if (isControlVisible)
            {
                btnAdd.Visible = true;
                btnUpdate.Visible = true;
                btnDelete.Visible = true;
            }

            dataGridView1.DataSource = m_serverGaugeManage.GetGaugeCodingFromBillNo(billID, goodsID);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)dataGridView1.DataSource;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[0][0].ToString() == txtGaugeCoding.Text)
                {
                    MessageDialog.ShowPromptMessage("不能重复录入同一量检具编号，请重新核实");
                    return;
                }
            }

            DataRow dr = dt.NewRow();

            dr["GaugeCoding"] = txtGaugeCoding.Text;

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
            try
            {
                if (dataGridView1.Rows.Count != _GoodsCount)
                {
                    throw new Exception("请录入相应数量的【编码】");
                }

                m_serverGaugeManage.UpdateGaugeOperation(m_strBillID, m_intGoodsID, (DataTable)dataGridView1.DataSource);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
            }
        }

        private void txtGaugeCoding_OnCompleteSearch()
        {
            txtGaugeCoding.Text = txtGaugeCoding.DataResult["量检具编号"].ToString();
        }
    }
}

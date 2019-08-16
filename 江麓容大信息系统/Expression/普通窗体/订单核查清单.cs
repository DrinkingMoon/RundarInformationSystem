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
using GlobalObject;
using UniversalControlLibrary;


namespace Expression
{
    /// <summary>
    /// 核查订单清单界面
    /// </summary>
    public partial class 订单核查清单 : Form
    {
        /// <summary>
        /// 服务
        /// </summary>
        IOrderFormAffrim m_orderFormAffrim = ServerModuleFactory.GetServerModule<IOrderFormAffrim>();

        /// <summary>
        /// 数据集
        /// </summary>
        B_WebForOrderFormList m_lnqList = new B_WebForOrderFormList();

        /// <summary>
        /// 数据集
        /// </summary>
        B_WebForAffirmTime m_lnqDate = new B_WebForAffirmTime();

        /// <summary>
        /// 单据号
        /// </summary>
        string m_strNy = "";

        /// <summary>
        /// 数据定位控件
        /// </summary>
        UserControlDataLocalizer m_dataLocalizer;

        /// <summary>
        /// 错误信息
        /// </summary>
        string m_err;

        /// <summary>
        /// 标志
        /// </summary>
        bool m_blBZ = true;

        public 订单核查清单(string yearAndMonth, AuthorityFlag m_authFlag,bool blFlag)
        {
            InitializeComponent();
            FaceAuthoritySetting.SetEnable(this.Controls, m_authFlag);
            FaceAuthoritySetting.SetVisibly(this.toolStrip1, m_authFlag);

            m_strNy = yearAndMonth;
            toolStrip1.Visible = blFlag;
            btnDateAdd.Visible = blFlag;
            btnDateDelete.Visible = blFlag;

            RefreshDataGirdView();
        }

        /// <summary>
        /// 刷新
        /// </summary>
        void RefreshDataGirdView()
        {
            DataTable dt = new DataTable();

            if (BasicInfo.ListRoles.Contains(CE_RoleEnum.采购员.ToString())
                && !BasicInfo.ListRoles.Contains(CE_RoleEnum.采购主管.ToString())
                && !BasicInfo.ListRoles.Contains(CE_RoleEnum.采购账务管理员.ToString()))
            {
                dt = m_orderFormAffrim.GetListInfo(m_strNy, BasicInfo.LoginName);
            }
            else
            {
                dt = m_orderFormAffrim.GetListInfo(m_strNy);
            }

            dgvOrderFormList.DataSource = dt;
            dgvOrderFormList.Columns["序号"].Visible = false;
            dgvOrderFormList.Columns["单据号"].Visible = false;
            dgvOrderFormList.Columns["物品ID"].Visible = false;

            if (m_dataLocalizer == null)
            {
                m_dataLocalizer = new UserControlDataLocalizer(dgvOrderFormList, this.Name, 
                    UniversalFunction.SelectHideFields(this.Name, dgvOrderFormList.Name, BasicInfo.LoginID));
                m_dataLocalizer.OnlyLocalize = true;
                panelPara.Controls.Add(m_dataLocalizer);
                m_dataLocalizer.Dock = DockStyle.Bottom;
            }

        }

        /// <summary>
        /// 清除数据
        /// </summary>
        void ClearDate()
        {
            txtBargainNumber.Text = "";
            txtBargainNumber.Tag = -1;
            txtCode.Text = "";
            txtCode.Tag = -1;
            txtName.Text = "";
            txtOrderFormNumber.Text = "";
            txtProvider.Text = "";
            txtSpec.Text = "";
            txtStockQuota.Text = "";
            numAmount.Value = 0;
            numCount.Value = 0;
            txtStockQuotaCount.Text = "0";
            dateTimeArrivalDate.Value = ServerTime.Time;
        }

        /// <summary>
        /// 获得明细信息的数据集
        /// </summary>
        void GetListMessage()
        {
            m_lnqList.BargainNumber = txtBargainNumber.Text;
            m_lnqList.CheckPersonnel = BasicInfo.LoginName;
            m_lnqList.GoodsID = Convert.ToInt32(txtCode.Tag);
            m_lnqList.ID = Convert.ToInt32(txtBargainNumber.Tag);
            m_lnqList.Ny = m_strNy;
            m_lnqList.OrderFormCount = numAmount.Value;
            m_lnqList.OrderFormNumber = txtOrderFormNumber.Text;
            m_lnqList.Provider = txtProvider.Text;
            m_lnqList.StockCount = Convert.ToDecimal(txtStockQuotaCount.Text);
            m_lnqList.CheckPersonnel = BasicInfo.LoginName;
            m_lnqList.ChangeFlag = true;
        }

        /// <summary>
        /// 获得到货日期的数据集
        /// </summary>
        void GetDateMessage()
        {
            m_lnqDate.ListID = Convert.ToInt32(txtBargainNumber.Tag);
            m_lnqDate.ID = Convert.ToInt32( dateTimeArrivalDate.Tag);
            m_lnqDate.AffirmTime = dateTimeArrivalDate.Value;
            m_lnqDate.AffirmCount = numCount.Value;
        }

        /// <summary>
        /// 检查数据
        /// </summary>
        /// <returns>检测通过返回True，否则返回False</returns>
        bool CheckListMessage()
        {
            if (txtBargainNumber.Text == "")
            {
                MessageDialog.ShowPromptMessage("请选择合同号");
                return false;
            }

            if (txtCode.Tag.ToString() == "-1")
            {
                MessageDialog.ShowPromptMessage("请选择物品");
                return false;
            }

            if (numAmount.Value == 0)
            {
                MessageDialog.ShowPromptMessage("实际订货数不能为0");
                return false;
            }

            if (!m_orderFormAffrim.IsListInfoIn(
                Convert.ToInt32(txtBargainNumber.Tag),
                Convert.ToInt32(txtCode.Tag),
                txtProvider.Text,
                out m_err))
            {
                MessageDialog.ShowPromptMessage("不能重复录入同一物品同一供应商");
                return false;
            }

            return true;
        }

        /// <summary>
        /// 检查数据
        /// </summary>
        /// <returns>检测通过返回True，否则返回False</returns>
        bool CheckDateMessage()
        {
            if (numCount.Value == 0)
            {
                MessageDialog.ShowPromptMessage("到货数不能为0");
                return false;
            }

            return true;
        }

        /// <summary>
        /// 检测到货数量
        /// </summary>
        void CheckAffrimCount()
        {
            DataTable dt = (DataTable)dgvOrderFormList.DataSource;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (Convert.ToDecimal(dt.Rows[i]["实际订货数"]) != 
                    m_orderFormAffrim.SumCount(Convert.ToInt32(dt.Rows[i]["序号"])))
                {
                    MessageDialog.ShowPromptMessage("[" + dt.Rows[i]["图号型号"].ToString() 
                        + "] [" + dt.Rows[i]["物品名称"].ToString() + "] [" 
                        + dt.Rows[i]["规格"].ToString() + "] [" 
                        + dt.Rows[i]["供应商"].ToString() 
                        + "] 的物品的到货总数与订货数不符，请重新确认及修改");
                    return;
                }
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            ClearDate();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!CheckListMessage())
            {
                return;
            }
            else
            {
                GetListMessage();

                if (!m_orderFormAffrim.AddBill(m_lnqList,out m_err))
                {
                    MessageDialog.ShowPromptMessage(m_err);
                }
                else
                {
                    if (!m_orderFormAffrim.UpdateBillStatus(m_strNy, out m_err))
                    {
                        MessageDialog.ShowPromptMessage(m_err);
                    }
                    else
                    {
                        MessageDialog.ShowPromptMessage("添加成功");
                        RefreshDataGirdView();
                    }
                }
            }
            
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!CheckListMessage())
            {
                return;
            }
            else
            {
                GetListMessage();

                if (!m_orderFormAffrim.UpdateListInfo(m_lnqList, out m_err))
                {
                    MessageDialog.ShowPromptMessage(m_err);
                }
                else
                {
                    if (!m_orderFormAffrim.UpdateBillStatus(m_strNy, out m_err))
                    {
                        MessageDialog.ShowPromptMessage(m_err);
                    }
                    else
                    {
                        MessageDialog.ShowPromptMessage("修改成功");
                        RefreshDataGirdView();
                    }
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            GetListMessage();

            if (MessageDialog.ShowEnquiryMessage("您确定要删除【" + m_strNy + "】号业务单据吗？") == DialogResult.Yes)
            {
                if (!m_orderFormAffrim.DeleteBill(Convert.ToInt32(m_lnqList.ID), out m_err))
                {
                    MessageDialog.ShowPromptMessage(m_err);
                }
                else
                {
                    if (!m_orderFormAffrim.UpdateBillStatus(m_strNy, out m_err))
                    {
                        MessageDialog.ShowPromptMessage(m_err);
                    }
                    else
                    {
                        MessageDialog.ShowPromptMessage("删除成功");
                        RefreshDataGirdView();
                    }
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnDateAdd_Click(object sender, EventArgs e)
        {
            if (!CheckDateMessage())
            {
                return;
            }
            else
            {
                GetDateMessage();

                if (!m_orderFormAffrim.AddAffrimInfo(m_lnqDate, out m_err))
                {
                    MessageDialog.ShowPromptMessage(m_err);
                }
                else
                {
                    MessageDialog.ShowPromptMessage("添加成功！");
                    m_blBZ = true;
                    dgvArriveList.DataSource = m_orderFormAffrim.GetAffrimInfo(
                        Convert.ToInt32(dgvOrderFormList.CurrentRow.Cells["序号"].Value.ToString()));
                    dgvArriveList.Columns["明细ID"].Visible = false;
                    dgvArriveList.Columns["序号"].Visible = false;
                }
            }
        }

        private void btnDateDelete_Click(object sender, EventArgs e)
        {
            GetDateMessage();

            if (!m_orderFormAffrim.DeleteAffrimInfo(Convert.ToInt32( m_lnqDate.ID), out m_err))
            {
                MessageDialog.ShowPromptMessage(m_err);
            }
            else
            {
                MessageDialog.ShowPromptMessage("删除成功！");
                m_blBZ = true;

                dgvArriveList.DataSource = m_orderFormAffrim.GetAffrimInfo(
                    Convert.ToInt32(dgvOrderFormList.CurrentRow.Cells["序号"].Value.ToString()));
                dgvArriveList.Columns["明细ID"].Visible = false;
                dgvArriveList.Columns["序号"].Visible = false;
            }
        }

        private void btnFindCode_Click(object sender, EventArgs e)
        {
            if (txtBargainNumber.Text.Length == 0)
            {
                txtBargainNumber.Focus();
                MessageDialog.ShowPromptMessage("请先选择合同号后再进行此操作！");

                return;
            }

            FormQueryInfo form = QueryInfoDialog.GetBargainGoodsForOrderFormDialog(txtBargainNumber.Text, true);

            if (form != null && form.ShowDialog() == DialogResult.OK)
            {
                txtCode.Text = form.GetDataItem("图号型号").ToString();
                txtName.Text = form.GetDataItem("物品名称").ToString();
                txtSpec.Text = form.GetDataItem("规格").ToString();
                txtCode.Tag = Convert.ToInt32(form.GetDataItem("物品ID").ToString());
            }
        }

        private void txtBargainNumber_TextChanged(object sender, EventArgs e)
        {
            if (txtBargainNumber.Text.Trim().ToString() == "")
            {
                txtOrderFormNumber.Text = "";
            }
            else
            {
                txtOrderFormNumber.Text = txtBargainNumber.Text.Trim().ToString() + "Auto" 
                    + ServerTime.Time.Year.ToString() + ServerTime.Time.Month.ToString();
            }
        }

        private void btnFindOrderForm_Click(object sender, EventArgs e)
        {

            FormQueryInfo form = QueryInfoDialog.GetBargainInfoDialog();

            if (form != null && DialogResult.OK == form.ShowDialog())
            {
                txtBargainNumber.Text = form.GetDataItem("合同号").ToString();
                txtProvider.Text = form.GetDataItem("供货单位").ToString();
            }
        }

        private void dgvOrderFormList_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            if (m_blBZ)
            {
                CheckAffrimCount();
                m_blBZ = false;
            }
        }

        private void dgvOrderFormList_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvOrderFormList.Rows.Count == 0)
            {
                return;
            }
            else
            {
                lbDW.Text = dgvOrderFormList.CurrentRow.Cells["单位"].Value.ToString();
                label13.Text = dgvOrderFormList.CurrentRow.Cells["单位"].Value.ToString();
                txtBargainNumber.Text = dgvOrderFormList.CurrentRow.Cells["合同号"].Value.ToString();
                txtBargainNumber.Tag = Convert.ToInt32(dgvOrderFormList.CurrentRow.Cells["序号"].Value.ToString());
                txtCode.Text = dgvOrderFormList.CurrentRow.Cells["图号型号"].Value.ToString();
                txtCode.Tag = Convert.ToInt32(dgvOrderFormList.CurrentRow.Cells["物品ID"].Value.ToString());
                txtName.Text = dgvOrderFormList.CurrentRow.Cells["物品名称"].Value.ToString();
                txtOrderFormNumber.Text = dgvOrderFormList.CurrentRow.Cells["订单号"].Value.ToString();
                txtProvider.Text = dgvOrderFormList.CurrentRow.Cells["供应商"].Value.ToString();
                txtSpec.Text = dgvOrderFormList.CurrentRow.Cells["规格"].Value.ToString();
                txtStockQuota.Text = dgvOrderFormList.CurrentRow.Cells["采购份额"].Value.ToString();
                txtStockQuotaCount.Text = dgvOrderFormList.CurrentRow.Cells["预计采购总数"].Value.ToString();
                numAmount.Value = Convert.ToDecimal(dgvOrderFormList.CurrentRow.Cells["实际订货数"].Value.ToString());

                dgvArriveList.DataSource = m_orderFormAffrim.GetAffrimInfo(
                    Convert.ToInt32(dgvOrderFormList.CurrentRow.Cells["序号"].Value.ToString()));
                dgvArriveList.Columns["明细ID"].Visible = false;
                dgvArriveList.Columns["序号"].Visible = false;
            }
        }

        private void dgvOrderFormList_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            for (int i = 0; i < dgvOrderFormList.Rows.Count; i++)
            {
                if ((bool)dgvOrderFormList.Rows[i].Cells["是否已修改"].Value)
                {
                    dgvOrderFormList.Rows[i].DefaultCellStyle.BackColor = Color.Yellow;
                }
            }
        }

        private void dgvArriveList_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvArriveList.Rows.Count == 0)
            {
                return;
            }
            else
            {
                dateTimeArrivalDate.Value = Convert.ToDateTime(dgvArriveList.CurrentRow.Cells["到货日期"].Value.ToString());
                numCount.Value = Convert.ToDecimal(dgvArriveList.CurrentRow.Cells["到货数"].Value.ToString());
                dateTimeArrivalDate.Tag = Convert.ToInt32(dgvArriveList.CurrentRow.Cells["序号"].Value.ToString());
                txtBargainNumber.Tag = Convert.ToInt32(dgvArriveList.CurrentRow.Cells["明细ID"].Value.ToString());
            }
        }
    }
}

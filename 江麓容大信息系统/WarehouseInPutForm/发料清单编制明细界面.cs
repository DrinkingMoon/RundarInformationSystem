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
using Service_Manufacture_Storage;
using FlowControlService;

namespace Form_Manufacture_Storage
{
    public partial class 发料清单编制明细界面 : CustomFlowForm
    {
        List<View_S_DebitSchedule> m_listDebitSchedule_Temp = new List<View_S_DebitSchedule>();

        /// <summary>
        /// 单据编号控制类
        /// </summary>
        BillNumberControl m_billNoControl;

        /// <summary>
        /// 单据类消息发布器
        /// </summary>
        IBillMessagePromulgatorServer m_billMessageServer = BasicServerFactory.GetServerModule<IBillMessagePromulgatorServer>();

        /// <summary>
        /// 发料清单服务组件
        /// </summary>
        Service_Manufacture_Storage.IProductOrder m_findProductOrder = Service_Manufacture_Storage.ServerModuleFactory.GetServerModule<Service_Manufacture_Storage.IProductOrder>();

        /// <summary>
        /// 数据列表
        /// </summary>
        List<View_S_DebitSchedule> m_listDebitSchedule = new List<View_S_DebitSchedule>();

        /// <summary>
        /// 单据号
        /// </summary>
        private string m_strBillNo;

        public string StrBillNo
        {
            get { return m_strBillNo; }
            set { m_strBillNo = value; }
        }

        /// <summary>
        /// 流程服务组件
        /// </summary>
        IFlowServer m_serverFlow = FlowControlService.ServerModuleFactory.GetServerModule<IFlowServer>();

        public 发料清单编制明细界面()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 清空信息
        /// </summary>
        void ClearInfo()
        {
            txtSDBNo.Text = "";
            lbSDBStatus.Text = "";
            txtCode.Text = "";
            txtName.Text = "";
            txtSpec.Text = "";
            numRedices.Value = 0;
            txtEditionCode.Text = "";

            cmbApplicable.DataSource = GeneralFunction.GetEumnList(typeof(CE_DebitScheduleApplicable));
            cmbApplicable.SelectedIndex = -1;
        }

        /// <summary>
        /// 显示信息
        /// </summary>
        void ShowInfo()
        {
            string status = m_serverFlow.GetNowBillStatus(m_strBillNo);

            if (status == null || status.Trim().Length == 0)
            {
                lbSDBStatus.Text = ProductOrderListStatus.新建单据.ToString();
                txtSDBNo.Text = this.FlowInfo_BillNo;
            }
            else
            {
                txtSDBNo.Text = m_strBillNo;
                lbSDBStatus.Text = status;

                if (m_listDebitSchedule != null && m_listDebitSchedule.Count > 0)
                {
                    txtEditionCode.Text = m_listDebitSchedule.First().总成编码;
                    cmbApplicable.Text = m_listDebitSchedule.First().适用范围;

                    txtEditionCode.Enabled = false;
                    cmbApplicable.Enabled = false;
                }

                if (status == ProductOrderListStatus.等待校验.ToString())
                {
                    btnAdd.Enabled = false;
                    btnDelete.Enabled = false;
                    btnModify.Enabled = false;
                }
            }

            RefreshDataGridView(m_listDebitSchedule);
        }

        void RefreshDataGridView(List<View_S_DebitSchedule> listInfo)
        {
            if (listInfo != null)
            {
                dataGridView1.DataSource = new BindingCollection<View_S_DebitSchedule>(listInfo);

                dataGridView1.Columns["单据号"].Visible = false;
                dataGridView1.Columns["总成编码"].Visible = false;
                dataGridView1.Columns["ID"].Visible = false;
                dataGridView1.Columns["适用范围"].Visible = false;

                userControlDataLocalizer1.Init(dataGridView1, this.Name,
                    UniversalFunction.SelectHideFields(this.Name, dataGridView1.Name, BasicInfo.LoginID));
            }
        }

        private void txtCode_OnCompleteSearch()
        {
            txtCode.Text = txtCode.DataResult["图号型号"].ToString();
            txtCode.Tag = Convert.ToInt32(txtCode.DataResult["序号"]);
            txtName.Text = txtCode.DataResult["物品名称"].ToString();
            txtSpec.Text = txtCode.DataResult["规格"].ToString();
        }

        private void txtEditionCode_OnCompleteSearch()
        {
            if (cmbApplicable.Text.Length == 0)
            {
                MessageDialog.ShowPromptMessage("请先选择适用范围");
                txtEditionCode.Text = "";
                txtEditionCode.Tag = null;
                return;
            }

            if (txtEditionCode.DataResult == null)
            {
                return;
            }

            txtEditionCode.Text = txtEditionCode.DataResult["图号型号"].ToString();

            if (lbSDBStatus.Text == ProductOrderListStatus.新建单据.ToString())
            {
                m_listDebitSchedule = new List<View_S_DebitSchedule>();
                DataTable tempTable = m_findProductOrder.GetAllData(txtEditionCode.Text.ToString(), 
                    GlobalObject.GeneralFunction.StringConvertToEnum<CE_DebitScheduleApplicable>(cmbApplicable.Text));

                foreach (DataRow dr in tempTable.Rows)
                {
                    View_S_DebitSchedule tempLnq = new View_S_DebitSchedule();

                    tempLnq.单据号 = txtSDBNo.Text;
                    tempLnq.规格 = dr["规格"].ToString();
                    tempLnq.基数 = (decimal)dr["基数"];
                    tempLnq.图号型号 = dr["零件编码"].ToString();
                    tempLnq.物品ID = (int)dr["物品ID"];
                    tempLnq.物品名称 = dr["零件名称"].ToString();
                    tempLnq.总成编码 = dr["产品编码"].ToString();
                    tempLnq.适用范围 = dr["适用范围"].ToString();
                    tempLnq.一次性整台份发料 = Convert.ToBoolean( dr["一次性整台份发料"]);

                    m_listDebitSchedule.Add(tempLnq);
                }

                RefreshDataGridView(m_listDebitSchedule);
            }
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                txtCode.Text = dataGridView1.CurrentRow.Cells["图号型号"].Value.ToString();
                txtCode.Tag = dataGridView1.CurrentRow.Cells["物品ID"].Value;
                txtName.Text = dataGridView1.CurrentRow.Cells["物品名称"].Value.ToString();
                txtSpec.Text = dataGridView1.CurrentRow.Cells["规格"].Value.ToString();
                numRedices.Value = Convert.ToDecimal(dataGridView1.CurrentRow.Cells["基数"].Value);
                chbOnceTheWholeIssue.Checked = Convert.ToBoolean(dataGridView1.CurrentRow.Cells["一次性整台份发料"].Value);
            }
        }

        private bool customForm_PanelGetDateInfo(CE_FlowOperationType flowOperationType)
        {
            m_listDebitSchedule = (dataGridView1.DataSource as BindingCollection<View_S_DebitSchedule>).ToList();

            ResultInfo = m_listDebitSchedule;
            FlowInfo_BillNo = txtSDBNo.Text;

            List<object> listobj = new List<object>();

            listobj.Add((object)flowOperationType);
            listobj.Add((object)txtEditionCode.Text);
            listobj.Add((object)cmbApplicable.Text);

            ResultList = listobj;

            return true;
        }

        bool CheckDate_ADD()
        {
            if (cmbApplicable.Text.Length == 0)
            {
                MessageDialog.ShowPromptMessage("请选择【适用范围】");
                return false;
            }

            int goodsID = 0;

            if (txtName.Text.Trim().Length == 0 || txtCode.Tag == null || !Int32.TryParse(txtCode.Tag.ToString(), out goodsID))
            {
                MessageDialog.ShowPromptMessage("请选择【物品】");
                return false;
            }

            if (txtEditionCode.Text.Trim().Length == 0)
            {
                MessageDialog.ShowPromptMessage("请选择【总成编码】");
                return false;
            }

            foreach (DataGridViewRow dgvr in dataGridView1.Rows)
            {
                if (dgvr.Cells["物品ID"].Value.ToString() == txtCode.Tag.ToString())
                {
                    MessageDialog.ShowPromptMessage(UniversalFunction.GetGoodsMessage((int)txtCode.Tag) + "已存在一条记录，不能插入重复记录");
                    return false;
                }
            }

            return true;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!CheckDate_ADD())
            {
                return;
            }

            m_listDebitSchedule = (dataGridView1.DataSource as BindingCollection<View_S_DebitSchedule>).ToList();

            View_S_DebitSchedule tempLnq = new View_S_DebitSchedule();

            tempLnq.单据号 = txtSDBNo.Text;
            tempLnq.规格 = txtSpec.Text;
            tempLnq.图号型号 = txtCode.Text;
            tempLnq.物品ID = (int)txtCode.Tag;
            tempLnq.物品名称 = txtName.Text;
            tempLnq.总成编码 = txtEditionCode.Text;
            tempLnq.基数 = numRedices.Value;
            tempLnq.适用范围 = cmbApplicable.Text;
            tempLnq.一次性整台份发料 = chbOnceTheWholeIssue.Checked;

            m_listDebitSchedule.Add(tempLnq);

            RefreshDataGridView(m_listDebitSchedule);
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                return;
            }

            dataGridView1.CurrentRow.Cells["图号型号"].Value = txtCode.Text;
            dataGridView1.CurrentRow.Cells["规格"].Value = txtSpec.Text;
            dataGridView1.CurrentRow.Cells["物品ID"].Value = txtCode.Tag;
            dataGridView1.CurrentRow.Cells["物品名称"].Value = txtName.Text;
            dataGridView1.CurrentRow.Cells["基数"].Value = numRedices.Value;
            dataGridView1.CurrentRow.Cells["一次性整台份发料"].Value = chbOnceTheWholeIssue.Checked;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                return;
            }

            dataGridView1.Rows.Remove(dataGridView1.CurrentRow);
        }

        private void btn_Import_Click(object sender, EventArgs e)
        {
            //string error = "";

            DataTable dtTemp = ExcelHelperP.RenderFromExcel(openFileDialog1);

            if (dtTemp == null)
            {
                //MessageDialog.ShowPromptMessage(error);
                return;
            }

            if (!dtTemp.Columns.Contains("图号型号"))
            {
                MessageDialog.ShowPromptMessage("无【图号型号】列");
            }

            if (!dtTemp.Columns.Contains("物品名称"))
            {
                MessageDialog.ShowPromptMessage("无【物品名称】列");
            }

            if (!dtTemp.Columns.Contains("规格"))
            {
                MessageDialog.ShowPromptMessage("无【规格】列");
            }

            if (!dtTemp.Columns.Contains("基数"))
            {
                MessageDialog.ShowPromptMessage("无【基数】列");
            }

            if (!dtTemp.Columns.Contains("一次性整台份发料"))
            {
                MessageDialog.ShowPromptMessage("无【一次性整台份发料】列");
            }

            m_listDebitSchedule = new List<View_S_DebitSchedule>();

            foreach (DataRow dr in dtTemp.Rows)
            {
                View_S_DebitSchedule tempLnq = new View_S_DebitSchedule();

                int goodsID = UniversalFunction.GetGoodsID(dr["图号型号"].ToString(), dr["物品名称"].ToString(), dr["规格"].ToString());

                if (goodsID != 0)
                {
                    tempLnq.物品ID = goodsID;

                    foreach (View_S_DebitSchedule item in m_listDebitSchedule)
                    {
                        if (item.物品ID == tempLnq.物品ID)
                        {
                            MessageDialog.ShowPromptMessage(UniversalFunction.GetGoodsMessage(tempLnq.物品ID) 
                                + "已存在一条记录，不能插入重复记录");
                        }
                    }
                }
                else
                {
                    MessageDialog.ShowPromptMessage("图号：" + dr["图号型号"].ToString()
                        + " 名称:" + dr["物品名称"].ToString() + " 规格:" + dr["规格"].ToString() + " 不存在于系统中");
                    return;
                }

                if (dr["基数"] == null || Convert.ToInt32(dr["基数"]) == 0)
                {
                    MessageDialog.ShowPromptMessage("图号：" + dr["图号型号"].ToString()
                        + " 名称:" + dr["物品名称"].ToString() + " 规格:" + dr["规格"].ToString() + " 【基数】不符合要求,请重新检查");
                }
                else
                {
                    tempLnq.基数 = Convert.ToDecimal(dr["基数"]);
                }

                if (dr["一次性整台份发料"] == null)
                {
                    MessageDialog.ShowPromptMessage("图号：" + dr["图号型号"].ToString()
                        + " 名称:" + dr["物品名称"].ToString() + " 规格:" + dr["规格"].ToString() + " 【一次性整台份发料】不符合要求,请重新检查");
                }
                else
                {
                    tempLnq.一次性整台份发料 = dr["一次性整台份发料"].ToString().Trim() == "是" ? true : false;
                }

                tempLnq.单据号 = txtSDBNo.Text;
                tempLnq.规格 = dr["规格"].ToString();
                tempLnq.图号型号 = dr["图号型号"].ToString();
                tempLnq.物品名称 = dr["物品名称"].ToString();
                tempLnq.总成编码 = txtEditionCode.Text;
                tempLnq.适用范围 = cmbApplicable.Text;

                m_listDebitSchedule.Add(tempLnq);
            }

            RefreshDataGridView(m_listDebitSchedule);
            MessageDialog.ShowPromptMessage("导入完成");
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Columns[e.ColumnIndex].Name == "一次性整台份发料")
            {
                dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = 
                    !Convert.ToBoolean(dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
            }
        }

        private void 选择ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1 != null && dataGridView1.Rows.Count > 0)
            {
                foreach (DataGridViewRow dgvr in dataGridView1.SelectedRows)
                {
                    dgvr.Cells["一次性整台份发料"].Value = true;
                }
            }
        }

        private void 取消ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1 != null && dataGridView1.Rows.Count > 0)
            {
                foreach (DataGridViewRow dgvr in dataGridView1.SelectedRows)
                {
                    dgvr.Cells["一次性整台份发料"].Value = false;
                }
            }
        }

        private void 全选ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1 != null && dataGridView1.Rows.Count > 0)
            {
                foreach (DataGridViewRow dgvr in dataGridView1.Rows)
                {
                    dgvr.Cells["一次性整台份发料"].Value = true;
                }
            }
        }

        private void 全消ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1 != null && dataGridView1.Rows.Count > 0)
            {
                foreach (DataGridViewRow dgvr in dataGridView1.Rows)
                {
                    dgvr.Cells["一次性整台份发料"].Value = false;
                }
            }
        }

        public override void LoadFormInfo()
        {
            try
            {
                m_strBillNo = this.FlowInfo_BillNo;
                m_billNoControl = new BillNumberControl(CE_BillTypeEnum.发料清单.ToString(), m_findProductOrder);
                m_billMessageServer.BillType = CE_BillTypeEnum.发料清单.ToString();
                m_listDebitSchedule = m_findProductOrder.GetListInfo(this.FlowInfo_BillNo);

                ClearInfo();
                ShowInfo();

                m_strBillNo = txtSDBNo.Text;
            }
            catch (Exception ex)
            {
                MessageDialog.ShowErrorMessage(ex.Message);
            }
        }

        private void txtEditionCode_Enter(object sender, EventArgs e)
        {
            txtEditionCode.StrEndSql = " and 序号 in (select distinct ParentID from BASE_BomStruct "+
                                       " where ParentID in (select GoodsID from F_GoodsAttributeRecord   "+
                                       " where AttributeID in (" + (int)CE_GoodsAttributeName.CVT + "," 
                                       + (int)CE_GoodsAttributeName.TCU + ","
                                       + (int)CE_GoodsAttributeName.部件 + ") and AttributeValue = '" + bool.TrueString + "'))";
        }

        private void cmbApplicable_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbApplicable.Text.Trim().Length > 0 && txtEditionCode.Text.Trim().Length > 0)
            {
                txtEditionCode_OnCompleteSearch();
            }
        }

        void SetTempSource()
        {
            m_listDebitSchedule_Temp = new List<View_S_DebitSchedule>();

            foreach (View_S_DebitSchedule item in m_listDebitSchedule)
            {
                View_S_DebitSchedule tempLnq = new View_S_DebitSchedule();

                tempLnq.单据号 = item.单据号;
                tempLnq.规格 = item.规格;
                tempLnq.基数 = item.基数;
                tempLnq.图号型号 = item.图号型号;
                tempLnq.物品ID = item.物品ID;
                tempLnq.物品名称 = item.物品名称;
                tempLnq.总成编码 = item.总成编码;
                tempLnq.适用范围 = item.适用范围;
                tempLnq.一次性整台份发料 = item.一次性整台份发料;

                m_listDebitSchedule_Temp.Add(tempLnq);
            }
        }

        private void chbDelete_CheckedChanged(object sender, EventArgs e)
        {
            if (chbDelete.Checked)
            {
                SetTempSource();
                foreach (DataGridViewRow dgvr in dataGridView1.Rows)
                {
                    dgvr.Cells["基数"].Value = (decimal)0;
                }
            }
            else
            {
                m_listDebitSchedule = m_listDebitSchedule_Temp;
                RefreshDataGridView(m_listDebitSchedule);
            }
        }

        private void txtCode_Enter(object sender, EventArgs e)
        {
            txtCode.StrEndSql = " and 禁用 = 0";
        }
    }
}

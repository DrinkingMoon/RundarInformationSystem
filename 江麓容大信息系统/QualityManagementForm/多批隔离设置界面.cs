using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ServerModule;
using Service_Quality_QC;
using GlobalObject;
using FlowControlService;
using UniversalControlLibrary;

namespace Form_Quality_QC
{
    public partial class 多批隔离设置界面 : Form
    {
        NotifyPersonnelInfo FlowInfo_NotifyInfo = null;

        IFlowServer m_serverFlow = FlowControlService.ServerModuleFactory.GetServerModule<IFlowServer>();

        /// <summary>
        /// 单据编号控制类
        /// </summary>
        BillNumberControl m_billNoControl;

        /// <summary>
        /// 服务组件
        /// </summary>
        IRejectIsolationService m_isolationService = Service_Quality_QC.ServerModuleFactory.GetServerModule<IRejectIsolationService>();

        public 多批隔离设置界面()
        {
            InitializeComponent();

            m_billNoControl = new BillNumberControl(CE_BillTypeEnum.不合格品隔离处置单.ToString(), m_isolationService);
            DataTable dtStorageInfo = UniversalFunction.GetStorageTb();

            cmbStorageID.DataSource = dtStorageInfo;
            cmbStorageID.DisplayMember = "StorageName";
        }

        private void txtGoodsCode_Enter(object sender, EventArgs e)
        {
            if (cmbStorageID.Text.Trim().Length > 0)
            {
                txtGoodsCode.StrEndSql = " and 库房代码 = '" + UniversalFunction.GetStorageID(cmbStorageID.Text) + "' and 库存数量 > 0";
            }
        }

        private void txtGoodsCode_OnCompleteSearch()
        {
            if (txtGoodsCode.DataResult != null)
            {
                txtGoodsCode.Tag = Convert.ToInt32(txtGoodsCode.DataResult["序号"]);
                txtGoodsCode.Text = txtGoodsCode.DataResult["图号型号"].ToString();
                txtGoodsName.Text = txtGoodsCode.DataResult["物品名称"].ToString();
                txtSpec.Text = txtGoodsCode.DataResult["规格"].ToString();

                txtProvider.Text = "";
                txtIsolationReason.Text = "";
                numGoodsCount.Value = 0;
                txtBatchNo.Text = "";
            }
            else
            {
                txtGoodsCode.Tag = null;
                txtGoodsName.Text = "";
                txtSpec.Text = "";
            }
        }

        private void txtBatchNo_OnCompleteSearch()
        {
            if (txtBatchNo.DataTableResult != null)
            {
                if (txtBatchNo.DataTableResult.Rows.Count > 1)
                {
                    txtProvider.Text = "";
                    numGoodsCount.Value = 0;
                }
                else if (txtBatchNo.DataTableResult.Rows.Count == 1)
                {
                    txtProvider.Text = txtBatchNo.DataResult["供应商编码"].ToString();
                    numGoodsCount.Value = Convert.ToDecimal(txtBatchNo.DataResult["库存数量"]);
                }
            }
            else
            {
                txtProvider.Text = "";
                numGoodsCount.Value = 0;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            List<string> lstBatchNo = txtBatchNo.Text.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();

            foreach (string batchNo in lstBatchNo)
            {
                foreach (DataGridViewRow dgvr in customDataGridView1.Rows)
                {
                    if (dgvr.Cells["物品ID"].Value.ToString() == txtGoodsCode.Tag.ToString()
                        && dgvr.Cells["所属库房"].Value.ToString() == cmbStorageID.Text
                        && dgvr.Cells["批次号"].Value.ToString() == batchNo)
                    {
                        MessageBox.Show("已存在【批次号】：" + txtBatchNo.Text + " 【库房名称】："
                            + cmbStorageID.Text + " 的记录，不能重复添加");
                        return;
                    }
                }

                if (txtIsolationReason.Text.Trim().Length == 0)
                {
                    MessageDialog.ShowPromptMessage("请填写【隔离原因】");
                    return;
                }

                if (m_isolationService.IsRepeatIsolation(Convert.ToInt32(txtGoodsCode.Tag), batchNo,
                    UniversalFunction.GetStorageID(cmbStorageID.Text)))
                {
                    MessageDialog.ShowPromptMessage("批次号【" + txtBatchNo.Text + "】已隔离，不能重复隔离");
                    return;
                }
            }

            foreach (DataRow dr in txtBatchNo.DataTableResult.Rows)
            {
                customDataGridView1.Rows.Add(new object[] {txtGoodsCode.Text, txtGoodsName.Text, txtSpec.Text, 
                    Convert.ToDecimal(dr["库存数量"]), dr["批次号"].ToString(), dr["供应商编码"].ToString(), txtIsolationReason.Text, 
                    cmbStorageID.Text, UniversalFunction.GetStorageID(cmbStorageID.Text), txtGoodsCode.Tag });
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            customDataGridView1.Rows.Remove(customDataGridView1.CurrentRow);
        }

        private void txtBatchNo_Enter(object sender, EventArgs e)
        {
            if (cmbStorageID.Text.Trim().Length > 0 && txtGoodsCode.Tag != null)
            {
                txtBatchNo.StrEndSql = " and 库房代码 = '" + UniversalFunction.GetStorageID(cmbStorageID.Text)
                    + "' and 物品ID = " + Convert.ToInt32(txtGoodsCode.Tag) + " and 库存数量 > 0";
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public bool GetNotifyPersonnel()
        {
            FlowInfo_NotifyInfo = new NotifyPersonnelInfo();
            FormSelectPersonnel2 frm = new FormSelectPersonnel2();

            if (frm.ShowDialog() != DialogResult.OK)
            {
                return false;
            }

            FlowInfo_NotifyInfo = frm.SelectedNotifyPersonnelInfo;
            return true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow dgvr in customDataGridView1.Rows)
            {
                string billNo = m_billNoControl.GetNewBillNo();

                bool isParallel = false;
                if (FlowInfo_NotifyInfo == null && m_serverFlow.IsPointPersonnel(billNo, out isParallel))
                {
                    CustomFlowForm flowForm = new CustomFlowForm();
                    if (!flowForm.GetNotifyPersonnel(true))
                    {
                        throw new Exception("请选择指定人或者角色点【确定】");
                    }
                    else
                    {
                        FlowInfo_NotifyInfo = flowForm.FlowInfo_NotifyInfo;
                    }
                }

                Business_QualityManagement_Isolation tempLnq = new Business_QualityManagement_Isolation();

                tempLnq.BatchNo = dgvr.Cells["批次号"].Value.ToString();
                tempLnq.BillNo = billNo;
                tempLnq.GoodsCount = Convert.ToDecimal(dgvr.Cells["数量"].Value);
                tempLnq.GoodsID = Convert.ToInt32(dgvr.Cells["物品ID"].Value);
                tempLnq.IsolationReason = dgvr.Cells["隔离原因及处理要求"].Value.ToString();
                tempLnq.StorageID = dgvr.Cells["库房代码"].Value.ToString();
                tempLnq.Provider = dgvr.Cells["供应商"].Value.ToString();

                m_isolationService.SaveInfo(tempLnq);
                m_isolationService.FinishBill(billNo);
                string keyWords = "【" + UniversalFunction.GetGoodsInfo(tempLnq.GoodsID).物品名称 + "】【" + tempLnq.BatchNo + "】";
                m_serverFlow.FlowPass(tempLnq.BillNo, "", tempLnq.StorageID, FlowInfo_NotifyInfo, keyWords);
            }

            MessageBox.Show("已全部提交");
            this.Close();
        }
    }
}

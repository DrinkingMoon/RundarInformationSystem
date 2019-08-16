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
    public partial class 看板生成领料单 : Form
    {
        IMaterialRequisitionServer serviceRequistion =
            ServerModule.ServerModuleFactory.GetServerModule<IMaterialRequisitionServer>();

        public 看板生成领料单()
        {
            InitializeComponent();
        }

        private void txtBarCode_TextChanged(object sender, EventArgs e)
        {
            if (cmbPickingType.Text.Trim().Length == 0)
            {
                MessageDialog.ShowPromptMessage("请选择【领料类型】");
                return;
            }

            if (txtBarCode.Text.Trim().Length == 11)
            {
                AddItem(txtBarCode.Text);
                txtBarCode.Text = "";
            }
        }

        void AddItem(string barCode)
        {
            int goodsID = Convert.ToInt32(barCode.Substring(0, 6));
            decimal goodsCount = Convert.ToDecimal(barCode.Substring(6, 5));

            View_F_GoodsPlanCost goodsInfo = UniversalFunction.GetGoodsInfo(goodsID);

            customDataGridView1.Rows.Add(new object[] { barCode, goodsInfo.序号, goodsInfo.图号型号, 
                goodsInfo.物品名称, goodsInfo.规格, goodsCount, cmbPickingType.Text, txtRemark.Text });

            S_MaterialRequisition_BoardIssue issue = new S_MaterialRequisition_BoardIssue();

            issue.BarCode = barCode;
            issue.IssueType = cmbPickingType.Text;
            issue.Remark = txtRemark.Text;

            serviceRequistion.AddBoardIssue(issue);
        }

        private void 看板生成领料单_Load(object sender, EventArgs e)
        {
            txtBarCode.Focus();

            List<S_MaterialRequisition_BoardIssue> listIssue = serviceRequistion.GetBoardIssueInfo(null);

            cmbStorageID.DataSource = UniversalFunction.GetStorageTb();
            cmbStorageID.DisplayMember = "StorageName";

            foreach (S_MaterialRequisition_BoardIssue item in listIssue)
            {
                int goodsID = Convert.ToInt32(item.BarCode.Substring(0, 6));
                decimal goodsCount = Convert.ToDecimal(item.BarCode.Substring(6, 5));

                View_F_GoodsPlanCost goodsInfo = UniversalFunction.GetGoodsInfo(goodsID);

                customDataGridView1.Rows.Add(new object[] { item.BarCode, goodsInfo.序号, goodsInfo.图号型号, 
                    goodsInfo.物品名称, goodsInfo.规格, goodsCount, item.IssueType, item.Remark });
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow dgvr in customDataGridView1.SelectedRows)
            {
                customDataGridView1.Rows.Remove(dgvr);
                S_MaterialRequisition_BoardIssue issue = new S_MaterialRequisition_BoardIssue();

                issue.BarCode = dgvr.Cells["条形码编码"].Value.ToString();
                issue.IssueType = dgvr.Cells["领料类型"].Value.ToString();

                serviceRequistion.DeleteBoardIssue(issue);
            }
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            if (customDataGridView1.Rows.Count == 0)
            {
                MessageDialog.ShowPromptMessage("请在条码录入框内扫入需要领用的物料条码");
                return;
            }

            try
            {
                List<string> lstType = serviceRequistion.GetBoardIssueInfo(null).Select( k => k.IssueType).Distinct().ToList();

                foreach (string issueType in lstType)
                {
                    List<S_MaterialRequisition_BoardIssue> listIssue = serviceRequistion.GetBoardIssueInfo(issueType);
                    List<S_MaterialRequisitionGoods> lstGoodsInfo = new List<S_MaterialRequisitionGoods>();

                    foreach (S_MaterialRequisition_BoardIssue boardIssue in listIssue)
                    {
                        S_MaterialRequisitionGoods goodsInfo = new S_MaterialRequisitionGoods();

                        goodsInfo.GoodsID = Convert.ToInt32(boardIssue.BarCode.Substring(0, 6));
                        goodsInfo.RequestCount = Convert.ToDecimal(boardIssue.BarCode.Substring(6, 5));
                        goodsInfo.Remark = boardIssue.Remark;

                        lstGoodsInfo.Add(goodsInfo);
                    }

                    serviceRequistion.AutoCreateBoardPicking(lstGoodsInfo, issueType, UniversalFunction.GetStorageID(cmbStorageID.Text));
                    serviceRequistion.DeleteBoardIssue(issueType);
                }

                MessageDialog.ShowPromptMessage("生成完成");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
                return;
            }
        }
    }
}

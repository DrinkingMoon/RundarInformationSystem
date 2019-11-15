using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ServerModule;
using GlobalObject;
using PlatformManagement;
using UniversalControlLibrary;
using Service_Project_Design;
using System.IO;
using FlowControlService;

namespace Form_Project_Design
{
    public partial class 生产BOM变更单明细 : CustomFlowForm
    {
        IBomServer _serviceBOM = ServerModule.ServerModuleFactory.GetServerModule<IBomServer>();

        IBOMInfoService _serviceBOMInfo = Service_Project_Design.ServerModuleFactory.GetServerModule<IBOMInfoService>();

        IFlowServer _serviceFlow = FlowControlService.ServerModuleFactory.GetServerModule<IFlowServer>();

        IPBOMChangeService _servicePBOMChange = Service_Project_Design.ServerModuleFactory.GetServerModule<IPBOMChangeService>();

        Bus_PBOM_Change _lnqPBOMBill = new Bus_PBOM_Change();

        BillNumberControl _billNoControl;

        public Bus_PBOM_Change LnqPBOMBill
        {
            get { return _lnqPBOMBill; }
            set { _lnqPBOMBill = value; }
        }

        List<View_Bus_PBOM_Change_Detail> _listDetail = new List<View_Bus_PBOM_Change_Detail>();

        public 生产BOM变更单明细()
        {
            InitializeComponent();
        }

        public override void LoadFormInfo()
        {
            try
            {
                List<string> lstAsscembly = _serviceBOM.GetAssemblyTypeList();

                if (lstAsscembly != null)
                {
                    cmbEdition.DataSource = lstAsscembly;

                    if (cmbEdition.Items.Count > 0)
                    {
                        cmbEdition.SelectedIndex = -1;
                        cmbDBOMVersion.DataSource = null;
                        cmbDBOMVersion.SelectedIndex = -1;
                    }
                }

                _billNoControl = new BillNumberControl(CE_BillTypeEnum.生产BOM变更单.ToString(), _servicePBOMChange);
                _lnqPBOMBill = _servicePBOMChange.GetItem(this.FlowInfo_BillNo);
                _listDetail = _servicePBOMChange.GetDetail(this.FlowInfo_BillNo);

                SetInfo();
            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
            }
        }

        private bool 生产BOM变更单明细_PanelGetDataInfo(CE_FlowOperationType flowOperationType)
        {
            try
            {
                GetInfo();

                if (lbBillStatus.Text == CE_CommonBillStatus.新建单据.ToString() 
                    && flowOperationType == CE_FlowOperationType.提交)
                {
                    CheckInfo();
                }

                FlowOperationType = flowOperationType;
                ResultInfo = _lnqPBOMBill;

                this.ResultList = new List<object>();
                ResultList.Add(_listDetail);

                return true;
            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
                return false;
            }
        }

        void SetInfo()
        {
            txtBillNo.Text = this.FlowInfo_BillNo;
            string billStatus = _serviceFlow.GetNowBillStatus(this.FlowInfo_BillNo);
            lbBillStatus.Text = billStatus == "" ? CE_CommonBillStatus.新建单据.ToString() : billStatus;

            if (_lnqPBOMBill != null)
            {
                txtFileCode.Text = _lnqPBOMBill.FileCode;
                txtReason.Text = _lnqPBOMBill.Reason;
                lbDownFile.Tag = _lnqPBOMBill.FileInfo.ToArray();
            }

            if (lbBillStatus.Text != CE_CommonBillStatus.新建单据.ToString())
            {
                lbUpFile.Enabled = false;
            }

            lbDownFile.Enabled = lbDownFile.Tag == null ? false : true;

            var varData = (from a in _listDetail select new { 总成型号 = a.总成型号, 设计BOM版本 = a.设计BOM版本 }).Distinct();

            foreach (var item in varData)
            {
                dataGridViewEdtion.Rows.Add(new object[] { item.总成型号, item.设计BOM版本 });
            }
        }

        void GetInfo()
        {
            _lnqPBOMBill = new Bus_PBOM_Change();

            _lnqPBOMBill.BillNo = txtBillNo.Text;
            _lnqPBOMBill.FileCode = txtFileCode.Text;

            if (lbDownFile.Tag != null)
            {
                _lnqPBOMBill.FileInfo = lbDownFile.Tag as Byte[];
            }

            _lnqPBOMBill.Reason = txtReason.Text;
        }

        void CheckInfo()
        {
            if (string.IsNullOrEmpty(_lnqPBOMBill.FileCode))
            {
                throw new Exception("技术变更单号不能为空");
            }

            if (string.IsNullOrEmpty(_lnqPBOMBill.Reason))
            {
                throw new Exception("变更原因不能为空");
            }

            if (_lnqPBOMBill.FileInfo == null)
            {
                throw new Exception("请上传技术通知单文档");
            }

            if (_listDetail.Count() == 0)
            {
                throw new Exception("请添加具体变更项");
            }

            foreach (View_Bus_PBOM_Change_Detail item in _listDetail)
            {
                if (item.生效日期 != null && string.IsNullOrEmpty(item.失效版次号))
                {
                    throw new Exception("【总成型号】：" + item.总成型号 +  UniversalFunction.GetGoodsMessage(item.物品ID) + "具有【生效日期】的同时必须填写【失效版次号】");
                }
            }
        }

        private void dataGridViewEdtion_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (_listDetail == null || _listDetail.Count() == 0)
            {
                return;
            }

            dataGridViewStruct.DataSource = (from a in _listDetail
                                             where a.总成型号 ==
                                                dataGridViewEdtion.CurrentRow.Cells["总成型号_ZC"].Value.ToString()
                                             orderby a.父级图号, a.零件图号
                                             select a).ToList();

            userControlDataLocalizer1.Init(dataGridViewStruct, this.Name,
                UniversalFunction.SelectHideFields(this.Name, dataGridViewStruct.Name, BasicInfo.LoginID));
        }

        private void btnParentAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(cmbEdition.Text))
                {
                    throw new Exception("请选择【总成型号】");
                }

                if (cmbDBOMVersion.Text.Length == 0)
                {
                    throw new Exception("请选择对应的【设计BOM版本号】");
                }

                List<BASE_BomVersion> lstVersion =
                    _serviceBOMInfo.GetBOMVersionInfoItems(cmbEdition.Text, Convert.ToDecimal(cmbDBOMVersion.Text));

                foreach (BASE_BomVersion item in lstVersion)
                {
                    View_Bus_PBOM_Change_Detail tempLnq = new View_Bus_PBOM_Change_Detail();

                    View_F_GoodsPlanCost goodsInfo = UniversalFunction.GetGoodsInfo((int)item.GoodsID);

                    tempLnq.单据号 = txtBillNo.Text;

                    if (item.ParentGoodsID != null)
                    {
                        View_F_GoodsPlanCost parentInfo = UniversalFunction.GetGoodsInfo((int)item.ParentGoodsID);
                        tempLnq.父级图号 = parentInfo.图号型号;
                        tempLnq.父级物品ID = parentInfo.序号;
                    }

                    tempLnq.基数 = item.Usage;
                    tempLnq.零件规格 = goodsInfo.规格;
                    tempLnq.零件名称 = goodsInfo.物品名称;
                    tempLnq.零件图号 = goodsInfo.图号型号;

                    List<View_BASE_BomStruct> lstTemp = _serviceBOMInfo.GetBOMList_Design(goodsInfo.序号);

                    if (lstTemp.Count > 0)
                    {
                        tempLnq.领料 = false;
                        tempLnq.采购 = false;
                    }
                    else
                    {
                        tempLnq.领料 = true;
                        tempLnq.采购 = true;
                    }

                    tempLnq.设计BOM版本 = item.DBOMSysVersion;
                    tempLnq.生效版次号 = item.GoodsVersion;
                    tempLnq.生效日期 = null;
                    tempLnq.失效版次号 = null;
                    tempLnq.物品ID = goodsInfo.序号;
                    tempLnq.总成型号 = item.Edtion;

                    _listDetail.Add(tempLnq);
                }

                dataGridViewEdtion.Rows.Add(new object[] { cmbEdition.Text, cmbDBOMVersion.Text });
            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
            }
        }

        private void btnParentDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridViewEdtion.CurrentRow == null)
                {
                    return;
                }

                _listDetail.RemoveAll(k => k.总成型号 == dataGridViewEdtion.CurrentRow.Cells["总成型号_ZC"].Value.ToString());
                dataGridViewEdtion.Rows.Remove(dataGridViewEdtion.CurrentRow);

                if (dataGridViewEdtion.Rows.Count != 0)
                {
                    dataGridViewEdtion.CurrentCell = dataGridViewEdtion.Rows[dataGridViewEdtion.Rows.Count - 1].Cells[0];
                    dataGridViewEdtion.FirstDisplayedScrollingRowIndex = dataGridViewEdtion.Rows.Count - 1;
                    dataGridViewEdtion_CellEnter(sender, new DataGridViewCellEventArgs(0, dataGridViewEdtion.Rows.Count - 1));
                }
                else
                {
                    dataGridViewStruct.DataSource = null;
                }
            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
            }
        }

        private void lbUpFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofdPic = new OpenFileDialog();
            ofdPic.Filter = "doc files (*.doc)|*.doc|pdf files (*.pdf)|*.pdf|All files (*.*)|*.*";

            if (ofdPic.ShowDialog() == DialogResult.OK)
            {
                string sPicPaht = ofdPic.FileName.ToString();
                FileStream file = new FileStream(sPicPaht, FileMode.Open);

                if (file.Length / 1024 / 1024 > 2)
                {
                    MessageDialog.ShowPromptMessage("文件不能超过2M");
                }
                else
                {
                    BinaryReader br = new BinaryReader(file);
                    lbDownFile.Tag = br.ReadBytes((int)file.Length);
                    lbDownFile.Enabled = true;

                    if (file != null)
                    {
                        file.Close();
                    }

                    MessageDialog.ShowPromptMessage("上传成功");
                }
            }
        }

        private void lbDownFile_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "doc files (*.doc)|*.doc|pdf files (*.pdf)|*.pdf|All files (*.*)|*.*";

            if (lbDownFile.Tag == null)
            {
                MessageDialog.ShowPromptMessage("该条变更单无文件");
                return;
            }

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (lbDownFile.Tag != null || lbDownFile.Tag.ToString() != "")
                {
                    FileStream fs = new FileStream(saveFileDialog1.FileName, FileMode.Create, FileAccess.Write);
                    BinaryWriter bw = new BinaryWriter(fs);
                    bw.Write(lbDownFile.Tag as byte[]);
                    bw.Close();
                    fs.Close();

                    MessageDialog.ShowPromptMessage("下载成功");
                }
            }
        }

        private void cmbEdition_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbEdition.Text.Length == 0)
            {
                return;
            }

            DataTable tempTable = _serviceBOM.GetBomBackUpBomEdtion(cmbEdition.Text);

            if (tempTable != null && tempTable.Rows.Count > 0)
            {
                cmbDBOMVersion.ValueMember = "设计BOM版本";
                cmbDBOMVersion.DisplayMember = "设计BOM版本";
                cmbDBOMVersion.DataSource = tempTable;
                cmbDBOMVersion.SelectedIndex = 0;
            }
            else
            {
                cmbDBOMVersion.DataSource = null;
                cmbDBOMVersion.SelectedIndex = -1;
            }
        }
    }
}

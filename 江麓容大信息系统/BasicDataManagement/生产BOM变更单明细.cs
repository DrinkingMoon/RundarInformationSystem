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
            _lnqPBOMBill.FileInfo = lbDownFile.Tag as Byte[];
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
                    throw new Exception(UniversalFunction.GetGoodsMessage(item.物品ID) + "具有【生效日期】的同时必须填写【失效版次号】");
                }
            }
        }

        private void dataGridViewEdtion_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (_listDetail == null || _listDetail.Count() == 0)
            {
                return;
            }

            dataGridViewStruct.DataSource = null;

            List<View_Bus_PBOM_Change_Detail> lstTemp = (from a in _listDetail
                                                         where a.总成型号 == 
                                                            dataGridViewEdtion.CurrentRow.Cells["总成型号"].Value.ToString()
                                                            orderby a.父级图号, a.零件图号
                                                         select a).ToList();

            dataGridViewStruct.DataSource = lstTemp;
            dataGridViewStruct.Columns["单据号"].Visible = false;

            //foreach (View_Bus_PBOM_Change_Detail item in lstTemp)
            //{
            //    dataGridViewStruct.Rows.Add(new object[]{item.父级图号, item.零件图号, item.零件名称, item.零件规格, item.基数, 
            //        item.领料, item.生效版次号, item.生效日期, item.失效版次号, item.单据号,
            //        item.父级物品ID, item.物品ID, item.总成型号, item.设计BOM版本});
            //}

            userControlDataLocalizer1.Init(dataGridViewStruct, this.Name,
                UniversalFunction.SelectHideFields(this.Name, dataGridViewStruct.Name, BasicInfo.LoginID));
        }

        private void btnParentAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtParentGoodsCode.Text))
                {
                    throw new Exception("请选择【总成型号】");
                }

                if (numVersion.Value == 0)
                {
                    throw new Exception("请填写对应的【设计BOM版本号】");
                }

                List<BASE_BomVersion> lstVersion =
                    _serviceBOMInfo.GetBOMVersionItems(txtParentGoodsCode.Text, numVersion.Value);

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
                    tempLnq.领料 = false;
                    tempLnq.设计BOM版本 = item.DBOMSysVersion;
                    tempLnq.生效版次号 = item.GoodsVersion;
                    tempLnq.生效日期 = null;
                    tempLnq.失效版次号 = null;
                    tempLnq.物品ID = goodsInfo.序号;
                    tempLnq.总成型号 = item.Edtion;

                    _listDetail.Add(tempLnq);
                }

                dataGridViewEdtion.Rows.Add(new object[] {txtParentGoodsCode.Text, numVersion.Value});

                dataGridViewEdtion.CurrentCell = dataGridViewEdtion.Rows[dataGridViewEdtion.Rows.Count - 1].Cells[0];
                dataGridViewEdtion.FirstDisplayedScrollingRowIndex = dataGridViewEdtion.Rows.Count - 1;
                dataGridViewEdtion_CellEnter(sender, new DataGridViewCellEventArgs(0, dataGridViewEdtion.Rows.Count - 1));
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

                _listDetail.RemoveAll(k => k.总成型号 == dataGridViewEdtion.CurrentRow.Cells["总成型号"].Value.ToString());
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

        private void txtParentGoodsCode_OnCompleteSearch()
        {
            if (txtParentGoodsCode.DataResult != null)
            {
                txtParentGoodsCode.Tag = txtParentGoodsCode.DataResult["物品ID"];
                txtParentGoodsCode.Text = txtParentGoodsCode.DataResult["零部件编码"].ToString();
                numVersion.Value = _serviceBOMInfo.GetMaxBOMVersion(txtParentGoodsCode.Text);
            }
            else
            {
                txtParentGoodsCode.Tag = null;
                txtParentGoodsCode.Text = "";
                numVersion.Value = 0;
            }
        }

        private void txtParentGoodsCode_Enter(object sender, EventArgs e)
        {
            txtParentGoodsCode.StrEndSql = " and 物品ID in (select GoodsID from F_GoodsAttributeRecord " +
                " where AttributeID in (" + (int)CE_GoodsAttributeName.CVT +") and AttributeValue = '" + bool.TrueString + "')";
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
                        file.Close();
                }
            }
        }

        private void lbDownFile_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "pdf files (*.pdf)|*.pdf|doc files (*.doc)|*.doc|All files (*.*)|*.*";

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
                }
            }
        }

        private void dataGridViewStruct_Leave(object sender, EventArgs e)
        {
            //_listDetail.RemoveAll(r => r.总成型号 == dataGridViewEdtion.CurrentRow.Cells["总成型号"].Value.ToString());

            //foreach (DataGridViewRow dr in dataGridViewStruct.Rows)
            //{
            //    if (dr.Cells["物品ID"].Value == null || dr.Cells["物品ID"].Value.ToString() == "0")
            //    {
            //        continue;
            //    }

            //    View_Bus_PBOM_Change_Detail tempLnq = new View_Bus_PBOM_Change_Detail();

            //    tempLnq.单据号 = txtBillNo.Text;
            //    tempLnq.父级图号 = dr.Cells["父级图号"].Value == null ?;
            //    tempLnq.父级物品ID = (int?)dr.Cells["父级物品ID1"].Value;
            //    tempLnq.零件规格 = dr.Cells["零件规格"].Value.ToString();
            //    tempLnq.基数 = Convert.ToDecimal(dr.Cells["基数"].Value);
            //    tempLnq.零件图号 = dr.Cells["零件图号"].Value.ToString();
            //    tempLnq.零件名称 = dr.Cells["零件名称"].Value.ToString();
            //    tempLnq.物品ID = Convert.ToInt32(dr.Cells["物品ID"].Value);
            //    tempLnq.领料 = Convert.ToBoolean(dr.Cells["领料"].Value);
            //    tempLnq.设计BOM版本 = Convert.ToDecimal(dr.Cells["设计BOM版本1"].Value);
            //    tempLnq.生效版次号 = dr.Cells["生效版次号"].Value.ToString();
            //    tempLnq.生效日期 = dr.Cells["生效日期"].Value == null ? null : (DateTime?)dr.Cells["生效日期"].Value;
            //    tempLnq.失效版次号 = dr.Cells["失效版次号"].Value.ToString();
            //    tempLnq.总成型号 = dr.Cells["总成型号"].Value.ToString();

            //    _listDetail.Add(tempLnq);
            //}
        }
    }
}

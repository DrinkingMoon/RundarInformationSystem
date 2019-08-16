using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UniversalControlLibrary;
using ServerModule;
using GlobalObject;
using Service_Quality_QC;
using FlowControlService;
using CommonBusinessModule;

namespace Form_Quality_QC
{
    public partial class 不合格品隔离处置单明细 : CustomFlowForm
    {
        /// <summary>
        /// 单据编号控制类
        /// </summary>
        BillNumberControl m_billNoControl;

        /// <summary>
        /// 单据号
        /// </summary>
        private Business_QualityManagement_Isolation m_lnqBillInfo = new Business_QualityManagement_Isolation();

        /// <summary>
        /// 公共属性
        /// </summary>
        public Business_QualityManagement_Isolation LnqBillInfo
        {
            get { return m_lnqBillInfo; }
            set { m_lnqBillInfo = value; }
        }

        /// <summary>
        /// 服务组件
        /// </summary>
        IRejectIsolationService m_serviceIsolation = Service_Quality_QC.ServerModuleFactory.GetServerModule<IRejectIsolationService>();

        /// <summary>
        /// 流程服务组件
        /// </summary>
        IFlowServer m_serverFlow = FlowControlService.ServerModuleFactory.GetServerModule<IFlowServer>();

        /// <summary>
        /// 当前流程节点信息
        /// </summary>
        Flow_FlowInfo m_lnqFlowInfo = null;

        public 不合格品隔离处置单明细()
        {
            InitializeComponent();
        }

        public override void LoadFormInfo()
        {
            try
            {
                m_billNoControl = new BillNumberControl(CE_BillTypeEnum.不合格品隔离处置单.ToString(), m_serviceIsolation);
                m_lnqBillInfo = m_serviceIsolation.GetSingleBillInfo(this.FlowInfo_BillNo);

                DataTable dtStorageInfo = UniversalFunction.GetStorageTb();

                cmbStorageID.DataSource = dtStorageInfo;
                cmbStorageID.DisplayMember = "StorageName";

                SetInfo();
                SetControl();
            }
            catch (Exception ex)
            {
                MessageDialog.ShowErrorMessage(ex.Message);
            }
        }

        void SetControl()
        {
            //foreach (Control cl in customPanel1.Controls)
            //{
            //    if (cl is GroupBox)
            //    {
            //        if (m_lnqFlowInfo != null)
            //        {
            //            if (((GroupBox)cl).Tag.ToString().Split(',').Contains(m_lnqFlowInfo.FlowID.ToString()))
            //            {
            //                ((GroupBox)cl).Enabled = true;
            //                continue;
            //            }

            //            ((GroupBox)cl).Enabled = false;
            //        }
            //    }
            //}
        }

        void SetInfo()
        {
            if (m_lnqBillInfo != null)
            {
                lbBillStatus.Text = m_serverFlow.GetNowBillStatus(m_lnqBillInfo.BillNo);

                m_lnqFlowInfo =
                    m_serverFlow.GetNowFlowInfo(m_serverFlow.GetBusinessTypeID(CE_BillTypeEnum.不合格品隔离处置单, null), 
                    m_lnqBillInfo.BillNo);

                txtBillNo.Text = m_lnqBillInfo.BillNo;

                View_F_GoodsPlanCost goodsInfo = UniversalFunction.GetGoodsInfo(m_lnqBillInfo.GoodsID);

                txtBatchNo.Text = m_lnqBillInfo.BatchNo;
                txtGoodsCode.Text = goodsInfo.图号型号;
                txtGoodsName.Text = goodsInfo.物品名称;
                txtSpec.Text = goodsInfo.规格;
                txtGoodsCode.Tag = m_lnqBillInfo.GoodsID;
                txtIsolationReason.Text = m_lnqBillInfo.IsolationReason;
                txtProcessMethodRequire.Text = m_lnqBillInfo.ProcessMethodRequire;
                txtProvider.Text = m_lnqBillInfo.Provider;

                cmbStorageID.Text = UniversalFunction.GetStorageName(m_lnqBillInfo.StorageID);

                numGoodsCount.Value = m_lnqBillInfo.GoodsCount;

                lbReportFile.Tag = m_lnqBillInfo.ReportFile;

                num_PH_DisqualifiendCount.Value = m_lnqBillInfo.PH_DisqualifiendCount == null ? 0 :
                    (decimal)m_lnqBillInfo.PH_DisqualifiendCount;
                num_PH_QualifiedCount.Value = m_lnqBillInfo.PH_QualifiedCount == null ? 0 :
                    (decimal)m_lnqBillInfo.PH_QualifiedCount;
                num_QC_ConcessionCount.Value = m_lnqBillInfo.QC_ConcessionCount == null ? 0 :
                    (decimal)m_lnqBillInfo.QC_ConcessionCount;
                num_QC_QualifiedCount.Value = m_lnqBillInfo.QC_QualifiedCount == null ? 0 :
                    (decimal)m_lnqBillInfo.QC_QualifiedCount;
                num_QC_DisqualifiedCount.Value = m_lnqBillInfo.QC_DisqualifiedCount == null ? 0 :
                    (decimal)m_lnqBillInfo.QC_DisqualifiedCount;
                num_QC_ScraptCount.Value = m_lnqBillInfo.QC_ScraptCount == null ? 0 :
                    (decimal)m_lnqBillInfo.QC_ScraptCount;
                num_WorkHours.Value = m_lnqBillInfo.WorkHours == null ? 0 :
                    (decimal)m_lnqBillInfo.WorkHours;

                if (m_lnqBillInfo.ReturnProcess != null)
                {
                    if (m_lnqBillInfo.ReturnProcess == rb_ReturnProcess_BF.Text)
                    {
                        rb_ReturnProcess_BF.Checked = true;
                    }
                    else if (m_lnqBillInfo.ReturnProcess == rb_ReturnProcess_TH.Text)
                    {
                        rb_ReturnProcess_TH.Checked = true;
                    }
                }

                this.customDataGridView1.DataSource = m_serviceIsolation.GetSupplementeMessageInfo(m_lnqBillInfo.BillNo);
            }
            else
            {
                lbBillStatus.Text = CE_CommonBillStatus.新建单据.ToString();

                m_lnqBillInfo = new Business_QualityManagement_Isolation();

                txtBillNo.Text = this.FlowInfo_BillNo;
                m_lnqBillInfo.BillNo = txtBillNo.Text;
            }
        }

        void GetInfo()
        {
            m_lnqBillInfo = new Business_QualityManagement_Isolation();

            m_lnqBillInfo.BatchNo = txtBatchNo.Text;
            m_lnqBillInfo.BillNo = txtBillNo.Text;
            m_lnqBillInfo.GoodsCount = numGoodsCount.Value;
            m_lnqBillInfo.GoodsID = Convert.ToInt32(txtGoodsCode.Tag);
            m_lnqBillInfo.IsolationReason = txtIsolationReason.Text;
            m_lnqBillInfo.PH_DisqualifiendCount = num_PH_DisqualifiendCount.Value;
            m_lnqBillInfo.PH_QualifiedCount = num_PH_QualifiedCount.Value;
            m_lnqBillInfo.ProcessMethodRequire = txtProcessMethodRequire.Text;
            m_lnqBillInfo.Provider = txtProvider.Text;
            m_lnqBillInfo.QC_DisqualifiedCount = num_QC_DisqualifiedCount.Value;
            m_lnqBillInfo.QC_ConcessionCount = num_QC_ConcessionCount.Value;
            m_lnqBillInfo.QC_QualifiedCount = num_QC_QualifiedCount.Value;
            m_lnqBillInfo.QC_ScraptCount = num_QC_ScraptCount.Value;
            m_lnqBillInfo.ReportFile = lbReportFile.Tag == null ? "" : lbReportFile.Tag.ToString();

            if (rb_ReturnProcess_BF.Checked)
            {
                m_lnqBillInfo.ReturnProcess = rb_ReturnProcess_BF.Text;
            }
            else if (rb_ReturnProcess_TH.Checked)
            {
                m_lnqBillInfo.ReturnProcess = rb_ReturnProcess_TH.Text;
            }

            m_lnqBillInfo.StorageID = UniversalFunction.GetStorageID(cmbStorageID.Text);
            m_lnqBillInfo.WorkHours = num_WorkHours.Value;
        }

        bool CheckData(CE_FlowOperationType flowOperationType)
        {
            if (txtGoodsCode.Tag == null || txtGoodsCode.Tag.ToString().Trim().Length == 0)
            {
                MessageDialog.ShowPromptMessage("请选择【物品】");
                return false;
            }

            if (cmbStorageID.Text.Trim().Length == 0)
            {
                MessageDialog.ShowPromptMessage("请选择【所属库房】");
                return false;
            }

            if (txtBatchNo.Text.Trim().Length == 0)
            {
                MessageDialog.ShowPromptMessage("请选择【批次号】");
                return false;
            }

            if (txtIsolationReason.Text.Trim().Length == 0)
            {
                MessageDialog.ShowPromptMessage("请填写【隔离原因及处理方法】");
                return false;
            }

            if (num_PH_DisqualifiendCount.Value != 0 && !rb_ReturnProcess_BF.Checked && !rb_ReturnProcess_TH.Checked)
            {
                MessageDialog.ShowPromptMessage("不合格数大于0时，请选择【退货方式】");
                return false;
            }

            if (numGoodsCount.Value == 0)
            {
                MessageDialog.ShowPromptMessage("无法隔离【隔离数】等于0的物品");
                return false;
            }

            if (m_lnqFlowInfo != null && flowOperationType == CE_FlowOperationType.提交)
            {
                switch (m_lnqFlowInfo.FlowID)
                {
                    case 58:

                        if (num_PH_DisqualifiendCount.Value + num_PH_QualifiedCount.Value != numGoodsCount.Value)
                        {
                            MessageDialog.ShowPromptMessage("需满足：【合格数】+【不合格数】=【隔离数】");
                            return false;
                        }

                        break;
                    case 59:

                        if (num_QC_ConcessionCount.Value +
                            num_QC_DisqualifiedCount.Value +
                            num_QC_QualifiedCount.Value +
                            num_QC_ScraptCount.Value != numGoodsCount.Value)
                        {
                            MessageDialog.ShowPromptMessage("需满足：【合格数】+【不合格数】+【让步数】+【检测报废数】=【隔离数】");
                            return false;
                        }

                        if (num_QC_DisqualifiedCount.Value > 0 || num_QC_QualifiedCount.Value > 0)
                        {
                            不合格品信息 form = new 不合格品信息(m_lnqBillInfo.BillNo);
                            form.ShowDialog();

                            if (!form.BlFlag)
                            {
                                MessageBox.Show("请完整填写不合格信息单，并且保存！", "提示");
                                return false;
                            }
                        }

                        break;
                    default:
                        break;
                }
            }

            return true;
        }

        private bool customForm_PanelGetDateInfo(CE_FlowOperationType flowOperationType)
        {
            try
            {
                if (!CheckData(flowOperationType))
                {
                    return false;
                }

                GetInfo();

                this.FlowInfo_BillNo = txtBillNo.Text;
                this.FlowInfo_StorageIDOrWorkShopCode = m_lnqBillInfo.StorageID;
                this.KeyWords = "【" + UniversalFunction.GetGoodsInfo(m_lnqBillInfo.GoodsID).物品名称 + "】【" + m_lnqBillInfo.BatchNo + "】";

                this.ResultList = new List<object>();

                this.ResultList.Add(m_lnqBillInfo);
                this.ResultList.Add(flowOperationType);

                return true;
            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
                return false;
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

        private void txtBatchNo_Enter(object sender, EventArgs e)
        {
            if (cmbStorageID.Text.Trim().Length > 0 && txtGoodsCode.Tag != null)
            {
                txtBatchNo.StrEndSql = " and 库房代码 = '" + UniversalFunction.GetStorageID(cmbStorageID.Text) 
                    +"' and 物品ID = " + Convert.ToInt32(txtGoodsCode.Tag) + " and 库存数量 > 0";
            }
        }

        private void txtBatchNo_OnCompleteSearch()
        {
            if (txtBatchNo.DataResult != null)
            {
                txtProvider.Text = txtBatchNo.DataResult["供应商编码"].ToString();
                numGoodsCount.Value = Convert.ToDecimal( txtBatchNo.DataResult["库存数量"]);
            }
            else
            {
                txtProvider.Text = "";
                numGoodsCount.Value = 0;
            }
        }

        private void btn_ReportFile_Up_Click(object sender, EventArgs e)
        {
            try
            {
                if (lbReportFile.Tag != null && lbReportFile.Tag.ToString().Length > 0)
                {
                    foreach (string fileItem in lbReportFile.Tag.ToString().Split(','))
                    {
                        UniversalControlLibrary.FileOperationService.File_Delete(new Guid(fileItem),
                                GlobalObject.GeneralFunction.StringConvertToEnum<CE_CommunicationMode>(BasicInfo.BaseSwitchInfo[(int)GlobalObject.CE_SwitchName.文件传输方式]));
                    }
                }

                string strFilePath = "";

                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    foreach (string filePath in openFileDialog1.FileNames)
                    {
                        Guid guid = Guid.NewGuid();
                        FileOperationService.File_UpLoad(guid, filePath,
                                GlobalObject.GeneralFunction.StringConvertToEnum<CE_CommunicationMode>(BasicInfo.BaseSwitchInfo[(int)GlobalObject.CE_SwitchName.文件传输方式]));
                        strFilePath += guid.ToString() + ",";
                    }

                    lbReportFile.Tag = strFilePath.Substring(0, strFilePath.Length - 1);
                    m_serviceIsolation.UpdateFilePath(txtBillNo.Text, lbReportFile.Tag.ToString());
                    MessageDialog.ShowPromptMessage("上传成功");
                }
            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
            }
        }

        private void btn_ReportFile_Down_Click(object sender, EventArgs e)
        {
            if (lbReportFile.Tag == null || lbReportFile.Tag.ToString().Length == 0)
            {
                MessageDialog.ShowPromptMessage("无附件下载");
                return;
            }

            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                string[] tempArray = lbReportFile.Tag.ToString().Split(',');

                for (int i = 0; i < tempArray.Length; i++)
                {
                    FileOperationService.File_DownLoad(new Guid(tempArray[i]),
                        folderBrowserDialog1.SelectedPath + "\\" + txtBillNo.Text + "_" + i.ToString(),
                                GlobalObject.GeneralFunction.StringConvertToEnum<CE_CommunicationMode>(BasicInfo.BaseSwitchInfo[(int)GlobalObject.CE_SwitchName.文件传输方式]));
                }

                MessageDialog.ShowPromptMessage("下载成功");
            }
        }

        private void txtGoodsCode_Enter(object sender, EventArgs e)
        {
            if (cmbStorageID.Text.Trim().Length > 0 )
            {
                txtGoodsCode.StrEndSql = " and 库房代码 = '" + UniversalFunction.GetStorageID(cmbStorageID.Text) + "' and 库存数量 > 0";
            }
        }
    }
}

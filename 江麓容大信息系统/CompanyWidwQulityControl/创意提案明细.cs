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
using Service_Peripheral_CompanyQuality;
using FlowControlService;

namespace Form_Peripheral_CompanyQuality
{
    public partial class 创意提案明细 : CustomFlowForm
    {
        /// <summary>
        /// 单据编号控制类
        /// </summary>
        BillNumberControl m_billNoControl;

        bool isDirectAdd = false;

        public bool IsDirectAdd
        {
            get { return isDirectAdd; }
            set { isDirectAdd = value; }
        }

        /// <summary>
        /// 单据号
        /// </summary>
        private Business_CWQC_CreativePersentation m_lnqBillInfo = new Business_CWQC_CreativePersentation();

        public Business_CWQC_CreativePersentation LnqBillInfo
        {
            get { return m_lnqBillInfo; }
            set { m_lnqBillInfo = value; }
        }

        /// <summary>
        /// 服务组件
        /// </summary>
        ICreativeePersentation m_mainService = Service_Peripheral_CompanyQuality.ServerModuleFactory.GetServerModule<ICreativeePersentation>();


        public 创意提案明细()
        {
            InitializeComponent();
        }

        public override void LoadFormInfo()
        {
            try
            {
                m_billNoControl = new BillNumberControl(CE_BillTypeEnum.创意提案.ToString(), m_mainService);
                m_lnqBillInfo = m_mainService.GetSingleBillInfo(this.FlowInfo_BillNo);
                SetInfo();
            }
            catch (Exception ex)
            {
                MessageDialog.ShowErrorMessage(ex.Message);
            }
        }

        void numFZRValueChanged(object sender, EventArgs e)
        {
            FZRSetInfo();
        }

        void numFGLDValueChanged(object sender, EventArgs e)
        {
            FGLDSetInfo();
        }

        void SetInfo()
        {
            IFlowServer serverFlow = FlowControlService.ServerModuleFactory.GetServerModule<IFlowServer>();

            dataGridView1.DataSource = m_mainService.GetReferenceInfo(dataGridView1.Tag.ToString());
            dataGridView2.DataSource = m_mainService.GetReferenceInfo(dataGridView2.Tag.ToString());
            dataGridView3.DataSource = m_mainService.GetReferenceInfo(dataGridView3.Tag.ToString());

            if (m_lnqBillInfo != null)
            {
                lbBillStatus.Text = serverFlow.GetNowBillStatus(m_lnqBillInfo.BillNo);
                txtBillNo.Text = m_lnqBillInfo.BillNo;

                Flow_FlowInfo flowInfo = serverFlow.GetNowFlowInfo(serverFlow.GetBusinessTypeID(CE_BillTypeEnum.创意提案, null),
                    m_lnqBillInfo.BillNo);
                Flow_FlowData flowData = serverFlow.GetBusinessOperationInfo(txtBillNo.Text, CE_CommonBillStatus.新建单据.ToString())[0];
                View_HR_Personnel sqrInfo = UniversalFunction.GetPersonnelInfo(m_lnqBillInfo.Propose);

                txtDept.Text = sqrInfo.部门名称;
                txtPropose.Text = sqrInfo.姓名;
                txtPropose.Tag = sqrInfo.工号;

                txtImproveConditions_After.Text = m_lnqBillInfo.ImproveConditions_After;
                txtImproveConditions_Before.Text = m_lnqBillInfo.ImproveConditions_Before;
                txtTask.Text = m_lnqBillInfo.Task;
                dtpImproveEndDate.Value = m_lnqBillInfo.ImproveEndDate;
                dtpImproveStartDate.Value = m_lnqBillInfo.ImproveStartDate;
                cmbExtensionCoverage.Text = m_lnqBillInfo.ExtensionCoverage;

                txtValueEffect.Text = m_lnqBillInfo.ValueEffect.ToString();

                numFGLD_Abstract.Value = m_lnqBillInfo.FGLD_Abstract == null ? 0 : (int)m_lnqBillInfo.FGLD_Abstract;
                numFGLD_Apply.Value = m_lnqBillInfo.FGLD_Apply == null ? 0 : (int)m_lnqBillInfo.FGLD_Apply;
                numFGLD_Economy.Value = m_lnqBillInfo.FGLD_Economy == null ? 0 : (int)m_lnqBillInfo.FGLD_Economy;
                numFGLD_Ideas.Value = m_lnqBillInfo.FGLD_Ideas == null ? 0 : (int)m_lnqBillInfo.FGLD_Ideas;
                numFGLD_Strive.Value = m_lnqBillInfo.FGLD_Strive == null ? 0 : (int)m_lnqBillInfo.FGLD_Strive;

                numFZR_Abstract.Value = m_lnqBillInfo.FZR_Abstract == null ? 0 : (int)m_lnqBillInfo.FZR_Abstract;
                numFZR_Apply.Value = m_lnqBillInfo.FZR_Apply == null ? 0 : (int)m_lnqBillInfo.FZR_Apply;
                numFZR_Economy.Value = m_lnqBillInfo.FZR_Economy == null ? 0 : (int)m_lnqBillInfo.FZR_Economy;
                numFZR_Ideas.Value = m_lnqBillInfo.FZR_Ideas == null ? 0 : (int)m_lnqBillInfo.FZR_Ideas;
                numFZR_Strive.Value = m_lnqBillInfo.FZR_Strive == null ? 0 : (int)m_lnqBillInfo.FZR_Strive;

                lbImproveConditions_After_FileNo_Look.Tag = m_lnqBillInfo.ImproveConditions_After_FileNo;
                lbImproveConditions_Before_FileNo_Look.Tag = m_lnqBillInfo.ImproveConditions_Before_FileNo;

                List<string> lstCheck = m_lnqBillInfo.ProposalType.Split(',').ToList();

                foreach (Control cl in customGroupBox1.Controls)
                {
                    if (cl is CheckBox)
                    {
                        CheckBox cb = cl as CheckBox;

                        if (lstCheck.Contains(cb.Text))
                        {
                            cb.Checked = true;
                        }
                    }
                }

                foreach (Control cl in tabPage1.Controls)
                {
                    if (cl is CustomGroupBox)
                    {
                        CustomGroupBox gb = cl as CustomGroupBox;

                        if (gb.Tag != null && gb.Tag.ToString().Trim().Length > 0)
                        {
                            if (flowInfo.FlowID == Convert.ToInt32( gb.Tag))
                            {
                                gb.Enabled = true;
                            }
                            else
                            {
                                gb.Enabled = false;
                            }
                        }
                    }
                }

                if (lbBillStatus.Text != CE_CommonBillStatus.新建单据.ToString())
                {
                    txtImproveConditions_After.ReadOnly = true;
                    txtImproveConditions_Before.ReadOnly = true;
                }
            }
            else
            {
                lbBillStatus.Text = CE_CommonBillStatus.新建单据.ToString();

                m_lnqBillInfo = new Business_CWQC_CreativePersentation();

                txtBillNo.Text = this.FlowInfo_BillNo;
                m_lnqBillInfo.BillNo = txtBillNo.Text;
                View_HR_Personnel sqrInfo = UniversalFunction.GetPersonnelInfo(BasicInfo.LoginID);

                txtDept.Text = sqrInfo.部门名称;
                txtPropose.Text = sqrInfo.姓名;
                txtPropose.Tag = sqrInfo.工号;

                if (!isDirectAdd)
                {
                    customGroupBox6.Enabled = false;
                    customGroupBox7.Enabled = false;
                }
            }

            if (lbBillStatus.Text == CE_CommonBillStatus.新建单据.ToString())
            {
                lbImproveConditions_After_FileNo_Up.Visible = true;
                lbImproveConditions_Before_FileNo_Up.Visible = true;
            }
            else
            {
                lbImproveConditions_After_FileNo_Up.Visible = false;
                lbImproveConditions_Before_FileNo_Up.Visible = false;
            }

            if (lbImproveConditions_After_FileNo_Look.Tag != null && lbImproveConditions_After_FileNo_Look.Tag.ToString().Trim().Length > 0)
            {
                lbImproveConditions_After_FileNo_Look.Visible = true;
            }
            else
            {
                lbImproveConditions_After_FileNo_Look.Visible = false;
            }

            if (lbImproveConditions_Before_FileNo_Look.Tag != null && lbImproveConditions_Before_FileNo_Look.Tag.ToString().Trim().Length > 0)
            {
                lbImproveConditions_Before_FileNo_Look.Visible = true;
            }
            else
            {
                lbImproveConditions_Before_FileNo_Look.Visible = false;
            }

            FZRSetInfo();
            FGLDSetInfo();
            SetInfo_EffectValue();
        }

        void SetInfo_EffectValue()
        {
            Business_CWQC_CreativePersentation_EffectValue effectValue = m_mainService.GetInfo_EffectValue(txtBillNo.Text);

            if (effectValue == null)
            {
                return;
            }
            else
            {
                numWorkReduce1.Value = effectValue.WorkReduce1;
                numWorkReduce2.Value = effectValue.WorkReduce2;
                numWorkReduce3.Value = effectValue.WorkReduce3;
                numWorkReduce4.Value = effectValue.WorkReduce4;

                numMaterialReduce1.Value = effectValue.MaterialReduce1;
                numMaterialReduce2.Value = effectValue.MaterialReduce2;
                numMaterialReduce3.Value = effectValue.MaterialReduce3;
                numMaterialReduce4.Value = effectValue.MaterialReduce4;

                numElseEffectValue.Value = effectValue.ElseEffectValue;

                txtElseContent.Text = effectValue.ElseContent;
            }
        }

        Business_CWQC_CreativePersentation_EffectValue GetInfo_EffectValue()
        {
            Business_CWQC_CreativePersentation_EffectValue tempLnq = new Business_CWQC_CreativePersentation_EffectValue();

            tempLnq.BillNo = txtBillNo.Text;

            tempLnq.ElseContent = txtElseContent.Text;
            tempLnq.ElseEffectValue = numElseEffectValue.Value;

            tempLnq.MaterialReduce1 = numMaterialReduce1.Value;
            tempLnq.MaterialReduce2 = numMaterialReduce2.Value;
            tempLnq.MaterialReduce3 = numMaterialReduce3.Value;
            tempLnq.MaterialReduce4 = numMaterialReduce4.Value;

            tempLnq.WorkReduce1 = numWorkReduce1.Value;
            tempLnq.WorkReduce2 = numWorkReduce2.Value;
            tempLnq.WorkReduce3 = numWorkReduce3.Value;
            tempLnq.WorkReduce4 = numWorkReduce4.Value;

            return tempLnq;
        }

        private void numWorkReduce_ValueChanged(object sender, EventArgs e)
        {
            numWorkReduceResult.Value = 
                numWorkReduce1.Value / 3600 * numWorkReduce2.Value * numWorkReduce3.Value - numWorkReduce4.Value;
        }

        private void numMaterialReduce_ValueChanged(object sender, EventArgs e)
        {
            numMaterialReduceResult.Value = 
                numMaterialReduce1.Value * numMaterialReduce2.Value * numMaterialReduce3.Value - numMaterialReduce4.Value;
        }

        void SetNumMax(CustomGroupBox groupBox)
        {
            DataTable tempTable = new DataTable();

            foreach (Control cl in groupBox.Controls)
            {
                if (cl is NumericUpDown)
                {
                    NumericUpDown numTemp = cl as NumericUpDown;

                    if (numTemp.Tag != null)
                    {
                        if (numTemp.Tag.ToString() == "经济效果")
                        {
                            tempTable = m_mainService.GetReferenceInfo(dataGridView1.Tag.ToString());
                            tempTable = DataSetHelper.OrderBy(tempTable, "Score");
                            numTemp.Maximum = Convert.ToInt32(tempTable.Rows[tempTable.Rows.Count - 1]["Score"]);
                        }
                        else
                        {
                            tempTable = m_mainService.GetReferenceInfo(dataGridView2.Tag.ToString());
                            tempTable = DataSetHelper.OrderBy(DataSetHelper.Where(tempTable, "ItemName = '" + numTemp.Tag.ToString() + "'"), "Score");
                            numTemp.Maximum = Convert.ToInt32(tempTable.Rows[tempTable.Rows.Count - 1]["Score"]);
                        }
                    }
                }
            }
        }

        void FGLDSetInfo()
        {
            int valueSum = (int)(numFGLD_Abstract.Value + numFGLD_Apply.Value + numFGLD_Economy.Value
                + numFGLD_Ideas.Value + numFGLD_Strive.Value);
            lbFGLD_Sum.Text = valueSum.ToString();

            if (valueSum == 0)
            {
                lbFGLD_Level.Text = "无";
                lbFGLD_Bonus.Text = valueSum.ToString();
            }
            else
            {
                DataTable dtBonus = m_mainService.GetReferenceInfo(dataGridView3.Tag.ToString());

                foreach (DataRow dr in dtBonus.Rows)
                {
                    int intMin = Convert.ToInt32(dr["Score"].ToString().Split('~').ToList()[0]);
                    int intMax = Convert.ToInt32(dr["Score"].ToString().Split('~').ToList()[1]);

                    if (valueSum >= intMin && valueSum <= intMax)
                    {
                        lbFGLD_Bonus.Text = dr["AwardAmount"].ToString();
                        lbFGLD_Level.Text = dr["Level"].ToString();
                        break;
                    }
                }
            }

            SetNumMax(customGroupBox7);
        }

        void FZRSetInfo()
        {
            int valueSum = (int)(numFZR_Abstract.Value + numFZR_Apply.Value + numFZR_Economy.Value
                + numFZR_Ideas.Value + numFZR_Strive.Value);
            lbFZR_Sum.Text = valueSum.ToString();

            if (valueSum == 0)
            {
                lbFZR_Level.Text = "无";
                lbFZR_Bonus.Text = valueSum.ToString();
            }
            else
            {
                DataTable dtBonus = m_mainService.GetReferenceInfo(dataGridView3.Tag.ToString());

                foreach (DataRow dr in dtBonus.Rows)
                {
                    int intMin = Convert.ToInt32(dr["Score"].ToString().Split('~').ToList()[0]);
                    int intMax = Convert.ToInt32(dr["Score"].ToString().Split('~').ToList()[1]);

                    if (valueSum >= intMin && valueSum <= intMax)
                    {
                        lbFZR_Bonus.Text = dr["AwardAmount"].ToString();
                        lbFZR_Level.Text = dr["Level"].ToString();
                        break;
                    }
                }
            }

            SetNumMax(customGroupBox6);
        }

        bool CheckData()
        {
            if (txtPropose.Text.Trim().Length == 0 || txtPropose.Tag == null || txtPropose.Tag.ToString().Length == 0)
            {
                MessageDialog.ShowPromptMessage("请选择【提案人】");
                return false;
            }

            if (txtTask.Text.Trim().Length == 0)
            {
                MessageDialog.ShowPromptMessage("请填写【课题】名称");
                return false;
            }

            bool checkFlag = false;

            foreach (Control cl in customGroupBox1.Controls)
            {
                if (cl is CheckBox)
                {
                    CheckBox cb = cl as CheckBox;

                    if (cb.Checked)
                    {
                        checkFlag = true;
                    }
                }
            }

            if (!checkFlag)
            {
                MessageDialog.ShowPromptMessage("请选择【提案类别】");
                return false;
            }

            if (cmbExtensionCoverage.Text.Trim().Length == 0)
            {
                MessageDialog.ShowPromptMessage("请选择【推广范围】");
                return false;
            }

            if (txtValueEffect.Text.Trim().Length == 0)
            {
                MessageDialog.ShowPromptMessage("请填写【估算合计效果】");
                return false;
            }
            else
            {
                decimal resultDec = 0;
                if (!Decimal.TryParse(txtValueEffect.Text, out resultDec))
                {
                    MessageDialog.ShowPromptMessage("请以【数值形式】填写【估算合计效果】");
                    return false;
                }
            }

            if (txtImproveConditions_Before.Text.Trim().Length == 0)
            {
                MessageDialog.ShowPromptMessage("请填写【改善前状况】");
                return false;
            }

            if (txtImproveConditions_After.Text.Trim().Length == 0)
            {
                MessageDialog.ShowPromptMessage("请填写【改善后状况】");
                return false;
            }

            return true;
        }

        void FileUp(LinkLabel lbLink)
        {
            try
            {
                string strFilePath = "";

                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    if (lbLink.Tag != null && lbLink.ToString().Length > 0)
                    {
                        foreach (string fileItem in lbLink.Tag.ToString().Split(','))
                        {
                            UniversalControlLibrary.FileOperationService.File_Delete(new Guid(fileItem),
                                GlobalObject.GeneralFunction.StringConvertToEnum<CE_CommunicationMode>(BasicInfo.BaseSwitchInfo[(int)GlobalObject.CE_SwitchName.文件传输方式]));
                        }
                    }

                    foreach (string filePath in openFileDialog1.FileNames)
                    {
                        Guid guid = Guid.NewGuid();
                        FileOperationService.File_UpLoad(guid, filePath,
                                GlobalObject.GeneralFunction.StringConvertToEnum<CE_CommunicationMode>(BasicInfo.BaseSwitchInfo[(int)GlobalObject.CE_SwitchName.文件传输方式]));
                        strFilePath += guid.ToString() + ",";
                    }

                    lbLink.Tag = strFilePath.Substring(0, strFilePath.Length - 1);
                    m_mainService.UpdateFilePath(txtBillNo.Text, lbLink.Tag.ToString(),
                        lbLink.Name.Contains(SelfSimpleEnum_CreativeePersentation.Before.ToString()) ?
                        SelfSimpleEnum_CreativeePersentation.Before : SelfSimpleEnum_CreativeePersentation.After);
                    MessageDialog.ShowPromptMessage("上传成功");
                    lbLink.Visible = true;
                }
            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
            }
        }

        void FileLook(LinkLabel lbLink)
        {
            if (lbLink.Tag == null || lbLink.Tag.ToString().Length == 0)
            {
                MessageDialog.ShowPromptMessage("无附件查看");
                return;
            }

            string[] tempArray = lbLink.Tag.ToString().Split(',');

            for (int i = 0; i < tempArray.Length; i++)
            {
                FileOperationService.File_Look(new Guid(tempArray[i]),
                        GlobalObject.GeneralFunction.StringConvertToEnum<CE_CommunicationMode>(BasicInfo.BaseSwitchInfo[(int)GlobalObject.CE_SwitchName.文件传输方式]));
            }
        }

        private bool 创意提案明细_PanelGetDataInfo(CE_FlowOperationType flowOperationType)
        {
            try
            {
                if (lbBillStatus.Text == CE_CommonBillStatus.新建单据.ToString() && !CheckData())
                {
                    return false;
                }

                m_lnqBillInfo = new Business_CWQC_CreativePersentation();

                m_lnqBillInfo.Propose = txtPropose.Tag.ToString();
                m_lnqBillInfo.BillNo = txtBillNo.Text;
                m_lnqBillInfo.ExtensionCoverage = cmbExtensionCoverage.Text;

                m_lnqBillInfo.FGLD_Abstract = (int)numFGLD_Abstract.Value;
                m_lnqBillInfo.FGLD_Apply = (int)numFGLD_Apply.Value;
                m_lnqBillInfo.FGLD_Economy = (int)numFGLD_Economy.Value;
                m_lnqBillInfo.FGLD_Ideas = (int)numFGLD_Ideas.Value;
                m_lnqBillInfo.FGLD_Strive = (int)numFGLD_Strive.Value;

                m_lnqBillInfo.FZR_Abstract = (int)numFZR_Abstract.Value;
                m_lnqBillInfo.FZR_Apply = (int)numFZR_Apply.Value;
                m_lnqBillInfo.FZR_Economy = (int)numFZR_Economy.Value;
                m_lnqBillInfo.FZR_Ideas = (int)numFZR_Ideas.Value;
                m_lnqBillInfo.FZR_Strive = (int)numFZR_Strive.Value;

                m_lnqBillInfo.ImproveConditions_After = txtImproveConditions_After.Text;
                m_lnqBillInfo.ImproveConditions_Before = txtImproveConditions_Before.Text;

                m_lnqBillInfo.ImproveConditions_After_FileNo = lbImproveConditions_After_FileNo_Look.Tag == null ? null :
                    lbImproveConditions_After_FileNo_Look.Tag.ToString();

                m_lnqBillInfo.ImproveConditions_Before_FileNo = lbImproveConditions_Before_FileNo_Look.Tag == null ? null :
                    lbImproveConditions_Before_FileNo_Look.Tag.ToString();

                m_lnqBillInfo.ImproveEndDate = dtpImproveEndDate.Value;
                m_lnqBillInfo.ImproveStartDate = dtpImproveEndDate.Value;

                foreach (Control cl in customGroupBox1.Controls)
                {
                    if (cl is CheckBox)
                    {
                        CheckBox cb = cl as CheckBox;

                        if (cb.Checked)
                        {
                            m_lnqBillInfo.ProposalType += cb.Text + ",";
                        }
                    }
                }

                m_lnqBillInfo.ProposalType = m_lnqBillInfo.ProposalType.Substring(0, m_lnqBillInfo.ProposalType.Length - 1);
                m_lnqBillInfo.Task = txtTask.Text;
                m_lnqBillInfo.ValueEffect = Convert.ToDecimal(txtValueEffect.Text);


                if (lbFGLD_Level.Text != "无")
                {
                    m_lnqBillInfo.SumScore = Convert.ToInt32(lbFGLD_Sum.Text);
                    m_lnqBillInfo.Level = lbFGLD_Level.Text;
                    m_lnqBillInfo.Bonus = Convert.ToInt32(lbFGLD_Bonus.Text);
                }
                else
                {
                    m_lnqBillInfo.SumScore = Convert.ToInt32(lbFZR_Sum.Text);
                    m_lnqBillInfo.Level = lbFZR_Level.Text;
                    m_lnqBillInfo.Bonus = Convert.ToInt32(lbFZR_Bonus.Text);
                }

                if (lbBillStatus.Text != CE_CommonBillStatus.新建单据.ToString() && m_lnqBillInfo.SumScore == 0)
                {
                    if (MessageDialog.ShowEnquiryMessage("【合计评价分】为【0分】，是否继续提交？") == DialogResult.No)
                    {
                        return false;
                    }
                }

                this.FlowInfo_BillNo = m_lnqBillInfo.BillNo;

                this.ResultList = new List<object>();
                this.ResultList.Add(m_lnqBillInfo);
                this.ResultList.Add(flowOperationType);
                this.ResultList.Add(GetInfo_EffectValue());

                return true;
            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
                return false;
            }
        }

        private void lbImproveConditions_Before_FileNo_Up_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            FileUp(lbImproveConditions_Before_FileNo_Look);
        }

        private void lbImproveConditions_After_FileNo_Up_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            FileUp(lbImproveConditions_After_FileNo_Look);
        }

        private void lbImproveConditions_Before_FileNo_Look_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            FileLook(lbImproveConditions_Before_FileNo_Look);
        }

        private void lbImproveConditions_After_FileNo_Look_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            FileLook(lbImproveConditions_After_FileNo_Look);
        }

        private void txtPropose_OnCompleteSearch()
        {
            if (txtPropose.DataResult != null)
            {
                txtPropose.Text = txtPropose.DataResult["员工姓名"].ToString();
                txtPropose.Tag = txtPropose.DataResult["员工编号"].ToString();
                txtDept.Text = txtPropose.DataResult["部门"].ToString();
            }
        }
    }
}

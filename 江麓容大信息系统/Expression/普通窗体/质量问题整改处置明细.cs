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
using System.Net;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Formatters;
using UniversalControlLibrary;

namespace Expression
{
    /// <summary>
    /// 质量问题整改处置明细界面
    /// </summary>
    public partial class 质量问题整改处置明细 : Form
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        string m_strErr;

        /// <summary>
        /// 数据集
        /// </summary>
        private ZL_QualityProblemRectificationDisposalBill m_lnqQualityProblem = new ZL_QualityProblemRectificationDisposalBill();

        public ZL_QualityProblemRectificationDisposalBill LnqQualityProblem
        {
            get { return m_lnqQualityProblem; }
            set { m_lnqQualityProblem = value; }
        }

        /// <summary>
        /// 质量问题整改处置服务组件
        /// </summary>
        IQualityProblemRectificationDisposalBill m_serverQualityProblem = 
            ServerModuleFactory.GetServerModule<IQualityProblemRectificationDisposalBill>();

        public 质量问题整改处置明细(string billID)
        {
            InitializeComponent();

            ClearMessage();

            if (billID == "")
            {
                lbBill_ID.Text = m_serverQualityProblem.GetBillID("质量整改处置单");
                lbBillStatus.Text = "新建单据";
            }
            else
            {
                m_lnqQualityProblem = m_serverQualityProblem.GetQualityProblemMessage(billID);
                SetMessage();
            }

            //if (lbBillStatus.Text != "等待分析判定")
            //{
            //    txtInterimMeasure.Enabled = false;
            //    chbInterimMeasureIsNeedTesting.Enabled = false;
            //    btnInterimMeasureFileUp.Enabled = false;

            //    txtAnalyseAndJudge.Enabled = false;
            //    chbAnalyseAndJudgeIsNeedAffirm.Enabled = false;
            //    btnAnalyseAndJudgeFileUp.Enabled = false;

            //    txtRelevantDepartment.Enabled = false;
            //    dtpRequiredTime.Enabled = false;
            //}

            //if (lbBillStatus.Text != "等待整改措施")
            //{
            //    txtCauseAnalysis.Enabled = false;
            //    chbCauseAnalysisIsNeedTesting.Enabled = false;
            //    btnCauseAnalysisFileUp.Enabled = false;

            //    txtRectificationMeasures.Enabled = false;
            //    chbRectificationMeasuresIsNeedChange.Enabled = false;
            //    btnRectificationMeasuresFileUp.Enabled = false;
            //}
        }

        /// <summary>
        /// 清空信息
        /// </summary>
        void ClearMessage()
        {
            lbAnalyseAndJudgeAffirmID.Text = "";
            lbCauseAnalysisTestingID.Text = "";
            lbEffectAffirmDate.Text = "";
            lbEffectAffirmPersonnel.Text = "";
            lbHappenAffirmPersonnel.Text = "";
            lbHappenFillInPersonnel.Text = "";
            lbInterimMeasureTestingID.Text = "";
            lbLaunchDepartmentAffirmDate.Text = "";
            lbLaunchDepartmentAffirmPersonnel.Text = "";
            lbQualityAffirmPersonnel.Text = "";
            lbQualityFillInPersonnel.Text = "";
            lbRectificationMeasuresChangeID.Text = "";
            lbRelevantAffirmPersonnel.Text = "";
            lbRelevantFillInPersonnel.Text = "";
            lbBill_ID.Text = "";
            lbBillStatus.Text = "";
        }

        /// <summary>
        /// 获得信息
        /// </summary>
        void GetMessage()
        {
            m_lnqQualityProblem.AnalyseAndJudge = txtAnalyseAndJudge.Text;
            m_lnqQualityProblem.AnalyseAndJudgeAffirmID = lbAnalyseAndJudgeAffirmID.Text;
            m_lnqQualityProblem.AnalyseAndJudgeIsNeedAffirm = chbAnalyseAndJudgeIsNeedAffirm.Checked;
            m_lnqQualityProblem.BatchNoOrSpec = txtBatchNoOrSpec.Text;
            m_lnqQualityProblem.Bill_ID = lbBill_ID.Text;
            m_lnqQualityProblem.BillStatus = lbBillStatus.Text;
            m_lnqQualityProblem.CauseAnalysis = txtCauseAnalysis.Text;
            m_lnqQualityProblem.CauseAnalysisIsNeedTesting = chbCauseAnalysisIsNeedTesting.Checked;
            m_lnqQualityProblem.CauseAnalysisTestingID = lbCauseAnalysisTestingID.Text;
            m_lnqQualityProblem.EffectAffirmDate = lbEffectAffirmDate.Text == "" ? ServerTime.Time : Convert.ToDateTime(lbEffectAffirmDate.Text);
            m_lnqQualityProblem.EffectAffirmPersonnel = lbEffectAffirmPersonnel.Text;
            m_lnqQualityProblem.EffectConfirmed = txtEffectConfirmed.Text;
            m_lnqQualityProblem.GoodsCode = txtGoodsCode.Text;
            m_lnqQualityProblem.GoodsName = txtGoodsName.Text;
            m_lnqQualityProblem.HappenAffirmDate = ServerTime.Time;
            m_lnqQualityProblem.HappenAffirmPersonnel = lbHappenAffirmPersonnel.Text;
            m_lnqQualityProblem.HappenDate = ServerTime.Time;
            m_lnqQualityProblem.HappenFillInDate = dtpHappenDate.Value;
            m_lnqQualityProblem.HappenFillInPersonnel = lbHappenFillInPersonnel.Text;
            m_lnqQualityProblem.HappenFrequency = cmbHappenFrequency.Text;
            m_lnqQualityProblem.HappenPlace = cmbHappenPlace.Text;
            m_lnqQualityProblem.InterimMeasure = txtInterimMeasure.Text;
            m_lnqQualityProblem.InterimMeasureIsNeedTesting = chbInterimMeasureIsNeedTesting.Checked;
            m_lnqQualityProblem.InterimMeasureTestingID = lbInterimMeasureTestingID.Text;
            m_lnqQualityProblem.LaunchDepartmentAffirm = txtLaunchDepartmentAffirm.Text;
            m_lnqQualityProblem.LaunchDepartmentAffirmDate = lbEffectAffirmDate.Text == "" ? ServerTime.Time : Convert.ToDateTime(lbEffectAffirmDate.Text);
            m_lnqQualityProblem.LaunchDepartmentAffirmPersonnel = lbLaunchDepartmentAffirmPersonnel.Text;
            m_lnqQualityProblem.ProblemsDescription = txtProblemsDescription.Text;
            m_lnqQualityProblem.Provider = txtProvider.Text;
            m_lnqQualityProblem.QualityAffirmDate = ServerTime.Time;
            m_lnqQualityProblem.QualityAffirmPersonnel = lbQualityAffirmPersonnel.Text;
            m_lnqQualityProblem.QualityFillInDate = ServerTime.Time;
            m_lnqQualityProblem.QualityFillInPersonnel = lbQualityFillInPersonnel.Text;
            m_lnqQualityProblem.RectificationMeasures = txtRectificationMeasures.Text;
            m_lnqQualityProblem.RectificationMeasuresChangeID = lbRectificationMeasuresChangeID.Text;
            m_lnqQualityProblem.RectificationMeasuresIsNeedChange = chbRectificationMeasuresIsNeedChange.Checked;
            m_lnqQualityProblem.RelevantAffirmDate = ServerTime.Time;
            m_lnqQualityProblem.RelevantAffirmPersonnel = lbRelevantAffirmPersonnel.Text;
            m_lnqQualityProblem.RelevantDepartment = txtRelevantDepartment.Text;
            m_lnqQualityProblem.RelevantFillInDate = ServerTime.Time;
            m_lnqQualityProblem.RelevantFillInPersonnel = lbRelevantFillInPersonnel.Text;
            m_lnqQualityProblem.RequiredTime = dtpRequiredTime.Value;
            m_lnqQualityProblem.Severity = cmbSeverity.Text;
            m_lnqQualityProblem.Responsible = txtResponsible.Text;
        }

        /// <summary>
        /// 获得信息
        /// </summary>
        void SetMessage()
        {
            cmbHappenFrequency.Text = m_lnqQualityProblem.HappenFrequency;
            cmbHappenPlace.Text = m_lnqQualityProblem.HappenPlace;
            cmbSeverity.Text = m_lnqQualityProblem.Severity;

            txtResponsible.Text = m_lnqQualityProblem.Responsible;
            txtAnalyseAndJudge.Text = m_lnqQualityProblem.AnalyseAndJudge;
            txtBatchNoOrSpec.Text = m_lnqQualityProblem.BatchNoOrSpec;
            txtCauseAnalysis.Text = m_lnqQualityProblem.CauseAnalysis;
            txtEffectConfirmed.Text = m_lnqQualityProblem.EffectConfirmed;
            txtGoodsCode.Text = m_lnqQualityProblem.GoodsCode;
            txtGoodsName.Text = m_lnqQualityProblem.GoodsName;
            txtInterimMeasure.Text = m_lnqQualityProblem.InterimMeasure;
            txtLaunchDepartmentAffirm.Text = m_lnqQualityProblem.LaunchDepartmentAffirm;
            txtProblemsDescription.Text = m_lnqQualityProblem.ProblemsDescription;
            txtProvider.Text = m_lnqQualityProblem.Provider;
            txtRectificationMeasures.Text = m_lnqQualityProblem.RectificationMeasures;
            txtRelevantDepartment.Text = m_lnqQualityProblem.RelevantDepartment;

            dtpHappenDate.Value = m_lnqQualityProblem.HappenDate;
            dtpRequiredTime.Value = m_lnqQualityProblem.RequiredTime == null? ServerTime.Time : Convert.ToDateTime( m_lnqQualityProblem.RequiredTime);
            
            lbAnalyseAndJudgeAffirmID.Text = m_lnqQualityProblem.AnalyseAndJudgeAffirmID;
            lbBill_ID.Text = m_lnqQualityProblem.Bill_ID;
            lbBillStatus.Text = m_lnqQualityProblem.BillStatus;
            lbCauseAnalysisTestingID.Text = m_lnqQualityProblem.CauseAnalysisTestingID;
            lbEffectAffirmDate.Text = m_lnqQualityProblem.EffectAffirmDate == null ? ServerTime.Time.ToString() : m_lnqQualityProblem.EffectAffirmDate.ToString();
            lbEffectAffirmPersonnel.Text = m_lnqQualityProblem.EffectAffirmPersonnel;
            lbHappenAffirmPersonnel.Text = m_lnqQualityProblem.HappenAffirmPersonnel;
            lbHappenFillInPersonnel.Text = m_lnqQualityProblem.HappenFillInPersonnel;
            lbInterimMeasureTestingID.Text = m_lnqQualityProblem.InterimMeasureTestingID;
            lbLaunchDepartmentAffirmDate.Text = m_lnqQualityProblem.LaunchDepartmentAffirmDate == null?ServerTime.Time.ToString() : m_lnqQualityProblem.LaunchDepartmentAffirmDate.ToString();
            lbLaunchDepartmentAffirmPersonnel.Text = m_lnqQualityProblem.LaunchDepartmentAffirmPersonnel;
            lbQualityAffirmPersonnel.Text = m_lnqQualityProblem.QualityAffirmPersonnel;
            lbQualityFillInPersonnel.Text = m_lnqQualityProblem.QualityFillInPersonnel;
            lbRectificationMeasuresChangeID.Text = m_lnqQualityProblem.RectificationMeasuresChangeID;
            lbRelevantAffirmPersonnel.Text = m_lnqQualityProblem.RelevantAffirmPersonnel;
            lbRelevantFillInPersonnel.Text = m_lnqQualityProblem.RelevantFillInPersonnel;

            chbAnalyseAndJudgeIsNeedAffirm.Checked = m_lnqQualityProblem.AnalyseAndJudgeIsNeedAffirm == null ? false : Convert.ToBoolean( m_lnqQualityProblem.AnalyseAndJudgeIsNeedAffirm);
            chbCauseAnalysisIsNeedTesting.Checked = m_lnqQualityProblem.CauseAnalysisIsNeedTesting == null ? false :  Convert.ToBoolean(m_lnqQualityProblem.CauseAnalysisIsNeedTesting);
            chbInterimMeasureIsNeedTesting.Checked = m_lnqQualityProblem.InterimMeasureIsNeedTesting == null ? false :  Convert.ToBoolean(m_lnqQualityProblem.InterimMeasureIsNeedTesting);
            chbRectificationMeasuresIsNeedChange.Checked = m_lnqQualityProblem.RectificationMeasuresIsNeedChange == null ? false :  Convert.ToBoolean(m_lnqQualityProblem.RectificationMeasuresIsNeedChange);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            GetMessage();

            if (!m_serverQualityProblem.SaveInfo(m_lnqQualityProblem, out m_strErr))
            {
                MessageDialog.ShowPromptMessage(m_strErr);
                return;
            }
            else
            {
                MessageDialog.ShowPromptMessage("保存成功!");
                this.Close();
            }
        }

        private void btnInterimMeasureFileUp_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofdPic = new OpenFileDialog();
            ofdPic.Filter = "pdf files (*.pdf)|*.pdf|doc files (*.doc)|*.doc|All files (*.*)|*.*";

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
                    m_lnqQualityProblem.InterimMeasureFile = br.ReadBytes((int)file.Length);
                    file.Close();
                }
            }
        }

        private void btnInterimMeasureFileDown_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "pdf files (*.pdf)|*.pdf|doc files (*.doc)|*.doc|All files (*.*)|*.*";

            if (m_lnqQualityProblem.InterimMeasureFile == null)
            {
                MessageDialog.ShowPromptMessage("未添加此附件");
                return;
            }

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (m_lnqQualityProblem.InterimMeasureFile != null || m_lnqQualityProblem.InterimMeasureFile.ToString() != "")
                {

                    FileStream fs = new FileStream(saveFileDialog1.FileName, FileMode.Create, FileAccess.Write);
                    BinaryWriter bw = new BinaryWriter(fs);
                    bw.Write(m_lnqQualityProblem.InterimMeasureFile.ToArray());
                    bw.Close();
                    fs.Close();
                }
            }
        }

        private void btnAnalyseAndJudgeFileUp_Click(object sender, EventArgs e)
        {

            OpenFileDialog ofdPic = new OpenFileDialog();
            ofdPic.Filter = "pdf files (*.pdf)|*.pdf|doc files (*.doc)|*.doc|All files (*.*)|*.*";

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
                    m_lnqQualityProblem.AnalyseAndJudgeFile = br.ReadBytes((int)file.Length);
                    file.Close();
                }
            }
        }

        private void btnCauseAnalysisFileUp_Click(object sender, EventArgs e)
        {

            OpenFileDialog ofdPic = new OpenFileDialog();
            ofdPic.Filter = "pdf files (*.pdf)|*.pdf|doc files (*.doc)|*.doc|All files (*.*)|*.*";

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
                    m_lnqQualityProblem.CauseAnalysisFile = br.ReadBytes((int)file.Length);
                    file.Close();
                }
            }
        }

        private void btnRectificationMeasuresFileUp_Click(object sender, EventArgs e)
        {

            OpenFileDialog ofdPic = new OpenFileDialog();
            ofdPic.Filter = "pdf files (*.pdf)|*.pdf|doc files (*.doc)|*.doc|All files (*.*)|*.*";

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
                    m_lnqQualityProblem.RectificationMeasuresFile = br.ReadBytes((int)file.Length);
                    file.Close();
                }
            }
        }

        private void btnAnalyseAndJudgeFileDown_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "pdf files (*.pdf)|*.pdf|doc files (*.doc)|*.doc|All files (*.*)|*.*";

            if (m_lnqQualityProblem.AnalyseAndJudgeFile == null)
            {
                MessageDialog.ShowPromptMessage("未添加此附件");
                return;
            }

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (m_lnqQualityProblem.AnalyseAndJudgeFile != null || m_lnqQualityProblem.AnalyseAndJudgeFile.ToString() != "")
                {

                    FileStream fs = new FileStream(saveFileDialog1.FileName, FileMode.Create, FileAccess.Write);
                    BinaryWriter bw = new BinaryWriter(fs);
                    bw.Write(m_lnqQualityProblem.AnalyseAndJudgeFile.ToArray());
                    bw.Close();
                    fs.Close();
                }
            }
        }

        private void btnCauseAnalysisFileDown_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "pdf files (*.pdf)|*.pdf|doc files (*.doc)|*.doc|All files (*.*)|*.*";

            if (m_lnqQualityProblem.CauseAnalysisFile == null)
            {
                MessageDialog.ShowPromptMessage("未添加此附件");
                return;
            }

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (m_lnqQualityProblem.CauseAnalysisFile != null || m_lnqQualityProblem.CauseAnalysisFile.ToString() != "")
                {

                    FileStream fs = new FileStream(saveFileDialog1.FileName, FileMode.Create, FileAccess.Write);
                    BinaryWriter bw = new BinaryWriter(fs);
                    bw.Write(m_lnqQualityProblem.CauseAnalysisFile.ToArray());
                    bw.Close();
                    fs.Close();
                }
            }
        }

        private void btnRectificationMeasuresFileDown_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "pdf files (*.pdf)|*.pdf|doc files (*.doc)|*.doc|All files (*.*)|*.*";

            if (m_lnqQualityProblem.RectificationMeasuresFile == null)
            {
                MessageDialog.ShowPromptMessage("未添加此附件");
                return;
            }

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (m_lnqQualityProblem.RectificationMeasuresFile != null || m_lnqQualityProblem.RectificationMeasuresFile.ToString() != "")
                {

                    FileStream fs = new FileStream(saveFileDialog1.FileName, FileMode.Create, FileAccess.Write);
                    BinaryWriter bw = new BinaryWriter(fs);
                    bw.Write(m_lnqQualityProblem.RectificationMeasuresFile.ToArray());
                    bw.Close();
                    fs.Close();
                }
            }
        }

        private void chbInterimMeasureIsNeedTesting_Click(object sender, EventArgs e)
        {
            if (chbInterimMeasureIsNeedTesting.Checked)
            {
                lbInterimMeasureTestingID.Text = m_serverQualityProblem.GetBillID("试验验证计划表");

                if (!m_serverQualityProblem.InsertSundrySchedule("试验验证计划表",lbInterimMeasureTestingID.Text,out m_strErr))
                {
                    MessageDialog.ShowPromptMessage(m_strErr);
                    return;
                }
                else
                {
                    试验验证计划表 form = new 试验验证计划表(lbInterimMeasureTestingID.Text);
                    form.ShowDialog();
                }
            }
            else
            {
                if (!m_serverQualityProblem.DeleteSundrySchedule("试验验证计划表",lbInterimMeasureTestingID.Text,out m_strErr))
                {
                    MessageDialog.ShowPromptMessage(m_strErr);
                    return;
                }

                lbInterimMeasureTestingID.Text = "";
            }
        }

        private void chbAnalyseAndJudgeIsNeedAffirm_Click(object sender, EventArgs e)
        {

            if (chbAnalyseAndJudgeIsNeedAffirm.Checked)
            {
                lbAnalyseAndJudgeAffirmID.Text = m_serverQualityProblem.GetBillID("搭车分析计划表");

                if (!m_serverQualityProblem.InsertSundrySchedule("搭车分析计划表", lbAnalyseAndJudgeAffirmID.Text, out m_strErr))
                {
                    MessageDialog.ShowPromptMessage(m_strErr);
                    return;
                }
                else
                {
                    搭车分析计划表 form = new 搭车分析计划表(lbAnalyseAndJudgeAffirmID.Text);
                    form.ShowDialog();
                }
            }
            else
            {
                if (!m_serverQualityProblem.DeleteSundrySchedule("搭车分析计划表", lbAnalyseAndJudgeAffirmID.Text, out m_strErr))
                {
                    MessageDialog.ShowPromptMessage(m_strErr);
                    return;
                }

                lbAnalyseAndJudgeAffirmID.Text = "";
            }
        }

        private void chbCauseAnalysisIsNeedTesting_Click(object sender, EventArgs e)
        {

            if (chbCauseAnalysisIsNeedTesting.Checked)
            {
                lbCauseAnalysisTestingID.Text = m_serverQualityProblem.GetBillID("试验验证计划表");

                if (!m_serverQualityProblem.InsertSundrySchedule("试验验证计划表", lbCauseAnalysisTestingID.Text, out m_strErr))
                {
                    MessageDialog.ShowPromptMessage(m_strErr);
                    return;
                }
                else
                {
                    试验验证计划表 form = new 试验验证计划表(lbCauseAnalysisTestingID.Text);
                    form.ShowDialog();
                }
            }
            else
            {
                if (!m_serverQualityProblem.DeleteSundrySchedule("试验验证计划表", lbCauseAnalysisTestingID.Text, out m_strErr))
                {
                    MessageDialog.ShowPromptMessage(m_strErr);
                    return;
                }

                lbCauseAnalysisTestingID.Text = "";
            }
        }

        private void chbRectificationMeasuresIsNeedChange_Click(object sender, EventArgs e)
        {

            if (chbRectificationMeasuresIsNeedChange.Checked)
            {
                lbRectificationMeasuresChangeID.Text = m_serverQualityProblem.GetBillID("新品开发计划表");

                if (!m_serverQualityProblem.InsertSundrySchedule("新品开发计划表", lbRectificationMeasuresChangeID.Text, out m_strErr))
                {
                    MessageDialog.ShowPromptMessage(m_strErr);
                    return;
                }
                else
                {
                    新品开发计划表 form = new 新品开发计划表(lbRectificationMeasuresChangeID.Text);
                    form.ShowDialog();
                }
            }
            else
            {
                if (!m_serverQualityProblem.DeleteSundrySchedule("新品开发计划表", lbRectificationMeasuresChangeID.Text, out m_strErr))
                {
                    MessageDialog.ShowPromptMessage(m_strErr);
                    return;
                }

                lbRectificationMeasuresChangeID.Text = "";
            }
        }

        private void lbInterimMeasureTestingID_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            试验验证计划表 form = new 试验验证计划表(lbInterimMeasureTestingID.Text);
            form.ShowDialog();
        }

        private void lbAnalyseAndJudgeAffirmID_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            搭车分析计划表 form = new 搭车分析计划表(lbAnalyseAndJudgeAffirmID.Text);
            form.ShowDialog();
        }

        private void lbCauseAnalysisTestingID_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            试验验证计划表 form = new 试验验证计划表(lbCauseAnalysisTestingID.Text);
            form.ShowDialog();
        }

        private void lbRectificationMeasuresChangeID_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            新品开发计划表 form = new 新品开发计划表(lbRectificationMeasuresChangeID.Text);
            form.ShowDialog();
        }
    }
}

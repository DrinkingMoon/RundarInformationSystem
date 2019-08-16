using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PlatformManagement;
using UniversalControlLibrary;
using ServerModule;
using Service_Peripheral_HR;
using GlobalObject;

namespace Form_Peripheral_HR
{
    public partial class 学习平台 : Form
    {
        ITrainLearn _ServiceLearn = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<ITrainLearn>();
        ITrainBasicInfo _ServiceBasicInfo = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<ITrainBasicInfo>();

        Dictionary<string, string> _DicAnswer = new Dictionary<string, string>();
        List<Guid> _ListGuidQuestion = new List<Guid>();
        int _IndexGuid = 0;

        public 学习平台()
        {
            InitializeComponent();
        }

        private void 学习平台_Load(object sender, EventArgs e)
        {
            DataTable viewTable = _ServiceLearn.GetTree_Course();
            GlobalObject.GeneralFunction.LoadTreeViewDt(treeView1, viewTable, "ShowView", "ChildID", "ParentID", "ParentID = ''");
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (treeView1.SelectedNode != null)
            {
                int k = 0;
                if (Int32.TryParse(treeView1.SelectedNode.Tag == null ? "" : treeView1.SelectedNode.Tag.ToString(), out k))
                {
                    customDataGridView1.DataSource = _ServiceBasicInfo.GetTable_Ware(k);
                }

                return;
            }

            if (customDataGridView1.Rows.Count > 0)
            {
                customDataGridView1.Rows.Clear();
            }
        }

        private void customDataGridView1_DoubleClick(object sender, EventArgs e)
        {
            if (customDataGridView1.CurrentRow == null)
            {
                return;
            }

            FileOperationService.File_Look(new Guid(customDataGridView1.CurrentRow.Cells["文件唯一编码"].Value.ToString()),
                    GlobalObject.GeneralFunction.StringConvertToEnum<CE_CommunicationMode>
                    (BasicInfo.BaseSwitchInfo[(int)GlobalObject.CE_SwitchName.文件传输方式]));
        }

        private void btnBeginExam_Click(object sender, EventArgs e)
        {
            _ListGuidQuestion = _ServiceLearn.GetRandomQuestion(Convert.ToInt32(treeView1.SelectedNode.Tag));

            if (_ListGuidQuestion.Count() > 0)
            {
                treeView1.Enabled = false;
                CreateExamControl(_ListGuidQuestion[_IndexGuid]);
            }
            else
            {
                MessageDialog.ShowPromptMessage("无试题无法进行考试");
            }
        }

        void CreateExamControl(Guid guid)
        {
            List<View_HR_Train_QuestionBank> lstBankInfo = _ServiceBasicInfo.GetListQuestionBank(guid);

            if (lstBankInfo.Count() == 0)
            {
                return;
            }

            plExam.Controls.Clear();

            CE_HR_Train_QuesitonsType quesType = 
                GeneralFunction.StringConvertToEnum<CE_HR_Train_QuesitonsType>(lstBankInfo[0].考题类型);

            Font font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));

            Label lbType = new Label();
            lbType.AutoSize = true;
            lbType.Font = font;
            lbType.Location = new System.Drawing.Point(32, 51);
            lbType.Size = new System.Drawing.Size(119, 14);
            lbType.Text = "题目类型：" + quesType.ToString();
            plExam.Controls.Add(lbType);


            Label lbTM = new Label();
            lbTM.AutoSize = true;
            lbTM.Font = font;
            lbTM.Location = new System.Drawing.Point(32, 93);
            lbTM.Size = new System.Drawing.Size(49, 14);
            lbTM.Text = (_IndexGuid + 1).ToString() + ". 题目：";
            plExam.Controls.Add(lbTM);

            Label lbQuestions = new Label();
            lbQuestions.Font = font;
            lbQuestions.Location = new System.Drawing.Point(32, 135);
            lbQuestions.Size = new System.Drawing.Size(573, 116);
            lbQuestions.Text = lstBankInfo[0].考题内容;
            plExam.Controls.Add(lbQuestions);

            int height = 278;
            switch (quesType)
            {
                case CE_HR_Train_QuesitonsType.判断题:

                    RadioButton rbYes = new RadioButton();
                    rbYes.AutoSize = true;
                    rbYes.Font = font;
                    rbYes.Location = new System.Drawing.Point(35, height);
                    rbYes.Size = new System.Drawing.Size(95, 18);
                    rbYes.Text = "YES";
                    plExam.Controls.Add(rbYes);

                    height += 42;
                    RadioButton rbNo = new RadioButton();
                    rbNo.AutoSize = true;
                    rbNo.Font = font;
                    rbNo.Location = new System.Drawing.Point(35, height);
                    rbNo.Size = new System.Drawing.Size(95, 18);
                    rbNo.TabStop = true;
                    rbNo.Text = "NO";
                    plExam.Controls.Add(rbNo);
                    break;
                case CE_HR_Train_QuesitonsType.单选题:
                    for (int i = 0; i < lstBankInfo.Count; i++)
                    {
                        if (i != 0)
                        {
                            height += 42;
                        }

                        RadioButton rbSingle = new RadioButton();
                        rbSingle.AutoSize = true;
                        rbSingle.Font = font;
                        rbSingle.Location = new System.Drawing.Point(35, height);
                        rbSingle.Size = new System.Drawing.Size(95, 18);
                        rbSingle.Text = lstBankInfo[i].选项 + "." + lstBankInfo[i].选项内容;
                        plExam.Controls.Add(rbSingle);
                    }

                    break;
                case CE_HR_Train_QuesitonsType.多选题:

                    for (int i = 0; i < lstBankInfo.Count; i++)
                    {
                        if (i != 0)
                        {
                            height += 42;
                        }

                        CheckBox chbMulit = new CheckBox();
                        chbMulit.AutoSize = true;
                        chbMulit.Font = font;
                        chbMulit.Location = new System.Drawing.Point(35, height);
                        chbMulit.Size = new System.Drawing.Size(95, 18);
                        chbMulit.Text = lstBankInfo[i].选项 + "." + lstBankInfo[i].选项内容;
                        plExam.Controls.Add(chbMulit);
                    }
                    break;
                default:
                    break;
            }

            height += 78;
            Button btnUp = new Button();
            btnUp.Font = font;
            btnUp.Location = new System.Drawing.Point(190, height);
            btnUp.Size = new System.Drawing.Size(75, 23);
            btnUp.Text = "上一题";
            plExam.Controls.Add(btnUp);
            
            Button btnNext = new Button();
            btnNext.Font = font;
            btnNext.Location = new System.Drawing.Point(342, height);
            btnNext.Size = new System.Drawing.Size(75, 23);

            if (_IndexGuid + 1 == _ListGuidQuestion.Count)
            {
                btnNext.Text = "提交试卷";
            }
            else
            {
                btnNext.Text = "下一题";
            }

            plExam.Controls.Add(btnNext);

            btnNext.Click +=new EventHandler(btnNext_Click);
            btnUp.Click +=new EventHandler(btnUp_Click);
        }

        void btnUp_Click(object sender, EventArgs e)
        {
            if (_IndexGuid - 1 >= 0)
            {
                GetAnswer();
                _IndexGuid -= 1;
                CreateExamControl(_ListGuidQuestion[_IndexGuid]);
                SetAnswer();
            }
            else
            {
                MessageDialog.ShowPromptMessage("已经是【第一题】");
            }
        }

        void btnNext_Click(object sender, EventArgs e)
        {
            if (_IndexGuid + 1 <= _ListGuidQuestion.Count - 1)
            {
                GetAnswer();
                _IndexGuid += 1;
                CreateExamControl(_ListGuidQuestion[_IndexGuid]);
                SetAnswer();
            }
            else
            {
                if (MessageDialog.ShowEnquiryMessage("您确定【考试结束】，提交试卷吗？") == DialogResult.Yes)
                {
                    GetAnswer();
                    RecordExamInfo();
                    treeView1.Enabled = true;
                }
            }
        }

        void RecordExamInfo()
        {
            try
            {
                HR_Train_ExamHistory history = new HR_Train_ExamHistory();

                history.CourseID = Convert.ToInt32(treeView1.SelectedNode.Tag);
                history.ExamScore = Math.Round( MarkingAnswer(), 2);

                _ServiceLearn.RecordExamHistory(ref history);

                MessageDialog.ShowPromptMessage("您的考试分数为【" + history.ExamScore.ToString() + "】," 
                    + ((bool)history.IsPass ? "【通过】" : "【未通过】"));

                plExam.Controls.Clear();
                plExam.Controls.Add(this.btnBeginExam);
            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
            }
        }

        decimal MarkingAnswer()
        {
            decimal rightCount = 0;

            foreach (Guid guid in _ListGuidQuestion)
            {
                HR_Train_QuestionBank bank = _ServiceBasicInfo.GetBankInfo(guid);

                if (bank != null)
                {
                    if (bank.Answer == _DicAnswer[guid.ToString()])
                    {
                        rightCount += 1;
                    }
                }
                else
                {
                    rightCount += 1;
                }
            }

            return rightCount / (decimal)_ListGuidQuestion.Count() * 100;
        }

        void GetAnswer()
        {
            string strAnswer = "";

            foreach (Control cl in plExam.Controls)
            {
                if (cl is RadioButton)
                {
                    RadioButton rb = cl as RadioButton;

                    if (rb.Text == "YES" || rb.Text == "NO")
                    {
                        if (rb.Checked)
                        {
                            if (_DicAnswer.Keys.Contains(_ListGuidQuestion[_IndexGuid].ToString()))
                            {
                                _DicAnswer[_ListGuidQuestion[_IndexGuid].ToString()] = rb.Text;
                            }
                            else
                            {
                                _DicAnswer.Add(_ListGuidQuestion[_IndexGuid].ToString(), rb.Text);
                            }
                            return;
                        }
                    }
                    else
                    {
                        if (rb.Checked)
                        {
                            if (_DicAnswer.Keys.Contains(_ListGuidQuestion[_IndexGuid].ToString()))
                            {
                                _DicAnswer[_ListGuidQuestion[_IndexGuid].ToString()] = rb.Text.Substring(0, rb.Text.IndexOf("."));
                            }
                            else
                            {
                                _DicAnswer.Add(_ListGuidQuestion[_IndexGuid].ToString(), rb.Text.Substring(0, rb.Text.IndexOf(".")));
                            }
                            return;
                        }
                    }
                }
                else if (cl is CheckBox)
                {
                    CheckBox chb = cl as CheckBox;

                    if (chb.Checked)
                    {
                        strAnswer += chb.Text.Substring(0, chb.Text.IndexOf("."));
                    }
                }
            }

            if (_DicAnswer.Keys.Contains(_ListGuidQuestion[_IndexGuid].ToString()))
            {
                _DicAnswer[_ListGuidQuestion[_IndexGuid].ToString()] = strAnswer;
            }
            else
            {
                _DicAnswer.Add(_ListGuidQuestion[_IndexGuid].ToString(), strAnswer);
            }

            return;
        }

        void SetAnswer()
        {
            if (!_DicAnswer.Keys.Contains(_ListGuidQuestion[_IndexGuid].ToString()))
            {
                return;
            }

            string strAnswer = _DicAnswer[_ListGuidQuestion[_IndexGuid].ToString()];

            if (strAnswer != null && strAnswer.Trim().Length > 0)
            {
                foreach (Control cl in plExam.Controls)
                {
                    if (cl is RadioButton)
                    {
                        RadioButton rb = cl as RadioButton;

                        if (rb.Text == "YES" || rb.Text == "NO")
                        {
                            if (strAnswer.Contains(rb.Text))
                            {
                                rb.Checked = true;
                            }
                        }
                        else
                        {
                            if (strAnswer.Contains(rb.Text.Substring(0, rb.Text.IndexOf("."))))
                            {
                                rb.Checked = true;
                            }
                        }
                    }
                    else if (cl is CheckBox)
                    {
                        CheckBox chb = cl as CheckBox;

                        if (strAnswer.Contains(chb.Text.Substring(0, chb.Text.IndexOf("."))))
                        {
                            chb.Checked = true;
                        }
                    }
                }
            }
        }
    }
}

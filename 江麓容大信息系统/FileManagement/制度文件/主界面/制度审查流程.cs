using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ServerModule;
using Expression;
using GlobalObject;
using PlatformManagement;
using Service_Quality_File;
using UniversalControlLibrary;

namespace Form_Quality_File
{
    public partial class 制度审查流程 : Form
    {
        /// <summary>
        /// 制度审查流程服务组件
        /// </summary>
        IInstitutionProcess m_serverInstitution = Service_Quality_File.ServerModuleFactory.GetServerModule<IInstitutionProcess>();

        /// <summary>
        /// 错误信息
        /// </summary>
        string m_strErr = "";

        /// <summary>
        /// 单据编号控制类
        /// </summary>
        BillNumberControl m_billNoControl;

        /// <summary>
        /// 单据类消息发布器
        /// </summary>
        IBillMessagePromulgatorServer m_billMessageServer = BasicServerFactory.GetServerModule<IBillMessagePromulgatorServer>();

        public 制度审查流程()
        {
            InitializeComponent();

            string[] strBillStatus = { "全部", 
                                     InstitutionBillStatus.新建流程.ToString(),
                                     InstitutionBillStatus.等待科长审查.ToString(),
                                     InstitutionBillStatus.等待负责人审查.ToString(),
                                     InstitutionBillStatus.等待相关负责人审查.ToString(),
                                     InstitutionBillStatus.等待相关分管领导审查.ToString(),
                                     InstitutionBillStatus.等待分管领导审查.ToString(),
                                     InstitutionBillStatus.等待总经理审查.ToString(),
                                     InstitutionBillStatus.流程已结束.ToString()};

            checkBillDateAndStatus1.InsertComBox(strBillStatus);
            m_billNoControl = new BillNumberControl(CE_BillTypeEnum.制度审查流程.ToString(), m_serverInstitution);
            m_billMessageServer.BillType = CE_BillTypeEnum.制度审查流程.ToString();
        }

        /// <summary>
        /// 重新窗体消息处理函数
        /// </summary>
        /// <param name="m">窗体消息</param>
        protected override void DefWndProc(ref Message m)
        {
            switch (m.Msg)
            {
                //接收自定义消息,放弃未提交的单据号
                case WndMsgSender.CloseMsg:
                    // 放弃未使用的单据号
                    m_billNoControl.CancelBill();
                    break;

                case WndMsgSender.PositioningMsg:
                    WndMsgData msg = new WndMsgData();      //这是创建自定义信息的结构
                    Type dataType = msg.GetType();

                    msg = (WndMsgData)m.GetLParam(dataType);//这里获取的就是作为LParam参数发送来的信息的结构

                    DataTable dtMessage = UniversalFunction.PositioningOneRecord(msg.MessageContent, CE_BillTypeEnum.制度审查流程.ToString());

                    if (dtMessage.Rows.Count == 0)
                    {
                        m_billMessageServer.DestroyMessage(msg.MessageContent);
                        MessageDialog.ShowPromptMessage("未找到相关记录");
                    }
                    else
                    {
                        dataGridView1.DataSource = dtMessage;
                        dataGridView1.Rows[0].Selected = true;
                    }

                    break;

                default:
                    base.DefWndProc(ref m);
                    break;
            }
        }

        /// <summary>
        /// 刷新数据
        /// </summary>
        void RefreshData()
        {
            m_serverInstitution.QueryResultFilter = checkBillDateAndStatus1.GetSqlString("申请日期", "流程状态");

            dataGridView1.DataSource = 
                UniversalControlLibrary.BillInfo.BillInfoShowFilter(m_serverInstitution.GetAllBill(CE_BillTypeEnum.制度审查流程), "流程编号");

            userControlDataLocalizer1.Init(dataGridView1, this.Name,
                UniversalFunction.SelectHideFields(this.Name, dataGridView1.Name, BasicInfo.LoginID));
        }

        /// <summary>
        /// 定位记录
        /// </summary>
        /// <param name="billNo">定位用的单据号</param>
        void PositioningRecord(string billNo)
        {
            string strColName = "";

            foreach (DataGridViewColumn col in dataGridView1.Columns)
            {
                if (col.Visible)
                {
                    strColName = col.Name;
                    break;
                }
            }

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if ((string)dataGridView1.Rows[i].Cells["流程编号"].Value == billNo)
                {
                    dataGridView1.FirstDisplayedScrollingRowIndex = i;
                    dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells[strColName];
                    break;
                }
            }
        }

        /// <summary>
        /// 检测数据
        /// </summary>
        /// <param name="lnqProcess">LINQ数据集</param>
        /// <returns>通过返回True,否则返回False</returns>
        bool CheckData(FM_InstitutionProcess lnqProcess)
        {
            if (lnqProcess.FileUnique.ToString() == "00000000-0000-0000-0000-000000000000")
            {
                MessageDialog.ShowPromptMessage("请上传申请文件");
                return false;
            }

            if (lnqProcess.BillStatus == InstitutionBillStatus.流程已结束.ToString())
            {
                MessageDialog.ShowPromptMessage("流程状态错误");
                return false;
            }

            if (lnqProcess.OperationMode == "修订" && lnqProcess.ReplaceFileID == null)
            {
                MessageDialog.ShowPromptMessage("修订模式，请选择替换文件");
                return false;
            }

            if (lnqProcess.FileName.Trim().Length == 0 || lnqProcess.FileNo.Trim().Length == 0)
            {
                MessageDialog.ShowPromptMessage("请填写文件编号/文件名称");
                return false;
            }

            if (lnqProcess.SortID == 0)
            {
                MessageDialog.ShowPromptMessage("请选择文件类别");
                return false;
            }

            return true;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            制度审查流程明细 frmDetail = new 制度审查流程明细();
            frmDetail.Custom_FlowInfo = m_serverInstitution.GetProcessInfo(null);
            FormCommonProcess frm = new FormCommonProcess(CE_BillTypeEnum.制度审查流程, null, frmDetail, CE_OperatorMode.添加);
            frm.CommonProcessSubmit += new FormCommonProcess.FormSubmit(frm_CommonProcessSubmit);

            if (frm.ShowDialog() != DialogResult.OK)
            {
                m_billNoControl.CancelBill(frmDetail.LnqInstitution.BillNo);
            }

            RefreshData();
            PositioningRecord(frmDetail.LnqInstitution.BillNo);
        }

        bool frm_CommonProcessSubmit(CustomFlowForm form, string advise)
        {
            FM_InstitutionProcess lnqProcess = (FM_InstitutionProcess)form.ResultInfo;

            if (!CheckData(lnqProcess))
            {
                return false;
            }

            MessageDialog.ShowPromptMessage("请选择相关部门");

            FormDataTableCheck frmCheck = new FormDataTableCheck(UniversalFunction.GetAllDeptInfo());

            if (frmCheck.ShowDialog() == DialogResult.OK)
            {
                List<string> list = DataSetHelper.ColumnsToList_Distinct(frmCheck._DtResult, "部门代码");

                if (!m_serverInstitution.AddInfo(lnqProcess, list, out m_strErr))
                {
                    MessageDialog.ShowPromptMessage(m_strErr);
                    return false;
                }
            }
            else
            {
                MessageDialog.ShowPromptMessage("请选择相关部门");
                return false;
            }

            return true;
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                return;
            }

            string billNo = dataGridView1.CurrentRow.Cells["流程编号"].Value.ToString();

            制度审查流程明细 frmDetail = new 制度审查流程明细();
            frmDetail.FlowInfo_BillNo = billNo;
            frmDetail.Custom_FlowInfo = m_serverInstitution.GetProcessInfo(billNo);
            frmDetail.Custom_FlowMagic = m_serverInstitution.GetExcuteInfo(billNo);
            FormCommonProcess frm = new FormCommonProcess(CE_BillTypeEnum.制度审查流程, null, frmDetail, CE_OperatorMode.编辑);
            frm.CommonProcessSubmit += new FormCommonProcess.FormSubmit(frm_CommonProcessSubmit1);
            frm.ShowDialog();

            RefreshData();
            PositioningRecord(billNo);
        }

        bool frm_CommonProcessSubmit1(CustomFlowForm form, string advise)
        {
            FM_InstitutionProcess lnqProcess = (FM_InstitutionProcess)form.ResultInfo;

            if (!m_serverInstitution.UpdateInfo(lnqProcess.BillNo, advise, out m_strErr))
            {
                MessageDialog.ShowPromptMessage(m_strErr);
                return false;
            }

            return true;
        }

        private void 制度审查流程_Load(object sender, EventArgs e)
        {
            if (!DesignMode)
            {
                checkBillDateAndStatus1.InitDateTime();
                RefreshData();
            }
        }

        private void checkBillDateAndStatus1_OnCompleteSearch()
        {
            RefreshData();
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            btnFind_Click(null, null);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshData();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow.Cells["流程状态"].Value.ToString() == "流程已结束")
            {
                MessageDialog.ShowPromptMessage("流程状态已结束，不能删除");
                return;
            }

            if (dataGridView1.CurrentRow.Cells["申请人"].Value.ToString() != BasicInfo.LoginName)
            {
                MessageDialog.ShowPromptMessage("必须由申请人本人才能删除此流程");
                return;
            }

            if (MessageDialog.ShowEnquiryMessage("是否要删除此流程?") == DialogResult.Yes)
            {
                string strSDBNo = dataGridView1.CurrentRow.Cells["流程编号"].Value.ToString();

                if (!m_serverInstitution.DeleteInfo(strSDBNo, out m_strErr))
                {
                    MessageDialog.ShowPromptMessage(m_strErr);
                    return;
                }
                else
                {
                    m_billMessageServer.DestroyMessage(strSDBNo);
                    m_billNoControl.CancelBill(strSDBNo);
                    MessageDialog.ShowPromptMessage("流程删除成功");
                }
            }

            RefreshData();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            ExcelHelperP.DatagridviewToExcel(saveFileDialog1, dataGridView1);
        }
    }
}


/******************************************************************************
 *
 * 文件名称:  TCU返修信息.cs
 * 作者    :  邱瑶       日期: 2013/9/11
 * 开发平台:  vs2008(c#)
 * 用于    :  生产线管理信息系统
 ******************************************************************************/
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
using Service_Peripheral_HR;
using UniversalControlLibrary;

namespace Expression
{
    public partial class TCU返修信息 : Form
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        string m_error;

        /// <summary>
        /// TCU返修信息管理服务类
        /// </summary>
        ITCURepairInfoServer m_tcuRepairServer = ServerModule.ServerModuleFactory.GetServerModule<ITCURepairInfoServer>();

        /// <summary>
        /// TCU返修信息数据集
        /// </summary>
        TCU_RepairInfo m_lnqRepairInfo = new TCU_RepairInfo();

        /// <summary>
        /// 操作权限
        /// </summary>
        PlatformManagement.AuthorityFlag m_authorityFlag;

        /// <summary>
        /// 人员档案服务组件
        /// </summary>
        IPersonnelArchiveServer m_personnerServer = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<IPersonnelArchiveServer>();

        /// <summary>
        /// 单据类消息发布器
        /// </summary>
        IBillMessagePromulgatorServer m_billMessageServer = BasicServerFactory.GetServerModule<IBillMessagePromulgatorServer>();

        /// <summary>
        /// 产品信息管理服务组件
        /// </summary>
        IProductInfoServer m_productInfoServer = ServerModule.ServerModuleFactory.GetServerModule<IProductInfoServer>();

        public TCU返修信息(PlatformManagement.FunctionTreeNodeInfo nodeInfo)
        {
            InitializeComponent();

            m_billMessageServer.BillType = "TCU返修信息管理";
            m_authorityFlag = nodeInfo.Authority;

            string[] strBillStatus = { "全部", "等待质管确认", "已完成" };

            checkBillDateAndStatus1.InsertComBox(strBillStatus);

            checkBillDateAndStatus1.OnCompleteSearch +=
                new GlobalObject.DelegateCollection.NonArgumentHandle(checkBillDateAndStatus1_OnCompleteSearch);


            checkBillDateAndStatus1.dtpStartTime.Value = ServerTime.Time.AddDays(1).AddMonths(-1);
            checkBillDateAndStatus1.dtpEndTime.Value = ServerTime.Time.AddDays(1);

            RefreshControl();
        }

        /// <summary>
        /// 权限控制
        /// </summary>
        /// <param name="authorityFlag">权限标志</param>
        void AuthorityControl(PlatformManagement.AuthorityFlag authorityFlag)
        {
            FaceAuthoritySetting.SetVisibly(toolStrip1, authorityFlag);
        }

        /// <summary>
        /// 定位记录
        /// </summary>
        /// <param name="billNo">定位用的单据号</param>
        void PositioningRecord(string billNo)
        {
            if (Convert.ToInt32(billNo) > 0)
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
                    if (dataGridView1.Rows[i].Cells["单号"].Value.ToString() == billNo)
                    {
                        dataGridView1.FirstDisplayedScrollingRowIndex = i;
                        dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells[strColName];
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 改变控件大小
        /// </summary>
        private void TCU返修信息_Resize(object sender, EventArgs e)
        {
            labelTitle.Location = new Point((this.Width - labelTitle.Width) / 2, labelTitle.Location.Y);
        }

        /// <summary>
        /// 初始化控件
        /// </summary>
        void ClearControl()
        {
            txtApplicant.Text = "";
            txtApplicant.Tag = "";
            dtpxtApplicant.Value = ServerTime.Time;
            txtAssociatedBillNo.Text = "";
            txtBatchNo.Text = "";
            txtBugDescribe.Text = "";
            txtConfirmDate.Text = "";
            txtConfirmor.Text = "";
            txtDispostMeasure.Text = "";
            txtMechanics.Text = "";
            txtMechanics.Tag = "";
            txtReasonAnalyse.Text = "";
            txtRecorder.Text = BasicInfo.LoginName;
            txtRecordTime.Text = ServerTime.Time.ToString();
            txtTCUID.Text = "";
            txtTCUVersions.Text = "";
            txtNewProductName.Text = "";
            txtProductName.Text = "";
            txtNewProductVersion.Text = "";
            txtProductVersion.Text = "";
            cmbSiteSource.SelectedIndex = -1;

            txtBugDescribe.ReadOnly = false;
            txtApplicant.Enabled = true;
            dtpxtApplicant.Enabled = true;
        }

        /// <summary>
        /// 刷新
        /// </summary>
        void RefreshControl()
        {
            m_tcuRepairServer.QueryResultFilter = QueryFilterControl.GetFilterString(labelTitle.Text)
                     + checkBillDateAndStatus1.GetSqlString("记录时间", "单据状态");

            IQueryResult result;

            if (!m_tcuRepairServer.GetAllData(out result, out m_error))
            {
                MessageDialog.ShowPromptMessage(m_error);
            }

            if (dataGridView1.Rows.Count > 0)
            {
                dataGridView1.Columns["员工编号"].Visible = false;
                dataGridView1.Columns["确认人编号"].Visible = false;
                dataGridView1.Columns["报修人编号"].Visible = false;
            }

            if (result.DataCollection == null || result.DataCollection.Tables.Count == 0)
            {
                return;
            }

            dataGridView1.DataSource = result.DataCollection.Tables[0];

            userControlDataLocalizer1.Init(dataGridView1, this.Name,
                UniversalFunction.SelectHideFields(this.Name, dataGridView1.Name, BasicInfo.LoginID));
        }

        private void txtApplicant_OnCompleteSearch()
        {
            txtApplicant.Text = txtApplicant.DataResult["姓名"].ToString();
            txtApplicant.Tag = txtApplicant.DataResult["工号"].ToString();
        }

        private void txtMechanics_OnCompleteSearch()
        {
            txtMechanics.Text = txtMechanics.DataResult["姓名"].ToString();
            txtMechanics.Tag = txtMechanics.DataResult["工号"].ToString();
        }

        private void checkBillDateAndStatus1_OnCompleteSearch()
        {
            RefreshControl();
        }

        private void 新建单据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClearControl();

            lbStatus.Text = "新建单据";
        }

        /// <summary>
        /// 赋值
        /// </summary>
        void GetMessage()
        {
            m_lnqRepairInfo.Applicant = txtApplicant.Tag.ToString();
            m_lnqRepairInfo.ApplicantTime = dtpxtApplicant.Value;
            m_lnqRepairInfo.AssociatedBillNo = txtAssociatedBillNo.Text;
            m_lnqRepairInfo.BugDescribe = txtBugDescribe.Text;

            if (txtConfirmor.Text != "")
            {
                m_lnqRepairInfo.Confirmor = txtConfirmor.Tag.ToString();
                m_lnqRepairInfo.ConfirmTime = Convert.ToDateTime(txtConfirmDate.Text);
            }

            m_lnqRepairInfo.DispostMeasure = txtDispostMeasure.Text;

            if (rbResultBF.Checked)
            {
                m_lnqRepairInfo.DispostResult = "报废";
            }
            else
            {
                m_lnqRepairInfo.DispostResult = "合格";
            }

            m_lnqRepairInfo.Mechanics = txtMechanics.Tag.ToString();
            m_lnqRepairInfo.NewTCUProcedureName = txtNewProductName.Text.ToUpper().Trim();
            m_lnqRepairInfo.NewProcedureVersion = txtNewProductVersion.Text.ToUpper().Trim();
            m_lnqRepairInfo.ReasonAnalyse = txtReasonAnalyse.Text;
            m_lnqRepairInfo.Recorder = BasicInfo.LoginID;
            m_lnqRepairInfo.RecordTime = ServerTime.Time;
            m_lnqRepairInfo.SiteSource = cmbSiteSource.Text;
            m_lnqRepairInfo.TCUAssemblyBatch = txtBatchNo.Text;
            m_lnqRepairInfo.TCUID = txtTCUID.Text;
            m_lnqRepairInfo.TCUProcedureName = txtProductName.Text.ToUpper().Trim();
            m_lnqRepairInfo.TCUProcedureVersion = txtProductVersion.Text.ToUpper().Trim();
            m_lnqRepairInfo.TCUVersions = txtTCUVersions.Text.ToUpper().Trim();
            m_lnqRepairInfo.Statua = "等待质管确认";
        }

        /// <summary>
        /// 检测控件的正确性
        /// </summary>
        /// <returns>正确返回true，不正确返回false</returns>
        bool CheckControl()
        {
            if (txtApplicant.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("请选择报修人员!");
                return false;
            }

            if (cmbSiteSource.Text == "")
            {
                MessageDialog.ShowPromptMessage("请选择站点/来源！");
                return false;
            }

            if (txtAssociatedBillNo.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("请填写关联单号！");
                return false;
            }

            if (txtBugDescribe.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("请填写故障描述！");
                return false;
            }

            if (txtReasonAnalyse.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("请填写原因分析！");
                return false;
            }

            if (txtDispostMeasure.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("请填写处理措施！");
                return false;
            }

            if (txtMechanics.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("请选择维修工程师！");
                return false;
            }

            if (txtNewProductName.Text == "")
            {
                MessageDialog.ShowPromptMessage("请选择重新烧写产品程序名称及版本！");
                return false;
            }

            if (txtTCUVersions.Text == "")
            {
                MessageDialog.ShowPromptMessage("请填写TCU硬件版本！");
                return false;
            }

            //if (txtProductName.Text == "")
            //{
            //    MessageDialog.ShowPromptMessage("请选择产品程序名称及版本！");
            //    return false;
            //}

            return true;
        }

        private void 提交单据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!CheckControl())
            {
                return;
            }

            GetMessage();

            if (!m_tcuRepairServer.InsertData(m_lnqRepairInfo, out m_error))
            {
                MessageDialog.ShowPromptMessage(m_error);
                return;
            }

            MessageDialog.ShowPromptMessage("新增成功！");

            m_billMessageServer.SendNewFlowMessage(m_tcuRepairServer.GetMaxBillNo(),
                       string.Format("{0} 号TCU返修信息，请质管人员确认", m_tcuRepairServer.GetMaxBillNo()), BillFlowMessage_ReceivedUserType.角色,
                       CE_RoleEnum.质量工程师.ToString());

            RefreshControl();
        }

        private void 刷新toolStripButton1_Click(object sender, EventArgs e)
        {
            RefreshControl();
        }

        private void 综合查询toolStripButton3_Click(object sender, EventArgs e)
        {
            IAuthorization authorization = PlatformFactory.GetObject<IAuthorization>();
            string businessID = "TCU返修信息管理";
            IQueryResult qr = authorization.Query(businessID, null, null, 0);
            List<string> lstFindField = new List<string>();

            if (qr.DataCollection == null || qr.DataCollection.Tables.Count == 0)
            {
                return;
            }

            DataColumnCollection columns = qr.DataCollection.Tables[0].Columns;

            if (qr.Succeeded && columns.Count > 0)
            {
                for (int i = 0; i < columns.Count; i++)
                {
                    lstFindField.Add(columns[i].ColumnName);
                }
            }

            FormConditionFind formFindCondition = new FormConditionFind(this, lstFindField.ToArray(), businessID, labelTitle.Text);
            formFindCondition.ShowDialog();
        }

        private void 主管审核toolStripButton1_Click(object sender, EventArgs e)
        {
            if (!CheckControl())
            {
                return;
            }

            if (!m_tcuRepairServer.ConfirmUpdateData(dataGridView1.CurrentRow.Cells["单号"].Value.ToString(), out m_error))
            {
                MessageDialog.ShowPromptMessage(m_error);
            }

            MessageDialog.ShowPromptMessage("确认成功！");

            List<string> noticeUser = new List<string>();

            noticeUser.Add(txtRecorder.Tag.ToString());
            
            m_billMessageServer.EndFlowMessage(dataGridView1.CurrentRow.Cells["单号"].Value.ToString(), string.Format("{0} 号TCU返修信息，已完成",
                dataGridView1.CurrentRow.Cells["单号"].Value.ToString()), null, noticeUser);

            RefreshControl();
        }

        private void 修改单据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!CheckControl())
            {
                return;
            }

            GetMessage();

            m_lnqRepairInfo.ID = Convert.ToInt32(dataGridView1.CurrentRow.Cells["单号"].Value.ToString());

            if (!m_tcuRepairServer.UpdateData(m_lnqRepairInfo, out m_error))
            {
                MessageDialog.ShowPromptMessage(m_error);
            }

            MessageDialog.ShowPromptMessage("修改成功！");

            m_billMessageServer.PassFlowMessage(m_tcuRepairServer.GetMaxBillNo(),
                       string.Format("{0} 号TCU返修信息，请质管人员确认", m_tcuRepairServer.GetMaxBillNo()), BillFlowMessage_ReceivedUserType.角色,
                       CE_RoleEnum.质量工程师.ToString());

            RefreshControl();
        }

        private void 删除单据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 1)
            {
                if (dataGridView1.CurrentRow.Cells["单据状态"].Value.ToString() == "已完成"
                    || dataGridView1.CurrentRow.Cells["记录人"].Value.ToString() != BasicInfo.LoginName)
                {
                    MessageDialog.ShowPromptMessage("单据已经完成或记录人员非本人！不能删除");
                    return;
                }

                if (!m_tcuRepairServer.DeleteData(dataGridView1.CurrentRow.Cells["单号"].Value.ToString(),out m_error))
                {
                    MessageDialog.ShowPromptMessage(m_error);
                    return;
                }

                if (MessageBox.Show("您是否确定要删除单据号为【" + dataGridView1.CurrentRow.Cells["单号"].Value.ToString() + "】",
               "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    m_billMessageServer.DestroyMessage(dataGridView1.CurrentRow.Cells["单号"].Value.ToString());
                    RefreshControl();
                }
            }
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            txtApplicant.Text = dataGridView1.CurrentRow.Cells["报修人"].Value.ToString();
            txtApplicant.Tag = dataGridView1.CurrentRow.Cells["报修人编号"].Value.ToString();
            txtAssociatedBillNo.Text = dataGridView1.CurrentRow.Cells["关联单号"].Value.ToString();
            txtBatchNo.Text = dataGridView1.CurrentRow.Cells["电路板总成批次"].Value.ToString();
            txtBugDescribe.Text = dataGridView1.CurrentRow.Cells["故障描述"].Value.ToString();
            txtConfirmDate.Text = dataGridView1.CurrentRow.Cells["质管确认时间"].Value.ToString();
            txtConfirmor.Text = dataGridView1.CurrentRow.Cells["质管确认人"].Value.ToString();
            txtDispostMeasure.Text = dataGridView1.CurrentRow.Cells["处理措施"].Value.ToString();
            txtMechanics.Text = dataGridView1.CurrentRow.Cells["维修工程师"].Value.ToString();
            txtMechanics.Tag = dataGridView1.CurrentRow.Cells["维修工程师编号"].Value.ToString();
            txtReasonAnalyse.Text = dataGridView1.CurrentRow.Cells["原因分析"].Value.ToString();
            txtRecorder.Text = dataGridView1.CurrentRow.Cells["记录人"].Value.ToString();
            txtRecorder.Tag = dataGridView1.CurrentRow.Cells["员工编号"].Value.ToString();
            txtRecordTime.Text = dataGridView1.CurrentRow.Cells["记录时间"].Value.ToString();
            txtTCUID.Text = dataGridView1.CurrentRow.Cells["TCU编号"].Value.ToString();
            txtTCUVersions.Text = dataGridView1.CurrentRow.Cells["TCU版本"].Value.ToString();
            txtNewProductName.Text = dataGridView1.CurrentRow.Cells["重新烧写产品程序名称"].Value.ToString();
            txtNewProductVersion.Text = dataGridView1.CurrentRow.Cells["重新烧写产品程序版本"].Value.ToString();
            txtProductName.Text = dataGridView1.CurrentRow.Cells["TCU产品程序名称"].Value.ToString();
            txtProductVersion.Text = dataGridView1.CurrentRow.Cells["产品程序版本"].Value.ToString();
            cmbSiteSource.Text = dataGridView1.CurrentRow.Cells["站点/来源"].Value.ToString();
            dtpxtApplicant.Value = Convert.ToDateTime(dataGridView1.CurrentRow.Cells["报修时间"].Value.ToString());
            lbStatus.Text = dataGridView1.CurrentRow.Cells["单据状态"].Value.ToString();

            if (dataGridView1.CurrentRow.Cells["处理结果"].Value.ToString() == "报废")
            {
                rbResultBF.Checked = true;
            }
            else
            {
                rbResultHG.Checked = true;
            }
        }

        private void txtAssociatedBillNo_Click(object sender, EventArgs e)
        {
            if (cmbSiteSource.SelectedIndex == 2)
            {
                FormQueryInfo frm = QueryInfoDialog.GetFeedBackBill("", "");
                frm.ShowDialog();

                if ((string)frm.GetDataItem("反馈单号") != null)
                {
                    txtAssociatedBillNo.ReadOnly = true;

                    txtAssociatedBillNo.Text = (string)frm.GetDataItem("反馈单号");
                    txtApplicant.Tag = m_personnerServer.GetPersonnelViewInfoByName((string)frm.GetDataItem("反馈人"));
                    txtApplicant.Text = (string)frm.GetDataItem("反馈人");

                    dtpxtApplicant.Value = (DateTime)frm.GetDataItem("反馈日期");
                    txtBugDescribe.Text = (string)frm.GetDataItem("故障说明");

                    txtBugDescribe.ReadOnly = true;
                    txtApplicant.Enabled = false;
                    dtpxtApplicant.Enabled = false;
                }
            }
            else
            {
                txtAssociatedBillNo.ReadOnly = false;
            }
        }

        private void txtBatchNo_OnCompleteSearch()
        {
            txtBatchNo.Text = txtBatchNo.DataResult["批次号"].ToString();
        }

        private void txtBatchNo_Enter(object sender, EventArgs e)
        {
            string sql = " and b.物品名称 like '%TCU电路板总成%'";

            txtBatchNo.StrEndSql = sql;
        }

        private void cmbSiteSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbSiteSource.SelectedIndex == 2)
            {
                txtAssociatedBillNo.ReadOnly = true;
            }
            else
            {
                txtAssociatedBillNo.ReadOnly = false;
            }
        }
    }
}

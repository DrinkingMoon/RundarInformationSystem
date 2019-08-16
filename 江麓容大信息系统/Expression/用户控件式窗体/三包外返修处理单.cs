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
    /// <summary>
    /// 三包外返修处理单界面
    /// </summary>
    public partial class 三包外返修处理单 : Form
    {
        /// <summary>
        /// 单据号
        /// </summary>
        string strBillID = "";

        /// <summary>
        /// 部门服务组件
        /// </summary>
        IDepartmentServer m_serverDepartment = ServerModuleFactory.GetServerModule<IDepartmentServer>();

        /// <summary>
        /// 营销产品服务组件
        /// </summary>
        IBomServer m_serviceBom = ServerModuleFactory.GetServerModule<IBomServer>();

        /// <summary>
        /// 三包外服务组件
        /// </summary>
        IThreePacketsOfTheRepairBill m_threePacketsOfTheRepairServer = ServerModuleFactory.GetServerModule<IThreePacketsOfTheRepairBill>();

        /// <summary>
        /// 数据集
        /// </summary>
        YX_ThreePacketsOfTheRepairBill m_lnqBill = new YX_ThreePacketsOfTheRepairBill();

        /// <summary>
        /// 可供查找的所有字段
        /// </summary>
        string[] m_findField = null;

        /// <summary>
        /// 错误信息
        /// </summary>
        string m_strErr;

        /// <summary>
        /// 权限标志
        /// </summary>
        AuthorityFlag m_authFlag;

        /// <summary>
        /// 单据编号控制类
        /// </summary>
        BillNumberControl m_billNoControl;

        /// <summary>
        /// 单据类消息发布器
        /// </summary>
        IBillMessagePromulgatorServer m_billMessageServer = BasicServerFactory.GetServerModule<IBillMessagePromulgatorServer>();

        /// <summary>
        /// 型号变更服务组件
        /// </summary>
        IProductChange m_product = ServerModuleFactory.GetServerModule<IProductChange>();

        public 三包外返修处理单(PlatformManagement.FunctionTreeNodeInfo nodeInfo)
        {
            InitializeComponent();

            m_billMessageServer.BillType = "三包外返修处理单";

            m_billNoControl = new BillNumberControl(CE_BillTypeEnum.三包外返修处理单, m_threePacketsOfTheRepairServer);

            m_authFlag = nodeInfo.Authority;
            cmbProductType.DataSource = m_serviceBom.GetAssemblyTypeList();

            string[] strBillStatus = { "全  部", 
                                     "新建单据", 
                                     "等待确认收货",
                                     "等待领料明细申请",
                                     "等待返修车间主管审核",
                                     "等待仓管确认出库",
                                     "等待销售策略",
                                     "等待营销总监审核",
                                     "等待财务确认",
                                     "等待返修完成",
                                     "等待营销确认",
                                     "已完成"};

            checkBillDateAndStatus1.InsertComBox(strBillStatus);

            checkBillDateAndStatus1.dtpStartTime.Value = ServerTime.Time.AddDays(1).AddMonths(-1);
            checkBillDateAndStatus1.dtpEndTime.Value = ServerTime.Time.AddDays(1);

            RefreshDataGirdView(m_threePacketsOfTheRepairServer.GetAllBill(checkBillDateAndStatus1.cmbBillStatus.Text, 
                checkBillDateAndStatus1.dtpStartTime.Value, checkBillDateAndStatus1.dtpEndTime.Value));
        }

        private void 三包外返修处理单_Load(object sender, EventArgs e)
        {
            AuthorityControl(m_authFlag);
        }

        /// <summary>
        /// 显示信息
        /// </summary>
        void ShowMessage()
        {
            lbDJZT.Text = m_lnqBill.DJZT;
            lbDJH.Text = m_lnqBill.Bill_ID;
            txtClient.Text = m_lnqBill.Client;
            txtVehicleShelfNumber.Text = m_lnqBill.VehicleShelfNumber;
            txtPhone.Text = m_lnqBill.Phone;
            numMileage.Value = Convert.ToDecimal(m_lnqBill.Mileage);
            txtMarketingRemark.Text = m_lnqBill.MarketingRemark;
            txtProductCode.Text = m_lnqBill.ProductCode;
            txtTheDiagnosis.Text = m_lnqBill.TheDiagnosis;
            cmbProductType.Text = m_lnqBill.ProductType;
        }

        /// <summary>
        /// 获得信息
        /// </summary>
        void GetMessage()
        {
            m_lnqBill.Bill_ID = lbDJH.Text;
            m_lnqBill.Client = txtClient.Text;
            m_lnqBill.DJZT = lbDJZT.Text;
            m_lnqBill.Mileage = numMileage.Value;
            m_lnqBill.VehicleShelfNumber = txtVehicleShelfNumber.Text;
            m_lnqBill.Phone = txtPhone.Text;
            m_lnqBill.MarketingRemark = txtMarketingRemark.Text;
            m_lnqBill.ProductCode = txtProductCode.Text;
            m_lnqBill.ProductType = cmbProductType.Text;
            m_lnqBill.TheDiagnosis = txtTheDiagnosis.Text;

        }

        /// <summary>
        /// 刷新
        /// </summary>
        /// <param name="source">数据集</param>
        void RefreshDataGirdView(DataTable source)
        {
            dataGridView1.DataSource = source;

            if (m_findField == null)
            {
                List<string> lstColumnName = new List<string>();

                for (int i = 0; i < dataGridView1.Columns.Count; i++)
                {
                    if (dataGridView1.Columns[i].Visible)
                    {
                        lstColumnName.Add(dataGridView1.Columns[i].Name);
                    }
                }

                m_findField = lstColumnName.ToArray();
            }

            userControlDataLocalizer1.Init(dataGridView1, this.Name,
                UniversalFunction.SelectHideFields(this.Name, dataGridView1.Name, BasicInfo.LoginID));

            dataGridView1.Columns["单据号"].Width = 150;
            dataGridView1.Columns["单据状态"].Width = 150;
            PositioningRecord(m_lnqBill.Bill_ID);
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
                if ((string)dataGridView1.Rows[i].Cells["单据号"].Value == billNo)
                {
                    dataGridView1.FirstDisplayedScrollingRowIndex = i;
                    dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells[strColName];
                    break;
                }
            }
        }

        /// <summary>
        /// 清空数据
        /// </summary>
        void ClearMessage()
        {
            txtVehicleShelfNumber.Text = "";
            numMileage.Value = 0;
            lbDJH.Text = "";
            lbDJZT.Text = "";
            txtProductCode.Text = "";
            txtTheDiagnosis.Text = "";
            txtMarketingRemark.Text = "";
            txtClient.Text = "";
            cmbProductType.SelectedIndex = -1;
        }

        /// <summary>
        /// 权限控制
        /// </summary>
        /// <param name="authorityFlag">权限标志</param>
        void AuthorityControl(PlatformManagement.AuthorityFlag authorityFlag)
        {
            FaceAuthoritySetting.SetVisibly(menuStrip, authorityFlag);
            FaceAuthoritySetting.SetEnable(this.Controls, authorityFlag);
        }

        /// <summary>
        /// 更新信息
        /// </summary>
        /// <returns>成功返回True，否则False</returns>
        bool UpdateMessage()
        {
            GetMessage();

            strBillID = m_lnqBill.Bill_ID;

            if (!m_threePacketsOfTheRepairServer.UpdateBill(m_lnqBill, out m_strErr))
            {
                MessageDialog.ShowErrorMessage(m_strErr);
                return false;
            }
            else
            {
                MessageDialog.ShowPromptMessage("提交成功");
            }


            RefreshDataGirdView(m_threePacketsOfTheRepairServer.GetAllBill(checkBillDateAndStatus1.cmbBillStatus.Text, 
                checkBillDateAndStatus1.dtpStartTime.Value, checkBillDateAndStatus1.dtpEndTime.Value));
            return true;
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

                    DataTable dtMessage = UniversalFunction.PositioningOneRecord(msg.MessageContent, "三包外返修处理单");

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

        private void checkBillDateAndStatus1_OnCompleteSearch()
        {
            RefreshDataGirdView(m_threePacketsOfTheRepairServer.GetAllBill(checkBillDateAndStatus1.cmbBillStatus.Text, 
                checkBillDateAndStatus1.dtpStartTime.Value, checkBillDateAndStatus1.dtpEndTime.Value));
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.RowCount == 0)
            {
                return;
            }

            m_lnqBill = m_threePacketsOfTheRepairServer.GetBill(dataGridView1.CurrentRow.Cells["单据号"].Value.ToString(),
                out m_strErr);

            if (m_lnqBill == null)
            {
                return;
            }
            else
            {
                ShowMessage();
            }
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                return;
            }
            else
            {
                三包外返修领料明细 form = new 三包外返修领料明细(m_authFlag, dataGridView1.CurrentRow.Cells["单据号"].Value.ToString());

                //下线车间要求改成非模态对话框 modify by cjb on 2014.9.2
                //form.ShowDialog();
                form.Show();

                m_lnqBill.MarketingStrategy = form.LnqBill.MarketingStrategy;
                m_lnqBill.PlantRemark = form.LnqBill.PlantRemark;
                m_lnqBill.RepairTaskTime = form.LnqBill.RepairTaskTime;

            }
        }

        #region 流程按钮事件

        private void 新建单据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClearMessage();
            lbDJH.Text = m_billNoControl.GetNewBillNo();
            lbDJZT.Text = "新建单据";
            m_lnqBill = new YX_ThreePacketsOfTheRepairBill();
        }

        private void 单据提交ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lbDJZT.Text != "新建单据")
            {
                MessageDialog.ShowPromptMessage("请重新确认单据状态");
            }
            else
            {
                if (UpdateMessage())
                {
                    m_billMessageServer.DestroyMessage(strBillID);
                    m_billMessageServer.SendNewFlowMessage(strBillID,
                        string.Format("{0} 号三包外返修处理单，请下线车间确认收货", strBillID),
                        CE_RoleEnum.下线车间人员);
                }
            }
        }

        private void 确认收货ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lbDJZT.Text != "等待确认收货")
            {
                MessageDialog.ShowPromptMessage("请重新确认单据状态");
            }
            else
            {
                if (UpdateMessage())
                {
                    m_billMessageServer.PassFlowMessage(strBillID,
                            string.Format("{0} 号三包外返修处理单，请下线车间提交领料清单", strBillID),
                            CE_RoleEnum.下线车间人员.ToString(), true);
                }
            }
        }

        private void 提交领料明细ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lbDJZT.Text != "等待领料明细申请")
            {
                MessageDialog.ShowPromptMessage("请重新确认单据状态");
            }
            else
            {
                if (UpdateMessage())
                {
                    m_billMessageServer.PassFlowMessage(strBillID,
                         string.Format("{0} 号三包外返修处理单，请质管确认清单责任归属", strBillID),
                         CE_RoleEnum.质量工程师.ToString(), true);

                }
            }
        }

        private void 领料明细审核ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lbDJZT.Text != "等待返修车间主管审核")
            {
                MessageDialog.ShowPromptMessage("请重新确认单据状态");
            }
            else
            {
                if (UpdateMessage())
                {
                    m_billMessageServer.PassFlowMessage(strBillID,
                            string.Format("{0} 号三包外返修处理单，请制造仓库管理员确认出库", strBillID),
                            CE_RoleEnum.制造仓库管理员.ToString(), true);
                }
            }
        }

        private void 确认出库ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lbDJZT.Text != "等待仓管确认出库")
            {
                MessageDialog.ShowPromptMessage("请重新确认单据状态");
            }
            else
            {
                if (UpdateMessage())
                {
                    m_billMessageServer.PassFlowMessage(strBillID,
                            string.Format("{0} 号三包外返修处理单，请下线车间人员提交返修完成", strBillID),
                            CE_RoleEnum.下线车间人员.ToString(), true);
                }
            }
        }

        private void 返修完成提交ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lbDJZT.Text != "等待返修完成")
            {
                MessageDialog.ShowPromptMessage("请重新确认单据状态");
            }
            else
            {
                if (m_lnqBill.RepairTaskTime == 0)
                {
                    if (MessageBox.Show("返修工时为0，是否继续?", "提示", MessageBoxButtons.YesNo) == DialogResult.No)
                    {
                        return;
                    }
                }

                if (UpdateMessage())
                {
                    m_billMessageServer.PassFlowMessage(strBillID,
                            string.Format("{0} 号三包外返修处理单，请营销人员确认返修完成", strBillID),
                            CE_RoleEnum.营销普通人员.ToString(), true);
                }
            }
        }

        private void 确认返修完成ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lbDJZT.Text != "等待营销确认")
            {
                MessageDialog.ShowPromptMessage("请重新确认单据状态");
            }
            else
            {
                if (UpdateMessage())
                {
                    m_billMessageServer.PassFlowMessage(strBillID,
                            string.Format("{0} 号三包外返修处理单，请营销人员提交营销策略", strBillID),
                            CE_RoleEnum.营销普通人员.ToString(), true);
                }
            }
        }

        private void 提交销售策略ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lbDJZT.Text != "等待销售策略")
            {
                MessageDialog.ShowPromptMessage("请重新确认单据状态");
            }
            else
            {
                if (Convert.ToInt32(m_lnqBill.MarketingStrategy) == 0)
                {
                    if (MessageBox.Show("营销策略设置为0,是否继续?", "提示", MessageBoxButtons.YesNo) == DialogResult.No)
                    {
                        return;
                    }
                }

                if (UpdateMessage())
                {
                    if (m_lnqBill.DJZT == "等待营销主管审核")
                    {
                        m_billMessageServer.PassFlowMessage(strBillID,
                            string.Format("{0} 号三包外返修处理单，请营销总监审核", strBillID),
                            CE_RoleEnum.营销主管.ToString(), true);
                    }

                }
            }
        }

        private void 审核通过ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lbDJZT.Text != "等待营销总监审核")
            {
                MessageDialog.ShowPromptMessage("请重新确认单据状态");
            }
            else
            {
                if (UpdateMessage())
                {
                    m_billMessageServer.PassFlowMessage(strBillID,
                        string.Format("{0} 号三包外返修处理单，请财务确认", strBillID),
                        CE_RoleEnum.会计.ToString(), true);
                }
            }
        }

        private void 财务确认ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lbDJZT.Text != "等待财务确认")
            {
                MessageDialog.ShowPromptMessage("请重新确认单据状态");
            }
            else
            {
                if (UpdateMessage())
                {
                    #region 发送知会消息


                    List<string> noticeRoles = new List<string>();

                    string strDept = m_serverDepartment.GetDeptInfoFromPersonnelInfo(
                    m_threePacketsOfTheRepairServer.GetBill(strBillID, out m_strErr).FoundPersonnel).Rows[0]["DepartmentCode"].ToString();

                    noticeRoles.AddRange(m_billMessageServer.GetDeptDirectorRoleName(strDept));

                    noticeRoles.Add(CE_RoleEnum.营销普通人员.ToString());
                    noticeRoles.Add(CE_RoleEnum.营销总监.ToString());
                    noticeRoles.Add(CE_RoleEnum.下线车间人员.ToString());
                    noticeRoles.Add(CE_RoleEnum.下线主管.ToString());
                    noticeRoles.Add(CE_RoleEnum.会计.ToString());
                    noticeRoles.Add(CE_RoleEnum.制造仓库管理员.ToString());

                    m_billMessageServer.EndFlowMessage(strBillID,
                        string.Format("{0} 号三包外返修处理单已经处理完毕", strBillID),
                        noticeRoles, null);

                    #endregion 发送知会消息

                }
            }
        }

        #endregion

        private void 单据报废ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                return;
            }
            else
            {
                if (dataGridView1.CurrentRow.Cells["单据状态"].Value.ToString().Contains("销")
                    || dataGridView1.CurrentRow.Cells["单据状态"].Value.ToString().Contains("财务")
                    || dataGridView1.CurrentRow.Cells["单据状态"].Value.ToString().Contains("完成")
                    || dataGridView1.CurrentRow.Cells["单据状态"].Value.ToString().Contains("检验"))
                {
                    MessageDialog.ShowPromptMessage("此单据不能删除");
                }
                else
                {

                    if (MessageDialog.ShowEnquiryMessage("您是否要删除此单据") == DialogResult.No)
                    {
                        return;
                    }

                    if (!m_threePacketsOfTheRepairServer.DeleteBill(dataGridView1.CurrentRow.Cells["单据号"].Value.ToString(), out m_strErr))
                    {
                        MessageDialog.ShowPromptMessage(m_strErr);
                    }
                    else
                    {
                        MessageDialog.ShowPromptMessage("删除成功");
                        m_billNoControl.CancelBill(dataGridView1.CurrentRow.Cells["单据号"].Value.ToString());
                        m_billMessageServer.DestroyMessage(dataGridView1.CurrentRow.Cells["单据号"].Value.ToString());
                    }
                }
            }

            ClearMessage();
            RefreshDataGirdView(m_threePacketsOfTheRepairServer.GetAllBill(checkBillDateAndStatus1.cmbBillStatus.Text,
                checkBillDateAndStatus1.dtpStartTime.Value, checkBillDateAndStatus1.dtpEndTime.Value));
        }

        private void btnFindPurpose_Click(object sender, EventArgs e)
        {
            FormQueryInfo dialog = QueryInfoDialog.GetProductCodeStockSearchMode("where b.产品编码 = '" + cmbProductType.Text + "'");

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                txtProductCode.Text = dialog.GetStringDataItem("箱体编号");
            }
        }

        private void 刷新数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RefreshDataGirdView(m_threePacketsOfTheRepairServer.GetAllBill(checkBillDateAndStatus1.cmbBillStatus.Text,
                checkBillDateAndStatus1.dtpStartTime.Value, checkBillDateAndStatus1.dtpEndTime.Value));
        }

        private void 设置返修零件销售单价ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            三包外返修零件单价设置 form = new 三包外返修零件单价设置();
            form.ShowDialog();
        }

        private void 确认清单责任归属ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lbDJZT.Text != "等待确认清单责任归属")
            {
                MessageDialog.ShowPromptMessage("请重新确认单据状态");
            }
            else
            {
                if (UpdateMessage())
                {
                    m_billMessageServer.PassFlowMessage(strBillID,
                            string.Format("{0} 号三包外返修处理单，请下线车间主管审核", strBillID),
                            CE_RoleEnum.下线主管.ToString(), true);
                }
            }
        }

        private void 检验通过ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lbDJZT.Text != "等待质检检验")
            {
                MessageDialog.ShowPromptMessage("请重新确认单据状态");
            }
            else
            {
                if (UpdateMessage())
                {
                    m_billMessageServer.PassFlowMessage(strBillID,
                        string.Format("{0} 号三包外返修处理单，请营销人员确定营销策略", strBillID),
                        CE_RoleEnum.营销普通人员.ToString(), true);
                }
            }
        }

        private void 审核通过ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (lbDJZT.Text != "等待营销主管审核")
            {
                MessageDialog.ShowPromptMessage("请重新确认单据状态");
            }
            else
            {
                if (UpdateMessage())
                {
                    if (Convert.ToDecimal(m_lnqBill.MarketingStrategy) > 50)
                    {
                        m_billMessageServer.PassFlowMessage(strBillID,
                            string.Format("{0} 号三包外返修处理单，请营销总监审核", strBillID),
                            CE_RoleEnum.营销总监.ToString(), true);
                    }
                    else
                    {
                        m_billMessageServer.PassFlowMessage(strBillID,
                            string.Format("{0} 号三包外返修处理单，请财务确认", strBillID),
                            CE_RoleEnum.会计.ToString(), true);
                    }
                }
            }
        }
    }
}

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
    /// 不合格品隔离处置单界面
    /// </summary>
    public partial class 不合格品隔离处置单 : Form
    {
        /// <summary>
        /// 部门服务组件
        /// </summary>
        IDepartmentServer m_serverDepartment = ServerModuleFactory.GetServerModule<IDepartmentServer>();

        /// <summary>
        /// 库存信息服务组件
        /// </summary>
        IStoreServer m_serverStore = ServerModuleFactory.GetServerModule<IStoreServer>();

        /// <summary>
        /// 单据类消息发布器
        /// </summary>
        IBillMessagePromulgatorServer m_billMessageServer = BasicServerFactory.GetServerModule<IBillMessagePromulgatorServer>();

        /// <summary>
        /// 单据编号控制类
        /// </summary>
        BillNumberControl m_billNoControl;

        /// <summary>
        /// 数据集
        /// </summary>
        S_IsolationManageBill m_lnqIslation = new S_IsolationManageBill();

        /// <summary>
        /// 可供查找的所有字段
        /// </summary>
        string[] m_findField = null;

        /// <summary>
        /// 查找条件窗体
        /// </summary>
        FormConditionFind m_formFindCondition;

        /// <summary>
        /// 数据定位控件
        /// </summary>
        UserControlDataLocalizer m_dataLocalizer;

        /// <summary>
        /// 权限标志
        /// </summary>
        AuthorityFlag m_authFlag;

        /// <summary>
        /// 服务类
        /// </summary>
        /// <param name="nodeInfo"></param>
        IIsolationManageBill m_serverIsolation = ServerModuleFactory.GetServerModule<IIsolationManageBill>();

        /// <summary>
        /// 物品信息集
        /// </summary>
        //DataRow m_drMessage;

        /// <summary>
        /// 错误信息
        /// </summary>
        string m_err;

        /// <summary>
        /// 退货处理方法
        /// </summary>
        string m_strRejectMode = null;

        /// <summary>
        /// 确认说明
        /// </summary>
        string m_strAdutingMessage = "";

        public 不合格品隔离处置单(PlatformManagement.FunctionTreeNodeInfo nodeInfo)
        {
            InitializeComponent();

            m_billMessageServer.BillType = "不合格品隔离处置单";

            m_billNoControl = new BillNumberControl(CE_BillTypeEnum.不合格品隔离处置单, m_serverIsolation);

            m_authFlag = nodeInfo.Authority;

            DataTable dt = UniversalFunction.GetStorageTb();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                cmbStorage.Items.Add(dt.Rows[i]["StorageName"].ToString());
            }

            cmbStorage.SelectedIndex = -1;

            string[] strBillStatus = { "全  部", 
                                     "新建单据", 
                                     "等待隔离原因",
                                     "等待处理结果",
                                     "等待审批确认",
                                     "等待质检结果",
                                     "等待质管主管确认",
                                     "等待采购退货",
                                     "单据已完成"};

            checkBillDateAndStatus1.InsertComBox(strBillStatus);

            checkBillDateAndStatus1.OnCompleteSearch +=
                new GlobalObject.DelegateCollection.NonArgumentHandle(checkBillDateAndStatus1_OnCompleteSearch);

            checkBillDateAndStatus1.dtpStartTime.Value = ServerTime.Time.AddDays(1).AddMonths(-1);
            checkBillDateAndStatus1.dtpEndTime.Value = ServerTime.Time.AddDays(1);

            RefreshDataGirdView(m_serverIsolation.GetAllBill(null));
            ClearMessage();
        }

        private void 不合格品隔离处置单_Load(object sender, EventArgs e)
        {
            AuthorityControl(m_authFlag);
            //qE操作ToolStripMenuItem.Visible = false;
        }

        void checkBillDateAndStatus1_OnCompleteSearch()
        {
            刷新数据ToolStripMenuItem_Click(null, null);
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

                    DataTable dtMessage = UniversalFunction.PositioningOneRecord(msg.MessageContent, "不合格品隔离处置单");

                    if (dtMessage.Rows.Count == 0)
                    {
                        m_billMessageServer.DestroyMessage(msg.MessageContent);
                        MessageDialog.ShowPromptMessage("未找到相关记录");
                    }
                    else
                    {
                        RefreshDataGirdView(GlobalObject.DataConverter.DataTableToList<View_S_IsolationManageBill>(dtMessage));
                        
                        dataGridView1.Rows[0].Selected = true;
                    }

                    break;

                default:
                    base.DefWndProc(ref m);
                    break;
            }
        }

        void txtName_OnCompleteSearch()
        {
            txtName.Text = txtName.DataResult["物品名称"].ToString();
            txtName.Tag = txtName.DataResult["序号"].ToString();
            txtCode.Text = txtName.DataResult["图号型号"].ToString();
            txtDepot.Text = txtName.DataResult["物品类别"].ToString();
            txtSpec.Text = txtName.DataResult["规格"].ToString();
            lbdw.Text = "";
        }

        void txtBatchNo_OnCompleteSearch()
        {
            txtBatchNo.Text = txtBatchNo.DataResult["批次号"].ToString();
            txtProvider.Tag = txtBatchNo.DataResult["供应商编码"].ToString();
            txtProvider.Text = txtBatchNo.DataResult["供应商编码"].ToString();
            lbdw.Text = txtBatchNo.DataResult["单位"].ToString();
            txtCount.Text = txtBatchNo.DataResult["库存数量"].ToString();
        }

        /// <summary>
        /// 刷新
        /// </summary>
        /// <param name="source">数据集</param>
        void RefreshDataGirdView(List<View_S_IsolationManageBill> source)
        {
            dataGridView1.DataSource = new BindingCollection<View_S_IsolationManageBill>(source);

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

            if (m_dataLocalizer == null)
            {
                m_dataLocalizer = new UserControlDataLocalizer(dataGridView1, this.Name, 
                    UniversalFunction.SelectHideFields(this.Name, dataGridView1.Name, BasicInfo.LoginID));

                panelPara.Controls.Add(m_dataLocalizer);

                m_dataLocalizer.Dock = DockStyle.Bottom;
            }
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
        /// 清空数据
        /// </summary>
        void ClearMessage()
        {
            chkIsOutsourcing.Checked = false;
            cmbStorage.SelectedIndex = -1;
            cmbStorage.Tag = "";
            txtBatchNo.Text = "";
            txtCode.Text = "";
            txtCount.Text = "0";
            txtDepot.Text = "";
            txtName.Text = "";
            txtProvider.Text = "";
            txtReason.Text = "";
            txtMeansAndAsk.Text = "";
            txtName.Tag = -1;
            txtProvider.Tag = "";

            NudQC_BF.Value = 0;
            NudQC_HG.Value = 0;
            NudQC_RB.Value = 0;
            NudQC_TH.Value = 0;
            NudQC_FQ.Value = 0;
            NudSQE_BHG.Value = 0;
            NudSQE_HG.Value = 0;
            NudSQE_TFGS.Value = 0;

            lbDJZT.Text = "";
            lbdw.Text = "单位";
            lbJYJG.Text = "";
            lbSH.Text = "";
            lbDJH.Text = "";
            lbBZ.Text = "";
            lbCLJG.Text = "";
            lbDC.Text = "";
            lbDR.Text = "";
            lbQE.Text = "";

        }

        /// <summary>
        /// 数据填充
        /// </summary>
        void GetMessage()
        {
            m_lnqIslation.DJH = lbDJH.Text;
            m_lnqIslation.DJZT = lbDJZT.Text;
            m_lnqIslation.GoodsID = Convert.ToInt32( txtName.Tag);
            m_lnqIslation.Provider = txtProvider.Text;
            m_lnqIslation.BatchNo = txtBatchNo.Text;
            m_lnqIslation.StorageID = UniversalFunction.GetStorageID(cmbStorage.Text);
            m_lnqIslation.IsolateReason = txtReason.Text;
            m_lnqIslation.IsolateMeansAndAsk = txtMeansAndAsk.Text;
            m_lnqIslation.QC_BFS = NudQC_BF.Value;
            m_lnqIslation.QC_HGS = NudQC_HG.Value;
            m_lnqIslation.QC_RBS = NudQC_RB.Value;
            m_lnqIslation.QC_THS = NudQC_TH.Value;
            m_lnqIslation.QC_FQS = NudQC_FQ.Value;
            m_lnqIslation.SQE_BHGS = NudSQE_BHG.Value;
            m_lnqIslation.SQE_HGS = NudSQE_HG.Value;
            m_lnqIslation.SQE_CLGS = NudSQE_TFGS.Value;
            m_lnqIslation.CLBM = lbCLBM.Tag.ToString();
            m_lnqIslation.QRSM = m_strAdutingMessage;
            m_lnqIslation.Amount = Convert.ToDecimal(m_serverStore.GetGoodsStockInfo(Convert.ToInt32(m_lnqIslation.GoodsID),
                                 m_lnqIslation.BatchNo, "", m_lnqIslation.StorageID).Rows[0]["ExistCount"]);

            m_lnqIslation.IsOutsourcing = chkIsOutsourcing.Checked;

            CheckRejectMode();
            m_lnqIslation.RejectMode = m_strRejectMode;
           
        }

        /// <summary>
        /// 提交信息
        /// </summary>
        /// <param name="needBillStatus">要求的单据状态</param>
        bool DataForMessage(string needBillStatus)
        {
            GetMessage();

            if (m_serverIsolation.UpdateBill(needBillStatus, m_lnqIslation,out m_err))
            {
                if (needBillStatus == "等待仓管调入")
                {
                    m_billNoControl.UseBill(m_lnqIslation.DJH);
                }

                MessageBox.Show("成功提交！","提示");
            }
            else
            {
                MessageDialog.ShowErrorMessage(m_err);
                return false;
            }

            RefreshDataGirdView(m_serverIsolation.GetAllBill(null));

            return true;
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
        /// 检查退货方式
        /// </summary>
        void CheckRejectMode()
        {
            if (rbJDBF.Checked)
            {
                m_strRejectMode = rbJDBF.Text;
                return;
            }

            if (rbTGYS.Checked)
            {
                m_strRejectMode = rbTGYS.Text;
                return;
            }
        }

        /// <summary>
        /// 设置radio退货方式
        /// </summary>
        void SetRejectMode()
        {
            rbJDBF.Checked = false;
            rbTGYS.Checked = false;

            if (m_strRejectMode.ToString() == rbJDBF.Text)
            {
                rbJDBF.Checked = true;
                return;
            }

            if (m_strRejectMode.ToString() == rbTGYS.Text)
            {
                rbTGYS.Checked = true;
                return;
            }
        }

        private void 新建单据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClearMessage();
            lbDJH.Text = m_billNoControl.GetNewBillNo();
            lbDJZT.Text = "新建单据";
            txtBatchNo.ShowResultForm = true;
            txtName.ShowResultForm = true;
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.RowCount == 0)
            {
                return;
            }

            #region 信息区显示

            BindingCollection<View_S_IsolationManageBill> dataSource = dataGridView1.DataSource as BindingCollection<View_S_IsolationManageBill>;

            View_S_IsolationManageBill bill = dataSource.First(p => p.单据号 == dataGridView1.CurrentRow.Cells["单据号"].Value.ToString());

            View_S_Stock stockInfo = m_serverStore.GetGoodsStockInfoView((int)bill.物品ID, bill.批次号, bill.供货单位, bill.库房代码);

            if (stockInfo == null)
            {
                return;
            }

            cmbStorage.Tag = stockInfo.库房代码;
            cmbStorage.Text = UniversalFunction.GetStorageName(stockInfo.库房代码);
            txtName.Text = stockInfo.物品名称;
            txtName.Tag = Convert.ToInt32(stockInfo.物品ID);
            txtCode.Text = stockInfo.图号型号;
            txtSpec.Text = stockInfo.规格;
            txtDepot.Text = stockInfo.材料类别编码;
            txtCount.Text = bill.隔离数量.ToString();
            txtProvider.Text = stockInfo.供货单位;
            txtBatchNo.Text = stockInfo.批次号;
            lbdw.Text = stockInfo.单位;

            #endregion 

            #region 单据内容

            txtReason.Text = bill.隔离原因;
            txtMeansAndAsk.Text = bill.隔离方法和要求;
            chkIsOutsourcing.Checked = bill.是否委外返修;
            m_strRejectMode = bill.退货方式 == null ? "" : bill.退货方式;
            lbCLBM.Tag = bill.处理部门代码;
            lbCLBM.Text = bill.处理部门;

            NudQC_FQ.Value = bill.QC废弃数;
            NudQC_BF.Value = bill.QC报废数;
            NudQC_HG.Value = bill.QC合格数;
            NudQC_RB.Value = bill.QC让步数;
            NudQC_TH.Value = bill.QC退货数;
            NudSQE_BHG.Value = bill.不合格数;
            NudSQE_HG.Value = bill.合格数;
            NudSQE_TFGS.Value = bill.处理工时;

            #endregion

            #region 单据信息

            lbDJH.Text = bill.单据号;
            lbDJZT.Text = bill.单据状态;
            lbBZ.Text = bill.编制人;
            lbSH.Text = bill.审核人;
            lbDC.Text = bill.调出人;
            lbCLJG.Text = bill.处理人;
            lbJYJG.Text = bill.检验人;
            lbDR.Text = bill.调入人;
            lbQE.Text = bill.QE人员;

            #endregion

            SetRejectMode();

            仓管员操作ToolStripMenuItem.Visible =
                UniversalFunction.CheckStorageAndPersonnel(bill.库房代码);

            if (lbCLBM.Tag == null || !BasicInfo.DeptCode.Contains(lbCLBM.Tag.ToString()))
            {
                sQE操作ToolStripMenuItem.Visible = false;
                处理结果提交ToolStripMenuItem.Visible = false;
            }
            else
            {
                sQE操作ToolStripMenuItem.Visible = true;
                处理结果提交ToolStripMenuItem.Visible = true;
            }

            if (lbDJZT.Text == "新建单据" || lbDJZT.Text == "等待主管审核" || lbDJZT.Text == "等待仓管调出")
            {
                txtBatchNo.ShowResultForm = true;
                txtName.ShowResultForm = true;
            }
            else
            {
                txtBatchNo.ShowResultForm = false;
                txtName.ShowResultForm = false;
            }
        }

        private void 刷新数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RefreshDataGirdView(m_serverIsolation.GetAllBill(checkBillDateAndStatus1.GetSqlString("编制日期", "单据状态")));
            ClearMessage();
        }

        private void 编制信息提交ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lbDJZT.Text == "新建单据" || lbDJZT.Text == "等待主管审核" || lbDJZT.Text == "等待仓管调出")
            {
                不合格品处理部门 form = new 不合格品处理部门();
                form.ShowDialog();

                if (!form.BlFlag)
                {
                    MessageDialog.ShowPromptMessage("请选择要求的处理部门");
                    return;
                }
                else
                {
                    lbCLBM.Tag = form.StrCLBM;
                    lbCLBM.Text = m_serverDepartment.GetDepartmentName(form.StrCLBM);
                }

                GetMessage();

                if (m_lnqIslation.GoodsID.ToString().Trim() == "" 
                    || m_lnqIslation.StorageID.ToString().Trim() == "")
                {
                    MessageBox.Show("请完整填写信息区的信息","提示");
                    return;
                }

                if (m_serverIsolation.UpdateBill(m_lnqIslation, true, out m_err))
                {
                    MessageBox.Show("成功提交！", "提示");

                    m_billMessageServer.DestroyMessage(m_lnqIslation.DJH);
                    m_billMessageServer.SendNewFlowMessage(m_lnqIslation.DJH,
                        string.Format("{0}号不合格品隔离处置单已提交，请等待质量工程师",
                        m_lnqIslation.DJH), CE_RoleEnum.质量工程师);
                }
                else
                {
                    MessageDialog.ShowErrorMessage(m_err);
                }

                RefreshDataGirdView(m_serverIsolation.GetAllBill(null));
            }
            else
            {
                MessageBox.Show("请重新确认单据状态！","提示");
            }

            PositioningRecord(m_lnqIslation.DJH);
        }

        private void 单据审核ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lbDJZT.Text == "等待主管审核")
            {
                DataForMessage("等待主管审核");

                m_billMessageServer.PassFlowMessage(m_lnqIslation.DJH,
                    string.Format("{0}号不合格品隔离处置单已提交，请等待仓管调出",
                    m_lnqIslation.DJH), m_billMessageServer.GetRoleStringForStorage(cmbStorage.Text).ToString(), true);
            }
            else
            {
                MessageBox.Show("请重新确认单据状态", "提示");
            }

            PositioningRecord(m_lnqIslation.DJH);
        }

        private void 确认隔离ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lbDJZT.Text == "等待仓管调出")
            {
                DataForMessage("等待仓管调出");

                m_billMessageServer.PassFlowMessage(m_lnqIslation.DJH,
                    string.Format("{0}号不合格品隔离处置单已提交，请等待处理",
                    m_lnqIslation.DJH), BillFlowMessage_ReceivedUserType.角色, 
                    m_billMessageServer.GetDeptDirectorRoleName(m_lnqIslation.CLBM).ToList());
            }
            else
            {
                MessageBox.Show("请重新确认单据状态", "提示");
            }

            PositioningRecord(m_lnqIslation.DJH);
        }

        private void 处理结果提交ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lbDJZT.Text == "等待处理结果")
            {
                if (Convert.ToDecimal(txtCount.Text) != (decimal)NudSQE_BHG.Value + (decimal)NudSQE_HG.Value)
                {
                    MessageBox.Show("【合格数 + 不合格数 = 挑选数】 请重新确认", "提示");
                    return;
                }

                if ((decimal)NudSQE_BHG.Value  > 0 
                    && !rbJDBF.Checked 
                    && !rbTGYS.Checked)
                {
                    MessageDialog.ShowPromptMessage("请选择处理方式");
                    return;
                }

                DataForMessage("等待处理结果");

                m_billMessageServer.PassFlowMessage(m_lnqIslation.DJH,
                    string.Format("{0}号不合格品隔离处置单已提交，请检验员检验",
                    m_lnqIslation.DJH), CE_RoleEnum.检验员.ToString(), true);
            }
            else
            {
                MessageBox.Show("请重新确认单据状态", "提示");
            }

            PositioningRecord(m_lnqIslation.DJH);
        }

        private void 检验结果提交ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lbDJZT.Text == "等待质检结果")
            {
                //if (NudQC_RB.Value > 0 || NudQC_TH.Value > 0)
                //{
                //    不合格品信息 form = new 不合格品信息(lbDJH.Text);
                //    form.ShowDialog();

                //    if (!form.BlFlag)
                //    {
                //        MessageBox.Show("请完整填写不合格信息单，并且保存！", "提示");
                //        return;
                //    }
                //}

                if (Convert.ToDecimal(txtCount.Text) !=(decimal)NudQC_BF.Value 
                    + (decimal)NudQC_HG.Value
                    + (decimal)NudQC_RB.Value
                    + (decimal)NudQC_TH.Value
                    + (decimal)NudQC_FQ.Value)
                {
                    MessageBox.Show("【合格数 + 让步数 + 退货数 + 报废数 + 废弃数 = 挑选数】 请重新确认", "提示");
                    return;
                }

                DataForMessage("等待质检结果");

                if ((decimal)NudQC_TH.Value > 0)
                {
                    m_billMessageServer.PassFlowMessage(m_lnqIslation.DJH,
                        string.Format("{0}号不合格品隔离处置单已提交，请采购员退货",
                        m_lnqIslation.DJH), CE_RoleEnum.采购员.ToString(), true);
                }
                else
                {
                    m_billMessageServer.PassFlowMessage(m_lnqIslation.DJH,
                        string.Format("{0}号不合格品隔离处置单已提交，请质控主管确认",
                        m_lnqIslation.DJH), CE_RoleEnum.质控主管.ToString(), true);
                }
            }
            else
            {
                MessageBox.Show("请重新确认单据状态", "提示");
            }

            PositioningRecord(m_lnqIslation.DJH);
        }

        private void 确认调入ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lbDJZT.Text == "等待仓管调入")
            {
                DataForMessage("等待仓管调入");

                #region 发送知会消息

                List<string> noticeRoles = new List<string>();

                noticeRoles.Add(CE_RoleEnum.质量工程师.ToString());
                noticeRoles.Add(CE_RoleEnum.质控主管.ToString());
                noticeRoles.Add(m_billMessageServer.GetRoleStringForStorage(cmbStorage.Text).ToString());
                noticeRoles.Add(CE_RoleEnum.检验员.ToString());
                noticeRoles.Add(m_billMessageServer.GetDeptDirectorRoleName(m_lnqIslation.CLBM)[0].ToString());

                m_billMessageServer.EndFlowMessage(m_lnqIslation.DJH,
                    string.Format("{0} 号不合格品隔离处置单已经处理完毕", m_lnqIslation.DJH),
                    noticeRoles, null);

                #endregion 发送知会消息

            }
            else
            {
                MessageBox.Show("请重新确认单据状态", "提示");
            }

            PositioningRecord(m_lnqIslation.DJH);
        }

        private void 单据报废ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (BasicInfo.LoginName != dataGridView1.CurrentRow.Cells["编制人"].Value.ToString())
            {
                MessageDialog.ShowPromptMessage("您不是此记录的编制者无法进行此操作");
                return;
            }

            if (dataGridView1.CurrentRow.Cells["单据号"].Value.ToString() == "")
            {
                MessageDialog.ShowPromptMessage("请选择需要删除的记录");
                return;
            }

            string strDJZT = UniversalFunction.GetBillStatus("S_IsolationManageBill", "DJZT", "DJH",
                    dataGridView1.CurrentRow.Cells["单据号"].Value.ToString());

            string strBillNo = dataGridView1.CurrentRow.Cells["单据号"].Value.ToString();

            if (strDJZT != "单据已完成")
            {
                if (MessageDialog.ShowEnquiryMessage("您是否要删除此单据") == DialogResult.No)
                {
                    return;
                }

                if (m_serverIsolation.ScrapBill(strBillNo, out m_err))
                {
                    m_billNoControl.CancelBill(strBillNo);
                    MessageBox.Show("报废成功", "提示");
                    m_billMessageServer.DestroyMessage(strBillNo);
                }
                else
                {
                    MessageDialog.ShowErrorMessage(m_err);
                }

                RefreshDataGirdView(m_serverIsolation.GetAllBill(null));
            }
        }

        private void 打印单据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow.Cells["单据状态"].Value.ToString() != "单据已完成")
            {
                MessageDialog.ShowPromptMessage("请选择已确认的记录后再进行此操作");
                return;
            }

            IBillReportInfo report = new 报表_隔离单(dataGridView1.CurrentRow.Cells["单据号"].Value.ToString(), labelTitle.Text);
            PrintReportBill print = new PrintReportBill(21,29.7, report);
            print.DirectPrintReport();
        }

        private void 导出EXCL表单ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_formFindCondition != null && !m_formFindCondition.SaveFlag)
            {
                m_formFindCondition = null;
            }

            if (m_formFindCondition == null)
            {
                m_formFindCondition = new FormConditionFind(this, m_findField, labelTitle.Text, labelTitle.Text);
            }

            m_formFindCondition.ShowDialog();
        }

        private void 回退单据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lbDJZT.Text == "等待主管审核" && qE主管操作ToolStripMenuItem.Visible == true)
            {
                ReturnBillStatus();
            }

            if (lbDJZT.Text == "等待处理结果" && sQE操作ToolStripMenuItem.Visible == true)
            {
                ReturnBillStatus();
            }

            if (lbDJZT.Text == "等待质检结果" && qE操作ToolStripMenuItem1.Visible == true)
            {
                ReturnBillStatus();
            }

            if (lbDJZT.Text == "等待质管主管确认" && qE主管操作ToolStripMenuItem.Visible == true)
            {
                ReturnBillStatus();
            }

            if (lbDJZT.Text == "等待仓管调出" && 仓管员操作ToolStripMenuItem.Visible == true)
            {
                ReturnBillStatus();
            }

            if ((lbDJZT.Text == "等待采购退货" || lbDJZT.Text == "等待仓管调入" || lbDJZT.Text == "等待领料") && 仓管员操作ToolStripMenuItem.Visible == true)
            {
                ReturnBillStatus();
            }
        }

        private void ReturnBillStatus()
        {
            if (lbDJZT.Text != "已完成")
            {
                回退单据 form = new 回退单据(CE_BillTypeEnum.不合格品隔离处置单, lbDJH.Text, lbDJZT.Text);

                if (form.ShowDialog() == DialogResult.OK)
                {
                    if (MessageBox.Show("您确定要回退此单据吗？", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        GetMessage();

                        if (m_serverIsolation.ReturnBill(form.StrBillID,
                            form.StrBillStatus, m_lnqIslation, out m_err, form.Reason))
                        {
                            MessageDialog.ShowPromptMessage("回退成功");

                        }
                        else
                        {
                            MessageDialog.ShowPromptMessage(m_err);
                        }
                    }

                    RefreshDataGirdView(m_serverIsolation.GetAllBill(null));
                    PositioningRecord(form.StrBillID);
                }
            }
            else
            {
                MessageDialog.ShowPromptMessage("请重新确认单据状态");
            }
        }

        private void txtName_Enter(object sender, EventArgs e)
        {
            string strSql = "";
            string strStorage = UniversalFunction.GetStorageID(cmbStorage.Text);

            if (strStorage == null)
            {
                strSql += " and 库房代码 is null ";
            }
            else
            {
                strSql += " and 库房代码 = '" + strStorage + "'";
            }

            txtName.StrEndSql = strSql;
        }

        private void txtBatchNo_Enter(object sender, EventArgs e)
        {
            string strSql = "";

            strSql = " and 库存数量 <> 0  and 物品状态 not in('报废','隔离') and 物品ID = " + Convert.ToInt32(txtName.Tag);

            string strStorage = UniversalFunction.GetStorageID(cmbStorage.Text);

            if (strStorage == null)
            {
                strSql += " and 库房代码 is null ";
            }
            else
            {
                strSql += " and 库房代码 = '" + strStorage + "'";
            }

            txtBatchNo.StrEndSql = strSql;
        }

        private void 确认解除隔离ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lbDJZT.Text == "等待质管主管确认")
            {
                string strTxt = "\n\n图号型号：" + dataGridView1.CurrentRow.Cells["图号型号"].Value.ToString()
                    + "\n\n物品名称：" + dataGridView1.CurrentRow.Cells["物品名称"].Value.ToString()
                    + "\n\n规格：" + dataGridView1.CurrentRow.Cells["规格"].Value.ToString()
                    + "\n\n批次号：" + dataGridView1.CurrentRow.Cells["批次号"].Value.ToString();

                不合格品隔离处置单质管主管确认窗体 form = new 不合格品隔离处置单质管主管确认窗体(strTxt);
                form.ShowDialog();

                if (form.DialogResult == DialogResult.OK)
                {
                    m_strAdutingMessage = form.StrRemark;

                    if (form.BlFlag)
                    {
                        if (DataForMessage("等待质管主管确认"))
                        {
                            if (m_lnqIslation.QC_THS > 0)
                            {
                                m_billMessageServer.PassFlowMessage(m_lnqIslation.DJH,
                                    string.Format("{0}号不合格品隔离处置单已提交，请采购员退货",
                                    m_lnqIslation.DJH), CE_RoleEnum.采购员.ToString(), true);
                            }
                            else
                            {
                                string strMsg = 
                                    string.Format("{0}号不合格品隔离处置单已提交,请仓管调入", m_lnqIslation.DJH.ToString()); 
                                m_billMessageServer.PassFlowMessage(m_lnqIslation.DJH, strMsg,
                                    m_billMessageServer.GetRoleStringForStorage(m_lnqIslation.StorageID).ToString(), true);
                            }
                        }
                    }
                    else
                    {
                        if (!m_serverIsolation.AffrimBill(dataGridView1.CurrentRow.Cells["单据号"].Value.ToString(),
                            form.StrRemark,out m_err))
                        {
                            MessageDialog.ShowPromptMessage(m_err);
                        }
                        else
                        {
                            MessageBox.Show("成功提交！", "提示");

                            m_billMessageServer.PassFlowMessage(m_lnqIslation.DJH,
                                string.Format("{0}号不合格品隔离处置单已提交，请等待处理",m_lnqIslation.DJH),
                                BillFlowMessage_ReceivedUserType.角色,
                                m_billMessageServer.GetDeptDirectorRoleName(m_lnqIslation.CLBM).ToList());
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("请重新确认单据状态", "提示");
            }

            PositioningRecord(m_lnqIslation.DJH);
        }

        private void 批量填写ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            批量零件隔离 form = new 批量零件隔离();
            form.ShowDialog();

            RefreshDataGirdView(m_serverIsolation.GetAllBill(null));
            ClearMessage();
        }

        private void 提交ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lbDJZT.Text == "等待隔离原因")
            {
                if (txtReason.Text.Trim() == ""|| txtMeansAndAsk.Text.Trim() == "")
                {
                    MessageBox.Show("请完整填写隔离信息", "提示");
                    return;
                }

                DataForMessage("等待隔离原因");

                if (m_lnqIslation.IsOutsourcing)
                {
                    m_billMessageServer.PassFlowMessage(m_lnqIslation.DJH,
                        string.Format("{0}号不合格品隔离处置单已提交，请等待采购领料",
                        m_lnqIslation.DJH), CE_RoleEnum.采购员.ToString() ,true);
                }
                else
                {
                    m_billMessageServer.PassFlowMessage(m_lnqIslation.DJH,
                        string.Format("{0}号不合格品隔离处置单已提交，请等待质管主管审核",
                        m_lnqIslation.DJH), BillFlowMessage_ReceivedUserType.角色, m_billMessageServer.GetDeptDirectorRoleName(BasicInfo.DeptCode).ToList());
                }
            }
            else
            {
                MessageBox.Show("请重新确认单据状态", "提示");
            }

            PositioningRecord(m_lnqIslation.DJH);
        }

        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            Rectangle rectangle = new Rectangle(e.RowBounds.Location.X,
                e.RowBounds.Location.Y,
                dataGridView1.RowHeadersWidth - 4,
                e.RowBounds.Height);

            TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(),
                dataGridView1.RowHeadersDefaultCellStyle.Font,
                rectangle,
                dataGridView1.RowHeadersDefaultCellStyle.ForeColor,
                TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
        }
    }
}

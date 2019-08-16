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
    /// 样品确认单界面
    /// </summary>
    public partial class 样品确认单 : Form
    {
        /// <summary>
        /// 部门服务组件
        /// </summary>
        IDepartmentServer m_serverDepartment = ServerModuleFactory.GetServerModule<IDepartmentServer>();

        /// <summary>
        /// 单据类消息发布器
        /// </summary>
        IBillMessagePromulgatorServer m_billMessageServer = BasicServerFactory.GetServerModule<IBillMessagePromulgatorServer>();

        /// <summary>
        /// 单据编号控制类
        /// </summary>
        BillNumberControl m_billNoControl;

        /// <summary>
        /// 可供查找的所有字段
        /// </summary>
        string[] m_findField = null;

        /// <summary>
        /// 查找条件窗体
        /// </summary>
        //FormConditionFind m_formFindCondition;

        /// <summary>
        /// 确认单服务
        /// </summary>
        IMusterAffirmBill m_serverMuster = ServerModuleFactory.GetServerModule<IMusterAffirmBill>();

        /// <summary>
        /// 错误信息
        /// </summary>
        string m_err;

        /// <summary>
        /// 全局TABLE数据集
        /// </summary>
        DataTable m_dtData = new DataTable();

        /// <summary>
        /// 数据集
        /// </summary>
        S_MusterAffirmBill m_lnqAffirm = new S_MusterAffirmBill();

        /// <summary>
        /// 权限标志
        /// </summary>
        AuthorityFlag m_authFlag;

        /// <summary>
        /// 单据号
        /// </summary>
        string m_strDJH = "";

        public 样品确认单(PlatformManagement.FunctionTreeNodeInfo nodeInfo)
        {
            InitializeComponent();

            m_billMessageServer.BillType = "样品确认申请单";

            m_billNoControl = new BillNumberControl(CE_BillTypeEnum.样品确认申请单, m_serverMuster);

            m_authFlag = nodeInfo.Authority;

            dtpStart.Value = ServerTime.Time.AddYears(-1);
            dtpEnd.Value = ServerTime.Time.AddDays(1);

            m_dtData = m_serverMuster.GetAllBill();

            foreach (Control cl in this.groupBox1.Controls)
            {
                if (cl is CheckBox)
                {
                    ((CheckBox)cl).Checked = true;
                    ((CheckBox)cl).CheckedChanged += new EventHandler(RefrshDate);
                }
            }

            RefrshDate(null, null);

            dtpStart.ValueChanged += new EventHandler(RefrshDate);
            dtpEnd.ValueChanged += new EventHandler(RefrshDate);
        }

        private void 样品确认单_Load(object sender, EventArgs e)
        {
            AuthorityControl(m_authFlag);

            申请人操作ToolStripMenuItem.Visible = false;
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

                    DataTable dtMessage = UniversalFunction.PositioningOneRecord(msg.MessageContent, "样品确认申请单");

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
        /// 提交数据
        /// </summary>
        private bool UpdateForBill()
        {
            string strBillID = dataGridView1.CurrentRow.Cells["单据号"].Value.ToString();
            m_lnqAffirm.DJH = strBillID;

            if (m_lnqAffirm.DJZT != "单据已完成" && m_lnqAffirm.DJZT != "单据已报废")
            {
                if (!m_serverMuster.UpdateBill(strBillID, out m_err))
                {
                    MessageDialog.ShowErrorMessage(m_err);
                    return false;
                }
                else
                {
                    if (dataGridView1.CurrentRow.Cells["单据状态"].Value.ToString() == "等待仓管确认入库")
                    {
                        m_billNoControl.UseBill(strBillID);
                    }

                    MessageBox.Show("成功提交", "提示");
                }
            }
            else
            {
                MessageBox.Show("请重新确认单据状态", "提示");
                return false;
            }

            m_dtData = m_serverMuster.GetAllBill();

            RefrshDate(null, null);
            PositioningRecord(strBillID);

            return true;
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
        }

        /// <summary>
        /// 回退单据
        /// </summary>
        private void ReturnBillStatus()
        {
            if (dataGridView1.CurrentRow.Cells["单据状态"].Value.ToString() != "已出库")
            {
                回退单据 form = new 回退单据(CE_BillTypeEnum.样品确认申请单,
                    dataGridView1.CurrentRow.Cells["单据号"].Value.ToString(),
                    dataGridView1.CurrentRow.Cells["单据状态"].Value.ToString());

                if (form.ShowDialog() == DialogResult.OK)
                {
                    if (MessageBox.Show("您确定要回退此单据吗？", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        if (m_serverMuster.ReturnBill(form.StrBillID,
                            form.StrBillStatus, out m_err, form.Reason))
                        {
                            MessageDialog.ShowPromptMessage("回退成功");
                        }
                        else
                        {
                            MessageDialog.ShowPromptMessage(m_err);
                        }

                        m_dtData = m_serverMuster.GetAllBill();

                        foreach (Control cl in this.groupBox1.Controls)
                        {
                            if (cl is CheckBox)
                            {
                                ((CheckBox)cl).Checked = true;
                            }
                        }

                        RefreshDataGirdView(m_dtData);
                    }
                }
            }
            else
            {
                MessageDialog.ShowPromptMessage("请重新确认单据状态");
            }
        }

        /// <summary>
        /// 刷新数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void RefrshDate(object sender, EventArgs e)
        {
            string strDJZT = GetCheckString();

            DataTable dt = m_dtData.Clone();

            if (strDJZT != "")
            {
                DataRow[] dr = m_dtData.Select("单据状态 in (" + strDJZT + ") and 创建日期 >= '"
                    + dtpStart.Value + "' and  创建日期 <= '" + dtpEnd.Value + "'");

                for (int i = 0; i < dr.Length; i++)
                {
                    dt.ImportRow(dr[i]);
                }

                RefreshDataGirdView(dt);
            }
            else
            {
                RefreshDataGirdView(null);
            }

        }

        /// <summary>
        /// 获得查找的字符串
        /// </summary>
        /// <returns>返回查找的字符串</returns>
        string GetCheckString()
        {
            string strCheck = "";

            foreach (Control cl in this.groupBox1.Controls)
            {
                if (cl is CheckBox)
                {
                    if (((CheckBox)cl).Checked == true)
                    {
                        strCheck += "'" + ((CheckBox)cl).Text + "',";
                    }
                }
            }

            if (strCheck == "")
            {
                return strCheck;
            }
            else
            {
                return strCheck.Remove(strCheck.Length - 1);
            }
        }

        private void 新建单据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            样品确认申请单清单 form = new 样品确认申请单清单("");
            form.ShowDialog();

            m_lnqAffirm = form.LnqMuster;

            m_dtData = m_serverMuster.GetAllBill();

            RefrshDate(null, null);

            if (m_lnqAffirm == null || m_lnqAffirm.DJH == null)
            {
                return;
            }

            PositioningRecord(m_lnqAffirm.DJH);
        }

        private void 提交单据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow.Cells["单据状态"].Value.ToString() == "新建单据")
            {

                if (!UpdateForBill())
                {
                    return;
                }

                m_billMessageServer.DestroyMessage(m_lnqAffirm.DJH);

                if (Convert.ToDecimal(m_lnqAffirm.SendCount) > 50)
                {
                    m_billMessageServer.SendNewFlowMessage(m_lnqAffirm.DJH,
                        string.Format("{0} 号样品确认申请单，请质管主管审核", m_lnqAffirm.DJH),
                        CE_RoleEnum.质控主管);
                }
                else
                {
                    if (m_lnqAffirm.IsOutsourcing && m_lnqAffirm.IsIncludeRawMaterial)
                    {
                        m_billMessageServer.SendNewFlowMessage(m_lnqAffirm.DJH,
                            string.Format("{0} 号样品确认申请单，请财务确认", m_lnqAffirm.DJH),
                            CE_RoleEnum.会计);
                    }
                    else
                    {
                        m_billMessageServer.SendNewFlowMessage(m_lnqAffirm.DJH,
                            string.Format("{0} 号样品确认申请单，请仓管确认到货", m_lnqAffirm.DJH),
                            CE_RoleEnum.样品库管理员);
                    }
                }
            }
            else
            {
                MessageDialog.ShowErrorMessage("请重新确认单据状态");
            }
        }

        private void 检验提交ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow.Cells["单据状态"].Value.ToString() == "等待检验")
            {
                if (!UpdateForBill())
                {
                    return;
                }

                List<string> listRoles = new List<string>();

                listRoles.Add(CE_RoleEnum.SQE组员.ToString());
                listRoles.Add(CE_RoleEnum.质量工程师.ToString());

                m_billMessageServer.PassFlowMessage(m_lnqAffirm.DJH,
                    string.Format("{0} 号样品确认申请单，请SQE/质量提交信息", m_lnqAffirm.DJH), BillFlowMessage_ReceivedUserType.角色,
                   listRoles, null);
            }
            else
            {
                MessageDialog.ShowErrorMessage("请重新确认单据状态");
            }
        }

        private void sQE提交ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow.Cells["单据状态"].Value.ToString() == "等待确认检验信息")
            {
                if (!UpdateForBill())
                {
                    return;
                }

                if (m_lnqAffirm.IsBlank)
                {
                    m_billMessageServer.PassFlowMessage(m_lnqAffirm.DJH,
                        string.Format("{0} 号样品确认申请单，请工艺人员进行评审", m_lnqAffirm.DJH),
                        CE_RoleEnum.工艺人员.ToString(), true);
                }
                else
                {
                    m_billMessageServer.PassFlowMessage(m_lnqAffirm.DJH,
                        string.Format("{0} 号样品确认申请单，请零件工程师进行评审", m_lnqAffirm.DJH),
                        CE_RoleEnum.零件工程师.ToString(), true);
                }
            }
            else
            {
                MessageDialog.ShowErrorMessage("请重新确认单据状态");
            }
        }

        private void 提交评审ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow.Cells["单据状态"].Value.ToString() == "等待工艺工程师评审")
            {
                if (!UpdateForBill())
                {
                    return;
                }


                m_billMessageServer.PassFlowMessage(m_lnqAffirm.DJH,
                    string.Format("{0} 号样品确认申请单，请零件工程师进行评审", m_lnqAffirm.DJH),
                    CE_RoleEnum.零件工程师.ToString(), true);
            }
            else
            {
                MessageDialog.ShowErrorMessage("请重新确认单据状态");
            }
        }

        private void 确认结果提交ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow.Cells["单据状态"].Value.ToString() == "等待项目经理确认")
            {
                if (!UpdateForBill())
                {
                    return;
                }

                m_billMessageServer.PassFlowMessage(m_lnqAffirm.DJH,
                    string.Format("{0} 号样品确认申请单，请等待试验结果", m_lnqAffirm.DJH),
                    CE_RoleEnum.零件工程师.ToString(), true);
            }
            else
            {
                MessageDialog.ShowErrorMessage("请重新确认单据状态");
            }
        }

        private void 提交试验结果ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow.Cells["单据状态"].Value.ToString() == "等待试验结果")
            {
                if (!UpdateForBill())
                {
                    return;
                }

                m_billMessageServer.PassFlowMessage(m_lnqAffirm.DJH,
                    string.Format("{0} 号样品确认申请单，请主管确认", m_lnqAffirm.DJH),
                    CE_RoleEnum.产品开发主管.ToString(), true);
            }
            else
            {
                MessageDialog.ShowErrorMessage("请重新确认单据状态");
            }
        }

        private void 提交确认信息ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow.Cells["单据状态"].Value.ToString() == "等待主管确认")
            {
                if (!UpdateForBill())
                {
                    return;
                }

                m_billMessageServer.PassFlowMessage(m_lnqAffirm.DJH,
                    string.Format("{0} 号样品确认申请单，请SQE处理", m_lnqAffirm.DJH),
                    CE_RoleEnum.SQE组员.ToString(), true);
            }
            else
            {
                MessageDialog.ShowErrorMessage("请重新确认单据状态");
            }
        }

        private void sQE处理结果提交ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow.Cells["单据状态"].Value.ToString() == "等待SQE处理")
            {
                if (!UpdateForBill())
                {
                    return;
                }

                m_billMessageServer.PassFlowMessage(m_lnqAffirm.DJH,
                    string.Format("{0} 号样品确认申请单，请仓管确认入库", m_lnqAffirm.DJH),
                    m_billMessageServer.GetRoleStringForStorage(m_lnqAffirm.StorageID).ToString(), true);
            }
            else
            {
                MessageDialog.ShowErrorMessage("请重新确认单据状态");
            }
        }

        private void 确认信息ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow.Cells["单据状态"].Value.ToString() == "等待仓管确认入库")
            {
                m_lnqAffirm.LayerNumber = m_lnqAffirm.LayerNumber == null ? "" : m_lnqAffirm.LayerNumber;
                m_lnqAffirm.ColumnNumber = m_lnqAffirm.ColumnNumber == null ? "" : m_lnqAffirm.ColumnNumber;
                m_lnqAffirm.ShelfArea = m_lnqAffirm.ShelfArea == null ? "" : m_lnqAffirm.ShelfArea;

                if (!UpdateForBill())
                {
                    return;
                }

                #region 发送知会消息

                List<string> noticeRoles = new List<string>();

                string strDept = m_serverDepartment.GetDeptInfoFromPersonnelInfo(
                    m_serverMuster.GetBill(m_lnqAffirm.DJH).SQR).Rows[0]["DepartmentCode"].ToString();
                noticeRoles.AddRange(m_billMessageServer.GetDeptDirectorRoleName(strDept));

                noticeRoles.Add(CE_RoleEnum.SQE组员.ToString());
                noticeRoles.Add(m_billMessageServer.GetRoleStringForStorage(m_lnqAffirm.StorageID).ToString());
                noticeRoles.Add(CE_RoleEnum.项目经理.ToString());
                noticeRoles.Add(CE_RoleEnum.检验员.ToString());
                noticeRoles.Add(CE_RoleEnum.产品开发主管.ToString());
                noticeRoles.Add(CE_RoleEnum.质控主管.ToString());
                noticeRoles.Add(CE_RoleEnum.零件工程师.ToString());

                m_billMessageServer.EndFlowMessage(m_lnqAffirm.DJH,
                    string.Format("{0} 号样品确认申请单已经处理完毕", m_lnqAffirm.DJH),
                    noticeRoles, null);

                #endregion 发送知会消息
            }
            else
            {
                MessageDialog.ShowErrorMessage("请重新确认单据状态");
            }
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            m_strDJH = dataGridView1.CurrentRow.Cells["单据号"].Value.ToString();

            if (m_strDJH != "" || m_strDJH != null)
            {
                样品确认申请单清单 form = new 样品确认申请单清单(m_strDJH);
                form.ShowDialog();
                m_lnqAffirm = form.LnqMuster;

                m_dtData = m_serverMuster.GetAllBill();
                RefrshDate(null, null);
                PositioningRecord(m_lnqAffirm.DJH);
            }
        }

        private void 刷新ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m_dtData = m_serverMuster.GetAllBill();

            foreach (Control cl in this.groupBox1.Controls)
            {
                if (cl is CheckBox)
                {
                    ((CheckBox)cl).Checked = true;
                }
            }

            RefreshDataGirdView(m_dtData);
        }

        private void 确认到货ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow.Cells["单据状态"].Value.ToString() == "等待仓管确认到货")
            {
                if (!UpdateForBill())
                {
                    return;
                }

                m_billMessageServer.PassFlowMessage(m_lnqAffirm.DJH,
                    string.Format("{0} 号样品确认申请单，请检验员检验", m_lnqAffirm.DJH),
                    CE_RoleEnum.检验员.ToString(), true);
            }
            else
            {
                MessageDialog.ShowErrorMessage("请重新确认单据状态");
            }
        }

        private void 回退单据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string strFlag = dataGridView1.CurrentRow.Cells["单据状态"].Value.ToString();

            if (toolStripMenuItem1.Visible == true && strFlag == "等待主管审核")
            {
                ReturnBillStatus();
            }
            else if (财务操作ToolStripMenuItem.Visible == true && strFlag == "等待财务确认")
            {
                ReturnBillStatus();
            }
            else if (仓管操作ToolStripMenuItem.Visible == true && strFlag == "等待仓管确认到货")
            {
                ReturnBillStatus();
            }
            else if (检验员操作ToolStripMenuItem.Visible == true && strFlag == "等待检验")
            {
                ReturnBillStatus();
            }
            else if (sQE操作ToolStripMenuItem.Visible == true && strFlag == "等待确认检验信息")
            {
                ReturnBillStatus();
            }
            else if (零件工程师操作ToolStripMenuItem.Visible == true && strFlag == "等待零件工程师评审")
            {
                ReturnBillStatus();
            }
            else if (评审员操作ToolStripMenuItem.Visible == true && strFlag == "等待工艺工程师评审")
            {
                ReturnBillStatus();
            }
            else if (项目经理操作ToolStripMenuItem.Visible == true && strFlag == "等待项目经理确认")
            {
                ReturnBillStatus();
            }
            else if (零件工程师操作ToolStripMenuItem.Visible == true && strFlag == "等待试验结果")
            {
                ReturnBillStatus();
            }
            else if (主管操作ToolStripMenuItem.Visible == true && strFlag == "等待主管确认")
            {
                ReturnBillStatus();
            }
            else if (sQE操作ToolStripMenuItem.Visible == true && strFlag == "等待SQE处理")
            {
                ReturnBillStatus();
            }
            else if (仓管操作ToolStripMenuItem.Visible == true && strFlag == "等待仓管确认入库")
            {
                ReturnBillStatus();
            }
        } 

        private void 删除单据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (BasicInfo.LoginName != dataGridView1.CurrentRow.Cells["申请人"].Value.ToString())
            {
                MessageDialog.ShowPromptMessage("您不是此记录的编制者无法进行此操作");
                return;
            }

            string strFlag = dataGridView1.CurrentRow.Cells["单据状态"].Value.ToString();

            if (strFlag != "单据已完成" || strFlag != "单据已报废")
            {

                if (MessageDialog.ShowEnquiryMessage("您是否要删除此单据") == DialogResult.No)
                {
                    return;
                }

                if (!m_serverMuster.ScarpBill(
                    dataGridView1.CurrentRow.Cells["单据号"].Value.ToString(), out m_err))
                {
                    m_billNoControl.CancelBill(dataGridView1.CurrentRow.Cells["单据号"].Value.ToString());
                    MessageDialog.ShowPromptMessage(m_err);
                    return;
                }
                else
                {
                    MessageDialog.ShowPromptMessage("报废成功");
                    m_billMessageServer.DestroyMessage(dataGridView1.CurrentRow.Cells["单据号"].Value.ToString());
                }
            }
            else
            {
                MessageDialog.ShowPromptMessage("请重新确认单据状态");
                return;
            }

            m_dtData = m_serverMuster.GetAllBill();

            foreach (Control cl in this.groupBox1.Controls)
            {
                if (cl is CheckBox)
                {
                    ((CheckBox)cl).Checked = true;
                }
            }

            RefreshDataGirdView(m_dtData);
        }

        private void 查找ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UniversalControlLibrary.FormPluginMethodCollection.BusinessDataSelect(CE_BillTypeEnum.样品确认申请单);
        }

        private void 条形码打印ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count < 1)
            {
                MessageDialog.ShowPromptMessage("请选择记录后再打印条形码");
                return;
            }

            List<View_S_InDepotGoodsBarCodeTable> lstBarCodeInfo = new List<View_S_InDepotGoodsBarCodeTable>();

            for (int i = 0; i < dataGridView1.SelectedRows.Count; i++)
            {
                string goodsCode = dataGridView1.SelectedRows[i].Cells["图号型号"].Value.ToString();
                string goodsName = dataGridView1.SelectedRows[i].Cells["物品名称"].Value.ToString();
                string spec = dataGridView1.SelectedRows[i].Cells["规格"].Value.ToString();
                string provider = dataGridView1.SelectedRows[i].Cells["供应商"].Value.ToString();
                string batchCode = dataGridView1.SelectedRows[i].Cells["批次号"].Value.ToString();
                string StorageID = dataGridView1.SelectedRows[i].Cells["库房编码"].Value.ToString();

                IBarCodeServer server = ServerModuleFactory.GetServerModule<IBarCodeServer>();
                View_S_InDepotGoodsBarCodeTable barcode = server.GetBarCodeInfo(goodsCode, goodsName, spec, provider, batchCode, StorageID);

                if (barcode == null)
                {
                    S_InDepotGoodsBarCodeTable newBarcode = new S_InDepotGoodsBarCodeTable();

                    IBasicGoodsServer basicServer = ServerModuleFactory.GetServerModule<IBasicGoodsServer>();
                    newBarcode.GoodsID = basicServer.GetGoodsID(goodsCode, goodsName, spec);
                    newBarcode.Provider = provider;
                    newBarcode.BatchNo = batchCode;
                    newBarcode.ProductFlag = "0";
                    newBarcode.StorageID = StorageID;

                    if (!server.Add(newBarcode, out m_err))
                    {
                        MessageDialog.ShowErrorMessage(m_err);
                        return;
                    }

                    barcode = server.GetBarCodeInfo(goodsCode, goodsName, spec, provider, batchCode, StorageID);
                }

                lstBarCodeInfo.Add(barcode);
            }

            foreach (var item in lstBarCodeInfo)
            {
                ServerModule.PrintPartBarcode.PrintBarcodeList(item);
            }
        }

        private void 单据审核toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow.Cells["单据状态"].Value.ToString() == "等待主管审核")
            {
                m_lnqAffirm = m_serverMuster.GetBill(dataGridView1.CurrentRow.Cells["单据号"].Value.ToString());

                if (!UpdateForBill())
                {
                    return;
                }

                if (m_lnqAffirm.IsOutsourcing && m_lnqAffirm.IsIncludeRawMaterial)
                {
                    m_billMessageServer.PassFlowMessage(m_lnqAffirm.DJH,
                        string.Format("{0} 号样品确认申请单，请财务确认", m_lnqAffirm.DJH),
                        CE_RoleEnum.会计.ToString(), true);
                }
                else
                {
                    m_billMessageServer.PassFlowMessage(m_lnqAffirm.DJH,
                        string.Format("{0} 号样品确认申请单，请仓管确认到货", m_lnqAffirm.DJH),
                        CE_RoleEnum.样品库管理员.ToString(), true);
                }
            }
            else
            {
                MessageDialog.ShowErrorMessage("请重新确认单据状态");
            }
        }

        private void 单据确认toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow.Cells["单据状态"].Value.ToString() == "等待财务确认")
            {
                if (!UpdateForBill())
                {
                    return;
                }

                m_billMessageServer.PassFlowMessage(m_lnqAffirm.DJH,
                    string.Format("{0} 号样品确认申请单，请财务确认", m_lnqAffirm.DJH),
                    CE_RoleEnum.会计.ToString(), true);
            }
            else
            {
                MessageDialog.ShowErrorMessage("请重新确认单据状态");
            }
        }

        private void chkSelectAllOrDeselectAll_CheckedChanged(object sender, EventArgs e)
        {
            checkBox1.CheckedChanged -= new EventHandler(RefrshDate);
            checkBox2.CheckedChanged -= new EventHandler(RefrshDate);
            checkBox3.CheckedChanged -= new EventHandler(RefrshDate);
            checkBox4.CheckedChanged -= new EventHandler(RefrshDate);
            checkBox5.CheckedChanged -= new EventHandler(RefrshDate);
            checkBox6.CheckedChanged -= new EventHandler(RefrshDate);
            checkBox7.CheckedChanged -= new EventHandler(RefrshDate);
            checkBox8.CheckedChanged -= new EventHandler(RefrshDate);
            checkBox9.CheckedChanged -= new EventHandler(RefrshDate);
            checkBox10.CheckedChanged -= new EventHandler(RefrshDate);
            checkBox11.CheckedChanged -= new EventHandler(RefrshDate);
            checkBox13.CheckedChanged -= new EventHandler(RefrshDate);
            checkBox14.CheckedChanged -= new EventHandler(RefrshDate);

            foreach (Control cl in this.groupBox1.Controls)
            {
                if (cl is CheckBox && cl.Text != "全选/全取消")
                {
                    ((CheckBox)cl).Checked = chkSelectAllOrDeselectAll.Checked;
                }
            }

            checkBox1.CheckedChanged += new EventHandler(RefrshDate);
            checkBox2.CheckedChanged += new EventHandler(RefrshDate);
            checkBox3.CheckedChanged += new EventHandler(RefrshDate);
            checkBox4.CheckedChanged += new EventHandler(RefrshDate);
            checkBox5.CheckedChanged += new EventHandler(RefrshDate);
            checkBox6.CheckedChanged += new EventHandler(RefrshDate);
            checkBox7.CheckedChanged += new EventHandler(RefrshDate);
            checkBox8.CheckedChanged += new EventHandler(RefrshDate);
            checkBox9.CheckedChanged += new EventHandler(RefrshDate);
            checkBox10.CheckedChanged += new EventHandler(RefrshDate);
            checkBox11.CheckedChanged += new EventHandler(RefrshDate);
            checkBox13.CheckedChanged += new EventHandler(RefrshDate);
            checkBox14.CheckedChanged += new EventHandler(RefrshDate);

            RefrshDate(sender, e);
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

        private void 质量确认检验信息ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (dataGridView1.CurrentRow.Cells["单据状态"].Value.ToString() == "等待确认检验信息")
            {
                if (!UpdateForBill())
                {
                    return;
                }

                if (m_lnqAffirm.IsBlank)
                {
                    m_billMessageServer.PassFlowMessage(m_lnqAffirm.DJH,
                        string.Format("{0} 号样品确认申请单，请工艺人员进行评审", m_lnqAffirm.DJH),
                        CE_RoleEnum.工艺人员.ToString(), true);
                }
                else
                {
                    m_billMessageServer.PassFlowMessage(m_lnqAffirm.DJH,
                        string.Format("{0} 号样品确认申请单，请零件工程师进行评审", m_lnqAffirm.DJH),
                        CE_RoleEnum.零件工程师.ToString(), true);
                }
            }
            else
            {
                MessageDialog.ShowErrorMessage("请重新确认单据状态");
            }
        }

        private void 提交评审结果ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow.Cells["单据状态"].Value.ToString() == "等待零件工程师评审")
            {
                if (!UpdateForBill())
                {
                    return;
                }

                if (m_lnqAffirm.IsBlank)
                {
                    m_billMessageServer.PassFlowMessage(m_lnqAffirm.DJH,
                        string.Format("{0} 号样品确认申请单，请SQE处理", m_lnqAffirm.DJH),
                        CE_RoleEnum.SQE组员.ToString(), true);
                }
                else
                {
                    if (m_lnqAffirm.MusterCarefulResult == "重样送样" || m_lnqAffirm.MusterCarefulResult == "入库")
                    {
                        m_billMessageServer.PassFlowMessage(m_lnqAffirm.DJH,
                            string.Format("{0} 号样品确认申请单，请主管确认", m_lnqAffirm.DJH),
                            CE_RoleEnum.产品开发主管.ToString(), true);
                    }
                    else
                    {
                        if (m_lnqAffirm.StorageID == "03")
                        {
                            m_billMessageServer.PassFlowMessage(m_lnqAffirm.DJH,
                                string.Format("{0} 号样品确认申请单，请等待试验结果", m_lnqAffirm.DJH),
                                CE_RoleEnum.零件工程师.ToString(), true);
                        }
                        else
                        {
                            m_billMessageServer.PassFlowMessage(m_lnqAffirm.DJH,
                                string.Format("{0} 号样品确认申请单，请等待项目经理确认", m_lnqAffirm.DJH),
                                CE_RoleEnum.项目经理.ToString(), true);
                        }
                    }
                }
            }
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            仓管操作ToolStripMenuItem.Visible = UniversalFunction.CheckStorageAndPersonnel(
                dataGridView1.CurrentRow.Cells["库房编码"].Value.ToString());
        }
    }
}

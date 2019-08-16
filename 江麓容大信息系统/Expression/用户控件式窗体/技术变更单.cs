using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PlatformManagement;
using ServerModule;
using GlobalObject;
using System.Net;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Formatters;
using UniversalControlLibrary;

namespace Expression
{
    /// <summary>
    /// 技术变更单界面
    /// </summary>
    public partial class 技术变更单 : Form
    {
        /// <summary>
        /// 产品信息服务组件
        /// </summary>
        IProductListServer m_serverProductList = ServerModuleFactory.GetServerModule<IProductListServer>();

        /// <summary>
        /// BOM表服务组件
        /// </summary>
        IBomServer m_serverBom = ServerModuleFactory.GetServerModule<IBomServer>();

        /// <summary>
        /// 单据编号控制类
        /// </summary>
        BillNumberControl m_billNoControl;

        /// <summary>
        /// 服务组件
        /// </summary>
        IProductInfoServer m_productInfoServer = ServerModuleFactory.GetServerModule<IProductInfoServer>();

        /// <summary>
        /// 服务组件
        /// </summary>
        ITechnologyChange m_serverTechnology = ServerModuleFactory.GetServerModule<ITechnologyChange>();

        /// <summary>
        /// 数据集
        /// </summary>
        S_TechnologyChangeBill m_lnqTechnology = new S_TechnologyChangeBill();

        /// <summary>
        /// 数据定位控件
        /// </summary>
        UserControlDataLocalizer m_dataLocalizer;

        /// <summary>
        /// 更改方式
        /// </summary>
        string m_strChangeMode = "";

        /// <summary>
        /// 权限标志
        /// </summary>
        AuthorityFlag m_authFlag;

        /// <summary>
        /// 错误信息
        /// </summary>
        string m_err;

        /// <summary>
        /// 二进制
        /// </summary>
        Byte[] m_byteData = null;

        /// <summary>
        /// 单据消息发布器
        /// </summary>
        IBillMessagePromulgatorServer m_msgPromulgator = BasicServerFactory.GetServerModule<IBillMessagePromulgatorServer>();

        public 技术变更单(PlatformManagement.FunctionTreeNodeInfo nodeInfo)
        {
            InitializeComponent();

            m_msgPromulgator.BillType = "技术变更单";

            m_billNoControl = new BillNumberControl(CE_BillTypeEnum.技术变更单, m_serverTechnology);

            m_authFlag = nodeInfo.Authority;

            ClearDate();

            RefreshDataGirdView(m_serverTechnology.GetAllBill());
        }

        private void 技术变更单_Load(object sender, EventArgs e)
        {
            AuthorityControl(m_authFlag);
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

                    DataTable dtMessage = UniversalFunction.PositioningOneRecord(msg.MessageContent, "技术变更单");

                    if (dtMessage.Rows.Count == 0)
                    {
                        m_msgPromulgator.DestroyMessage(msg.MessageContent);
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
        /// 权限控制
        /// </summary>
        /// <param name="authorityFlag">权限标志</param>
        void AuthorityControl(PlatformManagement.AuthorityFlag authorityFlag)
        {
            FaceAuthoritySetting.SetVisibly(menuStrip1, authorityFlag);
            FaceAuthoritySetting.SetEnable(this.Controls, authorityFlag);
        }

        /// <summary>
        /// 获得变更方式
        /// </summary>
        void GetChangeMode()
        {
            if (rb_Add.Checked)
            {
                m_strChangeMode = rb_Add.Text;
            }
            else if (rb_Delete.Checked)
            {
                m_strChangeMode = rb_Delete.Text;
            }
            else if (rb_Update.Checked)
            {
                m_strChangeMode = rb_Update.Text;
            }
        }

        /// <summary>
        /// 设置变更方式
        /// </summary>
        void SetChangeMode()
        {
            if (m_strChangeMode == rb_Add.Text)
            {
                rb_Add.Checked = true;
            }
            else if (m_strChangeMode == rb_Delete.Text)
            {
                rb_Delete.Checked = true;
            }
            else if (m_strChangeMode == rb_Update.Text)
            {
                rb_Update.Checked = true;
            }
        }

        /// <summary>
        /// 清除数据
        /// </summary>
        public void ClearDate()
        {
            txtChangeCoding.Text = "";
            m_byteData = null;
            txtDJH.Text = "";
            m_strChangeMode = "";
            txtChangeReason.Text = "";
            lbDJZT.Text = "";
            rb_Add.Checked = false;
            rb_Delete.Checked = false;
            rb_Update.Checked = false;

            txtNewName.Text = "";
            txtNewName.Tag = (int?)null;
            txtNewCode.Text = "";
            txtNewSpec.Text = "";
            txtNewVersion.Text = "";
            txtNewParentCode.Text = "";
            txtNewParentCode.Tag = (int?)null;
            txtNewCounts.Value = 0;


            txtOldName.Text = "";
            txtOldName.Tag = (int?)null;
            txtOldCode.Text = "";
            txtOldSpec.Text = "";
            txtOldVersion.Text = "";
            txtOldParentCode.Text = "";
            txtOldParentCode.Tag = (int?)null;
            txtOldCounts.Value = 0;

        }

        /// <summary>
        /// 刷新
        /// </summary>
        /// <param name="source">数据集</param>
        void RefreshDataGirdView(DataTable source)
        {
            dataGridView1.DataSource = source;
            dataGridView1.Columns["单据号"].Width = 120;

            if (m_dataLocalizer == null)
            {
                m_dataLocalizer = new UserControlDataLocalizer(dataGridView1, this.Name, 
                    UniversalFunction.SelectHideFields(this.Name, dataGridView1.Name, BasicInfo.LoginID));
                panelPara.Controls.Add(m_dataLocalizer);
                m_dataLocalizer.Dock = DockStyle.Bottom;
            }
        }

        /// <summary>
        /// 获得数据集
        /// </summary>
        public void GetMessage()
        {
            m_lnqTechnology.DJH = txtDJH.Text.Trim();
            m_lnqTechnology.DJZT = lbDJZT.Text;
            m_lnqTechnology.ChangeReason = txtChangeReason.Text.Trim();

            if (m_byteData == null || m_byteData.ToString() == "")
            {
                m_lnqTechnology.FileMessage = null;
            }
            else
            {
                m_lnqTechnology.FileMessage = m_byteData;
            }
            
            m_lnqTechnology.FileCode = txtChangeCoding.Text.Trim();

            GetChangeMode();

            m_lnqTechnology.NewParentID = txtNewParentCode.Text.Trim().Length == 0 ? (int?)null : Convert.ToInt32(txtNewParentCode.Tag);
            m_lnqTechnology.NewGoodsID = txtNewName.Text.Trim().Length == 0 ? (int?)null : Convert.ToInt32(txtNewName.Tag);

            m_lnqTechnology.NewCounts = Convert.ToInt32(txtNewCounts.Value);
            m_lnqTechnology.NewVersion = txtNewVersion.Text.Trim();

            m_lnqTechnology.OldParentID = txtOldParentCode.Text.Trim().Length == 0 ? (int?)null : Convert.ToInt32(txtOldParentCode.Tag);
            m_lnqTechnology.OldGoodsID = txtOldName.Text.Trim().Length == 0 ? (int?)null : Convert.ToInt32(txtOldName.Tag);
            
            m_lnqTechnology.OldCounts = Convert.ToInt32(txtOldCounts.Value);
            m_lnqTechnology.OldVersion = txtOldVersion.Text.Trim();

            m_lnqTechnology.ChangeMode = m_strChangeMode;
        }

        /// <summary>
        /// 检查数据
        /// </summary>
        /// <returns>通过返回True，否则返回False</returns>
        private bool CheckData()
        {
            if (txtChangeCoding.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("请填写变更单编号!");
                return false;
            }

            if (m_byteData == null || m_byteData.ToString() == "")
            {
                MessageDialog.ShowPromptMessage("请上传文件!");
                return false;
            }

            if (m_strChangeMode != "新增")
            {
                if (txtOldCode.Text.Trim() == "")
                {
                    MessageDialog.ShowPromptMessage("旧零件图号型号不能为空");
                    return false;
                }

                if (txtOldName.Text.Trim() == "")
                {
                    MessageDialog.ShowPromptMessage("旧零件名称不能为空");
                    return false;
                }
            }
            if (m_strChangeMode != "删除")
            {

                if (txtNewCode.Text.Trim() == "")
                {
                    MessageDialog.ShowPromptMessage("新零件的图型图号不能为空");
                    return false;

                }

                if (txtNewName.Text.Trim() == "")
                {
                    MessageDialog.ShowPromptMessage("新零件名称不能为空");
                    return false;
                }
            }

            return true;
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            txtDJH.Text = dataGridView1.CurrentRow.Cells["单据号"].Value.ToString();
            txtChangeReason.Text = dataGridView1.CurrentRow.Cells["变更原因"].Value.ToString();
            txtChangeCoding.Text = dataGridView1.CurrentRow.Cells["变更单编号"].Value.ToString();
            lbDJZT.Text = dataGridView1.CurrentRow.Cells["单据状态"].Value.ToString();
            m_strChangeMode = dataGridView1.CurrentRow.Cells["变更方式"].Value.ToString();


            txtNewCode.Text = dataGridView1.CurrentRow.Cells["新零件编码"].Value.ToString();
            txtNewCounts.Value = dataGridView1.CurrentRow.Cells["新零件基数"].Value.ToString().Trim().Length == 0 ?
                0 : Convert.ToDecimal(dataGridView1.CurrentRow.Cells["新零件基数"].Value.ToString());
            txtNewName.Text = dataGridView1.CurrentRow.Cells["新零件名称"].Value.ToString();
            txtNewName.Tag = dataGridView1.CurrentRow.Cells["新零件物品ID"].Value.ToString().Trim().Length == 0 ?
                (int?)null : Convert.ToInt32(dataGridView1.CurrentRow.Cells["新零件物品ID"].Value);

            txtNewParentCode.Text = dataGridView1.CurrentRow.Cells["新零件父级编码"].Value.ToString();
            txtNewParentCode.Tag = dataGridView1.CurrentRow.Cells["新零件父级ID"].Value.ToString().Trim().Length == 0 ? 
                (int?)null : Convert.ToInt32(dataGridView1.CurrentRow.Cells["新零件父级ID"].Value);

            txtNewSpec.Text = dataGridView1.CurrentRow.Cells["新零件规格"].Value.ToString();
            txtNewVersion.Text = dataGridView1.CurrentRow.Cells["新零件版次号"].Value.ToString();

            txtOldCode.Text = dataGridView1.CurrentRow.Cells["旧零件编码"].Value.ToString();
            txtOldCounts.Value = dataGridView1.CurrentRow.Cells["旧零件基数"].Value.ToString().Trim().Length == 0 ? 
                0 : Convert.ToDecimal(dataGridView1.CurrentRow.Cells["旧零件基数"].Value.ToString());

            txtOldName.Text = dataGridView1.CurrentRow.Cells["旧零件名称"].Value.ToString();
            txtOldName.Tag = dataGridView1.CurrentRow.Cells["旧零件物品ID"].Value.ToString().Trim().Length == 0 ?
                (int?)null : Convert.ToInt32(dataGridView1.CurrentRow.Cells["旧零件物品ID"].Value);

            txtOldParentCode.Text = dataGridView1.CurrentRow.Cells["旧零件父级总成编码"].Value.ToString();
            txtOldParentCode.Tag = dataGridView1.CurrentRow.Cells["旧零件父级ID"].Value.ToString().Trim().Length == 0 ? 
                (int?)null : Convert.ToInt32(dataGridView1.CurrentRow.Cells["旧零件父级ID"].Value);

            txtOldSpec.Text = dataGridView1.CurrentRow.Cells["旧零件规格"].Value.ToString();
            txtOldVersion.Text = dataGridView1.CurrentRow.Cells["旧零件版次号"].Value.ToString();

            SetChangeMode();

            DataRow dr = m_serverTechnology.GetBill(dataGridView1.CurrentRow.Cells["单据号"].Value.ToString());

            if (Convert.IsDBNull(dr["FileMessage"]))
            {
                m_byteData = null;
            }
            else
            {
                m_byteData = (byte[])dr["FileMessage"];
            }
        }

        private void 新建单据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClearDate();
            txtDJH.Text = m_billNoControl.GetNewBillNo();
            lbDJZT.Text = "新建单据";
            
        }

        private void 提交单据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lbDJZT.Text == "单据已完成")
            {
                MessageDialog.ShowPromptMessage("请重新确认单据状态");
            }
            else
            {
                GetMessage();

                if (m_lnqTechnology.OldParentID != null)
                {
                    DataTable dt = m_serverBom.GetBomStructInfo(Convert.ToInt32(m_lnqTechnology.OldParentID),
                        Convert.ToInt32(m_lnqTechnology.OldGoodsID));

                    if (dt.Rows.Count == 0)
                    {
                        MessageDialog.ShowPromptMessage("BOM表结构不匹配，请重新确认");
                        return;
                    }

                    m_lnqTechnology.OldCounts = Convert.ToInt32(dt.Rows[0]["Usage"]);
                }

                //m_lnqTechnology.DJH = m_billNoControl.GetNewBillNo();

                if (!CheckData())
                {
                    return;
                }
                else
                {
                    if (m_serverTechnology.SubmitBill(m_lnqTechnology, out m_err))
                    {
                        MessageDialog.ShowPromptMessage("提交成功");

                        m_msgPromulgator.DestroyMessage(txtDJH.Text);
                        m_msgPromulgator.SendNewFlowMessage(txtDJH.Text,
                            string.Format("{0} 号技术变更单，请零件工程师批准", txtDJH.Text),
                            CE_RoleEnum.零件工程师);
                    }
                    else
                    {
                        MessageDialog.ShowPromptMessage(m_err);
                        return;
                    }
                }
            }

            RefreshDataGirdView(m_serverTechnology.GetAllBill());
            PositioningRecord(m_lnqTechnology.DJH);
        }

        private void 报废单据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool blflag = false;

            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {
                if (BasicInfo.LoginName != row.Cells["申请人员"].Value.ToString())
                {
                    MessageDialog.ShowPromptMessage("您不是此记录的编制者无法进行此操作");
                    blflag = true;
                }

                if (row.Cells["单据状态"].Value.ToString() == "单据已完成")
                {
                    MessageDialog.ShowPromptMessage("请重新确认单据状态");
                    blflag = true;
                }
                else
                {
                    if (m_serverTechnology.DeleteBill(dataGridView1.CurrentRow.Cells["单据号"].Value.ToString(), out m_err))
                    {
                        m_billNoControl.CancelBill(dataGridView1.CurrentRow.Cells["单据号"].Value.ToString());
                        m_msgPromulgator.DestroyMessage(dataGridView1.CurrentRow.Cells["单据号"].Value.ToString());
                    }
                    else
                    {
                        MessageDialog.ShowPromptMessage(m_err);
                        blflag = true;
                    }
                }
            }

            if (!blflag)
            {
                MessageDialog.ShowPromptMessage("删除成功");
            }

            ClearDate();

            RefreshDataGirdView(m_serverTechnology.GetAllBill());
            PositioningRecord(m_lnqTechnology.DJH);
        }

        private void 确认通过ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool blflag = false;

            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {
                m_lnqTechnology.DJH = row.Cells["单据号"].Value.ToString();
                m_lnqTechnology.DJZT = row.Cells["单据状态"].Value.ToString();

                if (row.Cells["单据状态"].Value.ToString() != "等待批准" )
                {
                    MessageDialog.ShowPromptMessage("请重新确认单据状态");
                    blflag = true;
                }

                if (!m_serverTechnology.UpdateBill(m_lnqTechnology, out m_err))
                {
                    MessageDialog.ShowPromptMessage(m_err);
                    blflag = true;
                }
                else
                {
                    m_billNoControl.UseBill(m_lnqTechnology.DJH);

                    #region 发送知会消息

                    List<string> noticeRoles = new List<string>();
                    noticeRoles.Add(CE_RoleEnum.零件工程师.ToString());
                    noticeRoles.Add(CE_RoleEnum.开发部办公室文员.ToString());
                    noticeRoles.Add(CE_RoleEnum.采购账务管理员.ToString());

                    m_msgPromulgator.EndFlowMessage(m_lnqTechnology.DJH,
                        string.Format("{0} 号技术变更单已经处理完毕", m_lnqTechnology.DJH),
                        noticeRoles, null);

                    #endregion 发送知会消息
                }
            }

            if (!blflag)
            {
                MessageDialog.ShowPromptMessage("批准成功");
            }

            RefreshDataGirdView(m_serverTechnology.GetAllBill());
            PositioningRecord(m_lnqTechnology.DJH);
        }

        private void rb_Add_CheckedChanged(object sender, EventArgs e)
        {
            if (rb_Add.Checked)
            {
                groupBox3.Enabled = false;
                txtOldParentCode.Text = "";
                txtOldSpec.Text = "";
                txtOldName.Text = "";
                txtOldCode.Text = "";
                txtOldCounts.Value = 0;
                txtOldVersion.Text = "";
            }
            else
            {
                groupBox3.Enabled = true;
            }
        }

        private void rb_Delete_CheckedChanged(object sender, EventArgs e)
        {
            if (rb_Delete.Checked)
            {
                groupBox2.Enabled = false;
                txtNewVersion.Text = "";
                txtNewSpec.Text = "";
                txtNewParentCode.Text = "";
                txtNewName.Text = "";
                txtNewCounts.Value = 0;
                txtNewCode.Text = "";
            }
            else
            {
                groupBox2.Enabled = true;
            }
        }

        private void 刷新数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RefreshDataGirdView(m_serverTechnology.GetAllBill());
        }

        private void 回退单据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lbDJZT.Text != "等待批准")
            {
                MessageDialog.ShowPromptMessage("请重新确认单据状态");
            }
            else
            {
                GetMessage();

                if (m_lnqTechnology.OldParentID != null)
                {
                    DataTable dt = m_serverBom.GetBomStructInfo(Convert.ToInt32(m_lnqTechnology.OldParentID),
                        Convert.ToInt32(m_lnqTechnology.OldGoodsID));

                    if (dt.Rows.Count == 0)
                    {
                        MessageDialog.ShowPromptMessage("BOM表结构不匹配，请重新确认");
                        return;
                    }

                    m_lnqTechnology.OldCounts = Convert.ToInt32(dt.Rows[0]["Usage"]);
                }

                m_lnqTechnology.DJZT = "新建单据";

                if (m_serverTechnology.UpdateBill(m_lnqTechnology, out m_err))
                {
                    MessageDialog.ShowPromptMessage("回退成功");
                }
                else
                {
                    MessageDialog.ShowPromptMessage(m_err);
                    return;
                }
            }

            RefreshDataGirdView(m_serverTechnology.GetAllBill());
            PositioningRecord(m_lnqTechnology.DJH);
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
                    m_byteData = br.ReadBytes((int)file.Length);
                    if(file != null)
                        file.Close();
                }
            }
        }

        private void lbDownFile_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "pdf files (*.pdf)|*.pdf|doc files (*.doc)|*.doc|All files (*.*)|*.*";

            if (m_serverTechnology.GetBill(
                dataGridView1.CurrentRow.Cells["单据号"].Value.ToString())["FileMessage"] == DBNull.Value)
            {
                MessageDialog.ShowPromptMessage("该条变更单无文件");
                return;
            }

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                DataRow dr = m_serverTechnology.GetBill(dataGridView1.CurrentRow.Cells["单据号"].Value.ToString());

                if (Convert.IsDBNull(dr["FileMessage"]))
                {
                    m_byteData = null;
                }
                else
                {
                    m_byteData = (byte[])dr["FileMessage"];
                }

                

                if (m_byteData != null || m_byteData.ToString() != "")
                {
                    
                    FileStream fs = new FileStream(saveFileDialog1.FileName, FileMode.Create, FileAccess.Write);
                    BinaryWriter bw = new BinaryWriter(fs);
                    bw.Write(m_byteData);
                    bw.Close();
                    fs.Close();
                }
            }
        }

        private void txtOldName_Enter(object sender, EventArgs e)
        {
            GetChangeMode();

            if (m_strChangeMode == "")
            {
                MessageDialog.ShowPromptMessage("请选择变更方式");
            }
        }

        private void lbFindInfo_Click(object sender, EventArgs e)
        {
            if (txtOldCode.Text.Trim().Length == 0)
            {
                return;
            }

            DataTable dt = m_serverTechnology.GetBomInfo(txtOldCode.Text, txtOldName.Text, txtOldSpec.Text);

            FormDataShow form = new FormDataShow(dt);
            form.ShowDialog();
        }

        private void txtOldName_OnCompleteSearch()
        {
            if (txtOldParentCode.Tag != null)
            {
                DataTable dt = m_serverBom.GetBomStructInfo(Convert.ToInt32(txtOldParentCode.Tag),
                    Convert.ToInt32(txtOldName.DataResult["物品ID"]));

                if (dt.Rows.Count == 0)
                {
                    MessageDialog.ShowPromptMessage("BOM表结构不匹配，请重新确认");

                    txtOldName.Text = "";
                    txtOldName.Tag = (int?)null;
                    txtOldCode.Text = "";
                    txtOldSpec.Text = "";
                    txtOldVersion.Text = "";
                    txtOldCounts.Value = 0;

                    return;
                }

                txtOldCounts.Value = Convert.ToDecimal( dt.Rows[0]["Usage"]);
            }
            else
            {
                txtOldCounts.Value = m_serverBom.GetAllMostUsage(Convert.ToInt32(txtOldName.DataResult["物品ID"]));
            }

            txtOldName.Tag = Convert.ToInt32(txtOldName.DataResult["物品ID"]);

            txtOldCode.Text = txtOldName.DataResult["零部件编码"].ToString();
            txtOldName.Text = txtOldName.DataResult["零部件名称"].ToString();
            txtOldSpec.Text = txtOldName.DataResult["规格"].ToString();

            txtOldVersion.Text = txtOldName.DataResult["版次号"].ToString();

            GetChangeMode();

            if (m_strChangeMode == "修改")
            {
                txtNewParentCode.Text = txtOldParentCode.Text;
                txtNewParentCode.Tag = txtOldParentCode.Tag;

                txtNewName.Tag = Convert.ToInt32(txtOldName.DataResult["物品ID"]);

                txtNewCode.Text = txtOldName.DataResult["零部件编码"].ToString();
                txtNewName.Text = txtOldName.DataResult["零部件名称"].ToString();
                txtNewSpec.Text = txtOldName.DataResult["规格"].ToString();

                txtNewVersion.Text = txtOldVersion.Text;
                txtNewCounts.Value = txtOldCounts.Value;
            }
        }

        private void txtOldParentCode_OnCompleteSearch()
        {
            if (txtOldParentCode.Text.Trim().Length == 0)
            {
                txtOldParentCode.Tag = (int?)null;
            }

            txtOldParentCode.Tag = Convert.ToInt32(txtOldParentCode.DataResult["物品ID"]);
            txtOldParentCode.Text = txtOldParentCode.DataResult["零部件编码"].ToString();
        }

        private void txtNewParentCode_OnCompleteSearch()
        {
            txtNewParentCode.Tag = Convert.ToInt32(txtNewParentCode.DataResult["物品ID"]);
            txtNewParentCode.Text = txtNewParentCode.DataResult["零部件编码"].ToString();
        }

        private void txtNewName_OnCompleteSearch()
        {
            txtNewName.Tag = Convert.ToInt32(txtNewName.DataResult["序号"]);
            txtNewCode.Text = txtNewName.DataResult["图号型号"].ToString();
            txtNewName.Text = txtNewName.DataResult["物品名称"].ToString();
            txtNewSpec.Text = txtNewName.DataResult["规格"].ToString();
        }
    }
}

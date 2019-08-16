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
using System.IO;
using GlobalObject;
using UniversalControlLibrary;

namespace Expression
{
    public partial class 技术变更处置明细 : Form
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        string m_error;

        /// <summary>
        /// 单据号
        /// </summary>
        string m_billNo;

        /// <summary>
        /// S_TechnologyAlterationBill数据集
        /// </summary>
        S_TechnologyAlterationBill m_lnqTechnology = new S_TechnologyAlterationBill();

        /// <summary>
        /// 二进制
        /// </summary>
        Byte[] m_byteData = null;

        /// <summary>
        /// 单据编号控制类
        /// </summary>
        BillNumberControl m_billNoControl;

        /// <summary>
        /// BOM表服务组件
        /// </summary>
        IBomServer m_serverBom = ServerModuleFactory.GetServerModule<IBomServer>();

        /// <summary>
        /// 单据类消息发布器
        /// </summary>
        IBillMessagePromulgatorServer m_msgPromulgator = BasicServerFactory.GetServerModule<IBillMessagePromulgatorServer>();

        /// <summary>
        /// 技术变更服务组件
        /// </summary>
        ITechnologyAlteration m_technologyServer = ServerModuleFactory.GetServerModule<ITechnologyAlteration>();

        public 技术变更处置明细(string billNo, AuthorityFlag authFlag)
        {
            InitializeComponent();

            FaceAuthoritySetting.SetVisibly(this.menuStrip1, authFlag);
            menuStrip1.Visible = true;

            m_msgPromulgator.BillType = "技术变更单";

            m_billNoControl = new BillNumberControl(CE_BillTypeEnum.技术变更单, m_technologyServer);

            if (billNo == "")
            {
                txtDJH.Text = m_billNoControl.GetNewBillNo();
                txtStatus.Text = "新建单据";
            }
            else
            {
                m_billNo = billNo;

                DataRow row = m_technologyServer.GetBillInfo(m_billNo);

                if (row != null)
                {
                    txtDJH.Text = m_billNo;
                    txtStatus.Text = row["BillStatus"].ToString();
                    txtChangeReason.Text = row["ChangeReason"].ToString();
                    txtChangeCoding.Text = row["ChangeBillNo"].ToString();
                    m_byteData = (byte[])row["FileMessage"];
                    lbFileName.Text = row["FileName"].ToString();
                }
            }

            dataGridView1.DataSource = m_technologyServer.GetListInfo(billNo);

            dataGridView1.Columns["序号"].Visible = false;
            dataGridView1.Columns["单据号"].Visible = false;
            dataGridView1.Columns["NewParentID"].Visible = false;
            dataGridView1.Columns["NewGoodsID"].Visible = false;
            dataGridView1.Columns["OldParentID"].Visible = false;
            dataGridView1.Columns["OldGoodsID"].Visible = false;

            userControlDataLocalizer1.Init(dataGridView1, this.Name,
                UniversalFunction.SelectHideFields(this.Name, dataGridView1.Name, BasicInfo.LoginID));
        }

        private void 新建单据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClearDate();
            txtDJH.Text = m_billNoControl.GetNewBillNo();
            txtStatus.Text = "新建单据";
        }

        /// <summary>
        /// 清除数据
        /// </summary>
        private void ClearDate()
        {
            txtChangeCoding.Text = "";
            m_byteData = null;
            txtDJH.Text = "";
            txtChangeReason.Text = "";
            txtStatus.Text = "";

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

            if (dataGridView1.Rows.Count <= 0)
            {
                MessageDialog.ShowPromptMessage("请选择零件信息！");
                return false;
            }

            return true;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dtTemp = (DataTable)dataGridView1.DataSource;

                string changMode = "";

                if (rb_Update.Checked)
                {
                    changMode = "修改";
                }
                else if (rb_Add.Checked)
                {
                    changMode = "新增";
                }
                else
                {
                    changMode = "删除";
                }


                if (dataGridView1.Rows.Count > 0)
                {
                    for (int i = 0; i < dtTemp.Rows.Count; i++)
                    {
                        if (changMode == dtTemp.Rows[i]["变更模式"].ToString() && (txtOldParentCode.Tag != null
                            && dtTemp.Rows[i]["OldParentID"].ToString() != ""
                            && Convert.ToInt32(dtTemp.Rows[i]["OldParentID"]) == Convert.ToInt32(txtOldParentCode.Tag))
                            && Convert.ToInt32(txtOldName.Tag) == Convert.ToInt32(dtTemp.Rows[i]["OldGoodsID"])
                            && (txtNewParentCode.Tag != null && dtTemp.Rows[i]["NewParentID"].ToString() != ""
                            && Convert.ToInt32(dtTemp.Rows[i]["NewParentID"]) == Convert.ToInt32(txtNewParentCode.Tag))
                            && Convert.ToInt32(txtNewName.Tag) == Convert.ToInt32(dtTemp.Rows[i]["NewGoodsID"]))
                        {
                            MessageDialog.ShowPromptMessage("不能录入重复物品");
                            return;
                        }

                        if (txtOldName.Tag.ToString() != "0" && txtOldParentCode.Tag.ToString() != "0")
                        {
                            DataTable dt = m_serverBom.GetBomStructInfo(Convert.ToInt32(txtOldParentCode.Tag),
                                    Convert.ToInt32(txtOldName.Tag));

                            if (dt.Rows.Count == 0)
                            {
                                MessageDialog.ShowPromptMessage("第" + (i + 1) + "行中BOM表结构不匹配，请重新确认");
                                return;
                            }
                        }
                    }
                }
                
                if (changMode == "修改" && (txtOldParentCode.Tag == null || txtOldParentCode.Tag.ToString() == ""))
                {
                    if (MessageDialog.ShowEnquiryMessage("您没有选择父级零件，修改时将Bom表中所用到此零件的信息批量修改成现在的信息？") == DialogResult.No)
                    {
                        return;
                    }
                }

                DataRow dr = dtTemp.NewRow();

                dr["新零件父级编码"] = txtNewParentCode.Text;
                dr["新零件编码"] = txtNewCode.Text;

                if (cbGoodsInfo.Checked)
                {
                    dr["新零件名称"] = txtNewUseName.Text;
                }
                else
                {
                    dr["新零件名称"] = txtNewName.Text;
                }

                dr["新零件规格"] = txtNewSpec.Text;
                dr["新零件版次号"] = txtNewVersion.Text;
                dr["新零件基数"] = txtNewCounts.Value;

                dr["旧零件父级总成编码"] = txtOldParentCode.Text;
                dr["旧零件编码"] = txtOldCode.Text;
                dr["旧零件名称"] = txtOldName.Text;
                dr["旧零件规格"] = txtOldSpec.Text;
                dr["旧零件版次号"] = txtOldVersion.Text;
                dr["旧零件基数"] = txtOldCounts.Value;
                dr["变更原因"] = "";

                if (rb_Add.Checked)
                {
                    dr["变更模式"] = "新增";
                }
                else if (rb_Delete.Checked)
                {
                    dr["变更模式"] = "删除";
                }
                else
                {
                    dr["变更模式"] = "修改";
                }

                if (cbGoodsInfo.Checked)
                {
                    dr["是否修改零件本身"] = "是";
                }
                else
                {
                    dr["是否修改零件本身"] = "否";
                }

                dr["NewParentID"] = txtNewParentCode.Tag == null ? 0 : Convert.ToInt32(txtNewParentCode.Tag);
                dr["NewGoodsID"] = txtNewName.Tag == null ? 0 : Convert.ToInt32(txtNewName.Tag);

                //if (txtNewParentCode.Tag != null && txtNewParentCode.Tag.ToString() != "")
                //{
                    
                //}
                

                //if (txtNewName.Tag != null && txtNewName.Tag != "")
                //{
                //    dr["NewGoodsID"] = txtNewName.Tag;
                //}

                if (!rb_Add.Checked)
                {
                    dr["OldParentID"] = txtOldParentCode.Tag == null ? 0 : Convert.ToInt32(txtOldParentCode.Tag);
                    dr["OldGoodsID"] = txtOldName.Tag == null ? 0 : Convert.ToInt32(txtOldName.Tag);
                    //if (txtOldParentCode.Tag != null && txtOldParentCode.Tag.ToString() != "")
                    //{
                    //    dr["OldParentID"] = txtOldParentCode.Tag;
                    //}

                    //dr["OldGoodsID"] = txtOldName.Tag;
                }

                dtTemp.Rows.Add(dr);

                dataGridView1.DataSource = dtTemp;

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
            catch (Exception)
            {

                throw;
            }
        }

        private void rb_Add_CheckedChanged(object sender, EventArgs e)
        {
            if (rb_Add.Checked)
            {
                //groupBox3.Enabled = false;
                txtOldParentCode.Enabled = false;
                txtOldParentCode.Text = "";
                txtOldParentCode.Tag = null;
                txtOldSpec.Text = "";
                txtOldName.Enabled = false;
                txtOldName.Text = "";
                txtOldName.Tag = null;
                txtOldCode.Text = "";
                txtOldCounts.Value = 0;
                txtOldVersion.Text = "";
                txtOldCounts.Enabled = false;
                txtOldVersion.Enabled = false;
                rb_Add.Enabled = true;
                rb_Delete.Enabled = true;
                rb_Update.Enabled = true;
                cbGoodsInfo.Enabled = true;
            }
            else
            {
                groupBox3.Enabled = true;
                txtOldParentCode.Enabled = true;
                txtOldName.Enabled = true;
                txtOldCounts.Enabled = true;
                txtOldVersion.Enabled = true;
            }
        }

        private void rb_Delete_CheckedChanged(object sender, EventArgs e)
        {
            if (rb_Delete.Checked)
            {
                //groupBox2.Enabled = false;

                txtNewParentCode.Enabled = false;
                txtNewUseName.Enabled = false;

                txtNewVersion.Text = "";
                txtNewSpec.Text = "";
                txtNewParentCode.Text = "";
                txtNewParentCode.Tag = null;
                txtNewName.Text = "";
                txtNewName.Tag = null;
                txtNewCounts.Value = 0;
                txtNewCode.Text = "";

            }
            else
            {
                groupBox2.Enabled = true;
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

        private void txtOldName_OnCompleteSearch()
        {
            if (txtOldParentCode.Tag != null && txtOldParentCode.Tag.ToString() != "")
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

                txtOldCounts.Value = Convert.ToDecimal(dt.Rows[0]["Usage"]);
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

            if (rb_Update.Checked)
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
            else if (cbGoodsInfo.Checked)
            {
                txtNewName.Tag = Convert.ToInt32(txtOldName.DataResult["物品ID"]);
                txtNewParentCode.Text = txtOldParentCode.Text;
                txtNewParentCode.Tag = txtOldParentCode.Tag; 
                txtNewCode.Text = txtOldName.DataResult["零部件编码"].ToString();
                txtNewName.Text = txtOldName.DataResult["零部件名称"].ToString();
                txtNewSpec.Text = txtOldName.DataResult["规格"].ToString();
                txtNewVersion.Text = txtOldVersion.Text;
                txtNewCounts.Value = txtOldCounts.Value;
            }
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

        private void lbUpFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofdPic = new OpenFileDialog();
            ofdPic.Filter = "doc files (*.doc)|*.doc|pdf files (*.pdf)|*.pdf|All files (*.*)|*.*";

            if (ofdPic.ShowDialog() == DialogResult.OK)
            {
                lbFileName.Text = ofdPic.FileName.ToString().Substring(ofdPic.FileName.ToString().LastIndexOf(@"\") + 1);

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
                    file.Close();
                }
            }
        }

        private void lbDownFile_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "pdf files (*.pdf)|*.pdf|doc files (*.doc)|*.doc|All files (*.*)|*.*";

            if (m_technologyServer.GetBillInfo(
                dataGridView1.CurrentRow.Cells["单据号"].Value.ToString())["FileMessage"] == DBNull.Value)
            {
                MessageDialog.ShowPromptMessage("该条变更单无文件");
                return;
            }

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                DataRow dr = m_technologyServer.GetBillInfo(dataGridView1.CurrentRow.Cells["单据号"].Value.ToString());

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

        private void lbFindInfo_Click(object sender, EventArgs e)
        {
            if (txtOldCode.Text.Trim().Length == 0)
            {
                return;
            }

            DataTable dt = m_technologyServer.GetBomInfo(txtOldCode.Text, txtOldName.Text, txtOldSpec.Text);

            FormDataShow form = new FormDataShow(dt);
            form.ShowDialog();
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                return;
            }
            else
            {
                DataTable dtTemp = (DataTable)dataGridView1.DataSource;

                for (int i = 0; i < dtTemp.Rows.Count; i++)
                {
                    string changMode = "";

                    if (rb_Update.Checked)
                    {
                        changMode = "修改";
                    }
                    else if (rb_Add.Checked)
                    {
                        changMode = "新增";
                    }
                    else
                    {
                        changMode = "删除";
                    }

                    if (changMode == dtTemp.Rows[i]["变更模式"].ToString()
                        && Convert.ToInt32(txtOldParentCode.Tag) == Convert.ToInt32(dtTemp.Rows[i]["OldParentID"])
                        && Convert.ToInt32(txtNewParentCode.Tag) == Convert.ToInt32(dtTemp.Rows[i]["NewParentID"])
                        && Convert.ToInt32(txtOldName.Tag) == Convert.ToInt32(dtTemp.Rows[i]["OldGoodsID"])
                        && Convert.ToInt32(txtNewName.Tag) == Convert.ToInt32(dtTemp.Rows[i]["NewGoodsID"])
                        && txtNewVersion.Text == dtTemp.Rows[i]["新零件版次号"].ToString()
                        && txtNewCounts.Text == dtTemp.Rows[i]["新零件基数"].ToString()
                        && txtNewCode.Text == dtTemp.Rows[i]["新零件编码"].ToString()
                        && (txtNewName.Text == dtTemp.Rows[i]["新零件名称"].ToString() 
                        || txtNewUseName.Text == dtTemp.Rows[i]["新零件名称"].ToString())
                        && txtNewSpec.Text == dtTemp.Rows[i]["新零件规格"].ToString())
                    {
                        MessageDialog.ShowPromptMessage("不能录入重复物品");
                        return;
                    }

                    if (txtOldName.Tag.ToString() != "0" && txtOldParentCode.Tag.ToString() != "0" && txtOldParentCode.Tag.ToString() != "0")
                    {
                        DataTable dt = m_serverBom.GetBomStructInfo(Convert.ToInt32(txtOldParentCode.Tag),
                                Convert.ToInt32(txtOldName.Tag));

                        if (dt.Rows.Count == 0)
                        {
                            MessageDialog.ShowPromptMessage("第" + (i + 1) + "行中BOM表结构不匹配，请重新确认");
                            return;
                        }
                    }
                }

                dataGridView1.CurrentRow.Cells["新零件父级编码"].Value = txtNewParentCode.Text;
                dataGridView1.CurrentRow.Cells["新零件编码"].Value = txtNewCode.Text;

                if (cbGoodsInfo.Checked)
                {
                    dataGridView1.CurrentRow.Cells["新零件名称"].Value = txtNewUseName.Text;
                    txtNewName.Tag = txtOldName.Tag;
                }
                else
                {
                    dataGridView1.CurrentRow.Cells["新零件名称"].Value = txtNewName.Text;
                }

                dataGridView1.CurrentRow.Cells["新零件规格"].Value = txtNewSpec.Text;
                dataGridView1.CurrentRow.Cells["新零件版次号"].Value = txtNewVersion.Text;
                dataGridView1.CurrentRow.Cells["新零件基数"].Value = txtNewCounts.Value;
                dataGridView1.CurrentRow.Cells["旧零件父级总成编码"].Value = txtOldParentCode.Text;
                dataGridView1.CurrentRow.Cells["旧零件编码"].Value = txtOldCode.Text;
                dataGridView1.CurrentRow.Cells["旧零件名称"].Value = txtOldName.Text;
                dataGridView1.CurrentRow.Cells["旧零件规格"].Value = txtOldSpec.Text;
                dataGridView1.CurrentRow.Cells["旧零件版次号"].Value = txtOldVersion.Text;
                dataGridView1.CurrentRow.Cells["旧零件基数"].Value = txtOldCounts.Value;
                dataGridView1.CurrentRow.Cells["变更原因"].Value = "";

                if (rb_Add.Checked)
                {
                    dataGridView1.CurrentRow.Cells["变更模式"].Value = "新增";
                }
                else if (rb_Delete.Checked)
                {
                    dataGridView1.CurrentRow.Cells["变更模式"].Value = "删除";
                }
                else
                {
                    dataGridView1.CurrentRow.Cells["变更模式"].Value = "修改";
                }

                if (cbGoodsInfo.Checked)
                {
                    dataGridView1.CurrentRow.Cells["是否修改零件本身"].Value = "是";
                }
                else
                {
                    dataGridView1.CurrentRow.Cells["是否修改零件本身"].Value = "否";
                }

                if (txtNewParentCode.Tag != null)
                {
                    dataGridView1.CurrentRow.Cells["NewParentID"].Value = txtNewParentCode.Tag;
                }

                dataGridView1.CurrentRow.Cells["NewGoodsID"].Value = txtNewName.Tag;

                if (txtOldParentCode.Tag != null)
                {
                    dataGridView1.CurrentRow.Cells["OldParentID"].Value = txtOldParentCode.Tag;
                }

                dataGridView1.CurrentRow.Cells["OldGoodsID"].Value = txtOldName.Tag;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                return;
            }
            else
            {
                DataTable dt = (DataTable)dataGridView1.DataSource;

                dt.Rows.RemoveAt(dataGridView1.CurrentRow.Index);
                dataGridView1.DataSource = dt;
            }
        }

        private void 提交单据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (txtStatus.Text == "已完成")
            {
                MessageDialog.ShowPromptMessage("请重新确认单据状态");
            }
            else
            {
                GetMessage();

                if (!CheckData())
                {
                    return;
                }
                else
                {
                    DataTable dtTemp = (DataTable)dataGridView1.DataSource;

                    if (m_technologyServer.SubmitBill(m_lnqTechnology, dtTemp, out m_error))
                    {
                        MessageDialog.ShowPromptMessage("提交成功");

                        m_msgPromulgator.DestroyMessage(txtDJH.Text);
                        m_msgPromulgator.SendNewFlowMessage(txtDJH.Text,
                            string.Format("{0} 号技术变更单，请零件工程师批准", txtDJH.Text),
                            CE_RoleEnum.零件工程师);
                        this.Close();
                    }
                    else
                    {
                        MessageDialog.ShowPromptMessage(m_error);
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// 获得数据集
        /// </summary>
        public void GetMessage()
        {
            m_lnqTechnology.BillNo = txtDJH.Text.Trim();
            m_lnqTechnology.BillStatus = txtStatus.Text;
            m_lnqTechnology.ChangeReason = txtChangeReason.Text.Trim();

            if (m_byteData == null || m_byteData.ToString() == "")
            {
                m_lnqTechnology.FileMessage = null;
            }
            else
            {
                m_lnqTechnology.FileMessage = m_byteData;
            }

            m_lnqTechnology.ChangeBillNo = txtChangeCoding.Text.Trim();

            m_lnqTechnology.Applicant = BasicInfo.LoginID;
            m_lnqTechnology.ApplicantDate = ServerTime.Time;
            m_lnqTechnology.FileName = lbFileName.Text;
        }

        private void 确认通过ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (txtStatus.Text.Trim() != "等待批准")
            {
                MessageDialog.ShowPromptMessage("请重新确认单据状态");
                return;
            }

            GetMessage();

            if (!m_technologyServer.UpdateBill(m_lnqTechnology, (DataTable)dataGridView1.DataSource, out m_error))
            {
                MessageDialog.ShowPromptMessage(m_error);
                return;
            }
            else
            {
                MessageDialog.ShowPromptMessage("批准成功！");
                #region 发送知会消息

                bool flag = false;

                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    if (dataGridView1.Rows[i].Cells["是否修改零件本身"].Value.ToString() == "是")
                    {
                        flag = true;
                        break;
                    }
                }

                if (flag)
                {
                    string msg = string.Format("{0}号技术变更单,请财务复审", txtDJH.Text);

                    m_msgPromulgator.PassFlowMessage(txtDJH.Text, msg,
                             BillFlowMessage_ReceivedUserType.角色, CE_RoleEnum.会计.ToString());
                }
                else
                {
                    List<string> noticeRoles = new List<string>();
                    noticeRoles.Add(CE_RoleEnum.零件工程师.ToString());
                    noticeRoles.Add(CE_RoleEnum.开发部办公室文员.ToString());
                    noticeRoles.Add(CE_RoleEnum.采购账务管理员.ToString());

                    m_msgPromulgator.EndFlowMessage(m_lnqTechnology.BillNo,
                        string.Format("{0} 号技术变更单已经处理完毕", m_lnqTechnology.BillNo),
                        noticeRoles, null);

                }

                #endregion 发送知会消息
            }

            this.Close();
        }

        private void 技术变更处置明细_FormClosing(object sender, FormClosingEventArgs e)
        {
            m_billNoControl.CancelBill();
        }

        private void 审核ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (txtStatus.Text.Trim() != "等待财务审核")
            {
                MessageDialog.ShowPromptMessage("请重新确认单据状态");
                return;
            }

            GetMessage();

            if (!m_technologyServer.UpdateBill(m_lnqTechnology, (DataTable)dataGridView1.DataSource, out m_error))
            {
                MessageDialog.ShowPromptMessage(m_error);
                return;
            }
            else
            {
                MessageDialog.ShowPromptMessage("审核成功！");
                #region 发送知会消息

                List<string> noticeRoles = new List<string>();
                noticeRoles.Add(CE_RoleEnum.零件工程师.ToString());
                noticeRoles.Add(CE_RoleEnum.开发部办公室文员.ToString());
                noticeRoles.Add(CE_RoleEnum.采购账务管理员.ToString());

                m_msgPromulgator.EndFlowMessage(m_lnqTechnology.BillNo,
                    string.Format("{0} 号技术变更单已经处理完毕", m_lnqTechnology.BillNo),
                    noticeRoles, null);

                #endregion 发送知会消息
            }

            this.Close();
        }

        private void cbGoodsInfo_CheckedChanged(object sender, EventArgs e)
        {
            if (cbGoodsInfo.Checked)
            {
                txtNewName.Visible = false;
                txtNewUseName.Visible = true;
                txtNewName.ReadOnly = false;
                txtNewCode.ReadOnly = false;
                txtNewSpec.ReadOnly = false;
                txtNewCounts.ReadOnly = false;
                txtNewVersion.ReadOnly = false;
                txtNewCounts.ReadOnly = true;
                txtNewVersion.ReadOnly = true;

                txtNewName.Tag = txtOldName.Tag;
                txtNewUseName.ReadOnly = false;
            }
            else
            {
                txtNewUseName.Visible = false;
                txtNewName.Visible = true;
                txtNewName.Enabled = true;
                txtNewCode.ReadOnly = true;
                txtNewSpec.ReadOnly = true;
                txtNewCounts.ReadOnly = false;
                txtNewVersion.ReadOnly = false;
            }
        }

        private void 回退单据ToolStripMenuItem_Click(object sender, EventArgs e)
        {           
            if (txtStatus.Text.Trim() != "已完成")
            {
                回退单据 form = new 回退单据(CE_BillTypeEnum.技术变更处置单, txtDJH.Text, txtStatus.Text);

                if (form.ShowDialog() == DialogResult.OK)
                {
                    if (MessageBox.Show("您确定要回退此单据吗？", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        if (m_technologyServer.ReturnBill(form.StrBillID,
                            form.StrBillStatus, out m_error, form.Reason))
                        {
                            MessageDialog.ShowPromptMessage("回退成功");

                        }
                        else
                        {
                            MessageDialog.ShowPromptMessage(m_error);
                        }
                    }

                    this.Close();
                }
            }
            else
            {
                MessageDialog.ShowPromptMessage("请重新确认单据状态");
            }
        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            if (dataGridView1.Columns[e.ColumnIndex].ValueType == typeof(decimal))
            {
                e.Cancel = true;
            }
            else if (dataGridView1.Columns[e.ColumnIndex].ValueType == typeof(int))
            {
                e.Cancel = true;
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtNewParentCode.Text = dataGridView1.CurrentRow.Cells["新零件父级编码"].Value.ToString();
            txtNewCode.Text = dataGridView1.CurrentRow.Cells["新零件编码"].Value.ToString();
            txtNewSpec.Text = dataGridView1.CurrentRow.Cells["新零件规格"].Value.ToString();
            txtNewVersion.Text = dataGridView1.CurrentRow.Cells["新零件版次号"].Value.ToString();
            txtNewCounts.Value = Convert.ToInt32(dataGridView1.CurrentRow.Cells["新零件基数"].Value
                == DBNull.Value ? "0" : dataGridView1.CurrentRow.Cells["新零件基数"].Value);
            txtOldParentCode.Text = dataGridView1.CurrentRow.Cells["旧零件父级总成编码"].Value.ToString();
            txtOldCode.Text = dataGridView1.CurrentRow.Cells["旧零件编码"].Value.ToString();
            txtOldName.Text = dataGridView1.CurrentRow.Cells["旧零件名称"].Value.ToString();
            txtOldSpec.Text = dataGridView1.CurrentRow.Cells["旧零件规格"].Value.ToString();
            txtOldVersion.Text = dataGridView1.CurrentRow.Cells["旧零件版次号"].Value.ToString();
            txtOldCounts.Value = Convert.ToInt32(dataGridView1.CurrentRow.Cells["旧零件基数"].Value
                == DBNull.Value ? "0" : dataGridView1.CurrentRow.Cells["旧零件基数"].Value);

            if (dataGridView1.CurrentRow.Cells["变更模式"].Value.ToString() == "新增")
            {
                rb_Add.Checked = true;
            }
            else if (dataGridView1.CurrentRow.Cells["变更模式"].Value.ToString() == "删除")
            {
                rb_Delete.Checked = true;
            }
            else
            {
                rb_Update.Checked = true;
            }

            if (dataGridView1.CurrentRow.Cells["是否修改零件本身"].Value.ToString() == "是")
            {
                cbGoodsInfo.Checked = true;
            }
            else
            {
                cbGoodsInfo.Checked = false;
            }

            if (cbGoodsInfo.Checked)
            {
                txtNewUseName.Text = dataGridView1.CurrentRow.Cells["新零件名称"].Value.ToString();
                txtNewUseName.Visible = true;
            }
            else
            {
                txtNewName.Text = dataGridView1.CurrentRow.Cells["新零件名称"].Value.ToString();
                txtNewUseName.Visible = false;
            }

            txtNewParentCode.Tag = dataGridView1.CurrentRow.Cells["NewParentID"].Value == DBNull.Value ? 0 : Convert.ToInt32( dataGridView1.CurrentRow.Cells["NewParentID"].Value);
            txtNewName.Tag = dataGridView1.CurrentRow.Cells["NewGoodsID"].Value == DBNull.Value ? 0 : Convert.ToInt32(dataGridView1.CurrentRow.Cells["NewGoodsID"].Value);
            txtOldParentCode.Tag = dataGridView1.CurrentRow.Cells["OldParentID"].Value == DBNull.Value ? 0 : Convert.ToInt32(dataGridView1.CurrentRow.Cells["OldParentID"].Value);
            txtOldName.Tag = dataGridView1.CurrentRow.Cells["OldGoodsID"].Value == DBNull.Value ? 0 : Convert.ToInt32(dataGridView1.CurrentRow.Cells["OldGoodsID"].Value);
        }
    }
}

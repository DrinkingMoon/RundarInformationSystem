using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PlatformManagement;
using System.IO;
using Service_Peripheral_HR;
using Expression;
using ServerModule;
using GlobalObject;
using UniversalControlLibrary;

namespace Form_Peripheral_HR
{
    /// <summary>
    /// 员工合同模板主界面
    /// </summary>
    public partial class UserControlLaborContractTemplet : Form
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        string error;

        /// <summary>
        /// 文件流
        /// </summary>
        byte[] picbyte;

        /// <summary>
        /// 文件名
        /// </summary>
        string pathName;

        /// <summary>
        /// 可供查找的所有字段
        /// </summary>
        string[] m_findField = null;

        /// <summary>
        /// 操作权限
        /// </summary>
        PlatformManagement.AuthorityFlag m_authorityFlag;

        /// <summary>
        /// 合同管理服务类
        /// </summary>
        ILaborContractServer m_laborServer = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<ILaborContractServer>();

        public UserControlLaborContractTemplet(PlatformManagement.FunctionTreeNodeInfo nodeInfo)
        {
            InitializeComponent();

            m_authorityFlag = nodeInfo.Authority;

            RefreshControl();
        }

        /// <summary>
        /// 清除控件
        /// </summary>
        void ClearControl()
        {
            txtRemark.Text = "";
            txtVersion.Text = "";
            cmbLaborType.Items.Clear();
            cmbLaborType.SelectedIndex = -1;
            lblAnnxeName.Text = "附件名：";
        }

        /// <summary>
        /// 刷新
        /// </summary>
        void RefreshControl()
        {
            ClearControl();

            DataTable dt = m_laborServer.GetLaborContracType();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                cmbLaborType.Items.Add(dt.Rows[i]["类别名称"].ToString());
            }

            dataGridView1.DataSource = m_laborServer.GetLaborContractTemplet();

            if (dataGridView1.Rows.Count > 0)
            {
                dataGridView1.Columns["合同附件"].Visible = false;
            }

            dataGridView1.Refresh();

            this.dataGridView1.ColumnWidthChanged -= new System.Windows.Forms.DataGridViewColumnEventHandler(
                this.dataGridView1_ColumnWidthChanged);

            ColumnWidthControl.SetDataGridView(labelTitle.Text, dataGridView1);

            this.dataGridView1.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(
                this.dataGridView1_ColumnWidthChanged);

            // 添加查询用的列
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
        /// 权限控制
        /// </summary>
        /// <param name="authorityFlag">权限标志</param>
        void AuthorityControl(PlatformManagement.AuthorityFlag authorityFlag)
        {
            FaceAuthoritySetting.SetVisibly(toolStrip1, authorityFlag);
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            FormLaborContractType frm = new FormLaborContractType();
            frm.ShowDialog();

            RefreshControl();
        }

        /// <summary>
        /// 改变组件大小
        /// </summary>
        private void UserControlLaborContractTemplet_Resize(object sender, EventArgs e)
        {
            labelTitle.Location = new Point((this.Width - labelTitle.Width) / 2, labelTitle.Location.Y);
        }

        /// <summary>
        /// 上传附件
        /// </summary>
        private void llbAddAnnex_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenFileDialog ofdPic = new OpenFileDialog();

            ofdPic.FilterIndex = 1;
            ofdPic.RestoreDirectory = true;
            ofdPic.FileName = "";

            if (ofdPic.ShowDialog() == DialogResult.OK)
            {
                string sPicPaht = ofdPic.FileName.ToString();

                FileInfo fiPicInfo = new FileInfo(sPicPaht);

                long lPicLong = fiPicInfo.Length / 1024;
                string sPicName = fiPicInfo.Name;
                string sPicDirectory = fiPicInfo.Directory.ToString();
                string sPicDirectoryPath = fiPicInfo.DirectoryName;

                //如果文件大於300KB，警告    
                if (lPicLong > 300)
                {
                    MessageBox.Show("此文件大小为" + lPicLong + "K；已超过最大限制的300K范围！");
                }
                else
                {
                    pathName = ofdPic.SafeFileName;
                    Stream ms = ofdPic.OpenFile();
                    //picSignature.Image = Image.FromFile(filepath);
                    picbyte = new byte[ms.Length];
                    ms.Position = 0;
                    ms.Read(picbyte, 0, Convert.ToInt32(ms.Length));
                    ms.Close();

                    lblAnnxeName.Text = "附件名：";
                    lblAnnxeName.Text += pathName;
                    lblAnnxeName.Visible = true;

                    if (picbyte != null)
                    {
                        llbLoadAnnex.Visible = true;
                    }
                }
            }
        }

        /// <summary>
        /// 下载附件
        /// </summary>
        private void llbLoadAnnex_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            byte[] by = picbyte;
            string filepath = "";//保存路径

            FolderBrowserDialog folder = new FolderBrowserDialog();
            OpenFileDialog ofdSelectPic = new OpenFileDialog();

            if (folder.ShowDialog() == DialogResult.OK)
            {
                filepath = folder.SelectedPath + "\\" + pathName;
                lblAnnxeName.Text += pathName;

                FileStream fs = new FileStream(filepath, FileMode.CreateNew);
                BinaryWriter bw = new BinaryWriter(fs);

                bw.Write(picbyte, 0, picbyte.Length);
                bw.Close();
                fs.Close();
                MessageDialog.ShowPromptMessage("下载成功,路径：" + filepath);
            }
            else
            {
                MessageDialog.ShowPromptMessage("请选择下载路径");
            }
        }

        private void 添加toolStripButton1_Click(object sender, EventArgs e)
        {
            if (cmbLaborType.SelectedIndex == -1 || txtVersion.Text.Trim() == "" || txtRemark.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("请把界面上的信息填写完整");
                return;
            }

            if (picbyte == null)
            {
                MessageDialog.ShowPromptMessage("请上传附件");
                return;
            }

            HR_LaborContractTemplet contractTemp = new HR_LaborContractTemplet();

            contractTemp.LaborContractTypeCode = m_laborServer.GetLaborTypeByTypeName(cmbLaborType.Text,out error);
            contractTemp.Version = Convert.ToDecimal(txtVersion.Text);

            if (picbyte != null)
            {
                contractTemp.LaborContractFile = picbyte;
                contractTemp.LaborContractFileName = pathName;
            }

            contractTemp.Remark = txtRemark.Text;
            contractTemp.Recorder = BasicInfo.LoginID;
            contractTemp.RecordTime = ServerTime.Time;

            if (!m_laborServer.AddContractTemplet(contractTemp,out error))
            {
                MessageDialog.ShowPromptMessage(error);
                return;
            }

            RefreshControl();
        }

        private void 刷新toolStripButton1_Click(object sender, EventArgs e)
        {
            RefreshControl();
        }

        private void 删除toolStripButton1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageDialog.ShowPromptMessage("选择需要删除的数据行！");
                return;
            }

            if (MessageBox.Show("您是否确定要删除" + cmbLaborType.Text.Trim()+"版本为"+txtVersion.Text.Trim()
                + "信息?", "消息", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (!m_laborServer.DeleteContractTemplet(Convert.ToInt32(dataGridView1.CurrentRow.Cells["编号"].Value) , out error))
                {
                    MessageDialog.ShowPromptMessage(error);
                    return;
                }
            }

            RefreshControl();
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            cmbLaborType.Text = dataGridView1.CurrentRow.Cells["合同类别"].Value.ToString();
            txtVersion.Text = dataGridView1.CurrentRow.Cells["合同版本"].Value.ToString();
            txtRemark.Text = dataGridView1.CurrentRow.Cells["备注"].Value.ToString();
            picbyte = dataGridView1.CurrentRow.Cells["合同附件"].Value as byte[];
            pathName = dataGridView1.CurrentRow.Cells["附件名"].Value.ToString();

            if (picbyte == null)
            {
                llbLoadAnnex.Visible = false;
                lblAnnxeName.Visible = false;
            }
            else
            {
                llbLoadAnnex.Visible = true;
                lblAnnxeName.Visible = true;
                lblAnnxeName.Text = "附件名：";
                lblAnnxeName.Text += pathName;
            }
        }

        private void dataGridView1_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            ColumnWidthControl.SaveColumnParams(labelTitle.Text, e.Column);
        }

        private void 修改toolStripButton_Click(object sender, EventArgs e)
        {
            int billNo = Convert.ToInt32(dataGridView1.CurrentRow.Cells["编号"].Value.ToString());
            HR_LaborContractTemplet ContractTemplet = new HR_LaborContractTemplet();

            ContractTemplet.Remark = txtRemark.Text;
            ContractTemplet.LaborContractTypeCode = m_laborServer.GetLaborTypeByTypeName(cmbLaborType.Text, out error);
            ContractTemplet.Version = Convert.ToDecimal(txtVersion.Text);
            ContractTemplet.Recorder = BasicInfo.LoginID;
            ContractTemplet.RecordTime = ServerTime.Time;
            ContractTemplet.ID = billNo;

            if (picbyte != null)
            {
                ContractTemplet.LaborContractFile = picbyte;
                ContractTemplet.LaborContractFileName = pathName;
            }

            if (!m_laborServer.UpdateContractTemplet(ContractTemplet, out error))
            {
                MessageDialog.ShowPromptMessage(error);
                return;
            }

            RefreshControl();
        }

        private void UserControlLaborContractTemplet_Load(object sender, EventArgs e)
        {
            AuthorityControl(m_authorityFlag);
        }
    }
}

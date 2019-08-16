using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Expression;
using Form_Peripheral_HR;
using System.IO;
using GlobalObject;
using ServerModule;
using ProvidersServerModule;
using UniversalControlLibrary;
using PlatformManagement;

namespace ProvidersExpression
{
    public partial class 供应商基础信息显示 : Form
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        string m_error;

        /// <summary>
        /// 文件流
        /// </summary>
        byte[] m_picbyte;

        /// <summary>
        /// 文件名
        /// </summary>
        string m_pathName;

        /// <summary>
        /// 权限控制标志
        /// </summary>
        AuthorityFlag m_authFlag;

        /// <summary>
        /// 供应商联系人选中的行
        /// </summary>
        int m_dgvLinkManSelectRow;

        /// <summary>
        /// 供应商编号
        /// </summary>
        string m_providerCode;

        /// <summary>
        /// 供应商责任人
        /// </summary>
        List<ProviderPrincipal> m_listPrincipal = new List<ProviderPrincipal>();

        /// <summary>
        /// 供应商所供零件
        /// </summary>
        List<P_ProviderGoods> m_listProvidersGoods = new List<P_ProviderGoods>();

        /// <summary>
        /// 供应商档案信息
        /// </summary>
        P_ProvidersBaseInfo m_providerBaseInfo = new P_ProvidersBaseInfo();

        /// <summary>
        /// 供应商联系人
        /// </summary>
        List<P_ProviderLinkMan> m_listLinkMan = new List<P_ProviderLinkMan>();

        /// <summary>
        /// 供应商档案服务类
        /// </summary>
        IProvidersBaseServer m_providerServer = ProvidersServerModule.ServerModuleFactory.GetServerModule<IProvidersBaseServer>();

        public 供应商基础信息显示(AuthorityFlag authFlag,string providerCode)
        {
            InitializeComponent();

            m_authFlag = authFlag;
            AuthorityControl(m_authFlag);

            txtRecord.Text = BasicInfo.LoginName;
            txtRecordDate.Text = ServerTime.Time.ToString();
            m_providerCode = providerCode;
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
        /// 刷新数据
        /// </summary>
        private void RefreshControl()
        {
            P_ProvidersBaseInfo baseInfo = m_providerServer.GetBaseInfoByCode(m_providerCode,out m_error);

            if (baseInfo != null)
            {
                txtAddress.Text = baseInfo.Address;
                txtBankCode.Text = baseInfo.BankCode;
                txtCode.Text = baseInfo.ProviderCode;
                txtCompanyWeb.Text = baseInfo.CompanyWeb;
                txtEmail.Text = baseInfo.Email;
                txtFaxNo.Text = baseInfo.FaxNo;
                txtLegalRepresenta.Text = baseInfo.LegalRepresenta;
                txtOpenBank.Text = baseInfo.OpenBank;
                txtOpenInvoiceNumber.Text = baseInfo.OpenInvoiceNumber;
                txtPostcode.Text = baseInfo.Postcode;
                txtProviderCode.Text = baseInfo.ProviderCode;
                txtProviderName.Text = baseInfo.ProviderName;
                txtRecord.Text = UniversalFunction.GetPersonnelName(baseInfo.Record);
                txtRecordDate.Text = baseInfo.RecordDate.ToString();
                txtRemark.Text = baseInfo.Remark;
                txtShortName.Text = baseInfo.ShortName;
                txtTaxpayerNumber.Text = baseInfo.TaxpayerNumber;

                cmbStatus.Text = baseInfo.Status;
            }

            DataTable linkManDt = m_providerServer.GetLinkManByCode(m_providerCode);

            if (linkManDt != null && linkManDt.Rows.Count > 0)
            {
                dgvLinkMan.DataSource = linkManDt;
            }

            DataTable principalDt = m_providerServer.GetProviderPrincipal(m_providerCode);

            if (principalDt != null && principalDt.Rows.Count > 0)
            {
                dgvDutyOfficer.DataSource = principalDt;
            }
        }

        private void 添加联系人toolStripButton_Click(object sender, EventArgs e)
        {
            DataGridViewRow dr = new DataGridViewRow();
            dgvLinkMan.Rows.Add(dr);
        }

        private void 删除联系人toolStripButton_Click(object sender, EventArgs e)
        {
            dgvLinkMan.Rows.RemoveAt(m_dgvLinkManSelectRow - 1);
        }

        private void dgvLinkMan_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            m_dgvLinkManSelectRow = dgvLinkMan.CurrentRow.Index + 1;
        }

        private void dgvLinkMan_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }

        private void dgvLinkMan_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dgvLinkMan.IsCurrentCellDirty)
            {
                dgvLinkMan.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void 附件toolStripButton_Click(object sender, EventArgs e)
        {
            TabPage page = new TabPage();

            page.Text = "供应商附件";
            page.Name = "供应商附件";
            //page.Controls.Add(new FormResumeList(m_authFlag, m_personnelArchive.ID_Card).groupBox4);
            //page.Controls.Add(new FormResumeList(m_authFlag, m_personnelArchive.ID_Card).groupBox3);
            //page.Controls.Add(new FormResumeList(m_authFlag, m_personnelArchive.ID_Card).groupBox2);
            //page.Controls.Add(new FormResumeList(m_authFlag, m_personnelArchive.ID_Card).panel5);

            bool flag = false;

            for (int i = 0; i < tabControlMain.TabPages.Count; i++)
            {
                if (tabControlMain.TabPages[i].Name.Equals("供应商附件"))
                {
                    flag = true;
                    tabControlMain.TabPages.RemoveAt(i);
                }
            }

            if (!flag)
            {
                tabControlMain.TabPages.Add(page);
                tabControlMain.SelectTab(tabControlMain.TabCount - 1);
            }
        }

        private void 添加责任人toolStripButton_Click(object sender, EventArgs e)
        {
            FormSelectPersonnel form = new FormSelectPersonnel("员工");

            if (dgvDutyOfficer.Rows.Count > 0)
            {
                List<View_SelectPersonnel> list = new List<View_SelectPersonnel>();

                for (int i = 0; i < dgvDutyOfficer.Rows.Count; i++)
                {
                    View_SelectPersonnel person = new View_SelectPersonnel();

                    person.员工编号 = dgvDutyOfficer.Rows[i].Cells["员工编号"].Value.ToString();
                    list.Add(person);
                }

                form.SelectedUser = list;
            }

            form.DeptCode = BasicInfo.DeptCode;

            if (form.ShowDialog() == DialogResult.OK)
            {
                dgvDutyOfficer.DataSource = form.SelectedUser;
            }
        }

        private void 删除责任人toolStripButton_Click(object sender, EventArgs e)
        {
            for (int i = dgvDutyOfficer.SelectedRows.Count; i > 0; i--)
            {
                dgvDutyOfficer.Rows.Remove(dgvDutyOfficer.SelectedRows[i - 1]);
            }
        }

        private void dgvDutyOfficer_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dgvDutyOfficer.IsCurrentCellDirty)
            {
                dgvDutyOfficer.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        /// <summary>
        /// 上传附件
        /// </summary>
        private void llbUpAnnex_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
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

                //如果文件大於150KB，警告    
                if (lPicLong > 10000)
                {
                    MessageBox.Show("此文件大小为" + lPicLong + "K；已超过最大限制的10M范围！");
                }
                else
                {
                    m_pathName = ofdPic.SafeFileName;
                    lblAnnexName.Text = m_pathName;

                    Stream ms = ofdPic.OpenFile();

                    m_picbyte = new byte[ms.Length];
                    ms.Position = 0;
                    ms.Read(m_picbyte, 0, Convert.ToInt32(ms.Length));
                    ms.Close();
                }
            }
        }

        /// <summary>
        /// 下载附件
        /// </summary>
        private void llbDownAnnex_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            byte[] by = m_picbyte;
            string filepath = "";//保存路径

            FolderBrowserDialog folder = new FolderBrowserDialog();
            OpenFileDialog ofdSelectPic = new OpenFileDialog();

            if (folder.ShowDialog() == DialogResult.OK)
            {
                filepath = folder.SelectedPath + "\\" + m_pathName;

                FileStream fs = new FileStream(filepath, FileMode.CreateNew);
                BinaryWriter bw = new BinaryWriter(fs);

                bw.Write(m_picbyte, 0, m_picbyte.Length);
                bw.Close();
                fs.Close();
                MessageBox.Show("下载成功,路径：" + filepath);
            }
            else
            {
                MessageBox.Show("请选择下载路径");
            }
        }

        /// <summary>
        /// 清空控件
        /// </summary>
        void ClearControl()
        {
            foreach (Control cl in this.panel1.Controls)
            {
                if (cl is TextBox)
                {
                    ((TextBox)cl).Text = "";
                }
                else if (cl is ComboBox)
                {
                    ((ComboBox)cl).SelectedIndex = -1;
                }
            }

            m_picbyte = null;
            lblAnnexName.Text = "";
            txtRecord.Text = BasicInfo.LoginName;
            txtRecordDate.Text = ServerTime.Time.ToString();
        }

        private void 新建toolStripButton1_Click(object sender, EventArgs e)
        {
            ClearControl();

            if (dgvLinkMan.Rows.Count > 0)
            {
                for (int i = 0; i < dgvLinkMan.Rows.Count; i++)
                {
                    dgvLinkMan.Rows.RemoveAt(i);
                    i--;
                }
            }

            if (dgvDutyOfficer.Rows.Count > 0)
            {
                dgvDutyOfficer.DataSource = null;
            }
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <returns>获取成功返回true，否则false</returns>
        public bool GetBaseInfo()
        {
            m_listLinkMan = new List<P_ProviderLinkMan>();
            m_listPrincipal = new List<ProviderPrincipal>();
            m_listProvidersGoods = new List<P_ProviderGoods>();
            m_providerBaseInfo = new P_ProvidersBaseInfo();

            m_providerBaseInfo.Address = txtAddress.Text;

            if (m_picbyte != null)
            {
                m_providerBaseInfo.Annex = m_picbyte;
                m_providerBaseInfo.AnnexName = m_pathName;
            }

            m_providerBaseInfo.BankCode = txtBankCode.Text;
            m_providerBaseInfo.CompanyWeb = txtCompanyWeb.Text;
            m_providerBaseInfo.Email = txtEmail.Text;
            m_providerBaseInfo.FaxNo = txtFaxNo.Text;
            m_providerBaseInfo.IsPapers = cmbIsPapers.Text == "是" ? true : false;
            m_providerBaseInfo.LegalRepresenta = txtLegalRepresenta.Text;
            m_providerBaseInfo.OpenBank = txtOpenBank.Text;
            m_providerBaseInfo.OpenInvoiceNumber = txtOpenInvoiceNumber.Text;
            m_providerBaseInfo.Postcode = txtPostcode.Text;
            m_providerBaseInfo.Property = cmbProperty.Text;
            m_providerBaseInfo.ProviderCode = txtProviderCode.Text;
            m_providerBaseInfo.ProviderName = txtProviderName.Text;
            m_providerBaseInfo.ProviderType = cmbProviderType.Text;
            m_providerBaseInfo.Record = BasicInfo.LoginID;
            m_providerBaseInfo.RecordDate = ServerTime.Time;
            m_providerBaseInfo.Remark = txtRemark.Text;
            m_providerBaseInfo.ShortName = txtShortName.Text;
            m_providerBaseInfo.Status = cmbStatus.Text;
            m_providerBaseInfo.TaxpayerNumber = txtTaxpayerNumber.Text;

            for (int i = 0; i < dgvDutyOfficer.Rows.Count; i++)
            {
                ProviderPrincipal principal = new ProviderPrincipal();

                principal.IsMainDuty = Convert.ToBoolean(dgvDutyOfficer.Rows[i].Cells["是否主要责任人"].Value);
                principal.PrincipalWorkId = dgvDutyOfficer.Rows[i].Cells["员工编号"].Value.ToString();
                principal.Provider = txtProviderCode.Text;

                m_listPrincipal.Add(principal);
            }

            for (int i = 0; i < dgvGoods.Rows.Count; i++)
            {
                P_ProviderGoods goodsInfo = new P_ProviderGoods();

                goodsInfo.GoodsCode = dgvGoods.Rows[i].Cells["图号型号"].Value.ToString();
                goodsInfo.GoodsName = dgvGoods.Rows[i].Cells["物品名称"].Value.ToString();
                goodsInfo.Spec = dgvGoods.Rows[i].Cells["规格"].Value.ToString();
                goodsInfo.Price = Convert.ToDecimal(dgvGoods.Rows[i].Cells["购买价"].Value);
                goodsInfo.Quote = Convert.ToDecimal(dgvGoods.Rows[i].Cells["报价"].Value);
                goodsInfo.Remark = dgvGoods.Rows[i].Cells["备注"].Value.ToString();
                goodsInfo.Status = dgvGoods.Rows[i].Cells["零件状态"].Value.ToString();
                goodsInfo.Type = m_providerServer.GetGoodsType(dgvGoods.Rows[i].Cells["零件类别"].Value.ToString()).ID;

                m_listProvidersGoods.Add(goodsInfo);
            }

            for (int i = 0; i < dgvLinkMan.Rows.Count; i++)
            {
                P_ProviderLinkMan linkman = new P_ProviderLinkMan();

                linkman.Email = (string)dgvLinkMan.Rows[i].Cells["电子邮件地址"].Value;
                linkman.FaxNo = (string)dgvLinkMan.Rows[i].Cells["传真号码"].Value;
                linkman.Mobilephone = (string)dgvLinkMan.Rows[i].Cells["手机号码"].Value;
                linkman.Name = dgvLinkMan.Rows[i].Cells["姓名"].Value.ToString();
                linkman.Position = (string)dgvLinkMan.Rows[i].Cells["职位"].Value;
                linkman.ProviderCode = txtProviderCode.Text;
                linkman.Remark = (string)dgvLinkMan.Rows[i].Cells["备注"].Value;
                linkman.Sex = (string)dgvLinkMan.Rows[i].Cells["性别"].Value;
                linkman.Telephone = (string)dgvLinkMan.Rows[i].Cells["电话号码"].Value;

                m_listLinkMan.Add(linkman);
            }

            return true;
        }

        /// <summary>
        /// 检测控件的正确性和完整性
        /// </summary>
        /// <returns>正确无误返回true，否则返回false</returns>
        bool CheckControl()
        {
            foreach (Control cl in this.panel1.Controls)
            {
                if (cl is TextBox)
                {
                    if (((TextBox)cl).Text.Trim() == "" && ((TextBox)cl).Name != "txtRemark")
                    {
                        MessageDialog.ShowPromptMessage("请录入完整的信息（蓝色字体的内容必须填写）！");
                        return false;
                    }
                }
            }

            if (dgvLinkMan.Rows.Count > 0)
            {
                for (int i = 0; i < dgvLinkMan.Rows.Count; i++)
                {
                    if (dgvLinkMan.Rows[i].Cells["姓名"].Value == null
                        || dgvLinkMan.Rows[i].Cells["职位"].Value == null
                        || dgvLinkMan.Rows[i].Cells["手机号码"].Value == null)
                    {
                        MessageDialog.ShowPromptMessage("请将供应商联系人第" + (i + 1).ToString() + "行的数据填写完整！");
                        return false;
                    }
                }
            }
            else
            {
                MessageDialog.ShowPromptMessage("请输入供应商联系人！");
                return false;
            }

            if (dgvDutyOfficer.Rows.Count > 0)
            {
                for (int i = 0; i < dgvDutyOfficer.Rows.Count; i++)
                {
                    if (dgvDutyOfficer.Rows[i].Cells["员工姓名"].Value.ToString().Trim() == "")
                    {
                        MessageDialog.ShowPromptMessage("请选择第" + (i - 1).ToString() + "行的供应商责任人！");
                        return false;
                    }
                }
            }
            else
            {
                MessageDialog.ShowPromptMessage("请选择供应商责任人！");
                return false;
            }

            return true;
        }

        private void 添加toolStripButton1_Click(object sender, EventArgs e)
        {
            if (!CheckControl())
            {
                return;
            }

            if (GetBaseInfo())
            {
                if (m_providerServer.AddProvidersInfo(m_providerBaseInfo, m_listPrincipal, m_listLinkMan, out m_error))
                {
                    if (!m_providerServer.AddGoodsInfo(null, m_listProvidersGoods, txtProviderCode.Text, out m_error))
                    {
                        MessageDialog.ShowPromptMessage(m_error);
                        return;
                    }
                    else
                    {
                        MessageDialog.ShowPromptMessage("保存成功！");
                    }
                }
                else
                {
                    MessageDialog.ShowPromptMessage(m_error);
                    return;
                }
            }

            新建toolStripButton1_Click(null,null);
        }

        private void 零件保存toolStripButton_Click(object sender, EventArgs e)
        {
            if (!CheckControl())
            {
                return;
            }

            if (GetBaseInfo())
            {
                if (m_providerServer.AddProvidersInfo(m_providerBaseInfo,m_listPrincipal,m_listLinkMan, out m_error))
                {
                    if (!m_providerServer.AddGoodsInfo(null, m_listProvidersGoods, txtProviderCode.Text, out m_error))
                    {
                        MessageDialog.ShowPromptMessage(m_error);
                        return;
                    }
                    else
                    {
                        MessageDialog.ShowPromptMessage("保存成功！");
                    }
                }
            }

            tabControlMain.SelectedIndex = 0;
        }

        private void tabControlMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControlMain.SelectedIndex == 1)
            {
                DataTable dt = m_providerServer.GetGoodsInfoByCode(txtProviderCode.Text);

                dgvGoods.DataSource = dt;

                if (dt != null && dt.Rows.Count > 0)
                {
                    dgvGoods.Columns["零件类别编号"].Visible = false;
                }

                txtCode.Text = txtShortName.Text;
                txtCode.Tag = txtProviderCode.Text;
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            DataTable dtTemp = (DataTable)dgvGoods.DataSource;

            for (int i = 0; i < dtTemp.Rows.Count; i++)
            {
                if (txtGoodsCode.Text == dtTemp.Rows[i]["图号型号"].ToString()
                    && txtGoodsName.Text == dtTemp.Rows[i]["物品名称"].ToString()
                    && txtSpec.Text == dtTemp.Rows[i]["规格"].ToString())
                {
                    MessageDialog.ShowPromptMessage("不能录入重复物品");
                    return;
                }
            }

            DataRow dr = dtTemp.NewRow();

            dr["图号型号"] = txtGoodsCode.Text;
            dr["物品名称"] = txtGoodsName.Text;
            dr["规格"] = txtSpec.Text;
            dr["报价"] = numQuote.Value;
            dr["购买价"] = numPrice.Value;
            dr["零件类别"] = cmbGoodsType.Text;
            dr["零件状态"] = cmbGoodsStatus.Text;
            dr["备注"] = txtGoodsRemark.Text;

            dtTemp.Rows.Add(dr);

            dgvGoods.DataSource = dtTemp;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dgvGoods.CurrentRow == null)
            {
                return;
            }
            else
            {
                dgvGoods.CurrentRow.Cells["图号型号"].Value = txtGoodsCode.Text;
                dgvGoods.CurrentRow.Cells["物品名称"].Value = txtGoodsName.Text;
                dgvGoods.CurrentRow.Cells["规格"].Value = txtSpec.Text;
                dgvGoods.CurrentRow.Cells["报价"].Value = numQuote.Value;
                dgvGoods.CurrentRow.Cells["购买价"].Value = numPrice.Value;
                dgvGoods.CurrentRow.Cells["零件类别"].Value = cmbGoodsType.Text;
                dgvGoods.CurrentRow.Cells["零件状态"].Value = cmbGoodsStatus.Text;
                dgvGoods.CurrentRow.Cells["备注"].Value = txtGoodsRemark.Text;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvGoods.CurrentRow == null)
            {
                return;
            }
            else
            {
                dgvGoods.Rows.Remove(dgvGoods.CurrentRow);
            }
        }

        private void 供应商基础信息显示_Load(object sender, EventArgs e)
        {

        }

        private void dgvGoods_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvGoods.SelectedRows.Count > 0)
            {
                txtCode.Text = (string)dgvGoods.CurrentRow.Cells["供应商"].Value;
                txtGoodsCode.Text = (string)dgvGoods.CurrentRow.Cells["图号型号"].Value;
                txtGoodsName.Text = (string)dgvGoods.CurrentRow.Cells["物品名称"].Value;
                txtGoodsRemark.Text = dgvGoods.CurrentRow.Cells["备注"].Value==DBNull.Value? "" :
                    (string)dgvGoods.CurrentRow.Cells["备注"].Value;
                txtSpec.Text = (string)dgvGoods.CurrentRow.Cells["规格"].Value;
                numPrice.Value = Convert.ToDecimal(dgvGoods.CurrentRow.Cells["报价"].Value==DBNull.Value? 0 :
                    Convert.ToDecimal(dgvGoods.CurrentRow.Cells["报价"].Value));
                numQuote.Value = Convert.ToDecimal(dgvGoods.CurrentRow.Cells["购买价"].Value == DBNull.Value ? 0 :
                    Convert.ToDecimal(dgvGoods.CurrentRow.Cells["购买价"].Value));             
            }
        }
    }
}

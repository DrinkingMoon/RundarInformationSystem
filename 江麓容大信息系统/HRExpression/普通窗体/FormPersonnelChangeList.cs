using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ServerModule;
using System.IO;
using Expression;
using UniversalControlLibrary;

namespace Form_Peripheral_HR
{
    /// <summary>
    /// 员工档案变更历史界面
    /// </summary>
    public partial class FormPersonnelChangeList : Form
    {
        /// <summary>
        /// 文件流
        /// </summary>
        byte[] picbyte;

        /// <summary>
        /// 文件名
        /// </summary>
        string pathName;

        /// <summary>
        /// 人员档案变更数据集
        /// </summary>
        View_HR_PersonnelArchiveChange m_personnelChange;

        public FormPersonnelChangeList(View_HR_PersonnelArchiveChange personnelChange)
        {
            InitializeComponent();

            m_personnelChange = personnelChange;
            BindControl();
            toolStrip1.Visible = true;
        }

        private void llbLoadAnnex_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            byte[] by = picbyte;
            string filepath = "";//保存路径

            FolderBrowserDialog folder = new FolderBrowserDialog();
            OpenFileDialog ofdSelectPic = new OpenFileDialog();

            if (folder.ShowDialog() == DialogResult.OK)
            {
                filepath = folder.SelectedPath + "\\" + pathName;

                FileStream fs = new FileStream(filepath, FileMode.CreateNew);
                BinaryWriter bw = new BinaryWriter(fs);

                bw.Write(picbyte, 0, picbyte.Length);
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
        /// 绑定控件
        /// </summary>
        void BindControl()
        {
            txtWorkID.Text = m_personnelChange.员工编号;
            txtName.Text = m_personnelChange.员工姓名;
            cmbWorkPost.Text = m_personnelChange.工作岗位;
            cmbJobTitle.Text = m_personnelChange.外部职称;
            cmbLevel.Text = m_personnelChange.内部级别;
            cbCore.Checked = (bool)m_personnelChange.是否核心员工;
            cmbSex.Text = m_personnelChange.性别;
            cmbDept.Text = m_personnelChange.部门名称;
            dtpBirthday.Value = Convert.ToDateTime(m_personnelChange.出生日期);
            txtNationality.Text = m_personnelChange.国籍;
            txtRace.Text = m_personnelChange.民族;
            txtBirthplace.Text = m_personnelChange.籍贯;
            txtParty.Text = m_personnelChange.政治面貌;
            txtCard.Text = m_personnelChange.身份证;
            txtCollege.Text = m_personnelChange.毕业院校;
            txtEducatedDegree.Text = m_personnelChange.专业;
            txtEducatedMajor.Text = m_personnelChange.文化程度;
            txtFamilyAddress.Text = m_personnelChange.家庭住址;
            txtPostcode.Text = m_personnelChange.邮编;
            txtPhone.Text = m_personnelChange.电话;
            txtSpeciality.Text = m_personnelChange.特长;
            txtMobilePhone.Text = m_personnelChange.手机;
            txtTrainingAmount.Text = m_personnelChange.培训次数.ToString();
            txtPostAmount.Text = m_personnelChange.调动次数.ToString();
            txtBank.Text = m_personnelChange.开户银行;
            txtBankAccount.Text = m_personnelChange.银行账号;
            txtQQ.Text = m_personnelChange.QQ号码;
            txtEmail.Text = m_personnelChange.电子邮箱;
            txtHobby.Text = m_personnelChange.爱好;
            txtSSNumber.Text = m_personnelChange.社会保障号;
            txtResume.Value = Convert.ToDecimal(m_personnelChange.简历编号);
            dateTimePicker1.Value = (DateTime)m_personnelChange.入司时间;
            txtJobNature.Text = m_personnelChange.工作性质;
            txtGraduationYear.Text = m_personnelChange.毕业年份.ToString();
            txtLengthOfSchooling.Text = m_personnelChange.学制;
            cmbMaritalStatus.Text = m_personnelChange.婚姻状况;

            if (m_personnelChange.转正日期.ToString() != "")
            {
                dtpBecomeDate.Value = Convert.ToDateTime(m_personnelChange.转正日期);
            }

            if (m_personnelChange.参加工作的时间.ToString() != "")
            {
                dtpTakeJobDate.Value = Convert.ToDateTime(m_personnelChange.参加工作的时间);
            }

            if (m_personnelChange.照片 != null)
            {
                picbPhoto.Image = m_personnelChange.照片 == null ? null : GetPicture(m_personnelChange.照片.ToArray());
            }

            if (m_personnelChange.附件 != null)
            {
                picbyte = m_personnelChange.附件 == null ? null : m_personnelChange.附件.ToArray();
                pathName = m_personnelChange.附件名;
                lblAnnexName.Text = pathName;
            }
            else
            {
                llbLoadAnnex.Visible = false;
                lblAnnexName.Visible = false;
            }

            txtRemark.Text = m_personnelChange.备注;

            txtName.ReadOnly = true;
            txtCard.ReadOnly = true;
            txtWorkID.ReadOnly = true;
        }

        /// <summary>
        /// 二进制转化成图像
        /// </summary>
        /// <param name="byPct">二进制</param>
        /// <returns>返回单个图像</returns>
        private static Image GetPicture(byte[] byPct)
        {
            MemoryStream ms = new MemoryStream(byPct);
            ms.Write(byPct, 0, byPct.Length);
            Image img = Bitmap.FromStream(ms);

            ms.Close();
            return img;
        }

        private void 打印toolStripButton1_Click(object sender, EventArgs e)
        {
            报表_人员档案变更历史 report = new 报表_人员档案变更历史(m_personnelChange.编号);

            PrintReportBill print = new PrintReportBill();
            (report as 报表_人员档案变更历史).ShowDialog();
        }

        private void FormPersonnelChangeList_Resize(object sender, EventArgs e)
        {
            labelTitle.Location = new Point((this.Width - labelTitle.Width) / 2, labelTitle.Location.Y);
        }
    }
}

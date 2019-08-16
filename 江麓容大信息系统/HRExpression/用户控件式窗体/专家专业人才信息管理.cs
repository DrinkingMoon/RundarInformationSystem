using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PlatformManagement;
using Service_Peripheral_HR;
using UniversalControlLibrary;
using ServerModule;
using GlobalObject;

namespace Form_Peripheral_HR
{
    public partial class 专家专业人才信息管理 : Form
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        string m_error;

        /// <summary>
        /// 操作权限
        /// </summary>
        PlatformManagement.AuthorityFlag m_authorityFlag;

        /// <summary>
        /// 可供查找的所有字段
        /// </summary>
        string[] m_findField = null;

        /// <summary>
        /// 查询结果
        /// </summary>
        IQueryResult m_queryResult;

        /// <summary>
        /// 专家专业人才管理类
        /// </summary>
        IExpertEmployeServer m_expertServer = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<IExpertEmployeServer>();

        public 专家专业人才信息管理(PlatformManagement.FunctionTreeNodeInfo nodeInfo)
        {
            InitializeComponent();
            m_authorityFlag = nodeInfo.Authority;
        }

        /// <summary>
        /// 权限控制
        /// </summary>
        /// <param name="authorityFlag">权限标志</param>
        void AuthorityControl(PlatformManagement.AuthorityFlag authorityFlag)
        {
            FaceAuthoritySetting.SetVisibly(toolStrip1, authorityFlag);
        }

        private void 专家专业人才信息管理_Load(object sender, EventArgs e)
        {
            AuthorityControl(m_authorityFlag);
            RefreshControl();
        }

        /// <summary>
        /// 刷新
        /// </summary>
        private void RefreshControl()
        {
            if (!m_expertServer.GetAllInfo(out m_queryResult, out m_error))
            {
                MessageDialog.ShowErrorMessage(m_error);
                return;
            }

            m_queryResult.DataGridView = dataGridView1;

            DataTable dt = m_queryResult.DataCollection.Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                dataGridView1.Columns["序号"].Visible = false;
                dataGridView1.Columns["出生年月"].Visible = false;
            }

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

            dataGridView1.Refresh();

            dataGridView1.Columns[0].Frozen = true;
            dataGridView1.Columns[1].Frozen = true;
        }

        /// <summary>
        /// 改变标题文本的距离
        /// </summary>
        private void 专家专业人才信息管理_Resize(object sender, EventArgs e)
        {
            labelTitle.Location = new Point((this.Width - labelTitle.Width) / 2, labelTitle.Location.Y);
        }

        private void dataGridView1_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            ColumnWidthControl.SaveColumnParams(labelTitle.Text, e.Column);
        }

        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {

        }

        private void txtOnAccess_OnCompleteSearch()
        {
            txtOnAccess.Text = txtOnAccess.DataResult["姓名"].ToString();
            txtOnAccess.Tag = txtOnAccess.DataResult["工号"].ToString();
        }

        private void 综合查询toolStripButton3_Click(object sender, EventArgs e)
        {
            IAuthorization authorization = PlatformFactory.GetObject<IAuthorization>();
            string businessID = "查看专家专业人才库";
            IQueryResult qr = authorization.Query(businessID, null, null, 0);
            List<string> lstFindField = new List<string>();
            DataColumnCollection columns = qr.DataCollection.Tables[0].Columns;

            if (qr.Succeeded && columns.Count > 0)
            {
                ExcelHelperP.DatagridviewToExcel(saveFileDialog1, dataGridView1);

                for (int i = 0; i < columns.Count; i++)
                {
                    lstFindField.Add(columns[i].ColumnName);
                }
            }

            FormConditionFind formFindCondition = new FormConditionFind(this, lstFindField.ToArray(), businessID, labelTitle.Text);
            formFindCondition.ShowDialog();
        }

        /// <summary>
        /// 初始化控件
        /// </summary>
        void ClearControl()
        {
            txtName.Text = "";
            cmbSex.SelectedIndex = -1;
            dtpBirthday.Value = ServerTime.Time;
            numAge.Value = 0;
            txtPhone.Text = "";
            txtEmail.Text = "";
            txtMessage.Text = "";
            txtOnAccess.Text = "";
            txtOnAccess.Tag = null;
            txtDeclaration.Text = "";
            txtAddress.Text = "";
            txtCurrentCompany.Text = "";
            txtDomain.Text = "";
            txtStrong.Text = "";
            txtTechnicalPost.Text = "";
            txtIntroduction.Text = "";
            txtRemark.Text = "";
            txtRecorder.Text = BasicInfo.LoginName;
            txtRecordTime.Text = ServerTime.Time.ToString();
        }

        private void 新建toolStripButton1_Click(object sender, EventArgs e)
        {
            ClearControl();
        }

        private void 刷新toolStripButton1_Click(object sender, EventArgs e)
        {
            RefreshControl();
        }

        private void 导出toolStripButton_Click(object sender, EventArgs e)
        {
            ExcelHelperP.DatagridviewToExcel(saveFileDialog1, dataGridView1);
        }

        /// <summary>
        /// 获得专家专业人才数据集
        /// </summary>
        /// <returns>返回专家专业人才对象</returns>
        HR_ExpertEmploye GetExpertEmployeData()
        {
            HR_ExpertEmploye expertEmploye = new HR_ExpertEmploye();

            expertEmploye.Address = txtAddress.Text;
            expertEmploye.Age = Convert.ToInt32(numAge.Value);
            expertEmploye.Birthday = dtpBirthday.Value;
            expertEmploye.CurrentCompany = txtCurrentCompany.Text;
            expertEmploye.Declaration = txtDeclaration.Text;
            expertEmploye.Domain = txtDomain.Text;
            expertEmploye.Email = txtEmail.Text;
            expertEmploye.Introduction = txtIntroduction.Text;
            expertEmploye.MessageSource = txtMessage.Text;
            expertEmploye.Name = txtName.Text;
            expertEmploye.OnAccess = txtOnAccess.Text;
            expertEmploye.Phone = txtPhone.Text;
            expertEmploye.Recorder = BasicInfo.LoginID;
            expertEmploye.RecordTime = ServerTime.Time;
            expertEmploye.Remark = txtRemark.Text;
            expertEmploye.Sex = cmbSex.Text;
            expertEmploye.Strong = txtStrong.Text;
            expertEmploye.TechnicalPost = txtTechnicalPost.Text;

            return expertEmploye;
        }

        /// <summary>
        /// 检查录入是否正确
        /// </summary>
        /// <returns>正确返回true失败返回false</returns>
        bool CheckControl()
        {
            if (txtName.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("请输入正确的姓名！");
                return false;
            }

            if (dtpBirthday.Value.Year == ServerTime.Time.Year)
            {
                MessageDialog.ShowPromptMessage("请输入正确的出生年月！");
                return false;
            }

            if (txtOnAccess.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("请选择日常对接人！");
                return false;
            }

            return true;
        }

        private void 保存toolStripButton_Click(object sender, EventArgs e)
        {
            if (!CheckControl())
            {
                return;
            }

            if (!m_expertServer.AddExpertEmploye(GetExpertEmployeData(), out m_error))
            {
                MessageDialog.ShowPromptMessage(m_error);
            }
            else
            {
                MessageDialog.ShowPromptMessage("保存成功！");
                RefreshControl();
            }
        }

        private void dtpBirthday_ValueChanged(object sender, EventArgs e)
        {
            numAge.Value = ServerTime.Time.Year - dtpBirthday.Value.Year;
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            txtName.Text = dataGridView1.CurrentRow.Cells["姓名"].Value.ToString();
            cmbSex.Text = dataGridView1.CurrentRow.Cells["性别"].Value.ToString();
            dtpBirthday.Value = Convert.ToDateTime(dataGridView1.CurrentRow.Cells["出生年月"].Value);
            numAge.Value = Convert.ToInt32(dataGridView1.CurrentRow.Cells["年龄"].Value);
            txtPhone.Text = dataGridView1.CurrentRow.Cells["联系电话"].Value.ToString();
            txtEmail.Text = dataGridView1.CurrentRow.Cells["邮箱/QQ/MSN"].Value.ToString();
            txtMessage.Text = dataGridView1.CurrentRow.Cells["信息来源"].Value.ToString();
            txtOnAccess.Text = dataGridView1.CurrentRow.Cells["日常对接人"].Value.ToString();
            txtDeclaration.Text = dataGridView1.CurrentRow.Cells["信息申报人"].Value.ToString();
            txtAddress.Text = dataGridView1.CurrentRow.Cells["目前居住地"].Value.ToString();
            txtCurrentCompany.Text = dataGridView1.CurrentRow.Cells["目前就职"].Value.ToString();
            txtDomain.Text = dataGridView1.CurrentRow.Cells["专业领域"].Value.ToString();
            txtStrong.Text = dataGridView1.CurrentRow.Cells["个人强项"].Value.ToString();
            txtTechnicalPost.Text = dataGridView1.CurrentRow.Cells["职务职称"].Value.ToString();
            txtIntroduction.Text = dataGridView1.CurrentRow.Cells["简介（从事领域基本情况）"].Value.ToString();
            txtRemark.Text = dataGridView1.CurrentRow.Cells["备注"].Value.ToString();
            txtRecorder.Text = dataGridView1.CurrentRow.Cells["建档人"].Value.ToString();
            txtRecordTime.Text = dataGridView1.CurrentRow.Cells["建档时间"].Value.ToString();
        }

        private void 修改toolStripButton_Click(object sender, EventArgs e)
        {
            if (!CheckControl())
            {
                return;
            }

            try
            {
                int id = Convert.ToInt32(dataGridView1.CurrentRow.Cells["序号"].Value);

                if (!m_expertServer.UpdateExpertEmploye(GetExpertEmployeData(), id, out m_error))
                {
                    MessageDialog.ShowPromptMessage(m_error);
                }
                else
                {
                    MessageDialog.ShowPromptMessage("修改成功！");
                    RefreshControl();
                    PositioningRecord(id.ToString());
                }
            }
            catch (Exception)
            {
                throw;
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
                if (dataGridView1.Rows[i].Cells["序号"].Value.ToString() == billNo)
                {
                    dataGridView1.FirstDisplayedScrollingRowIndex = i;
                    dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells[strColName];
                    break;
                }
            }
        }

        private void 删除toolStripButton_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int id = Convert.ToInt32(dataGridView1.CurrentRow.Cells["序号"].Value);

                if (MessageDialog.ShowEnquiryMessage("确定删除【" + txtName.Text + "】的信息吗？") == DialogResult.Yes)
                {
                    if (!m_expertServer.DeleteTrainEmploye(id, out m_error))
                    {
                        MessageDialog.ShowPromptMessage(m_error);
                    }
                    else
                    {
                        MessageDialog.ShowPromptMessage("删除成功！");
                        RefreshControl();
                    }
                }
            }
            else
            {
                MessageDialog.ShowPromptMessage("请选择一行数据后再操作！");
            }
        }
    }
}

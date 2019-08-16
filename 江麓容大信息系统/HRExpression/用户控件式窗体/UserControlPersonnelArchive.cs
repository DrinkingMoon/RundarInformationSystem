using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PlatformManagement;
using Expression;
using Service_Peripheral_HR;
using ServerModule;
using GlobalObject;
using System.IO;
using UniversalControlLibrary;

namespace Form_Peripheral_HR
{
    /// <summary>
    /// 员工档案主界面
    /// </summary>
    public partial class UserControlPersonnelArchive : Form
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        string error;

        /// <summary>
        /// 离职时间
        /// </summary>
        string allowDate;

        /// <summary>
        /// 合同起始时间
        /// </summary>
        //string starTime;

        /// <summary>
        /// 合同终止时间
        /// </summary>
        string endTime;

        /// <summary>
        /// 操作权限
        /// </summary>
        PlatformManagement.AuthorityFlag m_authorityFlag;

        /// <summary>
        /// 人员档案变更数据集
        /// </summary>
        HR_PersonnelArchiveChange personnelChange;

        /// <summary>
        /// 可供查找的所有字段
        /// </summary>
        string[] m_findField = null;

        /// <summary>
        /// 查询结果
        /// </summary>
        IQueryResult m_queryResult;

        /// <summary>
        /// 岗位管理服务类
        /// </summary>
        IOperatingPostServer m_PostServer = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<IOperatingPostServer>();

        /// <summary>
        /// 职称信息管理类
        /// </summary>
        IJobTitleServer m_JobServer = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<IJobTitleServer>();

        /// <summary>
        /// 部门信息服务组件
        /// </summary>
        IOrganizationServer m_departmentServer = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<IOrganizationServer>();

        /// <summary>
        /// 人员档案管理类
        /// </summary>
        IPersonnelArchiveServer m_personnerServer = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<IPersonnelArchiveServer>();

        public UserControlPersonnelArchive(PlatformManagement.FunctionTreeNodeInfo nodeInfo)
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
                if (dataGridView1.Rows[i].Cells["员工编号"].Value.ToString() == billNo)
                {
                    dataGridView1.FirstDisplayedScrollingRowIndex = i;
                    dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells[strColName];
                    break;
                }
            }
        }

        /// <summary>
        /// 窗体加载
        /// </summary>
        private void UserControlPersonnelArchive_Load(object sender, EventArgs e)
        {
            AuthorityControl(m_authorityFlag);
            RefreshControl();
        }

        /// <summary>
        /// 改变组件大小
        /// </summary>
        private void UserControlPersonnelArchive_Resize(object sender, EventArgs e)
        {
            labelTitle.Location = new Point((this.Width - labelTitle.Width) / 2, labelTitle.Location.Y);
        }

        /// <summary>
        /// 刷新
        /// </summary>
        void RefreshControl()
        {
            if (!m_personnerServer.GetAllBill(out m_queryResult, out error))
            {
                MessageDialog.ShowErrorMessage(error);
                return;
            }

            m_queryResult.DataGridView = dataGridView1;

            DataTable dt = m_queryResult.DataCollection.Tables[0];
            
            if (dt != null && dt.Rows.Count > 0)
            {
                dataGridView1.Columns["拼音"].Visible = false;
                dataGridView1.Columns["五笔"].Visible = false;
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
        /// 绑定控件
        /// </summary>
        void BindControl()
        {
            if (dataGridView1.Rows.Count > 0)
            {
                personnelChange = new HR_PersonnelArchiveChange();

                personnelChange.WorkID = dataGridView1.CurrentRow.Cells["员工编号"].Value.ToString();
                personnelChange.Name = dataGridView1.CurrentRow.Cells["员工姓名"].Value.ToString();
                personnelChange.WorkPost = dataGridView1.CurrentRow.Cells["岗位"].Value.ToString();
                personnelChange.JobTitle = dataGridView1.CurrentRow.Cells["外部职称"].Value.ToString();
                personnelChange.JobLevel = dataGridView1.CurrentRow.Cells["内部级别"].Value.ToString();
                personnelChange.JoinDate = Convert.ToDateTime(dataGridView1.CurrentRow.Cells["入司时间"].Value);
                personnelChange.Sex = dataGridView1.CurrentRow.Cells["性别"].Value.ToString();
                personnelChange.DeptName = dataGridView1.CurrentRow.Cells["部门"].Value.ToString();
                personnelChange.Dept = m_departmentServer.GetDeptCode(dataGridView1.CurrentRow.Cells["部门"].Value.ToString());
                personnelChange.Birthday = Convert.ToDateTime(dataGridView1.CurrentRow.Cells["出生日期"].Value);
                personnelChange.Nationality = dataGridView1.CurrentRow.Cells["国籍"].Value.ToString();
                personnelChange.Race = dataGridView1.CurrentRow.Cells["民族"].Value.ToString();
                personnelChange.Birthplace = dataGridView1.CurrentRow.Cells["籍贯"].Value.ToString();
                personnelChange.Party = dataGridView1.CurrentRow.Cells["政治面貌"].Value.ToString();
                personnelChange.ID_Card = dataGridView1.CurrentRow.Cells["身份证"].Value.ToString();
                personnelChange.College = dataGridView1.CurrentRow.Cells["毕业院校"].Value.ToString();
                personnelChange.EducatedDegree = dataGridView1.CurrentRow.Cells["文化程度"].Value.ToString();
                personnelChange.EducatedMajor = dataGridView1.CurrentRow.Cells["专业"].Value.ToString();
                personnelChange.FamilyAddress = dataGridView1.CurrentRow.Cells["家庭地址"].Value.ToString();
                personnelChange.Phone = dataGridView1.CurrentRow.Cells["电话"].Value.ToString();
                personnelChange.Speciality = dataGridView1.CurrentRow.Cells["特长"].Value.ToString();
                personnelChange.MobilePhone = dataGridView1.CurrentRow.Cells["手机"].Value.ToString();
                personnelChange.QQ = dataGridView1.CurrentRow.Cells["QQ号码"].Value.ToString();
                personnelChange.Email = dataGridView1.CurrentRow.Cells["电子邮箱"].Value.ToString();
                personnelChange.Hobby = dataGridView1.CurrentRow.Cells["爱好"].Value.ToString();
                personnelChange.JobNature = dataGridView1.CurrentRow.Cells["工作性质"].Value.ToString();
                personnelChange.PersonnelStatus = dataGridView1.CurrentRow.Cells["人员状态"].Value.ToString();
                personnelChange.IsCore = Convert.ToBoolean(dataGridView1.CurrentRow.Cells["是否核心员工"].Value.ToString());

                if (dataGridView1.CurrentRow.Cells["离职时间"].Value.ToString() != "")
                {
                    personnelChange.DimissionDate = Convert.ToDateTime(dataGridView1.CurrentRow.Cells["离职时间"].Value);
                }
                else
                {
                    personnelChange.DimissionDate = ServerTime.Time;
                }

                if (dataGridView1.CurrentRow.Cells["个人档案所在地"].Value.ToString() != "")
                {
                    personnelChange.ArchivePosition = dataGridView1.CurrentRow.Cells["个人档案所在地"].Value.ToString();
                }
                else
                {
                    personnelChange.ArchivePosition = "";
                }

                if (dataGridView1.CurrentRow.Cells["转正日期"].Value.ToString() != "")
                {
                    personnelChange.BecomeRegularEmployeeDate = Convert.ToDateTime(dataGridView1.CurrentRow.Cells["转正日期"].Value);
                }

                if (dataGridView1.CurrentRow.Cells["毕业年份"].Value.ToString() != "")
                {
                    personnelChange.GraduationYear = Convert.ToInt32(dataGridView1.CurrentRow.Cells["毕业年份"].Value.ToString());
                }

                personnelChange.LengthOfSchooling = dataGridView1.CurrentRow.Cells["学制"].Value.ToString();
                personnelChange.MaritalStatus = dataGridView1.CurrentRow.Cells["婚姻状况"].Value.ToString();

                if (dataGridView1.CurrentRow.Cells["参加工作的时间"].Value.ToString() != "")
                {
                    personnelChange.TakeJobDate = Convert.ToDateTime(dataGridView1.CurrentRow.Cells["参加工作的时间"].Value);
                }

                //if (dataGridView1.CurrentRow.Cells["照片"].Value.ToString() != "")
                //{
                //    personnelChange.Photo = dataGridView1.CurrentRow.Cells["照片"].Value as byte[];
                //}

                //if (dataGridView1.CurrentRow.Cells["附件"].Value.ToString() != "")
                //{
                //    personnelChange.Annex = dataGridView1.CurrentRow.Cells["附件"].Value as byte[];
                //    personnelChange.AnnexName = dataGridView1.CurrentRow.Cells["附件名"].Value.ToString();
                //}

                if (dataGridView1.CurrentRow.Cells["关联编号"].Value.ToString() != "")
                {
                    personnelChange.ResumeID = Convert.ToInt32(dataGridView1.CurrentRow.Cells["关联编号"].Value);
                }

                personnelChange.Remark = dataGridView1.CurrentRow.Cells["备注"].Value.ToString();
                personnelChange.ChangerCode = BasicInfo.LoginID;
                personnelChange.ChangeTime = ServerTime.Time;
            }
        }

        /// <summary>
        /// 获得人员档案数据集
        /// </summary>
        /// <returns>返回人员档案集合</returns>
        HR_PersonnelArchive GetPersonnelArchiveData()
        {
            HR_PersonnelArchive personnel = m_personnerServer.GetPersonnelInfo(dataGridView1.CurrentRow.Cells["员工编号"].Value.ToString());

            if (dataGridView1.CurrentRow.Cells["离职时间"].Value.ToString() != "")
            {
                if (Convert.ToDateTime(dataGridView1.CurrentRow.Cells["离职时间"].Value).Year > 2000)
                {
                    allowDate = Convert.ToDateTime(dataGridView1.CurrentRow.Cells["离职时间"].Value).ToString();
                }
            }
            else
            {
                allowDate = "";
            }

            //if (dataGridView1.CurrentRow.Cells["合同起始时间"].Value.ToString() != "")
            //{
            //    if (Convert.ToDateTime(dataGridView1.CurrentRow.Cells["合同起始时间"].Value).Year > 2000)
            //    {
            //        starTime = Convert.ToDateTime(dataGridView1.CurrentRow.Cells["合同起始时间"].Value).ToString();
            //    }
            //}
            //else
            //{
            //starTime = "";
            //}

            if (dataGridView1.CurrentRow.Cells["合同到期日"].Value.ToString() != "")
            {
                if (Convert.ToDateTime(dataGridView1.CurrentRow.Cells["合同到期日"].Value).Year > 2000)
                {
                    endTime = Convert.ToDateTime(dataGridView1.CurrentRow.Cells["合同到期日"].Value).ToString();
                }
            }
            else
            {
                endTime = "";
            }

            return personnel;
        }

        private void 新建toolStripButton1_Click(object sender, EventArgs e)
        {
            //FormPersonnelArchiveList frm = new FormPersonnelArchiveList(m_authorityFlag,null,null,
            //    "","","",null);
            //frm.ShowDialog();

            //if (frm.updateFlag)
            //{
            //    RefreshControl();
            //}

            //FormPersonnelArchiveListShow frm = new FormPersonnelArchiveListShow(m_authorityFlag, null, null,
            //    "", "", "", null);
            //frm.ShowDialog();

            //if (frm.UpdateFlag)
            //{
            //    RefreshControl();
            //}

            员工档案明细 frm = new 员工档案明细(m_authorityFlag, null, null, null);
            frm.ShowDialog();

            if (frm.UpdateFlag)
            {
                RefreshControl();
            }
        }

        private void 导入toolStripButton_Click(object sender, EventArgs e)
        {
            DataTable dtTemp = ExcelHelperP.RenderFromExcel(openFileDialog1);

            if (dtTemp == null)
            {
                //MessageDialog.ShowPromptMessage(error);
                return;
            }

            if (CheckTable(dtTemp))
            {
                if (!m_personnerServer.InsertPersonnelArchiveInfo(dtTemp, out error))
                {
                    MessageDialog.ShowPromptMessage(error);
                    return;
                }
                else
                {
                    MessageDialog.ShowPromptMessage("导入成功!");
                }
            }

            RefreshControl();
        }

        /// <summary>
        /// 检查Excel表的数据
        /// </summary>
        /// <param name="dtcheck">表</param>
        /// <returns>返回是否正确</returns>
        bool CheckTable(DataTable dtcheck)
        {
            if (!dtcheck.Columns.Contains("员工编号"))
            {
                MessageDialog.ShowPromptMessage("此文件中不包含【员工编号】信息");
                return false;
            }

            if (!dtcheck.Columns.Contains("员工姓名"))
            {
                MessageDialog.ShowPromptMessage("此文件中不包含【员工姓名】信息");
                return false;
            }

            if (!dtcheck.Columns.Contains("班组"))
            {
                MessageDialog.ShowPromptMessage("此文件中不包含【班组】信息");
                return false;
            }

            if (!dtcheck.Columns.Contains("职位"))
            {
                MessageDialog.ShowPromptMessage("此文件中不包含【职位】信息");
                return false;
            }

            if (!dtcheck.Columns.Contains("外部职称"))
            {
                MessageDialog.ShowPromptMessage("此文件中不包含【外部职称】信息");
                return false;
            }

            if (!dtcheck.Columns.Contains("内部级别"))
            {
                MessageDialog.ShowPromptMessage("此文件中不包含【内部级别】信息");
                return false;
            }

            if (!dtcheck.Columns.Contains("核心员工"))
            {
                MessageDialog.ShowPromptMessage("此文件中不包含【核心员工】信息");
                return false;
            }

            if (!dtcheck.Columns.Contains("出生日期"))
            {
                MessageDialog.ShowPromptMessage("此文件中不包含【出生日期】信息");
                return false;
            }

            if (!dtcheck.Columns.Contains("国籍"))
            {
                MessageDialog.ShowPromptMessage("此文件中不包含【国籍】信息");
                return false;
            }

            if (!dtcheck.Columns.Contains("民族"))
            {
                MessageDialog.ShowPromptMessage("此文件中不包含【民族】信息");
                return false;
            }

            if (!dtcheck.Columns.Contains("籍贯"))
            {
                MessageDialog.ShowPromptMessage("此文件中不包含【籍贯】信息");
                return false;
            }

            if (!dtcheck.Columns.Contains("政治面貌"))
            {
                MessageDialog.ShowPromptMessage("此文件中不包含【政治面貌】信息");
                return false;
            }

            if (!dtcheck.Columns.Contains("身份证号码"))
            {
                MessageDialog.ShowPromptMessage("此文件中不包含【身份证号码】信息");
                return false;
            }
            if (!dtcheck.Columns.Contains("院校名称"))
            {
                MessageDialog.ShowPromptMessage("此文件中不包含【院校名称】信息");
                return false;
            }

            if (!dtcheck.Columns.Contains("文化程度"))
            {
                MessageDialog.ShowPromptMessage("此文件中不包含【文化程度】信息");
                return false;
            }

            if (!dtcheck.Columns.Contains("所学专业"))
            {
                MessageDialog.ShowPromptMessage("此文件中不包含【所学专业】信息");
                return false;
            }

            if (!dtcheck.Columns.Contains("家庭地址"))
            {
                MessageDialog.ShowPromptMessage("此文件中不包含【家庭地址】信息");
                return false;
            }

            if (!dtcheck.Columns.Contains("邮编"))
            {
                MessageDialog.ShowPromptMessage("此文件中不包含【邮编】信息");
                return false;
            }

            if (!dtcheck.Columns.Contains("电话"))
            {
                MessageDialog.ShowPromptMessage("此文件中不包含【电话】信息");
                return false;
            }

            if (!dtcheck.Columns.Contains("手机"))
            {
                MessageDialog.ShowPromptMessage("此文件中不包含【手机】信息");
                return false;
            }

            if (!dtcheck.Columns.Contains("开户银行"))
            {
                MessageDialog.ShowPromptMessage("此文件中不包含【开户银行】信息");
                return false;
            }

            if (!dtcheck.Columns.Contains("银行账号"))
            {
                MessageDialog.ShowPromptMessage("此文件中不包含【银行账号】信息");
                return false;
            }

            if (!dtcheck.Columns.Contains("社会保障号"))
            {
                MessageDialog.ShowPromptMessage("此文件中不包含【社会保障号】信息");
                return false;
            }

            if (!dtcheck.Columns.Contains("QQ号码"))
            {
                MessageDialog.ShowPromptMessage("此文件中不包含【QQ号码】信息");
                return false;
            }

            if (!dtcheck.Columns.Contains("电子邮箱"))
            {
                MessageDialog.ShowPromptMessage("此文件中不包含【电子邮箱】信息");
                return false;
            }

            if (!dtcheck.Columns.Contains("爱好"))
            {
                MessageDialog.ShowPromptMessage("此文件中不包含【爱好】信息");
                return false;
            }

            if (!dtcheck.Columns.Contains("特长"))
            {
                MessageDialog.ShowPromptMessage("此文件中不包含【特长】信息");
                return false;
            }

            if (!dtcheck.Columns.Contains("性别"))
            {
                MessageDialog.ShowPromptMessage("此文件中不包含【性别】信息");
                return false;
            }

            if (!dtcheck.Columns.Contains("调动次数"))
            {
                MessageDialog.ShowPromptMessage("此文件中不包含【调动次数】信息");
                return false;
            }

            if (!dtcheck.Columns.Contains("培训次数"))
            {
                MessageDialog.ShowPromptMessage("此文件中不包含【培训次数】信息");
                return false;
            }

            if (!dtcheck.Columns.Contains("备注"))
            {
                MessageDialog.ShowPromptMessage("此文件中不包含【备注】信息");
                return false;
            }

            if (!dtcheck.Columns.Contains("变更次数"))
            {
                MessageDialog.ShowPromptMessage("此文件中不包含【变更次数】信息");
                return false;
            }

            if (!dtcheck.Columns.Contains("人员状态"))
            {
                MessageDialog.ShowPromptMessage("此文件中不包含【人员状态】信息");
                return false;
            }

            if (!dtcheck.Columns.Contains("婚姻状况"))
            {
                MessageDialog.ShowPromptMessage("此文件中不包含【婚姻状况】信息");
                return false;
            }

            return true;
        }

        private void 导出toolStripButton_Click(object sender, EventArgs e)
        {
            ExcelHelperP.DatagridviewToExcel(saveFileDialog1, dataGridView1);
        }

        private void 综合查询toolStripButton3_Click(object sender, EventArgs e)
        {
            IAuthorization authorization = PlatformFactory.GetObject<IAuthorization>();
            string businessID = "人员档案管理";
            IQueryResult qr = authorization.Query(businessID, null, null, 0);
            List<string> lstFindField = new List<string>();
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

        private void dataGridView1_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            ColumnWidthControl.SaveColumnParams(labelTitle.Text, e.Column);
        }

        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
           
        }

        private void 刷新toolStripButton1_Click(object sender, EventArgs e)
        {
            RefreshControl();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            string status = "";

            if (cbStatusIn.Checked && cbStatusOut.Checked)
            {
                status = "全部";

                RefreshControl();
                return;
            }

            if (cbStatusIn.Checked && !cbStatusOut.Checked)
            {
                status = "在职";
            }
            else if (cbStatusOut.Checked && !cbStatusIn.Checked)
            {
                status = "离职";                
            }

            RefreshControl();
            DataTable dt = (DataTable)dataGridView1.DataSource;
            DataTable dtTemp = dt.Clone();

            DataRow[] row = dt.Select("人员状态='" + status + "'");

            foreach (var item in row)
            {
                dtTemp.ImportRow(item);
            }

            dataGridView1.DataSource = dtTemp;                   
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            BindControl();

            string workID = dataGridView1.CurrentRow.Cells["员工编号"].Value.ToString();

            //FormPersonnelArchiveList frm = new FormPersonnelArchiveList(
            //    m_authorityFlag, personnelChange, GetPersonnelArchiveData(), allowDate, starTime, endTime, m_queryResult);
            //frm.ShowDialog();

            //if (frm.updateFlag)
            //{
            //    RefreshControl();
            //}

            //FormPersonnelArchiveListShow frm = new FormPersonnelArchiveListShow(
            //    m_authorityFlag, personnelChange, GetPersonnelArchiveData(), allowDate, starTime, endTime, m_queryResult);
            //frm.ShowDialog();

            员工档案明细 frm = new 员工档案明细(m_authorityFlag, personnelChange, GetPersonnelArchiveData(), m_queryResult);

            frm.ShowDialog();

            if (frm.UpdateFlag)
            {
                RefreshControl();
            }

            PositioningRecord(workID);
        }

        private void toolStripButton批量修改部门_Click(object sender, EventArgs e)
        {
            批量修改部门 frm = new 批量修改部门();

            frm.ShowDialog();

            RefreshControl();
        }

        private void 导出平均年龄ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = new DataTable();

                if (!m_personnerServer.ExcelPersonAge(out dt, out error))
                {
                    MessageDialog.ShowPromptMessage(error);
                    return;
                }
                else
                {
                    string[] columns = { };
                    ExcelHelperP.DataTableToExcel(saveFileDialog1, dt, columns);
                }
            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
            }
        }

        private void 导出平均学历ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = new DataTable();

                if (!m_personnerServer.ExcelEducation(out dt, out error))
                {
                    MessageDialog.ShowPromptMessage(error);
                    return;
                }
                else
                {
                    string[] columns = { };
                    ExcelHelperP.DataTableToExcel(saveFileDialog1, dt, columns);
                }
            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
            }
        }

        private void 导出平均入司年份ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = new DataTable();

                if (!m_personnerServer.ExcelIncompanyYears(out dt, out error))
                {
                    MessageDialog.ShowPromptMessage(error);
                    return;
                }
                else
                {
                    string[] columns = { };
                    ExcelHelperP.DataTableToExcel(saveFileDialog1, dt, columns);
                }
            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
            }
        }

        private void 导出当月离职人员分析ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            分析离职人员 frm = new 分析离职人员();

            frm.ShowDialog();
        }

        private void 导出各部门在职人数ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = new DataTable();

                if (!m_personnerServer.ExcelOnjob(out dt, out error))
                {
                    MessageDialog.ShowPromptMessage(error);
                    return;
                }
                else
                {
                    string[] columns = { };
                    ExcelHelperP.DataTableToExcel(saveFileDialog1, dt, columns);
                }
            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
            }
        }

        private void 导出人员变化情况ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}

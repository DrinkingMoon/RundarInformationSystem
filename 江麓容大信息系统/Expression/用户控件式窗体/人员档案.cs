using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ServerModule;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using PlatformManagement;
using GlobalObject;
using UniversalControlLibrary;

namespace Expression
{
    /// <summary>
    /// 人员档案界面
    /// </summary>
    public partial class UserControlPersonnel : Form
    {
        #region variants
        /// <summary>
        /// 错误信息
        /// </summary>
        string m_err;

        /// <summary>
        /// 查找到的符合条件的人员信息
        /// </summary>
        IQueryable<HR_PositionType> m_findPositionType;

        /// <summary>
        /// 查找到的符合条件的部门信息
        /// </summary>
        IQueryable<View_Department> m_findDepartment;

        /// <summary>
        /// 查找到的符合条件的部门信息
        /// </summary>
        IQueryable<View_HR_Personnel> m_findPersonnel;

        /// <summary>
        /// 部门服务组件
        /// </summary>
        IDepartmentServer m_departmentServer = ServerModuleFactory.GetServerModule<IDepartmentServer>();

        /// <summary>
        /// 人员服务组件
        /// </summary>
        IPersonnelInfoServer m_personnelInfo = ServerModuleFactory.GetServerModule<IPersonnelInfoServer>();

        /// <summary>
        /// 计划成本服务组件
        /// </summary>
        IBasicGoodsServer m_planCostServer = ServerModuleFactory.GetServerModule<IBasicGoodsServer>();

        /// <summary>
        /// 选中的树节点
        /// </summary>
        TreeNode SelectedNode
        {
            get { return treeView1.SelectedNode; }
        }

        /// <summary>
        /// 权限标志
        /// </summary>
        AuthorityFlag m_authorityFlag;

        /// <summary>
        /// 数据定位控件
        /// </summary>
        UserControlDataLocalizer m_dataLocalizer;

        #endregion

        public UserControlPersonnel(FunctionTreeNodeInfo nodeInfo)
        {
            InitializeComponent();

            m_authorityFlag = nodeInfo.Authority;

            this.dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            this.dataGridView1.ColumnWidthChanged += new DataGridViewColumnEventHandler(this.dataGridView1_ColumnWidthChanged);
        }

        /// <summary>
        /// 窗体载入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControlPersonnel_Load(object sender, EventArgs e)
        {
            if (!m_departmentServer.GetAllDepartment(out m_findDepartment, out m_err))
            {
                MessageDialog.ShowErrorMessage(m_err);
                return;
            }

            m_findPositionType = m_personnelInfo.GetPositionType();

            if (m_findPositionType  == null)
            {
                MessageDialog.ShowErrorMessage(m_err);
                return;
            }

            cmbDepot.DisplayMember = "部门名称";
            cmbDepot.ValueMember = "部门代码";
            cmbWorkPost.DisplayMember = "PositionName";
            cmbWorkPost.ValueMember = "ID";
            cmbDepot.DataSource = m_findDepartment;
            cmbWorkPost.DataSource = m_findPositionType;
            cmbDepot.Text = "";
            cmbDepot.Tag = -1;
            cmbWorkPost.Text = "";
            cmbWorkPost.Tag = -1;
            RefreshTreeView(m_findDepartment);

            treeView1.ExpandAll();
            RefreshControl();

            cmbPositionStatus.SelectedIndex = 0;
            cmbUseStatus.SelectedIndex = 0;
            cmbUserStatus.SelectedIndex = 0;

            FaceAuthoritySetting.SetVisibly(toolStrip1, m_authorityFlag);
        }

        /// <summary>
        /// 生成树节点
        /// </summary>
        /// <param name="dept">部门信息</param>
        /// <returns>返回生成的树节点</returns>
        TreeNode GenerateTreeNode(View_Department dept)
        {
            TreeNode node = new TreeNode();

            node.Name = dept.部门代码;
            node.Text = string.Format("({0}) {1}", dept.部门代码, dept.部门名称);
            node.Tag = dept;

            return node;
        }

        /// <summary>
        /// 刷新树视图
        /// </summary>
        /// <param name="findDepartmentBill">部门信息</param>
        void RefreshTreeView(IQueryable<View_Department> findDepartmentBill)
        {
            if (findDepartmentBill.Count() == 0)
            {
                return;
            }

            treeView1.Nodes[0].Nodes.Clear();

            List<View_Department> deptInfo = findDepartmentBill.ToList();

            for (int i = 0; deptInfo.Count > 0; )
            {
                View_Department curDept = deptInfo[i];
                TreeNode node = GenerateTreeNode(curDept);

                this.treeView1.Nodes[0].Nodes.Add(node);
                deptInfo.RemoveAt(i);

                if (deptInfo.Count > 0 && deptInfo[i].部门代码.Length > curDept.部门代码.Length)
                {
                    RecursionGenerateTree(node, deptInfo);
                }
            }
        }

        /// <summary>
        /// 递归生成树
        /// </summary>
        /// <param name="parent">父节点</param>
        /// <param name="deptInfo">部门信息</param>
        void RecursionGenerateTree(TreeNode parent, List<View_Department> deptInfo)
        {
            for (; deptInfo.Count > 0; )
            {
                View_Department curDept = deptInfo[0];

                if ((parent.Tag as View_Department).部门代码.Length >= curDept.部门代码.Length)
                {
                    break;
                }

                TreeNode node = GenerateTreeNode(curDept);

                parent.Nodes.Add(node);
                deptInfo.RemoveAt(0);

                if (deptInfo.Count > 0 && deptInfo[0].部门代码.Length > curDept.部门代码.Length)
                {
                    RecursionGenerateTree(node, deptInfo);
                }
            }
        }

        /// <summary>
        /// 改变组件大小
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControlPersonnel_Resize(object sender, EventArgs e)
        {
            labelTitle.Location = new Point((this.Width - labelTitle.Width) / 2, labelTitle.Location.Y);
        }

        /// <summary>
        /// 清空控件值
        /// </summary>
        void ClearControl()
        {
            txtPersonnelCode.Text = "";
            txtPersonnelName.Text = "";
            cmbDepot.Text = "";
            cmbDepot.Tag = -1;
            txtPhone.Text = "";
            txtEmail.Text = "";
            txtRemark.Text = "";
            cmbWorkPost.Text = "";
            cmbWorkPost.Tag = -1;
           

            ckbDelete.Checked = false;
            ckbUser.Checked = false;
            ckbOnTheJob.Checked = false;
        }

        /// <summary>
        /// 刷新控件
        /// </summary>
        void RefreshControl()
        {
            ClearControl();

            if (SelectedNode == null )
            {
                return;
            }

            if (SelectedNode.Text == "部门分类")
            {
                m_findPersonnel = m_personnelInfo.GetAllInfo();
            }
            else
            {
                View_Department dept = SelectedNode.Tag as View_Department;

                m_findPersonnel = m_personnelInfo.GetAllInfo(dept.部门代码);
                cmbDepot.Text = dept.部门名称;
                cmbDepot.Tag = dept.部门代码;
            }

            string strSelect = "";

            if (cmbPositionStatus.Text != "全部")
            {
                if (cmbPositionStatus.Text == "在职")
                {
                    strSelect += " and 是否在职 = 1 ";
                }
                else
                {
                    strSelect += " and 是否在职 = 0 ";
                }
            }

            if (cmbUseStatus.Text != "全  部")
            {
                if (cmbUseStatus.Text == "已停用")
                {
                    strSelect += " and DeleteFlag = 1";
                }
                else
                {
                    strSelect += " and DeleteFlag = 0";
                }
            }

            if (cmbUserStatus.Text != "全    部")
            {
                if (cmbUserStatus.Text == "操作用户")
                {
                    strSelect += " and 是否操作用户 = 1";
                }
                else
                {
                    strSelect += " and 是否操作用户 = 0";
                }
            }

            DataTable dt = GlobalObject.GeneralFunction.ConvertToDataTable<View_HR_Personnel>(m_findPersonnel);

            DataTable dtHR = new DataTable();

            if (strSelect.Trim().Length > 0 && dt.Rows.Count > 0)
            {
                dtHR = dt.Clone();

                strSelect = strSelect.Substring(5, strSelect.Length - 5);

                DataRow[] dr = dt.Select(strSelect);

                for (int i = 0; i < dr.Length; i++)
                {
                    dtHR.ImportRow(dr[i]);
                }
            }
            else
            {
                dtHR = dt;
            }

            dataGridView1.DataSource = dtHR;

            this.dataGridView1.ColumnWidthChanged -= new System.Windows.Forms.DataGridViewColumnEventHandler(
                this.dataGridView1_ColumnWidthChanged);

            ColumnWidthControl.SetDataGridView(labelTitle.Text, dataGridView1);

            this.dataGridView1.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(
                this.dataGridView1_ColumnWidthChanged);

            if (m_dataLocalizer == null)
            {
                m_dataLocalizer = new UserControlDataLocalizer(dataGridView1, this.Name, 
                    UniversalFunction.SelectHideFields(this.Name, dataGridView1.Name, BasicInfo.LoginID));

                panelTop.Controls.Add(m_dataLocalizer);
                m_dataLocalizer.Dock = DockStyle.Bottom;
            }

            if (dataGridView1.Rows.Count == 0)
                panelTop.Visible = false;
            else
                panelTop.Visible = true;


            dataGridView1.Refresh();
        }

        /// <summary>
        /// 树的点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            RefreshControl();
        }

        /// <summary>
        /// DataGridView焦点动作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0 )
            {
                return;
            }

            txtPersonnelCode.Text = dataGridView1.Rows[e.RowIndex].Cells["工号"].Value.ToString();
            txtPersonnelName.Text = dataGridView1.Rows[e.RowIndex].Cells["姓名"].Value.ToString();
            cmbWorkPost.Text = dataGridView1.Rows[e.RowIndex].Cells["职位"].Value.ToString();
            cmbWorkPost.Tag = dataGridView1.Rows[e.RowIndex].Cells["职位编码"].Value.ToString();
            txtPhone.Text = dataGridView1.Rows[e.RowIndex].Cells["手机号"].Value == null ? 
                "" : dataGridView1.Rows[e.RowIndex].Cells["手机号"].Value.ToString();
            txtRemark.Text = dataGridView1.Rows[e.RowIndex].Cells["备注"].Value == null ? 
                "" : dataGridView1.Rows[e.RowIndex].Cells["备注"].Value.ToString();
            txtEmail.Text = dataGridView1.Rows[e.RowIndex].Cells["电子邮件"].Value == null ? 
                "" : dataGridView1.Rows[e.RowIndex].Cells["电子邮件"].Value.ToString();
            cmbDepot.Text = dataGridView1.Rows[e.RowIndex].Cells["部门名称"].Value.ToString();
            cmbDepot.Tag = m_departmentServer.GetDepartmentCode(dataGridView1.Rows[e.RowIndex].Cells["部门名称"].Value.ToString());

            if (Convert.ToBoolean(dataGridView1.Rows[e.RowIndex].Cells["是否在职"].Value))
            {
                ckbOnTheJob.Checked = true;
            }
            else
            {
                ckbOnTheJob.Checked = false;
            }
            
            if (Convert.ToBoolean(dataGridView1.Rows[e.RowIndex].Cells["是否操作用户"].Value))
            {
                ckbUser.Checked = true;
            }
            else
            {
                ckbUser.Checked = false;
            }
           
            if (Convert.ToBoolean(dataGridView1.Rows[e.RowIndex].Cells["DeleteFlag"].Value))
            {
                ckbDelete.Checked = true;
            }
            else
            {
                ckbDelete.Checked = false;
            }
            
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!CheckDataItem())
            {
                return;
            }

            HR_Personnel personnel = CreatePersonnelFromForm();

            if (personnel.WorkID != txtPersonnelCode.Text)
            {
                MessageDialog.ShowErrorMessage("不可修改人员编码");
                return;
            }

            if (!m_personnelInfo.UpdatePersonnel(personnel, out m_err))
            {
                MessageDialog.ShowErrorMessage(m_err);
                return;
            }
            else
            {
                MessageBox.Show("修改成功！","提示");
                treeView1_AfterSelect(sender, null);
            }
        }

        /// <summary>
        /// 根据界面数据生成部门信息对象
        /// </summary>
        /// <returns>返回生成的部门信息对象</returns>
        private HR_Personnel CreatePersonnelFromForm()
        {
            HR_Personnel personnel = new HR_Personnel();

            personnel.OnTheJob = ckbOnTheJob.Checked ;
            personnel.Name = txtPersonnelName.Text.Trim();
            personnel.IsOperationalUser = ckbUser.Checked ;
            personnel.WorkID = txtPersonnelCode.Text;
            personnel.Dept = cmbDepot.SelectedValue.ToString();
            personnel.WorkPost = (int)cmbWorkPost.SelectedValue;
            personnel.DeleteFlag = ckbDelete.Checked;
            personnel.PY = UniversalFunction.GetPYWBCode(txtPersonnelName.Text.Trim(), "PY");
            personnel.WB = UniversalFunction.GetPYWBCode(txtPersonnelName.Text.Trim(), "WB");

            return personnel;
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!CheckDataItem())
            {
                return;
            }

            HR_Personnel personnel = CreatePersonnelFromForm();

            if (!m_personnelInfo.AddPersonnel(personnel, out m_err))
            {
                MessageDialog.ShowErrorMessage(m_err);
                return;
            }
            else 
            {
                MessageBox.Show("添加成功！","提示");
            }

            treeView1_AfterSelect(sender, null);
        }

        /// <summary>
        /// 检查数据
        /// </summary>
        /// <returns>通过返回True，否则返回False</returns>
        private bool CheckDataItem()
        {
            txtPersonnelCode.Text = txtPersonnelCode.Text.Trim();

            if (txtPersonnelCode.Text == "")
            {
                MessageDialog.ShowPromptMessage("人员工号不能为空!");
                return false;
            }

            bool error = false;

            //if (txtPersonnelCode.Text.Length != 4)
            //{
            //    error = true;
            //}

            try
            {
                int code = Convert.ToInt32(txtPersonnelCode.Text);
            }
            catch (Exception exce)
            {
                error = true;
                Console.WriteLine(exce.Message);
            }

            if (error)
            {
                MessageDialog.ShowPromptMessage("您录入的人员工号不正确，只能为4位数字，不满4位前面应补0");
                return false;
            }

            if (txtPersonnelName.Text == "")
            {
                MessageDialog.ShowPromptMessage("人员名称不能为空!");
                return false;
            }

            bool isExist = (from r in m_findDepartment 
                            where r.部门名称 == cmbDepot.Text 
                            select r).Count() == 1;

            if (!isExist)
            {
                cmbDepot.Focus();
                MessageDialog.ShowPromptMessage("请选择可用的部门!");
                return false;
            }

            isExist = (from r in m_findPositionType 
                       where r.PositionName == cmbWorkPost.Text 
                       select r).Count() == 1;

            if (!isExist)
            {
                cmbWorkPost.Focus();
                MessageDialog.ShowPromptMessage("请选择可用的员工职位!");
                return false;
            }

            return true;
        }

        private void chkShowDisablePersonel_CheckedChanged(object sender, EventArgs e)
        {
            RefreshControl();
        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                if (!Convert.ToBoolean(dataGridView1.Rows[i].Cells["是否在职"].Value))
                {
                    dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                }

            }
        }

        private void dataGridView1_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            ColumnWidthControl.SaveColumnParams(this.labelTitle.Text, e.Column);
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

        /// <summary>
        /// 新增修改职位信息
        /// </summary>
        private void btnPosition_Click(object sender, EventArgs e)
        {
                职位信息 frm = new 职位信息();
                frm.ShowDialog();

                m_findPositionType = m_personnelInfo.GetPositionType();

                if (m_findPositionType == null)
                {
                    MessageDialog.ShowErrorMessage(m_err);
                    return;
                }
                cmbWorkPost.DisplayMember = "PositionName";
                cmbWorkPost.ValueMember = "ID";
                cmbWorkPost.DataSource = m_findPositionType;
                cmbWorkPost.Text = "";
                cmbWorkPost.Tag = -1;

                FunctionTreeNodeInfo nodeInfo = new FunctionTreeNodeInfo();
                m_authorityFlag = nodeInfo.Authority;
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            RefreshControl();
        }

        private void ckbOnTheJob_CheckedChanged(object sender, EventArgs e)
        {
            if (!ckbOnTheJob.Checked)
            {
                ckbUser.Checked = false;
                ckbDelete.Checked = true;
            }
        }

        private void ckbUser_CheckedChanged(object sender, EventArgs e)
        {
            if (ckbUser.Checked)
            {
                ckbOnTheJob.Checked = true;
            }
        }
    }
}

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
using Service_Peripheral_HR;
using Expression;
using GlobalObject;
using UniversalControlLibrary;

namespace Form_Peripheral_HR
{
    /// <summary>
    /// 岗位编制主界面
    /// </summary>
    public partial class UserControlNumOfPeople : Form
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        string m_error;

        /// <summary>
        /// 查找到的符合条件的部门信息
        /// </summary>
        IQueryable<View_HR_Dept> m_findDepartment;

        /// <summary>
        /// 部门信息服务组件
        /// </summary>
        IOrganizationServer m_departmentServer = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<IOrganizationServer>();

        /// <summary>
        /// 岗位管理服务类
        /// </summary>
        IOperatingPostServer m_postServer = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<IOperatingPostServer>();

        /// <summary>
        /// 人员档案管理类
        /// </summary>
        IPersonnelArchiveServer m_personnerServer = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<IPersonnelArchiveServer>();

        /// <summary>
        /// 权限标志
        /// </summary>
        AuthorityFlag m_authFlag;

        /// <summary>
        /// 选中的树节点
        /// </summary>
        TreeNode SelectedNode
        {
            get { return treeView1.SelectedNode; }
        }

        /// <summary>
        /// 上一次的选中节点
        /// </summary>
        TreeNode m_preSelectedNode;

        public UserControlNumOfPeople(PlatformManagement.FunctionTreeNodeInfo nodeInfo)
        {
            InitializeComponent();

            m_authFlag = nodeInfo.Authority;

            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
            UpdateStyles();
        }

        /// <summary>
        /// 生成树节点
        /// </summary>
        /// <param name="dept">部门信息</param>
        /// <returns>返回生成的树节点</returns>
        TreeNode GenerateTreeNode(View_HR_Dept dept)
        {
            TreeNode node = new TreeNode();

            node.Name = dept.部门代码;
            node.Text = string.Format("({0}) {1}", dept.部门代码, dept.部门名称);
            node.Tag = dept;

            return node;
        }

        /// <summary>
        /// 生成树节点
        /// </summary>
        /// <param name="dept">部门信息</param>
        /// <returns>返回生成的树节点</returns>
        TreeNode GenerateTreeNode(HR_Dept dept)
        {
            TreeNode node = new TreeNode();

            node.Name = dept.DeptCode;
            node.Text = string.Format("({0}) {1}", dept.DeptCode, dept.DeptName);
            node.Tag = dept;

            return node;
        }

        /// <summary>
        /// 刷新树视图
        /// </summary>
        /// <param name="findStore"></param>
        void RefreshTreeView(IQueryable<View_HR_Dept> findDepartmentBill)
        {
            if (findDepartmentBill.Count() == 0)
            {
                return;
            }

            List<View_HR_Dept> deptInfo = findDepartmentBill.ToList();

            for (int i = 0; deptInfo.Count > 0; )
            {
                View_HR_Dept curDept = deptInfo[i];
                TreeNode node = GenerateTreeNode(curDept);

                this.treeView1.Nodes[0].Nodes.Add(node);
                deptInfo.RemoveAt(i);

                if (deptInfo.Count > 0 && deptInfo[i].父级编码 != "")
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
        void RecursionGenerateTree(TreeNode parent, List<View_HR_Dept> deptInfo)
        {
            for (; deptInfo.Count > 0; )
            {
                View_HR_Dept curDept = deptInfo[0];

                if ((parent.Tag as View_HR_Dept).部门代码 != curDept.父级编码)
                {
                    break;
                }

                TreeNode node = GenerateTreeNode(curDept);
                parent.Nodes.Add(node);
                deptInfo.RemoveAt(0);

                if (deptInfo.Count > 0 && deptInfo[0].父级编码 == curDept.部门代码)
                {
                    RecursionGenerateTree(node, deptInfo);
                }
            }
        }

        /// <summary>
        /// 刷新
        /// </summary>
        void RefreshControl()
        {
            DataTable deptPostDt = m_postServer.GetDeptPost(null);

            dgvPostNum.DataSource = deptPostDt;
            dgvPostNum.Columns["序号"].Visible = false;
            dgvPostNum.Columns["部门名称"].Visible = false;
        }

        private void dgvPostNum_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            for (int i = 0; i < dgvPostNum.RowCount; i++)
            {
                if (Convert.ToInt32(dgvPostNum.Rows[i].Cells["编制人数"].Value) > Convert.ToInt32(
                    dgvPostNum.Rows[i].Cells["在岗人数"].Value))
                {
                    dgvPostNum.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                }
            }
        }

        private void UserControlNumOfPeople_Load(object sender, EventArgs e)
        {
            FaceAuthoritySetting.SetVisibly(toolStrip1, m_authFlag);
            FaceAuthoritySetting.SetEnable(this.Controls, m_authFlag);

            DataTable postDt = m_postServer.GetOperatingPost(null);

            for (int i = 0; i < postDt.Rows.Count; i++)
            {
                cmbWorkPost.Items.Add(postDt.Rows[i]["岗位名称"].ToString());
            }

            if (!m_departmentServer.GetAllDeptInfo(out m_findDepartment, out m_error))
            {
                MessageDialog.ShowErrorMessage(m_error);
                return;
            }

            RefreshTreeView(m_findDepartment);
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            treeView1.ExpandAll();

            RefreshControl();
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
             Color c = SelectedNode.BackColor;

            if (SelectedNode == null || SelectedNode.Tag == null || SelectedNode.Tag.ToString() == "system")
            {
                return;
            }

            string[] s = SelectedNode.Text.Split(' ');
            IQueryable<View_HR_DeptPost> postNum = m_postServer.GetDeptPostByDeptCode(s[1].ToString());

            DataTable dt = GlobalObject.GeneralFunction.ConvertToDataTable<View_HR_DeptPost>(postNum);
            dgvPostNum.DataSource = dt;

            if (dt.Rows.Count > 0)
            {
                dgvPostNum.Columns["序号"].Visible = false;
                dgvPostNum.Columns["部门名称"].Visible = false;
            }

            DataTable countDt = m_postServer.GetDeptCount(m_departmentServer.GetDeptCode(SelectedNode.Text.Split(' ')[1].ToString()));

            txtExistCount.Text = countDt.Rows[0]["ExistAmount"].ToString();
            txtCount.Text = countDt.Rows[0]["NumberOfPeople"].ToString();

            dgvPostNum.Refresh();

            if (m_preSelectedNode != null)
            {
                m_preSelectedNode.BackColor = treeView1.BackColor;
            }

            m_preSelectedNode = e.Node;
            e.Node.BackColor = Color.Yellow;

            DataTable personnerDt = m_personnerServer.GetAllInfoByDept(m_departmentServer.GetDeptCode(SelectedNode.Text.Split(' ')[1].ToString()));

            dataGridView1.DataSource = personnerDt;
        }

        /// <summary>
        /// 更改服务组件大小
        /// </summary>
        private void UserControlNumOfPeople_Resize(object sender, EventArgs e)
        {
            labelTitle.Location = new Point((this.Width - labelTitle.Width) / 2, labelTitle.Location.Y);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (cmbWorkPost.SelectedIndex == -1)
            {
                MessageDialog.ShowPromptMessage("请选择岗位！");
                return;
            }

            if (numNumberOfPeople.Value == 0m)
            {
                MessageDialog.ShowPromptMessage("设定的部门岗位编制人数不能为0！");
                return;
            }

            HR_DeptPost deptPost = new HR_DeptPost();

            deptPost.DeptCode = m_departmentServer.GetDeptCode(SelectedNode.Text.Split(' ')[1].ToString());
            deptPost.ExistAmount = 0;
            deptPost.NumberOfPeople = Convert.ToInt32(numNumberOfPeople.Value);
            deptPost.PostID = m_postServer.GetOperatingPostByPostName(cmbWorkPost.Text).岗位编号;
            deptPost.Recorder = BasicInfo.LoginID;
            deptPost.RecordTime = ServerTime.Time;
            deptPost.Remark = txtRemark.Text;

            if (!m_postServer.AddDeptPost(deptPost, out m_error))
            {
                MessageDialog.ShowPromptMessage(m_error);
                return;
            }

            string[] s = SelectedNode.Text.Split(' ');
            IQueryable<View_HR_DeptPost> postNum = m_postServer.GetDeptPostByDeptCode(s[1].ToString());

            DataTable dt = GlobalObject.GeneralFunction.ConvertToDataTable<View_HR_DeptPost>(postNum);
            dgvPostNum.DataSource = dt;
        }

        private void dgvPostNum_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtCount.Text = dgvPostNum.CurrentRow.Cells["编制人数"].Value.ToString();
            txtExistCount.Text = dgvPostNum.CurrentRow.Cells["在岗人数"].Value.ToString();
            cmbWorkPost.Text = dgvPostNum.CurrentRow.Cells["岗位名称"].Value.ToString();
            numNumberOfPeople.Value = Convert.ToDecimal(dgvPostNum.CurrentRow.Cells["编制人数"].Value);
            txtRemark.Text = dgvPostNum.CurrentRow.Cells["备注"].Value.ToString();
        }

        private void dgvPostNum_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            Rectangle rectangle = new Rectangle(e.RowBounds.Location.X,
                e.RowBounds.Location.Y,
                dgvPostNum.RowHeadersWidth - 4,
                e.RowBounds.Height);

            TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(),
                dgvPostNum.RowHeadersDefaultCellStyle.Font,
                rectangle,
                dgvPostNum.RowHeadersDefaultCellStyle.ForeColor,
                TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
        }

        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (txtExistCount.Text == "0")
            {
                HR_DeptPost deptPost = new HR_DeptPost();

                deptPost.DeptCode = m_departmentServer.GetDeptCode(SelectedNode.Text.Split(' ')[1].ToString());
                deptPost.PostID = m_postServer.GetOperatingPostByPostName(cmbWorkPost.Text).岗位编号;

                if (!m_postServer.DeleteDeptPost(deptPost, out m_error))
                {
                    MessageDialog.ShowPromptMessage(m_error);
                }
            }
            else
            {
                MessageDialog.ShowPromptMessage("此岗位有在岗人员，不能删除！");
                return;
            }

            string[] s = SelectedNode.Text.Split(' ');
            IQueryable<View_HR_DeptPost> postNum = m_postServer.GetDeptPostByDeptCode(s[1].ToString());

            DataTable dt = GlobalObject.GeneralFunction.ConvertToDataTable<View_HR_DeptPost>(postNum);
            dgvPostNum.DataSource = dt;
        }

        private void 导入toolStripButton_Click(object sender, EventArgs e)
        {
            DataTable dtTemp = ExcelHelperP.RenderFromExcel(openFileDialog1);

            if (dtTemp == null)
            {
                //MessageDialog.ShowPromptMessage(m_error);
                return;
            }

            if (CheckTable(dtTemp))
            {
                if (!m_postServer.AddDeptPost(dtTemp, out m_error))
                {
                    MessageDialog.ShowPromptMessage(m_error);
                    return;
                }
                else
                {
                    MessageDialog.ShowPromptMessage("导入成功!");
                }
            }
        }

        /// <summary>
        /// 检查Excel表的数据
        /// </summary>
        /// <param name="dtcheck">表</param>
        /// <returns>返回是否正确</returns>
        bool CheckTable(DataTable dtcheck)
        {
            if (!dtcheck.Columns.Contains("部门"))
            {
                MessageDialog.ShowPromptMessage("此文件中不包含【部门】信息");
                return false;
            }

            if (!dtcheck.Columns.Contains("岗位名称"))
            {
                MessageDialog.ShowPromptMessage("此文件中不包含【岗位名称】信息");
                return false;
            }

            if (!dtcheck.Columns.Contains("编制人数"))
            {
                MessageDialog.ShowPromptMessage("此文件中不包含【编制人数】信息");
                return false;
            }

            if (!dtcheck.Columns.Contains("备注"))
            {
                MessageDialog.ShowPromptMessage("此文件中不包含【备注】信息");
                return false;
            }

            return true;
        }

        private void 导出toolStripButton_Click(object sender, EventArgs e)
        {
            ExcelHelperP.DatagridviewToExcel(saveFileDialog1, dgvPostNum);
        }
    }
}

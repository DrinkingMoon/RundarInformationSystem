using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Service_Peripheral_HR;
using PlatformManagement;
using ServerModule;
using Expression;
using GlobalObject;
using UniversalControlLibrary;

namespace Form_Peripheral_HR
{
    /// <summary>
    /// 部门主界面
    /// </summary>
    public partial class UserControlHRDepartment : Form
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
        /// 人员档案服务组件
        /// </summary>
        IPersonnelArchiveServer m_personnerServer = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<IPersonnelArchiveServer>();

        /// <summary>
        /// 部门类型数组
        /// </summary>
        HR_DeptType[] m_deptTypes;

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

        public UserControlHRDepartment(PlatformManagement.FunctionTreeNodeInfo nodeInfo)
        {
            InitializeComponent();            

            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
            UpdateStyles();

            m_authFlag = nodeInfo.Authority;

            m_deptTypes = m_departmentServer.GetAllDeptType().ToArray();
            cmbDeptType.Items.AddRange((from r in m_deptTypes select r.TypeName).ToArray());
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
        void RefreshTreeView(TreeNode tn, IQueryable<View_HR_Dept> findDepartment)
        {
            DataTable tempTable = GlobalObject.GeneralFunction.ConvertToDataTable<View_HR_Dept>(findDepartment.AsEnumerable());

            tempTable.Columns.Add("ShowName");

            foreach (DataRow dr in tempTable.Rows)
            {
                dr["ShowName"] = string.Format("({0}) {1}", dr["部门代码"], dr["部门名称"]);
            }

            List<TreeNode> lstNode = GlobalObject.GeneralFunction.GetListNode(tempTable, "ShowName", "部门代码", 
                "父级编码", "父级编码 = ''");

            tn.Nodes.AddRange(lstNode.ToArray());
        }

        /// <summary>
        /// 递归生成树
        /// </summary>
        /// <param name="parent">父节点</param>
        /// <param name="deptInfo">部门信息</param>
        void RecursionGenerateTree(TreeNode parent, List<View_HR_Dept> deptInfo)
        {
            for (int i = 0; deptInfo.Count > 0; i++)
            {
                View_HR_Dept curDept = deptInfo[i];

                if ((parent.Tag as View_HR_Dept).部门代码 != curDept.父级编码)
                {
                    continue;
                }

                TreeNode node = GenerateTreeNode(curDept);
                parent.Nodes.Add(node);

                if (deptInfo.Count > 0 && deptInfo[0].父级编码 == curDept.部门代码)
                {
                    RecursionGenerateTree(node, deptInfo);
                }
            }
        }

        /// <summary>
        /// 根据界面数据生成部门信息对象
        /// </summary>
        /// <returns>返回生成的部门信息对象</returns>
        private HR_Dept CreateDeptObjectFromForm()
        {
            HR_Dept dept = new HR_Dept();

            dept.DeptCode = txtCode.Text;
            dept.DeptName = txtName.Text;
            dept.FatherCode = txtFatherCode.Text.Trim();
            dept.DeptTypeID = m_deptTypes[cmbDeptType.SelectedIndex].TypeID;
            dept.OrderID = Convert.ToInt32(numOrder.Value);
            dept.Telephone = txtTelephone.Text;
            dept.Fax = txtFax.Text;
            dept.DeleteFlag = false;
            dept.Remark = txtRemark.Text;
            dept.Recorder = BasicInfo.LoginID;
            dept.RecordTime = ServerTime.Time;

            return dept;
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            RefreshControl();
        }

        /// <summary>
        /// 更改服务组件大小
        /// </summary>
        private void UserControlHRDepartment_Resize(object sender, EventArgs e)
        {
            labelTitle.Location = new Point((this.Width - labelTitle.Width) / 2, labelTitle.Location.Y);
        }

        /// <summary>
        /// 窗体载入
        /// </summary>
        private void UserControlHRDepartment_Load(object sender, EventArgs e)
        {
            FaceAuthoritySetting.SetVisibly(toolStrip1, m_authFlag);
            FaceAuthoritySetting.SetEnable(this.Controls, m_authFlag);

            刷新toolStripButton1_Click(null, null);
            txtCode.Focus();
        }

        /// <summary>
        /// 清空控件值
        /// </summary>
        void ClearControl()
        {
            txtCode.Text = "";
            txtName.Text = "";
            cmbDeptType.SelectedIndex = -1;

            cmbPrincipal.Items.Clear();
            cmbPrincipal.SelectedIndex = -1;
            cmbPrincipal.Tag = null;

            cmbDirector.Items.Clear();
            cmbDirector.SelectedIndex = -1;
            cmbDirector.Tag = null;

            cmbLeader.Items.Clear();
            cmbLeader.SelectedIndex = -1;
            cmbLeader.Tag = null;

            txtTelephone.Text = "";
            txtFax.Text = "";
            txtRemark.Text = "";
        }

        /// <summary>
        /// 刷新控件
        /// </summary>
        void RefreshControl()
        {
            ClearControl();

            if (SelectedNode == null || SelectedNode.Tag == null || SelectedNode.Tag.ToString() == "system")
            {
                return;
            }

            View_HR_Dept dept = m_departmentServer.GetDeptByDeptCode(SelectedNode.Tag.ToString());

            txtCode.Text = dept.部门代码;
            txtName.Text = dept.部门名称;

            // 获取部门负责人
            IQueryable<View_SelectPersonnel> directorGroup = m_personnerServer.GetDirector(dept.部门代码,"1");

            if (directorGroup != null && directorGroup.Count() > 0)
            {
                foreach (var item in directorGroup)
                {
                    cmbPrincipal.Items.Add(item.员工姓名);
                }

                cmbPrincipal.SelectedIndex = 0;
                cmbPrincipal.Tag = directorGroup.ToList();
            }

            // 获取部门主管
            directorGroup = null;
            directorGroup = m_personnerServer.GetDirector(dept.部门代码,"0");

            if (directorGroup != null && directorGroup.Count() > 0)
            {
                foreach (var item in directorGroup)
                {
                    cmbDirector.Items.Add(item.员工姓名);
                }

                cmbDirector.SelectedIndex = 0;
                cmbDirector.Tag = directorGroup.ToList();
            }

            // 获取部门分管领导
            directorGroup = null;
            directorGroup = m_personnerServer.GetDirector(dept.部门代码,"2");

            if (directorGroup != null && directorGroup.Count() > 0)
            {
                foreach (var item in directorGroup)
                {
                    cmbLeader.Items.Add(item.员工姓名);
                }

                cmbLeader.SelectedIndex = 0;
                cmbLeader.Tag = directorGroup.ToList();
            }

            if (dept.电话 != null)
            {
                txtTelephone.Text = dept.电话;
            }

            if (dept.传真 != null)
            {
                txtFax.Text = dept.传真;
            }

            if (dept.备注 != null)
            {
                txtRemark.Text = dept.备注;
            }

            cmbDeptType.Text = dept.部门类型;
            numOrder.Value = dept.排序号;

            txtFatherCode.Text = dept.父级编码;
        }

        /// <summary>
        /// 检测是否允许添加/修改部门
        /// </summary> 
        bool CheckDataItem()
        {
            txtCode.Text = txtCode.Text.Trim();

            if (txtCode.Text == "")
            {
                MessageDialog.ShowPromptMessage("部门编码不能为空!");
                return false;
            }

            //if (txtCode.Text.Length != 2)
            //{
            //    string parentCode = txtCode.Text.Substring(0, txtCode.Text.Length - 2);

            //    if (0 == treeView1.Nodes.Find(parentCode, true).Length)
            //    {
            //        MessageDialog.ShowPromptMessage(string.Format("找不到 [{0}] 的父编码 [{1}]", txtCode.Text, parentCode));
            //        return false;
            //    }
            //}

            if (txtName.Text == "")
            {
                MessageDialog.ShowPromptMessage("部门名称不能为空!");
                return false;
            }

            if (txtFatherCode.Text.Trim() == "")
            {
                if (MessageDialog.ShowEnquiryMessage("添加的部门是否为第一级部门？") == DialogResult.No)
                {
                    MessageDialog.ShowPromptMessage("请输入父级部门编码！");
                    return false;
                }
            }

            if (txtFatherCode.Text.Trim() != "")
            {
                if (m_departmentServer.GetDeptByDeptCode(txtFatherCode.Text) == null)
                {
                    MessageDialog.ShowPromptMessage("父级部门编码错误，请填写正确的父部门编码！");
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 添加
        /// </summary>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!CheckDataItem())
            {
                return;
            }

            HR_Dept dept = CreateDeptObjectFromForm();

            if (!m_departmentServer.AddDeptInfo(dept, out m_error))
            {
                MessageDialog.ShowErrorMessage(m_error);
                return;
            }

            刷新toolStripButton1_Click(null, null);
        }

        /// <summary>
        /// 删除
        /// </summary>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (SelectedNode == null || SelectedNode.Tag == null)
            {
                MessageDialog.ShowPromptMessage("请选择需要删除的信息!");
                return;
            }
            else
            {
                View_HR_Dept dept = m_departmentServer.GetDeptByDeptCode(SelectedNode.Tag.ToString());

                if (MessageDialog.ShowEnquiryMessage("您是否确定要删除部门 [" + dept.部门名称 + "] 信息?") == DialogResult.Yes)
                {
                    if (!m_departmentServer.DeleteDeptInfo(dept, out m_error))
                    {
                        MessageDialog.ShowPromptMessage(m_error);
                        return;
                    }

                    刷新toolStripButton1_Click(null, null);
                }
                else
                {
                    return;
                }
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (SelectedNode == null || SelectedNode.Tag == null)
            {
                MessageDialog.ShowPromptMessage("请选择需要删除的信息!");
                return;
            }

            View_HR_Dept viewDept = m_departmentServer.GetDeptByDeptCode(SelectedNode.Tag.ToString());

            if (viewDept.部门代码 != txtCode.Text)
            {
                MessageDialog.ShowPromptMessage("部门编码不允许修改!");

                txtCode.Text = viewDept.部门代码;

                return;
            }

            HR_Dept dept = new HR_Dept();

            dept.DeptCode = txtCode.Text;
            dept.DeptName = txtName.Text;
            dept.DeptTypeID = m_deptTypes[cmbDeptType.SelectedIndex].TypeID;
            dept.OrderID = Convert.ToInt32(numOrder.Value);
            dept.Telephone = txtTelephone.Text;
            dept.Fax = txtFax.Text;
            dept.DeleteFlag = false;
            dept.Remark = txtRemark.Text;
            dept.Recorder = BasicInfo.LoginID;
            dept.RecordTime = ServerTime.Time;
            dept.FatherCode = txtFatherCode.Text.Trim();

            if (!m_departmentServer.UpdataDeptInfo(dept, out m_error))
            {
                MessageDialog.ShowErrorMessage(m_error);

                return;
            }

            SelectedNode.Text = string.Format("({0}) {1}", dept.DeptCode, dept.DeptName);
            SelectedNode.Tag = dept.DeptCode;

            MessageDialog.ShowPromptMessage("修改成功！");
            刷新toolStripButton1_Click(null, null);
        }

        private void 刷新toolStripButton1_Click(object sender, EventArgs e)
        {
            this.treeView1.Nodes.Clear();
            TreeNode node = new TreeNode();

            node.Text = "部门分类";
            node.Name = "部门分类";
            node.Tag = "system";
            this.treeView1.Nodes.Add(node);

            if (!m_departmentServer.GetAllDeptInfo(out m_findDepartment, out m_error))
            {
                MessageDialog.ShowErrorMessage(m_error);
                return;
            }

            RefreshTreeView(node, m_findDepartment);
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            treeView1.ExpandAll();

            RefreshControl();
        }

        private void 部门类别toolStripButton1_Click(object sender, EventArgs e)
        {
            FormDeptType frm = new FormDeptType();
            frm.ShowDialog();

            RefreshControl();
        }

        private void btnChoose_Click(object sender, EventArgs e)
        {
            FormSelectPersonnel form = new FormSelectPersonnel("员工");
            //form.DeptCode = txtCode.Text;

            if (cmbPrincipal.Tag != null)
            {
                form.SelectedUser = (cmbPrincipal.Tag as List<View_SelectPersonnel>);
            }

            if (form.ShowDialog() == DialogResult.OK)
            {
                cmbPrincipal.Items.Clear();
                m_personnerServer.DeleteDeptDirector(txtCode.Text,"1");

                if (form.SelectedUser != null && form.SelectedUser.Count > 0)
                {
                    cmbPrincipal.Items.AddRange((from r in form.SelectedUser select r.员工姓名).ToArray());
                    cmbPrincipal.Tag = form.SelectedUser;
                    cmbPrincipal.SelectedIndex = 0;

                    if (!m_personnerServer.AddDeptDirector(txtCode.Text, form.SelectedUser,"1",true, out m_error))
                        MessageDialog.ShowErrorMessage(m_error);
                }
            }
        }

        private void btnDirector_Click(object sender, EventArgs e)
        {
            FormSelectPersonnel form = new FormSelectPersonnel("员工");

            if (cmbPrincipal.Tag != null)
            {
                form.SelectedUser = (cmbDirector.Tag as List<View_SelectPersonnel>);
            }

            if (form.ShowDialog() == DialogResult.OK)
            {
                if (!cbIsPermission.Checked)
                {
                    if (MessageDialog.ShowEnquiryMessage("设置的主管没有审批权限吗？有，请退出后先勾选审批权再选择主管;没有请点击【确定】") == DialogResult.No)
                    {
                        return;
                    }
                }

                cmbDirector.Items.Clear();
                m_personnerServer.DeleteDeptDirector(txtCode.Text, "0");

                if (form.SelectedUser != null && form.SelectedUser.Count > 0)
                {
                    cmbDirector.Items.AddRange((from r in form.SelectedUser select r.员工姓名).ToArray());
                    cmbDirector.Tag = form.SelectedUser;
                    cmbDirector.SelectedIndex = 0;

                    if (!m_personnerServer.AddDeptDirector(txtCode.Text, form.SelectedUser, "0", cbIsPermission.Checked, out m_error))
                        MessageDialog.ShowErrorMessage(m_error);
                }
            }
        }

        private void btnLeader_Click(object sender, EventArgs e)
        {
            FormSelectPersonnel form = new FormSelectPersonnel("员工");

            if (cmbPrincipal.Tag != null)
            {
                form.SelectedUser = (cmbLeader.Tag as List<View_SelectPersonnel>);
            }

            if (form.ShowDialog() == DialogResult.OK)
            {
                cmbLeader.Items.Clear();
                m_personnerServer.DeleteDeptDirector(txtCode.Text, "2");

                if (form.SelectedUser != null && form.SelectedUser.Count > 0)
                {
                    cmbLeader.Items.AddRange((from r in form.SelectedUser select r.员工姓名).ToArray());
                    cmbLeader.Tag = form.SelectedUser;
                    cmbLeader.SelectedIndex = 0;

                    if (!m_personnerServer.AddDeptDirector(txtCode.Text, form.SelectedUser, "2",true, out m_error))
                        MessageDialog.ShowErrorMessage(m_error);
                }
            }
        }
    }
}

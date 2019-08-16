/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  UserControlDepartment.cs
 * 作者    :  夏石友    版本: v1.00    日期: 2009/06/15
 * 开发平台:  vs2005(c#)
 * 用于    :  生产线管理信息系统
 *----------------------------------------------------------------------------
 * 描述 : 关于界面
 * 其它 :
 *----------------------------------------------------------------------------
 * 历史记录:
 *     1. 日期时间: 2009/07/03 08:02:08 作者: 夏石友 当前版本: V1.00
 *        修改说明: 创建
 ******************************************************************************/
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
using UniversalControlLibrary;

namespace Expression
{
    /// <summary>
    /// 部门组件
    /// </summary>
    public partial class UserControlDepartment : Form
    {
        #region variants

        /// <summary>
        /// 错误信息
        /// </summary>
        string m_err;

        /// <summary>
        /// 查找到的符合条件的部门信息
        /// </summary>
        IQueryable<View_Department> m_findDepartment;

        /// <summary>
        /// 部门信息服务组件
        /// </summary>
        IDepartmentServer m_departmentServer = ServerModuleFactory.GetServerModule<IDepartmentServer>();

        /// <summary>
        /// 人员信息服务组件
        /// </summary>
        IPersonnelInfoServer m_personnelServer = ServerModuleFactory.GetServerModule<IPersonnelInfoServer>();

        /// <summary>
        /// 部门类型数组
        /// </summary>
        DepartmentType[] m_deptTypes;

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

        #endregion

        public UserControlDepartment(PlatformManagement.FunctionTreeNodeInfo nodeInfo)
        {
            InitializeComponent();

            m_authFlag = nodeInfo.Authority;
            
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
            UpdateStyles();

            m_deptTypes = m_departmentServer.GetAllDepartmentType().ToArray();
            cmbDeptType.Items.AddRange((from r in m_deptTypes select r.TypeName).ToArray());
        }

        /// <summary>
        /// 改变组件大小
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControlDepartment_Resize(object sender, EventArgs e)
        {
            labelTitle.Location = new Point((this.Width - labelTitle.Width) / 2, labelTitle.Location.Y);
        }

        /// <summary>
        /// 窗体载入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControlDepartment_Load(object sender, EventArgs e)
        {
            FaceAuthoritySetting.SetVisibly(toolStrip1, m_authFlag);
            FaceAuthoritySetting.SetEnable(this.Controls, m_authFlag);

            if (!m_departmentServer.GetAllDepartment(out m_findDepartment, out m_err))
            {
                MessageDialog.ShowErrorMessage(m_err);
                return;
            }

            RefreshTreeView(m_findDepartment);
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            treeView1.ExpandAll();

            txtCode.Focus();
            RefreshControl();
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
        /// 生成树节点
        /// </summary>
        /// <param name="dept">部门信息</param>
        /// <returns>返回生成的树节点</returns>
        TreeNode GenerateTreeNode(Department dept)
        {
            TreeNode node = new TreeNode();

            node.Name = dept.DepartmentCode;
            node.Text = string.Format("({0}) {1}", dept.DepartmentCode, dept.DepartmentName);
            node.Tag = dept;

            return node;
        }

        /// <summary>
        /// 刷新树视图
        /// </summary>
        /// <param name="findDepartmentBill">数据列表</param>
        void RefreshTreeView(IQueryable<View_Department> findDepartmentBill)
        {
            if (findDepartmentBill.Count() == 0)
            {
                return;
            }

            List<View_Department> deptInfo = findDepartmentBill.ToList();

            for (int i = 0; deptInfo.Count > 0;)
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
        /// 根据界面数据生成部门信息对象
        /// </summary>
        /// <returns>返回生成的部门信息对象</returns>
        private Department CreateDeptObjectFromForm()
        {
            Department dept = new Department();

            dept.DepartmentCode = txtCode.Text;
            dept.DepartmentName = txtName.Text;
            dept.DeptType = m_deptTypes[cmbDeptType.SelectedIndex].TypeID;
            dept.Telephone = txtTelephone.Text;
            dept.Fax = txtFax.Text;
            dept.Remark = txtRemark.Text;

            return dept;
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

            Department dept = CreateDeptObjectFromForm();

            if (!m_departmentServer.AddDepartment(dept, out m_err))
            {
                MessageDialog.ShowErrorMessage(m_err);
                return;
            }

            View_Department viewDept = m_departmentServer.GetDepartments(dept.DepartmentCode);

            if (txtCode.Text.Length == 2)
            {
                treeView1.Nodes.Add(GenerateTreeNode(viewDept));
            }
            else
            {
                string parentCode = txtCode.Text.Substring(0, txtCode.Text.Length - 2);
                treeView1.Nodes.Find(parentCode, true)[0].Nodes.Add(GenerateTreeNode(viewDept));
            }
        }

        /// <summary>
        /// 检测是否允许添加/修改部门
        /// </summary> 
        /// <returns>检测通过返回True，检测失败返回False</returns>
        bool CheckDataItem()
        {
            txtCode.Text = txtCode.Text.Trim();

            if (txtCode.Text == "")
            {
                MessageDialog.ShowPromptMessage("部门编码不能为空!");
                return false;
            }

            if (txtCode.Text.Length % 2 != 0 || txtCode.Text.Length > 6)
            {
                MessageDialog.ShowPromptMessage("部门编码格式不正确, 编码长度只能为2, 4, 6");
                return false;
            }

            if (txtCode.Text.Length != 2)
            {
                string parentCode = txtCode.Text.Substring(0, txtCode.Text.Length - 2);
                
                if (0 == treeView1.Nodes.Find(parentCode, true).Length)
                {
                    MessageDialog.ShowPromptMessage(string.Format("找不到 [{0}] 的父编码 [{1}]", txtCode.Text, parentCode));
                    return false;
                }
            }

            if (txtName.Text == "")
            {
                MessageDialog.ShowPromptMessage("部门名称不能为空!");
                return false;
            }

            return true;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (SelectedNode == null || SelectedNode.Tag == null)
            {
                MessageDialog.ShowPromptMessage("请选择需要删除的信息!");
                return;
            }
            else
            {
                string deptCode = (SelectedNode.Tag as View_Department).部门代码;

                if (MessageDialog.ShowEnquiryMessage("您是否确定要删除部门 [" + deptCode + "] 信息?") == DialogResult.Yes)
                {
                    if (!m_departmentServer.DeleteDepartment(deptCode, out m_err))
                    {
                        MessageDialog.ShowPromptMessage(m_err);
                        return;
                    }

                    SelectedNode.Parent.Nodes.Remove(SelectedNode);
                }
                else
                {
                    return;
                }
            }
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

            txtTelephone.Text = "";
            txtFax.Text = "";
            txtRemark.Text = "";
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (SelectedNode == null || SelectedNode.Tag == null)
            {
                MessageDialog.ShowPromptMessage("请选择需要删除的信息!");
                return;
            }

            View_Department viewDept = SelectedNode.Tag as View_Department;

            if (viewDept.部门代码 != txtCode.Text)
            {
                MessageDialog.ShowPromptMessage("部门编码不允许修改!");
                txtCode.Text = viewDept.部门代码;
                return;
            }

            Department dept = CreateDeptObjectFromForm();

            if (!m_departmentServer.UpdataDepartment(dept, out m_err))
            {
                MessageDialog.ShowErrorMessage(m_err);
                return;
            }

            SelectedNode.Text = string.Format("({0}) {1}", dept.DepartmentCode, dept.DepartmentName);
            SelectedNode.Tag = m_departmentServer.GetDepartments(dept.DepartmentCode);
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

            View_Department dept = SelectedNode.Tag as View_Department;

            txtCode.Text = dept.部门代码;
            txtName.Text = dept.部门名称;
            
            // 获取部门负责人

            cmbDeptType.Text = dept.部门类型;

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

            IQueryable<View_HR_Personnel> directorGroup = m_personnelServer.GetDeptDirector(dept.部门代码);

            if (directorGroup != null && directorGroup.Count() > 0)
            {
                foreach (var item in directorGroup)
                {
                    cmbPrincipal.Items.Add(item.姓名);
                }

                cmbPrincipal.SelectedIndex = 0;
                cmbPrincipal.Tag = directorGroup.ToList();
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            RefreshControl();
        }

        private void btnFindPrincipal_Click(object sender, EventArgs e)
        {
            FormSelectPersonnel form = new FormSelectPersonnel();
            //form.DeptCode = txtCode.Text;

            if (cmbPrincipal.Tag != null)
            {
                form.SelectedUser = (cmbPrincipal.Tag as List<View_HR_Personnel>);
            }

            if (form.ShowDialog() == DialogResult.OK)
            {
                cmbPrincipal.Items.Clear();
                m_personnelServer.DeleteDeptDirector(txtCode.Text);

                if (form.SelectedUser != null && form.SelectedUser.Count > 0)
                {
                    cmbPrincipal.Items.AddRange((from r in form.SelectedUser select r.姓名).ToArray());
                    cmbPrincipal.Tag = form.SelectedUser;
                    cmbPrincipal.SelectedIndex = 0;

                    if (!m_personnelServer.AddDeptDirector(txtCode.Text, form.SelectedUser, out m_err))
                        MessageDialog.ShowErrorMessage(m_err);
                }
            }
        }
    }
}

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

namespace UniversalControlLibrary
{
    /// <summary>
    /// 人员选择窗体
    /// </summary>
    public partial class FormSelectPersonnel2 : Form
    {
        bool _IsMultiSelect = true;

        public bool IsMultiSelect
        {
            get { return _IsMultiSelect; }
            set { _IsMultiSelect = value; }
        }

        BillFlowMessage_ReceivedUserType m_receivedUserType;

        ///// <summary>
        ///// 选择的人员实体集列表
        ///// </summary>
        //public BillFlowMessage_ReceivedUserType ReceivedUserType
        //{
        //    get { return m_receivedUserType; }
        //    set { m_receivedUserType = value; }
        //}

        /// <summary>
        /// 选择的人员实体集列表
        /// </summary>
        List<PersonnelBasicInfo> m_listSelectedPersonnels = new List<PersonnelBasicInfo>();

        ///// <summary>
        ///// 选择的人员实体集列表
        ///// </summary>
        //public List<PersonnelBasicInfo> SelectedPersonnels
        //{
        //    get { return m_listSelectedPersonnels; }
        //    set { m_listSelectedPersonnels = value; }
        //}

        NotifyPersonnelInfo m_notifyPersonnelInfo = new NotifyPersonnelInfo();

        public NotifyPersonnelInfo SelectedNotifyPersonnelInfo
        {
            get { return m_notifyPersonnelInfo; }
            set { m_notifyPersonnelInfo = value; }
        }

        #region 2013-11-18 夏石友，便于与调用方交互

        /// <summary>
        /// 检查选择的人员是否有效的事件
        /// </summary>
        public event GlobalObject.DelegateCollection.CheckSelectedPersonnel OnCheckSelectedPersonnel;

        #endregion

        /// <summary>
        /// 部门服务组件
        /// </summary>
        IDepartmentServer m_departmentServer = ServerModuleFactory.GetServerModule<IDepartmentServer>();

        /// <summary>
        /// 人员服务组件
        /// </summary>
        IPersonnelInfoServer m_personnelInfo = ServerModuleFactory.GetServerModule<IPersonnelInfoServer>();

        /// <summary>
        /// 选中的树节点
        /// </summary>
        TreeNode SelectedNode
        {
            get { return treeView1.SelectedNode; }
        }

        public FormSelectPersonnel2()
        {
            InitializeComponent();

            string strSql = @"select 部门代码,部门名称,case when  父级编码  = '' then '0' else 父级编码 end 父级编码
                                from View_HR_Dept where 部门代码 != '00' and DeleteFlag = 0 
                                Union all select '0','容大',''";

            GlobalObject.GeneralFunction.LoadTreeViewDt(this.treeView1,
                GlobalObject.DatabaseServer.QueryInfo(strSql), "部门名称", "部门代码", "父级编码", " 父级编码 = '' ");

            treeView1.Nodes[0].Expand();

            DataTable tempPersonnel = new DataTable();

            tempPersonnel.Columns.Add("员工编号");
            tempPersonnel.Columns.Add("员工姓名");

            dataGridView2.DataSource = tempPersonnel;
            customDataGridView1.DataSource = m_personnelInfo.GetInfo_Find(BillFlowMessage_ReceivedUserType.角色, "");
        }

        /// <summary>
        /// 刷新数据
        /// </summary>
        void RefreshData()
        {
            if (SelectedNode == null)
            {
                return;
            }

            DataTable dtTemp = m_personnelInfo.GetInfo(treeView1.SelectedNode.Tag.ToString() == "0" ? "" : 
                treeView1.SelectedNode.Tag.ToString());

            dataGridView2.DataSource = dtTemp;
        }

        private void btnCancenl_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void UserIn()
        {
            DataTable tempPersonnel = (DataTable)dataGridView1.DataSource;

            string strPersonnelCode = "";

            if (SelectedNode != null)
            {
                DataTable dtInfo = m_personnelInfo.GetInfo(treeView1.SelectedNode.Tag.ToString() == "0" ? "" :
                    treeView1.SelectedNode.Tag.ToString());

                foreach (DataRow drInfo in dtInfo.Rows)
                {
                    DataRow dr = tempPersonnel.NewRow();

                    dr["员工编号"] = drInfo["员工编号"];
                    dr["员工姓名"] = drInfo["员工姓名"];

                    bool flag = false;

                    foreach (DataRow drCheck in tempPersonnel.Rows)
                    {
                        if (drCheck["员工编号"].ToString() == drInfo["员工编号"].ToString())
                        {
                            flag = true;
                            break;
                        }
                    }

                    if (!flag)
                    {
                        tempPersonnel.Rows.Add(dr);
                        strPersonnelCode = dr["员工编号"].ToString();
                    }
                }
            }
            else
            {
                if (dataGridView2.CurrentRow == null)
                {
                    MessageBox.Show("请选择要添加的人员后再进行此操作");
                    return;
                }

                DataRow dr = tempPersonnel.NewRow();

                dr["员工编号"] = dataGridView2.CurrentRow.Cells["员工编号"].Value;
                dr["员工姓名"] = dataGridView2.CurrentRow.Cells["员工姓名"].Value;

                bool flag = false;

                foreach (DataRow drCheck in tempPersonnel.Rows)
                {
                    if (drCheck["员工编号"].ToString() == dataGridView2.CurrentRow.Cells["员工编号"].Value.ToString())
                    {
                        flag = true;
                        break;
                    }
                }

                if (!flag)
                {
                    tempPersonnel.Rows.Add(dr);
                    strPersonnelCode = dr["员工编号"].ToString();
                }
            }

            string strColName = "";

            foreach (DataGridViewColumn col in dataGridView1.Columns)
            {
                if (col.Visible)
                {
                    strColName = col.Name;
                    break;
                }
            }

            dataGridView1.DataSource = tempPersonnel;

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if ((string)dataGridView1.Rows[i].Cells["员工编号"].Value.ToString() == strPersonnelCode)
                {
                    dataGridView1.FirstDisplayedScrollingRowIndex = i;
                    dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells[strColName];
                    break;
                }
            }
        }

        void UserOut()
        {
            DataTable tempPersonnel = (DataTable)dataGridView1.DataSource;

            List<DataGridViewRow> lstRow = new List<DataGridViewRow>();

            for (int i = 0; i < dataGridView1.SelectedRows.Count; i++)
            {
                lstRow.Add(dataGridView1.SelectedRows[i]);
            }

            for (int i = 0; i < lstRow.Count; i++)
            {
                for (int j = 0; j < tempPersonnel.Rows.Count; j++)
                {
                    if (lstRow[i].Cells["员工编号"].Value.ToString() == tempPersonnel.Rows[j]["员工编号"].ToString())
                    {
                        tempPersonnel.Rows.RemoveAt(j);
                        break;
                    }
                }
            }

            dataGridView1.DataSource = tempPersonnel;
        }

        void RoleIn()
        {
            DataTable tempRole = (DataTable)dataGridView1.DataSource;

            string strRoleName = "";

            if (customDataGridView1.CurrentRow == null)
            {
                MessageBox.Show("请选择要添加的角色名称后再进行此操作");
                return;
            }

            DataRow dr = tempRole.NewRow();

            dr["角色名称"] = customDataGridView1.CurrentRow.Cells["角色名称"].Value;

            bool flag = false;

            foreach (DataRow drCheck in tempRole.Rows)
            {
                if (drCheck["角色名称"].ToString() == customDataGridView1.CurrentRow.Cells["角色名称"].Value.ToString())
                {
                    flag = true;
                    break;
                }
            }

            if (!flag)
            {
                tempRole.Rows.Add(dr);
                strRoleName = dr["角色名称"].ToString();
            }

            string strColName = "";

            foreach (DataGridViewColumn col in dataGridView1.Columns)
            {
                if (col.Visible)
                {
                    strColName = col.Name;
                    break;
                }
            }

            dataGridView1.DataSource = tempRole;

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if ((string)dataGridView1.Rows[i].Cells["角色名称"].Value.ToString() == strRoleName)
                {
                    dataGridView1.FirstDisplayedScrollingRowIndex = i;
                    dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells[strColName];
                    break;
                }
            }
        }

        void RoleOut()
        {
            DataTable tempRole = (DataTable)customDataGridView1.DataSource;
            foreach (DataRow dr in tempRole.Rows)
            {
                if (dr["角色名称"] == dataGridView1.CurrentRow.Cells["角色名称"].Value)
                {
                    tempRole.Rows.Remove(dr);
                    break;
                }
            }

            dataGridView1.DataSource = tempRole;
        }

        void InitForm()
        {
            m_receivedUserType =
                GlobalObject.GeneralFunction.StringConvertToEnum<BillFlowMessage_ReceivedUserType>(tabControl1.SelectedTab.Name);

            DataTable tempTable = new DataTable();

            switch (m_receivedUserType)
            {
                case BillFlowMessage_ReceivedUserType.用户:
                    tempTable = ((DataTable)dataGridView2.DataSource).Clone();
                    break;
                case BillFlowMessage_ReceivedUserType.角色:
                    tempTable = ((DataTable)customDataGridView1.DataSource).Clone();
                    break;
                default:
                    break;
            }

            dataGridView1.DataSource = tempTable;
        }

        private void btnIn_Click(object sender, EventArgs e)
        {
            m_receivedUserType =
                GlobalObject.GeneralFunction.StringConvertToEnum<BillFlowMessage_ReceivedUserType>(tabControl1.SelectedTab.Name);

            switch (m_receivedUserType)
            {
                case BillFlowMessage_ReceivedUserType.用户:
                    UserIn();
                    break;
                case BillFlowMessage_ReceivedUserType.角色:
                    RoleIn();
                    break;
                default:
                    break;
            }
        }

        private void btnOut_Click(object sender, EventArgs e)
        {
            m_receivedUserType =
                GlobalObject.GeneralFunction.StringConvertToEnum<BillFlowMessage_ReceivedUserType>(tabControl1.SelectedTab.Name);

            switch (m_receivedUserType)
            {
                case BillFlowMessage_ReceivedUserType.用户:
                    UserOut();
                    break;
                case BillFlowMessage_ReceivedUserType.角色:
                    RoleOut();
                    break;
                default:
                    break;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if ((DataTable)dataGridView1.DataSource == null || ((DataTable)dataGridView1.DataSource).Rows.Count == 0)
            {
                MessageDialog.ShowPromptMessage("请选择人员");
                return;
            }

            if (dataGridView1.Rows.Count > 1 && !_IsMultiSelect)
            {
                MessageDialog.ShowPromptMessage("无法选择多条记录");
                return;
            }

            m_receivedUserType =
                GlobalObject.GeneralFunction.StringConvertToEnum<BillFlowMessage_ReceivedUserType>(tabControl1.SelectedTab.Name);

            DataTable tempTable = new DataTable();

            switch (m_receivedUserType)
            {
                case BillFlowMessage_ReceivedUserType.用户:

                    DataTable tempPersonnel = (DataTable)dataGridView1.DataSource;

                    m_listSelectedPersonnels = new List<PersonnelBasicInfo>();

                    foreach (DataRow dr in tempPersonnel.Rows)
                    {
                        PersonnelBasicInfo entityInfo = new PersonnelBasicInfo();

                        entityInfo.工号 = dr["员工编号"].ToString();
                        entityInfo.姓名 = dr["员工姓名"].ToString();

                        m_listSelectedPersonnels.Add(entityInfo);
                    }
                    break;
                case BillFlowMessage_ReceivedUserType.角色:

                    DataTable tempRole = (DataTable)dataGridView1.DataSource;

                    m_listSelectedPersonnels = new List<PersonnelBasicInfo>();

                    foreach (DataRow dr in tempRole.Rows)
                    {
                        PersonnelBasicInfo entityInfo = new PersonnelBasicInfo();

                        entityInfo.角色 = dr["角色名称"].ToString();

                        m_listSelectedPersonnels.Add(entityInfo);
                    }
                    break;
                default:
                    break;
            }

            m_notifyPersonnelInfo.UserType = m_receivedUserType.ToString();
            m_notifyPersonnelInfo.PersonnelBasicInfoList = m_listSelectedPersonnels;

            #region 2013-11-18 如果选择人员有问题则返回
            if (OnCheckSelectedPersonnel != null)
            {
                if (!OnCheckSelectedPersonnel(m_listSelectedPersonnels))
                    return;
            }
            #endregion

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void FormSelectPersonnel2_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (DialogResult != DialogResult.OK)
            {
                m_listSelectedPersonnels = new List<PersonnelBasicInfo>();
            }
        }

        private void dataGridView1_Click(object sender, EventArgs e)
        {
            treeView1.SelectedNode = null;
            dataGridView2.CurrentCell = null;
            customDataGridView1.CurrentCell = null;
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            RefreshData();

            dataGridView1.CurrentCell = null;
            dataGridView2.CurrentCell = null;
        }

        private void dataGridView2_Click(object sender, EventArgs e)
        {
            treeView1.SelectedNode = null;
            dataGridView1.CurrentCell = null;
        }

        private void dataGridView2_DoubleClick(object sender, EventArgs e)
        {
            UserIn();
        }

        private void customDataGridView1_DoubleClick(object sender, EventArgs e)
        {
            RoleIn();
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            InitForm();
        }

        private void FormSelectPersonnel2_Load(object sender, EventArgs e)
        {
            InitForm();
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

        private void btnFind_Click(object sender, EventArgs e)
        {
            m_receivedUserType =
                GlobalObject.GeneralFunction.StringConvertToEnum<BillFlowMessage_ReceivedUserType>(tabControl1.SelectedTab.Name);

            switch (m_receivedUserType)
            {
                case BillFlowMessage_ReceivedUserType.用户:
                    dataGridView2.DataSource = m_personnelInfo.GetInfo_Find(m_receivedUserType, txtFind.Text.Trim());
                    break;
                case BillFlowMessage_ReceivedUserType.角色:
                    customDataGridView1.DataSource = m_personnelInfo.GetInfo_Find(m_receivedUserType, txtFind.Text.Trim());
                    break;
                default:
                    break;
            }
        }

        private void txtFind_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnFind_Click(sender, e);
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            ExcelHelperP.DatagridviewToExcel(saveFileDialog1, dataGridView1);
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            DataTable dtTemp = ExcelHelperP.RenderFromExcel(openFileDialog1);

            if (dtTemp == null)
            {
                return;
            }

            m_receivedUserType =
                GlobalObject.GeneralFunction.StringConvertToEnum<BillFlowMessage_ReceivedUserType>(tabControl1.SelectedTab.Name);

            switch (m_receivedUserType)
            {
                case BillFlowMessage_ReceivedUserType.用户:

                    if (!dtTemp.Columns.Contains("员工姓名") && !dtTemp.Columns.Contains("员工编号"))
                    {
                        MessageDialog.ShowPromptMessage("无【员工编号】或【员工姓名】列");
                        return;
                    }

                    DataTable tempTable = ((DataTable)dataGridView2.DataSource).Clone();

                    foreach (DataRow dr in dtTemp.Rows)
                    {
                        DataRow newRow = tempTable.NewRow();

                        View_HR_Personnel personnel = new View_HR_Personnel();

                        object obj = "";

                        if (dtTemp.Columns.Contains("员工编号"))
                        {
                            obj = dr["员工编号"];
                        }
                        else
                        {
                            obj = dr["员工姓名"];
                        }

                        if (obj == null)
                        {
                            MessageDialog.ShowPromptMessage("有一行记录为空，请重新检查");
                            return;
                        }

                        personnel = UniversalFunction.GetPersonnelInfo(obj.ToString());

                        if (personnel == null)
                        {
                            MessageDialog.ShowPromptMessage("【员工编号】/【员工姓名】： " + obj.ToString() + " 数据错误,请重新检查");
                            return;
                        }

                        newRow["员工编号"] = personnel.工号;
                        newRow["员工姓名"] = personnel.姓名;

                        tempTable.Rows.Add(newRow);
                    }

                    dataGridView1.DataSource = tempTable;

                    break;
                case BillFlowMessage_ReceivedUserType.角色:
                    break;
                default:
                    break;
            }
        }
    }
}

/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  UserControlAccessoryChoseConfect.cs
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
using PlatformManagement;
using UniversalControlLibrary;

namespace Expression
{
    /// <summary>
    /// 零件选配信息组件
    /// </summary>
    public partial class UserControlAccessoryChoseConfect : Form
    {
        /// <summary>
        /// datagridview列排序标志
        /// </summary>
        bool m_columnSortFlag = false;

        /// <summary>
        /// 错误信息
        /// </summary>
        string m_err;

        /// <summary>
        /// 零件选配信息字典
        /// </summary>
        Dictionary<string, Dictionary<string, List<decimal>>> m_dicAccessoryChoseConfectInfo = 
                                                            new Dictionary<string, Dictionary<string, List<decimal>>>();

        /// <summary>
        /// 零件信息窗体
        /// </summary>
        FormChoseConfectAccessorySpecCode m_formChoseConfectAccessorySpecCode;

        /// <summary>
        /// 选配表表头管理窗体
        /// </summary>
        FormChoseConfectHeadManage m_formChoseConfectHeadManage;

        /// <summary>
        /// 服务组件
        /// </summary>
        IChoseConfectServer m_choseConfectServer = ServerModuleFactory.GetServerModule<IChoseConfectServer>();

        /// <summary>
        /// 权限组件
        /// </summary>
        AuthorityFlag m_authFlag;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="nodeInfo">功能树节点信息</param>
        public UserControlAccessoryChoseConfect(PlatformManagement.FunctionTreeNodeInfo nodeInfo)
        {
            InitializeComponent();

            m_authFlag = nodeInfo.Authority;

            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);

            UpdateStyles();
        }

        private void UserControlAccessoryChoseConfect_Load(object sender, EventArgs e)
        {
            FaceAuthoritySetting.SetVisibly(toolStrip1, m_authFlag);
            List<string> listProduct = UniversalFunction.GetGoodsInfoList_Attribute(GlobalObject.CE_GoodsAttributeName.CVT, 
                bool.TrueString).Select(k => k.GoodsCode).ToList();
            listProduct.Add("");

            cmbProductType.DataSource = listProduct;
            cmbProductType.Text = "";
         
        }

        /// <summary>
        /// 初始化DataGridView的数据源的表头
        /// </summary>
        public void InitDataGridView1()
        {
            bool tableHeadFlag = false;
            string tableTitle = "零件选配信息";
            string id = "序号";
            string rangeTitle = "范围";
            string standardTitle = "选配";
            string unvisibleIndexTitle = "零件选配信息表ID";

            if (txtCode.Text.Trim().Length > 0)
            {
                if (m_choseConfectServer.GetChoseConfectTableHead(txtCode.Text, out tableTitle, 
                    out rangeTitle, out standardTitle, out m_err))
                {
                    labelTitle.Text = tableTitle;
                    labelTitle.Left = (this.Width - labelTitle.Width) / 2;
                    lbRangeData.Text = rangeTitle;
                    lbChoseData.Text = standardTitle;
                    tableHeadFlag = true;
                }
                else
                {
                    if (m_err != "没有找到任何数据")
                    {
                        MessageDialog.ShowErrorMessage(m_err);
                    }

                    btnAdd.Enabled = false;
                    btnUpdate.Enabled = false;
                    btnDelete.Enabled = false;
                    tableHeadFlag = false;
                    dataGridView1.DataSource = null;
                    //ResetDataGridView1(id, rangeTitle, standardTitle, unvisibleIndexTitle);
                    ResetPanelPara();
                    return;
                }
            }
            else
            {
                ResetPanelPara();
            }

            DataTable gridViewTable = new DataTable();

            gridViewTable.Columns.Add(id);
            gridViewTable.Columns.Add(rangeTitle);
            gridViewTable.Columns.Add(standardTitle);
            gridViewTable.Columns.Add(unvisibleIndexTitle);
            dataGridView1.DataSource = gridViewTable;
            dataGridView1.Columns[3].Visible = false;

            if (txtCode.Text.Trim().Length > 0)
            {
                InitDataGridViewContent(txtCode.Text, cmbProductType.Text, tableHeadFlag);
            }

            if (!m_columnSortFlag)
            {
                for (int i = 0; i < gridViewTable.Columns.Count; i++)
                {
                    //dataGridView1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                }

                m_columnSortFlag = true;
            }

            btnAdd.Enabled = true;
        }

        /// <summary>
        /// 还原dataGridView1初始数据源
        /// </summary>
        /// <param name="id">第1列表头名称</param>
        /// <param name="rangeTitle">第2列表头名称</param>
        /// <param name="standardTitle">第3列表头名称</param>
        /// <param name="indexs">第4列表头名称</param>
        /// <param name="spec">第5列表头名称</param>
        void ResetDataGridView1(string id, string rangeTitle, string standardTitle, string unvisibleIndexTitle)
        {
            DataTable gridViewTable = new DataTable();

            gridViewTable.Columns.Add(id);
            gridViewTable.Columns.Add(rangeTitle);
            gridViewTable.Columns.Add(standardTitle);
            gridViewTable.Columns.Add(unvisibleIndexTitle);
            dataGridView1.DataSource = gridViewTable;

            dataGridView1.Columns[3].Visible = false;
        }

        /// <summary>
        /// 重置右面板参数
        /// </summary>
        void ResetPanelPara()
        {
            labelTitle.Text = "零件选配信息";
            lbRangeData.Text = "范围：";
            lbChoseData.Text = "选配：";
            numMin.Value = 0;
            numMax.Value = 0;
            txtChoseConfect.Text = "";
        }

        /// <summary>
        /// 初始化DataGridView,显示某一零件的选配信息
        /// </summary>
        /// <param name="accessoryCode">零部件编码</param>
        /// <param name="productType">产品类型</param>
        /// <param name="tableHeadflag">选配表表头是否存在标志</param>
        void InitDataGridViewContent(string accessoryCode, string productType, bool tableHeadflag)
        {
            DataTable AccessoryChoseConfectTable;
            #region

            Dictionary<string, List<decimal>> dic1 = new Dictionary<string, List<decimal>>();

            if (m_dicAccessoryChoseConfectInfo.ContainsKey(accessoryCode))
            {
                m_dicAccessoryChoseConfectInfo.Remove(accessoryCode);
            }

            m_dicAccessoryChoseConfectInfo.Add(accessoryCode, dic1);

            #endregion

            if (m_choseConfectServer.GetAccessoryChoseConfectInfo(accessoryCode, productType, out AccessoryChoseConfectTable, out m_err))
            {
                if (AccessoryChoseConfectTable.Rows.Count > 0)
                {
                    m_dicAccessoryChoseConfectInfo[accessoryCode].Clear();

                    DataTable dataGridViewSourceTable = (DataTable)(dataGridView1.DataSource);

                    for (int i = 0; i < AccessoryChoseConfectTable.Rows.Count; i++)
                    {
                        int n = i + 1;

                        dataGridViewSourceTable.Rows.Add(new object[]{n.ToString(), AccessoryChoseConfectTable.Rows[i][1].ToString(),
                            AccessoryChoseConfectTable.Rows[i][2].ToString(), AccessoryChoseConfectTable.Rows[i][0].ToString()});

                        List<decimal> dbList = new List<decimal>();
                        string rangeData = AccessoryChoseConfectTable.Rows[i][1].ToString();

                        if (rangeData != "")
                        {
                            int index = rangeData.IndexOf("至");

                            string strMin = rangeData.Substring(0, index);
                            dbList.Add(Convert.ToDecimal(strMin));

                            string strMax = rangeData.Substring(index + 1, rangeData.Length - (index + 1));
                            dbList.Add(Convert.ToDecimal(strMax));

                        }

                        if (m_dicAccessoryChoseConfectInfo[accessoryCode].ContainsKey(AccessoryChoseConfectTable.Rows[i][2].ToString()))
                        {
                            m_dicAccessoryChoseConfectInfo[accessoryCode].Remove(AccessoryChoseConfectTable.Rows[i][2].ToString());
                        }

                        m_dicAccessoryChoseConfectInfo[accessoryCode].Add(AccessoryChoseConfectTable.Rows[i][2].ToString(), dbList);
                    }

                    dataGridView1.DataSource = dataGridViewSourceTable;
                    dataGridView1.Refresh();

                    if (dataGridViewSourceTable.Rows.Count > 0)
                    {
                        UpdataPanelPara();
                    }
                }
            }
            else
            {
                if (m_err != "没有找到任何数据")
                {
                    MessageDialog.ShowErrorMessage(m_err);
                }

                if (!tableHeadflag)
                {
                    ResetPanelPara();
                }
                else
                {
                    numMin.Value = 0;
                    numMax.Value = 0;
                    txtChoseConfect.Text = "";
                }

                btnUpdate.Enabled = false;
                btnDelete.Enabled = false;
            }
        }

        /// <summary>
        /// 更新右面板参数
        /// </summary>
        void UpdataPanelPara()
        {
            if (dataGridView1.CurrentRow != null)
            {
                string rangeData = dataGridView1.CurrentRow.Cells[1].Value.ToString();

                if (rangeData != "")
                {
                    int index = rangeData.IndexOf("至");

                    string strMin = rangeData.Substring(0, index);
                    numMin.Value = Convert.ToDecimal(strMin);

                    string strMax = rangeData.Substring(index + 1, rangeData.Length - (index + 1));
                    numMax.Value = Convert.ToDecimal(strMax);

                    txtChoseConfect.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();

                }
                else
                {
                    numMin.Value = 0;
                    numMax.Value = 0;
                    txtChoseConfect.Text = "";
                }
            }

            btnUpdate.Enabled = true;
            btnDelete.Enabled = true;
        }

        /// <summary>
        /// 查找零件编码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFindCode_Click(object sender, EventArgs e)
        {
            m_formChoseConfectAccessorySpecCode = null;
            m_formChoseConfectAccessorySpecCode = new FormChoseConfectAccessorySpecCode(txtCode);//, txtSpec
            m_formChoseConfectAccessorySpecCode.ShowDialog();
        }

        private void txtCode_TextChanged(object sender, EventArgs e)
        {
            InitDataGridView1();
        }

        /// <summary>
        /// 点击dataGridView1单元格事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            UpdataPanelPara();
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (CheckAddChoseConfectInfo(txtCode.Text))
            {
                if (UpdataAccessoryChoseConfectInfo(null))
                {
                    InitDataGridView1();
                }
                else
                {
                    MessageDialog.ShowPromptMessage("添加零件选配信息失败!");
                }
            }
        }

        /// <summary>
        /// 检验零件选配信息
        /// </summary>
        /// <returns>返回是否允许添加零件选配信息</returns>
        bool CheckAddChoseConfectInfo(string accessoryCode)
        {
            if (numMin.Value >= numMax.Value)
            {
                MessageDialog.ShowPromptMessage("范围顺序颠倒有误,请重新输入!");
                return false;
            }

            if (m_dicAccessoryChoseConfectInfo[accessoryCode].ContainsKey(txtChoseConfect.Text))
            {
                MessageDialog.ShowPromptMessage("数据库中已存在相同选配的信息!");
                return false;
            }
            else
            {
                foreach (KeyValuePair<string, List<decimal>> var in m_dicAccessoryChoseConfectInfo[accessoryCode])
                {
                    List<decimal> dbList = var.Value;

                    if (numMin.Value >= dbList[0] && numMax.Value <= dbList[1])
                    {
                        MessageDialog.ShowPromptMessage("该范围内的数据在数据库中有重叠,请更改范围!");
                        return false;
                    }

                    if (numMin.Value >= dbList[0] && numMin.Value <= dbList[1])
                    {
                        MessageDialog.ShowPromptMessage("该范围内的数据在数据库中有重叠,请更改范围!");
                        return false;
                    }

                    if (numMin.Value < dbList[0] && numMax.Value >= dbList[0] && numMax.Value <= dbList[1])
                    {
                        MessageDialog.ShowPromptMessage("该范围内的数据在数据库中有重叠,请更改范围!");
                        return false;
                    }

                    if (numMin.Value < dbList[0] && numMax.Value >= dbList[1])
                    {
                        MessageDialog.ShowPromptMessage("该范围内的数据在数据库中有重叠,请更改范围!");
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// 添加/修改选配表中零件信息记录
        /// </summary>
        /// <param name="strID">序号</param>
        /// <returns>返回是否成功更新零件信息</returns>
        bool UpdataAccessoryChoseConfectInfo(string strID)
        {
            string id = strID;
            string accessoryCode = "";

            if (txtCode.Text.Trim().Length > 0)
            {
                accessoryCode = txtCode.Text;
            }

            if (accessoryCode == "")
            {
                MessageDialog.ShowPromptMessage("零部件编码不能为空!");
                return false;
            }

            double minData = Convert.ToDouble(numMin.Value.ToString());
            double maxData = Convert.ToDouble(numMax.Value.ToString());

            if (minData > maxData)
            {
                MessageDialog.ShowPromptMessage("选值低范围不能大于选值高范围!");
                return false;
            }

            string rangeData = minData.ToString() + "至" + maxData.ToString();
            string choseConfectData = txtChoseConfect.Text;

            if (txtChoseConfect.Text == "")
            {
                choseConfectData = null;
            }

            if (!m_choseConfectServer.UpdataAccessoryChoseConfectInfo(id, accessoryCode, rangeData, 
                cmbProductType.Text, choseConfectData, out m_err))
            {
                MessageDialog.ShowErrorMessage(m_err);
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
            int allDataGridViewRowsCount = dataGridView1.RowCount;
            int n = dataGridView1.SelectedRows.Count;

            if (n == 0)
            {
                MessageBox.Show("请选择需要删除的数据行!", "消息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else if (n == 1)
            {
                string id = dataGridView1.CurrentRow.Cells[3].Value.ToString();

                if (MessageBox.Show("您是否确定要删除零件选配信息?", "消息", MessageBoxButtons.YesNo, 
                    MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (m_choseConfectServer.DeleteAccessoryChoseConfectInfo(id, out m_err))
                    {
                        InitDataGridView1();
                    }
                    else
                    {
                        MessageDialog.ShowErrorMessage(m_err);
                        return;
                    }
                }
                else
                {
                    return;
                }
            }
            else if (n > 1)
            {
                string[] ids = new string[n];

                for (int i = 0; i < n; i++)
                {
                    ids[i] = dataGridView1.SelectedRows[i].Cells[3].Value.ToString();
                }

                if (MessageBox.Show("您是否确定要删除零件选配信息?", "消息", MessageBoxButtons.YesNo, 
                    MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    for (int i = 0; i < ids.Length; i++)
                    {
                        if (!m_choseConfectServer.DeleteAccessoryChoseConfectInfo(ids[i], out m_err))
                        {
                            MessageDialog.ShowErrorMessage(m_err);
                            InitDataGridView1();
                            return;
                        }
                    }
                }

                InitDataGridView1();
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (CheckUpdataChoseConfectInfo(txtCode.Text))
            {
                if (UpdataAccessoryChoseConfectInfo(dataGridView1.CurrentRow.Cells[3].Value.ToString()))
                {
                    InitDataGridView1();
                }
                else
                {
                    MessageDialog.ShowPromptMessage("修改零件选配信息失败!");
                }
            }
        }

        /// <summary>
        /// 检验零件选配信息
        /// </summary>
        /// <returns>返回是否允许修改零件选配信息</returns>
        bool CheckUpdataChoseConfectInfo(string accessoryCode)
        {
            if (dataGridView1.CurrentRow.Cells[2].Value.ToString() != txtChoseConfect.Text)
            {
                if (m_dicAccessoryChoseConfectInfo[accessoryCode].ContainsKey(txtChoseConfect.Text))
                {
                    MessageDialog.ShowPromptMessage("数据库中已存在相同选配的信息!");
                    return false;
                }
            }

            if (numMin.Value >= numMax.Value)
            {
                MessageDialog.ShowPromptMessage("范围顺序颠倒有误,请重新输入!");
                return false;
            }

            string oldKey = "-1";
            List<decimal> dbValue = new List<decimal>();
            bool delFalg = false;

            if (m_dicAccessoryChoseConfectInfo[accessoryCode].ContainsKey(dataGridView1.CurrentRow.Cells[2].Value.ToString()))
            {
                oldKey = dataGridView1.CurrentRow.Cells[2].Value.ToString();
                dbValue = m_dicAccessoryChoseConfectInfo[accessoryCode][dataGridView1.CurrentRow.Cells[2].Value.ToString()];
                m_dicAccessoryChoseConfectInfo[accessoryCode].Remove(dataGridView1.CurrentRow.Cells[2].Value.ToString());
                delFalg = true;
            }

            foreach (KeyValuePair<string, List<decimal>> var in m_dicAccessoryChoseConfectInfo[accessoryCode])
            {
                List<decimal> dbList = var.Value;

                if (numMin.Value >= dbList[0] && numMax.Value <= dbList[1])
                {
                    MessageDialog.ShowPromptMessage("该范围内的数据在数据库中有重叠,请更改范围!");

                    if (delFalg)
                    {
                        m_dicAccessoryChoseConfectInfo[accessoryCode].Add(oldKey, dbValue);
                    }

                    return false;
                }

                if (numMin.Value >= dbList[0] && numMin.Value <= dbList[1])
                {
                    MessageDialog.ShowPromptMessage("该范围内的数据在数据库中有重叠,请更改范围!");

                    if (delFalg)
                    {
                        m_dicAccessoryChoseConfectInfo[accessoryCode].Add(oldKey, dbValue);
                    }

                    return false;
                }

                if (numMin.Value < dbList[0] && numMax.Value >= dbList[0] && numMax.Value <= dbList[1])
                {
                    MessageDialog.ShowPromptMessage("该范围内的数据在数据库中有重叠,请更改范围!");

                    if (delFalg)
                    {
                        m_dicAccessoryChoseConfectInfo[accessoryCode].Add(oldKey, dbValue);
                    }

                    return false;
                }

                if (numMin.Value < dbList[0] && numMax.Value >= dbList[1])
                {
                    MessageDialog.ShowPromptMessage("该范围内的数据在数据库中有重叠,请更改范围!");

                    if (delFalg)
                    {
                        m_dicAccessoryChoseConfectInfo[accessoryCode].Add(oldKey, dbValue);
                    }

                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 选配表表头管理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnChoseControlManage_Click(object sender, EventArgs e)
        {
            m_formChoseConfectHeadManage = new FormChoseConfectHeadManage(txtCode);//, txtSpec
            m_formChoseConfectHeadManage.ShowDialog();
        }

        private void cmbProductType_SelectedIndexChanged(object sender, EventArgs e)
        {
            InitDataGridView1();
        }
    }
}

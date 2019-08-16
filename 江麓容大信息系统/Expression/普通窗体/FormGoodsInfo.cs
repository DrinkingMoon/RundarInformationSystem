/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  FormGoodsInfo.cs
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
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ServerModule;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using UniversalControlLibrary;

namespace Expression
{
    /// <summary>
    /// 货物信息界面类
    /// </summary>
    public partial class FormGoodsInfo : Form
    {
        /// <summary>
        /// 查找到的符合条件的库存信息
        /// </summary>
        DataTable m_returnTable = new DataTable();

         /// <summary>
        /// 物品名称
        /// </summary>
        TextBox m_txtName;

        /// <summary>
        /// 图号型号
        /// </summary>
        TextBox m_txtCode;

        /// <summary>
        /// 规格
        /// </summary>
        TextBox m_txtSpec;

        /// <summary>
        /// 供应商
        /// </summary>
        TextBox m_txtProvider;

        /// <summary>
        /// 批次号
        /// </summary>
        TextBox m_txtBatchNo;

        /// <summary>
        /// 仓库类型(产品库与非产品库)
        /// </summary>
        string m_depotType;

        /// <summary>
        /// 是否显示供应商、批次等所有项
        /// </summary>
        string m_showAllFlag;

        /// <summary>
        /// 查找字段数组
        /// </summary>
        string[] m_findItem = new string[] { "图号型号", "规格", "物品名称"};

        /// <summary>
        /// 批次号是否显示标志
        /// </summary>
        bool m_batchCodeShowFlag = false;

        public FormGoodsInfo(TextBox textCode, TextBox textSpec, TextBox textName, TextBox textProvider, 
            TextBox textBatchNo, string depotType, bool batchCodeShowFlag, string showAllFlag)
        {
            InitializeComponent();

            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw | 
                ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);

            UpdateStyles();
            
            m_txtCode = textCode;
            m_txtSpec = textSpec;
            m_txtName = textName;
            m_txtProvider = textProvider;
            m_txtBatchNo = textBatchNo;
            m_depotType = depotType;
            m_showAllFlag = showAllFlag;
            m_batchCodeShowFlag = batchCodeShowFlag;
        }

        /// <summary>
        /// 改变组件大小
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormGoodsInfo_Resize(object sender, EventArgs e)
        {
            labelTitle.Location = new Point((this.Width - labelTitle.Width) / 2, labelTitle.Location.Y);

            if (dataGridView1.Columns.Count != 0)
            {
                dataGridView1.Columns[0].Visible = false;
            }
        }

        /// <summary>
        /// 窗体载入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormGoodsInfo_Load(object sender, EventArgs e)
        {
            cmbFindItem.Items.AddRange(m_findItem);
            cmbFindItem.SelectedIndex = 0;

            RefreshDataGridView(m_returnTable);

            if (m_batchCodeShowFlag)
            {
                if (m_depotType == "产品" || m_depotType == "其他")
                {
                    if (dataGridView1.Rows.Count > 0)
                    {
                        dataGridView1.Columns[0].Visible = false;
                    }
                }
                else if (m_depotType == "零部件" || m_depotType == "全部")
                {
                    if (dataGridView1.Rows.Count > 0)
                    {
                        dataGridView1.Columns[0].Visible = true;
                    }
                }
            }
        }

        /// <summary>
        /// 刷新DataGridView
        /// </summary>
        /// <param name="findStore">数据集</param>
        void RefreshDataGridView(DataTable findStore)
        {
            if (findStore != null)
            {
                DataTable table = findStore.Copy();
                dataGridView1.DataSource = table;

                ColumnWidthControl.SetDataGridView(this.Text, dataGridView1);
                dataGridView1.Refresh();
            }
        }

        /// <summary>
        /// 选定货物
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuItemSelect_Click(object sender, EventArgs e)
        {
            int codeIndex = 0;

            for (int i = 0; i < dataGridView1.Columns.Count; i++)
            {
                if (dataGridView1.Columns[i].Name=="图号型号")
                {
                    codeIndex = i;
                    break;
                }
            }

            if (m_txtCode != null)
            {
                m_txtCode.Text = "";

                if (dataGridView1.CurrentRow.Cells[codeIndex].Value != null)
                {
                    m_txtCode.Text = dataGridView1.CurrentRow.Cells[codeIndex].Value.ToString();
                }
            }

            if (m_txtSpec != null)
            {
                m_txtSpec.Text = "";

                if (dataGridView1.CurrentRow.Cells[codeIndex + 1].Value != null)
                {
                    m_txtSpec.Text = dataGridView1.CurrentRow.Cells[codeIndex + 1].Value.ToString();
                }
            }

            if (m_txtName != null)
            {
                m_txtName.Text = "";

                if (dataGridView1.CurrentRow.Cells[codeIndex + 2].Value != null)
                {
                    m_txtName.Text = dataGridView1.CurrentRow.Cells[codeIndex + 2].Value.ToString();
                }
            }

            if (m_txtProvider != null)
            {
                m_txtProvider.Text = "";

                if (dataGridView1.CurrentRow.Cells[codeIndex + 3].Value != null)
                {
                    m_txtProvider.Text = dataGridView1.CurrentRow.Cells[codeIndex + 3].Value.ToString();
                }
            }

            if (m_txtBatchNo != null)
            {
                m_txtBatchNo.Text = "";

                if (dataGridView1.CurrentRow.Cells[codeIndex + 4].Value != null)
                {
                    m_txtBatchNo.Text = dataGridView1.CurrentRow.Cells[codeIndex + 4].Value.ToString();
                }
            }

            this.Close();
        }

        private void dataGridView1_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
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

            if (e.RowIndex < 0 || e.ColumnIndex < 0)
            {
                return;
            }

            dataGridView1.ClearSelection(); 
            dataGridView1.CurrentCell = dataGridView1.Rows[e.RowIndex].Cells[strColName];
        }

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            menuItemSelect.PerformClick();
        }

        /// <summary>
        /// 查找内容发生变化时进行模糊查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtContext_TextChanged(object sender, EventArgs e)
        {
            dataGridView1.DataSource = FuzzyFindDataTableRecord.FindRecord(m_returnTable, cmbFindItem.Text, txtContext.Text);
            dataGridView1.Refresh();
        }
    }
}

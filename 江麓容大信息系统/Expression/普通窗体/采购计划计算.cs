using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ServerModule;
using PlatformManagement;
using GlobalObject;
using UniversalControlLibrary;

namespace Expression
{
    public partial class 采购计划计算 : Form
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        string m_strError;

        /// <summary>
        /// 采购计划服务组件
        /// </summary>
        IPurcharsingPlan m_serverPurcharsing = ServerModuleFactory.GetServerModule<IPurcharsingPlan>();

        /// <summary>
        /// 列的集合
        /// </summary>
        DataGridViewColumnCollection m_dataGirdViewColumns;

        /// <summary>
        /// 数据集
        /// </summary>
        CG_ProcurementSteps m_lnqProcurementSteps = new CG_ProcurementSteps();

        public 采购计划计算(DataGridViewColumnCollection dataGirdViewColumns)
        {
            InitializeComponent();
            m_dataGirdViewColumns = dataGirdViewColumns;
            RefreshData();
        }

        /// <summary>
        /// 刷新数据
        /// </summary>
        void RefreshData()
        {
            dataGridView1.DataSource = m_serverPurcharsing.GetMathSteps();
            dataGridView1.Columns["步骤ID"].Visible = false;
            dataGridView1.Columns["步骤名称"].Width = 300;
        }

        /// <summary>
        /// 获得信息
        /// </summary>
        void GetMessage()
        {
            m_lnqProcurementSteps = new CG_ProcurementSteps();

            m_lnqProcurementSteps.StepsID = numSteps.Tag == null ? 0 : Convert.ToInt32(numSteps.Tag);
            m_lnqProcurementSteps.CalculationSteps = numSteps.Value;
            m_lnqProcurementSteps.Remark = txtRemark.Text;
            m_lnqProcurementSteps.StepsName = txtStepsName.Text;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            GetMessage();

            if (!m_serverPurcharsing.OperatorMathSteps(m_lnqProcurementSteps,CE_OperatorMode.添加,out m_strError))
            {
                MessageDialog.ShowPromptMessage(m_strError);
                return;
            }
            else
            {
                MessageDialog.ShowPromptMessage("添加成功");
            }

            RefreshData();
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            GetMessage();

            if (!m_serverPurcharsing.OperatorMathSteps(m_lnqProcurementSteps,CE_OperatorMode.修改, out m_strError))
            {
                MessageDialog.ShowPromptMessage(m_strError);
                return;
            }
            else
            {
                MessageDialog.ShowPromptMessage("修改成功");
            }

            RefreshData();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            GetMessage();

            if (!m_serverPurcharsing.OperatorMathSteps(m_lnqProcurementSteps,  CE_OperatorMode.删除, out m_strError))
            {
                MessageDialog.ShowPromptMessage(m_strError);
                return;
            }
            else
            {
                MessageDialog.ShowPromptMessage("删除成功");
            }

            RefreshData();
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                return;
            }
            else
            {
                numSteps.Tag = dataGridView1.CurrentRow.Cells["步骤ID"].Value;
                numSteps.Value = Convert.ToDecimal(dataGridView1.CurrentRow.Cells["计算顺序"].Value);
                txtStepsName.Text = dataGridView1.CurrentRow.Cells["步骤名称"].Value.ToString();
                txtRemark.Text = dataGridView1.CurrentRow.Cells["备注"].Value.ToString();
            }
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            GetMessage();

            采购计划公式编辑 form = new 采购计划公式编辑(m_dataGirdViewColumns, m_lnqProcurementSteps);

            form.Show();
        }
    }
}

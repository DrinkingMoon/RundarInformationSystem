using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ServerModule;
using GlobalObject;
using PlatformManagement;
using System.Text.RegularExpressions;
using System.ServiceProcess;
using UniversalControlLibrary;

namespace Expression
{
    /// <summary>
    /// 外部库存查询与修改界面
    /// </summary>
    public partial class 外部库存查询与修改 : Form
    {
        /// <summary>
        /// 营销服务组件接口
        /// </summary>
        ISellIn m_serverSellIn = ServerModuleFactory.GetServerModule<ISellIn>();

        /// <summary>
        /// 初始化数据集
        /// </summary>
        S_OutStock m_lnqOutStock = new S_OutStock();

        /// <summary>
        /// 错误信息
        /// </summary>
        string m_strErr;

        public 外部库存查询与修改(PlatformManagement.FunctionTreeNodeInfo nodeInfo)
        {
            InitializeComponent();

            //this.testLable1.Text = "跑马灯！csdn baihe_591,文字改变时,重新显示,文字改变时,重新显示,文字改变时,重新显示,文字改变时,重新显示!";

            string a = "跑马灯！csdn baihe_591,文字改变时,重新显示,文字改变时,重新显示,文字改变时,重新显示,文字改变时,重新显示!";

            string b = "";

            for (int i = 0; i < a.Length; i++)
            {
                string k = a[i].ToString() + "\n";

                b = b + k;
            }

            //testLable1.Text = b;

            //testLable1.Run();

            DataTable dtTemp = UniversalFunction.GetStorageTb();

            for (int i = 0; i < dtTemp.Rows.Count; i++)
            {
                if (dtTemp.Rows[i]["StorageID"].ToString() == "02"
                    || dtTemp.Rows[i]["StorageID"].ToString() == "05")
                {
                    cmbStorage.Items.Add(dtTemp.Rows[i]["StorageName"].ToString());
                }

            }

            cmbStorage.Text = "";

            txtCode.OnCompleteSearch += new GlobalObject.DelegateCollection.NonArgumentHandle(txtCode_OnCompleteSearch);

            dataGridView1.DataSource = m_serverSellIn.GetOutStockInfo();
        }

        /// <summary>
        /// 数据集获得数据
        /// </summary>
        void GetMessage()
        {
            m_lnqOutStock.GoodsID = Convert.ToInt32(txtCode.Tag);
            m_lnqOutStock.StorageID = UniversalFunction.GetStorageID(cmbStorage.Text);
            m_lnqOutStock.Stock = nudStockCount.Value;
        }

        /// <summary>
        /// 检查数据
        /// </summary>
        /// <returns>通过返回True，否则False</returns>
        bool CheckDate()
        {
            if (txtCode.Tag == null || txtCode.Tag.ToString() == "")
            {
                MessageDialog.ShowPromptMessage("请选择需要操作的物品记录");
                return false;
            }

            if (cmbStorage.Text == "")
            {
                MessageDialog.ShowPromptMessage("请选择对应的库房");
                return false;
            }

            return true;
        }

        void txtCode_OnCompleteSearch()
        {
            txtCode.Text = txtCode.DataResult["图号型号"].ToString();
            txtName.Text = txtCode.DataResult["物品名称"].ToString();
            txtSpce.Text = txtCode.DataResult["规格"].ToString();
            lbUnit.Text = txtCode.DataResult["单位"].ToString();
            txtCode.Tag = Convert.ToInt32(txtCode.DataResult["序号"]);
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Rows.Count > 0)
            {
                txtCode.Enabled = false;
                cmbStorage.Enabled = false;
                txtName.Text = dataGridView1.CurrentRow.Cells["物品名称"].Value.ToString();
                txtCode.Tag = Convert.ToInt32(dataGridView1.CurrentRow.Cells["物品ID"].Value);
                txtCode.Text = dataGridView1.CurrentRow.Cells["图号型号"].Value.ToString();
                txtSpce.Text = dataGridView1.CurrentRow.Cells["规格"].Value.ToString();
                cmbStorage.Text = dataGridView1.CurrentRow.Cells["库房名称"].Value.ToString();
                nudStockCount.Value = Convert.ToDecimal(dataGridView1.CurrentRow.Cells["外部库存数"].Value);
                lbUnit.Text = dataGridView1.CurrentRow.Cells["单位"].Value.ToString();
            }
        }

        private void tlsbNew_Click(object sender, EventArgs e)
        {
            ClearDate();
            txtCode.Enabled = true;
            cmbStorage.Enabled = true;
        }

        /// <summary>
        /// 初始化数据
        /// </summary>
        void ClearDate()
        {
            txtCode.Enabled = false;
            cmbStorage.Enabled = false;
            txtCode.Text = "";
            txtName.Text = "";
            txtSpce.Text = "";
            cmbStorage.Text = "";
            nudStockCount.Value = 0;
            txtCode.Tag = null;
        }

        private void tlsbAdd_Click(object sender, EventArgs e)
        {
            if (!CheckDate())
            {
                return;
            }

            GetMessage();

            if (!m_serverSellIn.AddOutStockInfo(m_lnqOutStock,out m_strErr))
            {
                MessageDialog.ShowPromptMessage(m_strErr);
            }
            else
            {
                MessageDialog.ShowPromptMessage("添加成功");
            }

            ClearDate();
            dataGridView1.DataSource = m_serverSellIn.GetOutStockInfo();
        }

        private void tlsbRefresh_Click(object sender, EventArgs e)
        {
            DateTime dt = checkBillDateAndStatus1.dtpStartTime.Value;
            dt = checkBillDateAndStatus1.dtpEndTime.Value;

            string str = checkBillDateAndStatus1.cmbBillStatus.Text;

            dataGridView1.DataSource = m_serverSellIn.GetOutStockInfo();
        }

        private void tlsbUpdate_Click(object sender, EventArgs e)
        {
            if (!CheckDate())
            {
                return;
            }

            GetMessage();

            if (!m_serverSellIn.UpdateOutStockInfo(m_lnqOutStock, out m_strErr))
            {
                MessageDialog.ShowPromptMessage(m_strErr);
            }
            else
            {
                MessageDialog.ShowPromptMessage("修改成功");
            }

            ClearDate();
            dataGridView1.DataSource = m_serverSellIn.GetOutStockInfo();
        }

        private void tlsbDelete_Click(object sender, EventArgs e)
        {
            GetMessage();

            if (!m_serverSellIn.DeleteOutStockInfo(m_lnqOutStock, out m_strErr))
            {
                MessageDialog.ShowPromptMessage(m_strErr);
            }
            else
            {
                MessageDialog.ShowPromptMessage("删除成功");
            }

            ClearDate();
            dataGridView1.DataSource = m_serverSellIn.GetOutStockInfo();
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            外部库存变更查询 frmOutStockCheck = new 外部库存变更查询(Convert.ToInt32( dataGridView1.CurrentRow.Cells["物品ID"].Value),
                dataGridView1.CurrentRow.Cells["库房代码"].Value.ToString());
            frmOutStockCheck.ShowDialog();
        }

        private void 外部库存查询与修改_Load(object sender, EventArgs e)
        {
            string[] strBillStatus = { "全部", "新建单据", "等待主管审核", "等待出库" };
            checkBillDateAndStatus1.InsertComBox(strBillStatus);
            checkBillDateAndStatus1.OnCompleteSearch += 
                new GlobalObject.DelegateCollection.NonArgumentHandle(checkBillDateAndStatus1_OnCompleteSearch);

            #region 被要求使用服务器时间 Modify by cjb on 2012.6.15
            checkBillDateAndStatus1.dtpStartTime.Value = ServerTime.Time.AddDays(1).AddMonths(-1);
            checkBillDateAndStatus1.dtpEndTime.Value = ServerTime.Time.AddDays(1);

            #endregion
        }

        void checkBillDateAndStatus1_OnCompleteSearch()
        {
            DateTime dt = checkBillDateAndStatus1.dtpStartTime.Value;
            dt = checkBillDateAndStatus1.dtpEndTime.Value;

            string str = checkBillDateAndStatus1.cmbBillStatus.Text;
            dataGridView1.DataSource = m_serverSellIn.GetOutStockInfo();
        }
    }
}

/******************************************************************************
 *
 * 文件名称:  设置防错信息.cs
 * 作者    :  邱瑶       日期: 2014/03/20
 * 开发平台:  vs2008(c#)
 * 用于    :  装配线管理信息系统
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
using UniversalControlLibrary;
using GlobalObject;

namespace Expression
{
    public partial class 设置防错信息 : Form
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        string m_error;

        /// <summary>
        /// 服务组件
        /// </summary>
        IWorkbenchService m_workbenchServer = ServerModuleFactory.GetServerModule<IWorkbenchService>();

        /// <summary>
        /// 装配BOM 服务组件
        /// </summary>
        IAssemblingBom m_assemblingBom = ServerModuleFactory.GetServerModule<IAssemblingBom>();

        /// <summary>
        /// 防错服务组件
        /// </summary>
        IPreventErrorServer m_preventErrorServer = PMS_ServerFactory.GetServerModule<IPreventErrorServer>();

        /// <summary>
        /// 服务组件
        /// </summary>
        IProductInfoServer m_productInfoServer = ServerModuleFactory.GetServerModule<IProductInfoServer>();

        /// <summary>
        /// 产品信息
        /// </summary>
        List<View_P_ProductInfo> m_productInfo;

        /// <summary>
        /// 产品类型编码
        /// </summary>
        string m_productCode;

        /// <summary>
        /// 装配Bom的数据集
        /// </summary>
        List<View_P_AssemblingBom> m_assemble;

        /// <summary>
        /// 记录操作日志服务类
        /// </summary>
        ISystemLogServer m_sysLogServer = ServerModuleFactory.GetServerModule<ISystemLogServer>();

        public 设置防错信息()
        {
            InitializeComponent();

            #region 获取工位
            IQueryable<View_P_Workbench> workbench = m_workbenchServer.Workbenchs;

            if (workbench.Count() > 0)
            {
                cmbWorkBench.Items.AddRange((from r in workbench select r.工位).ToArray());
                cmbEWorkBench.Items.AddRange((from r in workbench select r.工位).ToArray());
                cmbCCDBench.Items.AddRange((from r in workbench select r.工位).ToArray());
                cmbCurrentBench.Items.AddRange((from r in workbench select r.工位).ToArray());
                cmbUpBench.Items.AddRange((from r in workbench select r.工位).ToArray());
                cmbUpBench.Items.Add("");
                cmbWorkBench.Items.Add("");
            }
            else
            {
                添加toolStripButton.Enabled = false;
                修改toolStripButton.Enabled = false;

                MessageDialog.ShowErrorMessage("没有获取到工位信息");
                return;
            }

            #endregion
           
            RefreshDataGridView();
            RefrshdgvElectronic();
            RefrshAssembleOrder();
            RefrshCCD();
        }

        private void 设置防错信息_Resize(object sender, EventArgs e)
        {
            labelTitle.Location = new Point((this.Width - labelTitle.Width) / 2, labelTitle.Location.Y);
            lbElectronTitle.Location = new Point((this.Width - lbElectronTitle.Width) / 2, lbElectronTitle.Location.Y);
            lbOrderTitle.Location = new Point((this.Width - lbOrderTitle.Width) / 2, lbOrderTitle.Location.Y);
            lbCCDTitle.Location = new Point((this.Width - lbCCDTitle.Width) / 2, lbCCDTitle.Location.Y);
        }

        /// <summary>
        /// 打扭矩防错刷新
        /// </summary>
        public void RefreshDataGridView()
        {
            DataTable dt = m_preventErrorServer.GetAllTorqueSpannerInfo();

            dataGridView1.DataSource = dt;

            if (dt != null && dt.Rows.Count > 0)
            {
                dataGridView1.Columns["序号"].Visible = false;

                this.dataGridView1.ColumnWidthChanged -= new System.Windows.Forms.DataGridViewColumnEventHandler(
                   this.dataGridView1_ColumnWidthChanged);

                ColumnWidthControl.SetDataGridView(labelTitle.Text, dataGridView1);

                this.dataGridView1.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(
                    this.dataGridView1_ColumnWidthChanged);

                userControlDataLocalizer1.Init(dataGridView1, this.Name,
                   UniversalFunction.SelectHideFields(this.Name, dataGridView1.Name, BasicInfo.LoginID));

                dataGridView1.Refresh();
            }
        }

        private void btnFindProduct_Click(object sender, EventArgs e)
        {
            FormProductType form = new FormProductType();

            if (dataGridView1.Rows.Count > 0)
            {
                List<View_P_ProductInfo> list = new List<View_P_ProductInfo>();

                string[] productType = txtProductType.Text.Split(',');

                foreach (string item in productType)
                {
                    View_P_ProductInfo product = new View_P_ProductInfo();

                    product.产品类型编码 = item;
                    list.Add(product);
                }

                form.SelectedProduct = list;
            }

            string productStr = "";

            if (form.ShowDialog() == DialogResult.OK)
            {
                List<View_P_ProductInfo> productList = form.SelectedProduct;

                if (form.SelectedProduct.Count != form.ProductCount)
                {
                    foreach (View_P_ProductInfo item in productList)
                    {
                        productStr += item.产品类型编码 + ",";
                    }

                    productStr = productStr.Substring(0, productStr.Length - 1);
                }
                else
                {
                    productStr = "全部";
                }
            }

            txtProductType.Text = productStr;
        }

        private void cbOtherWorkBenchPart_CheckedChanged(object sender, EventArgs e)
        {
            if (cbOtherWorkBenchPart.Checked)
            {
                lbGoodsWorkBench.Visible = true;
                cmbGoodsWorkBench.Visible = true;

                IQueryable<View_P_Workbench> workbench = m_workbenchServer.Workbenchs;

                if (workbench.Count() > 0)
                {
                    cmbGoodsWorkBench.Items.AddRange((from r in workbench select r.工位).ToArray());
                }
            }
            else
            {
                lbGoodsWorkBench.Visible = false;
                cmbGoodsWorkBench.Visible = false;
            }
        }

        /// <summary>
        /// 打扭矩防错--查找零件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGoodsInfo_Click(object sender, EventArgs e)
        {
            string workBenchPart = "";

            if (cbOtherWorkBenchPart.Checked)
            {
                workBenchPart = cmbGoodsWorkBench.Text.Trim();
            }
            else
            {
                workBenchPart = cmbWorkBench.Text.Trim();
            }

            if (workBenchPart != "")
            {
                FormQueryInfo form = QueryInfoDialog.GetAccessoryWorkbench(workBenchPart);

                if (form == null || form.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                txtGoodsCode.Text = form.GetDataItem("图号型号").ToString();
                txtGoodsName.Text = form.GetDataItem("物品名称").ToString();
                txtSpec.Text = form.GetDataItem("规格").ToString();
            }
            else
            {
                MessageDialog.ShowPromptMessage("请选择工位！");
                return;
            }
        }

        /// <summary>
        /// 检查控件
        /// </summary>
        /// <returns></returns>
        bool CheckControl()
        {
            if (cmbCommMode_Torque.Text.Trim().Length == 0)
            {
                MessageDialog.ShowPromptMessage("请选择防错模式！");
                return false;
            }

            if (cmbWorkBench.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("请选择打扭矩的工位！");
                return false;
            }

            if (cbOtherWorkBenchPart.Checked && cmbGoodsWorkBench.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("请选择物品所在的装配工位！");
                return false;
            }

            if (txtGoodsCode.Text.Trim() == "" && txtGoodsName.Text.Trim() == "" && txtSpec.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("请选择物品！");
                return false;
            }

            if (numSeconds.Value <= 0)
            {
                MessageDialog.ShowPromptMessage("请设置打一个物品的扭矩所需要的秒数！");
                return false;
            }

            return true;
        }

        /// <summary>
        /// 打扭矩添加
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 添加toolStripButton_Click(object sender, EventArgs e)
        {
            if (!CheckControl())
            {
                return;
            }

            if (cbOtherWorkBenchPart.Checked)
            {
                if (!m_preventErrorServer.IsWorkBenchGoods(cmbGoodsWorkBench.Text, txtGoodsCode.Text, txtGoodsName.Text, txtSpec.Text))
                {
                    MessageDialog.ShowPromptMessage("装配工位上没有选择的零件信息，请重新选择！");
                    return;
                }
            }
            else
            {
                if (!m_preventErrorServer.IsWorkBenchGoods(cmbWorkBench.Text, txtGoodsCode.Text, txtGoodsName.Text, txtSpec.Text))
                {
                    MessageDialog.ShowPromptMessage("打扭矩工位上没有选择的零件信息，请重新选择！");
                    return;
                }
            }

            ZPX_TorqueSpanner torqueSpanner = new ZPX_TorqueSpanner();

            torqueSpanner.CommPort = (int)numCommPort.Value;
            torqueSpanner.GoodsCode = txtGoodsCode.Text;
            torqueSpanner.GoodsName = txtGoodsName.Text;
            torqueSpanner.Spec = txtSpec.Text;
            torqueSpanner.GoodsWorkBench = cmbGoodsWorkBench.Text;
            torqueSpanner.IsOtherWorkBenchPart = cbOtherWorkBenchPart.Checked;
            torqueSpanner.ProductType = txtProductType.Text == "全部" ? "" : txtProductType.Text;
            torqueSpanner.Seconds = (int)numSeconds.Value;
            torqueSpanner.WorkBench = cmbWorkBench.Text;
            torqueSpanner.CommMode = cmbCommMode_Torque.Text;
            torqueSpanner.TorqueCount = Convert.ToInt32( numTorqueCount.Value);

            if (!m_preventErrorServer.InsertTorqueSpanner(torqueSpanner, out m_error))
            {
                MessageDialog.ShowPromptMessage(m_error);
            }
            else
            {
                MessageDialog.ShowPromptMessage("添加成功！");
            }

            m_sysLogServer.RecordLog<ZPX_TorqueSpanner>(CE_OperatorMode.添加, torqueSpanner, torqueSpanner);
            RefreshDataGridView();

            cmbWorkBench.Enabled = false;
            btnGoodsInfo.Enabled = false;
        }

        private void 新建toolStripButton_Click(object sender, EventArgs e)
        {
            cmbWorkBench.Enabled = true;
            btnGoodsInfo.Enabled = true;

            cbOtherWorkBenchPart.Checked = false;
            lbGoodsWorkBench.Visible = false;
            cmbGoodsWorkBench.Visible = false;

            cmbWorkBench.SelectedIndex = -1;
            cmbGoodsWorkBench.SelectedIndex = -1;

            cmbCommMode_Torque.SelectedIndex = -1;
            numTorqueCount.Value = 0;

            txtGoodsCode.Text = "";
            txtGoodsName.Text = "";
            txtSpec.Text = "";
        }

        /// <summary>
        /// 打扭矩防错dataGridView的焦点事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            txtGoodsCode.Text = dataGridView1.CurrentRow.Cells["图号型号"].Value.ToString();
            txtGoodsName.Text = dataGridView1.CurrentRow.Cells["物品名称"].Value.ToString();
            txtSpec.Text = dataGridView1.CurrentRow.Cells["规格"].Value.ToString();
            txtProductType.Text = dataGridView1.CurrentRow.Cells["产品型号"].Value.ToString();
            numCommPort.Value = Convert.ToInt32(dataGridView1.CurrentRow.Cells["端口号"].Value);
            numSeconds.Value = Convert.ToInt32(dataGridView1.CurrentRow.Cells["所需秒数"].Value);
            cmbWorkBench.Text = dataGridView1.CurrentRow.Cells["打扭矩工位"].Value.ToString();
            cmbCommMode_Torque.Text = dataGridView1.CurrentRow.Cells["通信模式"].Value.ToString();
            numTorqueCount.Value = Convert.ToDecimal(dataGridView1.CurrentRow.Cells["扭矩次数"].Value);

            cbOtherWorkBenchPart.Checked = Convert.ToBoolean(
                dataGridView1.CurrentRow.Cells["是否是其他装配工位的零件"].Value.ToString() == "否" ? false : true);

            if (cbOtherWorkBenchPart.Checked)
            {
                lbGoodsWorkBench.Visible = true;
                cmbGoodsWorkBench.Visible = true;

                cmbGoodsWorkBench.Text = dataGridView1.CurrentRow.Cells["所在的装配工位"].Value.ToString();
            }
            else
            {
                lbGoodsWorkBench.Visible = false;
                cmbGoodsWorkBench.Visible = false;
            }

            DateTime time = ServerTime.Time;
            dtpStarTime.Value = dataGridView1.CurrentRow.Cells["屏蔽开始时间"].Value == DBNull.Value ? time : 
                Convert.ToDateTime(dataGridView1.CurrentRow.Cells["屏蔽开始时间"].Value);
            dtpEndTime.Value = dataGridView1.CurrentRow.Cells["屏蔽终止时间"].Value == DBNull.Value ? time :
                Convert.ToDateTime(dataGridView1.CurrentRow.Cells["屏蔽终止时间"].Value);

        }

        private void dataGridView1_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            ColumnWidthControl.SaveColumnParams(labelTitle.Text, e.Column);
        }

        private void 刷新toolStripButton_Click(object sender, EventArgs e)
        {
            RefreshDataGridView();
        }

        private void 删除toolStripButton_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 1)
            {
                if (MessageDialog.ShowEnquiryMessage("确定删除选中的打扭矩信息吗？") == DialogResult.Yes)
                {
                    ZPX_TorqueSpanner oldTorqueSpanner = new ZPX_TorqueSpanner();

                    DataGridViewRow row = dataGridView1.CurrentRow;

                    oldTorqueSpanner.CommPort = Convert.ToInt32(row.Cells["端口号"].Value);
                    oldTorqueSpanner.GoodsCode = row.Cells["图号型号"].Value.ToString();
                    oldTorqueSpanner.GoodsName = row.Cells["物品名称"].Value.ToString();
                    oldTorqueSpanner.Spec = row.Cells["规格"].Value.ToString();
                    oldTorqueSpanner.GoodsWorkBench = row.Cells["所在的装配工位"].Value.ToString();
                    oldTorqueSpanner.IsOtherWorkBenchPart = Convert.ToBoolean(row.Cells["是否是其他装配工位的零件"].Value.ToString() == "是" ? "true" : "false");
                    oldTorqueSpanner.ProductType = row.Cells["产品型号"].Value.ToString();
                    oldTorqueSpanner.Seconds = Convert.ToInt32(row.Cells["所需秒数"].Value);
                    oldTorqueSpanner.WorkBench = row.Cells["打扭矩工位"].Value.ToString();

                    if (!m_preventErrorServer.DeleteTorqueSpanner(
                        Convert.ToInt32(dataGridView1.CurrentRow.Cells["序号"].Value), out m_error))
                    {
                        MessageDialog.ShowPromptMessage(m_error);
                        return;
                    }
                    else
                    {
                        MessageDialog.ShowPromptMessage("删除成功！");
                        m_sysLogServer.RecordLog<ZPX_TorqueSpanner>(CE_OperatorMode.删除, oldTorqueSpanner,oldTorqueSpanner);
                        RefreshDataGridView();
                    }
                }
            }
            else
            {
                MessageDialog.ShowPromptMessage("请选择一行再操作！");
            }
        }

        /// <summary>
        /// 电子称防错刷新
        /// </summary>
        public void RefrshdgvElectronic()
        {
            DataTable dt = m_preventErrorServer.GetAllLeakproofPartsInfo();
            dgvElectronic.DataSource = dt;

            if (dt!=null && dt.Rows.Count>0)
            {
                dgvElectronic.Columns["ID"].Visible = false;

                this.dgvElectronic.ColumnWidthChanged -= new System.Windows.Forms.DataGridViewColumnEventHandler(
                   this.dgvElectronic_ColumnWidthChanged);

                ColumnWidthControl.SetDataGridView(lbElectronTitle.Text, dgvElectronic);

                this.dgvElectronic.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(
                    this.dgvElectronic_ColumnWidthChanged);

                ElectronUserControlDataLocalizer.Init(dgvElectronic, this.Name,
                   UniversalFunction.SelectHideFields(this.Name, dgvElectronic.Name, BasicInfo.LoginID));

                dgvElectronic.Refresh();
            }
        }

        /// <summary>
        /// 电子称防错的工位选择事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbEWorkBench_SelectedIndexChanged(object sender, EventArgs e)
        {
            //txtEGoodsCode.Text = "";
            //txtEGoodsName.Text = "";
            //txtESpec.Text = "";
        }

        private void 电子称新建toolStripButton_Click(object sender, EventArgs e)
        {
            txtEGoodsCode.Text = "";
            txtEGoodsName.Text = "";
            txtESpec.Text = "";

            numTolerance.Value = 0;
        }

        /// <summary>
        /// 电子称防错的查找零件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEFindGoods_Click(object sender, EventArgs e)
        {
            if (cmbEWorkBench.Text.Trim() != "")
            {
                FormQueryInfo form = QueryInfoDialog.GetAccessoryWorkbench(cmbEWorkBench.Text.Trim());

                if (form == null || form.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                txtEGoodsCode.Text = form.GetDataItem("图号型号").ToString();
                txtEGoodsName.Text = form.GetDataItem("物品名称").ToString();
                txtESpec.Text = form.GetDataItem("规格").ToString();
            }
            else
            {
                MessageDialog.ShowPromptMessage("请选择工位！");
                return;
            }
        }

        /// <summary>
        /// 电子称添加事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 电子称toolStripButton_Click(object sender, EventArgs e)
        {
            if (cmbEWorkBench.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("请选择工位！");
                return;
            }

            if (txtEGoodsCode.Text == "" && txtEGoodsName.Text == "" && txtESpec.Text == "")
            {
                MessageDialog.ShowPromptMessage("请选择零件！");
                return;
            }

            if (numECommPort.Value == 0)
            {
                MessageDialog.ShowPromptMessage("请选择端口号！");
                return;
            }

            if (cmbEPreventMode.SelectedIndex == -1)
            {
                MessageDialog.ShowPromptMessage("请选择防错模式！");
                return;
            }

            ZPX_LeakproofParts leakproofParts = new ZPX_LeakproofParts();

            leakproofParts.电子秤端口号 = (int)numECommPort.Value;
            leakproofParts.防错模式 = cmbEPreventMode.Text;
            leakproofParts.防漏装零件规格 = txtESpec.Text;
            leakproofParts.防漏装零件名称 = txtEGoodsName.Text;
            leakproofParts.防漏装零件图号 = txtEGoodsCode.Text;
            leakproofParts.工位 = cmbEWorkBench.Text;
            leakproofParts.零件单重 = Convert.ToDouble(numGoodsWeight.Value);
            leakproofParts.公差 = (double?)numTolerance.Value;

            if (!m_preventErrorServer.InsertLeakproofParts(leakproofParts, out m_error))
            {
                MessageDialog.ShowPromptMessage(m_error);
                return;
            }
            else
            {
                MessageDialog.ShowPromptMessage("电子称防错添加成功！");

                RefrshdgvElectronic();
                m_sysLogServer.RecordLog<ZPX_LeakproofParts>(CE_OperatorMode.添加, leakproofParts, leakproofParts);
                cmbWorkBench.Enabled = false;
                btnGoodsInfo.Enabled = false;
            }
        }

        private void dgvElectronic_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            ColumnWidthControl.SaveColumnParams(lbElectronTitle.Text, e.Column);
        }

        private void 电子称修改toolStripButton_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(dgvElectronic.CurrentRow.Cells["ID"].Value);

            ZPX_LeakproofParts oldLeakproofParts = new ZPX_LeakproofParts();

            DataGridViewRow row = dgvElectronic.CurrentRow;

            oldLeakproofParts.电子秤端口号 = Convert.ToInt32(row.Cells["电子秤端口号"].Value);
            oldLeakproofParts.防错模式 = row.Cells["防错模式"].Value.ToString();
            oldLeakproofParts.防漏装零件规格 = row.Cells["防漏装零件规格"].Value.ToString();
            oldLeakproofParts.防漏装零件名称 = row.Cells["防漏装零件名称"].Value.ToString();
            oldLeakproofParts.防漏装零件图号 = row.Cells["防漏装零件图号"].Value.ToString();
            oldLeakproofParts.工位 = row.Cells["工位"].Value.ToString();
            oldLeakproofParts.零件单重 = Convert.ToDouble(row.Cells["零件单重"].Value.ToString());

            ZPX_LeakproofParts leakproofParts = new ZPX_LeakproofParts();

            leakproofParts.电子秤端口号 = (int)numECommPort.Value;
            leakproofParts.防错模式 = cmbEPreventMode.Text;
            leakproofParts.防漏装零件规格 = txtESpec.Text;
            leakproofParts.防漏装零件名称 = txtEGoodsName.Text;
            leakproofParts.防漏装零件图号 = txtEGoodsCode.Text;
            leakproofParts.工位 = row.Cells["工位"].Value.ToString();
            leakproofParts.零件单重 = Convert.ToDouble(numGoodsWeight.Value);
            leakproofParts.公差 = (double?)numTolerance.Value;

            if (!m_preventErrorServer.UpdateLeakproofParts(leakproofParts, out m_error))
            {
                MessageDialog.ShowPromptMessage(m_error);
            }
            else
            {
                MessageDialog.ShowPromptMessage("修改成功！");
                m_sysLogServer.RecordLog<ZPX_LeakproofParts>(CE_OperatorMode.修改, leakproofParts, oldLeakproofParts);
            }

            RefrshdgvElectronic();

            if (Convert.ToInt32(id) > 0)
            {
                string strColName = "";

                foreach (DataGridViewColumn col in dgvElectronic.Columns)
                {
                    if (col.Visible)
                    {
                        strColName = col.Name;
                        break;
                    }
                }

                for (int i = 0; i < dgvElectronic.Rows.Count; i++)
                {
                    if (dgvElectronic.Rows[i].Cells["ID"].Value.ToString() == id.ToString())
                    {
                        dgvElectronic.FirstDisplayedScrollingRowIndex = i;
                        dgvElectronic.CurrentCell = dgvElectronic.Rows[i].Cells[strColName];
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 打扭矩修改事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 修改toolStripButton_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(dataGridView1.CurrentRow.Cells["序号"].Value);

            ZPX_TorqueSpanner oldTorqueSpanner = new ZPX_TorqueSpanner();

            DataGridViewRow row = dataGridView1.CurrentRow;

            oldTorqueSpanner.CommPort = Convert.ToInt32(row.Cells["端口号"].Value);
            oldTorqueSpanner.GoodsCode = row.Cells["图号型号"].Value.ToString();
            oldTorqueSpanner.GoodsName = row.Cells["物品名称"].Value.ToString();
            oldTorqueSpanner.Spec = row.Cells["规格"].Value.ToString();
            oldTorqueSpanner.GoodsWorkBench = row.Cells["所在的装配工位"].Value.ToString();
            oldTorqueSpanner.IsOtherWorkBenchPart = row.Cells["是否是其他装配工位的零件"].Value.ToString() == "是" ? true : false;
            oldTorqueSpanner.ProductType = row.Cells["产品型号"].Value.ToString();
            oldTorqueSpanner.Seconds = Convert.ToInt32(row.Cells["所需秒数"].Value);
            oldTorqueSpanner.WorkBench = row.Cells["打扭矩工位"].Value.ToString();
            oldTorqueSpanner.CommMode = row.Cells["通信模式"].Value.ToString();
            oldTorqueSpanner.TorqueCount = Convert.ToInt32(row.Cells["扭矩次数"].Value);

            ZPX_TorqueSpanner torqueSpanner = new ZPX_TorqueSpanner();

            torqueSpanner.CommPort = (int)numCommPort.Value;
            torqueSpanner.GoodsCode = txtGoodsCode.Text;
            torqueSpanner.GoodsName = txtGoodsName.Text;
            torqueSpanner.Spec = txtSpec.Text;
            torqueSpanner.GoodsWorkBench = cmbGoodsWorkBench.Text;
            torqueSpanner.IsOtherWorkBenchPart = cbOtherWorkBenchPart.Checked;
            torqueSpanner.ProductType = txtProductType.Text == "全部" ? "" : txtProductType.Text;
            torqueSpanner.Seconds = (int)numSeconds.Value;
            torqueSpanner.WorkBench = row.Cells["打扭矩工位"].Value.ToString();
            torqueSpanner.CommMode = cmbCommMode_Torque.Text;
            torqueSpanner.TorqueCount = Convert.ToInt32(numTorqueCount.Value);

            if (!m_preventErrorServer.UpdateTorqueSpanner(torqueSpanner, out m_error))
            {
                MessageDialog.ShowPromptMessage(m_error);
            }
            else
            {
                MessageDialog.ShowPromptMessage("修改成功！");
                m_sysLogServer.RecordLog<ZPX_TorqueSpanner>(CE_OperatorMode.修改, torqueSpanner, oldTorqueSpanner);
            }

            RefreshDataGridView();

            if (Convert.ToInt32(id) > 0)
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
                    if (dataGridView1.Rows[i].Cells["序号"].Value.ToString() == id.ToString())
                    {
                        dataGridView1.FirstDisplayedScrollingRowIndex = i;
                        dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells[strColName];
                        break;
                    }
                }
            }
        }

        private void 电子称删除toolStripButton_Click(object sender, EventArgs e)
        {
            if (dgvElectronic.SelectedRows.Count == 1)
            {
                if (MessageDialog.ShowEnquiryMessage("确定删除选中的电子称防错信息吗？") == DialogResult.Yes)
                {
                    ZPX_LeakproofParts oldLeakproofParts = new ZPX_LeakproofParts();

                    DataGridViewRow row = dgvElectronic.CurrentRow;

                    oldLeakproofParts.电子秤端口号 = Convert.ToInt32(row.Cells["电子秤端口号"].Value);
                    oldLeakproofParts.防错模式 = row.Cells["防错模式"].Value.ToString();
                    oldLeakproofParts.防漏装零件规格 = row.Cells["防漏装零件规格"].Value.ToString();
                    oldLeakproofParts.防漏装零件名称 = row.Cells["防漏装零件名称"].Value.ToString();
                    oldLeakproofParts.防漏装零件图号 = row.Cells["防漏装零件图号"].Value.ToString();
                    oldLeakproofParts.工位 = row.Cells["工位"].Value.ToString();
                    oldLeakproofParts.零件单重 = Convert.ToDouble(row.Cells["零件单重"].Value.ToString());

                    if (!m_preventErrorServer.DeleteLeakproofParts(
                        Convert.ToInt32(dgvElectronic.CurrentRow.Cells["ID"].Value), out m_error))
                    {
                        MessageDialog.ShowPromptMessage(m_error);
                        return;
                    }
                    else
                    {
                        MessageDialog.ShowPromptMessage("删除成功！");
                        m_sysLogServer.RecordLog<ZPX_LeakproofParts>(CE_OperatorMode.删除, oldLeakproofParts, oldLeakproofParts);
                        RefrshdgvElectronic();
                    }
                }
            }
            else
            {
                MessageDialog.ShowPromptMessage("请选择一行再操作！");
            }
        }

        private void 电子称刷新toolStripButton_Click(object sender, EventArgs e)
        {
            RefrshdgvElectronic();
        }

        /// <summary>
        /// 电子称防错的dataGridView的焦点事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvElectronic_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            txtEGoodsCode.Text = dgvElectronic.CurrentRow.Cells["防漏装零件图号"].Value.ToString();
            txtEGoodsName.Text = dgvElectronic.CurrentRow.Cells["防漏装零件名称"].Value.ToString();
            txtESpec.Text = dgvElectronic.CurrentRow.Cells["防漏装零件规格"].Value.ToString();
            numECommPort.Value = Convert.ToInt32(dgvElectronic.CurrentRow.Cells["电子秤端口号"].Value);
            numGoodsWeight.Value = Convert.ToDecimal(dgvElectronic.CurrentRow.Cells["零件单重"].Value);
            cmbEPreventMode.Text = dgvElectronic.CurrentRow.Cells["防错模式"].Value.ToString();
            cmbEWorkBench.Text = dgvElectronic.CurrentRow.Cells["工位"].Value.ToString();

            DateTime time = ServerTime.Time;
            dtpLeakStarTime.Value = dgvElectronic.CurrentRow.Cells["屏蔽开始时间"].Value == DBNull.Value ? time :
                Convert.ToDateTime(dgvElectronic.CurrentRow.Cells["屏蔽开始时间"].Value);
            dtpLeakEndTime.Value = dgvElectronic.CurrentRow.Cells["屏蔽终止时间"].Value == DBNull.Value ? time :
                Convert.ToDateTime(dgvElectronic.CurrentRow.Cells["屏蔽终止时间"].Value);
        }

        /// <summary>
        /// 装配顺序防错刷新
        /// </summary>
        public void RefrshAssembleOrder()
        {
            DataTable dt = m_preventErrorServer.GetAllWorkbenchConfig();

            dgvAssembleOrder.DataSource = dt;

            if (dt!=null && dt.Rows.Count>0)
            {
                this.dgvAssembleOrder.ColumnWidthChanged -= new System.Windows.Forms.DataGridViewColumnEventHandler(
                this.dgvAssembleOrder_ColumnWidthChanged);

                ColumnWidthControl.SetDataGridView(lbOrderTitle.Text, dgvAssembleOrder);

                this.dgvAssembleOrder.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(
                    this.dgvAssembleOrder_ColumnWidthChanged);

                AssembleUserControlDataLocalizer.Init(dgvAssembleOrder, this.Name,
                   UniversalFunction.SelectHideFields(this.Name, dgvAssembleOrder.Name, BasicInfo.LoginID));

                dgvAssembleOrder.Refresh();
            }
        }

        private void 顺序新建toolStripButton_Click(object sender, EventArgs e)
        {
            cmbCurrentBench.SelectedIndex = -1;
            cmbAssemblyName.SelectedIndex = -1;
            cmbUpBench.SelectedIndex = cmbUpBench.Items.Count - 1;
            txtOrderProductType.Text = "";
        }

        private void 顺序修改toolStripButton_Click(object sender, EventArgs e)
        {
            bool flag = false;

            DataGridViewRow row = dgvAssembleOrder.CurrentRow;

            ZPX_ProductWorkbenchConfig oldWorkBenchConfig = new ZPX_ProductWorkbenchConfig();

            oldWorkBenchConfig.按序装配端口号 = Convert.ToInt32(row.Cells["按序装配端口号"].Value);
            oldWorkBenchConfig.按序装配末道工序值 = Convert.ToInt32(row.Cells["按序装配末道工序值"].Value);
            oldWorkBenchConfig.当前工位 = row.Cells["当前工位"].Value.ToString();
            oldWorkBenchConfig.分总成名称 = row.Cells["分总成名称"].Value.ToString();
            oldWorkBenchConfig.上道工位 = row.Cells["上道工位"].Value.ToString();
            oldWorkBenchConfig.是按顺序流水号装配 = row.Cells["是按顺序流水号装配"].Value.ToString() == "是" ? true : false;
            oldWorkBenchConfig.是本阶段末工位 = row.Cells["是本阶段末工位"].Value.ToString() == "是" ? true : false;
            oldWorkBenchConfig.是本阶段起始工位 = row.Cells["是本阶段起始工位"].Value.ToString() == "是" ? true : false;
            oldWorkBenchConfig.是气密性检测工位 = row.Cells["是气密性检测工位"].Value.ToString() == "是" ? true : false;
            oldWorkBenchConfig.是所有产品通用工位 = row.Cells["是所有产品通用工位"].Value.ToString() == "是" ? true : false;
            oldWorkBenchConfig.产品类型 = row.Cells["产品类型"].Value.ToString();
            oldWorkBenchConfig.需测量总成名称 = row.Cells["产品类型"].Value.ToString();

            if (m_productInfo != null)
            {
                foreach (View_P_ProductInfo item in m_productInfo)
                {
                    string assemble = "";

                    ZPX_ProductWorkbenchConfig workBenchConfig = new ZPX_ProductWorkbenchConfig();

                    workBenchConfig.按序装配端口号 = (int)numOrderCommPort.Value;
                    workBenchConfig.按序装配末道工序值 = (int)numBenchValue.Value;
                    workBenchConfig.当前工位 = cmbCurrentBench.Text;
                    workBenchConfig.分总成名称 = cmbAssemblyName.Text;
                    workBenchConfig.上道工位 = cmbUpBench.Text;
                    workBenchConfig.是按顺序流水号装配 = IsOrder.Text == "是" ? true : false;
                    workBenchConfig.是本阶段末工位 = IsEndBench.Text == "是" ? true : false;
                    workBenchConfig.是本阶段起始工位 = IsStartBench.Text == "是" ? true : false;
                    workBenchConfig.是气密性检测工位 = IsTightness.Text == "是" ? true : false;
                    workBenchConfig.是所有产品通用工位 = IsCommon.Text == "是" ? true : false;
                    workBenchConfig.产品类型 = item.产品类型编码;

                    if (m_assemble != null)
                    {
                        foreach (View_P_AssemblingBom itemAssemble in m_assemble)
                        {
                            if (m_preventErrorServer.IsAssemblingBom(itemAssemble.父总成名称, item.产品类型编码))
                            {
                                assemble += itemAssemble.父总成名称 + ",";
                            }
                        }

                        if (assemble != "")
                        {
                            workBenchConfig.需测量总成名称 = assemble.Substring(0, assemble.Length - 1);
                        }
                        else
                        {
                            workBenchConfig.需测量总成名称 = "";
                        }
                    }
                    else
                    {
                        workBenchConfig.需测量总成名称 = txtMeasure.Text;
                    }

                    if (!m_preventErrorServer.UpdateWorkbenchConfig(workBenchConfig, out m_error))
                    {
                        flag = true;
                        m_error += " " + m_error;
                        continue;
                    }

                    m_sysLogServer.RecordLog<ZPX_ProductWorkbenchConfig>(
                        CE_OperatorMode.修改, workBenchConfig, oldWorkBenchConfig);
                }
            }
            else
            {
                string assemble = "";

                ZPX_ProductWorkbenchConfig workBenchConfig = new ZPX_ProductWorkbenchConfig();

                workBenchConfig.按序装配端口号 = (int)numOrderCommPort.Value;
                workBenchConfig.按序装配末道工序值 = (int)numBenchValue.Value;
                workBenchConfig.当前工位 = cmbCurrentBench.Text;
                workBenchConfig.分总成名称 = cmbAssemblyName.Text;
                workBenchConfig.上道工位 = cmbUpBench.Text;
                workBenchConfig.是按顺序流水号装配 = IsOrder.Text == "是" ? true : false;
                workBenchConfig.是本阶段末工位 = IsEndBench.Text == "是" ? true : false;
                workBenchConfig.是本阶段起始工位 = IsStartBench.Text == "是" ? true : false;
                workBenchConfig.是气密性检测工位 = IsTightness.Text == "是" ? true : false;
                workBenchConfig.是所有产品通用工位 = IsCommon.Text == "是" ? true : false;
                workBenchConfig.产品类型 = txtOrderProductType.Text;

                if (m_assemble != null)
                {
                    foreach (View_P_AssemblingBom itemAssemble in m_assemble)
                    {
                        if (m_preventErrorServer.IsAssemblingBom(itemAssemble.父总成名称, txtOrderProductType.Text))
                        {
                            assemble += itemAssemble.父总成名称 + ",";
                        }
                    }

                    if (assemble != "")
                    {
                        workBenchConfig.需测量总成名称 = assemble.Substring(0, assemble.Length - 1);
                    }
                    else
                    {
                        workBenchConfig.需测量总成名称 = "";
                    }
                }
                else
                {
                    workBenchConfig.需测量总成名称 = txtMeasure.Text;
                }

                if (!m_preventErrorServer.UpdateWorkbenchConfig(workBenchConfig, out m_error))
                {
                    flag = true;
                }

                m_sysLogServer.RecordLog<ZPX_ProductWorkbenchConfig>(
                        CE_OperatorMode.修改, workBenchConfig, oldWorkBenchConfig);
            }

            if (flag)
            {
                MessageDialog.ShowPromptMessage(m_error);
            }
            else
            {
                MessageDialog.ShowPromptMessage("修改成功！");

                RefrshAssembleOrder();
            }
        }

        private void 顺序添加toolStripButton_Click(object sender, EventArgs e)
        {
            //foreach (Control cl in panel14.Controls)
            //{
            //    if (cl is ComboBox && cl.Text == "")
            //    {
            //        MessageDialog.ShowPromptMessage("请选择蓝色字体的信息！");
            //        return;
            //    }
            //}

            if (txtOrderProductType.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("请选择产品类型！");
                return;
            }

            bool flag = false;

            foreach (View_P_ProductInfo item in m_productInfo)
            {
                ZPX_ProductWorkbenchConfig workBenchConfig = new ZPX_ProductWorkbenchConfig();

                workBenchConfig.按序装配端口号 = (int)numOrderCommPort.Value;
                workBenchConfig.按序装配末道工序值 = (int)numBenchValue.Value;
                workBenchConfig.产品类型 = item.产品类型编码;
                workBenchConfig.当前工位 = cmbCurrentBench.Text;
                workBenchConfig.分总成名称 = cmbAssemblyName.Text;
                workBenchConfig.上道工位 = cmbUpBench.Text;
                workBenchConfig.是按顺序流水号装配 = IsOrder.Text == "是" ? true : false;
                workBenchConfig.是本阶段末工位 = IsEndBench.Text == "是" ? true : false;
                workBenchConfig.是本阶段起始工位 = IsStartBench.Text == "是" ? true : false;
                workBenchConfig.是气密性检测工位 = IsTightness.Text == "是" ? true : false;
                workBenchConfig.是所有产品通用工位 = IsCommon.Text == "是" ? true : false;

                if (m_assemble != null)
                {
                    string assemble = "";
                    foreach (View_P_AssemblingBom itemAssemble in m_assemble)
                    {
                        if (m_preventErrorServer.IsAssemblingBom(itemAssemble.父总成名称, item.产品类型编码))
                        {
                            assemble += itemAssemble.父总成名称 + ",";
                        }
                    }

                    if (assemble != "")
                    {
                        workBenchConfig.需测量总成名称 = assemble.Substring(0, assemble.Length - 1);
                    }
                    else
                    {
                        workBenchConfig.需测量总成名称 = "";
                    }
                }
                else
                {
                    workBenchConfig.需测量总成名称 = txtMeasure.Text;
                }

                if (!m_preventErrorServer.InsertWorkbenchConfig(workBenchConfig, out m_error))
                {
                    flag = true;
                    m_error += " " + m_error;
                    continue;
                }

                m_sysLogServer.RecordLog<ZPX_ProductWorkbenchConfig>(
                    CE_OperatorMode.添加, workBenchConfig, workBenchConfig);
            }

            if (flag)
            {
                MessageDialog.ShowPromptMessage(m_error);
            }
            else
            {
                MessageDialog.ShowPromptMessage("添加成功！");
            }

            RefrshAssembleOrder();

            btnOrderProduct.Enabled = false;
            cmbCurrentBench.Enabled = false;
            cmbAssemblyName.Enabled = false;
        }

        private void dgvAssembleOrder_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            ColumnWidthControl.SaveColumnParams(lbOrderTitle.Text, e.Column);
        }

        private void 顺序刷新toolStripButton_Click(object sender, EventArgs e)
        {
            RefrshAssembleOrder();
        }

        private void 顺序删除toolStripButton_Click(object sender, EventArgs e)
        {
            if (dgvAssembleOrder.SelectedRows.Count == 1)
            {
                if (MessageDialog.ShowEnquiryMessage("确定删除选中的防错信息吗？") == DialogResult.Yes)
                {
                    DataGridViewRow row = dgvAssembleOrder.CurrentRow;

                    ZPX_ProductWorkbenchConfig oldWorkBenchConfig = new ZPX_ProductWorkbenchConfig();

                    oldWorkBenchConfig.按序装配端口号 = Convert.ToInt32(row.Cells["按序装配端口号"].Value);
                    oldWorkBenchConfig.按序装配末道工序值 = Convert.ToInt32(row.Cells["按序装配末道工序值"].Value);
                    oldWorkBenchConfig.当前工位 = row.Cells["当前工位"].Value.ToString();
                    oldWorkBenchConfig.分总成名称 = row.Cells["分总成名称"].Value.ToString();
                    oldWorkBenchConfig.上道工位 = row.Cells["上道工位"].Value.ToString();
                    oldWorkBenchConfig.是按顺序流水号装配 = row.Cells["是按顺序流水号装配"].Value.ToString() == "是" ? true : false;
                    oldWorkBenchConfig.是本阶段末工位 = row.Cells["是本阶段末工位"].Value.ToString() == "是" ? true : false;
                    oldWorkBenchConfig.是本阶段起始工位 = row.Cells["是本阶段起始工位"].Value.ToString() == "是" ? true : false;
                    oldWorkBenchConfig.是气密性检测工位 = row.Cells["是气密性检测工位"].Value.ToString() == "是" ? true : false;
                    oldWorkBenchConfig.是所有产品通用工位 = row.Cells["是所有产品通用工位"].Value.ToString() == "是" ? true : false;
                    oldWorkBenchConfig.产品类型 = row.Cells["产品类型"].Value.ToString();
                    oldWorkBenchConfig.需测量总成名称 = row.Cells["产品类型"].Value.ToString();

                    if (!m_preventErrorServer.DeleteWorkbenchConfig(
                        txtOrderProductType.Text,cmbCurrentBench.Text, out m_error))
                    {
                        MessageDialog.ShowPromptMessage(m_error);
                        return;
                    }
                    else
                    {
                        MessageDialog.ShowPromptMessage("删除成功！");
                        m_sysLogServer.RecordLog<ZPX_ProductWorkbenchConfig>(
                            CE_OperatorMode.删除, oldWorkBenchConfig, oldWorkBenchConfig);
                        RefrshAssembleOrder();
                    }
                }
            }
            else
            {
                MessageDialog.ShowPromptMessage("请选择一行再操作！");
            }
        }
        
        /// <summary>
        /// 顺序防错的dataGridView的焦点事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvAssembleOrder_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            txtOrderProductType.Text = dgvAssembleOrder.CurrentRow.Cells["产品类型"].Value.ToString();

            #region 2017-03-18, 夏石友，因点击“需测量总成”按钮无数据故增加此代码
            m_productCode = txtOrderProductType.Text;
            #endregion

            cmbCurrentBench.Text = dgvAssembleOrder.CurrentRow.Cells["当前工位"].Value.ToString();
            cmbAssemblyName.Text = dgvAssembleOrder.CurrentRow.Cells["分总成名称"].Value.ToString();
            cmbUpBench.Text = dgvAssembleOrder.CurrentRow.Cells["上道工位"].Value.ToString();
            IsStartBench.Text = Convert.ToBoolean(
                dgvAssembleOrder.CurrentRow.Cells["是本阶段起始工位"].Value) == true ? "是" : "否";
            IsEndBench.Text = Convert.ToBoolean(
                dgvAssembleOrder.CurrentRow.Cells["是本阶段末工位"].Value.ToString()) == true ? "是" : "否";
            IsTightness.Text = Convert.ToBoolean(
                dgvAssembleOrder.CurrentRow.Cells["是气密性检测工位"].Value.ToString()) == true ? "是" : "否";
            IsCommon.Text = Convert.ToBoolean(
                dgvAssembleOrder.CurrentRow.Cells["是所有产品通用工位"].Value.ToString()) == true ? "是" : "否";
            IsOrder.Text = Convert.ToBoolean(
                dgvAssembleOrder.CurrentRow.Cells["是按顺序流水号装配"].Value.ToString()) == true ? "是" : "否";
            numBenchValue.Text = dgvAssembleOrder.CurrentRow.Cells["按序装配末道工序值"].Value.ToString();
            numOrderCommPort.Text = dgvAssembleOrder.CurrentRow.Cells["按序装配端口号"].Value.ToString();
            txtMeasure.Text = dgvAssembleOrder.CurrentRow.Cells["需测量总成名称"].Value.ToString();

            DateTime time = ServerTime.Time;
            dtpProductStarTime.Value = dgvAssembleOrder.CurrentRow.Cells["屏蔽开始时间"].Value == DBNull.Value ? time :
                Convert.ToDateTime(dgvAssembleOrder.CurrentRow.Cells["屏蔽开始时间"].Value);
            dtpProductEndTime.Value = dgvAssembleOrder.CurrentRow.Cells["屏蔽终止时间"].Value == DBNull.Value ? time :
                Convert.ToDateTime(dgvAssembleOrder.CurrentRow.Cells["屏蔽终止时间"].Value);
        }

        /// <summary>
        /// CCD拍照防错刷新
        /// </summary>
        public void RefrshCCD()
        {
            DataTable dt = m_preventErrorServer.GetAllCCDConfig();

            dgvCCD.DataSource = dt;

            if (dt != null && dt.Rows.Count > 0)
            {
                this.dgvCCD.ColumnWidthChanged -= new System.Windows.Forms.DataGridViewColumnEventHandler(
                   this.dgvCCD_ColumnWidthChanged);

                ColumnWidthControl.SetDataGridView(lbCCDTitle.Text, dgvAssembleOrder);

                this.dgvCCD.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(
                    this.dgvCCD_ColumnWidthChanged);

                CCDUserControlDataLocalizer.Init(dgvCCD, this.Name,
                    UniversalFunction.SelectHideFields(this.Name, dgvCCD.Name, BasicInfo.LoginID));

                dgvCCD.Refresh();
            }
        }

        private void btnCCDFindProduct_Click(object sender, EventArgs e)
        {
            FormProductType form = new FormProductType();

            if (dgvCCD.Rows.Count > 0)
            {
                List<View_P_ProductInfo> list = new List<View_P_ProductInfo>();

                string[] productType = txtProductType.Text.Split(',');

                foreach (string item in productType)
                {
                    View_P_ProductInfo product = new View_P_ProductInfo();

                    product.产品类型编码 = item;
                    list.Add(product);
                }

                form.SelectedProduct = list;
            }

            string productStr = "";

            if (form.ShowDialog() == DialogResult.OK)
            {
                List<View_P_ProductInfo> productList = form.SelectedProduct;

                if (form.SelectedProduct.Count != form.ProductCount)
                {
                    foreach (View_P_ProductInfo item in productList)
                    {
                        productStr += item.产品类型编码 + ",";
                    }

                    productStr = productStr.Substring(0, productStr.Length - 1);
                }
                else
                {
                    productStr = "全部";
                }
            }

            txtProductType.Text = productStr;
        }

        private void btnCCDFindGoods_Click(object sender, EventArgs e)
        {
            if (cmbCCDBench.Text.Trim() != "")
            {
                FormQueryInfo form = QueryInfoDialog.GetAccessoryWorkbench(cmbCCDBench.Text.Trim());

                if (form == null || form.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                txtCCDGoodsCode.Text = form.GetDataItem("图号型号").ToString();
                txtCCDGoodsName.Text = form.GetDataItem("物品名称").ToString();
                txtCCDSpec.Text = form.GetDataItem("规格").ToString();
            }
            else
            {
                MessageDialog.ShowPromptMessage("请选择工位！");
                return;
            }
        }

        private void dgvCCD_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            ColumnWidthControl.SaveColumnParams(lbCCDTitle.Text, e.Column);
        }

        private void CCD新建toolStripButton_Click(object sender, EventArgs e)
        {
            txtCCDGoodsCode.Text = "";
            txtCCDGoodsName.Text = "";
            txtCCDSpec.Text = "";
            txtProductType.Text = "";
            cmbCommMode_CCD.SelectedIndex = -1;
            cmbCCDBench.SelectedIndex = -1;
        }

        private void CCD刷新toolStripButton_Click(object sender, EventArgs e)
        {
            RefrshCCD();
        }

        private void CCD添加toolStripButton_Click(object sender, EventArgs e)
        {
            if (cmbCCDBench.SelectedIndex == -1)
            {
                MessageDialog.ShowPromptMessage("请选择工位！");
                return;
            }

            if (txtEGoodsCode.Text.Trim() == "" && txtEGoodsName.Text.Trim() == ""
                && txtCCDSpec.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("请选择零件！");
                return;
            }

            if (txtCCDCommPort.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("请输入测点号！");
                return;
            }

            if (cmbCommMode_CCD.Text.Trim().Length == 0)
            {
                MessageDialog.ShowPromptMessage("请选择通信模式!");
                return;
            }

            ZPX_CCDConfig config = new ZPX_CCDConfig();

            config.测点号 = txtCCDCommPort.Text.Trim();
            config.工位 = cmbCCDBench.Text.Trim();
            config.零件规格 = txtCCDSpec.Text.Trim();
            config.零件名称 = txtCCDGoodsName.Text.Trim();
            config.零件图号 = txtCCDGoodsCode.Text.Trim();
            config.适用产品类型 = txtCCDProductType.Text == "全部" ? "" : txtCCDProductType.Text;
            config.通信模式 = cmbCommMode_CCD.Text;

            if (!m_preventErrorServer.InsertCCDConfig(config, out m_error))
            {
                MessageDialog.ShowPromptMessage(m_error);
            }
            else
            {
                MessageDialog.ShowPromptMessage("CCD拍照防错添加成功！");
                RefrshCCD();

                m_sysLogServer.RecordLog<ZPX_CCDConfig>(CE_OperatorMode.添加, config, config);

                cmbCCDBench.Enabled = false;
                btnCCDFindGoods.Enabled = false;
            }
        }

        private void CCD修改toolStripButton_Click(object sender, EventArgs e)
        {
            DataGridViewRow row = dgvCCD.CurrentRow;

            ZPX_CCDConfig oldConfig = new ZPX_CCDConfig();

            oldConfig.测点号 = row.Cells["测点号"].Value.ToString();
            oldConfig.工位 = row.Cells["工位"].Value.ToString();
            oldConfig.零件规格 = row.Cells["零件规格"].Value.ToString();
            oldConfig.零件名称 = row.Cells["零件名称"].Value.ToString();
            oldConfig.零件图号 = row.Cells["零件图号"].Value.ToString();
            oldConfig.适用产品类型 = row.Cells["适用产品类型"].Value.ToString() == "全部" ? "" : row.Cells["适用产品类型"].Value.ToString();

            ZPX_CCDConfig config = new ZPX_CCDConfig();

            config.测点号 = txtCCDCommPort.Text.Trim();
            config.工位 = cmbCCDBench.Text.Trim();
            config.零件规格 = txtCCDSpec.Text.Trim();
            config.零件名称 = txtCCDGoodsName.Text.Trim();
            config.零件图号 = txtCCDGoodsCode.Text.Trim();
            config.适用产品类型 = txtCCDProductType.Text == "全部" ? "" : txtCCDProductType.Text;
            config.通信模式 = cmbCommMode_CCD.Text;

            if (!m_preventErrorServer.UpdateCCDConfig(config, out m_error))
            {
                MessageDialog.ShowPromptMessage(m_error);
            }
            else
            {
                MessageDialog.ShowPromptMessage("CCD拍照防错修改成功！");
                RefrshCCD();

                m_sysLogServer.RecordLog<ZPX_CCDConfig>(CE_OperatorMode.修改, config, oldConfig);
            }
        }

        private void CCD删除toolStripButton_Click(object sender, EventArgs e)
        {
            if (dgvCCD.SelectedRows.Count == 1)
            {
                if (MessageDialog.ShowEnquiryMessage("确定删除选中的防错信息吗？") == DialogResult.Yes)
                {
                    DataGridViewRow row = dgvCCD.CurrentRow;
                    ZPX_CCDConfig oldConfig = new ZPX_CCDConfig();

                    oldConfig.测点号 = row.Cells["测点号"].Value.ToString();
                    oldConfig.工位 = row.Cells["工位"].Value.ToString();
                    oldConfig.零件规格 = row.Cells["零件规格"].Value.ToString();
                    oldConfig.零件名称 = row.Cells["零件名称"].Value.ToString();
                    oldConfig.零件图号 = row.Cells["零件图号"].Value.ToString();
                    oldConfig.适用产品类型 = row.Cells["适用产品类型"].Value.ToString() == "全部" ? "" : row.Cells["适用产品类型"].Value.ToString();

                    if (!m_preventErrorServer.DeleteCCDConfig(txtCCDGoodsCode.Text,txtCCDGoodsName.Text,
                        txtCCDSpec.Text,cmbCCDBench.Text, out m_error))
                    {
                        MessageDialog.ShowPromptMessage(m_error);
                        return;
                    }
                    else
                    {
                        MessageDialog.ShowPromptMessage("删除成功！");
                        m_sysLogServer.RecordLog<ZPX_CCDConfig>(CE_OperatorMode.删除, oldConfig, oldConfig);
                        RefrshCCD();
                    }
                }
            }
            else
            {
                MessageDialog.ShowPromptMessage("请选择一行再操作！");
            }
        }

        /// <summary>
        /// CCD防错的dataGridView的焦点事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvCCD_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            txtCCDCommPort.Text = dgvCCD.CurrentRow.Cells["测点号"].Value.ToString();
            txtCCDGoodsCode.Text = dgvCCD.CurrentRow.Cells["零件图号"].Value.ToString();
            txtCCDGoodsName.Text = dgvCCD.CurrentRow.Cells["零件名称"].Value.ToString();
            txtCCDSpec.Text = dgvCCD.CurrentRow.Cells["零件规格"].Value.ToString();
            txtCCDProductType.Text = dgvCCD.CurrentRow.Cells["适用产品类型"].Value.ToString();
            cmbCCDBench.Text = dgvCCD.CurrentRow.Cells["工位"].Value.ToString();
            cmbCommMode_CCD.Text = dgvCCD.CurrentRow.Cells["通信模式"].Value.ToString();

            DateTime time = ServerTime.Time;
            dtpCCDStarTime.Value = dgvCCD.CurrentRow.Cells["屏蔽开始时间"].Value == DBNull.Value ? time :
                Convert.ToDateTime(dgvCCD.CurrentRow.Cells["屏蔽开始时间"].Value);
            dtpCCDEndTime.Value = dgvCCD.CurrentRow.Cells["屏蔽终止时间"].Value == DBNull.Value ? time :
                Convert.ToDateTime(dgvCCD.CurrentRow.Cells["屏蔽终止时间"].Value);
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 2)
            {
                #region 获取分总成名称

                IQueryable<P_AssemblingBom> queryAssemblingBom = null;

                if (!m_preventErrorServer.GetAssemblingBom(out queryAssemblingBom, out m_error))
                {
                    MessageDialog.ShowErrorMessage(m_error);
                }

                cmbAssemblyName.Items.AddRange((from r in queryAssemblingBom 
                                                where r.ParentName != null 
                                                orderby r.ParentName 
                                                select r.ParentName).Distinct().ToArray());
                #endregion

                IsStartBench.SelectedIndex = 1;
            }
        }

        private void btnOrderProduct_Click(object sender, EventArgs e)
        {
            m_productCode = "";

            FormProductType form = new FormProductType();

            if (dgvAssembleOrder.Rows.Count > 0)
            {
                List<View_P_ProductInfo> list = new List<View_P_ProductInfo>();

                string[] productType = txtProductType.Text.Split(',');

                foreach (string item in productType)
                {
                    View_P_ProductInfo product = new View_P_ProductInfo();

                    product.产品类型编码 = item;
                    list.Add(product);
                }

                form.SelectedProduct = list;
            }

            string productStr = "";

            if (form.ShowDialog() == DialogResult.OK)
            {
                m_productInfo = form.SelectedProduct;

                if (form.SelectedProduct.Count != form.ProductCount)
                {
                    foreach (View_P_ProductInfo item in m_productInfo)
                    {
                        m_productCode += item.产品类型编码 + "','";
                        productStr += item.产品类型编码 + ",";
                    }

                    productStr = productStr.Substring(0, productStr.Length - 1);
                    m_productCode = m_productCode.Substring(0, m_productCode.Length - 3);
                }
                else
                {
                    productStr = "全部";
                }
            }

            txtOrderProductType.Text = productStr;
        }

        private void 设置防错信息_Load(object sender, EventArgs e)
        {
            toolStrip1.Visible = true;
            toolStrip2.Visible = true;
            toolStrip3.Visible = true;
            toolStrip4.Visible = true;
        }

        private void btnMeasure_Click(object sender, EventArgs e)
        {
            FormFZC form = new FormFZC(m_productCode);

            if (dgvAssembleOrder.Rows.Count > 0)
            {
                List<View_P_AssemblingBom> list = new List<View_P_AssemblingBom>();

                string[] productType = txtOrderProductType.Text.Split(',');

                foreach (string item in productType)
                {
                    if (item != "")
                    {
                        View_P_AssemblingBom product = new View_P_AssemblingBom();

                        product.父总成编码 = item;
                        list.Add(product);
                    }
                }

                form.SelectedProduct = list;
            }

            string productStr = "";
           
            if (form.ShowDialog() == DialogResult.OK)
            {
                List<View_P_AssemblingBom> assemblingBom = form.SelectedProduct;
                m_assemble = assemblingBom;

                foreach (View_P_AssemblingBom item in assemblingBom)
                {
                    productStr += item.父总成名称 + ",";
                }

                productStr = productStr.Substring(0, productStr.Length - 1);
            }

            txtMeasure.Text = productStr;
        }

        private void btnTorqueShield_Click(object sender, EventArgs e)
        {
            //打扭矩屏蔽
            if (dataGridView1.SelectedRows.Count > 0)
            {
                if (MessageDialog.ShowEnquiryMessage("您确定屏蔽选中的零件吗？") == DialogResult.Yes)
                {
                    bool flag = false;

                    for (int i = 0; i < dataGridView1.SelectedRows.Count; i++)
                    {
                        if (!m_preventErrorServer.UpdateShieldTime(
                            dataGridView1.SelectedRows[i].Cells["打扭矩工位"].Value.ToString(),
                            dataGridView1.SelectedRows[i].Cells["图号型号"].Value.ToString(),
                            dataGridView1.SelectedRows[i].Cells["物品名称"].Value.ToString(),
                            dataGridView1.SelectedRows[i].Cells["规格"].Value.ToString(),
                            dtpStarTime.Value, dtpEndTime.Value, out m_error))
                        {
                            flag = true;

                            MessageDialog.ShowPromptMessage("工位："+
                                dataGridView1.SelectedRows[i].Cells["打扭矩工位"].Value.ToString()+
                                "物品名称：" + dataGridView1.SelectedRows[i].Cells["物品名称"].Value.ToString() 
                                + " " + m_error);
                            break;
                        }

                        m_sysLogServer.RecordLog<ZPX_TorqueSpanner>(CE_OperatorMode.修改, "屏蔽的起止时间由原来的" +
                            dataGridView1.SelectedRows[i].Cells["屏蔽开始时间"].Value.ToString() + "至" +
                            dataGridView1.SelectedRows[i].Cells["屏蔽终止时间"].Value.ToString() + " 修改为:" +
                            dtpStarTime.Value + "至" + dtpEndTime.Value);
                    }

                    if (!flag)
                    {
                        MessageDialog.ShowPromptMessage("屏蔽成功！");
                        RefreshDataGridView();
                    }
                }
            }
        }

        private void btnLeakproofShield_Click(object sender, EventArgs e)
        {
            //电子称屏蔽
            if (dgvElectronic.SelectedRows.Count > 0)
            {
                if (MessageDialog.ShowEnquiryMessage("您确定屏蔽选中的零件吗？") == DialogResult.Yes)
                {
                    bool flag = false;

                    for (int i = 0; i < dgvElectronic.SelectedRows.Count; i++)
                    {
                        if (!m_preventErrorServer.UpdateLeakproofTime(
                            dgvElectronic.SelectedRows[i].Cells["工位"].Value.ToString(),
                            dgvElectronic.SelectedRows[i].Cells["防漏装零件图号"].Value.ToString(),
                            dgvElectronic.SelectedRows[i].Cells["防漏装零件名称"].Value.ToString(),
                            dgvElectronic.SelectedRows[i].Cells["防漏装零件规格"].Value.ToString(),
                            dtpLeakStarTime.Value, dtpLeakEndTime.Value, out m_error))
                        {
                            flag = true;

                            MessageDialog.ShowPromptMessage("工位：" +
                                dgvElectronic.SelectedRows[i].Cells["工位"].Value.ToString() +
                                "物品名称：" + dgvElectronic.SelectedRows[i].Cells["防漏装零件名称"].Value.ToString()
                                + " " + m_error);
                            break;
                        }

                        m_sysLogServer.RecordLog<ZPX_LeakproofParts>(CE_OperatorMode.修改, "屏蔽的起止时间由原来的" +
                           dgvElectronic.SelectedRows[i].Cells["屏蔽开始时间"].Value.ToString() + "至" +
                           dgvElectronic.SelectedRows[i].Cells["屏蔽终止时间"].Value.ToString() + " 修改为:" +
                           dtpStarTime.Value + "至" + dtpEndTime.Value);
                    }

                    if (!flag)
                    {
                        MessageDialog.ShowPromptMessage("屏蔽成功！");
                        RefrshdgvElectronic();
                    }
                }
            }
        }

        private void btnProductShield_Click(object sender, EventArgs e)
        {
            //装配顺序屏蔽 
            if (dgvAssembleOrder.SelectedRows.Count > 0)
            {
                if (MessageDialog.ShowEnquiryMessage("您确定屏蔽选中的零件吗？") == DialogResult.Yes)
                {
                    bool flag = false;

                    for (int i = 0; i < dgvAssembleOrder.SelectedRows.Count; i++)
                    {
                        if (!m_preventErrorServer.UpdateProductWorkbenchTime(
                            dgvAssembleOrder.SelectedRows[i].Cells["当前工位"].Value.ToString(),
                            dgvAssembleOrder.SelectedRows[i].Cells["上道工位"].Value.ToString(),
                            dgvAssembleOrder.SelectedRows[i].Cells["产品类型"].Value.ToString(),
                            dgvAssembleOrder.SelectedRows[i].Cells["分总成名称"].Value.ToString(),
                            dtpProductStarTime.Value, dtpProductEndTime.Value, out m_error))
                        {
                            flag = true;

                            MessageDialog.ShowPromptMessage("工位：" +
                                dgvAssembleOrder.SelectedRows[i].Cells["当前工位"].Value.ToString() +
                                "物品名称：" + dgvAssembleOrder.SelectedRows[i].Cells["分总成名称"].Value.ToString()
                                + " " + m_error);
                            break;
                        }

                        m_sysLogServer.RecordLog<ZPX_ProductWorkbenchConfig>(CE_OperatorMode.修改, "屏蔽的起止时间由原来的" +
                           dgvAssembleOrder.SelectedRows[i].Cells["屏蔽开始时间"].Value.ToString() + "至" +
                           dgvAssembleOrder.SelectedRows[i].Cells["屏蔽终止时间"].Value.ToString() + " 修改为:" +
                           dtpStarTime.Value + "至" + dtpEndTime.Value);
                    }

                    if (!flag)
                    {
                        MessageDialog.ShowPromptMessage("屏蔽成功！");
                        RefrshAssembleOrder();
                    }
                }
            }
        }

        private void btnCCDShield_Click(object sender, EventArgs e)
        {
            //CCD屏蔽
            if (dgvCCD.SelectedRows.Count > 0)
            {
                if (MessageDialog.ShowEnquiryMessage("您确定屏蔽选中的零件吗？") == DialogResult.Yes)
                {
                    bool flag = false;

                    for (int i = 0; i < dgvCCD.SelectedRows.Count; i++)
                    {
                        if (!m_preventErrorServer.UpdateCCDConfigTime(
                            dgvCCD.SelectedRows[i].Cells["工位"].Value.ToString(),
                            dgvCCD.SelectedRows[i].Cells["零件图号"].Value.ToString(),
                            dgvCCD.SelectedRows[i].Cells["零件名称"].Value.ToString(),
                            dgvCCD.SelectedRows[i].Cells["零件规格"].Value.ToString(),
                            dtpCCDStarTime.Value, dtpCCDEndTime.Value, out m_error))
                        {
                            flag = true;

                            MessageDialog.ShowPromptMessage("工位：" +
                                dgvCCD.SelectedRows[i].Cells["工位"].Value.ToString() +
                                "物品名称：" + dgvCCD.SelectedRows[i].Cells["零件名称"].Value.ToString()
                                + " " + m_error);
                            break;
                        }

                        m_sysLogServer.RecordLog<ZPX_CCDConfig>(CE_OperatorMode.修改, "屏蔽的起止时间由原来的" +
                           dgvCCD.SelectedRows[i].Cells["屏蔽开始时间"].Value.ToString() + "至" +
                           dgvCCD.SelectedRows[i].Cells["屏蔽终止时间"].Value.ToString() + " 修改为:" +
                           dtpStarTime.Value + "至" + dtpEndTime.Value);
                    }

                    if (!flag)
                    {
                        MessageDialog.ShowPromptMessage("屏蔽成功！");
                        RefrshCCD();
                    }
                }
            }
        }
    }
}

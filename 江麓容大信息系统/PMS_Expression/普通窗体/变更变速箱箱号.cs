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
using UniversalControlLibrary;

namespace Expression
{
    public partial class 变更变速箱箱号 : Form
    {
        /// <summary>
        /// 电子档案服务组件
        /// </summary>
        private IElectronFileServer m_electronFileServer = ServerModuleFactory.GetServerModule<IElectronFileServer>();

        /// <summary>
        /// 变速箱维修信息服务
        /// </summary>
        private IConvertCVTNumber m_convertCVTServer = ServerModuleFactory.GetServerModule<IConvertCVTNumber>();

        /// <summary>
        /// 产品信息服务
        /// </summary>
        private IProductInfoServer m_productInfoServer = ServerModuleFactory.GetServerModule<IProductInfoServer>();

        /// <summary>
        /// 产品编号服务
        /// </summary>
        private IProductCodeServer m_productCodeServer = ServerModuleFactory.GetServerModule<IProductCodeServer>();

        /// <summary>
        /// 错误信息
        /// </summary>
        private string error;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="nodeInfo">功能树节点信息</param>
        public 变更变速箱箱号(PlatformManagement.FunctionTreeNodeInfo nodeInfo)
        {
            InitializeComponent();

            FaceAuthoritySetting.SetEnable(tabPageAdd.Controls, nodeInfo.Authority);

            Init();
        }

        /// <summary>
        /// 系统初始化
        /// </summary>
        private void Init()
        {
            #region 查询返修信息

            DateTime dtBegin, dtEnd;
            ServerTime.GetMonthlyBalance(ServerTime.Time, out dtBegin, out dtEnd);
            dateTimePickerET.Value = dtBegin.AddDays(1).Date;
            RefreshDataGridViewOfRepairInfo();

            #endregion

            string[] productType = null;

            if (!m_productInfoServer.GetAllProductType(out productType, out error))
            {
                MessageDialog.ShowErrorMessage(error);
                return;
            }

            List<string> lstProductType = productType.ToList();

            lstProductType.RemoveAll(p=> p.Contains(" FX"));

            cmbOldCVTType.Items.Clear();
            cmbNewCVTType.Items.Clear();

            cmbOldCVTType.Items.AddRange(lstProductType.ToArray());
            cmbNewCVTType.Items.AddRange(lstProductType.ToArray());

            cmbOldCVTType.SelectedIndex = 0;
            cmbNewCVTType.SelectedIndex = 0;
        }

        /// <summary>
        /// 控制新旧变速箱信息显示滚动条同步
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridViewOld_Scroll(object sender, ScrollEventArgs e)
        {
            if (e.ScrollOrientation == ScrollOrientation.HorizontalScroll)
            {
                dataGridViewNew.HorizontalScrollingOffset = dataGridViewOld.HorizontalScrollingOffset;
            }
        }

        /// <summary>
        /// 控制新旧变速箱信息显示滚动条同步
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridViewNew_Scroll(object sender, ScrollEventArgs e)
        {
            if (e.ScrollOrientation == ScrollOrientation.HorizontalScroll)
            {
                dataGridViewOld.HorizontalScrollingOffset = dataGridViewNew.HorizontalScrollingOffset;
            }
        }

        private void dataGridViewOld_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (dataGridViewNew.CurrentCell.RowIndex != e.RowIndex)
            {
                int rowIndex = e.RowIndex - 1 < 0 ? 0 : e.RowIndex - 1;

                if (rowIndex < dataGridViewNew.Rows.Count)
                {
                    dataGridViewNew.FirstDisplayedCell = dataGridViewNew.Rows[rowIndex].Cells[e.ColumnIndex];
                    dataGridViewNew.CurrentCell = dataGridViewNew.FirstDisplayedCell;
                }

                dataGridViewOld.FirstDisplayedCell = dataGridViewOld.Rows[rowIndex].Cells[e.ColumnIndex];
                dataGridViewOld.CurrentCell = dataGridViewOld.FirstDisplayedCell;
            }
        }

        /// <summary>
        /// 刷新维修信息数据显示控件
        /// </summary>
        private void RefreshDataGridViewOfRepairInfo()
        {
            dataGridViewRepair.DataSource = m_convertCVTServer.GetData(dateTimePickerST.Value, dateTimePickerET.Value);

            if (dataGridViewRepair.Columns.Count > 0)
            {
                dataGridViewRepair.Columns[0].Visible = false;
            }
        }

        private void RefreshDataGridView(string oldProductCode, string newProductCode)
        {
            IQueryable<View_P_ElectronFile> oldEFInfo = null;
            IQueryable<View_P_ElectronFile> newEFInfo = null;
            string error = null;

            dataGridViewNew.Rows.Clear();
            dataGridViewOld.Rows.Clear();

            if (!m_electronFileServer.GetElectronFile(oldProductCode, out oldEFInfo, out error))
            {
                MessageDialog.ShowErrorMessage(error);
                return;
            }

            if (!m_electronFileServer.GetElectronFile(newProductCode, out newEFInfo, out error))
            {
                MessageDialog.ShowErrorMessage(error);
                return;
            }

            int index = 0;

            foreach (var item in oldEFInfo)
            {
                dataGridViewOld.Rows.Add(new object[] {
                    item.序号, item.父总成名称, item.父总成编码, item.零部件编码, item.零部件名称, item.规格,
                    item.零件标识码, item.数量, item.供应商, item.批次号, item.工位, item.检测数据, item.实际数据,
                    item.装配人员, item.装配模式, item.装配时间, item.备注 });

                dataGridViewOld.Rows[index++].Tag = item;
            }

            index = 0;

            foreach (var item in newEFInfo)
            {
                dataGridViewNew.Rows.Add(new object[] {
                    item.序号, item.父总成名称, item.父总成编码, item.零部件编码, item.零部件名称, item.规格,
                    item.零件标识码, item.数量, item.供应商, item.批次号, item.工位, item.检测数据, item.实际数据,
                    item.装配人员, item.装配模式, item.装配时间, item.备注 });

                dataGridViewNew.Rows[index++].Tag = item;
            }

            for (int i = 0; i < dataGridViewOld.Rows.Count; i++)
            {
                View_P_ElectronFile oldItem =  dataGridViewOld.Rows[i].Tag as View_P_ElectronFile;
                View_P_ElectronFile newItem = null;

                if (i < dataGridViewNew.Rows.Count)
                {
                    newItem =  dataGridViewNew.Rows[i].Tag as View_P_ElectronFile;
                }

                if (oldItem == null || newItem == null)
                {
                    continue;
                }

                if (oldItem.父总成名称 != newItem.父总成名称 || oldItem.零部件编码 != newItem.零部件编码 || oldItem.规格 != newItem.规格)
                {
                    if (oldItem.父总成名称 == "")
                        continue;

                    int rowAmount = InsertRow(i);

                    for (int j = 0; j < rowAmount; j++)
                    {
                        // change color
                        dataGridViewOld.Rows[i + j].DefaultCellStyle.BackColor = Color.Red;
                        dataGridViewNew.Rows[i + j].DefaultCellStyle.BackColor = Color.Red;

                    }

                    i = i + rowAmount;
                }
            }

            //DataGridViewRow row = new DataGridViewRow();
            
            //row.CreateCells(dataGridViewNew);

            //row.Cells[0].Value = 1;
            //row.Cells[1].Value = "RDC15-RB 110400131";

            //dataGridViewNew.Rows.Add(row);
        }

        private int InsertRow(int index)
        {
            if (index >= dataGridViewNew.Rows.Count || index >= dataGridViewOld.Rows.Count)
            {
                return 0;
            }

            View_P_ElectronFile oldItem = dataGridViewOld.Rows[index].Tag as View_P_ElectronFile;
            View_P_ElectronFile newItem = dataGridViewNew.Rows[index].Tag as View_P_ElectronFile;
            View_P_ElectronFile item = null;

            // 扫描旧箱数据，查找新箱中的相同零件
            for (int i = 1; i < 10; i++)
            {
                if (index + i >= dataGridViewNew.Rows.Count)
                    break;

                item = dataGridViewNew.Rows[index + i].Tag as View_P_ElectronFile;

                if (oldItem.父总成名称 == item.父总成名称 && oldItem.零部件编码 == item.零部件编码)
                {
                    for (int row = 0; row < i; row++)
                    {
                        dataGridViewOld.Rows.Insert(index + row, new DataGridViewRow());
                    }

                    return i;
                }
            }

            // 扫描新箱数据，查找旧箱中的相同零件
            for (int i = 1; i < 10; i++)
            {
                if (index + i >= dataGridViewOld.Rows.Count)
                    break;

                item = dataGridViewOld.Rows[index + i].Tag as View_P_ElectronFile;

                if (item.父总成名称 == newItem.父总成名称 && item.零部件编码 == newItem.零部件编码)
                {
                    for (int row = 0; row < i; row++)
                    {
                        dataGridViewNew.Rows.Insert(index + row, new DataGridViewRow());
                    }

                    return i;
                }
            }

            return 0;
        }

        /// <summary>
        /// 检索维修信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            RefreshDataGridViewOfRepairInfo();
        }

        private void dataGridViewRepair_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
            {
                return;
            }

            // 比较新旧箱数据
            string oldProductCode = (string)dataGridViewRepair.Rows[e.RowIndex].Cells["旧箱型号"].Value + " " +
                (string)dataGridViewRepair.Rows[e.RowIndex].Cells["旧箱编号"].Value;

            string newProductCode = (string)dataGridViewRepair.Rows[e.RowIndex].Cells["新箱型号"].Value + " " +
                (string)dataGridViewRepair.Rows[e.RowIndex].Cells["新箱编号"].Value;

            lblOldCVTNumber.Text = oldProductCode;
            lblNewCVTNumber.Text = newProductCode;

            RefreshDataGridView(oldProductCode, newProductCode);
        }

        /// <summary>
        /// 绘制行号
        /// </summary>
        /// <param name="dataGridView">要绘制的控件</param>
        /// <param name="e">绘制事件</param>
        private void RowPostPaint(DataGridView dataGridView, DataGridViewRowPostPaintEventArgs e)
        {
            Rectangle rectangle = new Rectangle(e.RowBounds.Location.X,
                e.RowBounds.Location.Y,
                dataGridView.RowHeadersWidth - 4,
                e.RowBounds.Height);

            TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(),
                dataGridView.RowHeadersDefaultCellStyle.Font,
                rectangle,
                dataGridView.RowHeadersDefaultCellStyle.ForeColor,
                TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
        }

        private void dataGridViewOld_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            RowPostPaint(dataGridViewOld, e);
        }

        private void dataGridViewNew_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            RowPostPaint(dataGridViewNew, e);
        }

        private void dataGridViewRepair_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            RowPostPaint(dataGridViewRepair, e);
        }

        private void panelClientTop_Resize(object sender, EventArgs e)
        {
            labelTitle.Location = new Point((panelClientTop.Width - labelTitle.Width) / 2, labelTitle.Location.Y);
        }

        /// <summary>
        /// 离开旧箱箱号编辑框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtOldCVTNumber_Leave(object sender, EventArgs e)
        {
            #region 2012.07.28 屏蔽代码，因售后库房旧箱业务不正确（存在先退货再退库的现象，而不是先退库再退货）

            //// 检查旧箱是否存在，及是否0公里返修箱
            //ISellIn m_sellInServer = ServerModuleFactory.GetServerModule<ISellIn>();

            //// 业务类型
            //string businessType = m_sellInServer.GetYWFS(cmbOldCVTType.Text, txtOldCVTNumber.Text);

            //if (!businessType.Contains("退货"))
            //{
            //    MessageDialog.ShowErrorMessage("旧箱号业务类型不正确，其业务类型为：" + businessType);
            //    txtNewCVTNumber.Enabled = false;
            //    txtNewCVTNumber.Text = "";
            //    return;
            //}
            //else
            //{
            //    txtNewCVTNumber.Enabled = true;
            //}

            //chkReturnZeroDistance.Checked = businessType.Contains("0公里"); 
            #endregion
        }

        /// <summary>
        /// 生成新箱电子档案
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGenerateNewCVT_Click(object sender, EventArgs e)
        {
            if (txtRemark.Text.Trim() == "")
            {
                txtRemark.Focus();
                MessageDialog.ShowErrorMessage("说明不允许为空");
                return;
            }

            string oldProductCode = cmbOldCVTType.Text + " " + txtOldCVTNumber.Text;
            string newProductCode = cmbNewCVTType.Text + " " + txtNewCVTNumber.Text;

            if (!m_electronFileServer.IsExists(oldProductCode))
            {
                MessageDialog.ShowErrorMessage("电子档案中找不到旧箱信息，检查操作是否有误，无法继续");
                return;
            }

            if (m_electronFileServer.IsExists(newProductCode))
            {
                MessageDialog.ShowErrorMessage("电子档案中已经存在新箱信息，检查操作是否有误，无法继续");
                return;
            }

            // 检测录入的新箱箱号格式是否正确
            if (!m_productCodeServer.VerifyProductCodesInfo(
                cmbNewCVTType.Text, txtNewCVTNumber.Text, GlobalObject.CE_BarCodeType.内部钢印码, out error))
            {
                MessageDialog.ShowErrorMessage(error);

                txtNewCVTNumber.SelectAll();
                txtNewCVTNumber.Focus();

                return;
            }

            #region 2013-09-22 夏石友

            //if (m_convertCVTServer.IsNewCVT(cmbOldCVTType.Text, txtOldCVTNumber.Text))
            //{
            //    MessageDialog.ShowErrorMessage("旧箱箱号还没有进行营销业务，不允许进行此操作");

            //    txtOldCVTNumber.SelectAll();
            //    txtOldCVTNumber.Focus();

            //    return;
            //}

            #endregion

            if (!m_convertCVTServer.IsNewCVT(cmbNewCVTType.Text, txtNewCVTNumber.Text))
            {
                MessageDialog.ShowErrorMessage("新箱号已经被使用过，不允许再进行此操作");

                txtNewCVTNumber.SelectAll();
                txtNewCVTNumber.Focus();

                return;
            }

            if (m_convertCVTServer.IsExists(ConvertCVTNumber_CheckEnum.检查旧箱信息, cmbOldCVTType.Text, txtOldCVTNumber.Text))
            {
                MessageDialog.ShowErrorMessage("旧箱号已经变更过，不允许再进行此操作");

                txtOldCVTNumber.SelectAll();
                txtOldCVTNumber.Focus();

                return;
            }

            if (m_convertCVTServer.IsExists(ConvertCVTNumber_CheckEnum.检查新旧箱信息, cmbNewCVTType.Text, txtNewCVTNumber.Text))
            {
                MessageDialog.ShowErrorMessage("新箱号已经变更过，不允许再进行此操作");

                txtNewCVTNumber.SelectAll();
                txtNewCVTNumber.Focus();

                return;
            }

            if (m_convertCVTServer.IsExists(ConvertCVTNumber_CheckEnum.检查新箱档案信息, cmbNewCVTType.Text, txtNewCVTNumber.Text))
            {
                MessageDialog.ShowErrorMessage("新箱号电子档案中已经存在，不允许再进行此操作");

                txtNewCVTNumber.SelectAll();
                txtNewCVTNumber.Focus();

                return;
            }

            if (!ServerModuleFactory.GetServerModule<IPrintProductBarcodeInfo>().IsExists(newProductCode))
            {
                MessageDialog.ShowErrorMessage("新箱号还未分配，不允许进行此操作");

                txtNewCVTNumber.SelectAll();
                txtNewCVTNumber.Focus();

                return;
            }

            ZPX_ConvertedCVTNumber data = new ZPX_ConvertedCVTNumber();

            data.OldProductType = cmbOldCVTType.Text;
            data.OldProductNumber = txtOldCVTNumber.Text;
            data.NewProductType = cmbNewCVTType.Text;
            data.NewProductNumber = txtNewCVTNumber.Text;
            
            data.IsZeroKilometre = chkReturnZeroDistance.Checked;
            data.UserCode = GlobalObject.BasicInfo.LoginID;
            data.Date = ServerTime.Time;
            data.Remark = "单个变更：" + txtRemark.Text;

            if (!m_convertCVTServer.Add(data, out error))
            {
                MessageDialog.ShowErrorMessage(error);
            }
            else
            {
                MessageDialog.ShowPromptMessage("成功添加返修信息, 并在电子档案中生成新箱信息");

                RefreshDataGridViewOfRepairInfo();
            }
        }

        private void lblExpand_MouseEnter(object sender, EventArgs e)
        {
            lblExpand.ForeColor = Color.Blue;
        }

        private void lblExpand_MouseLeave(object sender, EventArgs e)
        {
            lblExpand.ForeColor = Color.Black;
        }

        private void picExpand_Click(object sender, EventArgs e)
        {
            if (panelLeft.Width < 950)
            {
                panelLeft.Width = 950;

                this.picExpand.Image = global::UniversalControlLibrary.Properties.Resources.回退;

                lblExpand.Text = "收拢";
            }
            else
            {
                panelLeft.Width = 266;

                this.picExpand.Image = global::UniversalControlLibrary.Properties.Resources.前进;

                lblExpand.Text = "展开";
            }
        }

        private void lblExpand_Click(object sender, EventArgs e)
        {
            picExpand_Click(sender, e);
        }

        private void picExpand_MouseEnter(object sender, EventArgs e)
        {
            lblExpand_MouseEnter(sender, e);
        }

        private void picExpand_MouseLeave(object sender, EventArgs e)
        {
            lblExpand_MouseLeave(sender, e);
        }
    }
}

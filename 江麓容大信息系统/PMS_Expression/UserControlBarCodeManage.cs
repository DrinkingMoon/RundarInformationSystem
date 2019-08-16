/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  UserControlBarCodeManage.cs
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
using System.Text;
using System.Windows.Forms;
using ServerModule;
using System.Linq;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using PlatformManagement;
using GlobalObject;
using UniversalControlLibrary;

namespace Expression
{
    /// <summary>
    /// 条形码管理组件
    /// </summary>
    public partial class UserControlBarCodeManage : Form
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        string m_err;

        /// <summary>
        /// 查找全部库存信息
        /// </summary>
        IQueryResult m_returnBill = null;

        /// <summary>
        /// 服务组件
        /// </summary>
        IBarCodeServer m_barCodeServer = ServerModuleFactory.GetServerModule<IBarCodeServer>();

        /// <summary>
        /// 数据定位控件
        /// </summary>
        UserControlDataLocalizer m_dataLocalizer;

        /// <summary>
        /// 查找条件字段列表
        /// </summary>
        List<string> m_lstFindField = new List<string>();

        /// <summary>
        /// 授权标志
        /// </summary>
        AuthorityFlag m_authorityFlag;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="nodeInfo">授权节点信息</param>
        public UserControlBarCodeManage(FunctionTreeNodeInfo nodeInfo)
        {
            InitializeComponent();

            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
            UpdateStyles();

            m_authorityFlag = nodeInfo.Authority;

            IAuthorization authorization = PlatformFactory.GetObject<IAuthorization>();
            string businessID = "条形码查询";
            IQueryResult qr = authorization.Query(businessID, null, "条形码 = 1", 0);
            DataColumnCollection columns = qr.DataCollection.Tables[0].Columns;

            if (qr.Succeeded && columns.Count > 0)
            {
                RefreshDataGridView(qr.DataCollection.Tables[0]);

                for (int i = 0; i < columns.Count; i++)
                {
                    m_lstFindField.Add(columns[i].ColumnName);
                }
            }
        }

        /// <summary>
        /// 改变组件大小
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControlBarCodeManage_Resize(object sender, EventArgs e)
        {
            labelTitle.Location = new Point((this.Width - labelTitle.Width) / 2, labelTitle.Location.Y);
        }

        /// <summary>
        /// 窗体载入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControlBarCodeManage_Load(object sender, EventArgs e)
        {
            FaceAuthoritySetting.SetVisibly(toolStrip1, m_authorityFlag);
        }

        /// <summary>
        /// 打印条形码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrintBarCode_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count < 1)
            {
                MessageDialog.ShowPromptMessage("选择打印的条形码记录行不允许为空!");
                return;
            }

            List<View_S_InDepotGoodsBarCodeTable> lstBarCodeInfo = new List<View_S_InDepotGoodsBarCodeTable>();

            for (int i = 0; i < dataGridView1.SelectedRows.Count; i++)
            {
                DataGridViewCellCollection cells = dataGridView1.SelectedRows[i].Cells;
                View_S_InDepotGoodsBarCodeTable barcode = new View_S_InDepotGoodsBarCodeTable();

                barcode.条形码 = (int)cells["条形码"].Value;
                barcode.图号型号 = cells["图号型号"].Value.ToString();
                barcode.物品名称 = cells["物品名称"].Value.ToString();
                barcode.规格 = cells["规格"].Value.ToString();
                barcode.供货单位 = cells["供货单位"].Value.ToString();
                barcode.批次号 = cells["批次号"].Value.ToString();
                barcode.货架 = cells["货架"].Value.ToString();
                barcode.层 = cells["层"].Value.ToString();
                barcode.列 = cells["列"].Value.ToString();
                barcode.工位 = cells["工位"].Value.ToString();

                //View_S_InDepotGoodsBarCodeTable barcode = m_barCodeServer.GetBarCodeInfo(
                //    cells["图号型号"].Value.ToString(), cells["物品名称"].Value.ToString(), cells["规格"].Value.ToString(),
                //    cells["供货单位"].Value.ToString(), cells["批次号"].Value.ToString(), cells["库房代码"].Value.ToString());

                lstBarCodeInfo.Add(barcode);
            }

            foreach (var item in lstBarCodeInfo)
            {
                ServerModule.PrintPartBarcode.PrintBarcodeList(item);
            }
        }

        private void dataGridView1_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            ColumnWidthControl.SaveColumnParams(labelTitle.Text, e.Column);
        }

        private int SortMethod(View_S_InDepotGoodsBarCodeTable data1, View_S_InDepotGoodsBarCodeTable data2)
        {
            return data1.工位.CompareTo(data2.工位);
        }

        private void btnPrintCVTBarcodes_Click(object sender, EventArgs e)
        {
            StringBuilder error = new StringBuilder();
            string errorTemp = null;

            打印整台份返修箱条码 form = new 打印整台份返修箱条码();
            form.ShowDialog();

            List<View_ZPX_ReparativeBarcode> lstPrintData = form.PrintData;

            if (lstPrintData != null && form.blPrintFlag && lstPrintData.Count > 0)
            {
                if (MessageDialog.ShowEnquiryMessage("此过程需要一段时间，是否继续？") == DialogResult.No)
                    return;

                List<View_S_InDepotGoodsBarCodeTable> lstBarCodeInfo = new List<View_S_InDepotGoodsBarCodeTable>();
                DateTime dt = ServerModule.ServerTime.Time;
                IBasicGoodsServer basicGoodsServer = ServerModuleFactory.GetServerModule<IBasicGoodsServer>();
                Dictionary<int, int> dicBarcode = new Dictionary<int, int>();

                try
                {
                    Cursor.Current = Cursors.WaitCursor;

                    foreach(var item in lstPrintData)
                    {
                        // 装配挑选回收件
                        string batchCode = "ZTJ" + dt.ToString("yyyyMMdd");
                        string goodsCode = item.零部件编码;
                        string goodsName = item.零部件名称;
                        string spec = item.规格;
                        string provider = item.供货单位;
                        string StorageID = "01";

                        View_S_InDepotGoodsBarCodeTable barcode = null;
                        S_InDepotGoodsBarCodeTable newBarcode = new S_InDepotGoodsBarCodeTable();

                        newBarcode.GoodsID = basicGoodsServer.GetGoodsID(goodsCode, goodsName, spec);

                        if (newBarcode.GoodsID == 0)
                        {
                            error.AppendLine("[" + goodsCode + "]" + "[" + goodsName + "]" + "[" + spec + "] 无物品ID");
                            continue;
                        }

                        newBarcode.Provider = provider;
                        newBarcode.BatchNo = batchCode;
                        newBarcode.ProductFlag = "0";
                        newBarcode.StorageID = StorageID;

                        barcode = m_barCodeServer.GetBarCodeInfo(goodsCode, goodsName, spec, provider, batchCode, StorageID);

                        if (barcode == null)
                        {
                            if (!m_barCodeServer.Add(newBarcode, out errorTemp))
                            {
                                MessageDialog.ShowErrorMessage(errorTemp);
                                return;
                            }
                        }

                        barcode = m_barCodeServer.GetBarCodeInfo(goodsCode, goodsName, spec, provider, batchCode, StorageID);
                        barcode.物品ID = item.数量;

                        dicBarcode.Add(barcode.条形码, item.数量);

                        if (barcode.工位 != null)
                        {
                            string[] workBench = barcode.工位.Split(new char[] { ',' });

                            if (workBench.Length == 1)
                            {
                                lstBarCodeInfo.Add(barcode);
                            }
                            else
                            {
                                foreach (var wb in workBench)
                                {
                                    View_S_InDepotGoodsBarCodeTable barCode =
                                        GlobalObject.CloneObject.CloneProperties<View_S_InDepotGoodsBarCodeTable>(barcode);

                                    barCode.工位 = wb;
                                    lstBarCodeInfo.Add(barCode);
                                }
                            }
                        }
                        else
                        {
                            barcode.工位 = "";
                            lstBarCodeInfo.Add(barcode);
                        }
                    }

                    lstBarCodeInfo.Sort(this.SortMethod);

                    foreach (var barcode in lstBarCodeInfo)
                    {
                        // barcode.物品ID 中保存的为数量
                        ServerModule.PrintPartBarcode.PrintBarcodeList(barcode, barcode.物品ID);
                    }

                    if (MessageDialog.ShowEnquiryMessage("是否将当前打印物品数量增加到多批次管理信息中？") == DialogResult.Yes)
                    {
                        IMultiBatchPartServer multiBatchPartServer = ServerModuleFactory.GetServerModule<IMultiBatchPartServer>();

                        if (!multiBatchPartServer.AddFromReparativePartList(GlobalObject.BasicInfo.LoginID, 
                                                                            form.PurposeID, dicBarcode, out m_err))
                        {
                            MessageDialog.ShowErrorMessage(m_err);
                        }
                    }
                }
                catch (Exception exce)
                {
                    MessageDialog.ShowErrorMessage(exce.Message);
                }
                finally
                {
                    Cursor.Current = Cursors.Arrow;
                }
            }

            if (error.Length > 0)
            {
                MessageDialog.ShowErrorMessage(error.ToString());
            }
        }

        /// <summary>
        /// 刷新数据显示控件
        /// </summary>
        /// <param name="dt">数据源</param>
        private void RefreshDataGridView(DataTable dt)
        {
            dataGridView1.DataSource = dt;
            dataGridView1.Refresh();

            if (dataGridView1.Rows.Count > 0)
            {
                dataGridView1.Columns[6].Visible = false;
            }

            // 添加数据定位控件
            if (m_dataLocalizer == null)
            {
                m_dataLocalizer = new UserControlDataLocalizer(dataGridView1, this.Name, 
                    UniversalFunction.SelectHideFields(this.Name, dataGridView1.Name, BasicInfo.LoginID));

                panelTop.Controls.Add(m_dataLocalizer);
                m_dataLocalizer.Dock = DockStyle.Bottom;
                m_dataLocalizer.Visible = true;

                this.dataGridView1.ColumnWidthChanged -= new System.Windows.Forms.DataGridViewColumnEventHandler(
                    this.dataGridView1_ColumnWidthChanged);

                ColumnWidthControl.SetDataGridView(labelTitle.Text, dataGridView1);

                this.dataGridView1.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(
                    this.dataGridView1_ColumnWidthChanged);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            FormFindCondition formFindCondition = new FormFindCondition(m_lstFindField.ToArray());

            if (formFindCondition.ShowDialog() != DialogResult.OK)
                return;

            if (!m_barCodeServer.GetBarCodeInfo(out m_returnBill, formFindCondition.SearchSQL, out m_err))
            {
                MessageDialog.ShowErrorMessage(m_err);
                return;
            }

            RefreshDataGridView(m_returnBill.DataCollection.Tables[0]);
        }

        /// <summary>
        /// 出厂条形码打印管理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnManageProductBarcode_Click(object sender, EventArgs e)
        {
            CVT出厂条形码打印管理 form = new CVT出厂条形码打印管理(m_authorityFlag);
            form.Show();
        }

        private void btnPrintBoardCode_Click(object sender, EventArgs e)
        {
            看板条形码打印 frm = new 看板条形码打印();
            frm.ShowDialog();
        }
    }
}

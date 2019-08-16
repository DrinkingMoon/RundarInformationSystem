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
using System.Collections;
using GlobalObject;

namespace Expression
{
    public partial class CVT终检信息查询 : Form
    {
        /// <summary>
        /// CVT出厂检验记录管理服务
        /// </summary>
        IProductDeliveryInspectionServer m_serverDeliveryInSpection = ServerModuleFactory.GetServerModule<IProductDeliveryInspectionServer>();

        /// <summary>
        /// 错误信息
        /// </summary>
        string m_strError = "";

        /// <summary>
        /// 服务组件
        /// </summary>
        IProductInfoServer m_productInfoServer = ServerModuleFactory.GetServerModule<IProductInfoServer>();

        public CVT终检信息查询()
        {
            InitializeComponent();

            #region 获取产品类型
            IQueryable<View_P_ProductInfo> productInfo = null;

            if (!m_productInfoServer.GetAllProductInfo(out productInfo, out m_strError))
            {
                MessageDialog.ShowErrorMessage(m_strError);
            }
            else
            {
                foreach (var item in productInfo)
                {
                    if (!item.是否返修专用)
                    {
                        cmbProductType.Items.Add(item.产品类型编码);
                    }
                }
            }

            cmbProductType.SelectedIndex = -1;
            #endregion
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                Hashtable hsTable = new Hashtable();

                hsTable.Add("@ProductCode", txtProductCode.Text.Trim());
                hsTable.Add("@ProductType", cmbProductType.Text);

                DataTable tempTable = 
                    GlobalObject.DatabaseServer.QueryInfoPro("CVTFinalInspection_Select", hsTable, out m_strError);

                DataTable sourceTable = (DataTable)customDataGridView1.DataSource;
                DataRow tempRow = sourceTable.NewRow();

                tempRow["型号"] = cmbProductType.Text;
                tempRow["箱号"] = txtProductCode.Text.Trim();
                tempRow["终检人"] = BasicInfo.LoginName;

                if (tempTable == null || tempTable.Rows.Count == 0)
                {
                    tempRow["下线试验信息"] = "";
                    tempRow["审核时间"] = "";
                    tempRow["称重信息"] = "";
                    tempRow["称重时间"] = "";
                    tempRow["气密性信息"] = "";
                    tempRow["检测时间"] = "";
                }
                else
                {
                    bool isReapt = false;

                    if (sourceTable != null)
                    {
                        for (int i = 0; i < sourceTable.Rows.Count; i++)
                        {
                            if (sourceTable.Rows[i]["型号"].ToString() == tempTable.Rows[0]["型号"].ToString()
                                && sourceTable.Rows[i]["箱号"].ToString() == tempTable.Rows[0]["箱号"].ToString())
                            {
                                isReapt = true;
                                return;
                            }
                        }
                    }

                    if (!isReapt)
                    {
                        tempRow["下线试验信息"] = tempTable.Rows[0]["下线试验信息"];
                        tempRow["审核时间"] = tempTable.Rows[0]["审核时间"];
                        tempRow["称重信息"] = tempTable.Rows[0]["称重信息"];
                        tempRow["称重时间"] = tempTable.Rows[0]["称重时间"];
                        tempRow["气密性信息"] = tempTable.Rows[0]["气密性信息"];
                        tempRow["检测时间"] = tempTable.Rows[0]["检测时间"];
                    }
                }

                sourceTable.Rows.Add(tempRow);

                ZL_CVTFinalInspection tempLnq = new ZL_CVTFinalInspection();

                tempLnq.ProductCode = txtProductCode.Text.Trim();
                tempLnq.ProductType = cmbProductType.Text;
                tempLnq.SelectDate = ServerTime.Time;

                m_serverDeliveryInSpection.AddCVTFinalInspectionInfo(tempLnq);

                customDataGridView1.DataSource = sourceTable;
                
            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (customDataGridView1.CurrentRow != null)
            {
                DateTime startTime = ServerTime.StartTime(dtpStart.Value);
                DateTime endTime = ServerTime.EndTime(dtpEnd.Value);

                ZL_CVTFinalInspection tempLnq = new ZL_CVTFinalInspection();

                tempLnq.ProductCode = customDataGridView1.CurrentRow.Cells["箱号"].Value.ToString();
                tempLnq.ProductType = customDataGridView1.CurrentRow.Cells["型号"].Value.ToString();
                tempLnq.SelectDate = ServerTime.Time;
                tempLnq.WorkID = BasicInfo.LoginID;

                m_serverDeliveryInSpection.DeleteCVTFinalInspectionInfo(startTime, endTime, tempLnq);

                customDataGridView1.Rows.Remove(customDataGridView1.CurrentRow);
            }

        }

        private void btnOutPut_Click(object sender, EventArgs e)
        {
            ExcelHelperP.DatagridviewToExcel(saveFileDialog1, customDataGridView1);
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            DateTime startTime = ServerTime.StartTime(dtpStart.Value);
            DateTime endTime = ServerTime.EndTime(dtpEnd.Value);

            DataTable tempTable = m_serverDeliveryInSpection.SelectFinalInspectionList(startTime, endTime);
            customDataGridView1.DataSource = tempTable;
        }

        private void CVT终检信息查询_Load(object sender, EventArgs e)
        {
            DataTable tempTable = m_serverDeliveryInSpection.SelectFinalInspectionList(null, null);
            customDataGridView1.DataSource = tempTable;
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == tabPage1)
            {
                DataTable tempTable = m_serverDeliveryInSpection.SelectFinalInspectionList(null, null);
                customDataGridView1.DataSource = tempTable;
            }
            else
            {
                customDataGridView1.DataSource = null;
            }
        }
    }
}

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
using UniversalControlLibrary;
using Service_Economic_Purchase;
namespace Form_Economic_Purchase
{
    public partial class 价格清单 : Form
    {
        IUnitPriceList _Service_UnitList = Service_Economic_Purchase.ServerModuleFactory.GetServerModule<IUnitPriceList>();

        public 价格清单()
        {
            InitializeComponent();
        }

        void DataGridViewShow()
        {
            DataTable tempTable = _Service_UnitList.GetList();

            customDataGridView1.DataSource = tempTable;
            userControlDataLocalizer1.Init(customDataGridView1, this.Name, null);
        }

        private void txtGoodsCode_OnCompleteSearch()
        {
            if (txtGoodsCode.DataResult == null)
            {
                return;
            }

            txtGoodsCode.Text = txtGoodsCode.DataResult["图号型号"].ToString();
            txtGoodsName.Text = txtGoodsCode.DataResult["物品名称"].ToString();
            txtSpec.Text = txtGoodsCode.DataResult["规格"].ToString();

            txtGoodsCode.Tag = txtGoodsCode.DataResult["序号"];
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                List<Bus_PurchasingMG_UnitPriceList> lstTemp = new List<Bus_PurchasingMG_UnitPriceList>();

                Bus_PurchasingMG_UnitPriceList tempInfo = new Bus_PurchasingMG_UnitPriceList();

                tempInfo.GoodsID = Convert.ToInt32(txtGoodsCode.Tag);
                tempInfo.Provider = txtProvider.Text;
                tempInfo.Rate = Convert.ToInt32(numRate.Value);
                tempInfo.UnitPrice = numUnitPrice.Value;
                tempInfo.ValidityStart = dtpStart.Value.Date;
                tempInfo.ValidityEnd = dtpEnd.Value.Date;

                lstTemp.Add(tempInfo);

                _Service_UnitList.SaveInfo(lstTemp);
                MessageDialog.ShowPromptMessage("保存成功");
                DataGridViewShow();
            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
            }
        }

        private void 价格清单_Load(object sender, EventArgs e)
        {
            DataGridViewShow();
        }

        private void btnInput_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dtTemp = ExcelHelperP.RenderFromExcel(openFileDialog1);

                if (dtTemp == null)
                {
                    return;
                }

                if (!dtTemp.Columns.Contains("图号型号"))
                {
                    throw new Exception("文件无【图号型号】列名");
                }

                if (!dtTemp.Columns.Contains("物品名称"))
                {
                    throw new Exception("文件无【物品名称】列名");
                }

                if (!dtTemp.Columns.Contains("规格"))
                {
                    throw new Exception("文件无【规格】列名");
                }

                if (!dtTemp.Columns.Contains("供应商"))
                {
                    throw new Exception("文件无【供应商】列名");
                }

                if (!dtTemp.Columns.Contains("单价"))
                {
                    throw new Exception("文件无【单价】列名");
                }

                if (!dtTemp.Columns.Contains("税率"))
                {
                    throw new Exception("文件无【税率】列名");
                }

                if (!dtTemp.Columns.Contains("有效期开始时间"))
                {
                    throw new Exception("文件无【有效期开始时间】列名");
                }

                if (!dtTemp.Columns.Contains("有效期结束时间"))
                {
                    throw new Exception("文件无【有效期结束时间】列名");
                }


                List<Bus_PurchasingMG_UnitPriceList> lstTemp = new List<Bus_PurchasingMG_UnitPriceList>();

                foreach (DataRow dr in dtTemp.Rows)
                {

                    if ((dr["图号型号"] == null || GlobalObject.GeneralFunction.IsNullOrEmpty(dr["图号型号"].ToString()))
                        && (dr["物品名称"] == null || GlobalObject.GeneralFunction.IsNullOrEmpty(dr["物品名称"].ToString()))
                        && (dr["规格"] == null || GlobalObject.GeneralFunction.IsNullOrEmpty(dr["规格"].ToString())))
                    {
                        continue;
                    }

                    Bus_PurchasingMG_UnitPriceList tempInfo = new Bus_PurchasingMG_UnitPriceList();

                    string goodsMessage = "【图号型号】:" + (dr["图号型号"] == null ? "" : dr["图号型号"].ToString().Trim()).ToString()
                            + "【物品名称】：" + (dr["物品名称"] == null ? "" : dr["物品名称"].ToString().Trim()).ToString()
                            + "【规格】：" + (dr["规格"] == null ? "" : dr["规格"].ToString().Trim()).ToString();

                    View_F_GoodsPlanCost goodsInfo = UniversalFunction.GetGoodsInfo(dr["图号型号"] == null ? "" : dr["图号型号"].ToString().Trim(),
                        dr["物品名称"] == null ? "" : dr["物品名称"].ToString().Trim(),
                        dr["规格"] == null ? "" : dr["规格"].ToString().Trim());

                    if (goodsInfo == null)
                    {
                        throw new Exception("未找到对应的系统物品：" + goodsMessage);
                    }

                    //if (goodsInfo.禁用)
                    //{
                    //    throw new Exception("此物品已被禁用无法导入：" + goodsMessage);
                    //}

                    tempInfo.GoodsID = goodsInfo.序号;

                    string strProvicer = dr["供应商"] == null ? "" : dr["供应商"].ToString();
                    View_Provider provider = UniversalFunction.GetProviderInfo(strProvicer);

                    if (provider == null)
                    {
                        throw new Exception("未找到对应的系统供应商：" + strProvicer);
                    }

                    if (!provider.是否在用)
                    {
                        throw new Exception("【系统供应商】：" + strProvicer + " 被禁用，无法导入");
                    }

                    tempInfo.Provider = provider.供应商编码;
                    tempInfo.Rate = dr["税率"] == null || dr["税率"].ToString().Trim() == "" ? 0 : Convert.ToInt32(dr["税率"]);
                    tempInfo.UnitPrice = dr["单价"] == null || dr["单价"].ToString().Trim() == "" ? 0 : Convert.ToDecimal(dr["单价"]);

                    if (dr["有效期开始时间"] == null || dr["有效期开始时间"].ToString().Trim() == "")
                    {
                        throw new Exception("有效期开始时间有误：" + goodsMessage);
                    }
                    else
                    {
                        tempInfo.ValidityStart = Convert.ToDateTime(dr["有效期开始时间"]).Date;
                    }

                    if (dr["有效期结束时间"] == null || dr["有效期结束时间"].ToString().Trim() == "")
                    {
                        throw new Exception("有效期结束时间有误：" + goodsMessage);
                    }
                    else
                    {
                        tempInfo.ValidityEnd = Convert.ToDateTime(dr["有效期结束时间"]).Date;
                    }

                    lstTemp.Add(tempInfo);
                }

                _Service_UnitList.SaveInfo(lstTemp);
                MessageDialog.ShowPromptMessage("导入并保存成功");
                DataGridViewShow();

            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
            }

        }

        private void btnOutput_Click(object sender, EventArgs e)
        {
            try
            {
                ExcelHelperP.DataTableToExcel(saveFileDialog1, _Service_UnitList.GetMaxVersionNoInfo(), null);
            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
            }
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            FormDataShow frm = new FormDataShow(_Service_UnitList.GetList_History());
            frm.Show();
        }

        private void customDataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (customDataGridView1.CurrentRow == null)
            {
                return;
            }

            txtGoodsCode.Tag = customDataGridView1.CurrentRow.Cells["GoodsID"].Value;

            txtGoodsCode.Text = customDataGridView1.CurrentRow.Cells["图号型号"].Value.ToString();
            txtGoodsName.Text = customDataGridView1.CurrentRow.Cells["物品名称"].Value.ToString();
            txtSpec.Text = customDataGridView1.CurrentRow.Cells["规格"].Value.ToString();
            txtProvider.Text = customDataGridView1.CurrentRow.Cells["供应商"].Value.ToString();

            numRate.Value = Convert.ToDecimal( customDataGridView1.CurrentRow.Cells["税率"].Value.ToString());
            numUnitPrice.Value = Convert.ToDecimal(customDataGridView1.CurrentRow.Cells["单价"].Value.ToString());

            dtpStart.Value = Convert.ToDateTime(customDataGridView1.CurrentRow.Cells["有效期开始时间"].Value.ToString());
            dtpEnd.Value = Convert.ToDateTime(customDataGridView1.CurrentRow.Cells["有效期结束时间"].Value.ToString());

        }

        private void txtGoodsCode_Enter(object sender, EventArgs e)
        {
            txtGoodsCode.StrEndSql = " and 禁用 = 0 ";
        }
    }
}

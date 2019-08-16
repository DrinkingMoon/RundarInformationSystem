using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ServerModule;
using UniversalControlLibrary;
using Service_Manufacture_WorkShop;
using GlobalObject;

namespace Form_Manufacture_WorkShop
{
    public partial class 车间在产 : Form
    {
        IInProductReport _Service_InProcutReport = Service_Manufacture_WorkShop.ServerModuleFactory.GetServerModule<IInProductReport>();

        public 车间在产()
        {
            InitializeComponent();
        }

        private void btnSelect_InProduct_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbYear.Text.Trim().Length == 0)
                {
                    throw new Exception("请选择【年份】");
                }

                if (cmbMonth.Text.Trim().Length == 0)
                {
                    throw new Exception("请选择【月份】");
                }

                foreach (Control cl in tcInProduct.SelectedTab.Controls)
                {
                    if (cl is CustomDataGridView)
                    {
                        ((CustomDataGridView)cl).DataSource =
                            _Service_InProcutReport.GetAllInProductInfo(tcInProduct.SelectedTab.Name, cmbYear.Text + cmbMonth.Text);

                        foreach (Control clusdl in tcInProduct.SelectedTab.Controls)
                        {
                            if (clusdl is UniversalControlLibrary.UserControlDataLocalizer)
                            {
                                ((UniversalControlLibrary.UserControlDataLocalizer)clusdl).Init((CustomDataGridView)cl, this.Name, null);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
            }
        }

        private void tcInProduct_SelectedIndexChanged(object sender, EventArgs e)
        {
            //btnSelect_InProduct_Click(null, null);
        }

        private void btnOutput_InProduct_Click(object sender, EventArgs e)
        {
            foreach (Control cl in tcInProduct.SelectedTab.Controls)
            {
                if (cl is TabControl)
                {
                    foreach (Control clSon in ((TabControl)cl).SelectedTab.Controls)
                    {
                        if (clSon is CustomDataGridView)
                        {
                            ExcelHelperP.DatagridviewToExcel(saveFileDialog1, (DataGridView)clSon);
                        }
                    }
                }
                else if (cl is CustomDataGridView)
                {
                    ExcelHelperP.DatagridviewToExcel(saveFileDialog1, (DataGridView)cl);
                }
            }
        }

        private void btnCreate_InProduct_Click(object sender, EventArgs e)
        {
            生成盘点辅助表 frm = new 生成盘点辅助表();

            if (frm.ShowDialog() == DialogResult.OK)
            {
                MessageDialog.ShowPromptMessage("生成成功");
            }
        }

        private void btnOutput_BOM_Click(object sender, EventArgs e)
        {
            ExcelHelperP.DatagridviewToExcel(saveFileDialog1, dgvbom);
        }

        private void btnSelect_BOM_Click(object sender, EventArgs e)
        {
            if (txtGoodsCode_BOM.Tag == null || txtGoodsCode_BOM.Text.Trim().Length == 0)
            {
                throw new Exception("请选择【总成/分总成】");
            }

            dgvbom.DataSource = _Service_InProcutReport.GetBom(Convert.ToInt32(txtGoodsCode_BOM.Tag));
            usdlbom.Init(dgvbom, this.Name, null);
        }

        private void btnInput_BOM_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtGoodsCode_BOM.Tag == null || txtGoodsCode_BOM.Text.Trim().Length == 0)
                {
                    throw new Exception("请选择【总成/分总成】");
                }

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

                if (!dtTemp.Columns.Contains("用量"))
                {
                    throw new Exception("文件无【用量】列名");
                }

                List<Bus_WorkShop_InProduct_Bom> lstInfo = new List<Bus_WorkShop_InProduct_Bom>();

                for (int i = 0; i < dtTemp.Rows.Count; i++)
                {
                    int goodsID = UniversalFunction.GetGoodsID(dtTemp.Rows[i]["图号型号"].ToString(), 
                        dtTemp.Rows[i]["物品名称"].ToString(), dtTemp.Rows[i]["规格"].ToString());

                    if (goodsID == 0)
                    {
                        throw new Exception(string.Format("第【{0}】行，图号型号：{1}，物品名称：{2}，规格：{3}，无法找到匹配的物品ID", i.ToString(),
                            dtTemp.Rows[i]["图号型号"].ToString(), dtTemp.Rows[i]["物品名称"].ToString(), dtTemp.Rows[i]["规格"].ToString()));
                    }

                    Bus_WorkShop_InProduct_Bom bom = new Bus_WorkShop_InProduct_Bom();
                    bom.F_Id = Guid.NewGuid().ToString();
                    bom.GoodsID = goodsID;
                    bom.ProductGoodsID = Convert.ToInt32(txtGoodsCode_BOM.Tag);

                    decimal goodsCount = 0;
                    Decimal.TryParse(dtTemp.Rows[i]["用量"] == null ? "" : dtTemp.Rows[i]["用量"].ToString(), out goodsCount);
                    bom.UseCount = goodsCount;

                    lstInfo.Add(bom);
                }

                _Service_InProcutReport.InputBom(lstInfo);
                MessageDialog.ShowPromptMessage("导入成功");
                btnSelect_BOM_Click(null, null);

            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
                return;
            }
        }

        private void btnSupplementary_InProduct_Click(object sender, EventArgs e)
        {
            try
            {
                string yearMonth = "";

                选择年月 frm = new 选择年月();

                if (frm.ShowDialog() == DialogResult.OK)
                {
                    yearMonth = frm._Str_YearMonth;
                }
                else
                {
                    return;
                }

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

                if (!dtTemp.Columns.Contains("实盘数"))
                {
                    throw new Exception("文件无【实盘数】列名");
                }

                List<Bus_WorkShop_InProduct> lstInfo = new List<Bus_WorkShop_InProduct>();

                for (int i = 0; i < dtTemp.Rows.Count; i++)
                {
                    int goodsID = UniversalFunction.GetGoodsID(dtTemp.Rows[i]["图号型号"].ToString(),
                        dtTemp.Rows[i]["物品名称"].ToString(), dtTemp.Rows[i]["规格"].ToString());

                    if (goodsID == 0)
                    {
                        throw new Exception(string.Format("第【{0}】行，图号型号：{1}，物品名称：{2}，规格：{3}，无法找到匹配的物品ID", i.ToString(),
                            dtTemp.Rows[i]["图号型号"].ToString(), dtTemp.Rows[i]["物品名称"].ToString(), dtTemp.Rows[i]["规格"].ToString()));
                    }

                    Bus_WorkShop_InProduct tempInfo = new Bus_WorkShop_InProduct();
                    tempInfo.F_Id = Guid.NewGuid().ToString();
                    tempInfo.GoodsID = goodsID;
                    tempInfo.YearMonth = yearMonth;

                    decimal goodsCount = 0;
                    Decimal.TryParse(dtTemp.Rows[i]["实盘数"] == null ? "" : dtTemp.Rows[i]["实盘数"].ToString(), out goodsCount);
                    tempInfo.NowBalanceCount = goodsCount;

                    lstInfo.Add(tempInfo);
                }

                _Service_InProcutReport.InputNowBalanceCount(lstInfo);
                MessageDialog.ShowPromptMessage("导入成功");
            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
                return;
            }
        }

        private void 车间在产_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < 10; i++)
            {
                cmbYear.Items.Add(ServerTime.Time.Year - i);
            }

            cmbYear.Text = ServerTime.Time.Year.ToString();
            cmbMonth.Text = ServerTime.Time.Month.ToString("D2");
        }

        private void btnResolve_InProduct_Click(object sender, EventArgs e)
        {
            try
            {
                string yearMonth = "";

                选择年月 frm = new 选择年月();

                if (frm.ShowDialog() == DialogResult.OK)
                {
                    yearMonth = frm._Str_YearMonth;
                }
                else
                {
                    return;
                }

                //DataTable tempTable = _Service_InProcutReport.GetAllInProductInfo(零件供应商耗用报表.Name, yearMonth);

                //if (tempTable != null && tempTable.Rows.Count > 0)
                //{
                //    if (MessageDialog.ShowEnquiryMessage("年月：【"+ yearMonth +"】,已存在记录，重复导入"))
                //    {
                        
                //    }
                //}

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

                if (!dtTemp.Columns.Contains("耗用数"))
                {
                    throw new Exception("文件无【耗用数】列名");
                }

                if (!dtTemp.Columns.Contains("供应商"))
                {
                    throw new Exception("文件无【供应商】列名");
                }

                List<Bus_WorkShop_InProduct_Resolve> lstInfo = new List<Bus_WorkShop_InProduct_Resolve>();

                for (int i = 0; i < dtTemp.Rows.Count; i++)
                {
                    int goodsID = UniversalFunction.GetGoodsID(dtTemp.Rows[i]["图号型号"].ToString(),
                        dtTemp.Rows[i]["物品名称"].ToString(), dtTemp.Rows[i]["规格"].ToString());

                    if (goodsID == 0)
                    {
                        throw new Exception(string.Format("第【{0}】行，图号型号：{1}，物品名称：{2}，规格：{3}，无法找到匹配的物品ID", i.ToString(),
                            dtTemp.Rows[i]["图号型号"].ToString(), dtTemp.Rows[i]["物品名称"].ToString(), dtTemp.Rows[i]["规格"].ToString()));
                    }

                    Bus_WorkShop_InProduct_Resolve tempInfo = new Bus_WorkShop_InProduct_Resolve();
                    tempInfo.F_Id = Guid.NewGuid().ToString();
                    tempInfo.GoodsID = goodsID;
                    tempInfo.Provider = dtTemp.Rows[i]["供应商"].ToString();
                    tempInfo.YearMonth = yearMonth;

                    decimal goodsCount = 0;
                    Decimal.TryParse(dtTemp.Rows[i]["耗用数"] == null ? "" : dtTemp.Rows[i]["耗用数"].ToString(), out goodsCount);
                    tempInfo.GoodsCount = goodsCount;

                    lstInfo.Add(tempInfo);
                }

                _Service_InProcutReport.InputResolveInfo(lstInfo);
                MessageDialog.ShowPromptMessage("导入成功");
            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
                return;
            }
        }

        private void txtGoodsCode_BOM_OnCompleteSearch()
        {
            if (txtGoodsCode_BOM.DataResult == null)
            {
                return;
            }

            txtGoodsCode_BOM.Text = txtGoodsCode_BOM.DataResult["图号型号"].ToString();
            txtGoodsName_BOM.Text = txtGoodsCode_BOM.DataResult["物品名称"].ToString();
            txtSpec_BOM.Text = txtGoodsCode_BOM.DataResult["规格"].ToString();

            txtGoodsCode_BOM.Tag = Convert.ToInt32(txtGoodsCode_BOM.DataResult["序号"]);
        }
    }
}

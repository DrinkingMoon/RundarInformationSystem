/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  UserControlPurchaseStore.cs
 * 作者    :  夏石友    版本: v1.00    日期: 2009/06/15
 * 开发平台:  vs2005(c#)
 * 用于    :  生产线管理信息系统
 *----------------------------------------------------------------------------
 * 描述 : 供应商信息界面
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
using GlobalObject;
using PlatformManagement;
using UniversalControlLibrary;
using System.Collections;

namespace Expression
{
    /// <summary>
    /// 货物库存组件
    /// </summary>
    public partial class UserControlPurchaseStore : Form
    {
        string _SourceType, _YearMonth, _StorageID, _GoodsStatus, _SumType, _Product, _SelectType;

        public UserControlPurchaseStore(FunctionTreeNodeInfo nodeInfo)
        {
            InitializeComponent();

            for (int i = 2012; i <= ServerTime.Time.Year; i++)
            {
                cmbYear.Items.Add(i.ToString());
            }
        }
        
        private void UserControlPurchaseStore_Load(object sender, EventArgs e)
        {
            cmbSumType.SelectedIndex = 0;
            cmbSource.SelectedIndex = 0;
        }

        private void cmbSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbSource.Text == "当前库存")
            {
                cmbYear.Enabled = false;
                cmbMonth.Enabled = false;
            }
            else
            {
                cmbYear.Enabled = true;
                cmbMonth.Enabled = true;
            }
        }

        private void cmbSumType_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtProduct.Text = "";
            txtProduct.Tag = null;

            switch (cmbSumType.Text)
            {
                case "库存信息":
                    txtProduct.Enabled = false;
                    break;
                case "采购BOM":
                    txtProduct.Enabled = true;
                    txtProduct.FindItem = TextBoxShow.FindType.采购BOM产品清单;
                    break;
                case "装配BOM":
                    txtProduct.Enabled = true;
                    txtProduct.FindItem = TextBoxShow.FindType.装配BOM产品清单;
                    break;
                case "设计BOM":
                    txtProduct.Enabled = true;
                    txtProduct.FindItem = TextBoxShow.FindType.设计BOM产品清单;
                    break;
                case "发料清单":
                    txtProduct.Enabled = true;
                    txtProduct.FindItem = TextBoxShow.FindType.发料清单产品清单;
                    break;
                default:
                    break;
            }
        }

        private void txtStorage_OnCompleteSearch()
        {
            if (txtStorage.DataTableResult == null || txtStorage.DataTableResult.Rows.Count == 0)
            {
                return;
            }

            txtStorage.Text = "";
            txtStorage.Tag = "";

            foreach (DataRow dr in txtStorage.DataTableResult.Rows)
            {
                txtStorage.Text += dr["库房名称"].ToString() + ",";
                txtStorage.Tag += "'" + dr["库房编码"].ToString() + "',";
            }

            txtStorage.Text = txtStorage.Text.Substring(0, txtStorage.Text.Length - 1);
            txtStorage.Tag = txtStorage.Tag.ToString().Substring(0, txtStorage.Tag.ToString().Length - 1);
        }

        private void txtProduct_OnCompleteSearch()
        {
            if (txtProduct.DataTableResult == null || txtProduct.DataTableResult.Rows.Count == 0)
            {
                return;
            }

            txtProduct.Text = "";
            txtProduct.Tag = "";

            foreach (DataRow dr in txtProduct.DataTableResult.Rows)
            {
                txtProduct.Text += dr["图号型号"].ToString() + ",";
                txtProduct.Tag += dr["物品ID"].ToString() + ",";
            }

            txtProduct.Text = txtProduct.Text.Substring(0, txtProduct.Text.Length - 1);
            txtProduct.Tag = txtProduct.Tag.ToString().Substring(0, txtProduct.Tag.ToString().Length - 1);
        }

        void GetCondition()
        {
            try
            {
                foreach (Control cl in customGroupBox1.Controls)
                {
                    if (cl is RadioButton)
                    {
                        if (((RadioButton)cl).Checked)
                        {
                            _SelectType = ((RadioButton)cl).Text;
                        }
                    }
                }

                if (GlobalObject.GeneralFunction.IsNullOrEmpty(cmbSource.Text))
                {
                    throw new Exception("请选择【数据来源】");
                }

                if (cmbSource.Text == "月度库存")
                {
                    if (GlobalObject.GeneralFunction.IsNullOrEmpty(cmbYear.Text) || GlobalObject.GeneralFunction.IsNullOrEmpty(cmbMonth.Text))
                    {
                        throw new Exception("请选择【年份】、【月份】");
                    }
                }

                _SourceType = cmbSource.Text;
                _YearMonth = cmbYear.Text + cmbMonth.Text;

                if (GlobalObject.GeneralFunction.IsNullOrEmpty(txtStorage.Text))
                {
                    throw new Exception("请选择【所属库房】");
                }

                _StorageID = txtStorage.Tag.ToString();

                _GoodsStatus = "";

                foreach (Control cl in this.customGroupBox1.Controls)
                {
                    if (cl is CheckBox)
                    {
                        if (((CheckBox)cl).Tag != null && ((CheckBox)cl).Checked)
                        {
                            if (rbMx.Checked)
                            {
                                _GoodsStatus += "'" + ((CheckBox)cl).Text.ToString() + "',";
                            }
                            else
                            {
                                _GoodsStatus += ((CheckBox)cl).Tag.ToString() + ",";
                            }
                        }
                    }
                }

                _GoodsStatus = _GoodsStatus.Substring(0, _GoodsStatus.Length - 1);

                if (GlobalObject.GeneralFunction.IsNullOrEmpty(cmbSumType.Text))
                {
                    throw new Exception("请选择【汇总方式】");
                }

                _SumType = cmbSumType.Text;

                if (_SumType != "库房信息" && GlobalObject.GeneralFunction.IsNullOrEmpty(txtProduct.Text))
                {
                    throw new Exception("请选择【产品类型】");
                }

                _Product = txtProduct.Tag == null ? "" : txtProduct.Tag.ToString();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            try
            {
                string error = null;

                GetCondition();

                Hashtable hsTable = new Hashtable();

                hsTable.Add("@SourceType", _SourceType);
                hsTable.Add("@YearMonth", _YearMonth);
                hsTable.Add("@StorageID", _StorageID);
                hsTable.Add("@GoodsStatus", _GoodsStatus);
                hsTable.Add("@SumType", _SumType);
                hsTable.Add("@Product", _Product);
                hsTable.Add("@SelectType", _SelectType);

                DataTable tempTable = GlobalObject.DatabaseServer.QueryInfoPro("Select_SUMStock", hsTable, out error);

                customDataGridView1.DataSourceBinding.DataSource = tempTable;
            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
            }
        }

        private void btnOutput_Click(object sender, EventArgs e)
        {
            UniversalControlLibrary.ExcelHelperP.DataTableToExcel(saveFileDialog1, 
                (DataTable)customDataGridView1.DataSourceBinding.DataSource, null);
        }

        private void rbHz_CheckedChanged(object sender, EventArgs e)
        {
            if (rbHz.Checked)
            {
                cmbSumType.Enabled = true;
                txtProduct.Enabled = true;
            }
            else
            {
                cmbSumType.Enabled = false;
                txtProduct.Enabled = false;
            }
        }
    }
}

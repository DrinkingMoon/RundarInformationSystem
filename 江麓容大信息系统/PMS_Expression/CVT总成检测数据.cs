/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  CVT总成检测数据.cs
 * 作者    :  夏石友    版本: v1.00    日期: 2013/12/09
 * 开发平台:  vs2005(c#)
 * 用于    :  生产线管理信息系统
 *----------------------------------------------------------------------------
 * 描述 : 供显示及查询CVT总成检测数据，数据来源从CVT下线试验台
 * 其它 :
 *----------------------------------------------------------------------------
 * 历史记录:
 *     1. 日期时间: 2013/12/09 16:02:08 作者: 夏石友 当前版本: V1.00
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
using GlobalObject;
using PlatformManagement;
using UniversalControlLibrary;
using Service_Peripheral_HR;

namespace Expression
{
    public partial class CVT总成检测数据 : Form
    {
        /// <summary>
        /// CVT检测数据服务接口
        /// </summary>
        ICvtCheckDataService m_cvtTestDataService = PMS_ServerFactory.GetServerModule<ICvtCheckDataService>();

        /// <summary>
        /// 授权标志
        /// </summary>
        AuthorityFlag m_authFlag;

        public CVT总成检测数据(FunctionTreeNodeInfo nodeInfo)
        {
            InitializeComponent();

            m_authFlag = nodeInfo.Authority;

            this.dateTimePickerST.Value = DateTime.Now.AddDays(-7).Date;
            this.dateTimePickerET.Value = DateTime.Now.Date;

            btnSearch_Click(null, null);

            this.userControlDataLocalizer1.Init(dataGridView1, this.Name,
                UniversalFunction.SelectHideFields(this.Name, dataGridView1.Name, BasicInfo.LoginID));
        }

        private void CVT总成检测数据_Load(object sender, EventArgs e)
        {
            FaceAuthoritySetting.SetEnable(this.Controls, m_authFlag);
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageDialog.ShowPromptMessage("请选择一条数据后再进行此操作");
                return;
            }

            报表_CVT总成检测数据 report = new 报表_CVT总成检测数据((Int64)dataGridView1.SelectedRows[0].Cells["序号"].Value);
            report.ShowDialog();
        }

        /// <summary>
        /// 日期检索
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            this.dateTimePickerST.Value = this.dateTimePickerST.Value.Date;
            this.dateTimePickerET.Value = this.dateTimePickerET.Value.Date.AddDays(1);

            this.dataGridView1.DataSource = m_cvtTestDataService.GetData(this.dateTimePickerST.Value, this.dateTimePickerET.Value);
        }

        private void btnSearchProduct_Click(object sender, EventArgs e)
        {
            if (GlobalObject.GeneralFunction.IsNullOrEmpty(txtNumber.Text.Trim()))
            {
                MessageDialog.ShowPromptMessage("请输入CVT编号后再进行此操作");
                return;
            }

            this.dataGridView1.DataSource = m_cvtTestDataService.GetData(this.txtNumber.Text);
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            btnPrint_Click(sender, e);
        }

        private void btnLoadExcel_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog1 = new OpenFileDialog();
                System.Data.DataTable dtTemp = ExcelHelperP.RenderFromExcel(openFileDialog1);

                if (dtTemp == null || dtTemp.Rows.Count == 0 || dtTemp.Columns.Count < 8 || dtTemp.Rows.Count != 39)
                {
                    MessageDialog.ShowPromptMessage("加载的EXCEL文件数据格式错误");
                    return;
                }

                string error = null;
                string productCode = dtTemp.Rows[0][3].ToString();
                string cvtNumber = dtTemp.Rows[0][6].ToString();

                if (!ServerModule.ServerModuleFactory.GetServerModule<IProductCodeServer>().VerifyProductCodesInfo(
                    productCode, cvtNumber, GlobalObject.CE_BarCodeType.内部钢印码, out error))
                {
                    MessageDialog.ShowErrorMessage(error);
                    return;
                }

                string 试验设备名称 = dtTemp.Rows[1][3].ToString();
                string 油品 = "出光1";
                string 试验结果 = dtTemp.Rows[37][5].ToString();

                IPersonnelArchiveServer personnelInfoServer = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<IPersonnelArchiveServer>();                           
    
                string 装配员工号 = personnelInfoServer.GetPersonnelViewInfoByName(dtTemp.Rows[38][4].ToString());

                if (GlobalObject.GeneralFunction.IsNullOrEmpty(装配员工号))
                {
                    MessageDialog.ShowPromptMessage("文件中的装配员姓名在公司不唯一，无法获取到员工编号");
                    return;
                }

                string 下线员工号 = personnelInfoServer.GetPersonnelViewInfoByName(dtTemp.Rows[38][7].ToString());

                if (GlobalObject.GeneralFunction.IsNullOrEmpty(下线员工号))
                {
                    MessageDialog.ShowPromptMessage("文件中的下线员姓名在公司不唯一，无法获取到员工编号");
                    return;
                }

                string remark = dtTemp.Rows[35][2].ToString();

                remark += string.Format("【工号：{0}, 姓名：{1}从EXCEL文件中导入】", BasicInfo.LoginID, BasicInfo.LoginName);

                DataTable dt = new DataTable();

                dt.Columns.Add("TestType");         // 试验类型，如：基本功能、N挡磨合等
                dt.Columns.Add("TestItemName");     // 试验项目，如：密封性、传动平稳
                dt.Columns.Add("TestCondition");    // 试验条件，如：整个试验过程中
                dt.Columns.Add("TestRequirement");  // 试验要求，如：总成各结合面、外表面及油封刃口处不得有渗透现象
                dt.Columns.Add("TestData");         // 试验数据，如：0.5Mpa
                dt.Columns.Add("TestResult");       // 试验结论，如：合格

                string 检测项目 = "";
                string 试验项目 = "";
                string 试验条件 = "";
                string 试验要求 = "";
                string 检测数据 = "";
                string 试验项目测试结果 = "";
                string 试验项目备份 = "";

                for (int i = 3; i < 35; i++)
                {
                    if (!GlobalObject.GeneralFunction.IsNullOrEmpty(dtTemp.Rows[i][1].ToString()))
                    {
                        检测项目 = dtTemp.Rows[i][1].ToString();
                        试验项目 = "";
                        试验项目备份 = "";
                    }

                    if (!GlobalObject.GeneralFunction.IsNullOrEmpty(dtTemp.Rows[i][2].ToString()))
                    {
                        试验项目备份 = dtTemp.Rows[i][2].ToString();

                        if (!GlobalObject.GeneralFunction.IsNullOrEmpty(dtTemp.Rows[i][3].ToString()))
                        {
                            试验项目 = dtTemp.Rows[i][2].ToString() + "-" + dtTemp.Rows[i][3].ToString();
                        }
                        else
                        {
                            试验项目 = dtTemp.Rows[i][2].ToString();
                        }

                        试验条件 = "";
                    }
                    else if (!GlobalObject.GeneralFunction.IsNullOrEmpty(dtTemp.Rows[i][3].ToString()))
                    {
                        试验项目 = 试验项目备份 + "-" + dtTemp.Rows[i][3].ToString();
                        试验条件 = "";
                    }

                    if (!GlobalObject.GeneralFunction.IsNullOrEmpty(dtTemp.Rows[i][4].ToString()))
                    {
                        试验条件 = dtTemp.Rows[i][4].ToString();    
                    }
                    
                    试验要求 = dtTemp.Rows[i][5].ToString();
                    检测数据 = dtTemp.Rows[i][6].ToString();
                    试验项目测试结果 = dtTemp.Rows[i][7].ToString();

                    dt.Rows.Add(new object[] { 检测项目, 试验项目, 试验条件, 试验要求, 检测数据, 试验项目测试结果 });
                }

                CvtTestInfo testInfo = new CvtTestInfo(
                    productCode + " " + cvtNumber, 试验设备名称, 油品, 试验结果, 装配员工号, 下线员工号, remark, dt);

                m_cvtTestDataService.SaveCVTExpData(testInfo);

                MessageDialog.ShowPromptMessage(string.Format("导入【{0}】文件成功！", openFileDialog1.FileName));
            }
            catch (Exception exce)
            {
                MessageDialog.ShowErrorMessage(exce.Message);
            }

            //for (int row = 0; row < dtTemp.Rows.Count; row++)
            //{
            //    for (int col = 0; col < dtTemp.Columns.Count; col++)
            //    {
            //        Console.Write("[{0}, {1}]：{2}\t", row, col, dtTemp.Rows[row][col].ToString());
            //    }

            //    Console.WriteLine();
            //}
        }
    }
}

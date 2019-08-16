using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GlobalObject;
using PlatformManagement;
using Microsoft.Reporting.WinForms;
using ServerModule;

namespace BaseModule_Economic.报表
{
    public partial class 供应商挂账单 : Form
    {
        string _YearMonth;

        string _Provider;

        public 供应商挂账单(string yearMonth, string provider)
        {
            InitializeComponent();

            _YearMonth = yearMonth;
            _Provider = provider;
        }

        private void 供应商挂账单_Load(object sender, EventArgs e)
        {

            View_Provider providerInfo = UniversalFunction.GetProviderInfo(_Provider);

            string strSql = "select 挂账年月, 图号型号, 物品名称, 规格, 供应商, 挂账方式 + '挂账' as 挂账方式, 协议单价, 税率, 上月未挂, 本月入库, 本月应挂, " +
                " 本月未挂, 到票数量, 到票金额, F_Id, GoodsID, 单位 as 计量单位 from View_Bus_PurchasingMG_Account "+
                " where 挂账年月 = '" + _YearMonth + "' and 供应商 = '" + _Provider + "' and 本月应挂 > 0";
            DataTable tempTable = GlobalObject.DatabaseServer.QueryInfo(strSql);
            reportViewer1.LocalReport.DataSources.Clear();
            List<ReportParameter> para = new List<ReportParameter>();
            //这里是添加两个字段  
            para.Add(new ReportParameter("供应商", providerInfo.供应商名称));
            para.Add(new ReportParameter("挂账年月", _YearMonth.Substring(0, 4) + "年" + _YearMonth.Substring(4, 2) + "月"));
            para.Add(new ReportParameter("打印人", BasicInfo.LoginName));


            this.reportViewer1.LocalReport.SetParameters(para);  

            ReportDataSource rds = new ReportDataSource("ReportDataSet_View_Bus_PurchasingMG_Account", tempTable);
            reportViewer1.LocalReport.DataSources.Add(rds);
            this.reportViewer1.RefreshReport();
        }
    }
}

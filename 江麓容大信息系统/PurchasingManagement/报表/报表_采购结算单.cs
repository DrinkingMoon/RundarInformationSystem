using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using ServerModule;
using Microsoft.Reporting.WinForms;
using GlobalObject;
using UniversalControlLibrary;
using Service_Economic_Purchase;
using FlowControlService;

namespace Form_Economic_Purchase
{
    public partial class 报表_采购结算单 : Form, IBillReportInfo
    {
        string m_billID = "";
        string m_billName = "";
        int flag = 0;

        IFlowServer serviceFlow = FlowControlService.ServerModuleFactory.GetServerModule<IFlowServer>();
        IProcurementStatement serviceStatement = Service_Economic_Purchase.ServerModuleFactory.GetServerModule<IProcurementStatement>();

        public 报表_采购结算单(string billID, string billName)
        {
            InitializeComponent();

            m_billID = billID;
            m_billName = billName;

            string billStatus = serviceFlow.GetNowBillStatus(billID);
            string billType = serviceStatement.GetSingleBillInfo(billID).BillType;

            if (GlobalObject.GeneralFunction.StringConvertToEnum<CE_ProcurementStatementBillTypeEnum>(billType) != 
                CE_ProcurementStatementBillTypeEnum.委托加工物资)
            {
                if (GlobalObject.GeneralFunction.StringConvertToEnum<CE_CommonBillStatus>(billStatus) ==
                    CE_CommonBillStatus.等待审核)
                {
                    flag = 1;
                }
                else if (GlobalObject.GeneralFunction.StringConvertToEnum<CE_CommonBillStatus>(billStatus) ==
                    CE_CommonBillStatus.新建单据)
                {
                    flag = 2;
                }
            }
            else
            {
                if (GlobalObject.GeneralFunction.StringConvertToEnum<CE_CommonBillStatus>(billStatus) ==
                    CE_CommonBillStatus.等待审核)
                {
                    flag = 3;
                }
                else if (GlobalObject.GeneralFunction.StringConvertToEnum<CE_CommonBillStatus>(billStatus) ==
                    CE_CommonBillStatus.新建单据)
                {
                    flag = 4;
                }
            }

            InitData();
        }

        #region IBillReportInfo 成员

        /// <summary>
        /// 获取单据类型, 如：领料单等
        /// </summary>
        public string BillType
        {
            get
            {
                return m_billName;
            }
        }

        /// <summary>
        /// 获取单据编号
        /// </summary>
        public string BillNo
        {
            get
            {
                return m_billID;
            }
        }

        /// <summary>
        /// 获取本地报表对象
        /// </summary>
        public LocalReport LocalReportObject
        {
            get
            {
                return reportViewer1.LocalReport;
            }
        }

        #endregion

        private void InitData()
        {
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = GlobalObject.GlobalParameter.StorehouseConnectionString;
                conn.Open();

                string sql = "select * from DepotManagement.dbo.View_Business_Settlement_ProcurementStatementDetail where 单据号='" + m_billID + "'";

                SqlCommand command = new SqlCommand(sql);
                command.Connection = conn;

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(this.SettlementDataSet.View_Business_Settlement_ProcurementStatementDetail);
                ReportDataSource rds = new ReportDataSource("SettlementDataSet_View_Business_Settlement_ProcurementStatementDetail",
                    this.SettlementDataSet.View_Business_Settlement_ProcurementStatementDetail);

                switch (flag)
                {
                    case 1:
                        reportViewer1.LocalReport.DataSources.Clear();
                        reportViewer1.LocalReport.DataSources.Add(rds);
                        break;
                    case 2:
                        reportViewer2.LocalReport.DataSources.Clear();
                        reportViewer2.LocalReport.DataSources.Add(rds);
                        break;
                    case 3:
                        reportViewer3.LocalReport.DataSources.Clear();
                        reportViewer3.LocalReport.DataSources.Add(rds);
                        break;
                    case 4:
                        reportViewer4.LocalReport.DataSources.Clear();
                        reportViewer4.LocalReport.DataSources.Add(rds);
                        break;
                    default:
                        break;
                }

                sql = "select * from DepotManagement.dbo.View_Business_Settlement_ProcurementStatement where BillNo ='" + m_billID + "'";

                command = new SqlCommand(sql);
                command.Connection = conn;

                adapter = new SqlDataAdapter(command);
                adapter.Fill(this.SettlementDataSet.View_Business_Settlement_ProcurementStatement);

                rds = new ReportDataSource("SettlementDataSet_View_Business_Settlement_ProcurementStatement", 
                    this.SettlementDataSet.View_Business_Settlement_ProcurementStatement);

                switch (flag)
                {
                    case 1:
                        reportViewer1.LocalReport.DataSources.Add(rds);
                        break;
                    case 2:
                        reportViewer2.LocalReport.DataSources.Add(rds);
                        break;
                    case 3:
                        reportViewer3.LocalReport.DataSources.Add(rds);
                        break;
                    case 4:
                        reportViewer4.LocalReport.DataSources.Add(rds);
                        break;
                    default:
                        break;
                }
            }

            switch (flag)
            {
                case 1:
                    reportViewer1.RefreshReport();
                    break;
                case 2:
                    reportViewer2.RefreshReport();
                    break;
                case 3:
                    reportViewer3.RefreshReport();
                    break;
                case 4:
                    reportViewer4.RefreshReport();
                    break;
                default:
                    break;
            }
        }

        private void 报表_采购结算单_Load(object sender, EventArgs e)
        {
            switch (flag)
            {
                case 1:
                    reportViewer1.Visible = true;
                    reportViewer1.Dock = DockStyle.Fill;
                    break;
                case 2:
                    reportViewer2.Visible = true;
                    reportViewer2.Dock = DockStyle.Fill;
                    break;
                case 3:
                    reportViewer3.Visible = true;
                    reportViewer3.Dock = DockStyle.Fill;
                    break;
                case 4:
                    reportViewer4.Visible = true;
                    reportViewer4.Dock = DockStyle.Fill;
                    break;
                default:
                    break;
            }
        }
    }
}

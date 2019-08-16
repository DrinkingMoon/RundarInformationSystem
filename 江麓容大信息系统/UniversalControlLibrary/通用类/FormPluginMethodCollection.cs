using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GlobalObject;
using ServerModule;
using System.Windows.Forms;
using System.Data;

namespace UniversalControlLibrary
{
    public static class FormPluginMethodCollection
    {
        static GlobalObject.DelegateCollection.ShowDataGirdViewInfo _FormShow;

        public static void BusinessDataSelect(CE_BillTypeEnum billType)
        {
            try
            {
                string error = "";
                ServerModule.IReport reportService = ServerModule.ServerModuleFactory.GetServerModule<ServerModule.IReport>();

                List<BASE_IntegrationReport> listReport = reportService.GetBusinessSelect(billType);

                if (listReport != null && listReport.Count > 0)
                {
                    FormDataComboBox frmCombox = new FormDataComboBox((from a in listReport select a.ReportName).ToList(),
                        billType.ToString() + "报表查询");

                    if (frmCombox.ShowDialog() == DialogResult.OK)
                    {
                        string reportCode = (from a in listReport where a.ReportName == frmCombox.Result select a.ReportCode).Single().ToString();
                        DataTable dtSoucre = reportService.QueryInfo(reportCode, null, out error);

                        FormConditionDataTableFind frmShow = new FormConditionDataTableFind(dtSoucre);
                        frmShow.ShowDialog();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
            }
        }

        public static void BusinessDataSelect(CE_BillTypeEnum billType, string keyName,
            GlobalObject.DelegateCollection.ShowDataGirdViewInfo formShow)
        {
            try
            {
                string error = "";
                _FormShow = formShow;
                ServerModule.IReport reportService = ServerModule.ServerModuleFactory.GetServerModule<ServerModule.IReport>();
                List<BASE_IntegrationReport> listReport = reportService.GetBusinessSelect(billType);

                if (listReport != null && listReport.Count > 0)
                {
                    FormDataComboBox frmCombox = new FormDataComboBox((from a in listReport select a.ReportName).ToList(),
                        billType.ToString() + "报表查询");

                    if (frmCombox.ShowDialog() == DialogResult.OK)
                    {
                        string reportCode = (from a in listReport 
                                             where a.ReportName == frmCombox.Result 
                                             select a.ReportCode).Single().ToString();

                        List<BASE_IntegrationReportList> list = reportService.GetIntegrationReportList(reportCode);
                        DataTable listInfo = null;

                        if (list != null && list.Count() > 0)
                        {
                            FormValueSet frm = new FormValueSet(list);

                            if (frm.ShowDialog() != DialogResult.OK)
                            {
                                return;
                            }

                            listInfo = new DataTable();

                            listInfo.Columns.Add("参数类型");
                            listInfo.Columns.Add("查询内容");
                            listInfo.Columns.Add("参数名");

                            for (int i = 0; i < frm.Result.Count(); i++)
                            {
                                DataRow dr = listInfo.NewRow();

                                dr["参数名"] = list[i].ParameterName;
                                dr["参数类型"] = frm.Result[i].ParameterType;
                                dr["查询内容"] = frm.Result[i].ReportCode;

                                listInfo.Rows.Add(dr);
                            }
                        }

                        DataTable dtSoucre = reportService.QueryInfo(reportCode, listInfo, out error);

                        FormConditionDataTableFind frmShow = new FormConditionDataTableFind(dtSoucre, keyName);
                        frmShow.FormShow += new DelegateCollection.ShowDataGirdViewInfo(frmShow_FormShow);
                        frmShow.ShowDialog();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
            }
        }

        static void frmShow_FormShow(DataGridView dgv, string keyName)
        {
            if (_FormShow != null)
            {
                _FormShow(dgv, keyName);
            }
        }
    }
}

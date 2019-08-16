using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using System.Collections;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using PlatformManagement;
using GlobalObject;
using ServerModule;
using PMS_ServerModule;

namespace UniversalControlLibrary
{
    public class BillInfo
    {
        public static DataTable BillInfoShowFilter(DataTable billInfo, string billColumnsName)
        {
            if (billInfo == null)
            {
                return null;
            }

            billInfo.AcceptChanges();
            string error = null;
            string strBillNo = "";
            DataTable resultTable = new DataTable();

            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            try
            {
                var varData = from a in ctx.Business_Base_BillNoFilterTemp
                              select a;

                ctx.Business_Base_BillNoFilterTemp.DeleteAllOnSubmit(varData);

                List<string> colList = DataSetHelper.ColumnsToList_Distinct(billInfo, billColumnsName);

                for (int i = 0; i < colList.Count; i++)
                {
                    Business_Base_BillNoFilterTemp tempLnq = new Business_Base_BillNoFilterTemp();

                    tempLnq.ID = Guid.NewGuid();
                    tempLnq.BillNo = colList[i].ToString();

                    ctx.Business_Base_BillNoFilterTemp.InsertOnSubmit(tempLnq);
                }

                ctx.SubmitChanges();

                Hashtable hsTable = new Hashtable();

                hsTable.Add("@WorkID", BasicInfo.LoginID);

                DataTable tempTable = GlobalObject.DatabaseServer.QueryInfoPro("Business_Base_BillInfoShowFilter", hsTable, out error);

                var varTemp = from a in ctx.Business_Base_BillNoFilterTemp
                              select a;

                if (varTemp.Count() == 0)
                {
                    billInfo.Clear();
                    resultTable = billInfo;
                }
                else
                {
                    foreach (var item in varTemp)
                    {
                        strBillNo += "'" + item.BillNo + "',";
                    }

                    strBillNo = strBillNo.Substring(0, strBillNo.Length - 1);

                    resultTable = DataSetHelper.SiftDataTable(billInfo, billColumnsName + " in (" + strBillNo + ")", out error);
                }

                return resultTable;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}

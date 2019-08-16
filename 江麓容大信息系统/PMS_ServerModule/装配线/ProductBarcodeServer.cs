using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data;

namespace ServerModule
{
    /// <summary>
    /// 产品条形码服务
    /// 管理与出厂条形码相关业务（如：出厂打印主表信息、出厂打印明细信息、打印日志、打印规则）
    /// </summary>
    class ProductBarcodeServer : IProductBarcodeServer
    {
        #region 获取数据

        /// <summary>
        /// 获取出厂条形码生成规则信息
        /// </summary>
        /// <returns>返回获取到的信息</returns>
        public IQueryable<View_P_BuildRuleForVehicleBarcode> GetBuildRule()
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var result = from r in ctx.View_P_BuildRuleForVehicleBarcode
                         select r;

            return result;
        }

        /// <summary>
        /// 获取指定编号出厂条形码生成规则信息
        /// </summary>
        /// <returns>返回获取到的信息</returns>
        public View_P_BuildRuleForVehicleBarcode GetBuildRule(int id)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var result = from r in ctx.View_P_BuildRuleForVehicleBarcode
                         where r.规则编号 == id
                         select r;

            return result.First();
        }

        /// <summary>
        /// 获取出厂条形码打印模式信息
        /// </summary>
        /// <returns>返回获取到的信息</returns>
        public IQueryable<P_PrintModeForVehicleBarcode> GetPrintMode()
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var result = from r in ctx.P_PrintModeForVehicleBarcode
                         select r;

            return result;
        }

        /// <summary>
        /// 获取指定打印编号的明细信息
        /// </summary>
        /// <param name="beginDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <returns>返回获取到的信息</returns>
        public IQueryable<View_P_PrintBillForVehicleBarcode> GetPrintSetting(DateTime beginDate, DateTime endDate)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var result = from r in ctx.View_P_PrintBillForVehicleBarcode
                         where r.打印设置日期 >= beginDate.Date && r.打印设置日期 <= endDate.Date
                         select r;

            return result;
        }

        /// <summary>
        /// 获取指定打印编号的打印设置主表信息
        /// </summary>
        /// <param name="billID">打印编号</param>
        /// <returns>返回获取到的信息</returns>
        public View_P_PrintBillForVehicleBarcode GetPrintSetting(int billID)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var result = from r in ctx.View_P_PrintBillForVehicleBarcode
                         where r.打印编号 == billID
                         select r;

            return result.First();
        }

        /// <summary>
        /// 获取指定打印编号的明细信息
        /// </summary>
        /// <param name="billID">打印编号</param>
        /// <returns>返回获取到的信息</returns>
        public IQueryable<View_P_PrintListForVehicleBarcode> GetPrintSettingList(int billID)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var result = from r in ctx.View_P_PrintListForVehicleBarcode
                         where r.打印编号 == billID
                         select r;

            return result;
        }

        /// <summary>
        /// 获取指定编号出厂条形码生成规则格式化字符串信息
        /// </summary>
        /// <param name="ruleID">规则编号</param>
        /// <param name="productData">产品日期</param>
        /// <param name="serialNumber">流水码</param>
        /// <returns>获取到的规则格式化字符串信息</returns>
        public string GetFormatStringOfBuildRule(int ruleID, DateTime productData, int serialNumber)
        {
            string error = null;
            Hashtable paramter = new Hashtable();

            paramter.Add("@RuleID", ruleID);
            paramter.Add("@Date", productData);
            paramter.Add("@SerialNumber", serialNumber.ToString());

            DataTable tempTable = GlobalObject.DatabaseServer.QueryInfoPro("ZPX_GetFactoryBarcode_RuleID", paramter, out error);

            return tempTable.Rows[0][0].ToString();
        }

        /// <summary>
        /// 获取打印日志
        /// </summary>
        /// <param name="beginDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <returns>返回获取到的信息</returns>
        public IQueryable<View_P_PrintLogForVehicleBarcode> GetPrintLog(DateTime beginDate, DateTime endDate)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var result = from r in ctx.View_P_PrintLogForVehicleBarcode
                         where r.打印日期 >= beginDate.Date && r.打印日期 <= endDate.Date
                         select r;

            return result;
        }

        #endregion 获取数据

        /// <summary>
        /// 判断打印明细是否已经打印过
        /// </summary>
        /// <param name="printInfo">打印明细信息</param>
        /// <returns>是返回true</returns>
        public bool IsPrint(View_P_PrintListForVehicleBarcode printInfo)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var result = from r in ctx.View_P_PrintLogForVehicleBarcode
                         where r.产品类型编码 == printInfo.产品类型编码 &&
                               r.产品日期.Date == printInfo.产品日期.Date &&
                               !((printInfo.打印起始编号 > r.打印结束编号) || (printInfo.打印结束编号 < r.打印起始编号))
                         select r;

            return result.Count() > 0;

        }

        /// <summary>
        /// 保存打印设置
        /// </summary>
        /// <param name="bill">打印主表信息</param>
        /// <param name="printList">打印明细信息</param>
        /// <param name="error">出错时返回的出错信息</param>
        /// <returns>操作是否成功的标志</returns>
        public bool SavePrintSetting(View_P_PrintBillForVehicleBarcode bill,
            List<P_PrintListForVehicleBarcode> printList, out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext ctx = CommentParameter.DepotDataContext;
                bool printFlag = false;

                if (bill.打印编号 != 0)
                {
                    var result = from r in ctx.P_PrintBillForVehicleBarcode
                                 where r.ID == bill.打印编号
                                 select r;

                    if (result.Count() > 0)
                    {
                        printFlag = result.Single().PrintFlag;
                        ctx.P_PrintBillForVehicleBarcode.DeleteAllOnSubmit(result);
                    }                    
                }

                P_PrintBillForVehicleBarcode billInfo = new P_PrintBillForVehicleBarcode();
                
                billInfo.PrintFlag = printFlag;
                billInfo.UserCode = bill.工号;
                billInfo.Date = bill.打印设置日期;
                billInfo.Remark = bill.打印说明;

                billInfo.P_PrintListForVehicleBarcode.AddRange(printList);

                ctx.P_PrintBillForVehicleBarcode.InsertOnSubmit(billInfo);
                ctx.SubmitChanges();

                bill.打印编号 = billInfo.ID;
                return true;
            }
            catch (Exception exce)
            {
                error = exce.Message;
                return false;
            }
        }

        /// <summary>
        /// 保存打印标志
        /// </summary>
        /// <param name="billID">打印主表编号</param>
        /// <param name="error">出错时返回的出错信息</param>
        /// <returns>操作是否成功的标志</returns>
        public bool SavePrintFlag(int billID, out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

                var result = from r in ctx.P_PrintBillForVehicleBarcode
                             where r.ID == billID
                             select r;

                if (result.Count() > 0)
                {
                    result.Single().PrintFlag = true;
                    ctx.SubmitChanges();
                }

                return true;
            }
            catch (Exception exce)
            {
                error = exce.Message;
                return false;
            }
        }

        /// <summary>
        /// 删除打印设置
        /// </summary>
        /// <param name="billID">打印主表编号</param>
        /// <param name="error">出错时返回的出错信息</param>
        /// <returns>操作是否成功的标志</returns>
        public bool DeletePrintSetting(int billID, out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

                var result = from r in ctx.P_PrintBillForVehicleBarcode
                             where r.ID == billID
                             select r;

                if (result.Count() > 0)
                {
                    ctx.P_PrintBillForVehicleBarcode.DeleteAllOnSubmit(result);
                    ctx.SubmitChanges();
                }

                return true;
            }
            catch (Exception exce)
            {
                error = exce.Message;
                return false;
            }
        }

        /// <summary>
        /// 写打印日志
        /// </summary>
        /// <param name="log">日志信息</param>
        /// <param name="error">出错时返回的出错信息</param>
        /// <returns>操作是否成功的标志</returns>
        public bool WritePrintLog(P_PrintLogForVehicleBarcode log, out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

                ctx.P_PrintLogForVehicleBarcode.InsertOnSubmit(log);
                ctx.SubmitChanges();

                return true;
            }
            catch (Exception exce)
            {
                error = exce.Message;
                return false;
            }
        }
    }
}

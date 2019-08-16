using System;
using System.Linq;
using ServerModule;
using System.Collections.Generic;

namespace ServerModule
{
    /// <summary>
    /// 产品条形码服务接口
    /// 管理与出厂条形码相关业务（如：出厂打印主表信息、出厂打印明细信息、打印日志、打印规则）
    /// </summary>
    public interface IProductBarcodeServer
    {
        /// <summary>
        /// 删除打印设置
        /// </summary>
        /// <param name="billID">打印主表编号</param>
        /// <param name="error">出错时返回的出错信息</param>
        /// <returns>操作是否成功的标志</returns>
        bool DeletePrintSetting(int billID, out string error);

        /// <summary>
        /// 判断打印明细是否已经打印过
        /// </summary>
        /// <param name="printInfo">打印明细信息</param>
        /// <returns>是返回true</returns>
        bool IsPrint(View_P_PrintListForVehicleBarcode printInfo);

        /// <summary>
        /// 保存打印设置
        /// </summary>
        /// <param name="bill">打印主表信息</param>
        /// <param name="printList">打印明细信息</param>
        /// <param name="error">出错时返回的出错信息</param>
        /// <returns>操作是否成功的标志</returns>
        bool SavePrintSetting(View_P_PrintBillForVehicleBarcode bill,
            List<P_PrintListForVehicleBarcode> printList, out string error);
                
        /// <summary>
        /// 保存打印标志
        /// </summary>
        /// <param name="billID">打印主表编号</param>
        /// <param name="error">出错时返回的出错信息</param>
        /// <returns>操作是否成功的标志</returns>
        bool SavePrintFlag(int billID, out string error);

        /// <summary>
        /// 写打印日志
        /// </summary>
        /// <param name="log">日志信息</param>
        /// <param name="error">出错时返回的出错信息</param>
        /// <returns>操作是否成功的标志</returns>
        bool WritePrintLog(P_PrintLogForVehicleBarcode log, out string error);

        /// <summary>
        /// 获取出厂条形码生成规则信息
        /// </summary>
        /// <returns>返回获取到的信息</returns>
        IQueryable<ServerModule.View_P_BuildRuleForVehicleBarcode> GetBuildRule();
                
        /// <summary>
        /// 获取指定编号出厂条形码生成规则信息
        /// </summary>
        /// <returns>返回获取到的信息</returns>
        View_P_BuildRuleForVehicleBarcode GetBuildRule(int id);
                

        /// <summary>
        /// 获取指定编号出厂条形码生成规则格式化字符串信息
        /// </summary>
        /// <param name="ruleID">规则编号</param>
        /// <param name="productData">产品日期</param>
        /// <param name="serialNumber">流水码</param>
        /// <returns>获取到的规则格式化字符串信息</returns>
        string GetFormatStringOfBuildRule(int ruleID, DateTime productData, int serialNumber);

        /// <summary>
        /// 获取打印日志
        /// </summary>
        /// <param name="beginDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <returns>返回获取到的信息</returns>
        IQueryable<View_P_PrintLogForVehicleBarcode> GetPrintLog(DateTime beginDate, DateTime endDate);

        /// <summary>
        /// 获取出厂条形码打印模式信息
        /// </summary>
        /// <returns>返回获取到的信息</returns>
        IQueryable<ServerModule.P_PrintModeForVehicleBarcode> GetPrintMode();
                
        /// <summary>
        /// 获取指定打印编号的明细信息
        /// </summary>
        /// <param name="beginDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <returns>返回获取到的信息</returns>
        IQueryable<View_P_PrintBillForVehicleBarcode> GetPrintSetting(DateTime beginDate, DateTime endDate);

        /// <summary>
        /// 获取指定打印编号的打印设置主表信息
        /// </summary>
        /// <param name="billID">打印编号</param>
        /// <returns>返回获取到的信息</returns>
        View_P_PrintBillForVehicleBarcode GetPrintSetting(int billID);

        /// <summary>
        /// 获取指定打印编号的明细信息
        /// </summary>
        /// <param name="billID">打印编号</param>
        /// <returns>返回获取到的信息</returns>
        IQueryable<View_P_PrintListForVehicleBarcode> GetPrintSettingList(int billID);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServerModule;
using System.Data;
using GlobalObject;

namespace Service_Project_Design
{
    public interface IBOMChangeService : FlowControlService.IFlowBusinessService
    {
        /// <summary>
        /// 获得单条业务总单信息
        /// </summary>
        /// <param name="billNo">业务号</param>
        /// <returns>返回业务总单信息</returns>
        Business_Base_BomChange GetSingleBillInfo(string billNo);

        /// <summary>
        /// 保存信息
        /// </summary>
        /// <param name="billInfo">单据信息</param>
        /// <param name="invoiceInfo">发票信息列表</param>
        /// <param name="detailInfo">明细信息列表</param>
        void SaveInfo(Business_Base_BomChange billInfo, List<View_Business_Base_BomChange_PartsLibrary> libraryInfo,
            List<View_Business_Base_BomChange_Struct> structInfo);

        /// <summary>
        /// 获得结构信息
        /// </summary>
        /// <param name="billNo">业务号</param>
        /// <returns>返回业务明细列表</returns>
        List<View_Business_Base_BomChange_Struct> GetListStructInfo(string billNo);

        /// <summary>
        /// 获得零件信息
        /// </summary>
        /// <param name="billNo">业务号</param>
        /// <returns>返回业务明细列表</returns>
        List<View_Business_Base_BomChange_PartsLibrary> GetListLibraryInfo(string billNo);

        /// <summary>
        /// 操作流程以外的业务
        /// </summary>
        /// <param name="billNo">单据号</param>
        void OperatarUnFlowBusiness(string billNo);

        /// <summary>
        /// 获得总成选择信息
        /// </summary>
        /// <returns></returns>
        DataTable GetAssemblyInfo();
    }
}

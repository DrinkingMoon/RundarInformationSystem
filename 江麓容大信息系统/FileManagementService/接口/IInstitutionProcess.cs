using System;
using ServerModule;
using System.Collections.Generic;
using GlobalObject;

namespace Service_Quality_File
{
    /// <summary>
    /// 制度流程服务类接口
    /// </summary>
    public interface IInstitutionProcess : IBasicBillServer, IBasicService
    {
        /// <summary>
        /// 获得流程执行图形信息
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <returns>返回字典</returns>
        Dictionary<int, Dictionary<string, bool>> GetExcuteInfo(string billNo);

        /// <summary>
        /// 获得所有单据信息
        /// </summary>
        /// <param name="billTypeEnum">单据类型</param>
        /// <returns>返回Table</returns>
        System.Data.DataTable GetAllBill(CE_BillTypeEnum billTypeEnum);

        /// <summary>
        /// 获得单条单据信息
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <returns>返回LINQ</returns>
        ServerModule.FM_InstitutionProcess GetSingleBill(string billNo);

        /// <summary>
        /// 获得流程意见
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <returns>返回LIST</returns>
        List<CommonProcessInfo> GetProcessInfo(string billNo);

        /// <summary>
        /// 添加信息
        /// </summary>
        /// <param name="institution">LINQ信息</param>
        /// <param name="deptList">部门信息</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True</returns>
        bool AddInfo(FM_InstitutionProcess institution, List<string> deptList, out string error);

        /// <summary>
        /// 更新流程
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="advise">意见</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        bool UpdateInfo(string billNo, string advise, out string error);

        /// <summary>
        /// 删除单据
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True,失败返回False</returns>
        bool DeleteInfo(string billNo, out string error);
    }
}

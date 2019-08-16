using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using PlatformManagement;

namespace WebServerModule
{
    /// <summary>
    /// 售后质量反馈接口服务类(不可调用ServerModule)
    /// </summary>
    public interface IServiceFeedBack
    {
        /// <summary>
        /// 获取或设置查询结果过滤器
        /// </summary>
        string QueryResultFilter { get; set; }

        /// <summary>
        /// 根据车架号获得反馈单的信息
        /// </summary>
        /// <param name="vehicleShelfNumber">车架号</param>
        /// <returns>返回获得的信息</returns>
        DataTable GetVehicleMaintenanceRecord(string vehicleShelfNumber);

        /// <summary>
        /// 获得所有单据信息
        /// </summary>
        /// <param name="billStatus">单据状态</param>
        /// <param name="startTime">起始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns>成功返回满足条件的数据集，失败返回错误信息</returns>
        DataTable GetServiceTable(string billStatus, DateTime startTime, DateTime endTime);

        /// <summary>
        /// 质管部确认，修改表信息
        /// </summary>
        /// <param name="billID">单据号</param>
        /// <param name="ChargeSuggestion">质管部意见</param>
        /// <param name="dutyDept">责任部门</param>
        /// <param name="replyTime">要求回复时间</param>
        /// <param name="appCount">出现次数</param>
        /// <param name="associatedBillNo">问题相似的关联单据</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        bool UpdateZGAffirm(string billID, string ChargeSuggestion, string dutyDept, 
                    DateTime replyTime, int appCount, string associatedBillNo, out string error);

        /// <summary>
        /// 责任部门确认，修改表
        /// </summary>
        /// <param name="billID">单据号</param>
        /// <param name="dutyPerson">责任人</param>
        /// <param name="finish">完成要求</param>
        /// <param name="stock">库存产品意见</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        bool UpdateDutyDept(string billID, string dutyPerson, string finish, 
            string stock, out string error);

        /// <summary>
        /// 责任人确认，修改表信息
        /// </summary>
        /// <param name="back">数据集对象</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        bool UpdateDutyPerson(S_ServiceFeedBack back, out string error);

        /// <summary>
        /// 质管部检验，修改表信息
        /// </summary>
        /// <param name="billID">单据号</param>
        /// <param name="cloes">是否关闭</param>
        /// <param name="practice">落实情况</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        bool UpdateZGCheck(string billID, string cloes, string practice, out string error);

        /// <summary>
        /// 添加返回件
        /// </summary>
        /// <param name="dtAddTb">数据集</param>
        /// <param name="strDJH">单据号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        bool InsertReplace(DataTable dtAddTb, string strDJH, out string error);

        /// <summary>
        /// 删除返回件
        /// </summary>
        /// <param name="strDJH">单据号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        bool DeleteReplace(string strDJH, out string error);

        /// <summary>
        /// 函电信息
        /// </summary>
        /// <param name="endTime">结束时间</param>
        /// <param name="starTime">起始时间</param>
        /// <returns>成功返回满足条件的数据集，失败返回null的dataTable</returns>
        DataTable GetAfterService(string starTime, string endTime);

        /// <summary>
        /// 获取函电信息
        /// </summary>
        /// <param name="returnInfo">函电信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        bool GetAllBill(out IQueryResult returnInfo, out string error);

         /// <summary>
        /// 获取质量反馈查询
        /// </summary>
        /// <param name="returnInfo">质量反馈信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        bool GetAllBillFeedBack(out IQueryResult returnInfo, out string error);

        /// <summary>
        /// 获得故障信息
        /// </summary>
        /// <param name="serviceID">函电单据号</param>
        /// <returns>成功返回满足条件的数据集，失败返回错误信息</returns>
        DataTable GetBugMessageByServiceID(string serviceID);
       
        /// <summary>
        /// 检查单据的编号
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <returns>成功返回单据编号，失败返回null</returns>
        DataRow IsExist(string billNo);

        /// <summary>
        /// 通过单据号删除函电信息
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        bool DeleteByBillNo(string billNo, out string error);
    }
}

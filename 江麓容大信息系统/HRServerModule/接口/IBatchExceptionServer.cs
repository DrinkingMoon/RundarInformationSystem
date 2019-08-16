using System;
using GlobalObject;
using ServerModule;
using System.Collections.Generic;
using System.Data;

namespace Service_Peripheral_HR
{
    /// <summary>
    /// 集体考勤异常信息操作类
    /// </summary>
    public interface IBatchExceptionServer
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="billID"></param>
        /// <returns></returns>
        DataTable GetDetailPersonnel(int billID);

        /// <summary>
        /// 新增集体考勤异常信息
        /// </summary>
        /// <param name="batchException">集体考勤异常信息</param>
        /// <param name="lstPersonnel">人员信息集合</param>
        /// <returns>成功返回True失败返回False</returns>
        void AddBatchException(HR_BatchException batchException, List<PersonnelBasicInfo> lstPersonnel);

        /// <summary>
        /// 主管审核修改集体考勤异常信息
        /// </summary>
        /// <param name="batchException">集体考勤异常信息</param>
        void AuditingBatchException(HR_BatchException batchException);

        /// <summary>
        /// 获取所有集体考勤异常
        /// </summary>
        /// <param name="date">异常日期</param>
        /// <returns>返回数据集</returns>
        System.Data.DataTable GetAllInfo(string date);

        /// <summary>
        /// 判断某一天中是否存在集体考勤异常
        /// </summary>
        /// <param name="date">异常日期</param>
        /// <returns>存在返回true，不存在返回False</returns>
        bool GetBatchByDate(string date);

        /// <summary>
        /// 删除集体考勤异常信息
        /// </summary>
        /// <param name="billID">单据号</param>
        void DeleteBatchException(int billID);
    }
}

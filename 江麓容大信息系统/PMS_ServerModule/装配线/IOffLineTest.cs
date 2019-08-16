using System;
using System.Collections.Generic;
using System.Data;
using System.Collections;
using System.Linq;

namespace ServerModule
{
    /// <summary>
    /// 下线试验结果信息操作类
    /// </summary>
    public interface IOffLineTest
    {

        void Delete(View_ZPX_CVTOffLineTestResult data);

        /// <summary>
        /// 获得记录表的数据
        /// </summary>
        /// <returns>返回Table</returns>
        IEnumerable GetLogInfo();

        /// <summary>
        /// 检测是否能进行试验
        /// </summary>
        /// <param name="productCode">箱号</param>
        /// <param name="error">出错时返回错误信息</param>
        /// <returns>允许试验返回true</returns>
        bool CanOffLineTest(string productCode, out string error);

        /// <summary>
        /// 根据参数实体查询数据
        /// </summary>
        /// <param name="data">检索不为空的数据</param>
        /// <returns>返回查询到的数据</returns>
        IEnumerable<ServerModule.View_ZPX_CVTOffLineTestResult> GetViewData(ServerModule.View_ZPX_CVTOffLineTestResult data);

        /// <summary>
        /// 根据日期范围查询数据
        /// </summary>
        /// <param name="begin">起始时间</param>
        /// <param name="end">结束时间</param>
        /// <returns>返回查询到的数据</returns>
        IEnumerable<View_ZPX_CVTOffLineTestResult> GetViewData(DateTime begin, DateTime end);

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="data">要添加的数据</param>
        /// <returns>操作是否成功的标志</returns>
        bool Add(ZPX_CVTOffLineTestResult data);

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="data">要更新的数据</param>
        /// <returns>操作是否成功的标志</returns>
        bool Update(View_ZPX_CVTOffLineTestResult data);
                
        /// <summary>
        /// 更新说明
        /// </summary>
        /// <param name="data">要更新的数据</param>
        /// <returns>操作是否成功的标志</returns>
        bool UpdateRemark(View_ZPX_CVTOffLineTestResult data);

        /// <summary>
        /// 审核数据
        /// </summary>
        /// <param name="dataID">要审核的数据编号</param>
        /// <returns>操作是否成功的标志</returns>
        bool Auditing(int dataID);

        #region 2013-09-12 夏石友

            /// <summary>
        /// 获取强制下线试验信息
        /// </summary>
        /// <returns></returns>
        IQueryable<View_ZPX_ForcedOffLineTest> GetForcedOffLineTestInfo();
            
        /// <summary>
        /// 添加强制下线试验信息
        /// </summary>
        /// <param name="data">要添加的数据</param>
        void AddForcedOffLineTestInfo(ZPX_ForcedOffLineTest data);
                
        /// <summary>
        /// 删除强制下线试验信息
        /// </summary>
        /// <param name="id">要删除的数据ID</param>
        void DeleteForcedOffLineTestInfo(int id);

        #endregion
    }
}

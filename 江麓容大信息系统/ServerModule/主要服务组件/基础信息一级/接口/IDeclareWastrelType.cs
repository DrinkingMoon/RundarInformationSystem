using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerModule
{
    /// <summary>
    /// 报废类别服务接口
    /// </summary>
    public interface IDeclareWastrelType
    {
        /// <summary>
        /// 获取报废类别
        /// </summary>
        /// <param name="typeId">类型ID</param>
        /// <returns>返回获取到的报废类别信息</returns>
        S_DeclareWastrelType GetWastrelType(int typeId);

        /// <summary>
        /// 获取报废类别
        /// </summary>
        /// <param name="typeName">类型名称</param>
        /// <returns>返回获取到的报废类别信息</returns>
        S_DeclareWastrelType GetWastrelType(string typeName);

        /// <summary>
        /// 获取所有报废类别
        /// </summary>
        /// <returns>返回获取到的报废类别信息</returns>
        IQueryable<S_DeclareWastrelType> GetAllType();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerModule
{
    /// <summary>
    /// 报废类别管理类
    /// </summary>
    class DeclareWastrelType : IDeclareWastrelType
    {
        /// <summary>
        /// 获取报废类别
        /// </summary>
        /// <param name="typeId">类型ID</param>
        /// <returns>返回获取到的报废类别信息</returns>
        public S_DeclareWastrelType GetWastrelType(int typeId)
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            var result = from r in dataContxt.S_DeclareWastrelType 
                         where r.ID == typeId 
                         select r;

            return result.Single();
        }

        /// <summary>
        /// 获取报废类别
        /// </summary>
        /// <param name="typeName">类型名称</param>
        /// <returns>返回获取到的报废类别信息, 失败返回null</returns>
        public S_DeclareWastrelType GetWastrelType(string typeName)
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            var result = from r in dataContxt.S_DeclareWastrelType 
                         where r.DeclareWastrelType == typeName 
                         select r;

            return result.Single();
        }

        /// <summary>
        /// 获取所有报废类别
        /// </summary>
        /// <returns>返回获取到的报废类别信息</returns>
        public IQueryable<S_DeclareWastrelType> GetAllType()
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            var result = from r in dataContxt.S_DeclareWastrelType 
                         select r;

            return result;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerModule
{
    /// <summary>
    /// 单据类别服务类
    /// </summary>
    class BillTypeServer : IBillTypeServer
    {
        #region IBillType 成员

        /// <summary>
        /// 获取所有单据类别
        /// </summary>
        /// <returns>返回获取到的单据类别集</returns>
        public IQueryable<BASE_BillType> GetAllType()
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            var result = from r in dataContxt.BASE_BillType select r;

            return result;
        }

        /// <summary>
        /// 根据单据编码获取单据类别信息
        /// </summary>
        /// <param name="typeCode">类别编码</param>
        /// <returns>成功则返回获取到的单据信息，失败则返回null</returns>
        public BASE_BillType GetBillTypeFromCode(string typeCode)
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            var result = from r in dataContxt.BASE_BillType
                         where r.TypeCode == typeCode
                         select r;

            if (result.Count() == 0)
            {
                return null;
            }

            return result.Single();
        }

        /// <summary>
        /// 根据单据名称获取单据类别信息
        /// </summary>
        /// <param name="dataContxt">数据上下文</param>
        /// <param name="typeName">类别名称</param>
        /// <returns>成功则返回获取到的单据信息，失败则返回null</returns>
        public BASE_BillType GetBillTypeFromName(DepotManagementDataContext dataContxt, string typeName)
        {
            var result = from r in dataContxt.BASE_BillType
                         where r.TypeName == typeName
                         select r;

            if (result.Count() == 0)
            {
                return null;
            }

            return result.Single();
        }

        /// <summary>
        /// 根据单据名称获取单据类别信息
        /// </summary>
        /// <param name="typeName">类别名称</param>
        /// <returns>成功则返回获取到的单据信息，失败则返回null</returns>
        public BASE_BillType GetBillTypeFromName(string typeName)
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            var result = from r in dataContxt.BASE_BillType
                         where r.TypeName == typeName
                         select r;

            if (result.Count() == 0)
            {
                return null;
            }

            return result.Single();
        }

        #endregion
    }
}

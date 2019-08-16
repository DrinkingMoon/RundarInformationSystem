using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServerModule;

namespace HRServerModule
{
    /// <summary>
    /// 单据类别服务接口
    /// </summary>
    public interface IBillTypeServer
    {
        /// <summary>
        /// 获取所有单据类别
        /// </summary>
        /// <returns>返回获取到的单据类别集</returns>
        IQueryable<BASE_BillType> GetAllType();

        /// <summary>
        /// 根据单据编码获取单据类别信息
        /// </summary>
        /// <param name="typeCode">类别编码</param>
        /// <returns>成功则返回获取到的单据信息，失败则返回null</returns>
        BASE_BillType GetBillTypeFromCode(string typeCode);

        /// <summary>
        /// 根据单据名称获取单据类别信息
        /// </summary>
        /// <param name="typeName">类别名称</param>
        /// <returns>成功则返回获取到的单据信息，失败则返回null</returns>
        BASE_BillType GetBillTypeFromName(string typeName);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerModule
{
    /// <summary>
    /// 领料单用途服务接口
    /// </summary>
    public interface IMaterialRequisitionPurposeServer
    {
        /// <summary>
        /// 获取所有用途
        /// </summary>
        /// <returns>返回获取到的单据用途信息</returns>
        IQueryable<BASE_MaterialRequisitionPurpose> GetAllPurpose();
        
        /// <summary>
        /// 根据用途编码获取用途信息
        /// </summary>
        /// <param name="purpose">用途编码/用途名称</param>
        /// <returns>返回获取到的用途信息, 失败返回null</returns>
        BASE_MaterialRequisitionPurpose GetBillPurpose(string purpose);

        /// <summary>
        /// 根据用途编码获取用途信息
        /// </summary>
        /// <param name="dataContxt">数据上下文</param>
        /// <param name="purpose">用途编码/用途名称</param>
        /// <returns>返回获取到的用途信息, 失败返回null</returns>
        BASE_MaterialRequisitionPurpose GetBillPurpose(DepotManagementDataContext dataContxt, string purpose);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerModule
{
    /// <summary>
    /// BomMapping类接口
    /// </summary>
    public interface IBomMappingServer : IBasicService
    {       
        /// <summary>
        /// 获取某一版本的BomMapping信息
        /// </summary>
        /// <param name="productName">产品名称</param>
        /// <returns>操作成功返回获取到的映射表数据</returns>
        IQueryable<View_P_ProductBomMapping> GetBomMapping(string productName);
        
        /// <summary>
        /// 获取指定工位的BomMapping信息
        /// </summary>
        /// <param name="workBench">工位号</param>
        /// <returns>操作成功返回获取到的映射表数据</returns>
        IQueryable<View_P_ProductBomMapping> GetBomMappingOfWorkBench(string workBench);

        /// <summary>
        /// 获取指定父总成名称的BomMapping信息
        /// </summary>
        /// <param name="partName">父总成名称</param>
        /// <returns>操作成功返回获取到的映射表数据</returns>
        IQueryable<View_P_ProductBomMapping> GetChildrenPart(string partName);

        /// <summary>
        /// 获取映射信息
        /// </summary>
        /// <param name="productName">产品名称</param>
        /// <param name="parentCode">零件父总成编码</param>
        /// <param name="partCode">零件编码</param>
        /// <returns>返回获得的工位信息</returns>
        List<View_P_ProductBomMapping> GetMappingInfo(string productName, string parentCode, string partCode);

        /// <summary>
        /// 检测设计与装配基数是否吻合
        /// </summary>
        /// <param name="designedBasicCount">设计BOM中此零件基数</param>
        /// <param name="productName">产品名称</param>
        /// <param name="parentCode">零件父总成编码</param>
        /// <param name="partCode">零件编码</param>
        /// <param name="assemblyAmount">此零件在BOM映射表中的装配数量之和</param>
        /// <returns>返回是否一致的标志</returns>
        bool IsCoincideBasicCount(int designedBasicCount, string productName, string parentCode, string partCode, out int assemblyAmount);

        /// <summary>
        /// 添加映射表信息
        /// </summary>
        /// <param name="bomMapping">要添加的信息</param>
        /// <returns>返回是否成功的标志</returns> 
        bool AddBomMapping(P_ProductBomMapping bomMapping);

        /// <summary>
        /// 修改指定映射表信息
        /// </summary>
        /// <param name="id">要更新的数据 ID</param>
        /// <param name="bomMapping">修改后的值</param>
        /// <returns>返回是否成功的标志</returns> 
        bool UpdateBomMapping(int id, P_ProductBomMapping bomMapping);

        /// <summary>
        /// 更新映射表中某一产品名称
        /// </summary>
        /// <param name="oldProductName">老产品名称</param>
        /// <param name="newProductName">新产品名称</param>
        /// <returns>返回是否成功更新BomMapping中某一版本的版本号</returns>
        bool UpdateProductName(string oldProductName, string newProductName);

        /// <summary>
        /// 删除指定映射表信息
        /// </summary>
        /// <param name="id">要删除的数据 ID</param>
        /// <returns>返回是否成功的标志</returns> 
        bool DeleteBomMapping(int id);
    }
}

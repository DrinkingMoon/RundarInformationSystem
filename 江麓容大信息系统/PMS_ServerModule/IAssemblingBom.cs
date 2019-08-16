using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ServerModule
{
    /// <summary>
    /// 装配BOM数据库操作接口
    /// </summary>
    public interface IAssemblingBom : IBasicService
    {
        /// <summary>
        /// 获取指定产品的装配BOM
        /// </summary>
        /// <param name="productCode">产品编码</param>
        /// <returns>返回获取到的信息</returns>
        List<View_P_AssemblingBom> GetAssemblingBom(string productCode);

        /// <summary>
        /// 获取参数对应的装配BOM信息
        /// </summary>
        /// <param name="productCode">产品编码</param>
        /// <param name="parentCode">父总成编码</param>
        /// <param name="partCode">零件编码</param>
        /// <param name="spec">规格</param>
        /// <returns>返回获取到的信息</returns>
        List<View_P_AssemblingBom> GetAssemblingBom(string productCode, string parentCode, string partCode, string spec);

        /// <summary>
        /// 获取参数对应的装配BOM信息
        /// </summary>
        /// <param name="productCode">产品编码</param>
        /// <param name="partCode">零件编码</param>
        /// <param name="spec">规格</param>
        /// <returns>返回获取到的信息</returns>
        List<View_P_AssemblingBom> GetAssemblingBom(string productCode, string partCode, string spec);

        /// <summary>
        /// 检查装配BOM中是否存在指定工位
        /// </summary>
        /// <param name="workbench">要检查的工位号</param>
        /// <returns>存在返回true，不存在返回false</returns>
        bool IsExistsWorkbench(string workbench);

        /// <summary>
        /// 获取包含指定工位的装配BOM信息
        /// </summary>
        /// <param name="workBench">工位号</param>
        /// <returns>返回获取到的信息</returns>
        IQueryable<View_P_AssemblingBom> GetInfoOfWorkBench(string workBench);

        /// <summary>
        /// 获取指定父总成名称的子零件信息
        /// </summary>
        /// <param name="parentName">父总成名称</param>
        /// <returns>返回获取到的信息</returns>
        IQueryable<View_P_AssemblingBom> GetChildrenPart(string parentName);

        /// <summary>
        /// 获取指定产品类型包含的总成名称(包含大总成)
        /// </summary>
        /// <param name="productType">产品类型</param>
        /// <returns>返回获取到的分总成名称列表</returns>
        string[] GetParentNames(string productType);

        /// <summary>
        /// 获取指定产品类型包含的总成名称(不包含大总成)
        /// </summary>
        /// <returns>返回获取到的分总成名称列表</returns>
        string[] GetParentNames();

        #region 夏石友，2012.07.12 16:00

        /// <summary>
        /// 获取指定产品类型根节点数据
        /// </summary>
        /// <param name="productType">产品类型</param>
        /// <returns>获取到的根节点数据</returns>
        P_AssemblingBom GetRootNode(string productType);

        #endregion

        /// <summary>
        /// 添加装配BOM零件信息
        /// </summary>
        /// <param name="info">装配BOM零件信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        bool Add(P_AssemblingBom info, out string error);

        /// <summary>
        /// 删除装配BOM零件信息
        /// </summary>
        /// <param name="id">装配BOM零件信息ID</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        bool Delete(int id, out string error);

        /// <summary>
        /// 更新装配BOM零件信息
        /// </summary>
        /// <param name="id">装配BOM零件信息ID</param>
        /// <param name="updateInfo">装配BOM零件更新后的信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        bool Update(int id, P_AssemblingBom updateInfo, out string error);

        /// <summary>
        /// 复制指定版本的装配BOM信息到目标版本
        /// </summary>
        /// <param name="surEdition">源装配BOM版本号</param>
        /// <param name="tarEdition">目标装配BOM版本号</param>
        /// <param name="error">操作错误时返回的错误信息</param>
        /// <returns>操作是否成功的标志</returns>
        bool CopyBomData(string surEdition, string tarEdition, out string error);

        /// <summary>
        /// 复制指定版本的装配BOM的指定分总成信息到目标版本
        /// </summary>
        /// <param name="surEdition">源装配BOM版本号</param>
        /// <param name="tarEdition">目标装配BOM版本号</param>
        /// <param name="tarParentName">分总成名称</param>
        /// <param name="error">操作错误时返回的错误信息</param>
        /// <returns>操作是否成功的标志</returns>
        bool CopyBomData(string surEdition, string tarEdition, string tarParentName, out string error);

        /// <summary>
        /// 删除分总成下所有零件
        /// </summary>
        /// <param name="surEdition">源装配BOM版本号</param>
        /// <param name="parentName">分总成名称</param>
        /// <param name="error">操作错误时返回的错误信息</param>
        /// <returns>操作是否成功的标志</returns>
        bool DeletePart(string surEdition, string parentName, out string error);

        /// <summary>
        /// 更新分总成下所有零件工位号
        /// </summary>
        /// <param name="surEdition">源装配BOM版本号</param>
        /// <param name="parentName">分总成名称</param>
        /// <param name="workBench">要设置的工位号</param>
        /// <param name="updateParentPart">是否更新时一并更新分总成零件的工位号</param>
        /// <param name="error">操作错误时返回的错误信息</param>
        /// <returns>操作是否成功的标志</returns>
        bool UpdateWorkBench(string surEdition, string parentName, string workBench, bool updateParentPart, out string error);

        /// <summary>
        /// 判断同一产品、同一零件在同一工位是否已经存在
        /// </summary>
        /// <param name="productCode">产品类型</param>
        /// <param name="goodsCode">零件编码</param>
        /// <param name="workbench">工位</param>
        /// <returns>存在返回true，不存在返回false</returns>
        bool IsExistGoodsWorkbench(string productCode, string goodsCode, string workbench);
            
        #region 2013-09-05 夏石友
        /// <summary>
        /// 保存装配顺序
        /// </summary>
        /// <param name="lstData">要保存的列表数据</param>
        void SaveOrderNo(List<View_P_AssemblingBom> lstData);

        /// <summary>
        /// 获取指定产品类型包含的工位
        /// </summary>
        /// <param name="productType">产品类型</param>
        /// <returns>返回获取到的工位列表</returns>
        string[] GetWorkbenchs(string productType);
        
        /// <summary>
        /// 复制指定版本的装配BOM的指定分总成信息到目标版本
        /// </summary>
        /// <param name="surProductType">源装配BOM产品类型</param>
        /// <param name="tarProductType">目标装配BOM产品类型</param>
        /// <param name="workbench">工位</param>
        /// <param name="error">操作错误时返回的错误信息</param>
        /// <returns>复制成功返回True，复制失败返回False</returns>
        bool CopyAssemblySequence(string surProductType, string tarProductType, string workbench, out string error);
 
        #endregion
    }
}

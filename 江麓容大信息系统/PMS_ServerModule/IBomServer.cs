using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ServerModule
{
    /// <summary>
    /// Bom类接口
    /// </summary>
    public interface IBomServer
    {
        /// <summary>
        /// 获得产品型号列表
        /// </summary>
        /// <returns>返回列表</returns>
        List<string> GetAssemblyTypeList();

        /// <summary>
        /// 获得BOM表的信息
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="code">图号型号</param>
        /// <param name="name">物品名称</param>
        /// <returns>返回Table</returns>
        DataRow GetBomInfo(DepotManagementDataContext ctx, string code, string name);

        /// <summary>
        /// 获得大多数物品基数
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <returns>返回基数</returns>
        decimal GetAllMostUsage(int goodsID);

        /// <summary>
        /// 获得版本信息
        /// </summary>
        /// <param name="goodsCode">图号型号</param>
        /// <param name="goodsName">零件名称</param>
        /// <returns>满足条件的版次号</returns>
        DataTable GetVersion(string goodsCode, string goodsName);

        /// <summary>
        /// 更新BOM零件库信息（版次号除外）
        /// </summary>
        /// <param name="bomPartsLibrary">LINQ数据集</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True,失败返回False</returns>
        bool UpdateBOMPartsLibrary(BASE_BomPartsLibrary bomPartsLibrary, out string error);

        /// <summary>
        /// 获得BOM表零件库信息
        /// </summary>
        /// <returns>返回TABLE</returns>
        DataTable GetBOMPartsLibrary();

        /// <summary>
        /// 获得BOM结构表某一条记录
        /// </summary>
        /// <param name="parentID">父级ID</param>
        /// <param name="goodsID">物品ID</param>
        /// <returns>返回Table</returns>
        DataTable GetBomStructInfo(int parentID, int goodsID);

        /// <summary>
        /// 获得对应产品型号的BOM版次号
        /// </summary>
        /// <param name="productCode">产品型号</param>
        /// <returns>返回TABLE</returns>
        DataTable GetBomBackUpBomEdtion(string productCode);
        
        /// <summary>
        /// 获得BOM历史记录
        /// </summary>
        /// <param name="productCode">产品型号</param>
        /// <param name="bomEdition">BOM版次号</param>
        /// <returns>返回DataTable</returns>
        DataTable GetBomBackUpInfo(string productCode, string bomEdition);

        /// <summary>
        /// 获得BOM表的信息
        /// </summary>
        /// <param name="code">图号型号</param>
        /// <param name="name">物品名称</param>
        /// <returns>返回Table</returns>
        DataRow GetBomInfo(string code, string name);

        /// <summary>
        /// 获得某一个产品下的分装总成图号型号
        /// </summary>
        /// <param name="productType">产品型号</param>
        /// <param name="parentName">产品名称</param>
        /// <returns>返回TABLE</returns>
        DataTable GetBomProductParentCode(string productType, string parentName);

        /// <summary>
        /// 获得对应所属总成，图号型号，物品名称，规格的BOM表记录
        /// </summary>
        /// <param name="edition">产品型号</param>
        /// <param name="goodCode">图号型号</param>
        /// <param name="goodName">物品名称</param>
        /// <param name="spec">规格</param>
        /// <returns>返回获得的对应的零件BOM的信息</returns>
        DataTable GetBomEdtionForGoodsInfo(string edition, string goodCode, string goodName, string spec);

        /// <summary>
        /// 获得其他关联的BOM表信息的零件
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <returns>返回其他关联的BOM表信息的零件信息</returns>
        DataTable GetJumblyGoods(int goodsID);

        /// <summary>
        /// 获得BOM表的基数
        /// </summary>
        /// <param name="edition">父级编码</param>
        /// <param name="code">图号型号</param>
        /// <param name="name">物品名称</param>
        /// <param name="spec">规格</param>
        /// <returns>返回基数</returns>
        int GetBomCounts(string edition, string code, string name, string spec);

        /// <summary>
        /// 获取指定产品下的父总成编码及名称
        /// </summary>
        /// <param name="productCode">产品编码</param>
        /// <param name="assemblyCodes">获取到的父总成编码</param>
        /// <param name="assemblyNames">获取到的父总成名称</param>
        /// <returns>成功获取到返回true</returns>
        bool GetAssemblyInfo(string productCode, out string[] assemblyCodes, out string[] assemblyNames);

        /// <summary>
        /// 获取某一版本的Bom信息
        /// </summary>
        /// <param name="edition">版本号</param>
        /// <param name="dataTable">Bom 数据表</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回true</returns>
        bool GetBom(string edition, out System.Data.DataTable dataTable, out string error);

        /// <summary>
        /// 获取某版本中属于多个父总成的零件
        /// </summary>
        /// <param name="edition">版本号</param>
        /// <returns>获取到的零件信息列表</returns>
        List<View_P_ProductBomMultiParentPart> GetMultiParentPart(string edition);

        /// <summary>
        /// 获取某一版本的Bom
        /// </summary>
        /// <param name="edition">版本号</param>
        /// <param name="dic">Bom字典</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功获取某一版本的Bom</returns>
        bool GetBom(string edition, out Dictionary<string, List<Bom>> dic, out string error);

        /// <summary>
        /// 获取某一版本的Bom信息
        /// </summary>
        /// <param name="edition">版本号</param>
        /// <returns>返回获取到的Bom信息</returns>
        IQueryable<View_P_ProductBom> GetBom(string edition);

        /// <summary>
        /// 克隆Bom表
        /// </summary>
        /// <param name="edition">版本号</param>
        /// <param name="listBom">Bom列表</param>
        /// <returns>返回克隆的Bom表</returns>
        Dictionary<string, List<Bom>> Clone(string edition, List<Bom> listBom);

        /// <summary>
        /// 修改指定BOM节点的总成标志信息
        /// </summary>
        /// <param name="edition">版本</param>
        /// <param name="bom">要修改的BOM节点</param>
        /// <param name="isAssembly">修改成的总成标志</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功的标志</returns> 
        //bool UpdateBom(string edition, Bom bom, bool isAssembly, out string error);

        /// <summary>
        /// 更新BOM信息(以事务方式添加、删除bom信息)
        /// </summary>
        /// <param name="edition">版本</param>
        /// <param name="lstBom">要操作的BOM信息列表</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功的标志</returns> 
        bool UpdateBom(string edition, List<Bom> lstBom, out string error);

        /// <summary>
        /// 添加BOM关联零件表
        /// </summary>
        /// <param name="jumbly">LINQ数据集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        bool AddJumbly(P_JumblyBomGoods jumbly, out string error);

        /// <summary>
        /// 更新BOM关联零件表
        /// </summary>
        /// <param name="jumbly">LINQ数据集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        bool UpdateJumbly(P_JumblyBomGoods jumbly, out string error);

        /// <summary>
        /// 删除BOM关联零件表
        /// </summary>
        /// <param name="jumbly">LINQ数据集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        bool DeleteJumbly(P_JumblyBomGoods jumbly, out string error);

        /// <summary>
        /// 获得BOM关联零件表
        /// </summary>
        /// <returns></returns>
        DataTable GetJumblyTable();
        

        #region 2012.3.5 夏石友，增加原因：测试装配BOM中的零件是否属于产品BOM表

        /// <summary>
        /// 获取某一版本的Bom信息
        /// </summary>
        /// <param name="edition">版本号</param>
        /// <returns>获取到指定版本的BOM表信息</returns>
        IQueryable<View_P_ProductBom> GetBomData(string edition);

        #endregion
    }
}

/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  IElectronFileServer.cs
 * 作者    :  夏石友    版本: v1.00    日期: 2010/07/17
 * 开发平台:  Visual C# 2008
 * 用于    :  仓库管理软件
 *----------------------------------------------------------------------------
 * 描述 : 
 * 其它 :
 *----------------------------------------------------------------------------
 * 公共信息: 参见系统'类帮助文档'
 *----------------------------------------------------------------------------
 * 历史记录:
 *     1. 日期时间: 2010/07/17 8:54:12 作者: 夏石友 当前版本: V1.00
 *        修改说明: 创建
 ******************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace ServerModule
{
    /// <summary>
    /// 电子档案管理类接口
    /// </summary>
    public interface IElectronFileServer
    {
        /// <summary>
        /// 获得装配数据范围内零件图号、名称、规格、批次号不同的电子档案零件信息
        /// 包括临时电子档案、电子档案
        /// </summary>
        /// <param name="beginDate">起始装配数据</param>
        /// <param name="endDate">截止装配数据</param>
        /// <returns>返回对应的电子档案零件信息(只有零件图号、名称、规格、批次号信息)</returns>
        DataTable GetDistinctPartInfo(DateTime beginDate, DateTime endDate);
                
        /// <summary>
        /// 获得指定参数对应的电子档案信息
        /// </summary>
        /// <param name="code">图号</param>
        /// <param name="name">名称</param>
        /// <param name="spec">规格</param>
        /// <param name="batchNo">批次号, 此参数为null时表示不要匹配批次号</param>
        /// <param name="begin">装配起始时间</param>
        /// <param name="end">装配截止时间</param>
        /// <param name="amount">输出匹配记录的零件装配总数</param>
        /// <returns>返回匹配的电子档案信息</returns>
        DataTable GetElectronFile(
            string code, string name, string spec,
            string batchNo, DateTime begin, DateTime end, out int amount);

        /// <summary>
        /// 获得对应的产品编码的电子档案
        /// </summary>
        /// <param name="electronProductCode">产品编码</param>
        /// <returns>返回对应的产品编码的电子档案</returns>
        DataTable GetProductElectronFile(string electronProductCode);

        /// <summary>
        /// 获得某一个箱子的某一个零件的电子档案
        /// </summary>
        /// <param name="productCode">箱号</param>
        /// <param name="code">图号</param>
        /// <param name="name">名称</param>
        /// <param name="spec">规格</param>
        /// <returns>返回信息</returns>
        DataTable GetProductElectronFile(string productCode, string code, string name, string spec);

        /// <summary>
        /// 检查指定产品编号是否存在
        /// </summary>
        /// <param name="productCode">产品编号</param>
        /// <returns>存在返回true, 不存在返回false</returns>
        bool IsExists(string productCode);
                
        /// <summary>
        /// 判断是否为总成
        /// 根据BOM中的总成标志判断
        /// </summary>
        /// <param name="partCode">零件编码</param>
        /// <returns>是总成则返回true</returns>
        bool IsAssemblyPart(string partCode);
                 
        /// <summary>
        /// 获取父总成零件装配虚拟编码与零件图号之间的映射
        /// </summary>
        /// <param name="productTypeCode">产品类型编码</param>
        /// <returns>成功返回获取到的映射字典，失败返回null</returns>
        Dictionary<string, string> GetVirtualPartMapping(string productTypeCode);
               
        /// <summary>
        /// 获取包含检测数据的可选配的零件信息（用于做为模板）
        /// </summary>
        /// <param name="productTypeCode">产品类型编码</param>
        /// <param name="partCode">选配件图号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>成功返回获取到的零件信息，失败返回null</returns>
        View_P_ElectronFile GetOptionPartInfo(string productTypeCode, string partCode,out string error);

        /// <summary>
        /// 存储装配信息至电子档案
        /// </summary>
        /// <param name="productCode">产品编码</param>
        /// <returns>返回操作是否的标志</returns>
        bool SaveElectronFile(string productCode);

        /// <summary>
        /// 获取从起始索引开始的指定行数的电子档案信息
        /// </summary>
        /// <typeparam name="T">只能为 View_P_ElectronFile 或者 P_ElectronFile</typeparam>
        /// <param name="startIndex">起始索引</param>
        /// <param name="recordAmount">记录数量, 如果为-1，则表示获取从起始索引开始的所有信息</param>
        /// <param name="electronWordInfo">获取到的档案信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        bool GetElectronFile<T>(int startIndex, int recordAmount, out IQueryable<T> electronWordInfo, out string error);

        /// <summary>
        /// 获取指定产品编号的电子档案信息
        /// </summary>
        /// <param name="productCode">产品编号</param>
        /// <param name="electronWordInfo">获取到的档案信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        bool GetElectronFile(string productCode, out IQueryable<View_P_ElectronFile> electronWordInfo, out string error);

        /// <summary>
        /// 获取指定产品编号的电子档案信息
        /// </summary>
        /// <param name="productCode">产品编号</param>
        /// <param name="electronWordInfo">获取到的档案信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        bool GetElectronFile(string productCode, out IQueryable<P_ElectronFile> electronWordInfo, out string error);

        /// <summary>
        /// 获取电子档案信息
        /// </summary>
        /// <param name="productCode">CVT箱号</param>
        /// <param name="parentCode">父总成图号</param>
        /// <param name="parentScanCode">父总成扫描码</param>
        /// <param name="goodsCode">物品图号</param>
        /// <param name="spec">物品规格</param>
        /// <param name="returnTable">查询结果</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        bool GetElectronFile(string productCode, string parentCode, string parentScanCode,
            string goodsCode, string spec, out DataTable returnTable, out string error);
  
        /// <summary>
        /// 获取电子档案信息(按零件编码和零件标识码查询)
        /// </summary>
        /// <param name="type">查询方式选择，0：按零件标识码查询；其他：按零件编码查询</param>
        /// <param name="conditions">查询方式为0时，参数为产品标识码，否则参数为零件图号</param>
        /// <param name="pageSize">分页记录数</param>
        /// <param name="pageStartNo">起始页号</param>
        /// <param name="returnTable">返回查询到的数据集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        bool GetEspeciallyElectronFile(int type, string conditions, int pageSize, int pageStartNo, out DataTable returnTable, out string error);

        /// <summary>
        /// 获取电子档案信息(按装配时间和供应商批次号查询)
        /// </summary>
        /// <param name="type">查询方式选择，2：按装配时间查询；其他：按供应商批次号查询</param>
        /// <param name="str1">查询方式为2时，参数为装配起始时间，否则供应商编码</param>
        /// <param name="str2">查询方式为2时，参数为装配终止时间，否则参数为批次号</param>
        /// <param name="pageSize">分页记录数</param>
        /// <param name="pageStartNo">起始页号</param>
        /// <param name="returnTable">返回查询到的数据集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        bool GetEspeciallyElectronFile(int type, string str1, string str2, int pageSize, int pageStartNo, out DataTable returnTable, out string error);

        /// <summary>
        /// 获取电子档案中指定供应商对应的所有批次号
        /// </summary>
        /// <param name="providerCode">供应商编码</param>
        /// <param name="table">批次号信息表</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        bool GetAllProviderBatchNo(string providerCode, out DataTable table, out string error);

        /// <summary>
        /// 获取页面号
        /// </summary>
        /// <param name="condition">查询值</param>
        /// <param name="type">查询方式</param>
        /// <returns>数据库中符合条件的记录数</returns>
        int GetIndexEspeciallyElectronFile(string condition, int type);

        /// <summary>
        /// 获取页面号
        /// </summary>
        /// <param name="str1">起始值</param>
        /// <param name="str2">终止值</param>
        /// <param name="type">查询方式</param>
        /// <returns>数据库中符合条件的记录数</returns>
        int GetIndexEspeciallyElectronFile(string str1, string str2, int type);

        /// <summary>
        /// 获取数据库中某段范围内的最后一页所有电子档案(按零件编码和零件标识码查询)
        /// </summary>
        /// <param name="condition">查询值</param>
        /// <param name="pageSize">每页显示的行数</param>
        /// <param name="type">查询方式</param>
        /// <param name="table">数据库中某段范围内的最后一页所有电子档案</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功获取数据库中某段范围内的最后一页所有电子档案</returns>
        bool GetLastEspeciallyElectronFile(string condition, int pageSize, int type, out DataTable table, out string error);

        /// <summary>
        /// 获取数据库中某段范围内的最后一页所有电子档案(按装配时间和供应商批次号查询)
        /// </summary>
        /// <param name="str1">起始值</param>
        /// <param name="str2">终止值</param>
        /// <param name="pageSize">每页显示的行数</param>
        /// <param name="type">查询方式</param>
        /// <param name="table">数据库中某段范围内的最后一页所有电子档案</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功获取数据库中某段范围内的最后一页所有电子档案</returns>
        bool GetLastEspeciallyElectronFile(string str1, string str2, int pageSize, int type, out DataTable table, out string error);

        /// <summary>
        /// 修改电子档案行记录
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="checkData">检测数据</param>
        /// <param name="factData">实际数据</param>
        /// <param name="remark">备注</param>
        /// <param name="error">出错时返回错误信息，正常时为NULL</param>
        /// <returns>操作是否成功的标志</returns>
        bool ModificateElectronFile(long id, string checkData, string factData, string remark, out string error);

        /// <summary>
        /// 根据产品标识码获取电子档案数据（如根据产品标识码获取电子档案数据失败则根据产品箱号获取电子档案）
        /// </summary>
        /// <param name="strProductOnlyCode">产品标识码</param>
        /// <param name="strProductCode">产品箱号</param>
        /// <param name="error">出错时返回错误信息，正常时为NULL</param>
        /// <returns>操作是否成功的标志</returns>
        DataTable GetTreeTable(string strProductOnlyCode, string strProductCode, out string error);

        /// <summary>
        /// 存储录入的电子档案
        /// </summary>
        /// <param name="goodsOnlyCode">零件标识码</param>
        /// <param name="dataTable">要存储的数据</param>
        /// <param name="loginName">登录名</param>
        /// <param name="error">出错时返回错误信息，正常时为NULL</param>
        /// <returns>操作是否成功的标志</returns>
        bool SaveData(string goodsOnlyCode, DataTable dataTable, string loginName, out string error);

        /// <summary>
        /// 获得所有数据的产品编码、零件唯一标识码的字段信息
        /// </summary>
        /// <returns>返回获取到的数据集</returns>
        DataTable GetAllSimpleData();

        /// <summary>
        /// 删除指定零件标识码的电子档案数据
        /// </summary>
        /// <param name="strGoodsOnlyCode">零件标识码</param>
        /// <param name="error">出错时返回错误信息，正常时为NULL</param>
        /// <returns>操作是否成功的标志</returns>
        bool DeleteData(string strGoodsOnlyCode, out string error);

        /// <summary>
        /// 添加信息到正式档案表中（主要用于返修时）
        /// </summary>
        /// <param name="ef">电子档案记录</param>
        /// <param name="error">出错时返回的错误信息</param>
        /// <returns>操作成功返回true</returns>
        bool AddElectronFile(P_ElectronFile ef, out string error);

        #region 夏石友，2012.07.12 16:00
                
        /// <summary>
        /// 获取指定产品编号根节点数据
        /// </summary>
        /// <param name="ctx">LINQ 数据上下文</param>
        /// <param name="productNumber">产品编号，示例：RDC15-FB 120700001</param>
        /// <returns>成功则返回获取到的根节点数据，失败返回null</returns>
        P_ElectronFile GetRootNode(DepotManagementDataContext ctx, string productNumber);
                
        /// <summary>
        /// 获取指定产品编号根节点数据
        /// </summary>
        /// <param name="productNumber">产品编号，示例：RDC15-FB 120700001</param>
        /// <returns>成功则返回获取到的根节点数据，失败返回null</returns>
        P_ElectronFile GetRootNode(string productNumber);

        /// <summary>
        /// 添加信息到正式档案表中
        /// </summary>
        /// <param name="ctx">LINQ 数据上下文</param>
        /// <param name="ef">电子档案记录</param>
        void AddElectronFile(DepotManagementDataContext ctx, P_ElectronFile ef);
        
        /// <summary>
        /// 生成缺省的电子档案对象
        /// </summary>
        /// <param name="productNumber">产品编号</param>
        /// <returns>返回生成的对象</returns>
        P_ElectronFile CreateElectronFile(string productNumber);

        #endregion

        /// <summary>
        /// 获取临时电子档案中的产品编码
        /// </summary>
        /// <param name="error">出错时返回的错误信息</param>
        /// <returns>成功返回获取到的信息，失败返回null</returns>
        string[] GetProductCodeOfTempElectronFile(out string error);

        /// <summary>
        /// 获取指定产品的临时电子档案信息
        /// </summary>
        /// <param name="productCode">产品编码</param>
        /// <param name="error">出错时返回的错误信息</param>
        /// <returns>成功返回获取到的信息, 失败返回null</returns>
        List<View_P_TempElectronFile> GetTempElectronFile(string productCode, out string error);
                
        /// <summary>
        /// 拷贝电子档案
        /// </summary>
        /// <param name="oldProductCode">要复制数据的模板产品编号</param>
        /// <param name="newProductCode">新产品编号</param>
        /// <param name="error">失败时返回的错误信息</param>
        /// <returns>操作是否成功的标志</returns>
        bool CopyElectronFile(string oldProductCode, string newProductCode, out string error);

        /// <summary>
        /// 根据装配BOM生成整台变速箱电子档案
        /// </summary>
        /// <param name="productType">要复制数据的模板产品编号</param>
        /// <param name="productNumber">箱号</param>
        /// <param name="error">失败时返回的错误信息</param>
        /// <returns>操作是否成功的标志</returns>
        bool GenerateElectronFile(string productType, string productNumber, out string error);

        /// <summary>
        /// 获取需要录入标识码的零件信息
        /// </summary>
        /// <returns>返回获取到的零件信息</returns>
        IQueryable<View_ZPX_PartNameWithUniqueCode> GetPartNameWithUniqueCode();
                
        /// <summary>
        /// 添加返修信息
        /// </summary>
        /// <param name="productTypeCode">产品类型编码</param>
        /// <param name="productName">产品名称</param>
        /// <param name="repairCount">返修次数</param>
        /// <param name="lstEF">返修的档案信息列表</param>
        /// <param name="error">错误时返回的错误信息</param>
        /// <returns>操作是否成功的标志</returns>
        bool AddRepairInfo(string productTypeCode, string productName, int repairCount, List<P_ElectronFile> lstEF, out string error);
                
        /// <summary>
        /// 获取变速箱从装配车间与下线车间之间的交接信息
        /// </summary>
        /// <param name="beginTime">检索开始时间</param>
        /// <param name="endTime">检索结束时间</param>
        /// <returns>返回获取到的数据</returns>
        DataTable GetCVTHandoverInfo(DateTime beginTime, DateTime endTime);
                
        /// <summary>
        /// 获取营销单据对应的产品型号,箱号信息(供生成CVT从装配车间与下线车间之间的交接信息用)
        /// </summary>
        /// <param name="billNo">营销单据号</param>
        /// <returns>返回获取到的数据</returns>
        DataTable GetProductNumberFromSellBill(string billNo);
        
        /// <summary>
        /// 保存变速箱从装配车间与下线车间之间的交接信息
        /// </summary>
        /// <param name="lstProductNumber">移交的产品箱号信息</param>
        /// <param name="destination">移交目的地</param>
        /// <param name="error">失败时返回的错误信息</param>
        /// <returns>z操作是否成功的标志</returns>
        bool SaveCVTHandoverInfo(List<string> lstProductNumber, string destination, out string error);

        /// <summary>
        /// 获取变速箱称重信息
        /// </summary>
        /// <param name="beginTime">检索开始时间</param>
        /// <param name="endTime">检索结束时间</param>
        /// <returns>返回获取到的数据</returns>
        DataTable GetCVTWeightInfo(DateTime beginTime, DateTime endTime);

        /// <summary>
        /// 获取从起始箱号到结束箱号的产品箱号列表
        /// </summary>
        /// <param name="productType">产品类型</param>
        /// <param name="productBeginNumber">起始箱号</param>
        /// <param name="productEndNumber">结束箱号</param>
        /// <returns>返回获取到的产品箱号列表</returns>
        List<string> GetProductNumber(string productType, string productBeginNumber, string productEndNumber);
        
        /// <summary>
        /// 保存气密性数据
        /// </summary>
        /// <param name="productTypeCode">产品类型编号</param>
        /// <param name="productName">产品名称</param>
        /// <param name="dicAirImpermeability">气密性字典，主键为箱号，值为气密性</param>
        /// <param name="workBench">进行气密性检测的工位</param>
        /// <param name="assemblingMode">装配模式</param>
        /// <param name="error">出错时返回的错误信息</param>
        /// <returns>操作是否成功的标志</returns>
        /// <remarks>装配车间的气密性工位为：Z19，下线车间为：Z26</remarks>
        bool SaveAirImpermeability(string productTypeCode, string productName,
            Dictionary<string, decimal> dicAirImpermeability, string workBench, string assemblingMode, out string error);
    }
}

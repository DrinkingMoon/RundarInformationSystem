/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  ElectronFileServer.cs
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
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using System.Collections;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using DBOperate;
using System.Diagnostics;
using GlobalObject;

namespace ServerModule
{
    /// <summary>
    /// 电子档案管理类
    /// </summary>
    class ElectronFileServer : IElectronFileServer
    {
        /// <summary>
        /// 数据库操作接口
        /// </summary>
        static IDBOperate m_dbOperate = CommentParameter.GetDBOperatorOfDepotManagement();

        /// <summary>
        /// BOM 映射服务组件
        /// </summary>
        IBomMappingServer m_bomMappingServer = PMS_ServerFactory.GetServerModule<IBomMappingServer>();

        /// <summary>
        /// 零件编码映射(将总成条形码生成规则表中的虚拟零件编码转换为真实零件编码)
        /// </summary>
        Dictionary<string, string> m_dicVirtualPartMapping = new Dictionary<string, string>();

        /// <summary>
        /// 检查指定产品编号是否存在
        /// </summary>
        /// <param name="productCode">产品编号</param>
        /// <returns>存在返回true, 不存在返回false</returns>
        public bool IsExists(string productCode)
        {
            DepotManagementDataContext context = CommentParameter.DepotDataContext;

            return (from r in context.P_ElectronFile
                    where r.ProductCode == productCode
                    select r.ProductCode).Take(1).Count() > 0;
        }

        /// <summary>
        /// 获得对应的产品编码的电子档案
        /// </summary>
        /// <param name="electronProductCode">产品编码</param>
        /// <returns>返回对应的产品编码的电子档案</returns>
        public DataTable GetProductElectronFile(string electronProductCode)
        {
            string strSql = "SELECT * FROM [DepotManagement].[dbo].[View_P_ElectronFile] " +
            " where 产品编码 = '" + electronProductCode
            + "' ORDER BY [产品编码],[父总成扫描码],[零部件编码]";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

            return dt;
        }

        /// <summary>
        /// 获得装配数据范围内零件图号、名称、规格、批次号不同的电子档案零件信息
        /// 包括临时电子档案、电子档案
        /// </summary>
        /// <param name="beginDate">起始装配数据</param>
        /// <param name="endDate">截止装配数据</param>
        /// <returns>返回对应的电子档案零件信息(只有零件图号、名称、规格、批次号信息)</returns>
        public DataTable GetDistinctPartInfo(DateTime beginDate, DateTime endDate)
        {
            Hashtable paramTable = new Hashtable();
            string strErr = "";

            paramTable.Add("@BeginDate", beginDate);
            paramTable.Add("@EndDate", endDate);

            return GlobalObject.DatabaseServer.QueryInfoPro(
                "ZPX_GetDistinctPartInfo", paramTable, out strErr);
        }

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
        public DataTable GetElectronFile(
            string code, string name, string spec,
            string batchNo, DateTime begin, DateTime end, out int amount)
        {
            amount = 0;

            DataSet ds = new DataSet();
            Hashtable inParams = new Hashtable();

            inParams.Add("@PartCode", code);
            inParams.Add("@PartName", name);
            inParams.Add("@PartSpec", spec);
            inParams.Add("@BatchNo", batchNo);
            inParams.Add("@BeginDate", begin);
            inParams.Add("@EndDate", end);

            Hashtable outParams = new Hashtable();

            outParams.Add("@Amount", 0);

            Dictionary<OperateCMD, object> dicOperateCMD = m_dbOperate.RunProc_CMD("ZPX_GetElectronFile", ds, inParams, ref outParams);              

            if (!Convert.ToBoolean(dicOperateCMD[OperateCMD.Return_OperateResult]))
            {
                return null;
            }
            else
            {
                amount = Convert.ToInt32(outParams["@Amount"]);

                return ds.Tables[0];
            }
        }

        /// <summary>
        /// 获得某一个箱子的某一个零件的电子档案
        /// </summary>
        /// <param name="productCode">箱号</param>
        /// <param name="code">图号</param>
        /// <param name="name">名称</param>
        /// <param name="spec">规格</param>
        /// <returns>返回信息</returns>
        public DataTable GetProductElectronFile(string productCode, string code, string name, string spec)
        {
            Hashtable paramTable = new Hashtable();

            paramTable.Add("@ProductCode", productCode);
            paramTable.Add("@GoodsCode", code);
            paramTable.Add("@GoodsName", name);
            paramTable.Add("@Spec", spec);

            string strErr = "";

            return GlobalObject.DatabaseServer.QueryInfoPro(
                "BASE_Select_P_ElectronFile_ProductMessage", paramTable, out strErr);
        }

        /// <summary>
        /// 获取父总成零件装配虚拟编码与零件图号之间的映射
        /// </summary>
        /// <param name="productTypeCode">产品类型编码</param>
        /// <returns>成功返回获取到的映射字典，失败返回null</returns>
        public Dictionary<string, string> GetVirtualPartMapping(string productTypeCode)
        {
            DepotManagementDataContext context = CommentParameter.DepotDataContext;

            var result = from r in context.P_AssemblyManagementBarcode
                         join rd in context.P_ProductInfo on Convert.ToInt32(r.ProductTypeID)
                         equals rd.ID
                         where !Nullable.Equals(r.PartCode, null) && (rd.ProductCode == productTypeCode
                         || r.PartName == "行星轮合件" || r.PartName == "液压阀块总成") && r.Remark != "RDC15-1516000"
                         select new
                         {
                             VirtualPartCode = r.PartCode,
                             PartCode = r.Remark
                         };

            if (productTypeCode == "RDC15FB-F")
            {
                result = from r in context.P_AssemblyManagementBarcode
                         join rd in context.P_ProductInfo on Convert.ToInt32(r.ProductTypeID)
                         equals rd.ID
                         where !Nullable.Equals(r.PartCode, null) && (rd.ProductCode == productTypeCode
                         || r.PartName == "行星轮合件" || r.PartName == "液压阀块总成") && r.Remark != "RDC15-1512000"
                         select new
                         {
                             VirtualPartCode = r.PartCode,
                             PartCode = r.Remark
                         };
            }

            if (result.Count() == 0)
            {
                return null;
            }

            Dictionary<string, string> dicVirtualPartMapping = new Dictionary<string, string>();

            foreach (var item in result)
            {
                dicVirtualPartMapping.Add(item.VirtualPartCode, item.PartCode);
            }

            return dicVirtualPartMapping;
        }

        /// <summary>
        /// 存储装配信息至电子档案
        /// </summary>
        /// <param name="productCode">产品编码</param>
        /// <returns>返回操作是否的标志</returns>
        public bool SaveElectronFile(string productCode)
        {
            DepotManagementDataContext context = CommentParameter.DepotDataContext;

            try
            {
                int index = productCode.IndexOf(' ');

                m_dicVirtualPartMapping.Clear();
                m_dicVirtualPartMapping = GetVirtualPartMapping(productCode.Substring(0, index));

                context.Connection.Open();
                context.Transaction = context.Connection.BeginTransaction();

                if (!ChangeTempElectronFile(context, productCode))
                {
                    context.Transaction.Rollback();
                    return false;
                }

                //if (!MappedTempElectronFileToBomTree(context, productCode))
                //{
                //    context.Transaction.Rollback();
                //    return false;
                //}

                //if (!MoveTempTableToFormalElectronFile(context, productCode))
                //{
                //    context.Transaction.Rollback();
                //    return false;
                //}

                context.SubmitChanges(ConflictMode.ContinueOnConflict);
                context.Transaction.Commit();
            }
            catch (ChangeConflictException)
            {
                context.Transaction.Rollback();
            }
            //using (System.Transactions.TransactionScope scope = new System.Transactions.TransactionScope())
            //{
            //    if (!ChangeTempElectronFile(context, productCode))
            //    {
            //        return false;
            //    }

            //    if (!MappedTempElectronFileToBomTree(context, productCode))
            //    {
            //        return false;
            //    }

            //    context.SubmitChanges(ConflictMode.ContinueOnConflict);
            //    scope.Complete();
            //}

            return true;
        }

        /// <summary>
        /// 根据虚拟图号获取零件真实图号
        /// </summary>
        /// <param name="virtualPartCode">虚拟图号</param>
        /// <returns>成功则返回获取到的零件真实图号，失败返回虚拟图号本身</returns>
        private string GetPartCode(string virtualPartCode)
        {
            if (GlobalObject.GeneralFunction.IsNullOrEmpty(virtualPartCode) || virtualPartCode.Length < 3)
            {
                return virtualPartCode;
            }

            if (m_dicVirtualPartMapping.ContainsKey(virtualPartCode.Substring(0,3)))
            {
                return m_dicVirtualPartMapping[virtualPartCode.Substring(0, 3)];
            }

            return virtualPartCode;
        }

        /// <summary>
        /// 修改临时电子档案文件，增加产品代码、完成时间等信息
        /// </summary>
        /// <param name="context">数据上下文</param>
        /// <param name="productCode">产品编码</param>
        /// <returns>操作成功返回true</returns>
        private bool ChangeTempElectronFile(DepotManagementDataContext context, string productCode)
        {
            P_TempElectronFile tempElectronFile = (from r in context.P_TempElectronFile 
                                                   where r.GoodsCode == productCode 
                                                   select r).First();

            string finishTime = tempElectronFile.FinishTime;

            var result = context.P_TempElectronFile.Where(c => c.ParentCode == productCode);

            foreach (var item in result)
            {
                item.FinishTime = finishTime;
                item.ProductCode = productCode;
                item.ParentCode = GetPartCode(item.ParentCode);
                item.GoodsCode = GetPartCode(item.GoodsCode);
                item.Remark = "OK";

                var children = context.P_TempElectronFile.Where(c => c.ParentCode == item.GoodsCode);

                if (children.Count() > 0)
                {
                    if (!ChangeTempElectronFile(context, productCode, finishTime, item.GoodsCode))
                        return false;
                }
            }

            context.SubmitChanges();
            return true;
        }

        /// <summary>
        /// 修改临时电子档案文件，增加产品代码、完成时间等信息
        /// </summary>
        /// <param name="context">数据上下文</param>
        /// <param name="productCode">产品编码</param>
        /// <param name="finishTime">完成时间</param>
        /// <param name="parentCode">父总成编码</param>
        /// <returns>操作成功返回true</returns>
        private bool ChangeTempElectronFile(DepotManagementDataContext context, string productCode, 
            string finishTime, string parentCode)
        {
            var result = context.P_TempElectronFile.Where(c => c.ParentCode == parentCode);

            foreach (var item in result)
            {
                item.FinishTime = finishTime;
                item.ProductCode = productCode;
                item.ParentCode = GetPartCode(item.ParentCode);
                item.GoodsCode = GetPartCode(item.GoodsCode);
                item.Remark = "OK";

                var children = context.P_TempElectronFile.Where(c => c.ParentCode == item.GoodsCode);

                if (children.Count() > 0)
                {
                    if (!ChangeTempElectronFile(context, productCode, finishTime, item.GoodsCode))
                        return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 将指定产品编码所属的记录从临时档案表中移动到正式档案表中
        /// </summary>
        /// <param name="context">数据上下文</param>
        /// <param name="productCode">产品编码</param>
        /// <returns>操作成功返回true</returns>
        private bool MoveTempTableToFormalElectronFile(DepotManagementDataContext context, string productCode)
        {
            var result = context.P_TempElectronFile.Where(c => c.ProductCode == productCode);

            foreach (var item in result)
            {
                P_ElectronFile ef = new P_ElectronFile();

                ef.BatchNo = item.BatchNo;
                ef.CheckDatas = item.CheckDatas;
                ef.Counts = item.Counts;
                ef.FactDatas = item.FactDatas;
                ef.FinishTime = item.FinishTime;
                ef.FittingPersonnel = item.FittingPersonnel;
                ef.FittingTime = item.FittingTime;
                ef.GoodsCode = item.GoodsCode;
                ef.GoodsName = item.GoodsName;
                ef.GoodsOnlyCode = item.GoodsOnlyCode;
                //ef.ID = item.ID;
                ef.ParentCode = item.ParentCode;
                ef.ProductCode = item.ProductCode;
                ef.Provider = item.Provider;
                ef.Remark = item.Remark;
                ef.Spec = item.Spec;
                ef.WorkBench = item.WorkBench;

                context.P_ElectronFile.InsertOnSubmit(ef);
            }

            context.P_TempElectronFile.DeleteAllOnSubmit(result);
            return true;
        }

        /// <summary>
        /// 修改临时电子档案数据使之与BOM树关系一致
        /// </summary>
        /// <param name="context">数据上下文</param>
        /// <param name="productCode">产品箱号</param>
        /// <returns>成功返回true</returns>
        private bool MappedTempElectronFileToBomTree(DepotManagementDataContext context, string productCode)
        {
            P_TempElectronFile tempElectronFile = (from r in context.P_TempElectronFile 
                                                   where r.ParentCode == null && r.GoodsCode == productCode 
                                                   select r).First();

            if (!MappedProductRecords(context, tempElectronFile.GoodsCode, tempElectronFile.GoodsName))
            {
                return false;
            }

            //IQueryable<P_TempElectronFile> tempFile = from r in tempDataContext.P_TempElectronFile where r.ParentCode == null select r;
            //foreach (var item in tempFile)
            //{
            //    if (!MappedProductRecords(tempDataContext, item.GoodsCode, item.GoodsName))
            //    {
            //        return false;
            //    }
            //}

            context.SubmitChanges();
            return true;
        }

        /// <summary>
        /// 映射用的零件信息
        /// </summary>
        struct MappedPartInfo
        {
            View_P_ProductBomMapping m_bomMapping;

            public View_P_ProductBomMapping BomMapping
            {
                get { return m_bomMapping; }
                set { m_bomMapping = value; }
            }

            /// <summary>
            /// 根据映射表中设计父总成名称对应在临时电子档案中具有唯一编码的父总成编码
            /// </summary>
            string m_designUniqueParentCode;

            public string DesignUniqueParentCode
            {
                get { return m_designUniqueParentCode; }
                set { m_designUniqueParentCode = value; }
            }

            /// <summary>
            /// 根据映射表中装配父总成名称对应在临时电子档案中具有唯一编码的父总成编码
            /// </summary>
            string m_assemblyUniqueParentCode;

            public string AssemblyUniqueParentCode
            {
                get { return m_assemblyUniqueParentCode; }
                set { m_assemblyUniqueParentCode = value; }
            }
        }

        /// <summary>
        /// 映射临时电子档案中属于指定产品的记录
        /// </summary>
        /// <param name="dataContext">数据上下文</param>
        /// <param name="productCode">产品编码</param>
        /// <param name="productName">产品名称</param>
        /// <returns>成功返回true</returns>
        private bool MappedProductRecords(DepotManagementDataContext dataContext, string productCode, string productName)
        {
            List<View_P_ProductBomMapping> bomMapping = m_bomMappingServer.GetBomMapping(productName).ToList();

            // 没有映射表
            if (bomMapping.Count() == 0)
            {
                return true;
            }

            // 在临时档案中提取所有指定产品的记录
            List<P_TempElectronFile> tempFile = (from r in dataContext.P_TempElectronFile 
                                                 where r.ProductCode == productCode 
                                                 select r).ToList();

            List<string> partCodeInmappingTable = (from  c in bomMapping 
                                                   select c.零件编码).ToList(); 

            // 获取在映射表的零件编码中存在的档案记录
            IQueryable<P_TempElectronFile> mappingTempEF = from r in dataContext.P_TempElectronFile
                                                           where r.ProductCode == productCode 
                                                           && partCodeInmappingTable.Contains(r.GoodsCode)
                                                     select r;

            #region 获取映射表所需的设计父总成编码及装配父总成名称
            List<MappedPartInfo> lstMappedPartInfo = new List<MappedPartInfo>(bomMapping.Count());

            foreach(var item in bomMapping)
            {
                 MappedPartInfo partInfo = new MappedPartInfo();
                partInfo.BomMapping = item;

                string name = (from r in tempFile where r.GoodsName == item.父总成名称 select r.GoodsCode).First();
                partInfo.DesignUniqueParentCode = name;

                name = (from r in tempFile where r.GoodsName == item.装配线总成名称 select r.GoodsCode).First();
                partInfo.AssemblyUniqueParentCode = name;

                lstMappedPartInfo.Add(partInfo);
            }
            #endregion

            // 修改临时电子档案中需要映射的记录
            foreach (var item in lstMappedPartInfo)
            {
                var result = from r in mappingTempEF
                             where r.ParentCode == item.AssemblyUniqueParentCode 
                             && r.GoodsCode == item.BomMapping.零件编码 && r.WorkBench.Contains(item.BomMapping.工位)
                             select r;

                if (result.Count() > 0)
                {
                    P_TempElectronFile tempElectronFile = result.First();

                    tempElectronFile.ParentCode = item.DesignUniqueParentCode;
                    tempElectronFile.Counts = item.BomMapping.装配数;
                }
            }

            return true;
        }

        /// <summary>
        /// 获取指定产品编号的电子档案信息
        /// </summary>
        /// <param name="productCode">产品编号</param>
        /// <param name="electronWordInfo">获取到的档案信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        public bool GetElectronFile(string productCode, out IQueryable<P_ElectronFile> electronWordInfo, out string error)
        {
            electronWordInfo = null;
            error = null;

            DepotManagementDataContext context = CommentParameter.DepotDataContext;

            var result = from r in context.P_ElectronFile
                         where r.ProductCode == productCode
                         orderby r.ParentName, r.ParentScanCode, r.GoodsName, r.GoodsCode, r.GoodsOnlyCode
                         select r;

            if (result.Count() == 0)
            {
                error = "找不到 " + productCode + " 相关的电子档案信息";
                return false;
            }
            else
            {
                electronWordInfo = result;
                return true;
            }
        }

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
        public bool GetElectronFile(string productCode, string parentCode, string parentScanCode,
            string goodsCode, string spec, out DataTable returnTable, out string error)
        {
            returnTable = null;
            error = null;

            Hashtable paramTable = new Hashtable();

            paramTable.Add("@ProductCode", productCode);
            paramTable.Add("@ParentCode", parentCode);
            paramTable.Add("@ParentScanCode", parentScanCode);
            paramTable.Add("@GoodsCode", goodsCode);
            paramTable.Add("@Spec", spec);

            DataSet ElectronDataSet = new DataSet();
            Dictionary<OperateCMD, object> dicOperateCMD = 
                m_dbOperate.RunProc_CMD("SelSomeP_ElectronFile", ElectronDataSet, paramTable);

            if (!Convert.ToBoolean(dicOperateCMD[OperateCMD.Return_OperateResult]))
            {
                error = Convert.ToString(dicOperateCMD[OperateCMD.Return_Errmsg]);
                return false;
            }

            returnTable = ElectronDataSet.Tables[0];
            return true;
        }

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
        public bool GetEspeciallyElectronFile(int type, string conditions, int pageSize, 
            int pageStartNo, out DataTable returnTable, out string error)
        {
            returnTable = null;
            error = null;

            Hashtable paramTable = new Hashtable();
            DataSet ds = new DataSet();
            Dictionary<OperateCMD, object> dicOperateCMD;

            if (type == 0) //按零件标识码查询
            {
                paramTable.Add("@GoodsOnlyCode", conditions);
                paramTable.Add("@PageSize", pageSize);
                paramTable.Add("@StartPageNO", pageStartNo);

                dicOperateCMD = m_dbOperate.RunProc_CMD("SelP_ElectronFileOrdByGoodsOnlyCode", ds, paramTable);
            }
            else //按零件编码查询
            {
                paramTable.Add("@GoodsCode", conditions);
                paramTable.Add("@PageSize", pageSize);
                paramTable.Add("@StartPageNO", pageStartNo);

                dicOperateCMD = m_dbOperate.RunProc_CMD("SelP_ElectronFileOrdByGoodsCode", ds, paramTable);
            }

            if (!Convert.ToBoolean(dicOperateCMD[OperateCMD.Return_OperateResult]))
            {
                error = Convert.ToString(dicOperateCMD[OperateCMD.Return_Errmsg]);

                if (error != "没有找到任何数据")
                {
                    return false;
                }
            }

            returnTable = ds.Tables[0];
            return true;
        }

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
        public bool GetEspeciallyElectronFile(int type, string str1, string str2, int pageSize, 
            int pageStartNo, out DataTable returnTable, out string error)
        {
            returnTable = null;
            error = null;

            Hashtable paramTable = new Hashtable();
            DataSet ds = new DataSet();
            Dictionary<OperateCMD, object> dicOperateCMD;

            if (type == 2) //按装配时间查询
            {
                paramTable.Add("@FittingTime1", str1);
                paramTable.Add("@FittingTime2", str2);
                paramTable.Add("@PageSize", pageSize);
                paramTable.Add("@StartPageNO", pageStartNo);

                dicOperateCMD = m_dbOperate.RunProc_CMD("SelP_ElectronFileOrdByFittingTime", ds, paramTable);
            }
            else //按供应商批次号查询
            {
                paramTable.Add("@ProviderCode", str1);
                paramTable.Add("@BatchNumber", str2);
                paramTable.Add("@PageSize", pageSize);
                paramTable.Add("@StartPageNO", pageStartNo);

                dicOperateCMD = m_dbOperate.RunProc_CMD("SelP_ElectronFileOrdByProviderBatchNo", ds, paramTable);
            }

            if (!Convert.ToBoolean(dicOperateCMD[OperateCMD.Return_OperateResult]))
            {
                error = Convert.ToString(dicOperateCMD[OperateCMD.Return_Errmsg]);

                if (error != "没有找到任何数据")
                {
                    return false;
                }
            }

            returnTable = ds.Tables[0];
            return true;
        }

        /// <summary>
        /// 获取电子档案中指定供应商对应的所有批次号
        /// </summary>
        /// <param name="providerCode">供应商编码</param>
        /// <param name="table">批次号信息表</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        public bool GetAllProviderBatchNo(string providerCode, out DataTable table, out string error)
        {
            table = null;
            error = null;

            Hashtable paramTable = new Hashtable();
            DataSet ds = new DataSet();

            paramTable.Add("@ProviderCode", providerCode);

            Dictionary<OperateCMD, object> dicOperateCMD = 
                m_dbOperate.RunProc_CMD("SelBatchNoFromElectronFileByProvider", ds, paramTable);

            if (!Convert.ToBoolean(dicOperateCMD[OperateCMD.Return_OperateResult]))
            {
                error = Convert.ToString(dicOperateCMD[OperateCMD.Return_Errmsg]);
                return false;
            }

            table = ds.Tables[0];
            return true;
        }

        /// <summary>
        /// 获取页面号
        /// </summary>
        /// <param name="condition">查询值</param>
        /// <param name="type">查询方式</param>
        /// <returns>数据库中符合条件的记录数</returns>
        public int GetIndexEspeciallyElectronFile(string condition, int type)
        {
            int total = 0;

            condition = "%" + condition + "%";

            if (type == 0)
            {
                try
                {
                    using (SqlConnection con = new SqlConnection(GlobalObject.GlobalParameter.StorehouseConnectionString))
                    {
                        SqlCommand cmd = new SqlCommand();

                        cmd.Connection = con;
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT COUNT(*) FROM [DepotManagement].[dbo].[P_ElectronFile] "+
                                          " WHERE GoodsOnlyCode like '" + condition + "'";
                        con.Open();
                        total = (int)(cmd.ExecuteScalar());
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                try
                {
                    using (SqlConnection con = new SqlConnection(GlobalObject.GlobalParameter.StorehouseConnectionString))
                    {
                        SqlCommand cmd = new SqlCommand();

                        cmd.Connection = con;
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT COUNT(*) FROM [DepotManagement].[dbo].[P_ElectronFile] "+
                                          " WHERE GoodsCode like '" + condition + "'";
                        con.Open();
                        total = (int)(cmd.ExecuteScalar());
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return total;
        }

        /// <summary>
        /// 获取页面号
        /// </summary>
        /// <param name="str1">起始值</param>
        /// <param name="str2">终止值</param>
        /// <param name="type">查询方式</param>
        /// <returns>数据库中符合条件的记录数</returns>
        public int GetIndexEspeciallyElectronFile(string str1, string str2, int type)
        {
            int total = 0;

            if (type == 2)
            {
                try
                {
                    using (SqlConnection con = new SqlConnection(GlobalObject.GlobalParameter.StorehouseConnectionString))
                    {
                        SqlCommand cmd = new SqlCommand();

                        cmd.Connection = con;
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT COUNT(*) FROM [DepotManagement].[dbo].[P_ElectronFile] "+
                                          " WHERE CAST(FittingTime AS DATETIME) >= '" + str1 + "'"+
                                          " AND CAST(FittingTime AS DATETIME) <=   '" + str2 + "'";
                        con.Open();
                        total = (int)(cmd.ExecuteScalar());
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
            else
            {
                try
                {
                    using (SqlConnection con = new SqlConnection(GlobalObject.GlobalParameter.StorehouseConnectionString))
                    {
                        SqlCommand cmd = new SqlCommand();
                        cmd.Connection = con;
                        cmd.CommandType = CommandType.Text;

                        if (str2 != "")
                        {
                            cmd.CommandText = "SELECT COUNT(*) FROM [DepotManagement].[dbo].[P_ElectronFile] "+
                                              " WHERE Provider = '" + str1 + "' AND BatchNo =   '" + str2 + "'";
                        }
                        else
                        {
                            cmd.CommandText = "SELECT COUNT(*) FROM [DepotManagement].[dbo].[P_ElectronFile] "+
                                              " WHERE Provider = '" + str1 + "'";
                        }

                        con.Open();
                        total = (int)(cmd.ExecuteScalar());
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return total;
        }

        /// <summary>
        /// 获取数据库中某段范围内的最后一页所有电子档案(按零件编码和零件标识码查询)
        /// </summary>
        /// <param name="condition">查询值</param>
        /// <param name="pageSize">每页显示的行数</param>
        /// <param name="type">查询方式</param>
        /// <param name="table">数据库中某段范围内的最后一页所有电子档案</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功获取数据库中某段范围内的最后一页所有电子档案</returns>
        public bool GetLastEspeciallyElectronFile(string condition, int pageSize, int type, out DataTable table, out string error)
        {
            table = null;
            error = null;

            Hashtable paramTable = new Hashtable();
            DataSet ds = new DataSet();
            Dictionary<OperateCMD, object> dicOperateCMD;

            if (type == 0)
            {
                paramTable.Add("@GoodsOnlyCode", condition);
                paramTable.Add("@PageSize", pageSize);
                dicOperateCMD = m_dbOperate.RunProc_CMD("SelLastP_ElectronFileOrdByGoodsOnlyCode", ds, paramTable);
            }
            else
            {
                paramTable.Add("@GoodsCode", condition);
                paramTable.Add("@PageSize", pageSize);
                dicOperateCMD = m_dbOperate.RunProc_CMD("SelLastP_ElectronFileOrdByGoodsCode", ds, paramTable);
            }

            if (!Convert.ToBoolean(dicOperateCMD[OperateCMD.Return_OperateResult]))
            {
                error = Convert.ToString(dicOperateCMD[OperateCMD.Return_Errmsg]);
                return false;
            }

            table = ds.Tables[0];
            return true;
        }

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
        public bool GetLastEspeciallyElectronFile(string str1, string str2, int pageSize, int type, 
            out DataTable table, out string error)
        {
            table = null;
            error = null;

            Hashtable paramTable = new Hashtable();
            DataSet ds = new DataSet();
            Dictionary<OperateCMD, object> dicOperateCMD;

            if (type == 2)
            {
                paramTable.Add("@FittingTime1", str1);
                paramTable.Add("@FittingTime2", str2);
                paramTable.Add("@PageSize", pageSize);
                dicOperateCMD = m_dbOperate.RunProc_CMD("SelLastP_ElectronFileOrdByFittingTime", ds, paramTable);
            }
            else
            {
                paramTable.Add("@ProviderCode", str1);

                if (str2 != "")
                {
                    paramTable.Add("@BatchNumber", str2);
                }
                else
                {
                    paramTable.Add("@BatchNumber", DBNull.Value);
                }

                paramTable.Add("@PageSize", pageSize);
                dicOperateCMD = m_dbOperate.RunProc_CMD("SelLastP_ElectronFileOrdByProviderBatchNo", ds, paramTable);
            }

            if (!Convert.ToBoolean(dicOperateCMD[OperateCMD.Return_OperateResult]))
            {
                error = Convert.ToString(dicOperateCMD[OperateCMD.Return_Errmsg]);
                return false;
            }

            table = ds.Tables[0];
            return true;
        }

        /// <summary>
        /// 修改电子档案行记录
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="checkData">检测数据</param>
        /// <param name="factData">实际数据</param>
        /// <param name="remark">备注</param>
        /// <param name="error">出错时返回错误信息，正常时为NULL</param>
        /// <returns>操作是否成功的标志</returns>
        public bool ModificateElectronFile(long id, string checkData, string factData, string remark, out string error)
        {
            error = null;

            Hashtable paramTable = new Hashtable();

            paramTable.Add("@ID", id);
            paramTable.Add("@CheckData", checkData);
            paramTable.Add("@FactData", factData);
            paramTable.Add("@Remark", remark);
            paramTable.Add("@Personnel", BasicInfo.LoginID);

            Dictionary<OperateCMD, object> dicOperateCMD = m_dbOperate.RunProc_CMD("UpdateP_ElectronFile", paramTable);

            if (!Convert.ToBoolean(dicOperateCMD[OperateCMD.Return_OperateResult]))
            {
                error = Convert.ToString(dicOperateCMD[OperateCMD.Return_Errmsg]);
                return false;
            }

            return true;
        }

        /// <summary>
        /// 根据产品标识码获取电子档案数据（如根据产品标识码获取电子档案数据失败则根据产品箱号获取电子档案）
        /// </summary>
        /// <param name="productOnlyCode">产品标识码</param>
        /// <param name="productCode">产品箱号</param>
        /// <param name="error">出错时返回错误信息，正常时为NULL</param>
        /// <returns>操作是否成功的标志</returns>
        public DataTable GetTreeTable(string productOnlyCode, string productCode, out string error)
        {
            DataTable dt = new DataTable();
            
            error = null;

            try
            {
                string strSql = " select ID, ProductCode,ParentCode,GoodsCode,GoodsName,Spec," +
	                            " Counts,BatchNo,CheckDatas,FactDatas,FittingPersonnel,"+
		                        " FittingTime,AmendPersonnel,AmendTime,WorkBench,Remark  "+
                                " from P_ElectronFile where GoodsOnlyCode = '" + productOnlyCode + "' order by ID";

                dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

                if (dt.Rows.Count == 0)
                {
                    strSql = "select * from P_ElectronMode where ProductCode = '" + productCode + "' order by ID";
                    dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

                    if (dt.Rows.Count == 0)
                    {
                        error = "无此产品模板";
                    }
                }

                return dt;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return null;
            }
        }

        /// <summary>
        /// 判断是否为总成
        /// 根据BOM中的总成标志判断
        /// </summary>
        /// <param name="partCode">零件编码</param>
        /// <returns>是总成则返回true</returns>
        public bool IsAssemblyPart(string partCode)
        {
            int intAssemblyFlag = 0;
            string strSql = "select AssemblyFlag from View_P_ProductBomImitate where PartCode = '" + partCode + "'";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

            if (dt.Rows.Count == 0)
            {
                return true;
            }

            intAssemblyFlag = Convert.ToInt32(dt.Rows[0][0].ToString());

            if (intAssemblyFlag == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 判断是否为保存过的记录
        /// </summary>
        /// <param name="code">要检查的编号</param>
        /// <returns>是则返回true</returns>
        private bool CheckIsSave(string code)
        {
            string strGet = code.Substring(code.Length-1, 1);

            if (strGet == ")")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 向电子档案中添加数据
        /// </summary>
        /// <param name="goodsOnlyCode">零件标识码，用于检测此零件是否已经存在</param>
        /// <param name="dataTabel">要添加的数据表</param>
        /// <param name="loginName">登录名</param>
        /// <param name="error">出错时返回错误信息，正常时为NULL</param>
        /// <returns>操作是否成功的标志</returns>
        private bool InsertData(string goodsOnlyCode, DataTable dataTabel, string loginName, out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                bool blFlag = CheckIsSave(dataTabel.Rows[0]["GoodsCode"].ToString());

                var vardate_In = from a in dataContxt.P_ElectronFile
                                 where a.GoodsOnlyCode == goodsOnlyCode
                                 select a;

                if (vardate_In.Count() != 0)
                {
                    error = "还存在数据";
                    return false;
                }
                
                List<P_ElectronFile> lisElectron = new List<P_ElectronFile>();

                for (int i = 0; i < dataTabel.Rows.Count; i++)
                {
                    P_ElectronFile lnqElectron = new P_ElectronFile();

                    if (blFlag)
                    {
                        lnqElectron.ProductCode = dataTabel.Rows[i]["ProductCode"].ToString();
                        lnqElectron.ParentCode = dataTabel.Rows[i]["ParentCode"].ToString() == "" ? 
                            null : dataTabel.Rows[i]["ParentCode"].ToString();
                        lnqElectron.GoodsCode = dataTabel.Rows[i]["GoodsCode"].ToString();
                    }
                    else
                    {
                        lnqElectron.ProductCode = dataTabel.Rows[i]["ProductCode"].ToString() + "(" + goodsOnlyCode + ")";
                        lnqElectron.ParentCode = dataTabel.Rows[i]["ParentCode"].ToString() == "" ? 
                            null : dataTabel.Rows[i]["ParentCode"].ToString() + "(" + goodsOnlyCode + ")";

                        if (IsAssemblyPart(dataTabel.Rows[i]["GoodsCode"].ToString()))
                        {
                            lnqElectron.GoodsCode = dataTabel.Rows[i]["GoodsCode"].ToString() + "(" + goodsOnlyCode + ")";
                        }
                        else
                        {
                            lnqElectron.GoodsCode = dataTabel.Rows[i]["GoodsCode"].ToString();
                        }
                    }

                    lnqElectron.GoodsName = dataTabel.Rows[i]["GoodsName"].ToString();
                    lnqElectron.Spec = dataTabel.Rows[i]["Spec"].ToString();
                    lnqElectron.GoodsOnlyCode = goodsOnlyCode;
                    lnqElectron.Counts = Convert.ToInt32( dataTabel.Rows[i]["Counts"].ToString());
                    lnqElectron.Provider = "";
                    lnqElectron.BatchNo = dataTabel.Rows[i]["BatchNo"].ToString();
                    lnqElectron.WorkBench = dataTabel.Rows[i]["WorkBench"].ToString();
                    lnqElectron.CheckDatas = dataTabel.Rows[i]["CheckDatas"].ToString();
                    lnqElectron.FactDatas = dataTabel.Rows[i]["FactDatas"].ToString();
                    lnqElectron.FittingPersonnel = dataTabel.Rows[i]["FittingPersonnel"].ToString();
                    lnqElectron.FittingTime = "";
                    //lnqElectron.AmendPersonnel = DtMode.Rows[i]["AmendPersonnel"].ToString();
                    //lnqElectron.AmendTime = "";
                    lnqElectron.Remark = dataTabel.Rows[i]["Remark"].ToString();
                    lnqElectron.FinishTime = "";
                    lnqElectron.FillInPersonnel = loginName;
                    lnqElectron.FillInDate = ServerTime.Time;

                    lisElectron.Add(lnqElectron);
                }

                dataContxt.P_ElectronFile.InsertAllOnSubmit(lisElectron);
                dataContxt.SubmitChanges();

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 删除指定零件标识码的电子档案数据
        /// </summary>
        /// <param name="goodsOnlyCode">零件唯一编码</param>
        /// <param name="error">出错时返回错误信息，正常时为NULL</param>
        /// <returns>删除成功返回True，删除失败返回False</returns>
        public bool DeleteData(string goodsOnlyCode, out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var vardate_DL = from a in dataContxt.P_ElectronFile
                                 where a.GoodsOnlyCode == goodsOnlyCode
                                 select a;

                if (vardate_DL.Count() > 0)
                {
                    dataContxt.P_ElectronFile.DeleteAllOnSubmit(vardate_DL);
                    dataContxt.SubmitChanges();
                }

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 存储录入的电子档案
        /// </summary>
        /// <param name="goodsOnlyCode">零件标识码</param>
        /// <param name="dataTable">要存储的数据</param>
        /// <param name="loginName">登录名</param>
        /// <param name="error">出错时返回错误信息，正常时为NULL</param>
        /// <returns>操作是否成功的标志</returns>
        public bool SaveData(string goodsOnlyCode, DataTable dataTable, string loginName, out string error)
        {
            error = null;

            if (!DeleteData(goodsOnlyCode, out error))
            {
                return false;
            }
            else
            {
                if (!InsertData(goodsOnlyCode, dataTable, loginName, out error))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        /// <summary>
        /// 获得所有数据的产品编码、零件唯一标识码的字段信息
        /// </summary>
        /// <returns>返回获取到的数据集</returns>
        public DataTable GetAllSimpleData()
        {
            string strSql = "select distinct ProductCode as Bom编码, GoodsOnlyCode as 产品唯一编码 from P_ElectronFile";
            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

            return dt;
        }

        /// <summary>
        /// 添加信息到正式档案表中（主要用于返修时）
        /// </summary>
        /// <param name="ef">电子档案记录</param>
        /// <param name="error">出错时返回的错误信息</param>
        /// <returns>操作成功返回true</returns>
        public bool AddElectronFile(P_ElectronFile ef, out string error)
        {
            try
            {
                error = null;

                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                dataContxt.P_ElectronFile.InsertOnSubmit(ef);
                dataContxt.SubmitChanges();

                return true;
            }
            catch (Exception exce)
            {
                error = exce.Message;
                return false;
            }
        }

        #region 夏石友，2012.07.12 16:00

        /// <summary>
        /// 获取指定产品编号根节点数据
        /// </summary>
        /// <param name="ctx">LINQ 数据上下文</param>
        /// <param name="productNumber">产品编号，示例：RDC15-FB 120700001</param>
        /// <returns>成功则返回获取到的根节点数据，失败返回null</returns>
        public P_ElectronFile GetRootNode(DepotManagementDataContext ctx, string productNumber)
        {
            var result = from r in ctx.P_ElectronFile
                         where r.ProductCode == productNumber && r.ParentCode == ""
                         select r;

            if (result.Count() == 0)
            {
                return null;
            }
            else
            {
                return result.Single();
            }
        }

        /// <summary>
        /// 获取指定产品编号根节点数据
        /// </summary>
        /// <param name="productNumber">产品编号，示例：RDC15-FB 120700001</param>
        /// <returns>成功则返回获取到的根节点数据，失败返回null</returns>
        public P_ElectronFile GetRootNode(string productNumber)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            return GetRootNode(ctx, productNumber);
        }

        /// <summary>
        /// 添加信息到正式档案表中
        /// </summary>
        /// <param name="ctx">LINQ 数据上下文</param>
        /// <param name="ef">电子档案记录</param>
        public void AddElectronFile(DepotManagementDataContext ctx, P_ElectronFile ef)
        {
            ctx.P_ElectronFile.InsertOnSubmit(ef);
            ctx.SubmitChanges();
        }

        /// <summary>
        /// 生成缺省的电子档案对象
        /// </summary>
        /// <param name="productNumber">产品编号</param>
        /// <returns>返回生成的对象</returns>
        public P_ElectronFile CreateElectronFile(string productNumber)
        {
            P_ElectronFile item = new P_ElectronFile();
            DateTime serverTime = ServerTime.Time;

            item.ProductCode = productNumber;
            item.ParentCode = "";
            item.ParentName = "";
            item.ParentScanCode = "";
            item.GoodsCode = "";
            item.GoodsName = "";
            item.Spec = "";
            item.GoodsOnlyCode = "";
            item.Counts = 1;
            item.WorkBench = "";
            item.Provider = "";
            item.BatchNo = "";
            item.CheckDatas = "";
            item.FactDatas = "";
            item.FittingPersonnel = GlobalObject.BasicInfo.LoginID;
            item.FittingTime = serverTime.ToString("yyyy-MM-dd HH:mm:ss");
            item.FinishTime = item.FittingTime;
            item.FillInPersonnel = GlobalObject.BasicInfo.LoginID;
            item.FillInDate = serverTime;
            item.AssemblingMode = "正常装配";
            item.Remark = "";

            return item;
        }

        #endregion

        #region 2011-12-11 增加，夏石友，原因：新增的“转换临时档案窗体”所需

        /// <summary>
        /// 获取临时电子档案中的产品编码
        /// </summary>
        /// <param name="error">出错时返回的错误信息</param>
        /// <returns>成功返回获取到的信息，失败返回null</returns>
        public string[] GetProductCodeOfTempElectronFile(out string error)
        {
            try
            {
                error = null;
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                return (from r in dataContxt.P_TempElectronFile
                        where r.ParentCode == ""
                        select r.ProductCode).ToArray();
            }
            catch (Exception exce)
            {
                error = exce.Message;
                return null;
            }
        }

        /// <summary>
        /// 获取指定产品的临时电子档案信息
        /// </summary>
        /// <param name="productCode">产品编码</param>
        /// <param name="error">出错时返回的错误信息</param>
        /// <returns>成功返回获取到的信息, 失败返回null</returns>
        public List<View_P_TempElectronFile> GetTempElectronFile(string productCode, out string error)
        {
            try
            {
                error = null;

                DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

                List<View_P_TempElectronFile> lstInfo = (from r in ctx.View_P_TempElectronFile
                                                         where r.父总成编码 == "" && r.产品编码 == productCode
                                                         select r).ToList();

                if (lstInfo.Count > 0)
                {
                    GetTempElectronFile(ctx, lstInfo, lstInfo[0].产品编码);
                }

                return lstInfo;
            }
            catch (Exception exce)
            {
                error = exce.Message;
                return null;
            }
        }

        /// <summary>
        /// 获取指定产品的临时电子档案信息
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="assemblyCode">父总成扫描码</param>
        /// <param name="lstInfo">临时电子档案信息</param>
        /// <returns>成功返回获取到的信息</returns>
        private void GetTempElectronFile(DepotManagementDataContext ctx, List<View_P_TempElectronFile> lstInfo, string assemblyCode)
        {
            List<View_P_TempElectronFile> result = (from r in ctx.View_P_TempElectronFile
                                                    where r.父总成扫描码 == assemblyCode
                                                    select r).ToList();

            for (int i = 0; i < result.Count; i++)
            {
                lstInfo.Add(result[i]);

                if (result[i].总成标志 == 1)
                    GetTempElectronFile(ctx, lstInfo, result[i].零件扫描码);
            }
        }

        #endregion

        #region 2011-12-15 增加，夏石友，原因：新增的“维修变速箱”窗体所需

        /// <summary>
        /// 获取从起始索引开始的指定行数的电子档案信息
        /// </summary>
        /// <typeparam name="T">只能为 View_P_ElectronFile 或者 P_ElectronFile</typeparam>
        /// <param name="startIndex">起始索引</param>
        /// <param name="recordAmount">记录数量, 如果为-1，则表示获取从起始索引开始的所有信息</param>
        /// <param name="electronWordInfo">获取到的档案信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        public bool GetElectronFile<T>(int startIndex, int recordAmount, out IQueryable<T> electronWordInfo, out string error)
        {
            Debug.Assert(startIndex >= 0, "获取电子档案信息的起始索引必须 >= 0");

            electronWordInfo = null;
            error = null;

            DepotManagementDataContext context = CommentParameter.DepotDataContext;
            IQueryable<T> result = null;

            if (recordAmount != -1)
            {
                if (typeof(T).FullName == typeof(View_P_ElectronFile).FullName)
                {
                    result = (IQueryable<T>)(from r in context.View_P_ElectronFile
                                             orderby r.父总成编码, r.零部件编码, r.规格
                                             select r).Skip(startIndex).Take(recordAmount);
                }
                else
                {
                    result = (IQueryable<T>)(from r in context.P_ElectronFile
                                             orderby r.ParentCode, r.GoodsCode, r.Spec
                                             select r).Skip(startIndex).Take(recordAmount);
                }
            }
            else
            {
                if (typeof(T).FullName == typeof(View_P_ElectronFile).FullName)
                {
                    result = (IQueryable<T>)(from r in context.View_P_ElectronFile
                                             orderby r.父总成编码, r.零部件编码, r.规格
                                             select r).Skip(startIndex);
                }
                else
                {
                    result = (IQueryable<T>)(from r in context.P_ElectronFile
                                             orderby r.ParentCode, r.GoodsCode, r.Spec
                                             select r).Skip(startIndex);
                }
            }

            if (recordAmount != 0 && result.Count() == 0)
            {
                error = "找不到所需的电子档案信息";
                return false;
            }
            else
            {
                electronWordInfo = result;
                return true;
            }
        }

        /// <summary>
        /// 获取指定产品编号的电子档案信息
        /// </summary>
        /// <param name="productCode">产品编号</param>
        /// <param name="electronWordInfo">获取到的档案信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        public bool GetElectronFile(string productCode, out IQueryable<View_P_ElectronFile> electronWordInfo, out string error)
        {
            electronWordInfo = null;
            error = null;

            DepotManagementDataContext context = CommentParameter.DepotDataContext;

            var result = from r in context.View_P_ElectronFile
                         where r.产品编码 == productCode
                         orderby r.父总成编码, r.零部件编码, r.规格, r.返修次数
                         select r;

            if (result.Count() == 0)
            {
                error = "找不到 " + productCode + " 相关的电子档案信息";
                return false;
            }
            else
            {
                electronWordInfo = result;
                return true;
            }
        }

        #endregion

        #region 2011-12-15 增加，夏石友，原因：电子档案窗体，复制电子档案基础信息

        /// <summary>
        /// 拷贝电子档案
        /// </summary>
        /// <param name="oldProductCode">要复制数据的模板产品编号</param>
        /// <param name="newProductCode">新产品编号</param>
        /// <param name="error">失败时返回的错误信息</param>
        /// <returns>操作是否成功的标志</returns>
        public bool CopyElectronFile(string oldProductCode, string newProductCode, out string error)
        {
            try
            {
                error = null;

                IQueryable<P_ElectronFile> oldEF = null;

                if (!GetElectronFile(oldProductCode, out oldEF, out error))
                    return false;

                if (oldEF.Count() == 0)
                {
                    error = "电子档案中找不到旧箱信息，检查操作是否有误，无法继续";
                    return false;
                }

                if (IsExists(newProductCode))
                {
                    error = "电子档案中已经存在新箱信息，检查操作是否有误，无法继续";
                    return false;
                }

                DepotManagementDataContext ctx = CommentParameter.DepotDataContext;
                string[] code = newProductCode.Split(new char[] { ' ' });

                DateTime dt = new DateTime(2000 + Convert.ToInt32(code[1].Substring(0, 2)),
                    Convert.ToInt32(code[1].Substring(2, 2)), 1);

                DateTime serverTime = ServerTime.Time;

                foreach (var item in oldEF)
                {
                    P_ElectronFile data = GlobalObject.CloneObject.CloneProperties<P_ElectronFile>(item);

                    data.ProductCode = newProductCode;

                    if (item.GoodsCode == oldProductCode)
                    {
                        data.GoodsCode = code[0];
                    }

                    if (item.ParentScanCode == oldProductCode)
                    {
                        data.ParentScanCode = newProductCode;
                    }

                    if (item.GoodsOnlyCode == oldProductCode)
                    {
                        data.GoodsOnlyCode = newProductCode;
                    }

                    data.Provider = "";
                    data.BatchNo = "";
                    data.CheckDatas = "";
                    data.FactDatas = "";
                    data.FittingPersonnel = "";
                    data.FittingTime = dt.ToString("yyyy-MM-dd HH:mm:ss");
                    data.FinishTime = dt.ToString("yyyy-MM-dd HH:mm:ss");
                    data.FillInPersonnel = GlobalObject.BasicInfo.LoginID;
                    data.FillInDate = serverTime;
                    data.AssemblingMode = "正常装配";
                    data.Remark = string.Format("{0}操作员基于 {1} 复制。", BasicInfo.LoginID, oldProductCode);

                    ctx.P_ElectronFile.InsertOnSubmit(data);
                }

                ctx.SubmitChanges();
                return true;
            }
            catch (Exception exce)
            {
                error = exce.Message;
                return false;
            }
        }

        /// <summary>
        /// 根据装配BOM生成整台变速箱电子档案
        /// </summary>
        /// <param name="productType">要复制数据的模板产品编号</param>
        /// <param name="productNumber">箱号</param>
        /// <param name="error">失败时返回的错误信息</param>
        /// <returns>操作是否成功的标志</returns>
        public bool GenerateElectronFile(string productType, string productNumber, out string error)
        {
            try
            {
                error = null;

                //IQueryable<P_ElectronFile> oldEF = null;

                if (IsExists(productNumber))
                {
                    error = "电子档案中已经存在“" + productNumber + "”的信息，无法继续";
                    return false;
                }

                // 获取装配BOM
                IAssemblingBom assemblingBomServer = PMS_ServerFactory.GetServerModule<IAssemblingBom>();
                List<View_P_AssemblingBom> lstAssemblingBomInfo = assemblingBomServer.GetAssemblingBom(productType);

                if (lstAssemblingBomInfo == null || lstAssemblingBomInfo.Count == 0)
                {
                    error = "装配BOM中不存在“" + productType + "”的信息，无法继续";
                    return false;
                }

                DepotManagementDataContext ctx = CommentParameter.DepotDataContext;
                string[] code = productNumber.Split(new char[] { ' ' });

                // 获取选配零件图号
                IQueryable<string> optionParts = PMS_ServerFactory.GetServerModule<IChoseConfectServer>().GetOptionPartCode();

                foreach (var item in lstAssemblingBomInfo)
                {
                    if (optionParts.Contains(item.零件编码))
                        continue;

                    P_ElectronFile data = CreateElectronFile(productNumber);

                    if (item.父总成编码 != null)
                    {
                        data.ParentCode = item.父总成编码;
                        data.ParentName = item.父总成名称;
                        data.ParentScanCode = item.父总成编码;
                    }

                    if (item.父总成编码 == productType)
                    {
                        data.ParentScanCode = productNumber;
                    }

                    data.GoodsCode = item.零件编码;
                    data.GoodsName = item.零件名称;
                    data.Spec = item.规格;

                    if (item.零件编码 == productType)
                    {
                        data.GoodsOnlyCode = productNumber;
                    }

                    data.Counts = item.装配数量;
                    data.WorkBench = item.工位;
                    data.Remark = "基于装配BOM生成";

                    ctx.P_ElectronFile.InsertOnSubmit(data);
                }

                ctx.SubmitChanges();
                return true;
            }
            catch (Exception exce)
            {
                error = exce.Message;
                return false;
            }
        }

        #endregion

        #region 2012-04-26 增加, 夏石友, 原因：新增的“快速返修变速箱”窗体所需

        /// <summary>
        /// 获取需要录入标识码的零件信息
        /// </summary>
        /// <returns>返回获取到的零件信息</returns>
        public IQueryable<View_ZPX_PartNameWithUniqueCode> GetPartNameWithUniqueCode()
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            return from r in dataContxt.View_ZPX_PartNameWithUniqueCode
                   select r;
        }

        /// <summary>
        /// 获取包含检测数据的可选配的零件信息（用于做为模板）
        /// </summary>
        /// <param name="productTypeCode">产品类型编码</param>
        /// <param name="partCode">选配件图号</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回获取到的零件信息，失败返回null</returns>
        public View_P_ElectronFile GetOptionPartInfo(string productTypeCode, string partCode,out string error)
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            error = null;
            productTypeCode += " ";

            try
            {
                return (from r in dataContxt.View_P_ElectronFile
                        where r.产品编码.Contains(productTypeCode) 
                        && r.零部件编码 == partCode && r.检测数据 != "" && r.实际数据 != ""
                        orderby Convert.ToDateTime(r.装配时间) descending
                        select r).First();
            }
            catch (Exception exce)
            {
                error = exce.Message;
                return null;
            }
        }

        /// <summary>
        /// 添加返修信息
        /// </summary>
        /// <param name="productTypeCode">产品类型编码</param>
        /// <param name="productName">产品名称</param>
        /// <param name="repairCount">返修次数</param>
        /// <param name="lstEF">返修的档案信息列表</param>
        /// <param name="error">错误时返回的错误信息</param>
        /// <returns>操作是否成功的标志</returns>
        public bool AddRepairInfo(string productTypeCode, string productName, int repairCount, List<P_ElectronFile> lstEF, out string error)
        {
            error = null;

            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            try
            {
                ctx.Connection.Open();
                ctx.Transaction = ctx.Connection.BeginTransaction();

                P_ElectronFile firstItem = lstEF[0];
                P_ElectronFile rootEF = lstEF.Find(p => p.GoodsName == productName);
                P_ElectronFile root = null;

                string rootRemark = "快速返修时由系统自动添加";

                IQueryable<P_ElectronFile> rootResult = from r in ctx.P_ElectronFile
                                                        where r.ProductCode == firstItem.ProductCode && r.GoodsName == productName
                                                        select r;

                if (rootEF != null)
                {
                    rootRemark = rootEF.Remark;
                    lstEF.Remove(rootEF);
                }

                if (rootResult.Count() > 0)
                {
                    root = rootResult.Single();

                    root.Remark += string.Format("；【返修信息：工号[{0}], 时间[{1}], 说明[{2}]】",
                        firstItem.FittingPersonnel, firstItem.FittingTime, rootRemark);
                }
                else
                {
                    root = CreateElectronFile(firstItem.ProductCode);

                    root.GoodsCode = productTypeCode;
                    root.GoodsName = productName;
                    root.GoodsOnlyCode = root.ProductCode;
                    root.WorkBench = "Z02";
                    root.AssemblingMode = firstItem.AssemblingMode;
                    root.Remark = rootRemark;

                    ctx.P_ElectronFile.InsertOnSubmit(root);

                    ctx.SubmitChanges();
                }

                var ri = from r in ctx.ZPX_RepairInfoOfCVT
                         where r.ElectronFileID == root.ID
                         select r;

                if (ri.Count() > 0)
                {
                    ri.Single().RepairCount = repairCount;
                }
                else
                {
                    ZPX_RepairInfoOfCVT data = new ZPX_RepairInfoOfCVT();

                    data.ElectronFileID = root.ID;
                    data.RepairCount = repairCount;

                    ctx.ZPX_RepairInfoOfCVT.InsertOnSubmit(data);
                }

                if (lstEF.Count > 0)
                {
                    ctx.P_ElectronFile.InsertAllOnSubmit(lstEF);
                    ctx.SubmitChanges(ConflictMode.ContinueOnConflict);

                    foreach (var item in lstEF)
                    {
                        ZPX_RepairInfoOfCVT data = new ZPX_RepairInfoOfCVT();

                        data.ElectronFileID = item.ID;
                        data.RepairCount = repairCount;

                        ctx.ZPX_RepairInfoOfCVT.InsertOnSubmit(data);
                    }
                }

                ctx.SubmitChanges(ConflictMode.ContinueOnConflict);
                ctx.Transaction.Commit();

                return true;
            }
            catch (ChangeConflictException exce)
            {
                error = exce.Message;

                ctx.Transaction.Rollback();
                ctx.Connection.Close();
                return false;
            }
            catch (Exception exce)
            {
                error = exce.Message;

                ctx.Transaction.Rollback();
                ctx.Connection.Close();
                return false;
            }
        }

        #endregion

        #region 2012-06-11 增加, 夏石友, 原因：新增的“查看变速箱交接信息”窗体所需

        /// <summary>
        /// 获取变速箱从装配车间与下线车间之间的交接信息
        /// </summary>
        /// <param name="beginTime">检索开始时间</param>
        /// <param name="endTime">检索结束时间</param>
        /// <returns>返回获取到的数据</returns>
        public DataTable GetCVTHandoverInfo(DateTime beginTime, DateTime endTime)
        {
            Hashtable htParams = new Hashtable();

            htParams.Add("@BeginTime", beginTime.Date);
            htParams.Add("@EndTime", endTime.Date);

            string error;
            DataSet ds = new DataSet();

            AccessDB.SetDBOperate("DepotManagement");
            AccessDB.ExecuteDbProcedure("ZPX_GetCVTHandoverInfo", htParams, ds, out error);

            return ds.Tables[0];
        }

        /// <summary>
        /// 获取营销单据对应的产品型号,箱号信息(供生成CVT从装配车间与下线车间之间的交接信息用)
        /// </summary>
        /// <param name="billNo">营销单据号</param>
        /// <returns>返回获取到的数据</returns>
        public DataTable GetProductNumberFromSellBill(string billNo)
        {
            Hashtable htParams = new Hashtable();

            htParams.Add("@BillNo", billNo);

            string error;
            DataSet ds = new DataSet();

            AccessDB.SetDBOperate("DepotManagement");
            AccessDB.ExecuteDbProcedure("ZPX_GetProductNumberFromSellBill", htParams, ds, out error);

            return ds.Tables[0];
        }

        /// <summary>
        /// 保存变速箱从装配车间与下线车间之间的交接信息
        /// </summary>
        /// <param name="lstProductNumber">移交的产品箱号信息</param>
        /// <param name="destination">移交目的地</param>
        /// <param name="error">失败时返回的错误信息</param>
        /// <returns>z操作是否成功的标志</returns>
        public bool SaveCVTHandoverInfo(List<string> lstProductNumber, string destination, out string error)
        {
            string[] arrayCMD = new string[lstProductNumber.Count];
            Hashtable[] arrayInParam = new Hashtable[lstProductNumber.Count];
            Hashtable[] arrayOutParam = new Hashtable[lstProductNumber.Count];

            error = null;

            for (int i = 0; i < lstProductNumber.Count; i++)
            {
                arrayCMD[i] = "ZPX_AddCVTHandoverInfo";

                Hashtable htParam = new Hashtable();

                htParam.Add("@ProductNumber", lstProductNumber[i]);
                htParam.Add("@UserCode", BasicInfo.LoginID);
                htParam.Add("@ToPlace", destination);

                arrayInParam[i] = htParam;
            }

            try
            {
                m_dbOperate = AccessDB.GetIDBOperate("DepotManagement");

                Dictionary<OperateCMD, object> dicOperateCMD = m_dbOperate.Transaction_CMD(arrayCMD, arrayInParam, ref arrayOutParam);

                if (!Convert.ToBoolean(dicOperateCMD[OperateCMD.Return_OperateResult]))
                {
                    error = Convert.ToString(dicOperateCMD[OperateCMD.Return_Errmsg]);
                    return false;
                }

                return true;

            }
            catch (Exception exce)
            {
                error = exce.Message;
                return false;
            }
        }

        #endregion 

        #region 2012-06-11 增加, 夏石友, 原因：新增的“查看变速箱称重信息”窗体所需

        /// <summary>
        /// 获取变速箱称重信息
        /// </summary>
        /// <param name="beginTime">检索开始时间</param>
        /// <param name="endTime">检索结束时间</param>
        /// <returns>返回获取到的数据</returns>
        public DataTable GetCVTWeightInfo(DateTime beginTime, DateTime endTime)
        {
            Hashtable htParams = new Hashtable();

            htParams.Add("@BeginTime", beginTime.Date);
            htParams.Add("@EndTime", endTime.Date);

            string error;
            DataSet ds = new DataSet();

            AccessDB.SetDBOperate("DepotManagement");
            AccessDB.ExecuteDbProcedure("ZPX_GetCVTWeightInfo", htParams, ds, out error);

            return ds.Tables[0];
        }

        #endregion

        #region 2014-09-30 夏石友 增加气密性设置功能时增加

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
        public bool SaveAirImpermeability(string productTypeCode, string productName,
            Dictionary<string, decimal> dicAirImpermeability, string workBench, string assemblingMode, out string error)
        {
            List<Hashtable> lstParam = new List<Hashtable>();
            List<string> lstCmd = new List<string>();

            foreach (KeyValuePair<string, decimal> var in dicAirImpermeability)
            {
                lstCmd.Add("ZPX_SetAirImpermeabilityForQuickRepairMode");

                string checkData = string.Format("气密性泄露量：{0} Pa", var.Value);
                string factData = checkData;

                Hashtable paramTable = new Hashtable();

                paramTable.Add("@ProductCode", var.Key);
                paramTable.Add("@GoodsCode", productTypeCode);
                paramTable.Add("@GoodsName", productName);
                paramTable.Add("@WorkBench", workBench);
                paramTable.Add("@CheckDatas", checkData);
                paramTable.Add("@FactDatas", factData);
                paramTable.Add("@FittingPersonnel", BasicInfo.LoginID);
                paramTable.Add("@AssemblingMode", assemblingMode);

                lstParam.Add(paramTable);
            }

            Hashtable[] outParams = new Hashtable[lstCmd.Count];
            Dictionary<OperateCMD, object> dicOperateCMD = m_dbOperate.Transaction_CMD(lstCmd.ToArray(), lstParam.ToArray(), ref outParams);

            return AccessDB.GetResult(dicOperateCMD, out error);
        }

        /// <summary>
        /// 获取从起始箱号到结束箱号的产品箱号列表
        /// </summary>
        /// <param name="productType">产品类型</param>
        /// <param name="productBeginNumber">起始箱号</param>
        /// <param name="productEndNumber">结束箱号</param>
        /// <returns>返回获取到的产品箱号列表</returns>
        public List<string> GetProductNumber(string productType, string productBeginNumber, string productEndNumber)
        {
            if (GlobalObject.GeneralFunction.IsNullOrEmpty(productType))
            {
                throw new ArgumentException("GetProductNumber: productType 不允许为空");
            }

            if (GlobalObject.GeneralFunction.IsNullOrEmpty(productBeginNumber))
            {
                throw new ArgumentException("GetProductNumber: productBeginNumber 不允许为空");
            }
           
            string error;

            if (!SCM_Level02_ServerFactory.GetServerModule<IProductCodeServer>().VerifyProductCodesInfo(
                productType, productBeginNumber, GlobalObject.CE_BarCodeType.内部钢印码, out error))
            {
                throw new Exception(error);
            }

            List<string> lstNumber = new List<string>();

            if (GlobalObject.GeneralFunction.IsNullOrEmpty(productEndNumber))
            {
                lstNumber.Add(string.Format("{0} {1}", productType, productBeginNumber));
                return lstNumber;
            }

            if (!SCM_Level02_ServerFactory.GetServerModule<IProductCodeServer>().VerifyProductCodesInfo(
                productType, productEndNumber, GlobalObject.CE_BarCodeType.内部钢印码, out error))
            {
                throw new Exception(error);
            }

            string tempNumber = productBeginNumber.Trim().ToUpper();

            int beginNumber = 0;

            int endNumber = 0;

            string prefix = tempNumber.Substring(0, 4);

            string postfix = "";

            for (int i = 4; i < tempNumber.Length; i++)
            {
                if (tempNumber[i] >= '0' && tempNumber[i] <= '9')
                {
                    break;
                }
                else
                {
                    prefix += tempNumber[i];
                }
            }

            tempNumber = tempNumber.Substring(prefix.Length);

            for (int i = tempNumber.Length - 1; i > 1; i--)
            {
                if (tempNumber[i] > '9')
                {
                    postfix = postfix.Insert(0, tempNumber[i].ToString());
                }
                else
                {
                    break;
                }
            }

            tempNumber = tempNumber.Substring(0, tempNumber.Length - postfix.Length);

            int len = tempNumber.Length;

            beginNumber = Convert.ToInt32(tempNumber);

            tempNumber = productEndNumber.Trim().ToUpper();

            if (!GlobalObject.GeneralFunction.IsNullOrEmpty(tempNumber))
            {
                endNumber = Convert.ToInt32(tempNumber.Substring(prefix.Length, len));
            }
            else
            {
                endNumber = beginNumber;
            }

            string format = "{0} {1}{2:d" + len.ToString() + "}{3}";

            for (int i = beginNumber; i <= endNumber; i++)
            {
                lstNumber.Add(string.Format(format, productType, prefix, i, postfix));
            }

            return lstNumber;
        }
        #endregion
    }
}

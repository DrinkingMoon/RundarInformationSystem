/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  BomServer.cs
 * 作者    :  夏石友    版本: v1.00    日期: 2009/06/15
 * 开发平台:  vs2005(c#)
 * 用于    :  生产线管理信息系统
 *----------------------------------------------------------------------------
 * 描述 : Bom服务组件类
 * 其它 :
 *----------------------------------------------------------------------------
 * 历史记录:
 *     1. 日期时间: 2009/07/03 08:02:08 作者: 夏石友 当前版本: V1.00
 *        修改说明: 创建
 ******************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Collections;
using DBOperate;
using System.Linq;
using GlobalObject;

namespace ServerModule
{
    /// <summary>
    /// Bom类
    /// </summary>
    class BomServer : IBomServer
    {
        /// <summary>
        /// 数据库操作接口
        /// </summary>
        static IDBOperate m_dbOperate = CommentParameter.GetDBOperatorOfDepotManagement();

        /// <summary>
        /// Bom
        /// </summary>
        static Dictionary<string, List<Bom>> m_dicBom = new Dictionary<string, List<Bom>>();

        /// <summary>
        /// 获得产品型号列表
        /// </summary>
        /// <returns>返回列表</returns>
        public List<string> GetAssemblyTypeList()
        {
            using (DepotManagementDataContext ctx = CommentParameter.DepotDataContext)
            {
                Dictionary<CE_GoodsAttributeName, String> dicCondition = new Dictionary<CE_GoodsAttributeName, string>();
                dicCondition.Add(CE_GoodsAttributeName.TCU, "True");
                dicCondition.Add(CE_GoodsAttributeName.CVT, "True");

                var varData = (from a in UniversalFunction.GetGoodsInfoList_Attribute(ctx, dicCondition)
                               join b in ctx.BASE_BomStruct
                               on a.ID equals b.ParentID
                               select a.GoodsCode).Distinct().OrderBy(k => k);

                if (varData != null && varData.Count() > 0)
                {
                    return varData.ToList();
                }
                else
                {
                    return new List<string>();
                }
            }
        }

        /// <summary>
        /// 更新BOM零件库信息（版次号除外）
        /// </summary>
        /// <param name="bomPartsLibrary">LINQ数据集</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True,失败返回False</returns>
        public bool UpdateBOMPartsLibrary(BASE_BomPartsLibrary bomPartsLibrary,out string error)
        {
            error = null;

            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.BASE_BomPartsLibrary
                          where a.GoodsID == bomPartsLibrary.GoodsID
                          select a;

            if (varData.Count() != 1)
            {
                error = "此零件在BOM零件库数据错误";
                return false;
            }
            else
            {
                BASE_BomPartsLibrary lnqLibrary = varData.Single();

                lnqLibrary.Material = bomPartsLibrary.Material;
                lnqLibrary.PartType = bomPartsLibrary.PartType;
                lnqLibrary.PivotalPart = bomPartsLibrary.PivotalPart;
                lnqLibrary.Remark = bomPartsLibrary.Remark;
                lnqLibrary.CreateDate = ServerTime.Time;
                lnqLibrary.CreatePersonnel = bomPartsLibrary.CreatePersonnel;
                
            }

            ctx.SubmitChanges();

            return true;
        }

        /// <summary>
        /// 获得BOM表零件库信息
        /// </summary>
        /// <returns>返回TABLE</returns>
        public DataTable GetBOMPartsLibrary()
        {
            string strSql = "select * from View_BASE_BomPartsLibrary";

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        /// <summary>
        /// 获得BOM结构表某一条记录
        /// </summary>
        /// <param name="parentID">父级ID</param>
        /// <param name="goodsID">物品ID</param>
        /// <returns>返回Table</returns>
        public DataTable GetBomStructInfo(int parentID, int goodsID)
        {
            string strSql = "select * from BASE_BomStruct where GoodsID = " + goodsID + " and ParentID =" + parentID;

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        /// <summary>
        /// 获得大多数物品基数
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <returns>返回基数</returns>
        public decimal GetAllMostUsage(int goodsID)
        {
            string strSql =@"select * from (	select GoodsID,Usage,COUNT(*) as Counts 
				                                from (	select Edition,b.序号 as GoodsID,SUM(Counts) as Usage 
						                                from fun_Imitate_ProductBom()  as a 
							                                inner join View_F_GoodsPlanCost as b on a.PartCode = b.图号型号 
											                                and a.PartName = b.物品名称 
											                                and a.Spec = b.规格
						                                where b.序号 = "+ goodsID +@" group by Edition,b.序号
					                                  ) as a 
				                                group by GoodsID,Usage
			                                ) as a 
                                order by Counts desc";

            DataTable dtTemp = GlobalObject.DatabaseServer.QueryInfo(strSql);

            if (dtTemp.Rows.Count == 0)
            {
                return 0;
            }
            else
            {
                return Convert.ToDecimal(dtTemp.Rows[0]["Usage"]);
            }
        }

        /// <summary>
        /// 获得对应产品型号的BOM版次号
        /// </summary>
        /// <param name="productCode">产品型号</param>
        /// <returns>返回TABLE</returns>
        public DataTable GetBomBackUpBomEdtion(string productCode)
        {
            string strSql = "select distinct b.SysVersion as BOM版次号 from fun_get_BomTree('" + productCode + "') as a " +
                " left join BASE_BomStruct_BackUp as b on a.ParentID = b.ParentID";

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        /// <summary>
        /// 获得BOM历史记录
        /// </summary>
        /// <param name="productCode">产品型号</param>
        /// <param name="bomEdition">BOM版次号</param>
        /// <returns>返回DataTable</returns>
        public DataTable GetBomBackUpInfo(string productCode,string bomEdition)
        {
            
            string error = "";

            Hashtable hsTable = new Hashtable();

            hsTable.Add("@Edition", productCode);
            hsTable.Add("@SysVersion", bomEdition);

            return GlobalObject.DatabaseServer.QueryInfoPro("SelAllP_ProductBom_History", hsTable, out error);

        }

        /// <summary>
        /// 获得版本信息
        /// </summary>
        /// <param name="goodsCode">图号型号</param>
        /// <param name="goodsName">零件名称</param>
        /// <returns>满足条件的版次号</returns>
        public DataTable GetVersion(string goodsCode, string goodsName)
        {
            string strSql = "select distinct 旧零件版次号 from (" +
                            " SELECT distinct 旧零件编码, 旧零件名称,旧零件版次号" +
                            " FROM View_S_TechnologyChangeBill" +
                            " union all" +
                            " select b.图号型号 as GoodsCode,b.物品名称 as GoodsName,Version  from  BASE_BomPartsLibrary as a  " +
                            " inner join View_F_GoodsPlanCost as b on a.GoodsID = b.序号 ) as a " +
                            " where 旧零件编码 = '" + goodsCode + "' and 旧零件名称 = '" + goodsName + "'" +
                            " order by 旧零件版次号 desc";

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        /// <summary>
        /// 获得BOM表的信息
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="code">图号型号</param>
        /// <param name="name">物品名称</param>
        /// <returns>返回Table</returns>
        public DataRow GetBomInfo(DepotManagementDataContext ctx, string code, string name)
        {
            var varData = from a in ctx.BASE_BomPartsLibrary
                          join b in ctx.View_F_GoodsPlanCost
                          on a.GoodsID equals b.序号
                          where b.图号型号 == code && b.物品名称 == name
                          select new
                          {
                              PartCode = b.图号型号,
                              PartName = b.物品名称,
                              Version = a.Version
                          };

            if (varData == null || varData.Count() == 0)
            {
                return null;
            }
            else
            {
                DataTable dtResult = new DataTable();

                dtResult.Columns.Add("PartCode");
                dtResult.Columns.Add("PartName");
                dtResult.Columns.Add("Version");

                DataRow dr = dtResult.NewRow();

                dr["PartCode"] = varData.First().PartCode;
                dr["PartName"] = varData.First().PartName;
                dr["Version"] = varData.First().Version;

                dtResult.Rows.Add(dr);

                return dtResult.Rows[0];
            }
        }

        /// <summary>
        /// 获得BOM表的信息
        /// </summary>
        /// <param name="code">图号型号</param>
        /// <param name="name">物品名称</param>
        /// <returns>返回Table</returns>
        public DataRow GetBomInfo(string code, string name)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.BASE_BomPartsLibrary
                          join b in ctx.View_F_GoodsPlanCost
                          on a.GoodsID equals b.序号
                          where b.图号型号 == code && b.物品名称 == name
                          select new
                          {
                              PartCode = b.图号型号,
                              PartName = b.物品名称,
                              Version = a.Version
                          };

            if (varData == null || varData.Count() == 0)
            {
                return null;
            }
            else
            {
                DataTable dtResult = new DataTable();

                dtResult.Columns.Add("PartCode");
                dtResult.Columns.Add("PartName");
                dtResult.Columns.Add("Version");

                DataRow dr = dtResult.NewRow();

                dr["PartCode"] = varData.First().PartCode;
                dr["PartName"] = varData.First().PartName;
                dr["Version"] = varData.First().Version;

                dtResult.Rows.Add(dr);

                return dtResult.Rows[0];
            }
        }

        /// <summary>
        /// 获得某一个产品下的分装总成图号型号
        /// </summary>
        /// <param name="productType">产品型号</param>
        /// <param name="parentName">产品名称</param>
        /// <returns>返回TABLE</returns>
        public DataTable GetBomProductParentCode(string productType, string parentName)
        {
            string strSql = "select * from View_P_ProductBomImitate where Edition  = '"
                + productType + "' and PartName = '" + parentName + "'";

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        /// <summary>
        /// 获得对应所属总成，图号型号，物品名称，规格的BOM表记录
        /// </summary>
        /// <param name="edition">产品型号</param>
        /// <param name="goodCode">图号型号</param>
        /// <param name="goodName">物品名称</param>
        /// <param name="spec">规格</param>
        /// <returns>返回获得的对应的零件BOM的信息</returns>
        public DataTable GetBomEdtionForGoodsInfo(string edition, string goodCode, string goodName, string spec)
        {
            string strSql = "select * from View_P_ProductBomImitate where  Edition = '" + edition + "' and PartCode = '"
                + goodCode + "' and PartName = '" + goodName + "' and Spec = '" + spec + "'";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

            return dt;
        }

        /// <summary>
        /// 获得其他关联的BOM表信息的零件
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <returns>返回其他关联的BOM表信息的零件信息</returns>
        public DataTable GetJumblyGoods(int goodsID)
        {
            string strSql = "select a.JumblyGoodsID from P_JumblyBomGoods as a " +
                " inner join ( " +
                " select * from P_JumblyBomGoods where JumblyGoodsID = " + goodsID + ")  " +
                " as b on a.BomGoodsCode = b.BomGoodsCode and a.BomGoodsName = b.BomGoodsName " +
                " and a.BomSpec = b.BomSpec " +
                " where a.JumblyGoodsID <> " + goodsID;

            DataTable dtTemp = GlobalObject.DatabaseServer.QueryInfo(strSql);

            return dtTemp;
        }

        /// <summary>
        /// 获得BOM表的基数
        /// </summary>
        /// <param name="edition">父级编码</param>
        /// <param name="code">图号型号</param>
        /// <param name="name">物品名称</param>
        /// <param name="spec">规格</param>
        /// <returns>返回基数</returns>
        public int GetBomCounts(string edition, string code, string name, string spec)
        {
            string strSql = "select * from  View_P_ProductBomImitate where PartCode = '"
                + code + "' and PartName = '" + name + "' and Spec = '" + spec
                + "' and (Edition = '" + edition + "' or ParentCode = '" + edition + "')";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

            if (dt.Rows.Count == 0)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(dt.Rows[0]["Counts"].ToString());
            }
        }

        /// <summary>
        /// 获取指定产品下的父总成编码及名称
        /// </summary>
        /// <param name="productCode">产品编码</param>
        /// <param name="assemblyCodes">获取到的父总成编码</param>
        /// <param name="assemblyNames">获取到的父总成名称</param>
        /// <returns>成功获取到返回true</returns>
        public bool GetAssemblyInfo(string productCode, out string[] assemblyCodes, out string[] assemblyNames)
        {
            assemblyCodes = null;
            assemblyNames = null;

            string strSql = "select distinct b.图号型号 as GoodsCode,b.物品名称 as GoodsName from dbo.fun_get_BomTree('" + productCode + "') as a " +
                        "inner join View_F_GoodsPlanCost as b on a.ParentID = b.序号";

            DataTable dtTemp = GlobalObject.DatabaseServer.QueryInfo(strSql);

            if (dtTemp.Rows.Count == 0)
            {
                return false;
            }
            else
            {
                assemblyCodes = new string[dtTemp.Rows.Count];
                assemblyNames = new string[dtTemp.Rows.Count];

                for (int i = 0; i < dtTemp.Rows.Count; i++)
                {
                    assemblyCodes[i] = dtTemp.Rows[i]["GoodsCode"].ToString();
                    assemblyNames[i] = dtTemp.Rows[i]["GoodsName"].ToString();
                }

                return true;
            }
        }

        /// <summary>
        /// 获取某一版本的Bom信息
        /// </summary>
        /// <param name="edition">版本号</param>
        /// <returns>获取到指定版本的BOM表信息</returns>
        public IQueryable<View_P_ProductBom> GetBomData(string edition)
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            return (from r in dataContxt.View_P_ProductBom
                    where r.版本 == edition
                    orderby r.父总成编码, r.零部件编码
                    select r);
        }

        /// <summary>
        /// 获取某一版本的Bom信息
        /// </summary>
        /// <param name="edition">版本号</param>
        /// <param name="dataTable">Bom 数据表</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回true</returns>
        public bool GetBom(string edition, out DataTable dataTable, out string error)
        {
            dataTable = null;
            error = null;

            Hashtable paramTable = new Hashtable();
            DataSet ds = new DataSet();

            paramTable.Add("@Edition", edition);

            Dictionary<OperateCMD, object> dicOperateCMD = m_dbOperate.RunProc_CMD("SelAllP_ProductBom", ds, paramTable);

            if (!Convert.ToBoolean(dicOperateCMD[OperateCMD.Return_OperateResult]))
            {
                error = Convert.ToString(dicOperateCMD[OperateCMD.Return_Errmsg]);

                if (error != "没有找到任何数据")
                {
                    return false;
                }
            }

            dataTable = ds.Tables[0];
            return true;
        }

        /// <summary>
        /// 获取某版本中属于多个父总成的零件
        /// </summary>
        /// <param name="edition">版本号</param>
        /// <returns>获取到的零件信息列表</returns>
        public List<View_P_ProductBomMultiParentPart> GetMultiParentPart(string edition)
        {
            DepotManagementDataContext context = CommentParameter.DepotDataContext;

            return (from r in context.View_P_ProductBomMultiParentPart
                    where r.产品版本 == edition
                    select r).ToList();
        }

        /// <summary>
        /// 获取某一版本的Bom
        /// </summary>
        /// <param name="edition">版本号</param>
        /// <param name="dic">Bom字典</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功获取某一版本的Bom</returns>
        public bool GetBom(string edition, out Dictionary<string, List<Bom>> dic, out string error)
        {
            dic = null;
            error = null;

            Hashtable paramTable = new Hashtable();
            DataSet ds = new DataSet();

            paramTable.Add("@Edition", edition);

            Dictionary<OperateCMD, object> dicOperateCMD = m_dbOperate.RunProc_CMD("SelAllP_ProductBom", ds, paramTable);

            if (!Convert.ToBoolean(dicOperateCMD[OperateCMD.Return_OperateResult]))
            {
                error = Convert.ToString(dicOperateCMD[OperateCMD.Return_Errmsg]);
                return false;
            }

            m_dicBom.Clear();

            DataTable bomTable = ds.Tables[0];

            if (bomTable.Rows.Count > 0)
            {
                List<Bom> listBom = new List<Bom>();
                Bom[] bom = new Bom[bomTable.Rows.Count];

                for (int i = 0; i < bomTable.Rows.Count; i++)
                {
                    string spec = "";

                    if (bomTable.Rows[i][4] != null)
                    {
                        spec = bomTable.Rows[i][4].ToString();
                    }

                    bom[i] = new Bom(Convert.ToInt32(bomTable.Rows[i][0]), bomTable.Rows[i][1].ToString(),
                        bomTable.Rows[i][2].ToString(), bomTable.Rows[i][3].ToString(),
                        spec, Convert.ToInt32(bomTable.Rows[i][5]), Convert.ToBoolean(bomTable.Rows[i][6]),
                        bomTable.Rows[i][7].ToString(), bomTable.Rows[i][8].ToString(),
                        (DateTime)bomTable.Rows[i][9], bomTable.Rows[i][11].ToString());
                    listBom.Add(bom[i]);
                }

                if (m_dicBom.ContainsKey(edition))
                {
                    m_dicBom.Remove(edition);
                }

                m_dicBom.Add(edition, listBom);

                dic = new Dictionary<string, List<Bom>>(m_dicBom);

                return true;
            }
            else
            {
                error = "数据库中该版本的Bom为空";
                return false;
            }
        }

        #region 2012.3.5 夏石友，增加原因：测试装配BOM中的零件是否属于产品BOM表

        /// <summary>
        /// 获取某一版本的Bom信息
        /// </summary>
        /// <param name="edition">版本号</param>
        /// <returns>返回获取到的Bom信息</returns>
        public IQueryable<View_P_ProductBom> GetBom(string edition)
        {
            return from r in CommentParameter.DepotDataContext.View_P_ProductBom
                   where r.版本 == edition
                   select r;
        }

        #endregion

        /// <summary>
        /// 克隆Bom表
        /// </summary>
        /// <param name="edition">版本号</param>
        /// <param name="listBom">Bom列表</param>
        /// <returns>返回克隆的Bom表</returns>
        public Dictionary<string, List<Bom>> Clone(string edition, List<Bom> listBom)
        {
            Bom[] arrayBom = new Bom[listBom.Count];

            listBom.CopyTo(arrayBom);

            List<Bom> bomList = new List<Bom>();

            bomList.AddRange(arrayBom);

            Dictionary<string, List<Bom>> dicBom = new Dictionary<string, List<Bom>>();

            dicBom.Add(edition, bomList);

            return new Dictionary<string, List<Bom>>(dicBom);
        }

        /// <summary>
        /// 修改指定BOM节点的总成标志信息
        /// </summary>
        /// <param name="edition">版本</param>
        /// <param name="bom">要修改的BOM节点</param>
        /// <param name="isAssembly">修改成的总成标志</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功的标志</returns> 
        //public bool UpdateBom(string edition, Bom bom, bool isAssembly, out string error)
        //{
        //    error = null;

        //    Hashtable paramTable = new Hashtable();

        //    if (bom.ParentCode != "")
        //    {
        //        paramTable.Add("@ParentCode", bom.ParentCode);
        //    }
        //    else
        //    {
        //        paramTable.Add("@ParentCode", DBNull.Value);
        //    }

        //    paramTable.Add("@PartCode", bom.PartCode);
        //    paramTable.Add("@AssemblyFlag", Convert.ToInt32(isAssembly));
        //    paramTable.Add("@Edition", edition);
        //    paramTable.Add("@UserCode", bom.UserCode);
        //    paramTable.Add("@FillDate", bom.FillData);
        //    paramTable.Add("@Version", bom.Version);

        //    Dictionary<OperateCMD, object> dicOperateCMD = m_dbOperate.Transaction_CMD("Update_ProductBomAssemblyFlag", paramTable);

        //    if (!Convert.ToBoolean(dicOperateCMD[OperateCMD.Return_OperateResult]))
        //    {
        //        error = Convert.ToString(dicOperateCMD[OperateCMD.Return_Errmsg]);
        //        return false;
        //    }

        //    return true;
        //}

        /// <summary>
        /// 删除BOM信息
        /// </summary>
        /// <param name="edition">版本</param>
        /// <param name="lstBom">要删除的BOM信息列表</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功的标志</returns> 
        public bool UpdateBom(string edition, List<Bom> lstBom, out string error)
        {
            error = null;

            string[] cmds = new string[lstBom.Count];
            Hashtable[] inParams = new Hashtable[lstBom.Count];
            Hashtable[] outParams = new Hashtable[lstBom.Count];
            int i = 0;

            foreach (var bom in lstBom)
            {
                Hashtable paramTable = new Hashtable();

                if (bom.ParentCode != "")
                {
                    paramTable.Add("@ParentCode", bom.ParentCode);
                }
                else
                {
                    paramTable.Add("@ParentCode", DBNull.Value);
                }

                paramTable.Add("@PartCode", bom.PartCode);
                paramTable.Add("@AssemblyFlag", Convert.ToInt32(bom.AssemblyFlag));
                paramTable.Add("@Edition", edition);
                paramTable.Add("@Spec", bom.Spec);
                paramTable.Add("@Version", bom.Version);

                if (bom.StatusFlag == Bom.Status.Remove)
                {
                    cmds[i] = "DelP_ProductBom";
                }
                else if (bom.StatusFlag == Bom.Status.Add || bom.StatusFlag == Bom.Status.Update)
                {
                    if (bom.StatusFlag == Bom.Status.Update)
                    {
                        paramTable.Add("@Id", bom.Id);
                        cmds[i] = "UpdateP_ProductBom";
                    }
                    else
                    {
                        cmds[i] = "AddP_ProductBom";
                    }

                    paramTable.Add("@PartName", bom.PartName);
                    paramTable.Add("@Counts", Convert.ToInt32(bom.Counts));

                    if (bom.Remark != "")
                    {
                        if (bom.AssemblyFlag)
                        {
                            paramTable.Add("@Remark", bom.Remark + "该记录为总成记录");
                        }
                        else
                        {
                            paramTable.Add("@Remark", bom.Remark);
                        }
                    }
                    else
                    {
                        paramTable.Add("@Remark", DBNull.Value);
                    }

                    paramTable.Add("@UserCode", bom.UserCode);
                    paramTable.Add("@FillDate", bom.FillData);
                }

                inParams[i++] = paramTable;
            }

            Dictionary<OperateCMD, object> dicOperateCMD = m_dbOperate.Transaction_CMD(cmds, inParams, ref outParams);

            if (!Convert.ToBoolean(dicOperateCMD[OperateCMD.Return_OperateResult]))
            {
                error = Convert.ToString(dicOperateCMD[OperateCMD.Return_Errmsg]);
                return false;
            }

            return true;
        }

        /// <summary>
        /// 获得BOM关联零件表
        /// </summary>
        /// <returns>返回View_P_JumblyGoods视图记录并且按BOM物品名称,
        /// BOM图号型号,图号型号,规格 列进行排序</returns>
        public DataTable GetJumblyTable()
        {
            string strSql = "select * from View_P_JumblyGoods order by BOM物品名称,BOM图号型号,图号型号,规格";
            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

            return dt;
        }

        /// <summary>
        /// 添加BOM关联零件表
        /// </summary>
        /// <param name="jumbly">一条P_JumblyBomGoods的LINQ记录集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>添加成功返回True，添加失败返回False</returns>
        public bool AddJumbly(P_JumblyBomGoods jumbly, out string error)
        {
            try
            {
                error = null;

                DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

                ctx.P_JumblyBomGoods.InsertOnSubmit(jumbly);
                ctx.SubmitChanges();
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
                throw;
            }
            return true;
        }

        /// <summary>
        /// 更新BOM关联零件表
        /// </summary>
        /// <param name="jumbly">一条LINQ的记录集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>更新成功返回True，更新失败返回False</returns>
        public bool UpdateJumbly(P_JumblyBomGoods jumbly, out string error)
        {
            try
            {
                error = null;

                DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

                var varData = from a in ctx.P_JumblyBomGoods
                              where a.ID == jumbly.ID
                              select a;

                if (varData.Count() != 1)
                {
                    error = "数据为空或者不唯一";
                    return false;
                }
                else
                {
                    P_JumblyBomGoods lnqGoods = varData.Single();

                    lnqGoods.IsJumbly = jumbly.IsJumbly;
                    lnqGoods.BomGoodsCode = jumbly.BomGoodsCode;
                    lnqGoods.BomGoodsName = jumbly.BomGoodsName;
                    lnqGoods.BomSpec = jumbly.BomSpec;
                    lnqGoods.IsStock = jumbly.IsStock;
                    lnqGoods.JumblyGoodsID = jumbly.JumblyGoodsID;
                    lnqGoods.Quato = jumbly.Quato;
                    lnqGoods.IsMath = jumbly.IsMath;
                    lnqGoods.Remark = jumbly.Remark;

                    ctx.SubmitChanges();
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
        /// 删除BOM关联零件表
        /// </summary>
        /// <param name="jumbly">一条LINQ的记录集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>删除成功返回True，删除失败返回False</returns>
        public bool DeleteJumbly(P_JumblyBomGoods jumbly, out string error)
        {
            try
            {
                error = null;

                DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

                var varJumbly = from a in ctx.P_JumblyBomGoods
                                where a.ID == jumbly.ID
                                select a;

                if (varJumbly.Count() != 1)
                {
                    error = "此数据不存在或者不唯一";
                    return false;
                }
                else
                {
                    P_JumblyBomGoods lnqGoods = varJumbly.Single();

                    ctx.P_JumblyBomGoods.DeleteOnSubmit(lnqGoods);

                    ctx.SubmitChanges();
                }

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }
    }

    /// <summary>
    /// Bom
    /// </summary>
    public class Bom
    {
        /// <summary>
        /// 载入、添加、删除、更新状态枚举
        /// </summary>
        public enum Status
        {
            /// <summary>
            /// Load
            /// </summary>
            Load,
            /// <summary>
            /// Add
            /// </summary>
            Add,
            /// <summary>
            /// Remove
            /// </summary>
            Remove,
            /// <summary>
            /// Update
            /// </summary>
            Update
        };

        #region variants

        /// <summary>
        /// 状态
        /// </summary>
        private Status m_statusFlag;

        /// <summary>
        /// 序号
        /// </summary>
        int m_id;

        /// <summary>
        /// 父总成编码
        /// </summary>
        string m_parentCode;

        /// <summary>
        /// 零部件编码
        /// </summary>
        string m_partCode;

        /// <summary>
        /// 零部件名称
        /// </summary>
        string m_partName;

        /// <summary>
        /// 规格
        /// </summary>
        string m_spec;

        /// <summary>
        /// 基数
        /// </summary>
        int m_count;

        /// <summary>
        /// 是否为总成标志
        /// </summary>
        bool m_assemblyFlag;

        /// <summary>
        /// 备注
        /// </summary>
        string m_remark;

        /// <summary>
        /// 创建人
        /// </summary>
        string m_userCode;

        /// <summary>
        /// 创建日期
        /// </summary>
        DateTime m_fillData;

        /// <summary>
        /// 版次号
        /// </summary>
        string m_Version;

        #endregion

        #region properties

        /// <summary>
        /// 获取及设置状态标志
        /// </summary>
        public Status StatusFlag
        {
            get { return m_statusFlag; }
            set { m_statusFlag = value; }
        }

        /// <summary>
        /// 获取或设置序号
        /// </summary>
        public int Id
        {
            get { return m_id; }
            set { m_id = value; }
        }

        /// <summary>
        /// 父总成编码
        /// </summary>
        public string ParentCode
        {
            get { return m_parentCode; }
            set { m_parentCode = value; }
        }

        /// <summary>
        /// 零部件编码
        /// </summary>
        public string PartCode
        {
            get { return m_partCode; }
            set { m_partCode = value; }
        }

        /// <summary>
        /// 零部件名称
        /// </summary>
        public string PartName
        {
            get { return m_partName; }
            set { m_partName = value; }
        }

        /// <summary>
        /// 规格
        /// </summary>
        public string Spec
        {
            get { return m_spec; }
            set { m_spec = value; }
        }

        /// <summary>
        /// 基数
        /// </summary>
        public int Counts
        {
            get { return m_count; }
            set { m_count = value; }
        }

        /// <summary>
        /// 是否为总成标志
        /// </summary>
        public bool AssemblyFlag
        {
            get { return m_assemblyFlag; }
            set { m_assemblyFlag = value; }
        }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return m_remark; }
            set { m_remark = value; }
        }

        /// <summary>
        /// 创建人
        /// </summary>
        public string UserCode
        {
            get { return m_userCode; }
            set { m_userCode = value; }
        }

        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime FillData
        {
            get { return m_fillData; }
            set { m_fillData = value; }
        }

        /// <summary>
        /// 创建版次号
        /// </summary>
        public string Version
        {
            get { return m_Version; }
            set { m_Version = value; }
        }
        #endregion

        /// <summary>
        /// Bom构造函数
        /// </summary>
        /// <param name="id">ID序号</param>
        /// <param name="parentCode">父总成编码</param>
        /// <param name="partsCode">零部件编码</param>
        /// <param name="partName">零部件名称</param>
        /// <param name="spec">规格</param>
        /// <param name="counts">数量</param>
        /// <param name="assemblyFlag">本身是否为总成标志</param>
        /// <param name="remark">备注</param>
        /// <param name="userCode">创建人员</param>
        /// <param name="fillDate">创建日期</param>
        /// <param name="version">版次号</param>
        public Bom(int id, string parentCode, string partsCode,
            string partName, string spec, int counts, bool assemblyFlag,
            string remark, string userCode, DateTime fillDate, string version)
        {
            Id = id;
            ParentCode = parentCode;
            PartCode = partsCode;
            PartName = partName;
            Spec = spec;
            Counts = counts;
            AssemblyFlag = assemblyFlag;
            Remark = remark;
            UserCode = userCode;
            FillData = fillDate;
            Version = version;

            m_statusFlag = Status.Load;
        }

        /// <summary>
        /// 克隆
        /// </summary>
        /// <returns>返回BOM的LINQ数据集</returns>
        public Bom Clone()
        {
            Bom bom = new Bom(Id, ParentCode, PartCode, PartName, Spec, Counts,
                AssemblyFlag, Remark, UserCode, FillData, Version);
            return bom;
        }
    }
}

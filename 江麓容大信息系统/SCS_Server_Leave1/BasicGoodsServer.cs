/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  PlanCostBillServer.cs
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
using System.Text;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using System.Collections;
using DBOperate;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using GlobalObject;

namespace ServerModule
{
    /// <summary>
    /// 物品分组
    /// </summary>
    /// <remarks>
    /// 防止由其他单据生成领料单时出现同一物品多条领料记录的现象，而在领料单中又无法显示出来
    /// 用于使用Group by的情况
    /// </remarks>
    public class GoodsGroup
    {
        /// <summary>
        /// 物品ID
        /// </summary>
        public int 物品ID { get; set; }

        /// <summary>
        /// 图号型号
        /// </summary>
        public string 图号型号 { get; set; }

        /// <summary>
        /// 物品名称
        /// </summary>
        public string 物品名称 { get; set; }

        /// <summary>
        /// 规格
        /// </summary>
        public string 规格 { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal 数量 { get; set; }
    }

    /// <summary>
    /// 基础物品信息表（包含 物品、零件计划价格等信息）服务类
    /// </summary>
    public class BasicGoodsServer : BasicServer, IBasicGoodsServer
    {
        /// <summary>
        /// 获得毛坯所属成品信息集合
        /// </summary>
        /// <param name="atrributeRecordID">属相记录ID</param>
        /// <returns>返回List</returns>
        public List<View_F_GoodsBlankToProduct> GetBlankToProductListInfo(int atrributeRecordID)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.View_F_GoodsBlankToProduct
                          where a.属性记录ID == atrributeRecordID
                          select a;

            return varData.ToList();
        }

        /// <summary>
        /// 获得产品编码信息列表
        /// </summary>
        /// <param name="atrributeRecordID">属相记录ID</param>
        /// <returns>返回List</returns>
        public List<F_ProductWaterCode> GetWaterCodeListInfo(int atrributeRecordID)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.F_ProductWaterCode
                          where a.AttributeRecordID == atrributeRecordID
                          select a;

            return varData.ToList();
        }

        /// <summary>
        /// 获得替换件信息集合
        /// </summary>
        /// <param name="atrributeRecordID">属相记录ID</param>
        /// <returns>返回List</returns>
        public List<View_F_GoodsReplaceInfo> GetReplaceListInfo(int atrributeRecordID)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.View_F_GoodsReplaceInfo
                          where a.属性记录ID == atrributeRecordID
                          select a;

            return varData.ToList();
        }

        /// <summary>
        /// 获得物品ID(没有便插入一个新的物品记录)
        /// </summary>
        /// <param name="code">图号型号</param>
        /// <param name="name">物品名称</param>
        /// <param name="spec">规格</param>
        /// <param name="goodsType">材料类别</param>
        /// <param name="unitID">单位ID</param>
        /// <param name="remark">备注</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回获得物品ID，返回0则表示获取失败</returns>
        public int GetGoodsID(string code, string name, string spec, string goodsType, int unitID, string remark, out string error)
        {
            try
            {
                error = null;

                string strSql = "select * from F_GoodsPlanCost where GoodsCode = '" +
                    code + "' and GoodsName = '" + name + "' and Spec = '" + spec + "'";
                DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

                if (dt.Rows.Count == 0)
                {
                    F_GoodsPlanCost goodsPlanCost = new F_GoodsPlanCost();

                    goodsPlanCost.GoodsType = goodsType;
                    goodsPlanCost.GoodsCode = code;
                    goodsPlanCost.GoodsName = name;
                    goodsPlanCost.GoodsUnitPrice = 0;
                    goodsPlanCost.Spec = spec;
                    goodsPlanCost.UserCode = BasicInfo.LoginID;
                    goodsPlanCost.UnitID = unitID;
                    goodsPlanCost.Remark = remark;
                    goodsPlanCost.PY = UniversalFunction.GetPYWBCode(goodsPlanCost.GoodsName, "PY");
                    goodsPlanCost.WB = UniversalFunction.GetPYWBCode(goodsPlanCost.GoodsName, "WB");

                    if (!AddGoods(goodsPlanCost, null, null , null , null, out error))
                    {
                        return 0;
                    }
                    else
                    {
                        strSql = "select Max(ID) from F_GoodsPlanCost";
                        dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

                        return Convert.ToInt32(dt.Rows[0][0].ToString());
                    }
                }
                else
                {
                    return Convert.ToInt32(dt.Rows[0]["ID"].ToString());
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return 0;

            }
        }

        /// <summary>
        /// 获得物品基础表的图号型号
        /// </summary>
        /// <returns>返回获得的图号型号列表</returns>
        public DataTable GetDistinctGoodsCode()
        {
            string strSql = "select distinct GoodsCode from F_GoodsPlanCost ";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

            return dt;
        }

        /// <summary>
        /// 获得物品基础表的规格
        /// </summary>
        /// <returns>返回获得的规格列表</returns>
        public DataTable GetDistinctSpec()
        {
            string strSql = "select distinct Spec from F_GoodsPlanCost ";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

            return dt;
        }

        /// <summary>
        /// 获得物品基础表的物品名称
        /// </summary>
        /// <returns>返回获得的物品名称列表</returns>
        public DataTable GetDistinctGoodsName()
        {
            string strSql = "select distinct GoodsName from F_GoodsPlanCost ";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

            return dt;
        }

        /// <summary>
        /// 获得物品信息（筛选物品属性）
        /// </summary>
        /// <param name="strWhere">筛选信息</param>
        /// <returns>返回Table</returns>
        public DataTable GetGoodsInfoSiftAttribute(string strWhere)
        {
            string strSql = " select distinct a.* from View_F_GoodsPlanCost as a left join F_GoodsAttributeRecord as b on a.序号 = b.GoodsID "+
                            " where 1=1 "+ strWhere + " order by a.图号型号, a.物品名称, a.规格";

            DataTable tempTable = GlobalObject.DatabaseServer.QueryInfo(strSql);

            return tempTable;
        }

        /// <summary>
        /// 获取物品的计划单价
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <param name="planUnitPrice">计划单价</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功获取物品的计划单价</returns>
        public bool GetPlanUnitPrice(int goodsID, out decimal planUnitPrice, out string error)
        {
            error = null;
            planUnitPrice = 0;

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from c in dataContxt.View_F_GoodsPlanCost
                             where c.序号 == goodsID
                             select c.单价;

                if (result.Count() == 0)
                {
                    error = string.Format("基础物品信息表中找不到物品ID为 [{1}] 的计划价格", goodsID);
                    return false;
                }
                else
                {
                    planUnitPrice = result.Single();
                    return true;
                }
            }
            catch (Exception err)
            {
                error = err.ToString();
                return false;
            }
        }

        /// <summary>
        /// 获取物品的计划单价
        /// </summary>
        /// <param name="goodsCode">图号型号</param>
        /// <param name="spec">规格</param>
        /// <param name="planUnitPrice">计划单价</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功获取物品的计划单价</returns>
        public bool GetPlanUnitPrice(string goodsCode, string spec, out decimal planUnitPrice, out string error)
        {
            error = null;
            planUnitPrice = 0;

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from c in dataContxt.View_F_GoodsPlanCost
                             where c.图号型号 == goodsCode && c.规格 == spec select c.单价;

                if (result.Count() == 0)
                {
                    error = string.Format("基础物品信息表中找不到图号型号[{0}],规格[{1}]的计划价格", goodsCode, spec);
                    return false;
                }
                else if (result.Count() > 1)
                {
                    error = string.Format("基础物品信息表中存在【图号型号[{0}],规格[{1}]】的物品[{2}]条计划价格,无法确定数据的正确性,请及时与管理员联系",
                        goodsCode, spec, result.Count());
                    return false;
                }
                else
                {
                    planUnitPrice = result.Single();
                    return true;
                }
            }
            catch (Exception err)
            {
                error = err.ToString();
                return false;
            }
        }

        /// <summary>
        /// 获取物品的计划单价
        /// </summary>
        /// <param name="goodsCode">图号型号</param>
        /// <param name="goodsName">物品名称</param>
        /// <param name="spec">规格</param>
        /// <param name="planUnitPrice">计划单价</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功获取物品的计划单价</returns>
        public bool GetPlanUnitPrice(string goodsCode, string goodsName, string spec, out decimal planUnitPrice, out string error)
        {
            error = null;
            planUnitPrice = 0;

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from c in dataContxt.View_F_GoodsPlanCost
                                 where c.图号型号 == goodsCode  && c.物品名称 == goodsName && c.规格 == spec 
                                 select c.单价;

                if (result.Count() == 0)
                {
                    error = string.Format("基础物品信息表中找不到图号型号[{0}],物品名称[{1}],规格[{2}]的计划价格", 
                        goodsCode, goodsName, spec);
                    return false;
                }
                else if (result.Count() > 1)
                {
                    error = string.Format("基础物品信息表中存在【图号型号[{0}],物品名称[{1}],规格[{2}]】的物品的[{3}]条计划价格,无法确定数据的正确性,请及时与管理员联系",
                        goodsCode, goodsName, spec, result.Count());
                    return false;
                }
                else
                {
                    planUnitPrice = result.Single();
                    return true;
                }
            }
            catch (Exception err)
            {
                error = err.ToString();
                return false;
            }
        }

        /// <summary>
        /// 判断某物品信息是否存在
        /// </summary>
        /// <param name="goodsCode">图号型号</param>
        /// <param name="goodsName">物品名称</param>
        /// <param name="spec">规格</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>存在返回true</returns>
        public bool IsExist(string goodsCode, string goodsName, string spec, out string error)
        {
            error = null;
            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from c in dataContxt.View_F_GoodsPlanCost
                             where c.图号型号 == goodsCode && 
                             c.物品名称 == goodsName && 
                             c.规格 == spec
                             select c.单价;

                if (result.Count() == 0)
                {
                    return true;
                }
                else 
                {
                    return false;
                }
            }
            catch (Exception err)
            {
                error = err.ToString();
                return false;
            }
        }

        /// <summary>
        /// 获取物品信息
        /// </summary>
        /// <param name="goodsCode">图号型号</param>
        /// <param name="goodsName">物品名称</param>
        /// <param name="spec">规格</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回获取到的物品信息, 失败返回null</returns>
        public View_F_GoodsPlanCost GetGoodsInfo(string goodsCode, string goodsName, string spec, out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from c in dataContxt.View_F_GoodsPlanCost
                             where c.图号型号 == goodsCode && c.物品名称 == goodsName && c.规格 == spec
                             select c;

                if (result.Count() == 0)
                {
                    error = string.Format("基础物品信息表中找不到图号型号[{0}],物品名称[{1}],规格[{2}]的记录", goodsCode, goodsName, spec);
                    return null;
                }
                else if (result.Count() > 1)
                {
                    error = string.Format("基础物品信息表中 【图号型号[{0}],物品名称[{1}],规格[{2}]】的物品存在[{3}]条记录,无法确定数据的正确性,请及时与管理员联系",
                        goodsCode, goodsName, spec, result.Count());
                    return null;
                }
                else
                {
                    return  result.Single();
                }
            }
            catch (Exception err)
            {
                error = err.ToString();
                return null;
            }
        }

        /// <summary>
        /// 获取物品信息(应用于报检入库单，防止图号一样而名称不同的现象)
        /// </summary>
        /// <param name="goodsCode">图号型号</param>
        /// <param name="spec">物品规格</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>找到返回物品信息，否则返回null</returns>
        public View_F_GoodsPlanCost GetGoodsInfo(string goodsCode, string spec, out string error)
        {
            try
            {
                error = null;

                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from r in dataContxt.View_F_GoodsPlanCost
                             where r.图号型号 == goodsCode && r.规格 == spec
                             select r;

                if (result.Count() > 0)
                {
                    if (result.Count() > 1)
                    {
                        var varData = from a in result
                                      where a.禁用 == false
                                      select a;

                        if (varData.Count() == 0)
                        {
                            return result.First();
                        }
                        else
                        {
                            return varData.First();
                        }
                    }
                    else
                    {
                        return result.Single();
                    }
                }
                else
                {
                    error = string.Format("找不到图号 [{0}], 规格 [{1}] 的基础物品信息，请与管理员联系", goodsCode, spec);
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message); ;
            }
        }

        /// <summary>
        /// 获取所有物品信息
        /// </summary>
        /// <param name="returnInfo">返回查询到的信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回操作是否成功</returns>
        public bool GetAllGoodsInfo(out IQueryable<View_BASE_GoodsPlanCost> returnInfo, out string error)
        {
            returnInfo = null;
            error = null;
            
            try
            {
                DepotManagementDataContext depotMangaeDataContext = CommentParameter.DepotDataContext;
                Table<View_BASE_GoodsPlanCost> goodsPriceTable = depotMangaeDataContext.GetTable<View_BASE_GoodsPlanCost>();

                returnInfo = from c in goodsPriceTable
                             orderby c.序号
                             select c;
                return true;
            }
            catch (Exception err)
            {
                error = err.ToString();
                return false;
            }
        }

        ///// <summary>
        ///// 获取包含在仓库类别列表中的所有信息
        ///// </summary>
        ///// <param name="userCode">用户编码</param>
        ///// <returns>返回获取到的信息</returns>
        //public IQueryable<View_F_GoodsPlanCost> GetGoodsInfo(string userCode)
        //{
        //    return GetGoodsInfo(userCode, null);
        //}

        /// <summary>
        /// 获取指定用户查询权限内的包含在仓库类别列表中的所有信息
        /// </summary>
        /// <param name="blUsing">是否仅显示在用</param>
        /// <returns>返回获取到的信息</returns>
        public IQueryable<View_F_GoodsPlanCost> GetGoodsInfo(bool blUsing)
        {
            DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;
            IQueryable<View_F_GoodsPlanCost> result = null;

            if (blUsing)
            {
                result = from c in dataContext.View_F_GoodsPlanCost
                         where c.禁用 == false
                         orderby c.图号型号, c.物品名称, c.规格
                         select c;
            }
            else
            {
                result = from c in dataContext.View_F_GoodsPlanCost
                         orderby c.图号型号, c.物品名称, c.规格
                         select c;
            }

            if (result.Count() == 0)
            {
                return null;
            }
            else
            {
                return result;
            }
        }

        ///// <summary>
        ///// 获取包含在仓库类别列表中的所有信息
        ///// </summary>
        ///// <param name="lstDepotCode">仓库类别列表</param>
        ///// <returns>返回获取到的信息</returns>
        //public IQueryable<View_F_GoodsPlanCost> GetGoodsInfo(List<string> lstDepotCode)
        //{
        //    return GetGoodsInfo(null, lstDepotCode);
        //}

        /// <summary>
        /// 获取指定ID的物品信息
        /// </summary>
        /// <param name="dataContext">数据上下文</param>
        /// <param name="id">物品ID</param>
        /// <returns>成功返回获取到的信息,失败返回null</returns>
        public F_GoodsPlanCost GetGoodsInfo(DepotManagementDataContext dataContext, int id)
        {
            var result = from c in dataContext.F_GoodsPlanCost
                         where c.ID == id
                         select c;

            if (result.Count() == 0)
                return null;
            else
                return result.Single();
        }

        /// <summary>
        /// 获取指定ID的物品信息
        /// </summary>
        /// <param name="id">物品ID</param>
        /// <returns>成功返回获取到的信息,失败返回null</returns>
        public F_GoodsPlanCost GetGoodsInfo(int id)
        {
            DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

            var result = from c in dataContext.F_GoodsPlanCost
                         where c.ID == id
                         select c;

            if (result.Count() == 0)
                return null;
            else
                return result.Single();
        }

        /// <summary>
        /// 获取指定ID的物品信息
        /// </summary>
        /// <param name="id">物品ID</param>
        /// <returns>成功返回获取到的信息,失败返回null</returns>
        public View_F_GoodsPlanCost GetGoodsInfoView(int id)
        {
            DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

            var result = from c in dataContext.View_F_GoodsPlanCost
                    where c.序号 == id
                    select c;

            if (result.Count() == 0)
                return null;
            else
                return result.Single();
        }

        /// <summary>
        /// 获取物品类别(应用于报检入库单，防止图号一样而名称不同的现象)
        /// </summary>
        /// <param name="goodsCode">图号型号</param>
        /// <param name="spec">物品规格</param>
        /// <returns>找到返回类别，否则返回null</returns>
        public string GetGoodsType(string goodsCode, string spec)
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            var result = from r in dataContxt.F_GoodsPlanCost
                         where r.GoodsCode == goodsCode && r.Spec == spec
                         select r;

            if (result.Count() > 0)
            {
                return result.Single().GoodsType;
            }
            else
            {
                return null;
            }

            //throw new Exception(string.Format("找不到图号 [{0}], 名称 [{1}], 规格 [{2}] 的物品的类别，请与管理员联系", goodsCode, goodsName, spec));
        }

        /// <summary>
        /// 获取物品类别
        /// </summary>
        /// <param name="goodsCode">图号型号</param>
        /// <param name="goodsName">物品名称</param>
        /// <param name="spec">物品规格</param>
        /// <returns>找到返回类别，否则返回null</returns>
        public string GetGoodsType(string goodsCode, string goodsName, string spec)
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            var result = from r in dataContxt.F_GoodsPlanCost
                         where r.GoodsCode == goodsCode && r.GoodsName == goodsName && r.Spec == spec
                         select r;

            if (result.Count() > 0)
            {
                return result.Single().GoodsType;
            }
            else
            {
                return null;
            }

            //throw new Exception(string.Format("找不到图号 [{0}], 名称 [{1}], 规格 [{2}] 的物品的类别，请与管理员联系", goodsCode, goodsName, spec));
        }

        #region 编辑基础物品信息

        /// <summary>
        /// 编辑基础物品信息
        /// </summary>
        /// <param name="goodsCost">基础物品主要信息</param>
        /// <param name="listRecord">属性记录列表</param>
        /// <param name="listReplace">替换件信息列表</param>
        /// <param name="listBlank">毛坯对应成品列表</param>
        /// <param name="listWaterCode">产品编码列表</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True,失败返回False</returns>
        public bool EditGoodsInfo(F_GoodsPlanCost goodsCost, List<F_GoodsAttributeRecord> listRecord,
            List<View_F_GoodsReplaceInfo> listReplace, List<View_F_GoodsBlankToProduct> listBlank, 
            List<F_ProductWaterCode> listWaterCode, out string error)
        {
            error = null;

            try
            {
                if (goodsCost.ID == 0)
                {
                    if (!AddGoods(goodsCost, listRecord, listReplace, listBlank, listWaterCode, out error))
                    {
                        throw new Exception(error);
                    }
                }
                else
                {
                    if (!UpdateGoods(goodsCost, listRecord, listReplace, listBlank, listWaterCode, out error))
                    {
                        throw new Exception(error);
                    }
                }

                RecordOperationLog(goodsCost);
                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        void RecordOperationLog(F_GoodsPlanCost goodsCost)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            if (goodsCost.ID == 0)
            {
                var varData = from a in ctx.F_GoodsPlanCost
                              where a.GoodsCode == goodsCost.GoodsCode
                              && a.GoodsName == goodsCost.GoodsName
                              && a.Spec == goodsCost.Spec
                              select a;

                if (varData.Count() != 1)
                {
                    return;
                }

                goodsCost.ID = varData.Single().ID;
            }

            F_GoodsOperationLog log = new F_GoodsOperationLog();

            log.GoodsID = goodsCost.ID;
            log.OperationDate = ServerTime.Time;
            log.OperationPersonnel = BasicInfo.LoginID;

            ctx.F_GoodsOperationLog.InsertOnSubmit(log);
            ctx.SubmitChanges();
        }

        /// <summary>
        /// 添加基础物品信息
        /// </summary>
        /// <param name="goodsCost">物品价格信息</param>
        /// <param name="listRecord">物品属性记录列表</param>
        /// <param name="listReplace">替换件信息列表</param>
        /// <param name="listBlank">毛坯对应成品列表</param>
        /// <param name="listWaterCode">产品编码列表</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功添加计划价格信息</returns>
        bool AddGoods(F_GoodsPlanCost goodsCost, List<F_GoodsAttributeRecord> listRecord,
            List<View_F_GoodsReplaceInfo> listReplace, List<View_F_GoodsBlankToProduct> listBlank,
            List<F_ProductWaterCode> listWaterCode, out string error)
        {
            error = null;

            try
            {
                AddGoods(goodsCost, listRecord, listReplace, listBlank, listWaterCode);
                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 添加基础物品信息
        /// </summary>
        /// <param name="goodsCost">物品价格信息</param>
        /// <param name="listRecord">物品属性记录列表</param>
        /// <param name="listReplace">替换件信息列表</param>
        /// <param name="listBlank">毛坯对应成品列表</param>
        /// <param name="listWaterCode">产品编码列表</param>
        void AddGoods(F_GoodsPlanCost goodsCost, List<F_GoodsAttributeRecord> listRecord,
            List<View_F_GoodsReplaceInfo> listReplace, List<View_F_GoodsBlankToProduct> listBlank,
            List<F_ProductWaterCode> listWaterCode)
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            dataContxt.Connection.Open();
            dataContxt.Transaction = dataContxt.Connection.BeginTransaction();

            try
            {
                goodsCost.Date = ServerTime.Time.Date;
                goodsCost.UserCode = BasicInfo.LoginID;
                goodsCost.PY = UniversalFunction.GetPYWBCode(goodsCost.GoodsName, "PY");
                goodsCost.WB = UniversalFunction.GetPYWBCode(goodsCost.GoodsName, "WB");

                dataContxt.F_GoodsPlanCost.InsertOnSubmit(goodsCost);
                dataContxt.SubmitChanges();

                var varData = from a in dataContxt.F_GoodsPlanCost
                              where a.GoodsCode == goodsCost.GoodsCode
                              && a.GoodsName == goodsCost.GoodsName
                              && a.Spec == goodsCost.Spec
                              select a;

                if (varData.Count() != 1)
                {
                    throw new Exception("物品添加失败");
                }
                else
                {
                    SaveGoodsAttirbuteList(dataContxt, varData.Single().ID, listRecord);
                    dataContxt.SubmitChanges();

                    SaveGoodsReplaceList(dataContxt, varData.Single().ID, listReplace);
                    dataContxt.SubmitChanges();

                    SaveGoodsBlankList(dataContxt, varData.Single().ID, listBlank);
                    dataContxt.SubmitChanges();

                    SaveGoodsWaterCodeList(dataContxt, varData.Single().ID, listWaterCode);
                    dataContxt.SubmitChanges();
                }

                dataContxt.Transaction.Commit();
            }
            catch (Exception exce)
            {
                dataContxt.Transaction.Rollback();
                throw new Exception(exce.Message);
            }
        }

        /// <summary>
        /// 修改基础物品信息
        /// </summary>
        /// <param name="goodsCost">物品价格信息</param>
        /// <param name="listRecord">物品属性记录列表</param>
        /// <param name="listReplace">替换件信息列表</param>
        /// <param name="listBlank">毛坯对应成品列表</param>
        /// <param name="listWaterCode">产品编码列表</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功添加计划价格信息</returns>
        bool UpdateGoods(F_GoodsPlanCost goodsCost, List<F_GoodsAttributeRecord> listRecord,
            List<View_F_GoodsReplaceInfo> listReplace, List<View_F_GoodsBlankToProduct> listBlank, 
            List<F_ProductWaterCode> listWaterCode, out string error)
        {
            error = null;

            try
            {
                UpdateGoods(goodsCost, listRecord, listReplace, listBlank, listWaterCode);
                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 修改基础物品信息
        /// </summary>
        /// <param name="goodsCost">物品价格信息</param>
        /// <param name="listRecord">物品属性记录列表</param>
        /// <param name="listReplace">替换件信息列表</param>
        /// <param name="listBlank">毛坯对应成品列表</param>
        /// <param name="listWaterCode">产品编码列表</param>
        void UpdateGoods(F_GoodsPlanCost goodsCost, List<F_GoodsAttributeRecord> listRecord,
            List<View_F_GoodsReplaceInfo> listReplace, List<View_F_GoodsBlankToProduct> listBlank,
            List<F_ProductWaterCode> listWaterCode)
        {
            string error = null;
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            dataContxt.Connection.Open();
            dataContxt.Transaction = dataContxt.Connection.BeginTransaction();

            try
            {
                var result = from r in dataContxt.F_GoodsPlanCost 
                             where r.ID == goodsCost.ID 
                             select r;

                if (result.Count() == 0)
                {
                    throw new Exception( "找不到基础物品信息无法进行此操作");
                }

                F_GoodsPlanCost goodsPlanCost = result.Single();

                goodsPlanCost.GoodsType = goodsCost.GoodsType;

                if (goodsCost.GoodsCode != goodsPlanCost.GoodsCode ||
                    goodsCost.GoodsName != goodsPlanCost.GoodsName ||
                    goodsCost.Spec != goodsPlanCost.Spec)
                {
                    if (IsExistInBusiness(goodsCost.ID, out error))
                    {
                        error += "，不允许进行此操作";
                        throw new Exception(error);
                    }
                }

                goodsPlanCost.GoodsCode = goodsCost.GoodsCode;
                goodsPlanCost.GoodsName = goodsCost.GoodsName;
                goodsPlanCost.Spec = goodsCost.Spec;

                goodsPlanCost.GoodsUnitPrice = goodsCost.GoodsUnitPrice;
                goodsPlanCost.UnitID = goodsCost.UnitID;
                goodsPlanCost.Remark = goodsCost.Remark;
                goodsPlanCost.UserCode = BasicInfo.LoginID;
                goodsPlanCost.Date = ServerTime.Time;
                goodsPlanCost.PY = UniversalFunction.GetPYWBCode(goodsPlanCost.GoodsName, "PY");
                goodsPlanCost.WB = UniversalFunction.GetPYWBCode(goodsPlanCost.GoodsName, "WB");

                if (!goodsPlanCost.IsDisable && goodsCost.IsDisable)
                {
                    CheckGoodsInfo(dataContxt, goodsCost);
                }

                goodsPlanCost.IsDisable = goodsCost.IsDisable;
                goodsPlanCost.GoodsType = goodsCost.GoodsType;

                SaveGoodsAttirbuteList(dataContxt, goodsPlanCost.ID, listRecord);
                dataContxt.SubmitChanges();

                SaveGoodsReplaceList(dataContxt, goodsPlanCost.ID, listReplace);
                dataContxt.SubmitChanges();

                SaveGoodsBlankList(dataContxt, goodsPlanCost.ID, listBlank);
                dataContxt.SubmitChanges();

                SaveGoodsWaterCodeList(dataContxt, goodsPlanCost.ID, listWaterCode);
                dataContxt.SubmitChanges();

                dataContxt.Transaction.Commit();
            }
            catch (Exception exce)
            {
                dataContxt.Transaction.Rollback();
                throw new Exception(exce.Message);
            }
        }

        void CheckGoodsInfo(DepotManagementDataContext ctx, F_GoodsPlanCost goodsInfo)
        {
            var varData = from a in ctx.F_GoodsPlanCost
                          where a.GoodsCode == goodsInfo.GoodsCode
                          && a.GoodsName == goodsInfo.GoodsName
                          && a.Spec == goodsInfo.Spec
                          && a.ID != goodsInfo.ID
                          select a;

            if (varData.Count() > 0)
            {
                throw new Exception("存在相同物品，无法录入");
            }
            else
            {
                if (goodsInfo.GoodsCode.Length > 0)
                {
                    varData = from a in ctx.F_GoodsPlanCost
                              where a.GoodsCode == goodsInfo.GoodsCode
                              && a.IsDisable == false
                              select a;

                    if (varData.Count() > 0)
                    {
                        foreach (var item in varData)
                        {
                            if (item.GoodsName == goodsInfo.GoodsName)
                            {
                                return;
                            }
                        }

                        throw new Exception("存在相同【图号型号】的物品必须【物品名称】相同，如需录入请用【规格】区分，否则无法录入");
                    }
                }
            }
        }

        /// <summary>
        /// 删除计划价格信息
        /// </summary>
        /// <param name="id">序号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功删除计划价格信息</returns>
        public bool DeleteGoods(int id, out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                if (IsExistInBusiness(id, out error))
                {
                    error += "，不允许进行此操作";
                    return false;
                }

                var delRow = from c in dataContxt.F_GoodsPlanCost
                             where c.ID == id
                             select c;

                dataContxt.F_GoodsPlanCost.DeleteAllOnSubmit(delRow);
                dataContxt.SubmitChanges();

                return true;
            }
            catch (Exception err)
            {
                error = err.Message;
                return false;
            }
        }

        /// <summary>
        /// 删除计划价格信息
        /// </summary>
        /// <param name="id">序号</param>
        /// <param name="returnInfo">计划价格信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功删除计划价格信息</returns>
        public bool DeleteGoods(int id, out IQueryable<View_BASE_GoodsPlanCost> returnInfo, out string error)
        {
            returnInfo = null;

            if (DeleteGoods(id, out error))
            {
                return GetAllGoodsInfo(out returnInfo, out error);
            }
            else
            {
                return false;
            }
        }

        void JudgeAssmebly(DepotManagementDataContext ctx, int goodsID, List<F_GoodsAttributeRecord> listNew,
            List<F_GoodsAttributeRecord> listOld)
        {
            bool isAssmeblyOld = false;

            foreach (F_GoodsAttributeRecord oldRecord in listOld)
            {
                if ((oldRecord.AttributeID == (int)CE_GoodsAttributeName.TCU && Convert.ToBoolean(oldRecord.AttributeValue))
                    || (oldRecord.AttributeID == (int)CE_GoodsAttributeName.CVT && Convert.ToBoolean(oldRecord.AttributeValue))
                    || (oldRecord.AttributeID == (int)CE_GoodsAttributeName.部件 && Convert.ToBoolean(oldRecord.AttributeValue)))
                {
                    isAssmeblyOld = true;
                    break;
                }
            }

            bool isAssmeblyNew = false;

            foreach (F_GoodsAttributeRecord newRecord in listNew)
            {
                if ((newRecord.AttributeID == (int)CE_GoodsAttributeName.TCU && Convert.ToBoolean(newRecord.AttributeValue))
                    || (newRecord.AttributeID == (int)CE_GoodsAttributeName.CVT && Convert.ToBoolean(newRecord.AttributeValue))
                    || (newRecord.AttributeID == (int)CE_GoodsAttributeName.部件 && Convert.ToBoolean(newRecord.AttributeValue)))
                {
                    isAssmeblyNew = true;
                    break;
                }
            }

            if (isAssmeblyOld && !isAssmeblyNew)
            {
                var varData = from a in ctx.BASE_BomStruct
                              where a.ParentID == goodsID
                              select a;

                if (varData.Count() > 0)
                {
                    throw new Exception(UniversalFunction.GetGoodsMessage(ctx, goodsID) 
                        + "由【总成变更为非总成】，请先剔除【设计BOM】中该物品下的BOM表结构");
                }
            }
        }

        /// <summary>
        /// 编辑某物品的全部属性
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <param name="listRecord">属性列表</param>
        void SaveGoodsAttirbuteList(DepotManagementDataContext ctx, int goodsID, List<F_GoodsAttributeRecord> listRecord)
        {
            try
            {
                var varData = from a in ctx.F_GoodsAttributeRecord
                              where a.GoodsID == goodsID
                              select a;

                JudgeAssmebly(ctx, goodsID, listRecord, varData.ToList());
                DeleteCGBom(ctx, goodsID, listRecord, varData.ToList());
                ctx.F_GoodsAttributeRecord.DeleteAllOnSubmit(varData);

                foreach (F_GoodsAttributeRecord record in listRecord)
                {
                    F_GoodsAttributeRecord tempLnq = new F_GoodsAttributeRecord();

                    tempLnq.AdditionalProperty1 = record.AdditionalProperty1;
                    tempLnq.AdditionalProperty2 = record.AdditionalProperty2;
                    tempLnq.AdditionalProperty3 = record.AdditionalProperty3;
                    tempLnq.AttributeID = record.AttributeID;
                    tempLnq.AttributeValue = record.AttributeValue;
                    tempLnq.GoodsID = goodsID;
                    tempLnq.IsHavingChildTable = record.IsHavingChildTable;

                    ctx.F_GoodsAttributeRecord.InsertOnSubmit(tempLnq);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        void DeleteCGBom(DepotManagementDataContext ctx, int goodsID, List<F_GoodsAttributeRecord> listNew,
            List<F_GoodsAttributeRecord> listOld)
        {
            bool isEOLOld = false;

            foreach (F_GoodsAttributeRecord oldRecord in listOld)
            {
                if ((oldRecord.AttributeID == (int)CE_GoodsAttributeName.停产 && Convert.ToBoolean(oldRecord.AttributeValue)))
                {
                    isEOLOld = true;
                    break;
                }
            }

            bool isEOLNew = false;

            foreach (F_GoodsAttributeRecord newRecord in listNew)
            {
                if ((newRecord.AttributeID == (int)CE_GoodsAttributeName.停产 && Convert.ToBoolean(newRecord.AttributeValue)))
                {
                    isEOLNew = true;
                    break;
                }
            }

            if (!isEOLOld && isEOLNew)
            {
                var varData = from a in ctx.CG_CBOM
                              join b in ctx.F_GoodsPlanCost
                              on a.Edition equals b.GoodsCode
                              where b.ID == goodsID
                              select a;

                foreach (CG_CBOM item in varData)
                {
                    ctx.CG_CBOM.DeleteOnSubmit(item);
                    ctx.SubmitChanges();

                    var varData1 = from a in ctx.CG_CBOM
                                   where a.GoodsID == item.GoodsID
                                   select a;

                    if (varData1.Count() == 0)
                    {
                        var varSafe = from a in ctx.S_SafeStock
                                      where a.GoodsID == item.GoodsID
                                      select a;

                        ctx.S_SafeStock.DeleteAllOnSubmit(varSafe);
                        ctx.SubmitChanges();

                        var varGoodsLeastPackAndStock = from a in ctx.B_GoodsLeastPackAndStock
                                                        where a.GoodsID == item.GoodsID
                                                        select a;

                        ctx.B_GoodsLeastPackAndStock.DeleteAllOnSubmit(varGoodsLeastPackAndStock);
                        ctx.SubmitChanges();

                        var varProcurementPlan = from a in ctx.CG_ProcurementPlan
                                                 where a.GoodsID == item.GoodsID
                                                 select a;

                        ctx.CG_ProcurementPlan.DeleteAllOnSubmit(varProcurementPlan);
                        ctx.SubmitChanges();
                    }
                }
            }
        }

        /// <summary>
        /// 保存毛坯对应成品列表
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="goodsID">物品ID</param>
        /// <param name="listInfo">毛坯对应成品列表</param>
        void SaveGoodsBlankList(DepotManagementDataContext ctx, int goodsID, List<View_F_GoodsBlankToProduct> listInfo)
        {
            try
            {
                var varRecord = from a in ctx.F_GoodsAttributeRecord
                                where a.GoodsID == goodsID && a.AttributeID == (int)CE_GoodsAttributeName.毛坯
                                && a.AttributeValue != null
                                select a;

                if (varRecord.Count() != 1)
                {
                    return;
                }

                var varData = from a in ctx.F_GoodsReplaceInfo
                              where a.AttributeRecordID == varRecord.Single().AttributeRecordID
                              select a;

                ctx.F_GoodsReplaceInfo.DeleteAllOnSubmit(varData);

                foreach (View_F_GoodsBlankToProduct replaceInfo in listInfo)
                {
                    F_GoodsBlankToProduct tempLnq = new F_GoodsBlankToProduct();

                    tempLnq.AttributeRecordID = varRecord.Single().AttributeRecordID;
                    tempLnq.GoodsIDProduct = replaceInfo.物品ID;

                    ctx.F_GoodsBlankToProduct.InsertOnSubmit(tempLnq);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 保存替换件列表
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="goodsID">物品ID</param>
        /// <param name="listInfo">替换件视图列表</param>
        void SaveGoodsReplaceList(DepotManagementDataContext ctx, int goodsID, List<View_F_GoodsReplaceInfo> listInfo)
        {
            try
            {
                var varRecord = from a in ctx.F_GoodsAttributeRecord
                                where a.GoodsID == goodsID && a.AttributeID == (int)CE_GoodsAttributeName.替换件
                                && a.AttributeValue != null
                                select a;

                if (varRecord.Count() != 1)
                {
                    return;
                }

                var varData = from a in ctx.F_GoodsReplaceInfo
                              where a.AttributeRecordID == varRecord.Single().AttributeRecordID
                              select a;

                ctx.F_GoodsReplaceInfo.DeleteAllOnSubmit(varData);

                foreach (View_F_GoodsReplaceInfo replaceInfo in listInfo)
                {
                    F_GoodsReplaceInfo tempLnq = new F_GoodsReplaceInfo();

                    tempLnq.AttributeRecordID = varRecord.Single().AttributeRecordID;
                    tempLnq.ReplaceGoodsID = replaceInfo.物品ID;
                    tempLnq.ReplaceRate = replaceInfo.替换比率;

                    ctx.F_GoodsReplaceInfo.InsertOnSubmit(tempLnq);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 保存产品编码列表
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="goodsID">物品ID</param>
        /// <param name="listInfo">产品编码列表</param>
        void SaveGoodsWaterCodeList(DepotManagementDataContext ctx, int goodsID, List<F_ProductWaterCode> listInfo)
        {
            try
            {
                var varRecord = from a in ctx.F_GoodsAttributeRecord
                                where a.GoodsID == goodsID && a.AttributeID == (int)CE_GoodsAttributeName.流水码
                                && a.AttributeValue != null
                                select a;

                if (varRecord.Count() != 1)
                {
                    return;
                }

                var varData = from a in ctx.F_GoodsReplaceInfo
                              where a.AttributeRecordID == varRecord.Single().AttributeRecordID
                              select a;

                ctx.F_GoodsReplaceInfo.DeleteAllOnSubmit(varData);

                foreach (F_ProductWaterCode item in listInfo)
                {
                    F_ProductWaterCode tempLnq = new F_ProductWaterCode();

                    tempLnq.AttributeRecordID = varRecord.Single().AttributeRecordID;
                    tempLnq.BarcodeExample = item.BarcodeExample;
                    tempLnq.BarcodeRole = item.BarcodeRole;
                    tempLnq.SteelSealExample = item.SteelSealExample;
                    tempLnq.SteelSealRole = item.SteelSealRole;

                    ctx.F_ProductWaterCode.InsertOnSubmit(tempLnq);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 编辑某条属性记录
        /// </summary>
        /// <param name="record">属性记录对象</param>
        public void SaveGoodsAttirbute(F_GoodsAttributeRecord record)
        {
            try
            {
                if (record == null)
                {
                    return;
                }

                DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

                var varData = from a in ctx.F_GoodsAttributeRecord
                              where a.GoodsID == record.GoodsID
                              && a.AttributeID == record.AttributeID
                              select a;

                ctx.F_GoodsAttributeRecord.DeleteAllOnSubmit(varData);
                ctx.F_GoodsAttributeRecord.InsertOnSubmit(record);
                ctx.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 修改计划价格信息
        /// </summary>
        /// <param name="id">物品ID</param>
        /// <param name="newUnitPrice">新计划价格</param>
        /// <param name="newUnitID">新单位ID</param>
        /// <param name="userCode">用户编码</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回操作是否成功的标志</returns>
        public bool UpdateGoodsPrice(int id, decimal newUnitPrice, int newUnitID, string userCode, out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from r in dataContxt.F_GoodsPlanCost where r.ID == id select r;

                if (result.Count() == 0)
                {
                    error = "找不到基础物品信息无法进行此操作";
                    return false;
                }

                F_GoodsPlanCost goodsPlanCost = result.Single();

                goodsPlanCost.GoodsUnitPrice = newUnitPrice;
                goodsPlanCost.UnitID = newUnitID;
                goodsPlanCost.UserCode = userCode;

                dataContxt.SubmitChanges();
                return true;
            }
            catch (Exception exce)
            {
                error = exce.Message;
                return false;
            }
        }

        #endregion

        /// <summary>
        /// 检查业务中是否存在此物品相关信息
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <param name="prompt">存在时返回的提示信息</param>
        /// <returns>存在返回true, 不存在返回false</returns>
        public bool IsExistInBusiness(int goodsID, out string prompt)
        {
            prompt = null;

            Hashtable paramTable = new Hashtable();
            DataSet ds = new DataSet();

            paramTable.Add("@GoodsID", goodsID);

            Dictionary<OperateCMD, object> dicOperateCMD = m_dbOperate.RunProc_CMD("S_IsExistGoodsInBusiness", ds, paramTable);

            if (!Convert.ToBoolean(dicOperateCMD[OperateCMD.Return_OperateResult]))
            {
                prompt = Convert.ToString(dicOperateCMD[OperateCMD.Return_Errmsg]);

                if (prompt == "没有找到任何数据")
                {
                    return false;
                }
                else
                {
                    prompt = ds.Tables[0].Rows[0][0].ToString();
                    return true;
                }
            }
            else
            {
                prompt = "存储过程执行失败";
                return true;
            }
        }

        /// <summary>
        /// 获得物品ID
        /// </summary>
        /// <param name="goodsCode">图号</param>
        /// <param name="goodsName">物品名称</param>
        /// <param name="spec">规格</param>
        /// <returns>成功返回获取到的物品ID，失败返回0</returns>
        public int GetGoodsID(string goodsCode, string goodsName, string spec)
        {
            string error;

            View_F_GoodsPlanCost goods = GetGoodsInfo(goodsCode, goodsName, spec, out error);

            if (goods != null)
            {
                return goods.序号;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 根据CVT型号获得CVT信息
        /// </summary>
        /// <param name="CVTCode">CVT型号</param>
        /// <param name="storageID">库房ID</param>
        /// <returns>成功返回CVT信息，失败返回null</returns>
        public DataTable GetCVTInfo(string CVTCode, string storageID)
        {
            string sql = @"select * from (select b.序号, b.图号型号, b.物品名称, b.规格, Sum(a.ExistCount) as 库存数量, b.单位, a.unitPrice 单价," +
                          " a.Depot as 物品类别,a.StorageID as 库房代码,a.provider as 供应商,a.BatchNo 批次" +
                          " from S_Stock as a inner join View_F_GoodsPlanCost as b on a.GoodsID = b.序号" +
                          "  group by b.序号, b.图号型号, b.物品名称, b.规格, b.单位," +
                          "  a.Depot,a.StorageID,a.provider,a.BatchNo,a.unitPrice) as a  where 图号型号 = " +
                          " '" + CVTCode + "' and 库房代码='" + storageID + "'";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            return dt;
        }

        /// <summary>
        /// 通过图号型号查找物品的序号
        /// </summary>
        /// <param name="goodsCode">图号型号</param>
        /// <param name="goodsName">物品名称</param>
        /// <param name="spec">规格</param>
        /// <returns>成功返回序号，失败返回null</returns>
        public int GetGoodsIDByGoodsCode(string goodsCode, string goodsName, string spec)
        {
            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from c in dataContxt.View_F_GoodsPlanCost
                             where c.图号型号 == goodsCode && c.物品名称 == goodsName && c.规格 == spec
                             select c.序号;

                return result.Single();
            }
            catch (Exception)
            {
                return 0;
            }
        }       

        /// <summary>
        /// 获得基础物品属性列表
        /// </summary>
        /// <returns>返回List</returns>
        public List<F_GoodsAttribute> GetGoodsAttributeList()
        {
            try
            {
                DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

                var varData = from a in ctx.F_GoodsAttribute
                              where a.IsDisable == false
                              select a;

                if (varData == null)
                {
                    return null;
                }
                else
                {
                    return varData.ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 获得属性对象
        /// </summary>
        /// <param name="attributeID">属性ID</param>
        /// <returns>返回属性对象</returns>
        public F_GoodsAttribute GetGoodsAttirbute(int attributeID)
        {
            try
            {
                DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

                var varData = from a in ctx.F_GoodsAttribute
                              where a.AttributeID == attributeID
                              select a;

                if (varData == null || varData.Count() == 0)
                {
                    return null;
                }
                else
                {
                    return varData.Single();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 获得物品的属性列表
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <returns>返回属性列表</returns>
        public List<F_GoodsAttributeRecord> GetGoodsAttirbuteRecordList(int goodsID)
        {
            try
            {
                DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

                var varData = from a in ctx.F_GoodsAttributeRecord
                              where a.GoodsID == goodsID
                              select a;

                if (varData.Count() == 0)
                {
                    return null;
                }
                else
                {
                    return varData.ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 获得包装数量
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <param name="provider">供应商</param>
        /// <returns>返回包装数</returns>
        public decimal GetPackCount(int goodsID, string provider)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.B_GoodsLeastPackAndStock
                          where a.GoodsID == goodsID
                          select a;

            if (varData.Count() == 0)
            {
                throw new Exception("无包装信息" + UniversalFunction.GetGoodsMessage(goodsID));
            }
            else
            {
                if (provider != null && provider.Trim().Length > 0)
                {
                    var varResult = from a in varData
                                    where a.Provider == provider
                                    select a;

                    if (varResult.Count() > 0)
                    {
                        return (decimal)varResult.First().LeastPack;
                    }
                }


                return (decimal)varData.First().LeastPack;
            }
        }

        public F_GoodsAttributeRecord GetGoodsAttirbuteRecord(DepotManagementDataContext ctx, int goodsID, int attributeID)
        {
            try
            {
                var varData = from a in ctx.F_GoodsAttributeRecord
                              where a.GoodsID == goodsID
                              && a.AttributeID == attributeID
                              select a;

                if (varData.Count() != 1)
                {
                    return null;
                }
                else
                {
                    return varData.Single();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public F_GoodsAttributeRecord GetGoodsAttirbuteRecord(int goodsID, int attributeID)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            try
            {
                var varData = from a in ctx.F_GoodsAttributeRecord
                              where a.GoodsID == goodsID
                              && a.AttributeID == attributeID
                              select a;

                if (varData.Count() != 1)
                {
                    return null;
                }
                else
                {
                    return varData.Single();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}

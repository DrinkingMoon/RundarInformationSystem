/******************************************************************************
 * 版权所有 (c) 2006-2010, 湖南江麓容大车辆传动有限责任公司
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
    class BasicGoodsServer : BasicServer, IBasicGoodsServer
    {
        #region 成员变量
        
        /// <summary>
        /// 仓库类别服务
        /// </summary>
        IDepotTypeForPersonnel m_depotTypeServer = ServerModuleFactory.GetServerModule<IDepotTypeForPersonnel>();

        #endregion

        /// <summary>
        /// 根据条件筛选出基础物品数据集
        /// </summary>
        /// <param name="goodsCode">图号型号</param>
        /// <param name="goodsName">物品名称</param>
        /// <param name="goodsSpec">规格</param>
        /// <param name="showColumn">显示列</param>
        /// <returns>Table</returns>
        public DataTable GetGoodsTable(string goodsCode, string goodsName, string goodsSpec, string showColumn)
        {
            string strSql = "select distinct " + showColumn + " from F_GoodsPlanCost where 1=1 ";

            if (goodsCode != "")
            {
                strSql += " and GoodsCode = '" + goodsCode + "'";
            }

            if (goodsName != "")
            {
                strSql += " and GoodsName = '" + goodsName + "'";
            }

            if (goodsSpec != "")
            {
                strSql += " and Spec = '" + goodsSpec + "'";
            }

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        /// <summary>
        /// 根据物品ID 获得物品信息
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <returns>获得此物品基础物品信息表的所有信息</returns>
        public DataTable GetGoodsData(int goodsID)
        {
            string strSql = "select a.*,b.UnitName,b.UnitSpec from F_GoodsPlanCost as a "+
                " inner join S_Unit as b on a.UnitID = b.ID where a.ID = " + goodsID;

            DataTable dtTemp = GlobalObject.DatabaseServer.QueryInfo(strSql);

            if (dtTemp == null || dtTemp.Rows.Count != 1)
            {
                return null;
            }
            else
            {
                return dtTemp;
            }

        }

        /// <summary>
        /// 根据图号型号，物品名称，规格，获得物品信息
        /// </summary>
        /// <param name="code">图号型号</param>
        /// <param name="name">物品名称</param>
        /// <param name="spec">规格</param>
        /// <returns>获得此物品基础物品信息表的所有信息</returns>
        public DataTable GetGoodsInfo(string code, string name, string spec)
        {
            string strSql = "select a.*,b.UnitName from F_GoodsPlanCost as a inner join S_Unit as b on a.UnitID = b.ID " +
                " where GoodsCode = '" + code + "' and GoodsName = '" + name + "' and Spec = '" + spec + "'";
            DataTable dtTemp = GlobalObject.DatabaseServer.QueryInfo(strSql);

            if (dtTemp == null || dtTemp.Rows.Count != 1)
            {
                return null;
            }
            else
            {
                return dtTemp;
            }

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
        public int GetGoodsID(string code, string name, string spec,
            string goodsType, int unitID,string remark, out string error)
        {
            try
            {

                // 查找到的符合条件的客户信息
                IQueryable<View_F_GoodsPlanCost> IQFindGoodsPlanCost;

                error = null;

                string strSql = "select * from  F_GoodsPlanCost where GoodsCode = '" +
                    code + "' and GoodsName = '" + name + "' and Spec = '" + spec + "'";


                DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

                if (dt == null || dt.Rows.Count == 0)
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

                    if (!AddGoods(goodsPlanCost, out IQFindGoodsPlanCost, out error))
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
            error = null;

            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            var result = from r in dataContxt.View_F_GoodsPlanCost
                         where r.图号型号 == goodsCode && r.规格 == spec
                         select r;

            if (result.Count() > 0)
            {
                return result.Single();
            }
            else
            {
                error = string.Format("找不到图号 [{0}], 规格 [{1}] 的基础物品信息，请与管理员联系", goodsCode, spec);
                return null;
            }
        }

        /// <summary>
        /// 获取所有物品信息
        /// </summary>
        /// <param name="returnInfo">返回查询到的信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回操作是否成功</returns>
        public bool GetAllGoodsInfo(out IQueryable<View_F_GoodsPlanCost> returnInfo, out string error)
        {
            returnInfo = null;
            error = null;
            
            try
            {
                DepotManagementDataContext depotMangaeDataContext = CommentParameter.DepotDataContext;
                Table<View_F_GoodsPlanCost> goodsPriceTable = depotMangaeDataContext.GetTable<View_F_GoodsPlanCost>();

                returnInfo = from c in goodsPriceTable
                             orderby c.图号型号, c.规格
                             select c;
                return true;
            }
            catch (Exception err)
            {
                error = err.ToString();
                return false;
            }
        }

        /// <summary>
        /// 获取包含在仓库类别列表中的所有信息
        /// </summary>
        /// <param name="userCode">用户编码</param>
        /// <returns>返回获取到的信息</returns>
        public IQueryable<View_F_GoodsPlanCost> GetGoodsInfo(string userCode)
        {
            return GetGoodsInfo(userCode, null);
        }

        /// <summary>
        /// 获取指定用户查询权限内的包含在仓库类别列表中的所有信息
        /// </summary>
        /// <param name="userCode">用户编码</param>
        /// <param name="lstDepotCode">仓库类别列表</param>
        /// <returns>返回获取到的信息</returns>
        public IQueryable<View_F_GoodsPlanCost> GetGoodsInfo(string userCode, List<string> lstDepotCode)
        {
            if (!string.IsNullOrEmpty(userCode))
            {
                List<string> lstUserDepotCode = m_depotTypeServer.GetDepotCodeForPersonnel(userCode);

                if (lstUserDepotCode != null && lstUserDepotCode.Count > 0)
                {
                    if (lstDepotCode != null && lstDepotCode.Count > 0)
                    {
                        lstDepotCode.RemoveAll(p => !lstUserDepotCode.Contains(p));
                    }
                    else
                    {
                        lstDepotCode = lstUserDepotCode;
                    }
                }
            }

            DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;
            IQueryable<View_F_GoodsPlanCost> result = null;

            if (lstDepotCode == null)
            {
                result = (from c in dataContext.View_F_GoodsPlanCost
                          orderby c.图号型号, c.物品名称, c.规格
                          select c);

            }
            else
            {
                result = (from c in dataContext.View_F_GoodsPlanCost
                          where lstDepotCode.Contains(c.物品类别)
                          orderby c.图号型号, c.物品名称, c.规格
                          select c);
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

        /// <summary>
        /// 获取包含在仓库类别列表中的所有信息
        /// </summary>
        /// <param name="lstDepotCode">仓库类别列表</param>
        /// <returns>返回获取到的信息</returns>
        public IQueryable<View_F_GoodsPlanCost> GetGoodsInfo(List<string> lstDepotCode)
        {
            return GetGoodsInfo(null, lstDepotCode);
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

        /// <summary>
        /// 添加计划价格信息
        /// </summary>
        /// <param name="dataContext">数据上下文</param>
        /// <param name="goodsCost">物品价格信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功添加计划价格信息</returns>
        public bool AddGoods(DepotManagementDataContext dataContext, F_GoodsPlanCost goodsCost, out string error)
        {
            error = null;

            try
            {
                var result = from r in dataContext.F_GoodsPlanCost
                             where r.GoodsCode == goodsCost.GoodsCode && r.GoodsName == goodsCost.GoodsName 
                             && r.Spec == goodsCost.Spec
                             select r;

                if (result.Count() > 0)
                {
                    return true;
                }

                goodsCost.Date = ServerTime.Time.Date;
                dataContext.F_GoodsPlanCost.InsertOnSubmit(goodsCost);

                return true;
            }
            catch (Exception exce)
            {
                error = exce.Message;
                return false;
            }
        }

        /// <summary>
        /// 添加计划价格信息
        /// </summary>
        /// <param name="goodsCost">物品价格信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功添加计划价格信息</returns>
        public bool AddGoods(F_GoodsPlanCost goodsCost, out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                goodsCost.Date = ServerTime.Time.Date;
                dataContxt.F_GoodsPlanCost.InsertOnSubmit(goodsCost);
                dataContxt.SubmitChanges();

                return true;
            }
            catch (Exception exce)
            {
                error = exce.Message;
                return false;
            }
        }

        /// <summary>
        /// 添加计划价格信息
        /// </summary>
        /// <param name="goodsCost">物品价格信息</param>
        /// <param name="returnInfo">计划价格信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功添加计划价格信息</returns>
        public bool AddGoods(F_GoodsPlanCost goodsCost, out IQueryable<View_F_GoodsPlanCost> returnInfo, out string error)
        {
            returnInfo = null;

            if (!AddGoods(goodsCost, out error))
            {
                return false;
            }

            GetAllGoodsInfo(out returnInfo, out error);

            return error == null;
        }

        /// <summary>
        /// 修改计划价格信息
        /// </summary>
        /// <param name="goodsCost">物品价格信息</param>
        /// <param name="returnInfo">计划价格信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功添加计划价格信息</returns>
        public bool UpdateGoods(F_GoodsPlanCost goodsCost, out IQueryable<View_F_GoodsPlanCost> returnInfo, out string error)
        {
            returnInfo = null;

            try
            {
                if (UpdateGoods(goodsCost, out error))
                {
                    return GetAllGoodsInfo(out returnInfo, out error);
                }
                else
                {
                    return false;
                }
            }
            catch (Exception exce)
            {
                error = exce.Message;
                return false;
            }
        }

        /// <summary>
        /// 修改计划价格信息
        /// </summary>
        /// <param name="goodsCost">物品价格信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功添加计划价格信息</returns>
        public bool UpdateGoods(F_GoodsPlanCost goodsCost, out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from r in dataContxt.F_GoodsPlanCost 
                             where r.ID == goodsCost.ID 
                             select r;

                if (result.Count() == 0)
                {
                    error = "找不到基础物品信息无法进行此操作";
                    return false;
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
                        return false;
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

                if (goodsPlanCost.GoodsType != goodsCost.GoodsType)
                {
                    UpdateGoodsType(dataContxt, goodsPlanCost.ID, goodsCost.GoodsType);
                }

                dataContxt.SubmitChanges();

                return true;
            }
            catch (Exception exce)
            {
                error = exce.Message;
                return false;
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

        /// <summary>
        /// 修改物品类别
        /// </summary>
        /// <param name="id">物品ID</param>
        /// <param name="newGoodsType">新物品类别</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回操作是否成功的标志</returns>
        public bool UpdateGoodsType(int id, string newGoodsType, out string error)
        {
            try
            {
                DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

                UpdateGoodsType(ctx, id, newGoodsType);

                ctx.SubmitChanges();

                error = null;

                return true;
            }
            catch (Exception exce)
            {
                error = exce.Message;
                return false;
            }
        }

        /// <summary>
        /// 修改物品类别
        /// </summary>
        /// <param name="ctx">LINQ 数据上下文</param>
        /// <param name="id">物品ID</param>
        /// <param name="newGoodsType">新物品类别</param>
        public void UpdateGoodsType(DepotManagementDataContext ctx, int id, string newGoodsType)
        {
            System.Nullable<System.Int32> goodsID = id;

            ctx.Update_GoodsDepotType(goodsID, newGoodsType);
        }

        /// <summary>
        /// 检查业务中是否存在此物品相关信息
        /// </summary>
        /// <param name="id">物品ID</param>
        /// <param name="prompt">存在时返回的提示信息</param>
        /// <returns>存在返回true, 不存在返回false</returns>
        public bool IsExistInBusiness(int id, out string prompt)
        {
            prompt = null;

            IStoreServer storeServer = ServerModuleFactory.GetServerModule<IStoreServer>();
            View_S_Stock storeData = storeServer.GetGoodsStore(id);

            if (storeServer != null)
            {
                prompt = "库存中存在此物品的相关信息";
                return true;
            }

            ICheckOutInDepotServer cs = ServerModuleFactory.GetServerModule<ICheckOutInDepotServer>();

            if (cs.IsExist(id))
            {
                prompt = "报检入库单中存在此物品的相关信息";
                return true;
            }

            IOrdinaryInDepotGoodsBill og = ServerModuleFactory.GetServerModule<IOrdinaryInDepotGoodsBill>();

            if (og.IsExist(id))
            {
                prompt = "普通入库单中存在此物品的相关信息";
                return true;
            }

            IHomemadePartInDepotServer hs = ServerModuleFactory.GetServerModule<IHomemadePartInDepotServer>();

            if (hs.IsExist(id))
            {
                prompt = "自制件入库单中存在此物品的相关信息";
                return true;
            }

            return false;
        }

        /// <summary>
        /// 删除计划价格信息
        /// </summary>
        /// <param name="id">序号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功删除计划价格信息</returns>
        public bool DeleteGoodsPrice(int id, out string error)
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
        public bool DeleteGoods(int id, out IQueryable<View_F_GoodsPlanCost> returnInfo, out string error)
        {
            returnInfo = null;

            if (DeleteGoodsPrice(id, out error))
            {
                return GetAllGoodsInfo(out returnInfo, out error);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 获得物品类别
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <returns>返回物品的单据类别</returns>
        public string GetDepot(int goodsID)
        {
            string strSql = "select GoodsType from F_GoodsPlanCost where ID = " + goodsID;
            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

            return dt.Rows[0][0].ToString();
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
            string sql = @"select * from (select b.ID as 序号, b.GoodsCode as 图号型号,b.GoodsName as 物品名称," +
                          " b.Spec as 规格,Sum(a.ExistCount) as 库存数量,c.UnitName as 单位,a.unitPrice 单价," +
                          " a.Depot as 物品类别,a.StorageID as 库房代码,a.provider as 供应商,a.BatchNo 批次" +
                          " from S_Stock as a inner join F_GoodsPlanCost as b" +
                          " on a.GoodsID = b.ID inner join S_Unit as c on b.UnitID = c.ID" +
                          "  group by b.ID,b.GoodsCode,b.GoodsName,b.Spec,c.UnitName," +
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
        /// <returns>成功返回序号，失败返回null</returns>
        public int GetGoodsIDByGoodsCode(string goodsCode,string goodsName)
        {
            int error = 0;

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from c in dataContxt.View_F_GoodsPlanCost
                             where c.图号型号 == goodsCode && c.物品名称 == goodsName
                             select c.序号;

                return result.Single();
            }
            catch (Exception err)
            {
                error = Convert.ToInt32(err.ToString());

                return error;
            }
        }

        /// <summary>
        /// 通过图号型号查找物品的序号
        /// </summary>
        /// <param name="goodsCode">图号型号</param>
        /// <returns>成功返回序号，失败返回null</returns>
        public int GetGoodsIDByGoodsCode(string goodsCode)
        {
            int error = 0;

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from c in dataContxt.View_F_GoodsPlanCost
                             where c.图号型号 == goodsCode
                             select c.序号;

                return result.Single();
            }
            catch (Exception err)
            {
                error = Convert.ToInt32(err.ToString());

                return error;
            }
        }
    }
}

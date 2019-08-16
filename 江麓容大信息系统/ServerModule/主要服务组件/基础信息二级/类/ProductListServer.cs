/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  ProductListServer.cs
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
using GlobalObject;

namespace ServerModule
{
    /// <summary>
    /// 用于营销产品类管理类
    /// </summary>
    class ProductListServer : IProductListServer
    {
        /// <summary>
        /// 数据库操作接口
        /// </summary>
        static IDBOperate m_dbOperate = CommentParameter.GetDBOperatorOfDepotManagement();

        /// <summary>
        /// 获得车型
        /// </summary>
        /// <returns>返回车型记录列表</returns>
        public DataTable GetMotorcycleType()
        {
            string strSql = "select * from S_MotorcycleType";

            DataTable dtTemp = GlobalObject.DatabaseServer.QueryInfo(strSql);
            return dtTemp;
        }

        /// <summary>
        /// 根据车型获得ID
        /// </summary>
        /// <param name="carModel">车型</param>
        /// <returns>返回车型ID</returns>
        public int GetMotorcycleType(string carModel)
        {
            string strSql = @"select ID from S_MotorcycleType where CarModel='" + carModel + "'";

            DataTable dtTemp = GlobalObject.DatabaseServer.QueryInfo(strSql);

            if (dtTemp.Rows.Count == 0)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32( dtTemp.Rows[0][0].ToString());
            }

        }

        /// <summary>
        /// 根据车型ID获得车型
        /// </summary>
        /// <param name="carModelID">车型ID</param>
        /// <returns>返回车型</returns>
        public string GetMotorcycleInfo(int carModelID)
        {
            string strSql = "select CarModel from S_MotorcycleType where ID = " + carModelID;

            DataTable dtTemp = GlobalObject.DatabaseServer.QueryInfo(strSql);

            return dtTemp.Rows[0][0].ToString();
        }

        /// <summary>
        /// 判断此物品是否存在于成品库中
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <returns>存在返回True,不存在返回False</returns>
        public bool IsInProductStock(int goodsID)
        {
            string strSql = "select * from S_Stock where StorageID = '02' and GoodsID = "+ goodsID;

            DataTable dtTemp = GlobalObject.DatabaseServer.QueryInfo(strSql);

            if (dtTemp.Rows.Count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 获得产品的GoodsID且同时判断此物品ID,图号型号是否属于产品
        /// </summary>
        /// <param name="productCode">产品型号</param>
        /// <param name="goodsID">物品ID</param>
        /// <param name="flagTCU">是否过滤TCU， True： 是，False： 否</param>
        /// <returns>返回物品ID，若为0则不属于产品</returns>
        public int GetProductGoodsID(string productCode, int goodsID , bool flagTCU)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            List<int> lstInt = new List<int>();
            lstInt.Add((int)CE_GoodsAttributeName.CVT);

            if (!flagTCU)
            {
                lstInt.Add((int)CE_GoodsAttributeName.TCU);
            }

            var varData = from a in ctx.F_GoodsAttributeRecord
                          join b in ctx.F_GoodsPlanCost
                          on a.GoodsID equals b.ID
                          where (b.ID == goodsID || b.GoodsCode == productCode)
                          && a.AttributeValue == "True" && lstInt.Contains(a.AttributeID)
                          select a.GoodsID;

            if (varData == null || varData.Count() == 0)
            {
                return 0;
            }
            else if(varData.Count() == 1)
            {
                return varData.Single();
            }
            else
            {
                throw new Exception("数据不唯一");
            }
        }

        /// <summary>
        /// 获得某一个产品信息
        /// </summary>
        /// <returns>返回一个产品信息</returns>
        public DataRow GetGoodsPlanID(string GoodsCode)
        {
            string strSql = "select 序号 from View_F_GoodsPlanCost WHERE (图号型号 = '" + GoodsCode + "')";

            return GlobalObject.DatabaseServer.QueryInfo(strSql).Rows[0];
        }

        /// <summary>
        /// 获得产品信息
        /// </summary>
        /// <returns>返回产品信息</returns>
        public DataTable GetProductInfo()
        {
            string strSql = "select a.GoodsCode as 产品编码, a.GoodsName as 产品名称 from F_GoodsPlanCost as a where ID in ( " +
                            " select distinct ParentID from BASE_BomStruct where ParentID in (select GoodsID from F_GoodsAttributeRecord   " +
                                       " where AttributeID in (" + (int)CE_GoodsAttributeName.CVT + ","
                                       + (int)CE_GoodsAttributeName.TCU + ") and AttributeValue = '" + bool.TrueString 
                                       + "') and ParentID not in (select GoodsID from F_GoodsAttributeRecord where AttributeID = " 
                                       + (int)CE_GoodsAttributeName.停产 + " and AttributeValue = '" + bool.TrueString + "'))";

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        /// <summary>
        /// 获取与营销有关的所有产品信息
        /// </summary>
        /// <param name="error">出错时输出的错误信息</param>
        /// <returns>成功返回获取到的产品信息，失败返回null</returns>
        public DataTable GetAllProductList(out string error)
        {
            error = null;

            DataSet ds = new DataSet();
            Dictionary<OperateCMD, object> dicOperateCMD = m_dbOperate.RunProc_CMD("", ds);

            if (!Convert.ToBoolean(dicOperateCMD[OperateCMD.Return_OperateResult]))
            {
                error = Convert.ToString(dicOperateCMD[OperateCMD.Return_Errmsg]);
                return null;
            }

            return ds.Tables[0];
        }
    }
}

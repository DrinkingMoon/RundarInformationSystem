/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  ProductInfoServer.cs
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

namespace ServerModule
{
    /// <summary>
    /// 产品信息管理类
    /// </summary>
    class ProductInfoServer : IProductInfoServer
    {
        /// <summary>
        /// 数据库操作接口
        /// </summary>
        IDBOperate m_dbOperate = CommentParameter.GetDBOperatorOfDepotManagement();

        /// <summary>
        /// 获取产品信息信息
        /// </summary>
        /// <param name="returnProductInfo">操作后查询返回的产品信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>获取成功返回True，获取失败返回False</returns>
        public bool GetAllProductInfo(out IQueryable<View_P_ProductInfo> returnProductInfo, out string error)
        {
            error = null;

            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            try
            {
                Table<View_P_ProductInfo> table = dataContxt.GetTable<View_P_ProductInfo>();

                returnProductInfo = from c in table
                                    select c;

                return true;
            }
            catch(Exception err)
            {
                return SetReturnError(err, out returnProductInfo, out error);
            }
        }

        /// <summary>
        /// 获取所有产品类型编码
        /// </summary>
        /// <param name="procuctTypes">获取到的产品类型编码</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回true</returns>
        public bool GetAllProductType(out string[] procuctTypes, out string error)
        {
            procuctTypes = null;
            error = null;

            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            try
            {
                Table<P_ProductInfo> table = dataContxt.GetTable<P_ProductInfo>();

                procuctTypes = (from c in table select c.ProductType).ToArray();

                return true;
            }
            catch (Exception err)
            {
                error = err.Message;
                return false;
            }
        }

        /// <summary>
        /// 获取产品信息信息
        /// </summary>
        /// <param name="returnProductInfo">操作后查询返回的产品信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>获取成功返回True，获取失败返回False</returns>
        public bool GetRemovedTCU(out IQueryable<View_P_ProductInfo> returnProductInfo, out string error)
        {
            error = null;

            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            try
            {
                Table<View_P_ProductInfo> table = dataContxt.GetTable<View_P_ProductInfo>();

                returnProductInfo = from c in table
                                    where !c.产品类型名称.Contains("TCU")
                                    && c.是否返修专用 == false
                                    select c;

                return true;
            }
            catch (Exception err)
            {
                return SetReturnError(err, out returnProductInfo, out error);
            }
        }

        /// <summary>
        /// 获取指定产品类型的产品信息
        /// </summary>
        /// <param name="productType">产品类型</param>
        /// <returns>返回获取到的信息</returns>
        public View_P_ProductInfo GetProductInfo(string productType)
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            try
            {
                var result = from c in dataContxt.View_P_ProductInfo 
                             where c.产品类型编码 == productType 
                             select c;

                return result.First();
            }
            catch (Exception err)
            {
                Console.WriteLine("{0}->GetProductInfo()：{1}", this.GetType(), err.Message);
                return null;
            }
        }

        /// <summary>
        /// 添加产品信息
        /// </summary>
        /// <param name="productInfo">产品信息</param>
        /// <param name="returnProductInfo">返回重新查询到的产品信息</param>
        /// <param name="error">出错时返回的错误信息</param>
        /// <returns>操作是否成功的标志</returns>
        public bool AddProductInfo(P_ProductInfo productInfo, out IQueryable<View_P_ProductInfo> returnProductInfo, out string error)
        {
            returnProductInfo = null;
            error = null;

            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            try
            {
                Table<P_ProductInfo> table = dataContxt.GetTable<P_ProductInfo>();

                var billGather = from c in table 
                                 where c.ProductType == productInfo.ProductType 
                                 select c;

                int intSameNoteCount = billGather.Count<P_ProductInfo>();

                if (intSameNoteCount == 0)
                {
                    dataContxt.P_ProductInfo.InsertOnSubmit(productInfo);
                    dataContxt.SubmitChanges();

                    return GetAllProductInfo(out returnProductInfo, out error);
                }
                else
                {
                    error = "数据库中已存在该编码的产品信息!";
                    return false;
                }
            }
            catch (Exception err)
            {
                return SetReturnError(err, out returnProductInfo, out error);
            }
        }

        /// <summary>
        /// 更新产品信息
        /// </summary>
        /// <param name="productInfo">更新后的产品信息</param>
        /// <param name="returnProductInfo">返回重新查询到的产品信息</param>
        /// <param name="error">出错时返回的错误信息</param>
        /// <returns>操作是否成功的标志</returns>
        public bool UpdateProductInfo(P_ProductInfo productInfo, out IQueryable<View_P_ProductInfo> returnProductInfo, out string error)
        {
            returnProductInfo = null;
            error = null;

            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            try
            {
                Table<P_ProductInfo> table = dataContxt.GetTable<P_ProductInfo>();

                var result = from c in table 
                             where c.ProductType == productInfo.ProductType 
                             select c;

                if (result.Count() != 0)
                {
                    P_ProductInfo updateInfo = result.Single();

                    updateInfo.ProductCode = productInfo.ProductCode;
                    updateInfo.ProductName = productInfo.ProductName;
                    updateInfo.IsReturn = productInfo.IsReturn;
                    updateInfo.Remark = productInfo.Remark;

                    dataContxt.SubmitChanges();
                }

                return GetAllProductInfo(out returnProductInfo, out error);
            }
            catch (Exception err)
            {
                return SetReturnError(err, out returnProductInfo, out error);
            }
        }

        /// <summary>
        /// 删除产品信息
        /// </summary>
        /// <param name="id">要删除的产品信息ID</param>
        /// <param name="returnProductInfo">返回重新查询到的产品信息</param>
        /// <param name="error">出错时返回的错误信息</param>
        /// <returns>操作是否成功的标志</returns>
        public bool DeleteProductInfo(int id, out IQueryable<View_P_ProductInfo> returnProductInfo, out string error)
        { 
            error = null;

            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            try
            {
                Table<P_ProductInfo> table = dataContxt.GetTable<P_ProductInfo>();

                var delRow = from c in table 
                             where c.ID == id 
                             select c;

                table.DeleteAllOnSubmit(delRow);
                dataContxt.SubmitChanges();

                return GetAllProductInfo(out returnProductInfo, out error);
            }
            catch (Exception err)
            {
                return SetReturnError(err, out returnProductInfo, out error);
            }
        }

        /// <summary>
        /// 设置出错返回值
        /// </summary>
        /// <param name="err">传入的错误信息</param>
        /// <param name="returnProductInfo">返回的信息</param>
        /// <param name="error">返回的错误信息</param>
        /// <returns>始终返回False</returns>
        bool SetReturnError(object err, out IQueryable<View_P_ProductInfo> returnProductInfo, out string error)
        {
            returnProductInfo = null;
            error = err.ToString();

            return false;
        }
    }
}

/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  UniqueIdentifier.cs
 * 作者    :  曹津彬    版本: v1.00    日期: 2014/01/22
 * 开发平台:  Visual C# 2008
 * 用于    :  仓库管理软件
 *----------------------------------------------------------------------------
 * 描述 : 
 * 其它 :
 *----------------------------------------------------------------------------
 * 公共信息: 参见系统'类帮助文档'
 *----------------------------------------------------------------------------
 * 历史记录:
 ******************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using System.Collections;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using ServerModule;
using DBOperate;
using PlatformManagement;
using GlobalObject;

namespace Service_Peripheral_External
{
    class UniqueIdentifier : IUniqueIdentifier
    {
        /// <summary>
        /// 删除标识码
        /// </summary>
        /// <param name="dataContext">数据上下文</param>
        /// <param name="uniqueIdentifier">标识码数据集</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool DeleteIdentifier(DepotManagementDataContext dataContext, Out_UniqueIdentifierData uniqueIdentifier, out string error)
        {
            error = null;

            try
            {
                var varData = from a in dataContext.Out_UniqueIdentifierData
                              select a;

                if (uniqueIdentifier.Bill_ID != null && uniqueIdentifier.GoodsID != null && uniqueIdentifier.Identifier != null)
                {
                    varData = from a in varData
                              where a.Bill_ID == uniqueIdentifier.Bill_ID
                              && a.GoodsID == uniqueIdentifier.GoodsID
                              && a.Identifier == uniqueIdentifier.Identifier
                              select a;
                }
                else if (uniqueIdentifier.Bill_ID != null && uniqueIdentifier.GoodsID != null)
                {
                    varData = from a in varData
                              where a.Bill_ID == uniqueIdentifier.Bill_ID
                              && a.GoodsID == uniqueIdentifier.GoodsID
                              select a;

                }
                else if (uniqueIdentifier.Bill_ID != null)
                {
                    varData = from a in varData
                              where a.Bill_ID == uniqueIdentifier.Bill_ID
                              select a;
                }
                else
                {
                    error = "输入信息错误，无法删除标识码";
                    return false;
                }

                dataContext.Out_UniqueIdentifierData.DeleteAllOnSubmit(varData);

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;

            }
        }

        /// <summary>
        /// 获得标识码数据
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="goodsID">物品ID</param>
        /// <param name="storageID">账务库房ID</param>
        /// <returns>返回Table</returns>
        public DataTable GetInfo(string billNo,int goodsID,string storageID)
        {
            string strSql = "select Identifier from Out_UniqueIdentifierData where Bill_ID = '"+ billNo +"' and GoodsID = " + goodsID +" and StorageID = '"+ storageID +"'";

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        /// <summary>
        /// 提交信息
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="goodsID">物品ID</param>
        /// <param name="storageID">账务库房ID</param>
        /// <param name="uniqueIdentifier">标识表</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool SubmitInfo(string billNo, int goodsID, string storageID, DataTable uniqueIdentifier, out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

                var varData = from a in dataContext.Out_UniqueIdentifierData
                              where a.Bill_ID == billNo
                              && a.GoodsID == goodsID
                              && a.StorageID == storageID
                              select a;

                dataContext.Out_UniqueIdentifierData.DeleteAllOnSubmit(varData);

                for (int i = 0; i < uniqueIdentifier.Rows.Count; i++)
                {
                    Out_UniqueIdentifierData lnqIdentifier = new Out_UniqueIdentifierData();

                    lnqIdentifier.Bill_ID = billNo;
                    lnqIdentifier.GoodsID = goodsID;
                    lnqIdentifier.Identifier = uniqueIdentifier.Rows[i][0].ToString();
                    lnqIdentifier.StorageID = storageID;

                    dataContext.Out_UniqueIdentifierData.InsertOnSubmit(lnqIdentifier);
                }

                dataContext.SubmitChanges();

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
                
            }
        }
    }
}

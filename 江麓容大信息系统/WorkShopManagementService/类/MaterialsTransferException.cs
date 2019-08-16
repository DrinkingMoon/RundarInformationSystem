/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  MaterialsTransferException.cs
 * 作者    :  曹津彬    版本: v1.00    日期: 2014/05/04
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
using System.Text;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using System.Collections;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using DBOperate;
using PlatformManagement;
using GlobalObject;
using System.Text.RegularExpressions;
using ServerModule;
using System.Reflection;
using System.Collections.Generic;
using System.IO;

namespace Service_Manufacture_WorkShop
{
    public enum MaterialsTransferExceptionStatus
    {
        待处理,
        已处理
    }

    /// <summary>
    /// 自动生成转换异常信息处理类
    /// </summary>
    public class MaterialsTransferException : IMaterialsTransferException
    {
        /// <summary>
        /// 检查某业务是否存在
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="billNo">业务号</param>
        /// <returns>存在返回true</returns>
        public bool IsExist(DepotManagementDataContext ctx, string billNo)
        {
            var varData = from a in ctx.WS_MaterialsTransferExceptionBill
                          where a.BillNo == billNo
                          select a;

            if (varData == null || varData.Count() == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 检查某单据是否存在
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <returns>存在返回true</returns>
        public bool IsExist(string billNo)
        {
            string sql = "SELECT * FROM [DepotManagement].[dbo].[WS_MaterialsTransferExceptionBill] where BillNo = '" + billNo + "'";

            System.Data.DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            if (dt != null && dt.Rows.Count > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 获得所有异常信息
        /// </summary>
        /// <returns>返回Table</returns>
        public DataTable GetBillInfo()
        {
            string strSql = "select * from View_WS_MaterialsTransferExceptionBill order by 单据号 desc";

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        /// <summary>
        /// 获得单条异常信息
        /// </summary>
        /// <param name="billNo">单号</param>
        /// <returns>返回Table</returns>
        public WS_MaterialsTransferExceptionBill GetSingleBillInfo(string billNo)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.WS_MaterialsTransferExceptionBill
                          where a.BillNo == billNo
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

        /// <summary>
        /// 获得单条异常的所有明细信息
        /// </summary>
        /// <param name="billNo">单号</param>
        /// <returns>返回Table</returns>
        public DataTable GetListInfo(string billNo)
        {
            string strSql = "select * from View_WS_MaterialsTransferExceptionList where 单据号 = '"+ billNo +"'";

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        /// <summary>
        /// 获得单条异常的单条明细信息
        /// </summary>
        /// <param name="billNo">单号</param>
        /// <param name="goodsID">物品ID</param>
        /// <param name="batchNo">批次号</param>
        /// <returns>返回单条LINQ</returns>
        public WS_MaterialsTransferExceptionList GetSingleListInfo(string billNo, int goodsID, string batchNo)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.WS_MaterialsTransferExceptionList
                          where a.BillNo == billNo
                          && a.GoodsID == goodsID
                          && a.BatchNo == batchNo
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

        /// <summary>
        /// 获得异常处理所有信息
        /// </summary>
        /// <param name="listID">异常信息ID</param>
        /// <returns>返回Table</returns>
        public DataTable GetDisposeInfo(int listID)
        {
            string strSql = "select * from View_WS_MaterialsTransferExceptionListDispose where 异常信息ID = " + listID;

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        /// <summary>
        /// 处理异常信息
        /// </summary>
        /// <param name="mode">操作模式</param>
        /// <param name="operationDispose">处理信息</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True,失败返回False</returns>
        public bool OperationDisposeInfo(GlobalObject.CE_OperatorMode mode, WS_MaterialsTransferExceptionListDispose operationDispose, 
            out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

                var varData = from a in ctx.WS_MaterialsTransferExceptionListDispose
                              select a;

                WS_MaterialsTransferExceptionListDispose tempLnq = new WS_MaterialsTransferExceptionListDispose();

                switch (mode)
                {
                    case CE_OperatorMode.添加:

                        tempLnq.BatchNo = operationDispose.BatchNo;
                        tempLnq.Counts = operationDispose.Counts;
                        tempLnq.GoodsID = operationDispose.GoodsID;
                        tempLnq.ListID = operationDispose.ListID;

                        ctx.WS_MaterialsTransferExceptionListDispose.InsertOnSubmit(operationDispose);
                        break;
                    case CE_OperatorMode.修改:

                        varData = from a in varData
                                  where a.ID == operationDispose.ID
                                  select a;

                        if (varData.Count() != 1)
                        {
                            throw new Exception("数据序号不唯一");
                        }
                        else
                        {
                            tempLnq = varData.Single();

                            tempLnq.BatchNo = operationDispose.BatchNo;
                            tempLnq.Counts = operationDispose.Counts;
                            tempLnq.GoodsID = operationDispose.GoodsID;
                        }

                        break;
                    case CE_OperatorMode.删除:
                        varData = from a in varData
                                      where a.ID == operationDispose.ID
                                      select a;

                        ctx.WS_MaterialsTransferExceptionListDispose.DeleteAllOnSubmit(varData);
                        break;
                    default:
                        break;
                }

                ctx.SubmitChanges();

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 删除单据
        /// </summary>
        /// <param name="billNo">单号</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True , 失败返回False</returns>
        public bool DeleteBill(string billNo, out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

                var varData = from a in ctx.WS_MaterialsTransferExceptionBill
                              where a.BillNo == billNo && a.BillStatus != MaterialsTransferExceptionStatus.已处理.ToString()
                              select a;

                ctx.WS_MaterialsTransferExceptionBill.DeleteAllOnSubmit(varData);

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 插入异常信息
        /// </summary>
        /// <param name="billNo">单号</param>
        /// <returns>成功返回True ,失败返回False</returns>
        public bool InsertExceptionInfo(string billNo, out string error)
        {
            Hashtable parameters = new Hashtable();

            parameters.Add("@InBillNo", billNo);
            parameters.Add("@OpertionPersonnel", BasicInfo.LoginName);

            if (GlobalObject.DatabaseServer.QueryInfoPro("WS_InsertException", parameters, out error) == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 执行异常信息处理
        /// </summary>
        /// <param name="billNo">单号</param>
        /// <returns>成功返回True ,失败返回False</returns>
        public bool ExcuteDisposeInfo(string billNo, out string error)
        {
            Hashtable parameters = new Hashtable();

            parameters.Add("@InBillNo", billNo);
            parameters.Add("@OpertionPersonnel", BasicInfo.LoginName);

            if (GlobalObject.DatabaseServer.QueryInfoPro("WS_ExceptionDispose", parameters, out error) == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}

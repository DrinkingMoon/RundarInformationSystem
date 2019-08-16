/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  ClientServer.cs
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
using GlobalObject;
using WebServerModule;

namespace ServerModule
{
    /// <summary>
    /// 客户管理类
    /// </summary>
    class ClientServer : IClientServer
    {
        /// <summary>
        /// 获得客户表
        /// </summary>
        /// <returns></returns>
        public DataTable GetClient()
        {
            string strErr = "";

            return GlobalObject.DatabaseServer.QueryInfoPro("BASE_Select_Client", null, out strErr);
        }

        /// <summary>
        /// 获取客户信息
        /// </summary>
        /// <param name="returnClient">客户信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功获取客户信息</returns>
        public bool GetAllClient(out IQueryable<View_Client> returnClient, out string error)
        {
            error = null;

            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            try
            {
                Table<View_Client> table = dataContxt.GetTable<View_Client>();

                returnClient = from c in table select c;
                return true;
            }
            catch (Exception err)
            {
                return SetReturnError(err, out returnClient, out error);
            }
        }

        /// <summary>
        /// 添加客户
        /// </summary>
        /// <param name="clientInfo">客户信息</param>
        /// <param name="returnClient">客户信息结果集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功添加客户信息</returns>
        public bool AddClient(Client clientInfo,
            out IQueryable<View_Client> returnClient, out string error)
        {
            returnClient = null;
            error = null;

            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;
            
            try
            {
                Table<Client> table = dataContxt.GetTable<Client>();

                var varClient = from c in table 
                                where c.ClientCode == clientInfo.ClientCode 
                                select c;

                int sameNoteCount = varClient.Count<Client>();

                if (sameNoteCount == 0)
                {
                    Table<Out_StockInfo> tableOut = dataContxt.GetTable<Out_StockInfo>();
                    var result = from c in tableOut 
                                where c.SecStorageID == clientInfo.ClientCode 
                                select c;

                    int count = result.Count<Out_StockInfo>();

                    if (count == 0)
                    {
                        if (clientInfo.IsSecStorage)
                        {
                            Out_StockInfo stock = new Out_StockInfo();

                            stock.SecStorageID = clientInfo.ClientCode;
                            stock.SecStorageName = clientInfo.ClientName;
                            stock.Remark = clientInfo.Remark;

                            dataContxt.Out_StockInfo.InsertOnSubmit(stock);
                        }

                    }

                    dataContxt.Client.InsertOnSubmit(clientInfo);

                    dataContxt.SubmitChanges();

                    return GetAllClient(out returnClient, out error);
                }
                else
                {
                    error = "该单据已提交,系统不允许重复提交相同编号的供应商!";
                    return false;
                }
            }
            catch (Exception err)
            {
                return SetReturnError(err, out returnClient, out error);
            }
        }

        /// <summary>
        /// 更新客户
        /// </summary>
        /// <param name="clientInfo">客户信息</param>
        /// <param name="oldClient">旧客户信息</param>
        /// <param name="returnClient">客户信息结果集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功更新客户信息</returns>
        public bool UpdateClient(Client clientInfo,Client oldClient,
            out IQueryable<View_Client> returnClient, out string error)
        {
            returnClient = null;
            error = null;

            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            try
            {
                if (clientInfo.ClientCode != oldClient.ClientCode)
                {
                    string strSql = "select distinct ObjectDept from (select ObjectDept from S_MarketingBill union all select OutStorageID " +
                        " from Out_ManeuverBill union all select InStorageID from Out_ManeuverBill) as a where ObjectDept = '" + oldClient.ClientCode + "'";

                    DataTable dtTemp = GlobalObject.DatabaseServer.QueryInfo(strSql);

                    if (dtTemp.Rows.Count > 0)
                    {
                        error = "此编码已存在于业务中不能修改";
                        return false;
                    }
                }

                Table<Client> table = dataContxt.GetTable<Client>();

                var varClientA = from c in table
                                where c.ClientCode != oldClient.ClientCode 
                                && (c.ClientCode == clientInfo.ClientCode || c.ClientName == clientInfo.ClientName)
                                select c;

                if (varClientA.Count() == 0)
                {
                    var varStockInfoA = from a in dataContxt.Out_StockInfo
                                       where a.SecStorageID != oldClient.ClientCode
                                       && (a.SecStorageID == clientInfo.ClientCode || a.SecStorageName == clientInfo.ClientName)
                                       select a;

                    if (varStockInfoA.Count() == 0)
                    {
                        if (oldClient.IsSecStorage == clientInfo.IsSecStorage)
                        {
                            if (clientInfo.IsSecStorage)
                            {


                                var varStockInfo = from a in dataContxt.Out_StockInfo
                                                   where a.SecStorageID == oldClient.ClientCode
                                                   select a;

                                foreach (var item in varStockInfo)
                                {
                                    item.SecStorageID = clientInfo.ClientCode;
                                    item.SecStorageName = clientInfo.ClientName;
                                }
                            }
                        }
                        else
                        {
                            if (clientInfo.IsSecStorage)
                            {
                                Out_StockInfo lnqStock = new Out_StockInfo();

                                lnqStock.SecStorageID = clientInfo.ClientCode;
                                lnqStock.SecStorageName = clientInfo.ClientName;
                                lnqStock.Remark = clientInfo.Remark;

                                dataContxt.Out_StockInfo.InsertOnSubmit(lnqStock);
                            }
                            else
                            {
                                var varDel = from a in dataContxt.Out_StockInfo
                                             where a.SecStorageID == oldClient.ClientCode
                                             && a.SecStorageName == oldClient.ClientName
                                             select a;

                                dataContxt.Out_StockInfo.DeleteAllOnSubmit(varDel);
                            }
                        }

                        var varClient = from c in table
                                        where c.ClientCode == oldClient.ClientCode
                                        select c;

                        foreach (var ei in varClient)
                        {
                            ei.ClientName = clientInfo.ClientName;
                            ei.Remark = clientInfo.Remark;
                            ei.Address = clientInfo.Address;
                            ei.Linkman = clientInfo.Linkman;
                            ei.Phone = clientInfo.Phone;
                            ei.Principal = clientInfo.Principal;
                            ei.Province = clientInfo.Province;
                            ei.ServiceArea = clientInfo.ServiceArea;
                            ei.IsSecStorage = clientInfo.IsSecStorage;
                            ei.AllName = clientInfo.AllName;
                        }

                        dataContxt.SubmitChanges();
                    }
                    else
                    {
                        error = "二级库房编码或名称重复";
                        return false;
                    }
                }
                else
                {
                    error = "编码或名称重复";
                    return false;
                }

                return GetAllClient(out returnClient, out error);
            }
            catch (Exception err)
            {
                return SetReturnError(err, out returnClient, out error);
            }
        }

        /// <summary>
        /// 删除客户
        /// </summary>
        /// <param name="clientCode">客户编码</param>
        /// <param name="returnClient">客户信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功删除客户信息</returns>
        public bool DeleteClient(string clientCode, out IQueryable<View_Client> returnClient, out string error)
        {
            error = null;

            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;
            
            try
            {
                Table<Client> table = dataContxt.GetTable<Client>();

                var delRow = from c in table 
                             where c.ClientCode == clientCode 
                             select c;

                foreach (var ei in delRow)
                {
                    table.DeleteOnSubmit(ei);
                }

                var delInfo = from a in dataContxt.Out_StockInfo
                              where a.SecStorageID == clientCode
                              select a;

                dataContxt.Out_StockInfo.DeleteAllOnSubmit(delInfo);

                dataContxt.SubmitChanges();

                return GetAllClient(out returnClient, out error);
            }
            catch (Exception err)
            {
                return SetReturnError(err, out returnClient, out error);
            }
        }

        /// <summary>
        /// 设置出错返回值
        /// </summary>
        /// <param name="err">传入的错误信息</param>
        /// <param name="returnClient">单位信息</param>
        /// <param name="error">返回的错误信息</param>
        /// <returns>始终为false</returns>
        bool SetReturnError(object err, out IQueryable<View_Client> returnClient, out string error)
        {
            returnClient = null;

            error = err.ToString();

            return false;
        }

        /// <summary>
        /// 获得客户名
        /// </summary>
        /// <param name="clientCode">客户编码</param>
        /// <returns>成功则返回客户名称，失败返回空串</returns>
        public string GetClientName(string clientCode)
        {
            string strSql = "select ClientName from Client where ClientCode = '" + clientCode + "'";
            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

            if (dt.Rows.Count == 0)
            {
                return "";
            }
            else
            {
                return dt.Rows[0][0].ToString();
            }
        }

        /// <summary>
        /// 获得客户编码
        /// </summary>
        /// <param name="clientName">客户名</param>
        /// <returns>成功则返回客户编码，失败返回空串</returns>
        public string GetClientCode(string clientName)
        {
            string strSql = "select ClientCode from Client where ClientName like '%" + clientName + "%'";
            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

            if (dt.Rows.Count == 0)
            {
                return "";
            }
            else
            {
                return dt.Rows[0][0].ToString();
            }
        }
    }
}

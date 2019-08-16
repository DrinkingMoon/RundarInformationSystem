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
    /// 系统日志操作组件
    /// </summary>
    class SystemLogServer : ISystemLogServer
    {
        /// <summary>
        /// 添加基础信息日志
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="ctx">数据上下文</param>
        /// <param name="logID">日志唯一编码</param>
        /// <param name="mode">操作类型</param>
        void AddMainInfo<T>(DepotManagementDataContext ctx, Guid logID, CE_OperatorMode mode)
        {
            SystemLog_Main lnqMain = new SystemLog_Main();

            lnqMain.LogID = logID;
            lnqMain.OperationTime = ServerTime.Time;
            lnqMain.OperationType = mode.ToString();
            lnqMain.Operator = BasicInfo.LoginID;
            lnqMain.TableName = typeof(T).Name;

            ctx.SystemLog_Main.InsertOnSubmit(lnqMain);
        }

        /// <summary>
        /// 添加操作表主键日志
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="ctx">数据上下文</param>
        /// <param name="logID">日志唯一编码</param>
        /// <param name="singleKey">单一主键值</param>
        void AddPrimaryKeyInfo<T>(DepotManagementDataContext ctx, Guid logID, object singleKey)
        {
            string strTableName = typeof(T).Name;

            string strSql = " Select col_name(object_id('" + strTableName + "'),colid)  '主键字段' " +
                " From sysobjects as o Inner Join sysindexes as i On i.name=o.name   " +
                " Inner Join sysindexkeys  as k On k.indid=i.indid  Where o.xtype = 'PK' and parent_obj=object_id('" +
                strTableName + "') and k.id= object_id('" + strTableName + "') ";

            DataTable tempTable = GlobalObject.DatabaseServer.QueryInfo(strSql);

            if (tempTable != null && tempTable.Rows.Count > 0)
            {
                SystemLog_PrimaryKey lnqKey = new SystemLog_PrimaryKey();

                lnqKey.LogID = logID;
                lnqKey.PrimaryKeyName = tempTable.Rows[0][0].ToString();
                lnqKey.PrimaryKeyContent = singleKey.ToString();

                ctx.SystemLog_PrimaryKey.InsertOnSubmit(lnqKey);
            }
        }

        /// <summary>
        /// 添加操作表主键日志
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="ctx">数据上下文</param>
        /// <param name="logID">日志唯一编码</param>
        /// <param name="tObj">操作对象</param>
        void AddPrimaryKeyInfo<T>(DepotManagementDataContext ctx, Guid logID, T tObj)
        {
            string strTableName = typeof(T).Name;

            string strSql = " Select col_name(object_id('" + strTableName + "'),colid)  '主键字段' " +
                " From sysobjects as o Inner Join sysindexes as i On i.name=o.name   " +
                " Inner Join sysindexkeys  as k On k.indid=i.indid  Where o.xtype = 'PK' and parent_obj=object_id('" +
                strTableName + "') and k.id= object_id('" + strTableName + "') ";

            DataTable tempTable = GlobalObject.DatabaseServer.QueryInfo(strSql);

            if (tempTable != null && tempTable.Rows.Count > 0)
            {
                foreach (DataRow dr in tempTable.Rows)
                {
                    object keyContent = GlobalObject.GeneralFunction.GetItemValue<T>(tObj, dr[0].ToString());

                    if (keyContent == null)
                    {
                        break;
                    }

                    SystemLog_PrimaryKey lnqKey = new SystemLog_PrimaryKey();

                    lnqKey.LogID = logID;
                    lnqKey.PrimaryKeyName = dr[0].ToString();
                    lnqKey.PrimaryKeyContent = keyContent.ToString();

                    ctx.SystemLog_PrimaryKey.InsertOnSubmit(lnqKey);
                }
            }
        }

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="mode">操作模式</param>
        /// <param name="operationContent">操作对象</param>
        /// <param name="NoUpdatedContent">操作前对象</param>
        public void RecordLog<T>(CE_OperatorMode mode, T operationContent, T NoUpdatedContent)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            try
            {
                List<string> listFeildName = GlobalObject.GeneralFunction.GetFeildList<T>();
                object objOperationContent, objNoUpdatedContent;

                Guid guid = Guid.NewGuid();

                AddMainInfo<T>(ctx, guid, mode);
                AddPrimaryKeyInfo<T>(ctx, guid, NoUpdatedContent);

                foreach (string propertyName in listFeildName)
                {
                    objOperationContent = GlobalObject.GeneralFunction.GetItemValue<T>(operationContent, propertyName);
                    objNoUpdatedContent = GlobalObject.GeneralFunction.GetItemValue<T>(NoUpdatedContent, propertyName);

                    if (mode == CE_OperatorMode.修改)
                    {
                        if (objOperationContent == objNoUpdatedContent)
                        {
                            continue;
                        }
                    }

                    SystemLog_Content lnqContent = new SystemLog_Content();

                    lnqContent.FieldName = propertyName;
                    lnqContent.LogID = guid;
                    lnqContent.NoUpdatedContent = objNoUpdatedContent == null ? null : objNoUpdatedContent.ToString();
                    lnqContent.OperationContent = objOperationContent == null ? null : objOperationContent.ToString();

                    ctx.SystemLog_Content.InsertOnSubmit(lnqContent);
                }

                ctx.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="mode">操作模式</param>
        /// <param name="tObj">操作对象</param>
        /// <param name="listContent">操作内容日志列表</param>
        public void RecordLog<T>(CE_OperatorMode mode, T tObj, List<SystemLog_Content> listContent)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            try
            {
                Guid guid = Guid.NewGuid();

                AddMainInfo<T>(ctx, guid, mode);
                AddPrimaryKeyInfo<T>(ctx, guid, tObj);

                foreach (SystemLog_Content content in listContent)
                {
                    content.LogID = guid;
                    ctx.SystemLog_Content.InsertOnSubmit(content);
                }

                ctx.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="mode">操作模式</param>
        /// <param name="explainContent">操作说明</param>
        /// <param name="singlePrimaryKeyContent">单一主键值</param>
        public void RecordLog<T>(CE_OperatorMode mode, string explainContent, object singlePrimaryKeyContent)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            try
            {
                Guid guid = Guid.NewGuid();
                AddMainInfo<T>(ctx, guid, mode);
                AddPrimaryKeyInfo(ctx, guid, singlePrimaryKeyContent);

                ctx.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="mode">操作模式</param>
        /// <param name="explainContent">操作说明</param>
        public void RecordLog<T>(CE_OperatorMode mode, string explainContent)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            try
            {
                Guid guid = Guid.NewGuid();
                AddMainInfo<T>(ctx, guid, mode);

                ctx.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="explainContent">操作说明</param>
        public void RecordLog<T>(string explainContent)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            SystemLog_Main lnqMain = new SystemLog_Main();

            lnqMain.LogID = Guid.NewGuid();
            lnqMain.TableName = typeof(T).Name;
            lnqMain.OperationTime = ServerTime.Time;
            lnqMain.Operator = BasicInfo.LoginID;
            lnqMain.Explain = explainContent;

            ctx.SystemLog_Main.InsertOnSubmit(lnqMain);
            ctx.SubmitChanges();
        }

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="explainContent">操作说明</param>
        public void RecordLog(string explainContent)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            SystemLog_Main lnqMain = new SystemLog_Main();

            lnqMain.LogID = Guid.NewGuid();
            lnqMain.OperationTime = ServerTime.Time;
            lnqMain.Operator = BasicInfo.LoginID;
            lnqMain.Explain = explainContent;

            ctx.SystemLog_Main.InsertOnSubmit(lnqMain);
            ctx.SubmitChanges();
        }
    }
}

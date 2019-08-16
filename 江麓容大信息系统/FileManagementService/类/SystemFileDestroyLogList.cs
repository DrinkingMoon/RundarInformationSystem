/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  SystemFileDestroyLogList.cs
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
using DBOperate;
using PlatformManagement;
using GlobalObject;
using System.Text.RegularExpressions;
using ServerModule;

namespace Service_Quality_File
{
    public enum SystemFileDestroyLog
    {
        添加,
        修改,
        删除,
        批准,
        销毁
    }

    /// <summary>
    /// 文件销毁记录服务组件
    /// </summary>
    class SystemFileDestroyLogList : ISystemFileDestroyLogList
    {

        /// <summary>
        /// 文件基础服务组件
        /// </summary>
        ISystemFileBasicInfo m_serverBasicInfo = Service_Quality_File.ServerModuleFactory.GetServerModule<ISystemFileBasicInfo>();

        /// <summary>
        /// 获得数据集
        /// </summary>
        /// <returns>返回Table</returns>
        public DataTable GetTableInfo()
        {
            string strSql = "select * from View_FM_DestroyLogList";

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        /// <summary>
        /// 添加信息
        /// </summary>
        /// <param name="dorrList">LNQ数据集</param>
        public void Add(FM_DestroyLogList destroyList)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            FM_DestroyLogList lnqTemp = new FM_DestroyLogList();

            lnqTemp.FileID = destroyList.FileID;
            lnqTemp.CoverFile = destroyList.CoverFile;
            lnqTemp.Copies = destroyList.Copies;
            lnqTemp.DestroyWay = destroyList.DestroyWay;
            lnqTemp.Proposer = BasicInfo.LoginName;
            lnqTemp.ProposerTime = ServerTime.Time;

            ctx.FM_DestroyLogList.InsertOnSubmit(lnqTemp);

            ctx.SubmitChanges();
        }

        /// <summary>
        /// 修改信息
        /// </summary>
        /// <param name="dorrList">LNQ数据集</param>
        public void Update(FM_DestroyLogList destroyList)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.FM_DestroyLogList
                          where a.FileID == destroyList.FileID
                          select a;

            if (varData.Count() != 1)
            {
                throw new Exception("错误信息");
            }
            else
            {
                FM_DestroyLogList lnqTemp = varData.Single();

                if (lnqTemp.Approver == null && lnqTemp.ApproverTime == null)
                {
                    lnqTemp.FileID = destroyList.FileID;
                    lnqTemp.CoverFile = destroyList.CoverFile;
                    lnqTemp.Copies = destroyList.Copies;
                    lnqTemp.DestroyWay = destroyList.DestroyWay;
                    lnqTemp.Proposer = BasicInfo.LoginName;
                    lnqTemp.ProposerTime = ServerTime.Time;
                }
                else
                {
                    throw new Exception("已批准的记录不能被修改");
                }
            }

            ctx.SubmitChanges();
        }

        /// <summary>
        /// 删除信息
        /// </summary>
        /// <param name="fileID">文件ID</param>
        public void Delete(int fileID)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.FM_DestroyLogList
                          where a.FileID == fileID
                          select a;

            if (varData.Single().DestroyPersonnel == null && varData.Single().DestroyTime == null)
            {
                ctx.FM_DestroyLogList.DeleteAllOnSubmit(varData);
            }
            else
            {
                throw new Exception("已完成销毁的记录不能被删除");
            }

            ctx.SubmitChanges();
        }

        /// <summary>
        /// 批准
        /// </summary>
        /// <param name="fileID">文件ID</param>
        public void Approve(int fileID)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.FM_DestroyLogList
                          where a.FileID == fileID
                          select a;

            if (varData.Count() != 1)
            {
                throw new Exception("数据错误");
            }
            else
            {
                FM_DestroyLogList lnqTemp = varData.Single();

                if (lnqTemp.Approver == null && lnqTemp.ApproverTime == null)
                {
                    lnqTemp.Approver = BasicInfo.LoginName;
                    lnqTemp.ApproverTime = ServerTime.Time;
                }
                else
                {
                    throw new Exception("记录流程错误,请重新确认");
                }
            }

            ctx.SubmitChanges();
        }

        /// <summary>
        /// 销毁
        /// </summary>
        /// <param name="fileID">文件ID</param>
        public void Destroy(int fileID)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.FM_DestroyLogList
                          where a.FileID == fileID
                          select a;

            if (varData.Count() != 1)
            {
                throw new Exception("数据错误");
            }
            else
            {
                FM_DestroyLogList lnqTemp = varData.Single();

                if (lnqTemp.Approver != null && lnqTemp.ApproverTime != null
                    && lnqTemp.DestroyPersonnel == null && lnqTemp.DestroyTime == null)
                {
                    lnqTemp.DestroyPersonnel = BasicInfo.LoginName;
                    lnqTemp.DestroyTime = ServerTime.Time;

                    m_serverBasicInfo.OperatorFTPSystemFile(ctx, lnqTemp.FileID, 0);
                }
                else
                {
                    throw new Exception("记录流程错误,请重新确认");
                }
            }

            ctx.SubmitChanges();
        }
    }
}

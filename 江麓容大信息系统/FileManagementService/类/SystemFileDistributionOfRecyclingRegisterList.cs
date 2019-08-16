/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  SystemFileDistributionOfRecyclingRegisterList.cs
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
    public enum SystemFileDORRList
    {
        添加,
        修改,
        删除,
        签收,
        回收
    }

    /// <summary>
    /// 文件发放回收登记表服务组件
    /// </summary>
    class SystemFileDistributionOfRecyclingRegisterList:ISystemFileDistributionOfRecyclingRegisterList
    {
        /// <summary>
        /// 文件基础服务组件
        /// </summary>
        ISystemFileBasicInfo m_serverBasicInfo = Service_Quality_File.ServerModuleFactory.GetServerModule<ISystemFileBasicInfo>();

        /// <summary>
        /// 检查某业务是否存在
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="billNo">业务号</param>
        /// <returns>存在返回true</returns>
        public bool IsExist(DepotManagementDataContext ctx, string billNo)
        {
            var varData = from a in ctx.FM_DistributionOfRecyclingRegisterList
                          where a.ID == Convert.ToInt32( billNo.Substring(4))
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
            string sql = "SELECT * FROM [DepotManagement].[dbo].[FM_DistributionOfRecyclingRegisterList] where ID = '" + billNo.Substring(4) + "'";

            System.Data.DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            if (dt != null && dt.Rows.Count > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 获得数据集
        /// </summary>
        /// <returns>返回Table</returns>
        public DataTable GetTableInfo()
        {
            string strSql = "select * from View_FM_DistributionOfRecyclingRegisterList";

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        /// <summary>
        /// 添加信息
        /// </summary>
        /// <param name="dorrList">LNQ数据集</param>
        public void Add(ref FM_DistributionOfRecyclingRegisterList dorrList)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            FM_DistributionOfRecyclingRegisterList lnqTemp = new FM_DistributionOfRecyclingRegisterList();

            lnqTemp.FileID = dorrList.FileID;
            lnqTemp.GrantDepartment = dorrList.GrantDepartment;
            lnqTemp.RecoverDepartment = dorrList.RecoverDepartment;
            lnqTemp.GrantPersonnel = BasicInfo.LoginName;
            lnqTemp.GrantTime = ServerTime.Time;

            ctx.FM_DistributionOfRecyclingRegisterList.InsertOnSubmit(lnqTemp);

            ctx.SubmitChanges();

            dorrList.ID = (from a in ctx.FM_DistributionOfRecyclingRegisterList
                           select a.ID).Max();
        }

        /// <summary>
        /// 修改信息
        /// </summary>
        /// <param name="dorrList">LNQ数据集</param>
        public void Update(FM_DistributionOfRecyclingRegisterList dorrList)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.FM_DistributionOfRecyclingRegisterList
                          where a.ID == dorrList.ID
                          select a;

            if (varData.Count() != 1)
            {
                throw new Exception("错误信息");
            }
            else
            {
                FM_DistributionOfRecyclingRegisterList lnqTemp = varData.Single();

                if (lnqTemp.SignPersonnel == null && lnqTemp.SignTime == null)
                {
                    lnqTemp.GrantTime = ServerTime.Time;
                    lnqTemp.GrantPersonnel = BasicInfo.LoginName;
                    lnqTemp.GrantDepartment = dorrList.GrantDepartment;
                    lnqTemp.RecoverDepartment = dorrList.RecoverDepartment;
                    lnqTemp.FileID = dorrList.FileID;
                }
                else
                {
                    throw new Exception("已签收的记录不能被修改");
                }
            }

            ctx.SubmitChanges();
        }

        /// <summary>
        /// 删除信息
        /// </summary>
        /// <param name="id">序号</param>
        public void Delete(int id)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.FM_DistributionOfRecyclingRegisterList
                          where a.ID == id
                          select a;

            if (varData.Single().SignPersonnel == null && varData.Single().SignTime == null)
            {
                ctx.FM_DistributionOfRecyclingRegisterList.DeleteAllOnSubmit(varData);
            }
            else
            {
                throw new Exception("已签收的记录不能被删除");
            }

            ctx.SubmitChanges();
        }

        /// <summary>
        /// 签收
        /// </summary>
        /// <param name="id">序号</param>
        public void Sign(int id)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.FM_DistributionOfRecyclingRegisterList
                          where a.ID == id
                          select a;

            if (varData.Count() != 1)
            {
                throw new Exception("数据错误");
            }
            else
            {
                FM_DistributionOfRecyclingRegisterList lnqTemp = varData.Single();

                if (lnqTemp.SignPersonnel == null && lnqTemp.SignTime == null)
                {
                    lnqTemp.SignPersonnel = BasicInfo.LoginName;
                    lnqTemp.SignTime = ServerTime.Time;
                }
                else
                {
                    throw new Exception("记录流程错误,请重新确认");
                }
            }

            ctx.SubmitChanges();
        }

        /// <summary>
        /// 确认回收
        /// </summary>
        /// <param name="id">序号</param>
        public void Recover(int id)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            try
            {

                var varData = from a in ctx.FM_DistributionOfRecyclingRegisterList
                              where a.ID == id
                              select a;

                if (varData.Count() != 1)
                {
                    throw new Exception("数据错误");
                }
                else
                {
                    FM_DistributionOfRecyclingRegisterList lnqTemp = varData.Single();

                    if (lnqTemp.RecoverPersonnel == null && lnqTemp.RecoverTime == null
                        && lnqTemp.SignPersonnel != null && lnqTemp.SignTime != null)
                    {
                        lnqTemp.RecoverPersonnel = BasicInfo.LoginName;
                        lnqTemp.RecoverTime = ServerTime.Time;

                        ctx.SubmitChanges();

                        varData = from a in ctx.FM_DistributionOfRecyclingRegisterList
                                  where a.FileID == lnqTemp.FileID
                                  && a.RecoverPersonnel == null
                                  && a.RecoverTime == null
                                  select a;

                        if (varData.Count() == 0 && m_serverBasicInfo.GetFile(lnqTemp.FileID).SortID == 10)
                        {
                            m_serverBasicInfo.OperatorFTPSystemFile(ctx, lnqTemp.FileID, 11);
                        }

                        ctx.SubmitChanges();
                    }
                    else
                    {
                        throw new Exception("记录流程错误,请重新确认");
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}

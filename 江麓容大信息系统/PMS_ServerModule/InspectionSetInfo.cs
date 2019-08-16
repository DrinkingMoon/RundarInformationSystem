/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  InspectionSetInfo.cs
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
using System.Linq;
using System.Data;
using System.Text;
using System.IO;
using System.Data.Sql;
using System.Data.SqlClient;
using DBOperate;
using GlobalObject;

namespace ServerModule
{
    /// <summary>
    /// 点检服务组件
    /// </summary>
    public class InspectionSetInfo : IInspectionSetInfo
    {
        /// <summary>
        /// 根据ContentID 获得适用CVT型号
        /// </summary>
        /// <param name="contentID"></param>
        /// <returns></returns>
        public List<string> GetListForCVTType(int contentID)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            List<string> lstResult = (from a in ctx.ZPX_InspectionContentCVTType
                                      where a.ContentID == contentID
                                      select a.CVTType).ToList();
            return lstResult;
        }

        /// <summary>
        /// 获得CVT型号
        /// </summary>
        /// <returns></returns>
        public DataTable GetCVTType()
        {
            string strSql = "select ProductType as CVTType FROM P_ProductInfo WHERE (IsReturn = 0) AND (ProductType NOT LIKE '%15151%')";

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        /// <summary>
        /// 获得类型ID
        /// </summary>
        /// <param name="modeName">类型名称</param>
        /// <returns>返回类型ID</returns>
        public int GetInspectionModeID(string modeName)
        {
            DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

            var varData = from a in dataContext.ZPX_InspectionMode
                          where a.ModeName == modeName
                          select a;

            if (varData.Count() == 1)
            {
                return varData.Single().ID;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 获得一条点检内容记录
        /// </summary>
        /// <param name="inspectionContentSet">点检内容数据集</param>
        /// <returns>返回数据集</returns>
        public ZPX_InspectionContentSet GetSingleContentInfo(ZPX_InspectionContentSet inspectionContentSet)
        {
            DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

            var varData = from a in dataContext.ZPX_InspectionContentSet
                          where a.InspectionContent == inspectionContentSet.InspectionContent
                          && a.WorkBench == inspectionContentSet.WorkBench
                          select a;

            return varData.Single();
        }

        /// <summary>
        /// 获得一条点检项目记录
        /// </summary>
        /// <param name="inspectionItem">点检项目数据集</param>
        /// <returns>返回数据集</returns>
        public ZPX_InspectionItemSet GetSingleItemInfo(ZPX_InspectionItemSet inspectionItem)
        {
            DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

            var varData = from a in dataContext.ZPX_InspectionItemSet
                          where a.ContentID == inspectionItem.ContentID
                          && a.ItemName == inspectionItem.ItemName
                          select a;

            return varData.Single();
        }

        /// <summary>
        /// 获得点检设备或零件
        /// </summary>
        /// <returns>返回TABLE</returns>
        public DataTable GetContentInfo()
        {
            string strSql = "select distinct 工位,点检设备或零件,ContentID from View_ZPX_InspectionInfoSet order by 工位,点检设备或零件";

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        /// <summary>
        /// 获得对应的点检项目
        /// </summary>
        /// <param name="ContentID">点检内容ID</param>
        /// <returns>返回TABLE</returns>
        public DataTable GetItemInfo(int ContentID)
        {
            string strSql = "select 点检项目名称,点检类型,频率,最小值,最大值,ItemID,ContentID from " +
                " View_ZPX_InspectionInfoSet where ContentID = " + ContentID;

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        /// <summary>
        /// 新增点检内容
        /// </summary>
        /// <param name="inspectionContentSet">点检内容数据集</param>
        /// <param name="cvtTypeList">适用CVT型号数据列表</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool AddContent(ZPX_InspectionContentSet inspectionContentSet, List<string> cvtTypeList, out string error)
        {
            error = null;

            DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

            dataContext.Connection.Open();
            dataContext.Transaction = dataContext.Connection.BeginTransaction();

            try
            {
                var varData = from a in dataContext.ZPX_InspectionContentSet
                              where a.InspectionContent == inspectionContentSet.InspectionContent
                              && a.WorkBench == inspectionContentSet.WorkBench
                              select a;

                if (varData.Count() != 0)
                {
                    error = "数据不唯一";
                    throw new Exception(error);
                }
                else
                {
                    dataContext.ZPX_InspectionContentSet.InsertOnSubmit(inspectionContentSet);
                }

                dataContext.SubmitChanges();

                int ContentID = (from a in dataContext.ZPX_InspectionContentSet
                                 where a.InspectionContent == inspectionContentSet.InspectionContent
                                 && a.WorkBench == inspectionContentSet.WorkBench
                                 select a).Single().ID;

                foreach (string cvtType in cvtTypeList)
                {
                    ZPX_InspectionContentCVTType lnqCVTType = new ZPX_InspectionContentCVTType();

                    lnqCVTType.ContentID = ContentID;
                    lnqCVTType.CVTType = cvtType;

                    dataContext.ZPX_InspectionContentCVTType.InsertOnSubmit(lnqCVTType);
                }

                dataContext.SubmitChanges();
                dataContext.Transaction.Commit();
                return true;
            }
            catch (Exception ex)
            {
                dataContext.Transaction.Rollback();
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 新增点检项目
        /// </summary>
        /// <param name="inspectionItem">点检项目数据集</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool AddItem(ZPX_InspectionItemSet inspectionItem, out string error)
        {
            error = null;

            DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

            var varData = from a in dataContext.ZPX_InspectionItemSet
                          where a.ContentID == inspectionItem.ContentID
                          && a.ItemName == inspectionItem.ItemName
                          && a.Mode == inspectionItem.Mode
                          select a;

            if (varData.Count() != 0)
            {
                error = "数据不唯一";
                return false;
            }
            else
            {
                dataContext.ZPX_InspectionItemSet.InsertOnSubmit(inspectionItem);
            }

            dataContext.SubmitChanges();

            return true;

        }

        /// <summary>
        /// 删除点检内容
        /// </summary>
        /// <param name="ContentID">点检内容ID</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool DeleteContent(int ContentID,out string error)
        {
            error = null;

            try
            {

                DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

                var varData = from a in dataContext.ZPX_InspectionContentSet
                              where a.ID == ContentID
                              select a;

                dataContext.ZPX_InspectionContentSet.DeleteAllOnSubmit(varData);

                var varItem = from a in dataContext.ZPX_InspectionItemSet
                              where a.ContentID == ContentID
                              select a;

                dataContext.ZPX_InspectionItemSet.DeleteAllOnSubmit(varItem);

                dataContext.SubmitChanges();

                return true;

            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 删除点检项目
        /// </summary>
        /// <param name="ItemID">项目ID</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool DeleteItem(int ItemID,out string error)
        {
            error = null;

            try
            {

                DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

                var varItem = from a in dataContext.ZPX_InspectionItemSet
                              where a.ID == ItemID
                              select a;

                dataContext.ZPX_InspectionItemSet.DeleteAllOnSubmit(varItem);

                dataContext.SubmitChanges();

                if (varItem.Count() == 0)
                {
                    error = "无数据可删除";
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 修改点检项目
        /// </summary>
        /// <param name="inspectionItem">点检项目数据集</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool UpdateItem(ZPX_InspectionItemSet inspectionItem, out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

                var varData = from a in dataContext.ZPX_InspectionItemSet
                              where a.ID == inspectionItem.ID
                              select a;

                if (varData.Count() != 1)
                {
                    error = "数据为空或者不唯一";
                    return false;
                }
                else
                {
                    ZPX_InspectionItemSet lnqItem = varData.Single();

                    lnqItem.ItemName = inspectionItem.ItemName;
                    lnqItem.Mode = inspectionItem.Mode;
                    lnqItem.Rate = inspectionItem.Rate;
                    lnqItem.MaxValue = inspectionItem.MaxValue;
                    lnqItem.MinValue = inspectionItem.MinValue;
                    lnqItem.WorkID = BasicInfo.LoginID;
                    lnqItem.Date = ServerTime.Time;

                    dataContext.SubmitChanges();
                }

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 修改点检内容
        /// </summary>
        /// <param name="inspectionContentSet">点检内容数据集</param>
        /// <param name="cvtTypeList">适用CVT型号数据列表</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool UpdateContent(ZPX_InspectionContentSet inspectionContentSet, List<string> cvtTypeList, out string error)
        {
            error = null;

            DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;
            dataContext.Connection.Open();
            dataContext.Transaction = dataContext.Connection.BeginTransaction();

            try
            {

                var varData = from a in dataContext.ZPX_InspectionContentSet
                              where a.ID == inspectionContentSet.ID
                              select a;

                if (varData.Count() != 1)
                {
                    error = "数据为空或者不唯一";
                    throw new Exception(error);
                }
                else
                {
                    ZPX_InspectionContentSet lnqContent = varData.Single();

                    lnqContent.InspectionContent = inspectionContentSet.InspectionContent;
                    lnqContent.WorkBench = inspectionContentSet.WorkBench;
                    lnqContent.Date = ServerTime.Time;
                    lnqContent.WorkID = BasicInfo.LoginID;

                    dataContext.SubmitChanges();
                }

                var varDataCVT = from a in dataContext.ZPX_InspectionContentCVTType
                                 where a.ContentID == inspectionContentSet.ID
                                 select a;

                dataContext.ZPX_InspectionContentCVTType.DeleteAllOnSubmit(varDataCVT);

                foreach (string cvtType in cvtTypeList)
                {
                    ZPX_InspectionContentCVTType lnqCVTType = new ZPX_InspectionContentCVTType();

                    lnqCVTType.ContentID = inspectionContentSet.ID;
                    lnqCVTType.CVTType = cvtType;

                    dataContext.ZPX_InspectionContentCVTType.InsertOnSubmit(lnqCVTType);
                }

                dataContext.SubmitChanges();

                dataContext.Transaction.Commit();
                return true;
            }
            catch (Exception ex)
            {
                dataContext.Transaction.Rollback();
                error = ex.Message;
                return false;
            }
        }
    }
}

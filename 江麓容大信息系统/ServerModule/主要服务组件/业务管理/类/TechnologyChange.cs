/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  TechnologyChange.cs
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

namespace ServerModule
{
    /// <summary>
    /// 技术变更单管理类
    /// </summary>
    class TechnologyChange : BasicServer, ServerModule.ITechnologyChange
    {
        /// <summary>
        /// 检查某业务是否存在
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="billNo">业务号</param>
        /// <returns>存在返回true</returns>
        public bool IsExist(DepotManagementDataContext ctx, string billNo)
        {
            var varData = from a in ctx.S_TechnologyChangeBill
                          where a.DJH == billNo
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
            string sql = "SELECT * FROM [DepotManagement].[dbo].[S_TechnologyChangeBill] where DJH = '" + billNo + "'";

            System.Data.DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            if (dt != null && dt.Rows.Count > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 获得单条记录
        /// </summary>
        /// <param name="djh">单据号</param>
        /// <returns>返回获得的单据信息</returns>
        public DataRow GetBill(string djh)
        {
            string strSql = "select * from S_TechnologyChangeBill where DJH = '" + djh + "'";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

            return dt.Rows[0];
        }

        /// <summary>
        /// 获得全部单据
        /// </summary>
        /// <returns>返回获得的全部单据信息</returns>
        public DataTable GetAllBill()
        {
            string strSql = "select * from View_S_TechnologyChangeBill order by 申请日期 desc";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

            return dt;
        }

        /// <summary>
        /// 提交单据
        /// </summary>
        /// <param name="inBill">变更单信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        public bool SubmitBill(S_TechnologyChangeBill inBill, out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

                var varData = from a in dataContext.S_TechnologyChangeBill
                              where a.DJH == inBill.DJH
                              select a;

                S_TechnologyChangeBill lnqBill = null;

                if (varData.Count() > 1)
                {
                    error = "单据不唯一";
                    return false;
                }
                else if (varData.Count() == 0)
                {
                    lnqBill = new S_TechnologyChangeBill();

                    lnqBill.DJH = inBill.DJH;
                    lnqBill.DJZT = "等待批准";

                    lnqBill.ChangeMode = inBill.ChangeMode;
                    lnqBill.ChangeReason = inBill.ChangeReason;

                    lnqBill.NewParentID = inBill.NewParentID;
                    lnqBill.NewGoodsID = inBill.NewGoodsID;
                    lnqBill.NewVersion = inBill.NewVersion;
                    lnqBill.NewCounts = inBill.NewCounts;

                    lnqBill.OldParentID = inBill.OldParentID;
                    lnqBill.OldGoodsID = inBill.OldGoodsID;
                    lnqBill.OldCounts = inBill.OldCounts;
                    lnqBill.OldVersion = inBill.OldVersion;

                    lnqBill.SQRQ = ServerTime.Time;
                    lnqBill.SQRY = BasicInfo.LoginName;
                    lnqBill.FileCode = inBill.FileCode;
                    lnqBill.FileMessage = inBill.FileMessage;

                    dataContext.S_TechnologyChangeBill.InsertOnSubmit(lnqBill);
                }
                else if (varData.Count() == 1)
                {
                    lnqBill = varData.Single();
                    lnqBill.DJH = inBill.DJH;
                    lnqBill.DJZT = "等待批准";

                    lnqBill.ChangeMode = inBill.ChangeMode;
                    lnqBill.ChangeReason = inBill.ChangeReason;

                    lnqBill.NewParentID = inBill.NewParentID;
                    lnqBill.NewGoodsID = inBill.NewGoodsID;
                    lnqBill.NewVersion = inBill.NewVersion;
                    lnqBill.NewCounts = inBill.NewCounts;

                    lnqBill.OldParentID = inBill.OldParentID;
                    lnqBill.OldGoodsID = inBill.OldGoodsID;
                    lnqBill.OldCounts = inBill.OldCounts;
                    lnqBill.OldVersion = inBill.OldVersion;

                    lnqBill.SQRQ = ServerTime.Time;
                    lnqBill.SQRY = BasicInfo.LoginName;
                    lnqBill.FileCode = inBill.FileCode;
                    lnqBill.FileMessage = inBill.FileMessage;

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

        /// <summary>
        /// 更改单据状态
        /// </summary>
        /// <param name="inBill">变更单信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>更改成功返回True，更改失败返回False</returns>
        public bool UpdateBill(S_TechnologyChangeBill inBill, out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

                var varData = from a in dataContext.S_TechnologyChangeBill
                              where a.DJH == inBill.DJH
                              select a;

                if (varData.Count() != 1)
                {
                    error = "单据不唯一或者为空";
                    return false;
                }
                else
                {
                    S_TechnologyChangeBill lnqBill = varData.Single();


                    switch (inBill.DJZT)
                    {
                        case "等待批准":

                            lnqBill.PZRQ = ServerTime.Time;
                            lnqBill.PZRY = BasicInfo.LoginName;
                            lnqBill.DJZT = "单据已完成";

                            if (!UpdateBOMDate(dataContext,lnqBill,out error))
                            {
                                return false;
                            }

                            break;
                        case "新建单据":
                            lnqBill.DJZT = "新建单据";
                            break;
                        default:
                            break;
                    }

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
        /// 对BOM表中数据进行变更
        /// </summary>
        /// <param name="context">数据上下文</param>
        /// <param name="inBill">变更单信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>变更成功返回True，变更失败返回False</returns>
        public bool UpdateBOMDate(DepotManagementDataContext context, 
            S_TechnologyChangeBill inBill, out string error)
        {
            error = null;

            try
            {
                BASE_BomStruct lnqBomStruct = new BASE_BomStruct();

                BASE_BomPartsLibrary lnqLibrary = new BASE_BomPartsLibrary();

                switch (inBill.ChangeMode)
                {
                    case "新增":

                        if (inBill.NewGoodsID != null)
                        {
                            var varBomPartsLibrary = from a in context.BASE_BomPartsLibrary
                                                     where a.GoodsID == Convert.ToInt32(inBill.NewGoodsID)
                                                     select a;

                            if (varBomPartsLibrary.Count() == 0)
                            {
                                lnqLibrary.CreateDate = ServerTime.Time;
                                lnqLibrary.CreatePersonnel = UniversalFunction.GetPersonnelCode(inBill.SQRY);
                                lnqLibrary.GoodsID = Convert.ToInt32(inBill.NewGoodsID);
                                lnqLibrary.Material = "未知";
                                lnqLibrary.PartType = "产品";
                                lnqLibrary.PivotalPart = "A";
                                lnqLibrary.Remark = "由技术变更单【"+ inBill.DJH +"】生成";
                                lnqLibrary.Version = inBill.NewVersion;

                                context.BASE_BomPartsLibrary.InsertOnSubmit(lnqLibrary);
                            }
                            else if (varBomPartsLibrary.Count() == 1)
                            {
                                lnqLibrary = varBomPartsLibrary.Single();

                                lnqLibrary.Version = inBill.NewVersion;
                                lnqLibrary.CreateDate = ServerTime.Time;
                                lnqLibrary.CreatePersonnel = UniversalFunction.GetPersonnelCode(inBill.SQRY);
                            }
                        }

                        if (inBill.NewParentID != null && inBill.NewGoodsID != null)
                        {
                            lnqBomStruct.CreateDate = ServerTime.Time;
                            lnqBomStruct.CreatePersonnel = UniversalFunction.GetPersonnelCode(inBill.SQRY);
                            lnqBomStruct.GoodsID = Convert.ToInt32(inBill.NewGoodsID);
                            lnqBomStruct.ParentID = Convert.ToInt32(inBill.NewParentID);
                            lnqBomStruct.SysVersion = 1;
                            lnqBomStruct.Usage = Convert.ToDecimal(inBill.NewCounts);

                            context.BASE_BomStruct.InsertOnSubmit(lnqBomStruct);
                        }

                        break;
                    case "修改":

                        #region
                        //if (inBill.NewGoodsID != null)
                        //{
                        //    var varBomPartsLibrary = from a in context.BASE_BomPartsLibrary
                        //                             where a.GoodsID == Convert.ToInt32(inBill.NewGoodsID)
                        //                             select a;

                        //    if (varBomPartsLibrary.Count() != 1)
                        //    {
                        //        error = "新零件在零件库中不存在";
                        //        return false;
                        //    }
                        //    else
                        //    {
                        //        lnqLibrary = varBomPartsLibrary.Single();

                        //        lnqLibrary.Version = inBill.NewVersion;
                        //    }
                        //}

                        //if (inBill.OldParentID != null 
                        //    && inBill.NewParentID != null
                        //    && inBill.OldGoodsID != null
                        //    && inBill.NewGoodsID != null)
                        //{
                        //    var varBomStruct = from a in context.BASE_BomStruct
                        //                       where a.ParentID == Convert.ToInt32(inBill.OldParentID)
                        //                       && a.GoodsID == Convert.ToInt32(inBill.OldGoodsID)
                        //                       select a;

                        //    if (varBomStruct.Count() != 1)
                        //    {
                        //        error = "BOM结构表中不存在此记录";
                        //        return false;
                        //    }
                        //    else
                        //    {
                        //        lnqBomStruct = varBomStruct.Single();

                        //        lnqBomStruct.CreateDate = ServerTime.Time;
                        //        lnqBomStruct.CreatePersonnel = UniversalFunction.GetPersonnelCode(inBill.SQRY);
                        //        lnqBomStruct.GoodsID = Convert.ToInt32(inBill.NewGoodsID);
                        //        lnqBomStruct.ParentID = Convert.ToInt32(inBill.NewParentID);
                        //        lnqBomStruct.SysVersion = lnqBomStruct.SysVersion + Convert.ToDecimal( 0.01);
                        //        lnqBomStruct.Usage = Convert.ToDecimal(inBill.NewCounts);
                        //    }
                        //}
                        #endregion

                        break;
                    case "删除":

                        if (inBill.OldParentID == null)
                        {

                            var varStruct = from a in context.BASE_BomStruct
                                            where a.GoodsID == Convert.ToInt32(inBill.OldGoodsID)
                                            select a;

                            context.BASE_BomStruct.DeleteAllOnSubmit(varStruct);

                            var varLibrary = from a in context.BASE_BomPartsLibrary
                                            where a.GoodsID == Convert.ToInt32(inBill.OldGoodsID)
                                            select a;

                            context.BASE_BomPartsLibrary.DeleteAllOnSubmit(varLibrary);
                        }
                        else
                        {
                            var varStruct = from a in context.BASE_BomStruct
                                            where a.ParentID == Convert.ToInt32(inBill.OldParentID)
                                            && a.GoodsID == Convert.ToInt32(inBill.OldGoodsID)
                                            select a;

                            if (varStruct.Count() != 1)
                            {
                                error = "BOM结构表中不存在此记录";
                                return false;
                            }
                            else
                            {
                                lnqBomStruct = varStruct.Single();
                                context.BASE_BomStruct.DeleteOnSubmit(lnqBomStruct);
                            }
                        }

                        break;
                    default:
                        break;
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
        /// 获得同名称同型号同规格在BOM表中的记录
        /// </summary>
        /// <param name="code">图号型号</param>
        /// <param name="name">物品名称</param>
        /// <param name="spec">规格</param>
        /// <returns>返回获取的BOM表信息</returns>
        public DataTable GetBomInfo(string code,string name,string spec)
        {
            string strSql = "select a.Edition as 产品型号,c.ProductName as 产品名称,a.ParentCode as 父总成编号, "+
                " b.PartName as 父总成名称, a.Version as 版次号,a.Counts as 基数 from  View_P_ProductBomImitate as a inner join " +
                " (select Edition,PartCode,PartName from View_P_ProductBomImitate where AssemblyFlag = 1) as b " +
                " on a.Edition = b.Edition and a.ParentCode = b.PartCode inner join P_ProductInfo as c "+
                " on a.Edition = c.ProductCode where a.PartCode = '" + code + "' and a.PartName = '" + name + 
                "' and a.Spec = '" + spec + "' order by a.Edition,a.Counts,a.ParentCode";

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        /// <summary>
        /// 获得BOM表信息
        /// </summary>
        /// <param name="code">图号型号</param>
        /// <param name="name">物品名称</param>
        /// <param name="spec">规格</param>
        /// <param name="edition">型号</param>
        /// <returns>返回DataRow</returns>
        public DataRow GetProductInfo(string code,string name,string spec,string edition)
        {
            string strSql = "select * FROM  View_P_ProductBomImitate where PartCode = '" + code + "' and PartName = '" + name 
                +"' and Spec = '"+ spec +"' and Edition = '"+ edition +"'";

            DataTable dtTemp = GlobalObject.DatabaseServer.QueryInfo(strSql);

            if (dtTemp.Rows.Count == 0)
            {
                return null;
            }
            else
            {
                return dtTemp.Rows[0];
            }
        }

        /// <summary>
        /// 删除单据
        /// </summary>
        /// <param name="djh">单据号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>删除成功返回True，删除失败返回False</returns>
        public bool DeleteBill(string djh,out string error)
        {
            error = null;
            try
            {
                DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

                var varData = from a in dataContext.S_TechnologyChangeBill
                              where a.DJH == djh
                              select a;

                if (varData.Count() != 1)
                {
                    error = "数据不唯一或者为空";
                    return false;
                }
                else
                {

                    if (varData.Single().DJZT == "单据已完成")
                    {
                        error = "不能删除已完成的单据";
                        return false;
                    }

                    S_TechnologyChangeBill lnqbill = varData.Single();
                    //lnqbill.DJZT = "已报废";
                    dataContext.S_TechnologyChangeBill.DeleteOnSubmit(varData.Single());
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

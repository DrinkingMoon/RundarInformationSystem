/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  UnProductTestingSingle.cs
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

namespace ServerModule
{
    /// <summary>
    /// 非产品件检验单服务组件
    /// </summary>
    class UnProductTestingSingle :BasicServer, IUnProductTestingSingle
    {
        /// <summary>
        /// 检查某业务是否存在
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="billNo">业务号</param>
        /// <returns>存在返回true</returns>
        public bool IsExist(DepotManagementDataContext ctx, string billNo)
        {
            var varData = from a in ctx.ZL_UnProductTestingSingleBill
                          where a.Bill_ID == billNo
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
            string sql = "SELECT * FROM [DepotManagement].[dbo].[ZL_UnProductTestingSingleBill] where Bill_ID = '" + billNo + "'";

            System.Data.DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            if (dt != null && dt.Rows.Count > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 获得所有单据信息
        /// </summary>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="billStatus">单据状态</param>
        /// <returns>返回Table</returns>
        public DataTable GetAllInfo(DateTime startTime, DateTime endTime, string billStatus)
        {
            string strSql = "select * from View_ZL_UnProductTestingSingleBill where 申请时间 >= '" + startTime + "' and 申请时间 <= '" + endTime + "'";

            if (billStatus != "全部")
            {
                strSql += " and 单据状态 = '" + billStatus + "'";
            }

            strSql += " order by 申请时间 desc";

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        /// <summary>
        /// 获得检验单据信息
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <returns>返回LINQ数据集</returns>
        public ZL_UnProductTestingSingleBill GetInfo(string billNo)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.ZL_UnProductTestingSingleBill
                          where a.Bill_ID == billNo
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
        /// 获得检验验证记录信息
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="type">类型</param>
        /// <returns>返回Table</returns>
        public DataTable GetListInfo(string billNo,string type)
        {
            string strSql = "select InspectionItem as 检验验证项目, TechnicalRequirements as 技术要求, TestingMethod as 检验验证方法,"+
                " TestRecords as 检验验证记录 from ZL_UnProductTestingSingleList where Bill_ID = '"+ billNo +"' and Type = '"+ type +"'";

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        /// <summary>
        /// 保存检验验证记录
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="billNo">单据号</param>
        /// <param name="infoTable">检验验证记录信息集</param>
        /// <param name="type">记录类型：检验，验证</param>
        public void SaveDataTableInfo(DepotManagementDataContext ctx, string billNo, DataTable infoTable,string type)
        {   
            var varData = from a in ctx.ZL_UnProductTestingSingleList
                          where a.Bill_ID == billNo && a.Type == type
                          select a;

            ctx.ZL_UnProductTestingSingleList.DeleteAllOnSubmit(varData);

            foreach (DataRow dr in infoTable.Rows)
            {
                ZL_UnProductTestingSingleList lnqList = new ZL_UnProductTestingSingleList();

                lnqList.Bill_ID = billNo;
                lnqList.InspectionItem = dr["检验验证项目"].ToString();
                lnqList.TechnicalRequirements = dr["技术要求"].ToString();
                lnqList.TestingMethod = dr["检验验证方法"].ToString();
                lnqList.TestRecords = dr["检验验证记录"].ToString();
                lnqList.Type = type;

                ctx.ZL_UnProductTestingSingleList.InsertOnSubmit(lnqList);
            }
        }

        /// <summary>
        /// 单据流程
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool FlowBill(string billNo,out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

                var varData = from a in ctx.ZL_UnProductTestingSingleBill
                              where a.Bill_ID == billNo
                              select a;

                if (varData.Count() != 1)
                {
                    error = "单据号不存在或者重复";
                    return false;
                }

                ZL_UnProductTestingSingleBill lnqBill = varData.Single();

                switch (lnqBill.BillStatus)
                {
                    case "新建单据":

                        lnqBill.BillStatus = "等待检验要求";
                        lnqBill.Proposer = BasicInfo.LoginName;
                        lnqBill.ProposerTime = ServerTime.Time;

                        break;
                    case "等待检验要求":

                        lnqBill.BillStatus = "等待检验";
                        lnqBill.InspectorRequest = BasicInfo.LoginName;
                        lnqBill.InspectorRequestTime = ServerTime.Time;

                        break;
                    case "等待检验":

                        lnqBill.BillStatus = "等待验证要求";
                        lnqBill.Inspector = BasicInfo.LoginName;
                        lnqBill.InspectorTime = ServerTime.Time;

                        break;
                    case "等待验证要求":

                        lnqBill.BillStatus = "等待验证";
                        lnqBill.ProvingRequest = BasicInfo.LoginName;
                        lnqBill.ProvingRequestTime = ServerTime.Time;

                        break;
                    case "等待验证":

                        lnqBill.BillStatus = "等待最终判定";
                        lnqBill.Proving = BasicInfo.LoginName;
                        lnqBill.ProvingTime = ServerTime.Time;

                        break;
                    case "等待最终判定":

                        lnqBill.BillStatus = "单据已完成";
                        lnqBill.FinalJudge = BasicInfo.LoginName;
                        lnqBill.FinalJudgeTime = ServerTime.Time;

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
        /// 保存信息
        /// </summary>
        /// <param name="lnqBill">LINQ数据集</param>
        /// <param name="InspectionTable">检验记录表</param>
        /// <param name="ProvingTable">验证记录表</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool SaveInfo(ZL_UnProductTestingSingleBill lnqBill, DataTable InspectionTable,DataTable ProvingTable,out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

                if (lnqBill == null)
                {
                    error = "数据为空无法保存";
                    return false;
                }

                if (lnqBill.BillStatus == null)
                {
                    error = "单据状态为空无法判断";
                    return false;
                }

                var varData = from a in ctx.ZL_UnProductTestingSingleBill
                              where a.Bill_ID == lnqBill.Bill_ID
                              select a;

                ZL_UnProductTestingSingleBill lnqTemp = new ZL_UnProductTestingSingleBill();

                switch (lnqBill.BillStatus)
                {
                    case "新建单据":

                        if (varData.Count() > 1)
                        {
                            error = "单据号重复";
                            return false;
                        }
                        else if(varData.Count() == 0)
                        {
                            lnqTemp.Bill_ID = lnqBill.Bill_ID;
                            lnqTemp.BillStatus = lnqBill.BillStatus;
                            lnqTemp.GoodsID = lnqBill.GoodsID;
                            lnqTemp.Amount = lnqBill.Amount;
                            lnqTemp.Designer = lnqBill.Designer;
                            lnqTemp.Remark = lnqBill.Remark;

                            lnqTemp.Proposer = BasicInfo.LoginName;
                            lnqTemp.ProposerTime = ServerTime.Time;

                            ctx.ZL_UnProductTestingSingleBill.InsertOnSubmit(lnqTemp);
                        }
                        else if (varData.Count() == 1)
                        {
                            lnqTemp = varData.Single();

                            lnqTemp.GoodsID = lnqBill.GoodsID;
                            lnqTemp.Amount = lnqBill.Amount;
                            lnqTemp.Designer = lnqBill.Designer;
                            lnqTemp.Remark = lnqBill.Remark;

                            lnqTemp.Proposer = BasicInfo.LoginName;
                            lnqTemp.ProposerTime = ServerTime.Time;
                        }

                        break;
                    case "等待检验要求":

                        lnqTemp = varData.Single();

                        lnqTemp.InspectorRequest = BasicInfo.LoginName;
                        lnqTemp.InspectorRequestTime = ServerTime.Time;

                        SaveDataTableInfo(ctx, lnqBill.Bill_ID, InspectionTable, "检验");

                        break;
                    case "等待检验":
                        lnqTemp = varData.Single();

                        lnqTemp.Inspector = BasicInfo.LoginName;
                        lnqTemp.InspectorTime = ServerTime.Time;
                        lnqTemp.InspectorVerdict = lnqBill.InspectorVerdict;

                        SaveDataTableInfo(ctx, lnqBill.Bill_ID, InspectionTable, "检验");

                        break;
                    case "等待验证要求":

                        lnqTemp = varData.Single();

                        lnqTemp.ProvingRequest = BasicInfo.LoginName;
                        lnqTemp.ProvingRequestTime = ServerTime.Time;

                        SaveDataTableInfo(ctx, lnqBill.Bill_ID, ProvingTable, "验证");

                        break;
                    case "等待验证":

                        lnqTemp = varData.Single();

                        lnqTemp.Proving = BasicInfo.LoginName;
                        lnqTemp.ProvingTime = ServerTime.Time;
                        lnqTemp.ProvingVerdict = lnqBill.ProvingVerdict;

                        SaveDataTableInfo(ctx, lnqBill.Bill_ID, ProvingTable, "验证");

                        break;
                    case "等待最终判定":

                        lnqTemp = varData.Single();

                        lnqTemp.IsOK = lnqBill.IsOK;
                        lnqTemp.JudgeInfo = lnqBill.JudgeInfo;
                        lnqTemp.FinalJudge = BasicInfo.LoginName;
                        lnqTemp.FinalJudgeTime = ServerTime.Time;

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
        /// <param name="billNo">单据号</param>
        public void DeleteBill(string billNo)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.ZL_UnProductTestingSingleBill
                          where a.Bill_ID == billNo
                          select a;

            var varDataList = from a in ctx.ZL_UnProductTestingSingleList
                              where a.Bill_ID == billNo
                              select a;

            ctx.ZL_UnProductTestingSingleBill.DeleteAllOnSubmit(varData);
            ctx.ZL_UnProductTestingSingleList.DeleteAllOnSubmit(varDataList);

            ctx.SubmitChanges();
        }
    }
}

/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  MessMessageFeedback.cs
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
    /// 质量信息反馈单管理类
    /// </summary>
    class MessMessageFeedback :BasicServer, ServerModule.IMessMessageFeedback
    {
        /// <summary>
        /// 获得入库单号
        /// </summary>
        /// <returns>返回单据号</returns>
        public string GetBillID()
        {
            string strNewDJH = "";

            try
            {
                string strDJH = "FKD" + ServerTime.Time.Year.ToString();
                string strSql = "select max(substring(DJH,10,5)) from S_MessMessageFeedback where DJH like '" + strDJH + "%'";

                DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

                if (dt.Rows[0][0].ToString() != "")
                {
                    string strValue = (Convert.ToInt32(dt.Rows[0][0].ToString()) + 1).ToString("D5");
                    strNewDJH = strDJH + ServerTime.GetMouth() + strValue;
                }
                else
                {
                    strNewDJH = strDJH + ServerTime.GetMouth() + "00001";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            return strNewDJH;
        }

        /// <summary>
        /// 获得全部单据
        /// </summary>
        /// <param name="billStatus">单据状态,若为“全  部”则显示所有单据信息</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns>返回获得的单据信息</returns>
        public DataTable GetAllData(string billStatus, DateTime startTime, DateTime endTime)
        {
            string strSelect = "";

            if (billStatus != "全  部")
            {
                strSelect += "单据状态 = '" + billStatus + "' and ";
            }

            strSelect += "创建日期 >= '" + startTime + "' and 创建日期 <= '" + endTime + "'";

            string strSql = "select * from View_S_MessMessageFeedbackBill where " + strSelect + " order by 单据号 desc";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

            return dt;
        }

        /// <summary>
        /// 插入新数据
        /// </summary>
        /// <param name="inMess">反馈单信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>插入成功返回True，插入失败返回False</returns>
        public bool AddData(S_MessMessageFeedback inMess, out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

                var varData = from a in ctx.S_MessMessageFeedback
                              where a.InDepotBillID == inMess.InDepotBillID
                              select a;

                ctx.S_MessMessageFeedback.DeleteAllOnSubmit(varData);

                S_MessMessageFeedback lnqMess = new S_MessMessageFeedback();

                lnqMess.DJH = inMess.DJH;
                lnqMess.ForGoodsStatus = inMess.ForGoodsStatus;
                lnqMess.MessageFromStatus = inMess.MessageFromStatus ;
                lnqMess.DisqualificationDepict = inMess.DisqualificationDepict;
                lnqMess.QCRY = BasicInfo.LoginID;
                lnqMess.QCRQ = ServerTime.Time;
                lnqMess.DJZT = "等待STA意见";
                lnqMess.InDepotBillID = inMess.InDepotBillID;

                ctx.S_MessMessageFeedback.InsertOnSubmit(lnqMess);
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
        /// 插入记录
        /// </summary>
        /// <param name="inMessMessage">反馈单信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>插入成功返回True，插入失败返回False</returns>
        public bool InsertData(S_MessMessageFeedback inMessMessage, out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

                dataContext.S_MessMessageFeedback.InsertOnSubmit(inMessMessage);
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
        /// 更新数据库
        /// </summary>
        /// <param name="inMessMessage">反馈单信息</param>
        /// <param name="flag">更新状态</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>更新成功返回True，更新失败返回False</returns>
        public bool UpdateData(S_MessMessageFeedback inMessMessage,string flag,
            out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var varData = from a in dataContxt.S_MessMessageFeedback
                              where a.DJH == inMessMessage.DJH
                              select a;

                if (varData.Count() == 1)
                {
                    S_MessMessageFeedback lnqMess = varData.Single();

                    switch (flag)
                    {
                        case "不合格品信息":
                            lnqMess.DJH = inMessMessage.DJH;
                            lnqMess.ForGoodsStatus = inMessMessage.ForGoodsStatus;
                            lnqMess.MessageFromStatus = inMessMessage.MessageFromStatus;
                            lnqMess.DisqualificationDepict = inMessMessage.DisqualificationDepict;
                            lnqMess.QCRY = BasicInfo.LoginID;
                            lnqMess.QCRQ = ServerTime.Time;
                            lnqMess.DJZT = "等待STA意见";
                            break;
                        case "STA意见":
                            lnqMess.LinkEmail = inMessMessage.LinkEmail;
                            lnqMess.LinkMan = inMessMessage.LinkMan;
                            lnqMess.LinkPhone = inMessMessage.LinkPhone;
                            lnqMess.JLRD_LinkEmail = inMessMessage.JLRD_LinkEmail;
                            lnqMess.JLRD_LinkMan = inMessMessage.JLRD_LinkMan;
                            lnqMess.JLRD_LinkPhone = inMessMessage.JLRD_LinkPhone;
                            lnqMess.RevertTime = inMessMessage.RevertTime;
                            lnqMess.SQEMindStatus = inMessMessage.SQEMindStatus;
                            lnqMess.SQEElseMindMessage = inMessMessage.SQEElseMindMessage;
                            lnqMess.AllCount = inMessMessage.AllCount;
                            lnqMess.DefectiveRate = inMessMessage.DefectiveRate;
                            lnqMess.Picture = inMessMessage.Picture;
                            lnqMess.SQERY = BasicInfo.LoginID;
                            lnqMess.SQERQ = ServerTime.Time;
                            lnqMess.DJZT = "等待STA验证";
                            break;
                        case "STA验证":
                            lnqMess.SQEvalidateMessage = inMessMessage.SQEvalidateMessage;
                            lnqMess.SQEYZRY = BasicInfo.LoginID;
                            lnqMess.SQEYZRQ = ServerTime.Time;
                            lnqMess.DJZT = "等待质管部确认";
                            break;
                        case "质管部确认":
                            lnqMess.QEAffirmMessage = inMessMessage.QEAffirmMessage;
                            lnqMess.QEQRRY = BasicInfo.LoginID;
                            lnqMess.QEQRRQ = ServerTime.Time;
                            lnqMess.DJZT = "单据已完成";
                            break;
                        default:
                            break;
                    }

                    dataContxt.SubmitChanges();
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
        /// 获得单条记录信息
        /// </summary>
        /// <param name="djh">单据号</param>
        /// <returns>返回获取到的单据记录信息</returns>
        public S_MessMessageFeedback GetData(string djh)
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            return (from a in dataContxt.S_MessMessageFeedback
                   where a.DJH == djh
                   select a).Single();
        }

        /// <summary>
        /// 获得报检单中的信息
        /// </summary>
        /// <param name="djh">单据号</param>
        /// <returns>返回获得报检单的信息</returns>
        public View_S_CheckOutInDepotBill GetCheckInDepotBill(string djh)
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            if (djh.Substring(0,3).ToString() != "BJD"
                && djh.Substring(0, 3).ToString() != "HJD"
                && djh.Substring(0, 3).ToString() != "FJD")
            {
                string strSql = "select * from View_S_CheckOutInDepotBill as a inner join "+
                    " S_IsolationManageBill as b on a.物品ID =  b.GoodsID and a.批次号 = "+
                    " b.BatchNo where b.DJH = '"+ djh +"'";

                DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

                if (dt.Rows.Count > 0 )
                {
                    return (from a in dataContxt.View_S_CheckOutInDepotBill
                            where a.入库单号 == dt.Rows[0]["入库单号"].ToString()
                            select a).Single();
                }
                else
                {
                    strSql = "select e.物品名称 as GoodsName,e.图号型号 as GoodsCode,e.规格 as Spec,e.物品类别 as GoodsType,b.BatchNo, "+
                        " d.Date,d.Provider,b.GoodsID,d.StorageID from S_MessMessageFeedback as a " +
                        " inner join S_IsolationManageBill as b on a.IndepotBillID = b.DJH "+
                        " inner join S_Stock as d on b.GoodsID = d.GoodsID and b.BatchNo = d.BatchNo "+
                        " inner join View_F_GoodsPlanCost as e on e.序号 = b.GoodsID where b.DJH = '"+ djh +"'";

                    dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

                    View_S_CheckOutInDepotBill lnqCheck = new View_S_CheckOutInDepotBill();

                    lnqCheck.图号型号 = dt.Rows[0]["GoodsCode"].ToString();
                    lnqCheck.物品名称 = dt.Rows[0]["GoodsName"].ToString();
                    lnqCheck.规格 = dt.Rows[0]["Spec"].ToString();
                    lnqCheck.批次号 = dt.Rows[0]["BatchNo"].ToString();
                    lnqCheck.仓库 = dt.Rows[0]["GoodsType"].ToString();
                    lnqCheck.到货日期 = (DateTime)dt.Rows[0]["Date"];
                    lnqCheck.订单号 = "";
                    lnqCheck.供货单位 = dt.Rows[0]["Provider"].ToString();
                    lnqCheck.物品ID = Convert.ToInt32(dt.Rows[0]["GoodsID"]);
                    lnqCheck.库房代码 = dt.Rows[0]["StorageID"].ToString();

                    return lnqCheck;
                }
            }
            else
            {
                return (from a in dataContxt.View_S_CheckOutInDepotBill
                        where a.入库单号 == djh
                        select a).Single();
            }
        }

        /// <summary>
        /// 报废单据
        /// </summary>
        /// <param name="djh">单据号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>报废单据成功返回True，报废单据失败返回False</returns>
        public bool ScarpData(string djh,out string error)
        {
            error = null;

            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            try
            {
                var varData = from a in dataContxt.S_MessMessageFeedback
                              where a.DJH == djh
                              select a;

                if (varData.Count() == 1)
                {
                    S_MessMessageFeedback lnqMess = varData.Single();

                    lnqMess.DJZT = "单据已报废";

                    dataContxt.SubmitChanges();
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
        /// 获得最近的单据的联系人信息
        /// </summary>
        /// <param name="providerName">供应商名称</param>
        /// <returns>返回最近单据的联系人信息</returns>
        public DataTable GetNearestLinkManInfo(string providerName)
        {
            string strSql = "select * from   S_MessMessageFeedback where ID = (select Max(ID) from " +
                " (select a.ID,c.ProviderName,LinkMan from S_MessMessageFeedback as a " +
                " inner join View_S_CheckOutInDepotBill as b on a.IndepotBillID = b.入库单号 " +
                " inner join Provider as c on c.ProviderCode = b.供货单位 " +
                " union all select a.ID,c.ProviderName,LinkMan from S_MessMessageFeedback as a  " +
                " inner join S_IsolationManageBill as b on a.IndepotBillID = b.DJH " +
                " inner join S_Stock as d on b.GoodsID = d.GoodsID and b.BatchNo = d.BatchNo " +
                " inner join Provider as c on c.ProviderCode = d.Provider) as a  "+
                " where ProviderName = '" + providerName + "'  and LinkMan is not null)";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

            return dt;
        }

        /// <summary>
        /// 回退单据
        /// </summary>
        /// <param name="djh">单据号</param>
        /// <param name="flag">回退状态</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <param name="rebackReason">回退原因</param>
        /// <returns>回退成功返回True，回退失败返回False</returns>
        public bool ReturnData(string djh, string flag, out string error, string rebackReason)
        {
            error = null;

            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            try
            {
                var varData = from a in dataContxt.S_MessMessageFeedback
                              where a.DJH == djh
                              select a;

                if (varData.Count() == 1)
                {
                    S_MessMessageFeedback lnqMess = varData.Single();

                    switch (flag)
                    {
                        case "等待QC质量信息提交":
                            lnqMess.DJZT = "等待QC质量信息提交";
                            lnqMess.SQERQ = null;
                            lnqMess.SQERY = null;
                            lnqMess.SQEYZRQ = null;
                            lnqMess.SQEYZRY = null;
                            lnqMess.QEQRRQ = null;
                            lnqMess.QEQRRY = null;
                            lnqMess.SQEvalidateMessage = null;
                            lnqMess.SQEMindStatus = null;
                            lnqMess.SQEElseMindMessage = null;
                            lnqMess.ForGoodsStatus = null;
                            lnqMess.MessageFromStatus = null;
                            lnqMess.Picture = null;
                            lnqMess.DisqualificationDepict = null;
                            break;
                        case "等待STA意见":
                            lnqMess.DJZT = "等待STA意见";
                            lnqMess.SQEYZRQ = null;
                            lnqMess.SQEYZRY = null;
                            lnqMess.QEQRRQ = null;
                            lnqMess.QEQRRY = null;
                            lnqMess.SQEvalidateMessage = null;
                            lnqMess.AllCount = null;
                            lnqMess.DefectiveRate = null;
                            lnqMess.Picture = null;
                            lnqMess.SQEMindStatus = null;
                            lnqMess.SQEElseMindMessage = null;
                            break;
                        case "等待质管部意见":
                            lnqMess.DJZT = "等待质管部意见";
                            lnqMess.SQEYZRQ = null;
                            lnqMess.SQEYZRY = null;
                            lnqMess.QEQRRQ = null;
                            lnqMess.QEQRRY = null;
                            lnqMess.SQEvalidateMessage = null;
                            break;
                        case "等待STA验证":
                            lnqMess.DJZT = "等待STA验证";
                            lnqMess.QEQRRQ = null;
                            lnqMess.QEQRRY = null;
                            lnqMess.SQEvalidateMessage = null;
                            break;
                        default:
                            break;
                    }

                    dataContxt.SubmitChanges();
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
        /// 获得本批总数
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <param name="batchNo">批次号</param>
        /// <param name="storageID">库房ID</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回本批总数信息</returns>
        public decimal GetAllCount(int goodsID,string batchNo,string storageID,
            out string error)
        {
            error = null;

            try
            {
                //报检入库
                string strSql = "select * from View_S_CheckOutInDepotBill where 物品ID = "
                    + goodsID + " and 批次号 = '" + batchNo + "' and StorageID = '"+ storageID +"'";

                DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

                if (dt != null && dt.Rows.Count != 0)
                {
                    return Convert.ToDecimal(dt.Rows[0]["入库数量"]);
                }
                else if (dt.Rows.Count > 1)
                {
                    error = "报检入库单不唯一";
                }


                //自制件入库
                strSql = "select * from View_S_HomemadePartBill where 物品ID = " 
                    + goodsID + " and 批次号 = '" + batchNo + "' and StorageID = '"+ storageID +"'";

                dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

                if (dt != null && dt.Rows.Count != 0)
                {
                    return Convert.ToDecimal(dt.Rows[0]["入库数量"]);
                }
                else if (dt.Rows.Count > 1)
                {
                    error = "自制件入库单不唯一";
                }

                return 0;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return 0;
            }
        }
    }
}

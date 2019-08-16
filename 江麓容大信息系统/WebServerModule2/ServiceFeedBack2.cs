using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using GlobalObject;
using ServerModule;
using PlatformManagement;
using Service_Peripheral_HR;
using Service_Peripheral_External;
using DBOperate;

namespace WebServerModule2
{
    /// <summary>
    /// 售后信息服务类
    /// </summary>
    class ServiceFeedBack2 : WebServerModule.ServiceFeedBack, WebServerModule2.IServiceFeedBack2
    {
        /// <summary>
        /// 单据类消息发布器
        /// </summary>
        IBillMessagePromulgatorServer m_billMessageServer = BasicServerFactory.GetServerModule<IBillMessagePromulgatorServer>();

        /// <summary>
        /// 调运单服务类
        /// </summary>
        IManeuverServer m_maneuverServer = Service_Peripheral_External.ServerModuleFactory.GetServerModule<IManeuverServer>();

        #region 获取单号

        /// <summary>
        /// 对小于10的月份 进行加0
        /// </summary>
        /// <returns>成功返回正确的字符串，失败返回null</returns>
        private string GetMouth()
        {
            if (DateTime.Now.Month < 10)
            {
                return "0" + DateTime.Now.Month.ToString();
            }
            else
            {
                return DateTime.Now.Month.ToString();
            }
        }

        /// <summary>
        /// 获得单据状态
        /// </summary>
        /// <param name="bill_ID">单据号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        private bool GetBillStatusTable(string bill_ID, out string error)
        {
            error = "";

            WebSiteDataClassesDataContext dataContxt = CommentParameter2.WebDataContext;

            var varData = from a in dataContxt.OF_BillID_Status
                          where a.Bill_ID == bill_ID
                          select a;

            if (varData.Count() != 0)
            {
                if (varData.Single().Bill_ID == bill_ID && varData.Single().UseStatus == false)
                {
                    OF_BillID_Status status = varData.Single();

                    status.UseStatus = true;
                    dataContxt.SubmitChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                OF_BillID_Status bill = new OF_BillID_Status();

                bill.Bill_ID = bill_ID;
                bill.UseStatus = true;

                dataContxt.OF_BillID_Status.InsertOnSubmit(bill);
                dataContxt.SubmitChanges();

                return true;
            }
        }

        /// <summary>
        /// 删除没有使用的单据
        /// </summary>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        public bool DeleteBillStatus()
        {
            string sql = @"select Bill_ID from OF_BillID_Status where Bill_ID like 'SHFK%'";

            DataTable dt = GlobalObject.DatabaseServer.WebQueryInfo(sql);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (GetTableID(dt.Rows[i]["Bill_ID"].ToString()).Rows.Count == 0)
                {
                    WebSiteDataClassesDataContext dataContxt = CommentParameter2.WebDataContext;

                    var varData = from a in dataContxt.OF_BillID_Status
                                  where a.Bill_ID == dt.Rows[i]["Bill_ID"].ToString()
                                  select a;

                    dataContxt.OF_BillID_Status.DeleteAllOnSubmit(varData);
                    dataContxt.SubmitChanges();
                }
            }

            return true;
        }

        /// <summary>
        /// 判断单据是否存在
        /// </summary>
        /// <param name="bill_ID">单据号</param>
        /// <returns>成功返回满足条件的数据集，失败返回空的数据集</returns>
        private DataTable GetTableID(string bill_ID)
        {
            string sql = @"select FK_Bill_ID from S_ServiceFeedBack where FK_Bill_ID='" + bill_ID + "'";

            DataTable dt = GlobalObject.DatabaseServer.WebQueryInfo(sql);

            return dt;
        }
        #endregion

        /// <summary>
        /// 获取新的单据号
        /// </summary>
        /// <param name="checkOutGoodsType">物品类别</param>
        /// <returns>成功返回单号，失败抛出异常</returns>
        public string GetNextBillID(int checkOutGoodsType)
        {
            WebSiteDataClassesDataContext dataContxt = CommentParameter2.WebDataContext;

            long maxValue = 1;
            string prefix = "";

            DateTime dt = ServerTime.Time;

            switch (checkOutGoodsType)
            {
                case 1:
                    prefix = "SH";
                    break;
                case 2:
                    prefix = "SHFK";
                    break;
            }

            if (checkOutGoodsType == 1)
            {
                if (dataContxt.S_AfterService.Count() > 0)
                {
                    var result = from c in dataContxt.S_AfterService
                                 where c.ServiceID.Substring(2, 4) == dt.Year.ToString() //.Bill_Time.Year == dt.Year
                                 select c;

                    if (result.Count() > 0)
                    {
                        maxValue += (from c in result select Convert.ToInt32(c.ServiceID.Substring(8))).Max();
                    }
                }

                return string.Format("{0}{1:D4}{2:D2}{3:D4}", prefix, dt.Year, dt.Month, maxValue);
            }
            else
            {
                if (dataContxt.S_ServiceFeedBack.Count() > 0)
                {
                    var result = from c in dataContxt.S_ServiceFeedBack
                                 where c.FK_Bill_ID.Substring(4, 4) == dt.Year.ToString() //.Bill_Time.Year == dt.Year
                                 select c;

                    if (result.Count() > 0)
                    {
                        maxValue += (from c in result select Convert.ToInt32(c.FK_Bill_ID.Substring(10))).Max();
                    }
                }

                return string.Format("{0}{1:D4}{2:D2}{3:D4}", prefix, dt.Year, dt.Month, maxValue);
            }
        }

        /// <summary>
        /// 根据单据号查询反馈信息
        /// </summary>
        /// <param name="bill_ID">单据号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>成功返回满足条件的数据集，失败返回错误信息</returns>
        public DataTable GetTableByBill(string bill_ID, out string error)
        {
            error = "";

            string sql = @"select * from S_ServiceFeedBack where FK_Bill_ID='" + bill_ID + "'";

            DataTable dt = GlobalObject.DatabaseServer.WebQueryInfo(sql);

            return dt;
        }

        /// <summary>
        /// 通过删除反馈信息
        /// </summary>
        /// <param name="feedBack">数据集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        public bool DeleteFeedBack(S_ServiceFeedBack feedBack, out string error)
        {
            error = "";

            try
            {
                WebSiteDataClassesDataContext dataContxt = CommentParameter2.WebDataContext;

                var varData = from a in dataContxt.S_ServiceFeedBack
                              where a.FK_Bill_ID == feedBack.FK_Bill_ID
                              select a;

                if (varData.Count() != 1)
                {
                    error = "单据信息有误！";

                    return false;
                }
                else
                {
                    dataContxt.S_ServiceFeedBack.DeleteAllOnSubmit(varData);
                    dataContxt.SubmitChanges();

                    return true;
                }
            }
            catch
            {
                error = "数据有误！";

                return false;
            }
        }

        /// <summary>
        /// 添加反馈信息
        /// </summary>
        /// <param name="feedBack">反馈的数据集</param>
        /// <param name="messageInfo">故障信息数据集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        public bool InsertFeedBack(S_ServiceFeedBack feedBack, OF_BugMessageInfo messageInfo, out string error)
        {
            error = "";

            try
            {
                WebSiteDataClassesDataContext dataContxt = CommentParameter2.WebDataContext;

                var varData = from a in dataContxt.S_ServiceFeedBack
                              where a.FK_Bill_ID == feedBack.FK_Bill_ID
                              select a;

                if (varData.Count() != 0)
                {
                    if (!DeleteFeedBack(feedBack, out error))
                    {
                        return false;
                    }
                }
                else
                {
                    var varData2 = from a in dataContxt.S_ServiceFeedBack
                                   where a.ServiceID == feedBack.ServiceID
                                   select a;

                    if (varData2.Count() > 0)
                    {
                        error = "【" + feedBack.ServiceID + "】函电的反馈信息已经提交，此操作失败！";
                        return false;
                    }
                }

                S_ServiceFeedBack list = new S_ServiceFeedBack();

                list.FK_Bill_ID = feedBack.FK_Bill_ID;
                list.ServiceID = feedBack.ServiceID;
                list.MessageSource = feedBack.MessageSource;
                list.SiteName = feedBack.SiteName;
                list.CarModel = feedBack.CarModel;
                list.CVTCode = feedBack.CVTCode;
                list.CVTID = feedBack.CVTID;
                list.ChassisNum = feedBack.ChassisNum;
                list.TCUCode = feedBack.TCUCode;
                list.NewSoftware = feedBack.NewSoftware;
                list.BugCode = feedBack.BugCode;
                list.CVTCondition = feedBack.CVTCondition;
                list.UserName = feedBack.UserName;
                list.LinkTel = feedBack.LinkTel;
                list.Linkman = feedBack.UserName;
                list.BugNumber = feedBack.BugNumber;
                list.BatchNumber = feedBack.BatchNumber;
                list.BuyCarTime = feedBack.BuyCarTime;
                list.RunMileage = feedBack.RunMileage;
                list.ProcessName = feedBack.ProcessName;
                list.ProcessMode = feedBack.ProcessMode;
                list.ProcessTime = feedBack.ProcessTime;
                list.BugCode = feedBack.BugCode;
                list.DiagnoseSituation = GetBugMessageByServiceID(list.ServiceID).Rows[0]["ID"].ToString();
                list.Solution = feedBack.Solution;
                list.IsBack = feedBack.IsBack;
                list.Status = "等待确认返回时间";

                dataContxt.S_ServiceFeedBack.InsertOnSubmit(list);
                dataContxt.SubmitChanges();
            }
            catch (Exception ex)
            {
                error = ex.Message;

                return false;
            }

            return true;
        }

        /// <summary>
        /// 确认返回件返回时间
        /// </summary>
        /// <param name="feedBackID">反馈单编号</param>
        /// <param name="cvtCode">变速箱型号</param>
        /// <param name="cvtID">变速箱编号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        public bool UpdateFeedBackTime(string feedBackID, string cvtID, string cvtCode, out string error)
        {
            error = "";

            try
            {
                WebSiteDataClassesDataContext dataContxt = CommentParameter2.WebDataContext;

                var varData = from a in dataContxt.S_ServiceFeedBack
                              where a.FK_Bill_ID == feedBackID
                              select a;

                if (varData.Count() > 0)
                {
                    S_ServiceFeedBack list = varData.Single();

                    list.CVTCode = cvtCode;
                    list.CVTID = cvtID;
                    list.Status = "等待主管审核";

                    if (cvtID.ToString() != "")
                    {
                        list.CVTID = cvtID;

                        var varService = from a in dataContxt.S_AfterService
                                         where a.FKBillID == feedBackID
                                         select a;

                        S_AfterService service = varService.Single();
                        service.CVTID = cvtID;
                        service.CVTCode = cvtCode;
                    }

                    dataContxt.SubmitChanges();

                    m_billMessageServer.BillType = "售后服务质量反馈单";
                    m_billMessageServer.PassFlowMessage(feedBackID,
                        string.Format("【车型】：{0} 【CVT型号】: {1} 【CVT箱号】：{2} 【车架号】：{3}   ※※※ 等待【{4}】处理",
                        list.CarModel, list.CVTCode, list.CVTID, list.ChassisNum, CE_RoleEnum.营销主管.ToString()),
                        CE_RoleEnum.营销主管.ToString(), true);
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;

                return false;
            }

            return true;
        }

        /// <summary>
        /// 添加故障信息
        /// </summary>
        /// <param name="messageInfo">实体对象</param>
        /// <param name="serviceID">函电单号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>成功返回True；失败返回False</returns>
        public bool InsertBugMessage(OF_BugMessageInfo messageInfo, string serviceID, out string error)
        {
            error = "";

            try
            {
                WebSiteDataClassesDataContext dataContxt = CommentParameter2.WebDataContext;

                var varData = from a in dataContxt.OF_BugMessageInfo
                              where a.ServiceID == serviceID
                              select a;

                if (varData.Count() == 0)
                {
                    OF_BugMessageInfo LnqBugInfo = new OF_BugMessageInfo();

                    LnqBugInfo.ServiceID = serviceID;
                    LnqBugInfo.BugCode = messageInfo.BugCode;
                    LnqBugInfo.CarMainBug = messageInfo.CarMainBug;
                    LnqBugInfo.CarSecendBug = messageInfo.CarSecendBug;
                    LnqBugInfo.Frequency = messageInfo.Frequency;
                    LnqBugInfo.Condition = messageInfo.Condition;
                    LnqBugInfo.BugDeclare = messageInfo.BugDeclare;
                    LnqBugInfo.CVTOilDetection = messageInfo.CVTOilDetection;
                    LnqBugInfo.PressureSensor = messageInfo.PressureSensor;
                    LnqBugInfo.ActiveSensor = messageInfo.ActiveSensor;
                    LnqBugInfo.PassivitySensor = messageInfo.PassivitySensor;
                    LnqBugInfo.ShiftKnob = messageInfo.ShiftKnob;
                    LnqBugInfo.OverLinkStatus = messageInfo.OverLinkStatus;
                    LnqBugInfo.OilSump = messageInfo.OilSump;
                    LnqBugInfo.PKey = messageInfo.PKey;
                    LnqBugInfo.RKey = messageInfo.RKey;
                    LnqBugInfo.NKey = messageInfo.NKey;
                    LnqBugInfo.DKey = messageInfo.DKey;
                    LnqBugInfo.SKey = messageInfo.SKey;

                    dataContxt.OF_BugMessageInfo.InsertOnSubmit(LnqBugInfo);
                    dataContxt.SubmitChanges();

                    return true;
                }
                else
                {
                    OF_BugMessageInfo LnqBugInfo = varData.Single();

                    LnqBugInfo.BugCode = messageInfo.BugCode;
                    LnqBugInfo.CarMainBug = messageInfo.CarMainBug;
                    LnqBugInfo.CarSecendBug = messageInfo.CarSecendBug;
                    LnqBugInfo.Frequency = messageInfo.Frequency;
                    LnqBugInfo.Condition = messageInfo.Condition;
                    LnqBugInfo.BugDeclare = messageInfo.BugDeclare;
                    LnqBugInfo.CVTOilDetection = messageInfo.CVTOilDetection;
                    LnqBugInfo.PressureSensor = messageInfo.PressureSensor;
                    LnqBugInfo.ActiveSensor = messageInfo.ActiveSensor;
                    LnqBugInfo.PassivitySensor = messageInfo.PassivitySensor;
                    LnqBugInfo.ShiftKnob = messageInfo.ShiftKnob;
                    LnqBugInfo.OverLinkStatus = messageInfo.OverLinkStatus;
                    LnqBugInfo.OilSump = messageInfo.OilSump;
                    LnqBugInfo.PKey = messageInfo.PKey;
                    LnqBugInfo.RKey = messageInfo.RKey;
                    LnqBugInfo.NKey = messageInfo.NKey;
                    LnqBugInfo.DKey = messageInfo.DKey;
                    LnqBugInfo.SKey = messageInfo.SKey;

                    dataContxt.SubmitChanges();

                    return true;
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;

                return false;
            }
        }

        /// <summary>
        /// 营销主管审核，创建调运单或营销退货单
        /// </summary>
        /// <param name="dtProductCodes">物品信息</param>
        /// <param name="dtMxCK">营销退库单主表信息</param>
        /// <param name="row">营销退库单子表信息</param>
        /// <param name="type">类型</param>
        /// <param name="maneuverBill">调运单主表信息</param>
        /// <param name="dtManeuverList">调运单子表信息</param>
        /// <param name="backID">反馈单号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>成功返回True失返回False</returns>
        public bool UpdateYXCheckCreateManeuverBill(DataTable dtProductCodes, DataTable dtMxCK, DataRow row, string type, 
            Out_ManeuverBill maneuverBill, DataTable dtManeuverList,string backID,out string error)
        {
            error = "";

            try
            {
                WebSiteDataClassesDataContext dataContxt = CommentParameter2.WebDataContext;

                if (dtMxCK != null)
                {
                    if (dtProductCodes == null)
                    {
                        if (!ServerModule.ServerModuleFactory.GetServerModule<ISellIn>().UpdateBill(
                            dtMxCK, row, CE_MarketingType.退库.ToString(), out error))
                        {
                            return false;
                        }
                    }
                    else
                    {
                        if (ServerModule.ServerModuleFactory.GetServerModule<IProductCodeServer>().UpdateProducts(
                        dtProductCodes, dtProductCodes.Rows[0]["GoodsCode"].ToString(), "",
                        Convert.ToInt32(dtProductCodes.Rows[0]["GoodsID"].ToString()), backID, out error))
                        {
                            if (!ServerModule.ServerModuleFactory.GetServerModule<ISellIn>().UpdateBill(
                                dtMxCK, row, CE_MarketingType.退库.ToString(), out error))
                            {
                                return false;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                }

                dataContxt.SubmitChanges();
                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;

                return false;
            }
        }

        /// <summary>
        /// 营销主管审核，更新件与返回件不同的情况下创建领料单和领料退库单、修改Out_Stoct的GoodsID
        /// </summary>
        /// <param name="m_inTheDepotBill">领料退库主信息</param>
        /// <param name="m_inTheDepotGoods">领料退库明细</param>
        /// <param name="m_requisitionBill">领料单主信息</param>
        /// <param name="m_lnqGoods">领料单明细</param>
        /// <param name="lnqList">产品编号表信息</param>
        /// <param name="maneuverBillInfo">调运单主信息</param>
        /// <param name="dtManeuverList">调运单明细</param>
        /// <param name="ClientCode">服务站编号</param>
        /// <param name="m_strErr">错误信息</param>
        /// <returns>操作成功返回True，失败返回False</returns>
        public bool UpdateYXCheckCreateManeuverBill(S_MaterialReturnedInTheDepot m_inTheDepotBill, S_MaterialListReturnedInTheDepot m_inTheDepotGoods, 
            S_MaterialRequisition m_requisitionBill, S_MaterialRequisitionGoods m_lnqGoods, ProductsCodes lnqList, Out_ManeuverBill maneuverBillInfo, 
            DataTable dtManeuverList,string ClientCode, out string m_strErr)
        {
            m_strErr = "";

            WebSiteDataClassesDataContext dataContxt = CommentParameter2.WebDataContext;
            DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

            dataContxt.Connection.Open();
            dataContxt.Transaction = dataContxt.Connection.BeginTransaction();

            try
            {
                DBOperate.IDBOperate dbOperate = CommentParameter.GetDBOperatorOfDepotManagement();

                string sql = "alter table S_Stock disable trigger Update_BackUp";
                Dictionary<OperateCMD, object> dic = dbOperate.RunProc(sql);

                string sql2 = "alter table Out_Stock disable trigger Update_Out_Stock";
                Dictionary<OperateCMD, object> dic2 = dbOperate.RunProc(sql2);

                var varData_Del = from a in dataContext.ProductsCodes
                                  where a.DJH == m_requisitionBill.Bill_ID
                                        && a.GoodsID == m_lnqGoods.GoodsID
                                        && a.ProductCode == lnqList.ProductCode
                                  select a;

                if (varData_Del.Count() == 0)
                {
                    dataContext.ProductsCodes.InsertOnSubmit(lnqList);
                    dataContext.SubmitChanges();

                    var varRequisition = from a in dataContext.S_MaterialRequisition
                                         where a.AssociatedBillNo == m_requisitionBill.AssociatedBillNo
                                         select a;

                    if (varRequisition.Count() == 0)
                    {
                        dataContext.S_MaterialRequisition.InsertOnSubmit(m_requisitionBill);
                        dataContext.SubmitChanges();
                    }
                    else
                    {
                        m_lnqGoods.Bill_ID = varRequisition.First().Bill_ID;
                    }

                    dataContext.S_MaterialRequisitionGoods.InsertOnSubmit(m_lnqGoods);                    

                    if (dic.ContainsKey(OperateCMD.Return_OperateResult) && dic2.ContainsKey(OperateCMD.Return_OperateResult))
                    {
                        var resultStoct = from r in dataContext.S_Stock
                                          where r.GoodsID == m_lnqGoods.GoodsID && r.StorageID == m_requisitionBill.StorageID
                                          select r;

                        S_Stock stockRecord = resultStoct.Single();

                        if (stockRecord.ExistCount < 1)
                        {
                            m_strErr = "仓库中的物品信息的存货量达不到领取的数目, 请重新确定后再进行此操作";

                            return false;
                        }

                        stockRecord.ExistCount -= 1;
                    }

                    dataContext.SubmitChanges();

                    dataContext.S_MaterialReturnedInTheDepot.InsertOnSubmit(m_inTheDepotBill);
                    dataContext.SubmitChanges();

                    dataContext.S_MaterialListReturnedInTheDepot.InsertOnSubmit(m_inTheDepotGoods);

                    var varStoct = from r in dataContext.S_Stock
                                   where r.GoodsID == m_inTheDepotGoods.GoodsID && r.StorageID == m_inTheDepotBill.StorageID
                                   select r;

                    S_Stock stoctIn = varStoct.Single();

                    stoctIn.ExistCount += 1;

                    dataContxt.SubmitChanges();

                    var relOutStoct = from a in dataContext.Out_Stock
                                   where a.SecStorageID == ClientCode
                                   && a.GoodsID == m_lnqGoods.GoodsID
                                   select a;

                    if (relOutStoct.Count() > 0)
                    {
                        Out_Stock outStoct = relOutStoct.Single();

                        outStoct.GoodsID = m_inTheDepotGoods.GoodsID;
                    }

                    dataContxt.SubmitChanges(); 
                    dataContext.Transaction.Commit();

                    string sql3 = "alter table S_Stock enable trigger Update_BackUp";
                    Dictionary<OperateCMD, object> dic3 = dbOperate.RunProc(sql3);

                    string sql4 = "alter table Out_Stock enable trigger Update_Out_Stock";
                    Dictionary<OperateCMD, object> dic4 = dbOperate.RunProc(sql4);
                }
                else
                {
                    m_strErr = "更新件的产品编码错误！";
                    dataContxt.Transaction.Rollback();
                    return false;
                }
                
                return true;
            }
            catch (Exception ex)
            {
                dataContxt.Transaction.Rollback();
                m_strErr = ex.Message;
                return false;
            }
        }

        /// <summary>
        ///  营销主管审核，修改表信息
        /// </summary>
        /// <param name="backID">反馈单号</param>
        /// <param name="strDJH">单据号</param>
        /// <param name="chargeSuggestionYX">主管意见</param>
        /// <param name="TKFS">退库方式</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>成功返回True失返回False</returns>
        public bool UpdateYXCheck(string strDJH, string chargeSuggestionYX, string backID, string TKFS, out string error)
        {
             error = "";

            try
            {
                WebSiteDataClassesDataContext dataContxt = CommentParameter2.WebDataContext;

                var varData = from a in dataContxt.S_ServiceFeedBack
                              where a.FK_Bill_ID == strDJH
                              select a;

                if (varData.Count() != 1)
                {
                    error = "数据有错误，不能进行此操作!";
                    return false;
                }
                else
                {
                    S_ServiceFeedBack list = varData.Single();

                    list.YXChargeSuggestion = chargeSuggestionYX;
                    list.BugCode = backID;
                    list.Status = "等待质管确认";
                    list.YXSignatureDate = ServerTime.Time;
                    list.YXSignature = BasicInfo.LoginID;
                    list.IsBack = TKFS;

                    dataContxt.SubmitChanges();

                    return true;
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;

                return false;
            }
        }

        /// <summary>
        /// 责任人确认，修改表信息
        /// </summary>
        /// <param name="back">数据集对象</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        public bool UpdateDutyPerson(S_ServiceFeedBack back, out string error)
        {
            error = "";

            try
            {
                WebSiteDataClassesDataContext dataContxt = CommentParameter2.WebDataContext;

                var varData = from a in dataContxt.S_ServiceFeedBack
                              where a.FK_Bill_ID == back.FK_Bill_ID
                              select a;

                if (varData.Count() != 1)
                {
                    error = "数据有错误！";
                    return false;
                }
                else
                {
                    S_ServiceFeedBack list = varData.Single();

                    list.Temporary = back.Temporary;
                    list.Analyse = back.Analyse;
                    list.foreverImplement = back.foreverImplement;
                    list.IsFMEAfile = back.IsFMEAfile;
                    list.IsOpen = back.IsOpen;
                    list.Status = "等待质管检查";
                    list.DutyPersonDate = ServerTime.Time;

                    dataContxt.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;

                return false;
            }

            return true;
        }

        /// <summary>
        /// 删除返回件
        /// </summary>
        /// <param name="strDJH">单据号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        private bool DeleteReplaceReturn(string strDJH, out string error)
        {
            error = "";

            try
            {
                WebSiteDataClassesDataContext dataContxt = CommentParameter2.WebDataContext;

                var vardata = from a in dataContxt.S_ReplaceAccessory
                              where a.ServiceID == strDJH
                              select a;

                if (vardata.Count() != 0)
                {
                    dataContxt.S_ReplaceAccessory.DeleteAllOnSubmit(vardata);
                }

                dataContxt.SubmitChanges();
            }
            catch (Exception ex)
            {
                error = ex.Message;

                return false;
            }

            return true;
        }

        /// <summary>
        /// 获得所有信息来源
        /// </summary>
        /// <returns>成功返回满足条件的数据集，失败返回null的DataTable</returns>
        public DataTable GetMessageSource()
        {
            string sql = "select * from View_S_MessageSource";

            DataTable dt = GlobalObject.DatabaseServer.WebQueryInfo(sql);

            return dt;
        }

        /// <summary>
        /// 获取返回件信息
        /// </summary>
        /// <param name="strDJH">单据号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>成功返回满足条件的数据集，失败返回null的dataTable</returns>
        public DataTable GetReplace(string strDJH, out string error)
        {
            error = "";

            string sql = @"select ServiceID 关联单号, OldGoodsName " +
                         " 返回件,OldGoodsCode 返回件图号, OldSpec " +
                         " 规格,OldGoodsID 返回件编号,OldCvtID 总成编号, GiveOutDate 旧件发出时间,BackTime 返回时间, NewGoodsName 更新件," +
                         " NewGoodsCode 图号,NewSpec 规格,NewGoodsID 更新件编号,NewCvtID 总成编号 from " +
                         " S_ReplaceAccessory where ServiceID='" + strDJH + "'";

            DataTable dt = GlobalObject.DatabaseServer.WebQueryInfo(sql);

            return dt;
        }

        /// <summary>
        /// 获取返回件信息
        /// </summary>
        /// <param name="strDJH">单据号</param>
        /// <returns>成功返回满足条件的数据集，失败返回错误信息</returns>
        public DataTable GetReplaceByID(string strDJH)
        {
            string sql = @"select * from S_ReplaceAccessory where ServiceID='" + strDJH + "'";

            return GlobalObject.DatabaseServer.WebQueryInfo(sql);
        }

        /// <summary>
        /// 通过单据号查询函电信息
        /// </summary>
        /// <param name="strDJH">单据号</param>
        /// <returns>成功返回满足条件的数据集，失败返回null的dataTable</returns>
        public DataTable GetAfterServiceByBillID(string strDJH)
        {
            string sql = @"select * from dbo.View_S_AfterService where 单据号='" + strDJH + "'";

            DataTable dt = GlobalObject.DatabaseServer.WebQueryInfo(sql);

            return dt;
        }

        /// <summary>
        /// 通过单据号查询售后反馈单信息
        /// </summary>
        /// <param name="serviceID">售后反馈单号</param>
        /// <returns>成功返回满足条件的数据集，失败返回null</returns>
        public DataRow GetServiceFeedBackBill(string serviceID)
        {
            string strSql = @"select * from S_ServiceFeedBack where ServiceID = '" + serviceID + "'";

            DataTable dt = GlobalObject.DatabaseServer.WebQueryInfo(strSql);

            if (dt.Rows.Count != 1)
            {
                return null;
            }
            else
            {
                return dt.Rows[0];
            }
        }

        /// <summary>
        /// 获取所有来电类型
        /// </summary>
        /// <returns>成功返回满足条件的数据集，失败返回null的dataTable</returns>
        public DataTable GetMessageType()
        {
            string sql = @"select * from dbo.S_Commtion";

            DataTable dt = GlobalObject.DatabaseServer.WebQueryInfo(sql);

            return dt;
        }

        /// <summary>
        /// 根据客户名称获得客户编码
        /// </summary>
        /// <param name="clientName">客户名称</param>
        /// <returns>成功返回客户编码，失败返回null</returns>
        public DataRow GetClient(string clientName)
        {
            string sql = @"select ClientName,ClientCode from Client where ClientCode='" + clientName + "' or ClientName='" + clientName + "'";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            return dt.Rows[0];
        }

        /// <summary>
        /// 获得所有的故障代码
        /// </summary>
        /// <returns>成功返回满足条件的数据集，失败返回null的datatable</returns>
        public DataTable GetBugCode()
        {
            string sql = @"select ID,BugName from dbo.OF_BugCode";

            DataTable dt = GlobalObject.DatabaseServer.WebQueryInfo(sql);

            return dt;
        }

        /// <summary>
        /// 获得故障代码
        /// </summary>
        /// <returns>成功返回ID，失败返回null</returns>
        public DataRow GetBugCodeByID(string bugName)
        {
            string sql = @"select ID from dbo.OF_BugCode where BugName='" + bugName + "'";

            DataTable dt = GlobalObject.DatabaseServer.WebQueryInfo(sql);

            return dt.Rows[0];
        }

        /// <summary>
        /// 获得故障名称
        /// </summary>
        /// <returns>成功返回名称，失败返回null</returns>
        public DataRow GetBugCodeByName(int bugName)
        {
            string sql = @"select BugName from dbo.OF_BugCode where ID=" + bugName;

            DataTable dt = GlobalObject.DatabaseServer.WebQueryInfo(sql);

            return dt.Rows[0];
        }

        /// <summary>
        /// 变更里程数
        /// </summary>
        /// <param name="vehicleShelfNumber">车架号</param>
        /// <param name="vKT">里程数</param>
        /// <returns>成功返回True，失败返回False</returns>
        bool ChangeVKT(string vehicleShelfNumber, int vKT)
        {
            DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

            var varData = from a in dataContext.YX_CVTCustomerInformation
                          where a.VehicleShelfNumber == vehicleShelfNumber
                          select a;

            if (varData.Count() != 1)
            {
                return false;
            }
            else
            {
                YX_CVTCustomerInformation lnqCustomerInfo = varData.Single();

                lnqCustomerInfo.VKT = vKT;

                dataContext.SubmitChanges();

                return true;
            }
        }

        /// <summary>
        /// 客服中心添加单据基础信息
        /// </summary>
        /// <param name="service">数据对象集</param>
        /// <param name="messageInfo">故障信息数据集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>添加成功返回True失败返回False</returns>
        public bool InsertService(S_AfterService service, OF_BugMessageInfo messageInfo, out string error)
        {
            error = "";

            try
            {
                WebSiteDataClassesDataContext dataContxt = CommentParameter2.WebDataContext;

                var varData = from a in dataContxt.S_AfterService
                              where a.ServiceID == service.ServiceID
                              select a;

                if (varData.Count() > 0)
                {
                    if (InsertBugMessage(messageInfo, service.ServiceID, out error))
                    {
                        S_AfterService list = varData.Single();

                        if (BasicInfo.LoginID == list.Applicant && list.Status == "等待接单")
                        {
                            list.MessageSource = service.MessageSource;
                            list.ServerType = service.ServerType;
                            list.ContentType = service.ContentType;
                            list.SiteName = service.SiteName;
                            list.LinkTel = service.LinkTel;
                            list.UserName = service.UserName;
                            list.UserTel = service.UserTel;
                            list.UserAddress = service.UserAddress;
                            list.CarModel = service.CarModel;
                            list.CVTCode = service.CVTCode;
                            list.CVTID = service.CVTID;
                            list.ChassisNum = service.ChassisNum;
                            list.BugAddress = service.BugAddress;
                            list.BuyCarTime = service.BuyCarTime;
                            list.RunMileage = service.RunMileage;
                            list.UseProperty = service.UseProperty;
                            list.UserAttitude = service.UserAttitude;
                            list.ServiceIdea = service.ServiceIdea;
                            list.ProcessName = service.ProcessName;
                            list.HelpMoney = service.HelpMoney;
                            list.CVTStatus = service.CVTStatus;
                            list.CustomerDate = service.CustomerDate;
                            list.NoticeDate = service.NoticeDate;
                            list.StrategyDate = service.StrategyDate;
                            list.AcceptName = service.AcceptName;
                            list.AcceptTime = service.AcceptTime;
                            list.TCUCode = service.TCUCode;
                            list.NewSoftware = service.NewSoftware;
                            list.IsThreeGuarantees = service.IsThreeGuarantees;
                            list.IsServiceStock = service.IsServiceStock;
                        }

                        list.BugDescribe = GetBugMessageByServiceID(list.ServiceID).Rows[0]["ID"].ToString();

                        if (!ChangeVKT(service.ChassisNum, Convert.ToInt32(service.RunMileage)))
                        {
                            error = "修改客户信息里程数出错";
                            return false;
                        }

                        dataContxt.SubmitChanges();
                    }
                }
                else
                {
                    S_AfterService list = new S_AfterService();

                    list.ServiceID = service.ServiceID;

                    if (InsertBugMessage(messageInfo, list.ServiceID, out error))
                    {
                        list.MessageSource = service.MessageSource;
                        list.ServerType = service.ServerType;
                        list.ContentType = service.ContentType;
                        list.AcceptName = service.AcceptName;
                        list.AcceptTime = service.AcceptTime;
                        list.SiteName = service.SiteName;
                        list.LinkTel = service.LinkTel;
                        list.UserName = service.UserName;
                        list.UserTel = service.UserTel;
                        list.UserAddress = service.UserAddress;
                        list.CarModel = service.CarModel;
                        list.CVTCode = service.CVTCode;
                        list.CVTID = service.CVTID;
                        list.ChassisNum = service.ChassisNum;
                        list.BugAddress = service.BugAddress;
                        list.BuyCarTime = service.BuyCarTime;
                        list.RunMileage = service.RunMileage;
                        list.UseProperty = service.UseProperty;
                        list.UserAttitude = service.UserAttitude;
                        list.BugDescribe = GetBugMessageByServiceID(list.ServiceID).Rows[0]["ID"].ToString();
                        list.ServiceIdea = service.ServiceIdea;
                        list.ProcessName = service.ProcessName;
                        list.HelpMoney = service.HelpMoney;
                        list.CVTStatus = service.CVTStatus;
                        list.Status = "等待接单";
                        list.CustomerDate = service.CustomerDate;
                        list.NoticeDate = service.NoticeDate;
                        list.StrategyDate = service.StrategyDate;
                        list.ApplicantDate = service.ApplicantDate;
                        list.Applicant = service.Applicant; 
                        list.TCUCode = service.TCUCode;
                        list.NewSoftware = service.NewSoftware;
                        list.IsThreeGuarantees = service.IsThreeGuarantees;
                        list.IsServiceStock = service.IsServiceStock;
                        list.IsCVTBug = service.IsCVTBug;

                        if (!ChangeVKT(service.ChassisNum, Convert.ToInt32(service.RunMileage)))
                        {
                            error = "修改客户信息里程数出错";
                            return false;
                        }

                        dataContxt.S_AfterService.InsertOnSubmit(list);
                        dataContxt.SubmitChanges();
                    }
                    else
                    {
                        error = "返回件信息有错误！";
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;

                return false;
            }
            return true;
        }

        #region 售后人员填写故障处理
        /// <summary>
        /// 售后人员填写故障处理
        /// </summary>
        /// <param name="service">数据对象</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <param name="dt">明细数据集</param>
        /// <param name="messageInfo">故障信息数据集</param>
        /// <param name="flag">是否为接单状态，是：true，否：false</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        public bool UpdateSaleTable(S_AfterService service, DataTable dt, OF_BugMessageInfo messageInfo,bool flag, out string error)
        {
            error = "";

            try
            {
                WebSiteDataClassesDataContext dataContxt = CommentParameter2.WebDataContext;

                var varData = from a in dataContxt.S_AfterService
                              where a.ServiceID == service.ServiceID
                              select a;

                if (varData.Count() > 0)
                {
                    if (InsertBugMessage(messageInfo, service.ServiceID, out error))
                    {
                        S_AfterService list = varData.Single();

                        list.BugDescribe = GetBugMessageByServiceID(list.ServiceID).Rows[0]["ID"].ToString();
                        list.ProcessMode = service.ProcessMode;
                        list.ProcessTime = service.ProcessTime;
                        list.DiagnoseSituation = service.DiagnoseSituation;
                        list.Solution = service.Solution;
                        list.ProcessResult = service.ProcessResult;
                        list.LocaleProcess = service.LocaleProcess;
                        list.IsCVTBug = service.IsCVTBug;

                        if (flag)
                        {
                            list.Status = "等待结果确认";
                        }
                        else
                        {
                            list.Status = "等待审核";
                        }

                        list.TCUCode = service.TCUCode;

                        if (InsertReplace(dt, service.ServiceID, out error))
                        {
                            dataContxt.SubmitChanges();
                        }
                        else
                        {
                            return false;
                        }

                        dataContxt.SubmitChanges();
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;

                return false;
            }

            return true;
        }
        #endregion

        #region 审核
        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="serviceID">单据号</param>
        /// <param name="checkName">审核人</param>
        /// <param name="checkTime">审核时间</param>
        /// <param name="messageInfo">故障信息数据集</param>
        /// <param name="feedBack">反馈信息数据集</param>
        /// <param name="fkBill">反馈单号</param>
        /// <param name="processResult">是否填写反馈单</param>
        /// <param name="diagnoseSituation">诊断及测试结果</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        public bool UpdateCheckTable(string serviceID, string checkName, string checkTime, string fkBill,
            S_AfterService feedBack, OF_BugMessageInfo messageInfo, string processResult,
            string diagnoseSituation, out string error)
        {
            error = "";

            try
            {
                WebSiteDataClassesDataContext dataContxt = CommentParameter2.WebDataContext;

                var varData = from a in dataContxt.S_AfterService
                              where a.ServiceID == serviceID
                              select a;

                if (varData.Count() != 1)
                {
                    error = "【" + serviceID + "】" + "单据不存在或数据有错误！";
                    return false;
                }
                else
                {
                    if (fkBill.Equals("是"))
                    {
                        S_AfterService list = varData.Single();

                        list.CheckName = checkName;
                        list.CheckTime = Convert.ToDateTime(checkTime);
                        list.FKBillID = GetNextBillID(2);

                        var vardataFK = from b in dataContxt.S_ServiceFeedBack
                                        where b.ServiceID == serviceID
                                        select b;

                        if (vardataFK.Count() == 0)
                        {
                            S_ServiceFeedBack listFK = new S_ServiceFeedBack();

                            listFK.FK_Bill_ID = list.FKBillID;
                            listFK.ServiceID = feedBack.ServiceID;
                            listFK.MessageSource = feedBack.MessageSource;
                            listFK.SiteName = GetClient(feedBack.SiteName)["ClientCode"].ToString();
                            listFK.CVTCode = feedBack.CVTCode;
                            listFK.CarModel = feedBack.CarModel;
                            listFK.CVTID = feedBack.CVTID;
                            listFK.ChassisNum = feedBack.ChassisNum;
                            listFK.UserName = feedBack.UserName;
                            listFK.Linkman = feedBack.UserName;
                            listFK.LinkTel = feedBack.LinkTel;
                            //listFK;

                            if (feedBack.BuyCarTime != null)
                            {
                                listFK.BuyCarTime = Convert.ToDateTime(feedBack.BuyCarTime);
                            }

                            listFK.RunMileage = feedBack.RunMileage;
                            listFK.ProcessName = feedBack.ProcessName;
                            listFK.ProcessTime = feedBack.ProcessTime;
                            listFK.ProcessMode = "已处理";
                            listFK.Solution = processResult;
                            listFK.DiagnoseSituation = GetBugMessageByServiceID(serviceID).Rows[0]["ID"].ToString();

                            var vardataList = from f in dataContxt.S_ReplaceAccessory
                                              where f.ServiceID == serviceID
                                              select f;

                            if (vardataList.Count() > 0)
                            {
                                listFK.Status = "等待确认返回时间";
                            }
                            else
                            {
                                listFK.Status = "等待主管审核";
                            }

                            listFK.BugNumber = "1";
                            listFK.BatchNumber = "1";
                            listFK.CVTCondition = feedBack.CVTStatus;
                            listFK.TCUCode = feedBack.TCUCode;
                            listFK.NewSoftware = feedBack.NewSoftware;

                            dataContxt.S_ServiceFeedBack.InsertOnSubmit(listFK);
                            list.Status = "等待回访";

                            dataContxt.SubmitChanges();
                            return true;
                        }
                        else
                        {
                            error = "此单据的反馈单已经填写，不能再次写入！";
                            return false;
                        }
                    }
                    else
                    {
                        S_AfterService list = varData.Single();

                        list.CheckName = checkName;
                        list.CheckTime = Convert.ToDateTime(checkTime);
                        list.FKBillID = "无";
                        list.Status = "等待回访";

                        dataContxt.SubmitChanges();

                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;

                return false;
            }
        }
        #endregion

        #region 客户回访
        /// <summary>
        /// 客户回访，修改表
        /// </summary>
        /// <param name="service">数据对象</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>成功返回true失败返回False</returns>
        public bool UpdateResultTable(S_AfterService service, out string error)
        {
            error = "";

            try
            {
                WebSiteDataClassesDataContext dataContxt = CommentParameter2.WebDataContext;

                var varData = from a in dataContxt.S_AfterService
                              where a.ServiceID == service.ServiceID
                              select a;

                if (varData.Count() != 1)
                {
                    error = "【" + service.ServiceID + "】" + "单据不存在或数据有错误！";
                    return false;
                }
                else
                {
                    S_AfterService list = varData.Single();

                    list.RepairQuality = service.RepairQuality;
                    list.ServiceAttitude = service.ServiceAttitude;
                    list.IsCharge = service.IsCharge;
                    list.Amount = service.Amount;
                    list.FailureResults = service.FailureResults;
                    list.ReturnName = service.ReturnName;
                    list.ReturnTime = service.ReturnTime;
                    list.Status = "处理完成";

                    dataContxt.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;

                return false;
            }

            return true;
        }
        #endregion

        /// <summary>
        /// 通过反馈单据号查找id
        /// </summary>
        /// <returns>成功返回一行数据集，失败返回null</returns>
        public DataRow GetFeedBackID()
        {
            string sql = "select max(id) as id,FK_Bill_ID from dbo.S_ServiceFeedBack group by FK_Bill_ID";

            DataTable dt = GlobalObject.DatabaseServer.WebQueryInfo(sql);

            return dt.Rows[0];
        }

        /// <summary>
        /// 判断是否使用了用户信息
        /// </summary>
        /// <param name="chassisNum">车架号</param>
        /// <returns>成功返回整形，失败返回null</returns>
        public DataRow GetAfterServicerByUserInfo(string chassisNum)
        {
            string sql = "select Count(*) from dbo.S_AfterService where ChassisNum='" + chassisNum + "'";

            DataTable dt = GlobalObject.DatabaseServer.WebQueryInfo(sql);

            return dt.Rows[0];
        }

        /// <summary>
        /// 判断是否使用了用户信息
        /// </summary>
        /// <param name="chassisNum">车架号</param>
        /// <returns>成功返回整形，失败返回null</returns>
        public DataRow GetServiceFeedBackByUserInfo(string chassisNum)
        {
            string sql = "select Count(*) from dbo.S_ServiceFeedBack where ChassisNum='" + chassisNum + "'";

            DataTable dt = GlobalObject.DatabaseServer.WebQueryInfo(sql);

            return dt.Rows[0];
        }

        /// <summary>
        /// 回退反馈单
        /// </summary>
        /// <param name="strDJH">反馈单号</param>
        /// <param name="strBillStatus">单据状态</param>
        /// <param name="strRebackReason">回退原因</param>
        /// <param name="YXBillNo">营销退库单号</param>
        /// <param name="error">错误信息</param>
        /// <returns>回退成功返回True失败返回False</returns>
        public bool ReturnFeedBackBill(string strDJH, string strBillStatus, string strRebackReason,string YXBillNo, out string error)
        {
            error = "";

            try
            {
                string strMsg = "";
                m_billMessageServer.BillType = "售后反馈单";

                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;
                var resultList = from a in dataContxt.Out_ManeuverBill
                                 where a.Bill_ID == YXBillNo
                                 select a;

                WebSiteDataClassesDataContext webDataContxt = CommentParameter2.WebDataContext;

                var result = from c in webDataContxt.S_ServiceFeedBack
                             where c.FK_Bill_ID == strDJH
                             select c;

                if (result.Count() == 1)
                {
                    S_ServiceFeedBack feed = result.Single();

                    switch (strBillStatus)
                    {
                        case "等待确认返回时间":
                            if (resultList.Count() > 0)
                            {
                                if (resultList.Single().BillStatus == "已完成")
                                {
                                    error = "此单据已生成的调运单【" + YXBillNo + "】已经完成，不能回退！";
                                    return false;
                                }
                                else
                                {
                                    if (!Service_Peripheral_External.ServerModuleFactory.GetServerModule<IManeuverServer>().DeleteBill(resultList.Single().Bill_ID, out error))
                                    {
                                        return false;
                                    }
                                }
                            }

                            strMsg = string.Format("{0}号售后反馈单已回退，请您重新处理单据; 回退原因为" + strRebackReason, strDJH);
                            m_billMessageServer.PassFlowMessage(strDJH, strMsg, CE_RoleEnum.营销普通人员.ToString(), false);

                            feed.Status = "等待确认返回时间";
                            feed.YXChargeSuggestion = "";
                            break;
                        case "等待主管审核":
                            if (resultList.Count() > 0)
                            {
                                if (resultList.Single().BillStatus == "已完成")
                                {
                                    error = "此单据已生成的调运单【" + YXBillNo + "】已经完成，不能回退！";
                                    return false;
                                }
                                else
                                {
                                    if (!Service_Peripheral_External.ServerModuleFactory.GetServerModule<IManeuverServer>().DeleteBill(resultList.Single().Bill_ID, out error))
                                    {
                                        return false;
                                    }
                                }
                            }

                            feed.Status = "等待主管审核";
                            feed.YXChargeSuggestion = "";
                            feed.BugCode = "";
                            feed.IsBack = "";
                            strMsg = string.Format("{0}号售后反馈单已回退，请您重新处理单据; 回退原因为" + strRebackReason, strDJH);
                            m_billMessageServer.PassFlowMessage(strDJH, strMsg, CE_RoleEnum.营销主管.ToString(), false);
                            break;
                        case "等待质管确认":
                            feed.Status = "等待质管确认";
                            feed.ZGChargeSuggestion = "";
                            feed.ZGCheckName = "";
                            feed.AppearCount = 0;
                            feed.DutyDept = "";

                            strMsg = string.Format("{0}号售后反馈单已回退，请您重新处理单据; 回退原因为" + strRebackReason, strDJH);
                            m_billMessageServer.PassFlowMessage(strDJH, strMsg, CE_RoleEnum.质量工程师.ToString(), false);

                            break;
                        case "等待责任部门确认":
                            feed.Status = "等待责任部门确认";
                            feed.DutyDeptCharge = "";
                            feed.DutyPerson = "";
                            feed.FinishClaim = "";
                            feed.StockSuggestion = "";
                            break;
                        case "等待责任人确认":
                            feed.Status = "等待责任人确认";
                            feed.Temporary = "";
                            feed.Analyse = "";
                            feed.foreverImplement = "";
                            feed.IsFMEAfile = "";
                            feed.IsOpen = "";

                            strMsg = string.Format("{0}号售后函电处理单已回退，请您重新处理单据; 回退原因为" + strRebackReason, strDJH);
                            m_billMessageServer.PassFlowMessage(strDJH, strMsg, Service_Peripheral_HR.ServerModuleFactory.GetServerModule<IPersonnelArchiveServer>().GetPersonnelViewInfoByName(feed.DutyPerson), false);
                            break;
                        case "等待质管检查":
                            feed.Status = "等待质管检查";
                            feed.IsClose = "";
                            feed.Practicable = "";
                            break;
                        default:
                            break;
                    }

                    webDataContxt.SubmitChanges();
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
        /// 回退单据
        /// </summary>
        /// <param name="strDJH">单据号</param>
        /// <param name="strBillStatus">单据状态</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <param name="strRebackReason">回退原因</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        public bool ReturnBill(string strDJH, string strBillStatus, out string error, string strRebackReason)
        {
            error = null;

            try
            {
                m_billMessageServer.BillType = "售后函电处理单";
                WebSiteDataClassesDataContext webDataContxt = CommentParameter2.WebDataContext;

                var varData = from a in webDataContxt.S_AfterService
                              where a.ServiceID == strDJH
                              select a;

                var result = from c in webDataContxt.S_ServiceFeedBack
                             where c.ServiceID == strDJH
                             select c;

                if (result.Count() > 0)
                {
                    error = "此函电已经生成了售后反馈单，不能回退！";
                    return false;
                }

                string strMsg = "";

                if (varData.Count() == 1)
                {
                    S_AfterService service = varData.Single();

                    switch (strBillStatus)
                    {
                        case "等待接单":
                            strMsg = string.Format("{0}号售后函电处理单已回退，请您重新处理单据; 回退原因为" + strRebackReason, strDJH);
                            m_billMessageServer.PassFlowMessage(strDJH, strMsg, CE_RoleEnum.营销普通人员.ToString(), false);

                            service.Status = "等待接单";
                            //service.ProcessResult = "";
                            service.ProcessTime = ServerTime.Time;
                            //service.DiagnoseSituation = "";
                            //service.Solution = "";

                            //if (!DeleteReplaceReturn(strDJH, out error))
                            //{
                            //    return false;
                            //}
                            break;
                        case "等待审核":
                            strMsg = string.Format("{0}号售后函电处理单已回退，请您重新处理单据; 回退原因为" + strRebackReason, strDJH);

                            m_billMessageServer.PassFlowMessage(strDJH, strMsg,
                                CE_RoleEnum.营销主管.ToString(), false);

                            service.Status = "等待审核";
                            service.CheckName = "";
                            service.CheckTime = ServerTime.Time;
                            //service.FKBillID = System.DBNull.Value.ToString();
                            break;
                        default:
                            break;
                    }

                    webDataContxt.SubmitChanges();

                    return true;
                }
                else
                {
                    error = "数据不唯一或者为空";

                    return false;
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;

                return false;
            }
        }

        /// <summary>
        /// 通过函电处理单据号删除反馈信息
        /// </summary>
        /// <param name="serviceID">单据号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        private bool DeleteFeedBackByServiceID(string serviceID, out string error)
        {
            error = "";

            try
            {
                WebSiteDataClassesDataContext dataContxt = CommentParameter2.WebDataContext;

                var varData = from a in dataContxt.S_ServiceFeedBack
                              where a.ServiceID == serviceID
                              select a;

                if (varData.Count() > 1)
                {
                    error = "单据信息有误！";
                    return false;
                }
                else if (varData.Count() == 0)
                {
                    return true;
                }
                else
                {
                    if (varData.Single().Status != "单据完成")
                    {
                        dataContxt.S_ServiceFeedBack.DeleteAllOnSubmit(varData);
                    }
                    else
                    {
                        error = serviceID + " 函电处理单所对应的质量反馈单已经处理完成，所以不能回退单据！";

                        return false;
                    }

                    dataContxt.SubmitChanges();

                    return true;
                }
            }
            catch
            {
                error = "数据有误！";

                return false;
            }
        }

        /// <summary>
        /// 通过单据号查询附件内容
        /// </summary>
        /// <param name="strDJH">单据号</param>
        /// <returns>成功返回满足条件的数据集，失败返回null的dataTable</returns>
        public DataTable GetAfterServiceFile(string strDJH)
        {
            string sql = @"select * from dbo.S_ServiceFileDown where ServiceID='" + strDJH + "'";

            DataTable dt = GlobalObject.DatabaseServer.WebQueryInfo(sql);

            return dt;
        }

        /// <summary>
        /// 获得库存信息
        /// </summary>
        /// <param name="siteCode">服务站编号</param>
        /// <param name="goodsID">物品编号</param>
        /// <returns>成功返回整形，失败返回null</returns>
        public DataTable GetStockNum(string siteCode, string goodsID)
        {
            string sql = "select b.StockQty from  View_F_GoodsPlanCost as a inner join " +
                         " Out_Stock as b on a.序号 = b.GoodsID" +
                         " where SecStorageID='" + siteCode + "' and goodsID=" + goodsID;

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            return dt;
        }
    }
}

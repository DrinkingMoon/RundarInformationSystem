/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  CVTCustomerInformationServer.cs
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
using DBOperate;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using PlatformManagement;
using ServerModule;
using GlobalObject;

namespace ServerModule
{
    /// <summary>
    /// CVT客户信息服务类
    /// </summary>
    class CVTCustomerInformationServer :BasicServer, ICVTCustomerInformationServer
    {
        /// <summary>
        /// 获得CVT客户基础信息
        /// </summary>
        /// <returns>返回CVT客户基础信息</returns>
        public DataTable GetCVTCustomerInformation()
        {
            string strSql = "select * from View_YX_CVTCustomerInformation";

            DataTable dtTemp = GlobalObject.DatabaseServer.QueryInfo(strSql);

            return dtTemp;
        }

        /// <summary>
        /// 删除数据CVT客户基础信息
        /// </summary>
        /// <param name="cvtCustomer">CVT客户信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>删除成功返回True，删除失败返回False</returns>
        public bool DeleteCVTCustomerInformation(YX_CVTCustomerInformation cvtCustomer, out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

                var varData = from a in dataContext.YX_CVTCustomerInformation
                              where a.ID == cvtCustomer.ID
                              select a;

                dataContext.YX_CVTCustomerInformation.DeleteAllOnSubmit(varData);
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
        /// 插入数据CVT客户基础信息
        /// </summary>
        /// <param name="cvtCustomer">要插入的CVT客户信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>插入成功返回True，插入失败返回False</returns>
        public bool InsertCVTCustomerInformation(YX_CVTCustomerInformation cvtCustomer, out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

                var varData = from a in dataContext.YX_CVTCustomerInformation
                              where a.VehicleShelfNumber == cvtCustomer.VehicleShelfNumber
                              select a;

                if (varData.Count() != 0)
                {
                    error = string.Format("已经存在车架号【{0}】的信息，不允许重复录入",
                        cvtCustomer.VehicleShelfNumber);

                    return false;
                }
                else
                {
                    dataContext.YX_CVTCustomerInformation.InsertOnSubmit(cvtCustomer);
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
        /// 更改数据CVT客户基础信息
        /// </summary>
        /// <param name="cvtCustomer">CVT客户信息</param>
        /// <param name="error">c错误信息</param>
        /// <returns>更改成功返回True，更改失败返回False</returns>
        public bool UpdateCVTCustomerInformation(YX_CVTCustomerInformation cvtCustomer, out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

                var varData = from a in dataContext.YX_CVTCustomerInformation
                              where a.ID != cvtCustomer.ID
                              && a.VehicleShelfNumber == cvtCustomer.VehicleShelfNumber
                              select a;

                if (varData.Count() != 0)
                {
                    error = "数据不唯一";
                    return false;
                }
                else
                {
                    var varCVT = from a in dataContext.YX_CVTCustomerInformation
                                 where a.ID == cvtCustomer.ID
                                 select a;

                    if (varCVT.Count() == 1)
                    {
                        YX_CVTCustomerInformation lnqCustomer = varCVT.Single();

                        lnqCustomer.CarModelID = cvtCustomer.CarModelID;
                        lnqCustomer.ClientName = cvtCustomer.ClientName;
                        lnqCustomer.CVTNumber = cvtCustomer.CVTNumber;
                        lnqCustomer.DealerName = cvtCustomer.DealerName;
                        lnqCustomer.FullAddress = cvtCustomer.FullAddress;
                        lnqCustomer.PhoneNumber = cvtCustomer.PhoneNumber;
                        lnqCustomer.ProductID = cvtCustomer.ProductID;
                        lnqCustomer.Remark = cvtCustomer.Remark;
                        lnqCustomer.SellDate = cvtCustomer.SellDate;
                        lnqCustomer.SiteCity = cvtCustomer.SiteCity;
                        lnqCustomer.SiteProvince = cvtCustomer.SiteProvince;
                        lnqCustomer.VehicleShelfNumber = cvtCustomer.VehicleShelfNumber;
                        lnqCustomer.VKT = cvtCustomer.VKT;
                        lnqCustomer.OverTheReason = cvtCustomer.OverTheReason;
                        lnqCustomer.ProofNo = cvtCustomer.ProofNo;

                        var varInfo = from a in dataContext.YX_CVTCustomerInformation
                                      where a.VehicleShelfNumber == cvtCustomer.VehicleShelfNumber
                                      select a;

                        if (varInfo.Count() == 1)
                        {
                            YX_CVTCustomerInformation lnqInfo = varInfo.Single();


                            if (lnqInfo.CVTNumber != cvtCustomer.CVTNumber)
                            {
                                YX_CVTCustomerInformationHistory lnqCustomerHistory = new YX_CVTCustomerInformationHistory();

                                IProductListServer serverProcutList = ServerModuleFactory.GetServerModule<IProductListServer>();

                                lnqCustomerHistory.CarModel = serverProcutList.GetMotorcycleInfo(Convert.ToInt32(cvtCustomer.CarModelID));
                                lnqCustomerHistory.ClientName = cvtCustomer.ClientName;

                                IBasicGoodsServer serverBasicGoods = ServerModuleFactory.GetServerModule<IBasicGoodsServer>();

                                F_GoodsPlanCost lnqGoods = serverBasicGoods.GetGoodsInfo(Convert.ToInt32(cvtCustomer.ProductID));

                                lnqCustomerHistory.CVTType = lnqGoods.GoodsCode;
                                lnqCustomerHistory.DealerName = cvtCustomer.DealerName;
                                lnqCustomerHistory.FinishDate = ServerTime.Time;
                                lnqCustomerHistory.FinishPersonnel = BasicInfo.LoginName;
                                lnqCustomerHistory.NewPartCode = cvtCustomer.CVTNumber;
                                lnqCustomerHistory.OldPartCode = lnqInfo.CVTNumber;
                                lnqCustomerHistory.Remark = "手动修改";
                                lnqCustomerHistory.ReplaceCode = lnqGoods.GoodsCode;
                                lnqCustomerHistory.ReplaceName = lnqGoods.GoodsName;
                                lnqCustomerHistory.ReplaceSpec = lnqGoods.Spec;
                                lnqCustomerHistory.VehicleShelfNumber = cvtCustomer.VehicleShelfNumber;

                                dataContext.YX_CVTCustomerInformationHistory.InsertOnSubmit(lnqCustomerHistory);
                            }
                        }
                    }
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
        /// 批量插入CVT客户基础信息
        /// </summary>
        /// <param name="cvtCustomerInfomation">CVT客户信息列表</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>插入成功返回True，插入失败返回False</returns>
        public bool BatchInsertCVTCustomerInformation(DataTable cvtCustomerInfomation,
            out string error)
        {
            error = null;

            string strTemp = "";

            int intFlag = 0;

            ProductListServer serverProductList = new ProductListServer();

            try
            {
                DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

                for (int i = 0; i < cvtCustomerInfomation.Rows.Count; i++)
                {
                    strTemp = cvtCustomerInfomation.Rows[i]["车架号"].ToString().Trim();

                    if (strTemp == "")
                    {
                        continue;
                    }

                    YX_CVTCustomerInformation lnqCVTCustomerInfo = new YX_CVTCustomerInformation();

                    int intGoodsID = serverProductList.GetProductGoodsID(
                        cvtCustomerInfomation.Rows[i]["CVT型号"].ToString().Trim(), 0, false);

                    if (intGoodsID == 0)
                    {
                        error = "[CVT型号]不符合标准,车架号为[" + strTemp + "]";
                        return false;
                    }

                    if (cvtCustomerInfomation.Rows[i]["销售日期"].ToString().Trim() == "")
                    {
                        error = error + "[" + cvtCustomerInfomation.Rows[i]["车架号"].ToString().Trim() + "]";
                    }

                    var varData = from a in dataContext.YX_CVTCustomerInformation
                                  where a.VehicleShelfNumber == strTemp
                                  select a;

                    if (varData.Count() == 0)
                    {
                        lnqCVTCustomerInfo.ClientName = cvtCustomerInfomation.Rows[i]["客户名称"].ToString().Trim();
                        lnqCVTCustomerInfo.CVTNumber = cvtCustomerInfomation.Rows[i]["CVT编号"].ToString().Trim();
                        lnqCVTCustomerInfo.DealerName = cvtCustomerInfomation.Rows[i]["经销商名称"].ToString().Trim();
                        lnqCVTCustomerInfo.FullAddress = cvtCustomerInfomation.Rows[i]["详细地址"].ToString().Trim();
                        lnqCVTCustomerInfo.PhoneNumber = cvtCustomerInfomation.Rows[i]["联系电话"].ToString().Trim();
                        lnqCVTCustomerInfo.ProductID = intGoodsID;
                        lnqCVTCustomerInfo.Remark = cvtCustomerInfomation.Rows[i]["备注"].ToString().Trim();
                        lnqCVTCustomerInfo.SellDate = ServerTime.ConvertToDateTime(
                            cvtCustomerInfomation.Rows[i]["销售日期"].ToString().Trim());
                        lnqCVTCustomerInfo.SiteCity = cvtCustomerInfomation.Rows[i]["车辆所在地"].ToString().Trim();
                        lnqCVTCustomerInfo.SiteProvince = cvtCustomerInfomation.Rows[i]["省份"].ToString().Trim();
                        lnqCVTCustomerInfo.VehicleShelfNumber = strTemp;
                        lnqCVTCustomerInfo.PY = UniversalFunction.GetPYWBCode(lnqCVTCustomerInfo.ClientName, "PY");
                        lnqCVTCustomerInfo.WB = UniversalFunction.GetPYWBCode(lnqCVTCustomerInfo.ClientName, "WB");
                        lnqCVTCustomerInfo.CarModelID = serverProductList.GetMotorcycleType(
                            cvtCustomerInfomation.Rows[i]["车型"].ToString().Trim());
                        lnqCVTCustomerInfo.ProofNo = cvtCustomerInfomation.Rows[i]["三包凭证号"].ToString().Trim();

                        dataContext.YX_CVTCustomerInformation.InsertOnSubmit(lnqCVTCustomerInfo);
                    }
                    else
                    {
                        lnqCVTCustomerInfo = varData.Single();

                        lnqCVTCustomerInfo.SiteCity = cvtCustomerInfomation.Rows[i]["车辆所在地"].ToString().Trim();
                        lnqCVTCustomerInfo.DealerName = cvtCustomerInfomation.Rows[i]["经销商名称"].ToString().Trim();
                        lnqCVTCustomerInfo.SiteProvince = cvtCustomerInfomation.Rows[i]["省份"].ToString().Trim();
                        lnqCVTCustomerInfo.ClientName = cvtCustomerInfomation.Rows[i]["客户名称"].ToString().Trim();
                        lnqCVTCustomerInfo.SellDate =
                            ServerTime.ConvertToDateTime(cvtCustomerInfomation.Rows[i]["销售日期"].ToString().Trim());
                        lnqCVTCustomerInfo.FullAddress = cvtCustomerInfomation.Rows[i]["详细地址"].ToString().Trim();
                        lnqCVTCustomerInfo.PhoneNumber = cvtCustomerInfomation.Rows[i]["联系电话"].ToString().Trim();
                    }

                    dataContext.SubmitChanges();
                    strTemp = "";
                }

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message + ",车架号为[" + strTemp + "]" + intFlag;
                return false;
            }
        }

        /// <summary>
        /// 根据车架号获得其历史信息
        /// </summary>
        /// <param name="vehicleShelfNumber">车架号</param>
        /// <returns>返回获得的信息</returns>
        public DataTable GetCVTCustomerHistoryInfo(string vehicleShelfNumber)
        {
            string strSql = " select  FinishDate as 更换日期,VehicleShelfNumber as 车架号, "+
                            " OldPartCode as 旧件编号,NewPartCode as 新件编号, " +
                            " ReplaceName as 更新件物品名称,ReplaceCode as 更新件图号型号, " +
                            " CVTType as 变速箱型号,CarModel as 车型, "+
                            " Remark  as 备注 from YX_CVTCustomerInformationHistory "+
                            " where VehicleShelfNumber = '" + vehicleShelfNumber + "'";

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        /// <summary>
        /// 批量自动匹配CVT编号
        /// </summary>
        public void BatchMatchingCVTNumber()
        {
            Hashtable parameters = new Hashtable();

            DataSet ds = new DataSet();

            Dictionary<OperateCMD, object> dicOperateCMD = m_dbOperate.RunProc_CMD("YX_BatchUpdate_CVTCustomerInformation_CVTNumber", ds, parameters);

        }

        /// <summary>
        /// 插入客户信息历史记录，若更换的是CVT，则修改客户信息中对应的车架号的CVT编号
        /// </summary>
        /// <param name="serviceID">反馈单号</param>
        /// <param name="vehicleShelfNumber">车架号</param>
        /// <param name="cvtType">变速箱型号</param>
        /// <param name="carModel">车型</param>
        /// <param name="clientName">客户名称</param>
        /// <param name="dealerName">经销商名称</param>
        /// <param name="replaceAccessoryList">更换件列表</param>
        /// <param name="error">错误信息</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        public bool InsertCustomerHistoryInfo(string serviceID, string vehicleShelfNumber, string cvtType, string carModel, string clientName, string dealerName,
            DataTable replaceAccessoryList, out string error)
        {

            error = null;

            try
            {
                DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

                for (int i = 0; i < replaceAccessoryList.Rows.Count; i++)
                {
                    YX_CVTCustomerInformationHistory lnqCustomerHistory = new YX_CVTCustomerInformationHistory();

                    lnqCustomerHistory.CarModel = carModel;
                    lnqCustomerHistory.ClientName = clientName;
                    lnqCustomerHistory.CVTType = cvtType;
                    lnqCustomerHistory.DealerName = dealerName;
                    lnqCustomerHistory.FinishDate = ServerTime.Time;
                    lnqCustomerHistory.FinishPersonnel = BasicInfo.LoginName;

                    lnqCustomerHistory.NewPartCode = replaceAccessoryList.Rows[i]["NewGoodsID"].ToString().Trim() == "" ?
                        replaceAccessoryList.Rows[i]["NewCvtID"].ToString().Trim() :
                        replaceAccessoryList.Rows[i]["NewGoodsID"].ToString().Trim();

                    lnqCustomerHistory.OldPartCode = replaceAccessoryList.Rows[i]["OldGoodsID"].ToString().Trim() == "" ?
                        replaceAccessoryList.Rows[i]["OldCvtID"].ToString().Trim() :
                        replaceAccessoryList.Rows[i]["OldGoodsID"].ToString().Trim();

                    lnqCustomerHistory.ReplaceCode = replaceAccessoryList.Rows[i]["OldGoodsCode"].ToString();
                    lnqCustomerHistory.ReplaceName = replaceAccessoryList.Rows[i]["OldGoodsName"].ToString();
                    lnqCustomerHistory.ReplaceSpec = replaceAccessoryList.Rows[i]["OldSpec"].ToString();
                    lnqCustomerHistory.VehicleShelfNumber = vehicleShelfNumber;

                    lnqCustomerHistory.Remark = "由单号为【" + serviceID + "】售后反馈单，自动生成";

                    dataContext.YX_CVTCustomerInformationHistory.InsertOnSubmit(lnqCustomerHistory);

                    IProductListServer serverProductList = ServerModuleFactory.GetServerModule<IProductListServer>();

                    int intGoodsID = serverProductList.GetProductGoodsID(replaceAccessoryList.Rows[i]["OldGoodsCode"].ToString(), 0, true);

                    if (intGoodsID != 0)
                    {
                        var varData = from a in dataContext.YX_CVTCustomerInformation
                                      where a.VehicleShelfNumber == vehicleShelfNumber
                                      select a;

                        if (varData.Count() == 1)
                        {
                            YX_CVTCustomerInformation lnqCustomerInfo = varData.Single();

                            lnqCustomerInfo.ProductID = intGoodsID;
                            lnqCustomerInfo.CVTNumber = replaceAccessoryList.Rows[i]["NewCvtID"].ToString().Trim();
                        }
                        else
                        {
                            error = "车架号在客户信息内不唯一或者不存在";
                            return false;
                        }
                    }
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

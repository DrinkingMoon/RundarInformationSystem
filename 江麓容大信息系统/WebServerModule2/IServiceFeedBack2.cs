using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ServerModule;

namespace WebServerModule2
{
    /// <summary>
    /// 售后质量反馈接口服务类(可调用ServerModule)
    /// </summary>
    public interface IServiceFeedBack2 : WebServerModule.IServiceFeedBack
    {
        /// <summary>
        /// 根据单据号查询反馈信息
        /// </summary>
        /// <param name="strDJH">单据号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>成功返回满足条件的数据集，失败返回错误信息</returns>
        DataTable GetTableByBill(string strDJH, out string error);

        /// <summary>
        /// 添加反馈信息
        /// </summary>
        /// <param name="feedBack">反馈的数据集</param>
        /// <param name="messageInfo">故障信息数据集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        bool InsertFeedBack(S_ServiceFeedBack feedBack, OF_BugMessageInfo messageInfo, out string error);

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
        bool UpdateYXCheckCreateManeuverBill(DataTable dtProductCodes, DataTable dtMxCK, DataRow row, string type, 
            Out_ManeuverBill maneuverBill, DataTable dtManeuverList,string backID, out string error);

        /// <summary>
        ///  营销主管审核，修改表信息
        /// </summary>
        /// <param name="backID">反馈单号</param>
        /// <param name="strDJH">单据号</param>
        /// <param name="chargeSuggestionYX">主管意见</param>
        /// <param name="TKFS">退库方式</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>成功返回True失返回False</returns>
        bool UpdateYXCheck(string strDJH, string chargeSuggestionYX, string backID, string TKFS, out string error);

        /// <summary>
        /// 责任人确认，修改表信息
        /// </summary>
        /// <param name="back">数据集对象</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        bool UpdateDutyPerson(S_ServiceFeedBack back, out string error);

        /// <summary>
        /// 获得所有信息来源
        /// </summary>
        /// <returns>成功返回满足条件的数据集，失败返回null的DataTable</returns>
        DataTable GetMessageSource();

        /// <summary>
        /// 获取返回件信息
        /// </summary>
        /// <param name="strDJH">单据号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>成功返回满足条件的数据集，失败返回null的dataTable</returns>
        DataTable GetReplace(string strDJH, out string error);

        /// <summary>
        /// 获取返回件信息
        /// </summary>
        /// <param name="strDJH">单据号</param>
        /// <returns>成功返回满足条件的数据集，失败返回错误信息</returns>
        DataTable GetReplaceByID(string strDJH);

        /// <summary>
        /// 通过单据号查询函电信息
        /// </summary>
        /// <param name="strDJH">单据号</param>
        /// <returns>成功返回满足条件的数据集，失败返回null的dataTable</returns>
        DataTable GetAfterServiceByBillID(string strDJH);

        /// <summary>
        /// 通过单据号查询售后反馈单信息
        /// </summary>
        /// <param name="serviceID">售后反馈单号</param>
        /// <returns>成功返回满足条件的数据集，失败返回null的dataTable</returns>
        DataRow GetServiceFeedBackBill(string serviceID);

        /// <summary>
        /// 获取所有来电类型
        /// </summary>
        /// <returns>成功返回满足条件的数据集，失败返回null的dataTable</returns>
        DataTable GetMessageType();

        /// <summary>
        /// 获得所有的故障代码
        /// </summary>
        /// <returns>成功返回满足条件的数据集，失败返回null的datatable</returns>
        DataTable GetBugCode();

        /// <summary>
        /// 根据客户名称获得客户编码
        /// </summary>
        /// <param name="clientName">客户名称</param>
        /// <returns>成功返回客户编码，失败返回null</returns>
        DataRow GetClient(string clientName);

        /// <summary>
        /// 添加故障信息
        /// </summary>
        /// <param name="messageInfo">实体对象</param>
        /// <param name="serviceID">函电单号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>成功返回True；失败返回False</returns>
        bool InsertBugMessage(OF_BugMessageInfo messageInfo, string serviceID, out string error);

        /// <summary>
        /// 确认返回件返回时间
        /// </summary>
        /// <param name="feedBackID">反馈单编号</param>
        /// <param name="cvtCode">变速箱型号</param>
        /// <param name="cvtID">变速箱编号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        bool UpdateFeedBackTime(string feedBackID, string cvtID,string cvtCode, out string error);

        /// <summary>
        /// 获得故障代码
        /// </summary>
        /// <returns>成功返回ID，失败返回null</returns>
        DataRow GetBugCodeByID(string bugName);

        /// <summary>
        /// 获得故障名称
        /// </summary>
        /// <returns>成功返回名称，失败返回null</returns>
        DataRow GetBugCodeByName(int bugName);

        /// <summary>
        /// 删除没有使用的单据
        /// </summary>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        bool DeleteBillStatus();

        /// <summary>
        /// 客服中心添加单据基础信息
        /// </summary>
        /// <param name="service">数据对象集</param>
        /// <param name="messageInfo">故障信息数据集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>添加成功返回True失败返回False</returns>
        bool InsertService(S_AfterService service, OF_BugMessageInfo messageInfo, out string error);

        /// <summary>
        /// 售后人员填写故障处理
        /// </summary>
        /// <param name="service">数据对象</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <param name="dt">明细数据集</param>
        /// <param name="messageInfo">故障信息数据集</param>
        /// <param name="flag">是否为接单状态，是：true，否：false</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        bool UpdateSaleTable(S_AfterService service, DataTable dt, OF_BugMessageInfo messageInfo,bool flag, out string error);

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
        bool UpdateCheckTable(string serviceID, string checkName, string checkTime, string fkBill, S_AfterService feedBack, 
            OF_BugMessageInfo messageInfo, string processResult,string diagnoseSituation, out string error);

        /// <summary>
        /// 客户回访，修改表
        /// </summary>
        /// <param name="service">数据对象</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>成功返回true失败返回False</returns>
        bool UpdateResultTable(S_AfterService service, out string error);

        /// <summary>
        /// 通过反馈单据号查找id
        /// </summary>
        /// <returns>成功返回一行数据集，失败返回null</returns>
        DataRow GetFeedBackID();

        /// <summary>
        /// 判断是否使用了用户信息
        /// </summary>
        /// <param name="chassisNum">车架号</param>
        /// <returns>成功返回整形，失败返回null</returns>
        DataRow GetAfterServicerByUserInfo(string chassisNum);

        /// <summary>
        /// 判断是否使用了用户信息
        /// </summary>
        /// <param name="chassisNum">车架号</param>
        /// <returns>成功返回整形，失败返回null</returns>
        DataRow GetServiceFeedBackByUserInfo(string chassisNum);

        /// <summary>
        /// 获取新的单据号
        /// </summary>
        /// <param name="checkOutGoodsType">物品类别</param>
        /// <returns>成功返回单号，失败返回错误信息</returns>
        string GetNextBillID(int checkOutGoodsType);

        /// <summary>
        /// 回退单据
        /// </summary>
        /// <param name="strDJH">单据号</param>
        /// <param name="strBillStatus">单据状态</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <param name="strRebackReason">回退原因</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        bool ReturnBill(string strDJH, string strBillStatus, out string error, string strRebackReason);

        /// <summary>
        /// 回退反馈单
        /// </summary>
        /// <param name="strDJH">反馈单号</param>
        /// <param name="strBillStatus">单据状态</param>
        /// <param name="strRebackReason">回退原因</param>
        /// <param name="YXBillNo">营销退库单号</param>
        /// <param name="error">错误信息</param>
        /// <returns>回退成功返回True失败返回False</returns>
        bool ReturnFeedBackBill(string strDJH, string strBillStatus, string strRebackReason, string YXBillNo, out string error);

        /// <summary>
        /// 通过单据号查询附件内容
        /// </summary>
        /// <param name="strDJH">单据号</param>
        /// <returns>成功返回满足条件的数据集，失败返回null的dataTable</returns>
        DataTable GetAfterServiceFile(string strDJH);

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
        bool UpdateYXCheckCreateManeuverBill(S_MaterialReturnedInTheDepot m_inTheDepotBill, S_MaterialListReturnedInTheDepot m_inTheDepotGoods, 
            S_MaterialRequisition m_requisitionBill, S_MaterialRequisitionGoods m_lnqGoods, ProductsCodes lnqList, Out_ManeuverBill maneuverBillInfo, 
            DataTable dtManeuverList, string ClientCode, out string m_strErr);

        /// <summary>
        /// 获得库存信息
        /// </summary>
        /// <param name="siteCode">服务站编号</param>
        /// <param name="goodsID">物品编号</param>
        /// <returns>成功返回整形，失败返回null</returns>
        DataTable GetStockNum(string siteCode, string goodsID);
    }
}

/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  CustomEntity.cs
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
using System.Text;
using System.Windows.Forms;

namespace GlobalObject
{
    public class Entity_BusinessOperationInfo
    {
        string _BusinessNo;

        public string BusinessNo
        {
            get { return _BusinessNo; }
            set { _BusinessNo = value; }
        }

        CE_BillTypeEnum _BillType;

        public CE_BillTypeEnum BillType
        {
            get { return _BillType; }
            set { _BillType = value; }
        }

        CE_OperatorMode _OperatorMode;

        public CE_OperatorMode OperatorMode
        {
            get { return _OperatorMode; }
            set { _OperatorMode = value; }
        }

        List<CommonProcessInfo> _FlowInfoList;

        public List<CommonProcessInfo> FlowInfoList
        {
            get { return _FlowInfoList; }
            set { _FlowInfoList = value; }
        }

        Dictionary<int, Dictionary<string, bool>> _FlowMagicDic;

        public Dictionary<int, Dictionary<string, bool>> FlowMagicDic
        {
            get { return _FlowMagicDic; }
            set { _FlowMagicDic = value; }
        }
    }

    public class Entity_AttendanceDateTimeRules
    {
        DateTime _DayDate;

        public DateTime DayDate
        {
            get { return _DayDate; }
            set { _DayDate = value; }
        }

        CE_HR_AttendanceExceptionType _StartExceptionType;

        public CE_HR_AttendanceExceptionType StartExceptionType
        {
            get { return _StartExceptionType; }
            set { _StartExceptionType = value; }
        }

        string _StartExceptionBillNo;

        public string StartExceptionBillNo
        {
            get { return _StartExceptionBillNo; }
            set { _StartExceptionBillNo = value; }
        }

        CE_HR_AttendanceExceptionType _EndExceptionType;

        public CE_HR_AttendanceExceptionType EndExceptionType
        {
            get { return _EndExceptionType; }
            set { _EndExceptionType = value; }
        }

        string _EndExceptionBillNo;

        public string EndExceptionBillNo
        {
            get { return _EndExceptionBillNo; }
            set { _EndExceptionBillNo = value; }
        }

        DateTime _StartTime;

        public DateTime StartTime
        {
            get { return _StartTime; }
            set { _StartTime = value; }
        }
        DateTime _EndTime;

        public DateTime EndTime
        {
            get { return _EndTime; }
            set { _EndTime = value; }
        }

        bool _StartRecode_IsExist;

        public bool StartRecode_IsExist
        {
            get { return _StartRecode_IsExist; }
            set { _StartRecode_IsExist = value; }
        }

        DateTime _StartRecord_Begin;

        public DateTime StartRecord_Begin
        {
            get { return _StartRecord_Begin; }
            set { _StartRecord_Begin = value; }
        }
        DateTime _StartRecode_End;

        public DateTime StartRecode_End
        {
            get { return _StartRecode_End; }
            set { _StartRecode_End = value; }
        }

        bool _EndRcord_IsExist;

        public bool EndRcord_IsExist
        {
            get { return _EndRcord_IsExist; }
            set { _EndRcord_IsExist = value; }
        }

        DateTime _EndRcord_Begin;

        public DateTime EndRcord_Begin
        {
            get { return _EndRcord_Begin; }
            set { _EndRcord_Begin = value; }
        }
        DateTime _EndRecod_End;

        public DateTime EndRecod_End
        {
            get { return _EndRecod_End; }
            set { _EndRecod_End = value; }
        }
    }

    public class Entity_AttendanceRecordTime
    {
        DateTime _RecordTime;


        public DateTime RecordTime
        {
            get { return _RecordTime; }
            set { _RecordTime = value; }
        }

    }

    /// <summary>
    /// 仓库查找条件
    /// </summary>
    public class QueryCondition_Store
    {
        int _GoodsID;

        public int GoodsID
        {
            get { return _GoodsID; }
            set { _GoodsID = value; }
        }

        string _StorageID;

        public string StorageID
        {
            get { return _StorageID; }
            set { _StorageID = value; }
        }

        string _GoodsCode;

        public string GoodsCode
        {
            get { return _GoodsCode; }
            set { _GoodsCode = value; }
        }

        string _GoodsName;

        public string GoodsName
        {
            get { return _GoodsName; }
            set { _GoodsName = value; }
        }

        string _Spec;

        public string Spec
        {
            get { return _Spec; }
            set { _Spec = value; }
        }

        string _Provider;

        public string Provider
        {
            get { return _Provider; }
            set { _Provider = value; }
        }

        string _BatchNo;

        public string BatchNo
        {
            get { return _BatchNo; }
            set { _BatchNo = value; }
        }

        string _Depot;

        public string Depot
        {
            get { return _Depot; }
            set { _Depot = value; }
        }

        string _ShelfArea;

        public string ShelfArea
        {
            get { return _ShelfArea; }
            set { _ShelfArea = value; }
        }

        string _ColumnNumber;

        public string ColumnNumber
        {
            get { return _ColumnNumber; }
            set { _ColumnNumber = value; }
        }

        string _LayerNumber;

        public string LayerNumber
        {
            get { return _LayerNumber; }
            set { _LayerNumber = value; }
        }
    }

    public class FormInfo
    {
        ContainerControl _ContainerControl;

        public ContainerControl ContainerControl
        {
            get { return _ContainerControl; }
            set { _ContainerControl = value; }
        }

        object _InfoMessage;

        public object InfoMessage
        {
            get { return _InfoMessage; }
            set { _InfoMessage = value; }
        }

    }

    public class PersonnelBasicInfo
    {
        string _工号;

        public string 工号
        {
            get { return _工号; }
            set { _工号 = value; }
        }

        string _姓名;

        public string 姓名
        {
            get { return _姓名; }
            set { _姓名 = value; }
        }


        string _角色;

        public string 角色
        {
            get { return _角色; }
            set { _角色 = value; }
        }
    }

    public class NotifyPersonnelInfo
    {
        string _userType;

        public string UserType
        {
            get { return _userType; }
            set { _userType = value; }
        }

        List<PersonnelBasicInfo> _personnelBasicInfoList;

        public List<PersonnelBasicInfo> PersonnelBasicInfoList
        {
            get { return _personnelBasicInfoList; }
            set { _personnelBasicInfoList = value; }
        }
    }

    public class CommonProcessInfo
    {
        string _工号;

        public string 工号
        {
            get { return _工号; }
            set { _工号 = value; }
        }

        string _操作节点;

        public string 操作节点
        {
            get { return _操作节点; }
            set { _操作节点 = value; }
        }

        string _人员;

        public string 人员
        {
            get { return _人员; }
            set { _人员 = value; }
        }

        string _时间;

        public string 时间
        {
            get { return _时间; }
            set { _时间 = value; }
        }

        string _意见;

        public string 意见
        {
            get { return _意见; }
            set { _意见 = value; }
        }
    }

    public class BarCodeInfo
    {
        int _GoodsID;

        public int GoodsID
        {
            get { return _GoodsID; }
            set { _GoodsID = value; }
        }

        string _GoodsCode;

        public string GoodsCode
        {
            get { return _GoodsCode; }
            set { _GoodsCode = value; }
        }

        string _GoodsName;

        public string GoodsName
        {
            get { return _GoodsName; }
            set { _GoodsName = value; }
        }

        string _Spec;

        public string Spec
        {
            get { return _Spec; }
            set { _Spec = value; }
        }

        string _BatchNo;

        public string BatchNo
        {
            get { return _BatchNo; }
            set { _BatchNo = value; }
        }

        decimal _Count;

        public decimal Count
        {
            get { return _Count; }
            set { _Count = value; }
        }

        string _Remark;

        public string Remark
        {
            get { return _Remark; }
            set { _Remark = value; }
        }

        string _CarModel;

        public string CarModel
        {
            get { return _CarModel; }
            set { _CarModel = value; }
        }

        string _Version;

        public string Version
        {
            get { return _Version; }
            set { _Version = value; }
        }

    }

    public class GoodsInfo
    {
        int _GoodsID;

        public int GoodsID
        {
            get { return _GoodsID; }
            set { _GoodsID = value; }
        }

        string _BatchNo;

        public string BatchNo
        {
            get { return _BatchNo; }
            set { _BatchNo = value; }
        }

        string _Provider;

        public string Provider
        {
            get { return _Provider; }
            set { _Provider = value; }
        }

        decimal _GoodsCount;

        public decimal GoodsCount
        {
            get { return _GoodsCount; }
            set { _GoodsCount = value; }
        }

        List<string> _ListInfo;

        public List<string> ListInfo
        {
            get { return _ListInfo; }
            set { _ListInfo = value; }
        }

    }

}

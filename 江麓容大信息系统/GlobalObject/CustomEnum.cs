/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  Enum.cs
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

namespace GlobalObject
{
    public enum CE_SystemName
    {
        泸州容大,
        湖南容大
    }

    public enum CE_HR_Train_QuesitonsType
    {
        判断题,
        单选题,
        多选题
    }

    public enum CE_MonthValue
    {
        一月 = 1,
        二月 = 2,
        三月 = 3,
        四月 = 4,
        五月 = 5,
        六月 = 6,
        七月 = 7,
        八月 = 8,
        九月 = 9,
        十月 = 10,
        十一月 = 11,
        十二月 = 12
    }

    public enum CE_WorkShop_BatchNoChangeType
    {
        先进先出,
        单个,
        批次
    }

    public enum CE_HR_Train_PlanType
    {
        年度培训计划,
        临时培训计划
    }

    public enum CE_PickingPurposeProperty
    {
        破坏性检测,
        三包外维修,
        三包外装配,
        盘点
    }

    public enum CE_CarModelType
    {
        传统车型,
        新能源车型
    }

    public enum CE_BarCodeType
    {
        内部钢印码 = 1,
        出厂条形码 = 2
    }

    public enum CE_LV
    {
        A = 1,
        B = 2,
        C = 3,
        D = 4,
        E = 5,
        F = 6
    }

    public enum CE_CW_BasicParameters
    {
        核算_科目,
        核算_用途,
        核算_库房,
        核算_科目与用途关系,
        预算_科目
    }

    public enum CE_FormFlowType
    {
        默认,
        自定义
    }

    public enum CE_RoleStyleType
    {
        仓管,
        负责人,
        分管领导,
        主管,
        上级领导,
        所有上级领导,
        节点人,
        未知
    }

    public enum CE_CommunicationMode
    {
        Socket,
        FTP
    }

    public enum CE_StockGoodsStatus
    {
        正常 = 0,
        正在挑选 = 1,
        样品 = 2,
        隔离 = 3,
        报废 = 4,
        待处理 = 5,
        仅限于返修箱用 = 6,
        仅限于售后备件 = 7,
        未知 = 999
    }

    public enum CE_HR_WaterInfoType
    {
        上班正常,
        迟到,
        上班旷工,
        旷工,
        下班旷工,
        早退,
        下班正常,
        上班日中间加班,
        上班日上班前加班,
        上班日下班后加班,
        节假日加班,
        考勤正常
    }

    public enum CE_HR_SchedulingType
    {
        公休,
        保安白班,
        保安中班,
        保安晚班,
        车间白班,
        车间中班,
        车间晚班,
        食堂正常班,
        电工白班,
        电工晚班,
        电工24小时班,
        实验白班,
        实验晚班
    }

    public enum CE_HR_LeaveType
    {
        调休,
        婚假,
        晚婚假,
        病假_带薪,
        医疗期,
        年假_10年内,
        孕检假_3至6月内,
        孕检假_6月以上,
        丧假_3天,
        产假,
        陪产假,
        陪产假_晚婚晚育,
        考试假,
        事假,
        其它,
        车间调休,
        年假_10年以上,
        丧假_4天
    }

    public enum CE_HR_AttendanceScheme
    {
        自然月考勤,
        非自然月考勤,
        自然月排班考勤,
        非自然月排班考勤,
        不考勤,
        夏令时,
        冬令时
    }

    public enum CE_HR_AttendanceExceptionType
    {
        应考勤 = 0,
        迟到 = 1,
        早退 = 2,
        旷工 = 3,
        请假 = 4,
        加班 = 5,
        出差 = 7,
        漏打卡 = 8,
        正常 = 9,
        已登记 = 10,
        外出办事 = 11,
        哺乳期 = 12,
        集体异常 = 13,
        节假 = 14
    }

    /// <summary>
    /// 员工状态
    /// </summary>
    public enum CE_HR_PersonnelStatus
    {
        在职 = 1,
        离职 = 3,
        退休 = 4,
        全部 = 0
    }

    /// <summary>
    /// 扣货单操作模式
    /// </summary>
    public enum CE_BusinessOperateMode 
    { 
        新建,
        查看, 
        修改, 
        采购确认,
        仓库核实
    }

    /// <summary>
    /// DataGridView记录选择类型
    /// </summary>
    public enum CE_DataGridViewItemsType
    {
        全部项,
        选中项,
        上一条,
        下一条
    }
    
    /// <summary>
    /// 库房名称枚举
    /// </summary>
    public enum CE_StorageName
    {
        制造库房 = 1,
        原材料库 = 101,
        产成品库 = 102,
        成品库房 = 2,
        电子元器件库房 = 3,
        备件库房 = 4,
        低辅料库 = 104,
        售后库房 = 5,
        油品库 = 6,
        量检具库 = 7,
        自制半成品库 = 8,
        半成品库 = 108,
        售后配件库房 = 9,
        受托品库房 = 10,
        材料库 = 11,
        原材料样品库 = 12,
        自制半成品样品库 = 13
    }

    /// <summary>
    /// 业务枚举
    /// </summary>
    public enum CE_MarketingType
    {
        /// <summary>
        /// 入库
        /// </summary>
        入库,

        /// <summary>
        /// 出库
        /// </summary>
        出库,

        /// <summary>
        /// 退库
        /// </summary>
        退库,

        /// <summary>
        /// 返修
        /// </summary>
        返修,

        /// <summary>
        /// 退货
        /// </summary>
        退货,

        /// <summary>
        /// 领料
        /// </summary>
        领料,

        /// <summary>
        /// 领料退库
        /// </summary>
        领料退库,

        /// <summary>
        /// 调拨
        /// </summary>
        调入,

        /// <summary>
        /// 调拨
        /// </summary>
        调出,

        /// <summary>
        /// 报废
        /// </summary>
        库房报废,

        /// <summary>
        /// 未知
        /// </summary>
        未知
    }

    /// <summary>
    /// 账务类型
    /// </summary>
    public enum CE_SubsidiaryOperationType
    {
        领料 = 1,
        领料退库 = 2,
        自制件入库 = 3,
        营销退货 = 4,
        营销入库 = 5,
        车间耗用 = 6,
        车间调入 = 7,
        车间调出 = 8,
        物料转换前 = 9,
        物料转换后 = 10,
        车间盘盈 = 11,
        车间盘亏 = 12,
        自制件退货 = 13,
        自制件工装入库 = 14,
        报废 = 15,
        借货 = 16,
        样品耗用 = 17,
        报检入库 = 18,
        委外报检入库 = 19,
        普通入库 = 20,
        采购退货 = 21,
        营销出库 = 22,
        营销退库 = 23,
        还货 = 24,
        库房盘盈 = 25,
        库房盘亏 =26,
        库房调入 = 27,
        库房调出 = 28,
        扣货 = 29,
        财务调盈 = 30,
        财务调亏 = 31,
        财务红冲 = 32,
        财务对冲 = 33,
        财务调整 = 34,
        未知 = 100
    }

    public enum CE_SwitchName
    {
        检测完成装配 = 1,
        检测钢印码及条形码 = 2,
        检测业务规范 = 3,
        开启车间管理模块 = 4,
        月结日 = 5,
        月结日推算方式 = 6, 
        开启工具台帐管理 = 7,
        工具类别编码 = 8,
        量检具类别编码 = 9,
        开启借还货账务管理 = 11,
        借货账目允许小于0 = 12,
        公告内容 = 13,
        文件传输方式 = 14
    }

    public enum CE_FlowOperationType
    {
        提交,
        暂存,
        回退,
        未知
    }

    public enum CE_GoodsType
    {
        CVT,
        TCU,
        工装,
        量检具,
        工具,
        零件,
        未知物品
    }

    public enum CE_BusinessType
    {
        库房业务,
        车间业务,
        综合业务,
        未知
    }

    /// <summary>
    /// 操作方式
    /// </summary>
    public enum CE_OperatorMode
    {
        添加,

        修改,

        删除,

        查看,

        编辑,

        保存,

        批量添加,

        批量删除
    }

    /// <summary>
    /// 文件类型
    /// </summary>
    public enum CE_SystemFileType
    {
        Word,

        Excel,

        Excel2010,

        PPT,

        PDF,

        Miss
    }

    public enum CE_FileType
    {
        体系文件 = 0,
        制度文件 = 1
    }

    /// <summary>
    /// 车间代码
    /// </summary>
    public enum CE_WorkShopCode
    {
        /// <summary>
        /// 机加车间
        /// </summary>
        JJCJ,

        /// <summary>
        /// 装配车间
        /// </summary>
        ZPCJ,
        
        /// <summary>
        /// 下线车间
        /// </summary>
        XXCJ,

        /// <summary>
        /// TCU车间
        /// </summary>
        TCUCJ
    }

    /// <summary>
    /// 批次号前缀
    /// </summary>
    public enum CE_BatchNoPrefix
    {
        /// <summary>
        /// 报检入库件
        /// </summary>
        BJD,
        /// <summary>
        /// 委外件
        /// </summary>
        WJD,
        /// <summary>
        /// 回收件
        /// </summary>
        HJD,
        /// <summary>
        /// 自制件
        /// </summary>
        ZRD,
        /// <summary>
        /// 普通入库件
        /// </summary>
        PR,
        /// <summary>
        /// 样品
        /// </summary>
        CGY,
        /// <summary>
        /// 售后返修——拆箱
        /// </summary>
        FT,
        /// <summary>
        /// 0公里返修或者内部返修——拆箱
        /// </summary>
        ZT,
        /// <summary>
        /// 维修件
        /// </summary>
        WT
    }

    public enum CE_ScreenType
    {
        数字,
        字母,
        汉字
    }

    public enum CE_BusinessBillType
    {
        领料 = 0,
        营销出库 = 0,
        库房调出 = 0,

        入库 = 1,
        领料退库 = 1,
        营销退库 = 1,
        库房调入 = 1
    }

    public enum CE_ScrapProviderType
    {
        责任供应商,
        供应商
    }

    public enum CE_GoodsAttributeName
    {
        生产耗用 = 1,
        防锈期 = 2,
        保质期 = 3,
        安全库存 = 4,
        最高库存 = 5,
        毛坯 = 6,
        缺料计算考虑毛坯 = 7,
        物料价值ABC分类 = 8,
        技术等级ABC分类 = 9,
        来料须依据检验结果入库 = 10,
        采购件 = 11,
        自制件 = 12,
        委外加工 = 13,
        部件 = 14,
        零件 = 15,
        选配件 = 16,
        替换件 = 17,
        CVT = 18,
        TCU = 19,
        虚拟件 = 20,
        领用上限 = 21,
        整包发料 = 22,
        刀具寿命 = 23,
        流水码 = 24,
        厂商编码 = 29,
        停产 = 30,
        装箱数 = 31
    }

    public enum CE_FileOperatorType
    {
        上传,
        下载,
        在线编辑,
        在线阅读,
        无操作
    }

    public enum CE_CommonBillStatus
    {
        新建单据,
        等待审核,
        等待复审,
        等待评审,
        等待检验,
        等待试验,
        等待试装,
        等待验证,
        等待判定,
        等待校对,
        等待通过,
        等待确认,
        等待处理,
        单据完成
    }

    public enum CE_CommonFlowName
    {
        新建,
        审核,
        批准,
        确认,
        评审,
        处理,
        通过,
        校对,
        校验,
        检验,
        试验,
        判定,
        评价,
        完成
    }

    public enum CE_InPutBusinessType
    {
        生产采购,
        普通采购,
        委外采购,
        样品采购,
        自制件入库,
        领料退库,
        营销入库,
        营销退库
    }

    public enum CE_OutPutBusinessType
    {
        领料,
        营销出库,
        营销退货,
        采购退货,
        自制件退货
    }

    /// <summary>
    /// 单据类型
    /// </summary>
    public enum CE_BillTypeEnum
    {
        未知单据,
        合同管理,
        报检入库单,
        普通入库单,
        借货单,
        领料单,
        报废单,
        采购退货单,
        领料退库单,
        产品入库单,
        自制件入库单,
        销售出库单,
        销售退库单,
        营销出库单,
        营销入库单,
        营销退货单,
        营销退库单,
        不合格品隔离处置单,
        技术变更单,
        库房调拨单,
        库房盘点单,
        生产计划,
        挑选返工返修单,
        样品耗用单,
        样品确认申请单,
        CVT出厂检验记录表,
        售后服务备件制造申请单,
        营销要货计划,
        委外报检入库单,
        产品变更单,
        自制件退货单,
        工装验证报告单,
        物料扣货单,
        三包外返修处理单,
        自制件工装报检,
        下线不合格品放行单,
        售后配件申请单,
        调运单,
        设备维修安装申请单,
        非产品件检验单,
        文件审查流程,
        工装台帐,
        质量问题整改处置单,
        文件发布流程,
        文件修订废止申请单,
        文件发放回收登记表,
        车间耗用单,
        车间调运单,
        车间物料转换单,
        车间异常信息记录表,
        供应质量信息反馈单,
        售后函电处理单,
        售后服务质量反馈单,
        销售清单,
        技术变更处置单,
        零星采购单,
        制度审查流程,
        制度发布流程,
        制度修订废弃申请流程,
        制度销毁申请流程,
        发料清单,
        物料录入申请单,
        还货单,
        质量数据库,
        测试业务,
        入库申请单,
        到货单,
        入库单,
        检验报告,
        判定报告,
        出库申请单,
        出库单,
        整台份请领单,
        采购结算单,
        BOM变更单,
        供应商与零件归属变更单,
        请假单,
        出差单,
        加班单,
        创意提案,
        TCU软件升级,
        IT运维申请单,
        培训计划申请表,
        车间批次管理变更,
        年度预算申请表,
        月度预算申请表,
        供应商应付账款,
        重点工作,
        纠正预防措施报告
    }

    /// <summary>
    /// 角色枚举
    /// </summary>
    public enum CE_RoleEnum
    {
        业务系统管理员 = 10000,
        流程监控器 = 9000,

        /// <summary>
        /// 根据角色数据范围可以用于权限控制, 0-99为高管, 100-199为主管级
        /// </summary>
        总经理 = 0,
        常务副总经理,
        副总经理,
        技术总监,
        财务副总,
        质量总监,
        营销总监,
        营销分管领导,
        外协分管领导,
        制造分管领导,
        采购分管领导,
        产品开发分管领导,
        行政分管领导,
        财务分管领导,
        质管分管领导,
        公司办分管领导,
        新能源开发分管领导,
        生产管理分管领导,

        质控负责人,
        制造负责人,
        采购负责人,
        营销负责人,
        产品开发负责人,
        财务负责人,
        行政负责人,
        公司办负责人,
        设计部负责人,
        电控部负责人,
        实验室负责人,
        新能源开发负责人,
        工艺负责人,
        工艺组长,
        生产管理部负责人,

        质控主管 = 100,
        制造主管,
        采购主管,
        营销主管,
        产品开发主管,
        财务主管,
        行政主管,
        下线主管,
        TCU主管,
        人力资源主管,
        经营主管,
        质检室主管,


        制造仓库管理员 = 200,
        营销仓库管理员,
        备件仓库管理员,
        成品仓库管理员,
        量检具库管理员,
        自制半成品库管理员,
        受托品库房管理员,
        售后配件库管理员,
        油品库管理员,
        售后库房管理员,
        电子元器件仓库管理员,
        样品库管理员,
        材料库管理员,
        制造仓库收货员,
        下线车间管理员,
        机加车间管理员,
        装配车间管理员,
        TCU车间管理员,
        制造仓库二管理员,
        成品仓库二管理员,
        自制半成品库二管理员,
        物流公司,


        采购员 = 300,
        采购部办公室文员,
        SQE组长,
        SQE组员,
        采购账务管理员,

        质量工程师 = 400,
        IQC质量工程师,
        体系工程师,
        计量工程师,
        机械检验组长,
        电子检验组长,
        自制件检验组长,
        计划管理科科长,
        检验员,
        工艺人员,
        质检员_装配,
        质检员_下线,
        质检员_机加,
        质检员_售后,
        质检员_委外,
        质检员_工艺,
        质检员_设备,
        质检员_出厂,

        会计 = 600,

        班组长 = 700,
        制造信息员,
        工装管理员,
        装配用工装管理员,
        机加用工装管理员,
        下线用工装管理员,
        机加操作员,
        设备组员,
        设备组长,

        市场助理 = 800,

        电控组长 = 900,
        设计组长,
        零件工程师,
        项目经理,

        普通操作员 = 1000,
        营销发货员,
        营销收货员,
        下线车间人员,
        样品领料员,
        营销普通人员,
        营销售后客户回访人员,

        制造部办公室文员 = 2000,
        下线信息员,
        营销部办公室文员,
        开发部办公室文员,
        质管部办公室文员,
        计划管理员_采购,
        人力资源部办公室文员,
        基础物品管理员_采购,
        基础物品管理员_财务,
        基础物品管理员_工艺,
        基础物品管理员_技术,
        基础物品管理员_生管,
        基础物品管理员_质量,
        生产管理部人员,



        未知 = 10000
    }
    
    /// <summary>
    /// 采购结算单类别
    /// </summary>
    public enum CE_ProcurementStatementBillTypeEnum
    {
        CVT零部件,
        零星采购加工,
        委托加工物资
    }

    /// <summary>
    /// 发票类别
    /// </summary>
    public enum CE_InvoiceTypeEnum
    {
        普通发票,
        专用发票
    }

    /// <summary>
    /// 发料适用范围
    /// </summary>
    public enum CE_DebitScheduleApplicable
    {
        正常装配,
        售后配件,
        售后返修
    }

    public static class EnumOperation
    {
        public static CE_MarketingType InPutBusinessTypeConvertToMarketingType(CE_InPutBusinessType inPutType)
        {
            CE_MarketingType result = CE_MarketingType.未知;

            switch (inPutType)
            {
                case CE_InPutBusinessType.生产采购:
                case CE_InPutBusinessType.普通采购:
                case CE_InPutBusinessType.委外采购:
                case CE_InPutBusinessType.样品采购:
                case CE_InPutBusinessType.自制件入库:
                case CE_InPutBusinessType.营销入库:
                    result = CE_MarketingType.入库;
                    break;
                case CE_InPutBusinessType.领料退库:
                    result = CE_MarketingType.领料退库;
                    break;
                case CE_InPutBusinessType.营销退库:
                    result = CE_MarketingType.退库;
                    break;
                default:
                    break;
            }

            return result;
        }

        public static CE_SubsidiaryOperationType InPutBusinessTypeConvertToSubsidiaryOperationType(CE_InPutBusinessType inPutType)
        {
            CE_SubsidiaryOperationType subsdiaryType = CE_SubsidiaryOperationType.未知;

            switch (inPutType)
            {
                case CE_InPutBusinessType.生产采购:
                case CE_InPutBusinessType.样品采购:
                    subsdiaryType = CE_SubsidiaryOperationType.报检入库;
                    break;
                case CE_InPutBusinessType.委外采购:
                    subsdiaryType = CE_SubsidiaryOperationType.委外报检入库;
                    break;
                case CE_InPutBusinessType.普通采购:
                    subsdiaryType = CE_SubsidiaryOperationType.普通入库;
                    break;
                case CE_InPutBusinessType.自制件入库:
                    subsdiaryType = CE_SubsidiaryOperationType.自制件入库;
                    break;
                case CE_InPutBusinessType.营销入库:
                    subsdiaryType = CE_SubsidiaryOperationType.营销入库;
                    break;
                case CE_InPutBusinessType.领料退库:
                    subsdiaryType = CE_SubsidiaryOperationType.领料退库;
                    break;
                case CE_InPutBusinessType.营销退库:
                    subsdiaryType = CE_SubsidiaryOperationType.营销退库;
                    break;
                default:
                    break;
            }

            return subsdiaryType;
        }

        public static CE_MarketingType OutPutBusinessTypeConvertToMarketingType(CE_OutPutBusinessType outPutType)
        {
            CE_MarketingType result = CE_MarketingType.未知;

            switch (outPutType)
            {
                case CE_OutPutBusinessType.领料:
                    result = CE_MarketingType.领料;
                    break;
                case CE_OutPutBusinessType.营销出库:
                    result = CE_MarketingType.出库;
                    break;
                case CE_OutPutBusinessType.营销退货:
                case CE_OutPutBusinessType.采购退货:
                case CE_OutPutBusinessType.自制件退货:
                    result = CE_MarketingType.退货;
                    break;
                default:
                    break;
            }

            return result;
        }

        public static CE_SubsidiaryOperationType OutPutBusinessTypeConvertToSubsidiaryOperationType(CE_OutPutBusinessType outPutType)
        {
            CE_SubsidiaryOperationType subsdiaryType = CE_SubsidiaryOperationType.未知;

            switch (outPutType)
            {
                case CE_OutPutBusinessType.领料:
                    subsdiaryType = CE_SubsidiaryOperationType.领料;
                    break;
                case CE_OutPutBusinessType.营销出库:
                    subsdiaryType = CE_SubsidiaryOperationType.营销出库;
                    break;
                case CE_OutPutBusinessType.营销退货:
                    subsdiaryType = CE_SubsidiaryOperationType.营销退货;
                    break;
                case CE_OutPutBusinessType.采购退货:
                    subsdiaryType = CE_SubsidiaryOperationType.采购退货;
                    break;
                case CE_OutPutBusinessType.自制件退货:
                    subsdiaryType = CE_SubsidiaryOperationType.自制件退货;
                    break;
                default:
                    break;
            }

            return subsdiaryType;
        }
    }
}

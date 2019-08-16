/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  Socket_FittingAccessoryInfo.cs
 * 作者    :  夏石友    版本: v1.00    日期: 2010/07/17
 * 开发平台:  Visual C# 2008
 * 用于    :  仓库管理软件
 *----------------------------------------------------------------------------
 * 描述 : 
 * 其它 :
 *----------------------------------------------------------------------------
 * 公共信息: 参见系统'类帮助文档'
 *----------------------------------------------------------------------------
 * 历史记录:
 *     1. 日期时间: 2010/07/17 8:54:12 作者: 夏石友 当前版本: V1.00
 *        修改说明: 创建
 ******************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SocketCommDefiniens
{
    /// <summary>
    /// 用于通信的装配零件信息分类
    /// </summary>
    public class Socket_FittingAccessoryInfo
    {
        /// <summary>
        /// 操作状态枚举
        /// </summary>
        public enum OperateStateEnum
        {
            条形码有误,
            无法获取Bom基数,
            电子档案临时表中无此信息无法覆盖,
            操作成功,
            获取电子档案临时表失败,
            存储装配信息失败,
            获取选配信息失败,
            该零件非选配零件,
            该工位无选配零件
        }

        /// <summary>
        /// 条形码
        /// </summary>
        string m_barCode;

        /// <summary>
        /// 设置或获取条形码
        /// </summary>
        public string BarCode
        {
            get { return m_barCode; }
            set { m_barCode = value; }
        }

        /// <summary>
        /// 父总成编码
        /// </summary>
        string m_parentCode;

        /// <summary>
        /// 设置或获取父总成编码
        /// </summary>
        public string ParentCode
        {
            get { return m_parentCode; }
            set { m_parentCode = value; }
        }

        /// <summary>
        /// 零部件编码
        /// </summary>
        string m_goodsCode;

        /// <summary>
        /// 设置或获取零部件编码
        /// </summary>
        public string GoodsCode
        {
            get { return m_goodsCode; }
            set { m_goodsCode = value; }
        }

        /// <summary>
        /// 规格
        /// </summary>
        string m_spec;

        /// <summary>
        /// 设置或获取规格
        /// </summary>
        public string Spec
        {
            get { return m_spec; }
            set { m_spec = value; }
        }

        /// <summary>
        /// 零部件名称
        /// </summary>
        string m_goodsName;

        /// <summary>
        /// 设置或获取零部件名称
        /// </summary>
        public string GoodsName
        {
            get { return m_goodsName; }
            set { m_goodsName = value; }
        }

        /// <summary>
        /// 零件标识码
        /// </summary>
        string m_goodsOnlyCode;

        /// <summary>
        /// 设置或获取零件标识码
        /// </summary>
        public string GoodsOnlyCode
        {
            get { return m_goodsOnlyCode; }
            set { m_goodsOnlyCode = value; }
        }

        /// <summary>
        /// 数量
        /// </summary>
        int m_counts;

        /// <summary>
        /// 设置或获取数量
        /// </summary>
        public int Counts
        {
            get { return m_counts; }
            set { m_counts = value; }
        }

        /// <summary>
        /// 供应商
        /// </summary>
        string m_provider;

        /// <summary>
        /// 设置或获取供应商
        /// </summary>
        public string Provider
        {
            get { return m_provider; }
            set { m_provider = value; }
        }

        /// <summary>
        /// 批次号
        /// </summary>
        string m_batchCode;

        /// <summary>
        /// 设置或获取批次号
        /// </summary>
        public string BatchNo
        {
            get { return m_batchCode; }
            set { m_batchCode = value; }
        }

        /// <summary>
        /// 检测数据
        /// </summary>
        string m_checkData;

        /// <summary>
        /// 设置或获取检测数据
        /// </summary>
        public string CheckData
        {
            get { return m_checkData; }
            set { m_checkData = value; }
        }

        /// <summary>
        /// 实际数据
        /// </summary>
        string m_factData;

        /// <summary>
        /// 设置或获取实际数据
        /// </summary>
        public string FactData
        {
            get { return m_factData; }
            set { m_factData = value; }
        }

        /// <summary>
        /// 备注
        /// </summary>
        string m_remark;

        /// <summary>
        /// 设置或获取备注
        /// </summary>
        public string Remark
        {
            get { return m_remark; }
            set { m_remark = value; }
        }

        /// <summary>
        /// 操作状态枚举
        /// </summary>
        OperateStateEnum m_operateStateEnum;

        /// <summary>
        /// 设置或获取操作状态枚举
        /// </summary>
        public OperateStateEnum OperateState
        {
            get { return m_operateStateEnum; }
            set { m_operateStateEnum = value; }
        }

        /// <summary>
        /// 覆盖标志(True表示覆盖原信息,False表示不覆盖原信息)
        /// </summary>
        bool m_overlayFlag;

        /// <summary>
        /// 设置或获取覆盖标志(True表示覆盖原信息,False表示不覆盖原信息)
        /// </summary>
        public bool OverlayFlag
        {
            get { return m_overlayFlag; }
            set { m_overlayFlag = value; }
        }

        /// <summary>
        /// 总成标志(0表示GoodsCode编码的零件为非总成,1表示GoodsCode编码的零件总成)
        /// </summary>
        int m_assemblyFlag;

        /// <summary>
        /// 设置或获取总成标志(0表示GoodsCode编码的零件为非总成,1表示GoodsCode编码的零件总成)
        /// </summary>
        public int AssemblyFlag
        {
            get { return m_assemblyFlag; }
            set { m_assemblyFlag = value; }
        }

        /// <summary>
        /// 分总成完成时间
        /// </summary>
        string m_finishTime;

        /// <summary>
        /// 设置或获取分总成完成时间
        /// </summary>
        public string FinishTime
        {
            get { return m_finishTime; }
            set { m_finishTime = value; }
        }

        /// <summary>
        /// 选配信息类
        /// </summary>
        Sock_ChoseMatchInfo m_choseMatchInfo;

        /// <summary>
        /// 设置或获取选配信息类
        /// </summary>
        public Sock_ChoseMatchInfo ChoseMatchInfo
        {
            get { return m_choseMatchInfo; }
            set { m_choseMatchInfo = value; }
        }
    }

    /// <summary>
    /// 选配信息类
    /// </summary>
    public class Sock_ChoseMatchInfo
    {
        /// <summary>
        /// 选配值
        /// </summary>
        List<string> m_choseMatchData;

        /// <summary>
        /// 设置或获取选配值
        /// </summary>
        public List<string> ChoseMatchData
        {
            get { return m_choseMatchData; }
            set { m_choseMatchData = value; }
        }

        /// <summary>
        /// 选配范围最小值列表
        /// </summary>
        List<double> m_minDataList;

        /// <summary>
        /// 选配范围最小值列表
        /// </summary>
        public List<double> MinDataList
        {
            get { return m_minDataList; }
            set { m_minDataList = value; }
        }

        /// <summary>
        /// 选配范围最大值列表
        /// </summary>
        List<double> m_maxDataList;

        /// <summary>
        /// 选配范围最大值列表
        /// </summary>
        public List<double> MaxDataList
        {
            get { return m_maxDataList; }
            set { m_maxDataList = value; }
        } 
    }
}

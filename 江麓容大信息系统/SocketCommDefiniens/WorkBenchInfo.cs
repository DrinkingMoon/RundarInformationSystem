using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SocketCommDefiniens
{
    /// <summary>
    /// 工位类型枚举
    /// </summary>
    public enum WorkbenchTypeEnum { 分装, 总装 };

    /// <summary>
    /// 产品信息
    /// </summary>
    public struct ProductInfo
    {
        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProduceCode;

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProduceName;
    }

    /// <summary>
    /// 工位零件信息
    /// </summary>
    public struct WorkbenchPartInfo
    {
        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProduceName;

        /// <summary>
        /// 父总成编码
        /// </summary>
        public string ParentCode;

        /// <summary>
        /// 父总成名称
        /// </summary>
        public string ParentName;

        /// <summary>
        /// 零件编号
        /// </summary>
        public string PartCode;

        /// <summary>
        /// 零件名称
        /// </summary>
        public string PartName;

        /// <summary>
        /// 规格
        /// </summary>
        public string Spec;

        /// <summary>
        /// 装配数量
        /// </summary>
        public int Amount;
    }

    /// <summary>
    /// 工位包含的装配信息
    /// </summary>
    public class Socket_WorkBenchInfo
    {
        /// <summary>
        /// 操作状态枚举
        /// </summary>
        public enum ReturnStateEnum
        {
            超时,
            操作成功,
            获取工位初始化信息失败
        }

        /// <summary>
        /// 操作状态
        /// </summary>
        public ReturnStateEnum OperateState;

        /// <summary>
        /// 错误信息
        /// </summary>
        public String ErrorInfo;

        /// <summary>
        /// 是否第一个总装工位的标志
        /// </summary>
        public bool IsFirstWorkBenchInAssemblingLine;

        /// <summary>
        /// 是否最后一个总装工位的标志
        /// </summary>
        public bool IsLastWorkBenchAssemblingLine;

        /// <summary>
        /// 工位
        /// </summary>
        string m_workBench;

        /// <summary>
        /// 获取或设置工位
        /// </summary>
        public string WorkBench
        {
            get { return m_workBench; }
            set { m_workBench = value; }
        }

        /// <summary>
        /// 工位类型
        /// </summary>
        string m_workBenchType;

        /// <summary>
        /// 获取或设置工位类型
        /// </summary>
        public string WorkBenchType
        {
            get { return m_workBenchType; }
            set { m_workBenchType = value; }
        }

        /// <summary>
        /// 本工位能装配的产品类型名称,用分隔符隔开
        /// </summary>
        string m_productName;

        /// <summary>
        /// 获取或设置本工位能装配的产品类型名称,用分隔符隔开
        /// </summary>
        public string ProductName
        {
            get { return m_productName; }
            set { m_productName = value; }
        }

        ///// <summary>
        ///// 产品编码
        ///// </summary>
        //string m_productCode;

        ///// <summary>
        ///// 获取或设置产品编码
        ///// </summary>
        //public string ProductCode
        //{
        //    get { return m_productCode; }
        //    set { m_productCode = value; }
        //}

        /// <summary>
        /// 本位的所有分总成,用分隔符隔开
        /// </summary>
        string m_fzc;

        /// <summary>
        /// 获取或设置本工位所有分总成名称,用分隔符隔开
        /// </summary>
        public string FZC
        {
            get { return m_fzc; }
            set { m_fzc = value; }
        }

        /// <summary>
        /// 本工位可以完成的分总成,用分隔符隔开
        /// </summary>
        string m_finishedFzc;

        /// <summary>
        /// 获取或设置本工位可以完成的分总成,用分隔符隔开
        /// </summary>
        public string FinishedFZC
        {
            get { return m_finishedFzc; }
            set { m_finishedFzc = value; }
        }

        /// <summary>
        /// 本工位当前产品需装配零件
        /// </summary>
        public List<WorkbenchPartInfo> WorkBenchParts;
    }
}

/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  Socket_FetchMaterial.cs
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
using System.Text;

namespace SocketCommDefiniens
{
    /// <summary>
    /// 用于通信的领料信息类
    /// </summary>
    public class Socket_FetchMaterial
    {
        /// <summary>
        /// 操作状态枚举
        /// </summary>
        public enum FetchStateEnum
        {
            操作成功,
            更新领料清单失败,
            条形码有误,
            单据号有误,
            领料单中无该零件的领料信息
        }

        /// <summary>
        /// 领料单据号
        /// </summary>
        string m_billID;

        /// <summary>
        /// 设置或获取领料单据号
        /// </summary>
        public string BillID
        {
            get { return m_billID; }
            set { m_billID = value; }
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
        /// 图号型号
        /// </summary>
        string m_goodsCode;

        /// <summary>
        /// 设置或获取图号型号
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
        /// 名称
        /// </summary>
        string m_goodsName;

        /// <summary>
        /// 设置或获取名称
        /// </summary>
        public string GoodsName
        {
            get { return m_goodsName; }
            set { m_goodsName = value; }
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
        /// 请领数
        /// </summary>
        int m_desireCount;

        /// <summary>
        /// 设置或获取请领数
        /// </summary>
        public int DesireCount
        {
            get { return m_desireCount; }
            set { m_desireCount = value; }
        }

        /// <summary>
        /// 实发数
        /// </summary>
        int m_factCount;

        /// <summary>
        /// 设置或获取实发数
        /// </summary>
        public int FactCount   
        {
            get { return m_factCount; }
            set { m_factCount = value; }
        }

        /// <summary>
        /// 操作状态
        /// </summary>
        FetchStateEnum m_fetchState;

        /// <summary>
        /// 设置或获取操作状态
        /// </summary>
        public FetchStateEnum FetchState
        {
            get { return m_fetchState; }
            set { m_fetchState = value; }
        }
    }
}

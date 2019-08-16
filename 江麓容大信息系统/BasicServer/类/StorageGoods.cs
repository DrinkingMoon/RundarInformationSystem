using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerModule
{
    /// <summary>
    /// 仓库物品信息定义类（会在多个组件中引用）
    /// </summary>
    public struct StorageGoods
    {
        /// <summary>
        /// 库房ID
        /// </summary>
        public string StorageID;

        /// <summary>
        /// 库房名称
        /// </summary>
        public string StorageName;

        /// <summary>
        /// 物品ID
        /// </summary>
        public int GoodsID;

        /// <summary>
        /// 图号
        /// </summary>
        public string GoodsCode;

        /// <summary>
        /// 物品名称
        /// </summary>
        public string GoodsName;

        /// <summary>
        /// 规格
        /// </summary>
        public string Spec;

        /// <summary>
        /// 供应商
        /// </summary>
        public string Provider;

        /// <summary>
        /// 批次号
        /// </summary>
        public string BatchNo;

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Quantity;
    }
}

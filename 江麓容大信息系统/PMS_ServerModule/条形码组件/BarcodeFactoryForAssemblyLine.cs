/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  BarcodeFactoryForAssemblyLine.cs
 * 作者    :  夏石友    版本: v1.00    日期: 2009/06/15
 * 开发平台:  vs2005(c#)
 * 用于    :  生产线管理信息系统
 *----------------------------------------------------------------------------
 * 描述 : 装配线条形码类厂, 用于获取条形码管理对象实例
 * 其它 :
 *----------------------------------------------------------------------------
 * 历史记录:
 *     1. 日期时间: 2009/06/15 16:02:08 作者: 夏石友 当前版本: V1.00
 *        修改说明: 创建
 ******************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;

namespace ServerModule
{
    /// <summary>
    /// 装配线条形码类厂
    /// </summary>
    public class BarcodeFactoryForAssemblyLine
    {
        /// <summary>
        /// 嵌套类
        /// </summary>
        class Nested
        {
            /// <summary>
            /// 构造函数
            /// </summary>
            static Nested()
            {
            }

            /// <summary>
            /// 装配线管理条形码对象实例
            /// </summary>
            internal static readonly IBarcodeForAssemblyLine instance = new BarcodeForAssemblyLine();
        }

        /// <summary>
        /// 私有构造函数
        /// </summary>
        private BarcodeFactoryForAssemblyLine() { }

        /// <summary>
        /// 获取装配线管理条形码对象实例
        /// </summary>
        public static IBarcodeForAssemblyLine Instance
        {
            get
            {
                return Nested.instance;
            }
        }
    }
}

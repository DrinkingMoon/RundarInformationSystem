/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  ChoseConfectServer.cs
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
using System.Data;
using System.Collections;
using DBOperate;

namespace ServerModule
{
    /// <summary>
    /// 零件选配信息管理类
    /// </summary>
    class ChoseConfectServer : IChoseConfectServer
    {
        /// <summary>
        /// 数据库操作接口
        /// </summary>
        static IDBOperate m_dbOperate = CommentParameter.GetDBOperatorOfDepotManagement();

        /// <summary>
        /// 获取选配表表头所有信息
        /// </summary>
        /// <param name="table">返回查询的数据信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>获取成功返回True，获取失败返回False</returns>
        public bool GetAllChoseConfectTableHead(out DataTable table, out string error)
        {
            table = null;
            error = null;

            DataSet tempDataSet = new DataSet();

            Dictionary<OperateCMD, object> dicOperateCMD = m_dbOperate.RunProc_CMD("SelAllP_ChoseConfectTableHead", tempDataSet);

            if (!Convert.ToBoolean(dicOperateCMD[OperateCMD.Return_OperateResult]))
            {
                error = Convert.ToString(dicOperateCMD[OperateCMD.Return_Errmsg]);
                return false;
            }

            table = tempDataSet.Tables[0];

            return true;
        }

        /// <summary>
        /// 获取所有选配零件信息
        /// </summary>
        /// <returns>返回获取到的选配零件信息</returns>
        public IQueryable<P_ChoseConfectTable> GetAllChoseConfectInfo()
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            var result = from r in dataContxt.P_ChoseConfectTable
                         select r;

            return result;
        }

        /// <summary>
        /// 是否存在指定选配零件信息
        /// </summary>
        /// <param name="goodsCode">图号型号</param>
        /// <returns>存在指定的选配零件信息返回true</returns>
        public bool IsExistChoseConfectInfo(string goodsCode)
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            var result = from r in dataContxt.P_ChoseConfectTable
                         where r.AccessoryCode == goodsCode
                         select r;

            return result.Count() > 0;
        }

        /// <summary>
        /// 添加/修改零件选配信息表
        /// </summary>
        /// <param name="id">序号</param>
        /// <param name="accessoryCode">零部件编码</param>
        /// <param name="rangeData">范围</param>
        /// <param name="productType">产品类型</param>
        /// <param name="choseConfectData">选配数据</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功添加/修改零件选配信息表</returns>
        public bool UpdataAccessoryChoseConfectInfo(string id, string accessoryCode, string rangeData, string productType,
            string choseConfectData, out string error)
        {
            Hashtable paramTable = new Hashtable();

            error = null;

            if (id != null)
            {
                paramTable.Add("@ID", Convert.ToInt32(id));
            }
            else
            {
                paramTable.Add("@ID", DBNull.Value);
            }

            paramTable.Add("@AccessoryCode", accessoryCode);
            paramTable.Add("@Ranges", rangeData);
            paramTable.Add("@ProductType", productType);

            if (choseConfectData != null)
            {
                paramTable.Add("@ChoseConfect", choseConfectData);
            }
            else
            {
                paramTable.Add("@ChoseConfect", DBNull.Value);
            }

            Dictionary<OperateCMD, object> dicOperateCMD = m_dbOperate.RunProc_CMD("AddP_ChoseConfectTable", paramTable);

            if (!Convert.ToBoolean(dicOperateCMD[OperateCMD.Return_OperateResult]))
            {
                error = Convert.ToString(dicOperateCMD[OperateCMD.Return_Errmsg]);
                return false;
            }

            return true;
        }

        /// <summary>
        /// 删除某一零件选配信息
        /// </summary>
        /// <param name="id">序号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功删除某一零件选配信息</returns>
        public bool DeleteAccessoryChoseConfectInfo(string id, out string error)
        {
            error = null;

            Hashtable paramTable = new Hashtable();

            paramTable.Add("@ID", Convert.ToInt32(id));

            Dictionary<OperateCMD, object> dicOperateCMD = m_dbOperate.RunProc_CMD("DelP_ChoseConfectTable", paramTable);

            if (!Convert.ToBoolean(dicOperateCMD[OperateCMD.Return_OperateResult]))
            {
                error = Convert.ToString(dicOperateCMD[OperateCMD.Return_Errmsg]);
                return false;
            }

            return true;
        }

        /// <summary>
        /// 添加/修改选配表表头
        /// </summary>
        /// <param name="accessoryCode">零部件编码</param>
        /// <param name="tableName">选配表表名</param>
        /// <param name="firstColumn">选配表表头第1列列名</param>
        /// <param name="secondColumn">选配表表头第2列列名</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功添加/修改选配表表头</returns>
        public bool UpdataAccessoryChoseConfectHead(string accessoryCode, string tableName, 
            string firstColumn, string secondColumn, out string error)
        {
            error = null;

            Hashtable paramTable = new Hashtable();

            paramTable.Add("@AccessoryCode", accessoryCode);
            paramTable.Add("@TableName", tableName);
            paramTable.Add("@RangeData", firstColumn);
            paramTable.Add("@ChoseData", secondColumn);

            Dictionary<OperateCMD, object> dicOperateCMD = m_dbOperate.RunProc_CMD("AddP_ChoseConfectTableHead", paramTable);

            if (!Convert.ToBoolean(dicOperateCMD[OperateCMD.Return_OperateResult]))
            {
                error = Convert.ToString(dicOperateCMD[OperateCMD.Return_Errmsg]);
                return false;
            }

            return true;
        }

        /// <summary>
        /// 删除某一编码的选配表表头
        /// </summary>
        /// <param name="accessoryCode">零部件编码</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功删除某一编码的选配表表头</returns>
        public bool DeleteChoseConfectTableHead(string accessoryCode, out string error)
        {
            Hashtable paramTable = new Hashtable();

            error = null;

            paramTable.Add("@AccessoryCode", accessoryCode);

            Dictionary<OperateCMD, object> dicOperateCMD = m_dbOperate.RunProc_CMD("DelP_ChoseConfectTableHead", paramTable);

            if (!Convert.ToBoolean(dicOperateCMD[OperateCMD.Return_OperateResult]))
            {
                error = Convert.ToString(dicOperateCMD[OperateCMD.Return_Errmsg]);
                return false;
            }

            return true;
        }

        /// <summary>
        /// 获取选配表表名及表头
        /// </summary>
        /// <param name="accessoryCode">零部件编码</param>
        /// <param name="tableTitle">选配表表名及表头</param>
        /// <param name="rangeTitle">返回表头</param>
        /// <param name="choseConfectTitle">返回选择的表头</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是够成功获取选配表表名及表头</returns>
        public bool GetChoseConfectTableHead(string accessoryCode, out string tableTitle, 
            out string rangeTitle, out string choseConfectTitle, out string error)
        {
            tableTitle = null;
            rangeTitle = null;
            choseConfectTitle = null;
            error = null;

            Hashtable paramTable = new Hashtable();
            DataSet ds = new DataSet();

            paramTable.Add("@AccessoryCode", accessoryCode);

            Dictionary<OperateCMD, object> dicOperateCMD = 
                m_dbOperate.RunProc_CMD("SelP_ChoseConfectTableHead", ds, paramTable);

            if (!Convert.ToBoolean(dicOperateCMD[OperateCMD.Return_OperateResult]))
            {
                error = Convert.ToString(dicOperateCMD[OperateCMD.Return_Errmsg]);
                return false;
            }

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                tableTitle = ds.Tables[0].Rows[i][2].ToString();
                rangeTitle = ds.Tables[0].Rows[i][3].ToString();
                choseConfectTitle = ds.Tables[0].Rows[i][4].ToString();
            }
            
            return true;
        }

        /// <summary>
        /// 获取某一零件的选配信息
        /// </summary>
        /// <param name="accessoryCode">零部件编码</param>
        /// <param name="productType">产品类型</param>
        /// <param name="table">某一零件的选配信息表</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功获取某一零件的选配信息</returns>
        public bool GetAccessoryChoseConfectInfo(string accessoryCode, string productType, out DataTable table, out string error)
        {
            table = null;
            error = null;

            Hashtable paramTable = new Hashtable();

            paramTable.Add("@AccessoryCode", accessoryCode);
            paramTable.Add("@ProductType", productType);

            DataSet AccessoryChoseConfectDataSet = new DataSet();

            Dictionary<OperateCMD, object> dicOperateCMD = 
                m_dbOperate.RunProc_CMD("SelP_ChoseConfectTable", AccessoryChoseConfectDataSet, paramTable);

            if (!Convert.ToBoolean(dicOperateCMD[OperateCMD.Return_OperateResult]))
            {
                error = Convert.ToString(dicOperateCMD[OperateCMD.Return_Errmsg]);
                return false;
            }

            table = AccessoryChoseConfectDataSet.Tables[0];

            return true;
        }

        #region 2012-04-26 增加, 夏石友, 原因：新增的“快速返修变速箱”窗体所需

        /// <summary>
        /// 获取可选配的零件图号型号信息
        /// </summary>
        /// <returns>返回获取到的零件图号型号信息</returns>
        public IQueryable<string> GetOptionPartCode()
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            return from r in dataContxt.P_ChoseConfectTable
                   select r.AccessoryCode;
        }

        #endregion
    }
}

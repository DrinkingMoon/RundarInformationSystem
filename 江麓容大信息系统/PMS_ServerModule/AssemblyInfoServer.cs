/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  AssemblyInfoServer.cs
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
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using System.Collections;
using DBOperate;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace ServerModule
{
    /// <summary>
    /// Bom附属表服务类
    /// </summary>
    class AssemblyInfoServer : BasicServer, IAssemblyInfoServer
    {
        /// <summary>
        /// 获取附属BOM表中满足版本及总成标志的信息
        /// </summary>
        /// <param name="edition">产品版本</param>
        /// <param name="assemblyFlag">总成标志</param>
        /// <returns>返回获取到的附属BOM表中满足版本及总成标志的信息</returns>
        public IQueryable<P_PertainProductBom> GetPertainProductBomInfo(string edition, int assemblyFlag)
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            var result = from r in dataContxt.P_PertainProductBom
                         where r.AssemblyFlag == assemblyFlag && r.ProductType == edition
                         select r;

            return result;
        }

        /// <summary>
        /// 获取Bom附属表总成/选配零件信息
        /// </summary>
        /// <param name="assemblyFlag">总成标志</param>
        /// <param name="edition">产品版本</param>
        /// <param name="returnAccessory">返回获得到的信息</param>
        /// <param name="error">False时返回的错误信息</param>
        /// <returns>获取成功返回True，获取失败返回False</returns>
        public bool GetPertainProductBomInfo(string assemblyFlag, string edition, 
            out DataTable returnAccessory, out string error)
        {
            returnAccessory = null;
            error = null;

            Hashtable paramTable = new Hashtable();

            paramTable.Add("@AssemblyFlag", assemblyFlag);
            paramTable.Add("@Edition", edition);

            DataSet AssecblyInfoDataSet = new DataSet();

            Dictionary<OperateCMD, object> dicOperateCMD = 
                m_dbOperate.RunProc_CMD("SelAllP_PertainProductBom", AssecblyInfoDataSet, paramTable);

            if (!Convert.ToBoolean(dicOperateCMD[OperateCMD.Return_OperateResult]))
            {
                error = Convert.ToString(dicOperateCMD[OperateCMD.Return_Errmsg]);
                return false;
            }

            returnAccessory = AssecblyInfoDataSet.Tables[0];
            return true;
        }

        /// <summary>
        /// 获取指定产品的所有选配零件信息
        /// </summary>
        /// <returns>选配信息结果集</returns>
        public IQueryable GetAllChoseConfectPartInfo()
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            //var result = (from r in dataContxt.P_PertainProductBom 
            //             where r.AssemblyFlag == 0
            //             select new { 零件编码 = r.GoodsCode, 零件名称 = r.GoodsName, 规格 = r.Spec }).Distinct();

            var result = (from r in dataContxt.P_AssemblingBom
                          where r.IsAdaptingPart
                          select new { 零件编码 = r.PartCode, 零件名称 = r.PartName, 规格 = r.Spec }).Distinct();

            return result;
        }

        /// <summary>
        /// 添加Bom附属表
        /// </summary>
        /// <param name="productType">产品类型</param>
        /// <param name="goodsCode">图号型号</param>
        /// <param name="goodsName">物品名称</param>
        /// <param name="spec">规格</param>
        /// <param name="assemblyFlag">总成标志</param>
        /// <param name="returnTable">返回获得Bom附属表信息</param>
        /// <param name="error">False时返回的错误信息</param>
        /// <returns>添加成功返回True，添加失败返回False</returns>
        public bool AddPertainProductBomInfo(string productType, string goodsCode, string goodsName, string spec, 
            int assemblyFlag, out DataTable returnTable, out string error)
        {
            returnTable = null;
            error = null;

            Hashtable paramTable = new Hashtable();

            paramTable.Add("@ProductType", productType);
            paramTable.Add("@GoodsCode", goodsCode);
            paramTable.Add("@GoodsName", goodsName);
            paramTable.Add("@Spec", spec);
            paramTable.Add("@AssemblyFlag", assemblyFlag);

            DataSet AssecblyInfoDataSet = new DataSet();
            Dictionary<OperateCMD, object> dicOperateCMD = 
                m_dbOperate.RunProc_CMD("AddP_PertainProductBom", AssecblyInfoDataSet, paramTable);

            if (!Convert.ToBoolean(dicOperateCMD[OperateCMD.Return_OperateResult]))
            {
                error = Convert.ToString(dicOperateCMD[OperateCMD.Return_Errmsg]);

                if (error != "没有找到任何数据" && error != null)
                {
                    return false;
                }
            }

            if (assemblyFlag == 0)
            {
                GetPertainProductBomInfo("False", productType, out returnTable, out error);
            }
            else if (assemblyFlag == 1)
            {
                GetPertainProductBomInfo("True", productType, out returnTable, out error);
            }

            return true;
        }

        /// <summary>
        /// 删除Bom附属表
        /// </summary>
        /// <param name="id">序号</param>
        /// <param name="productType">产品类型</param>
        /// <param name="assemblyFlag">总成标志</param>
        /// <param name="returnTable">返回BOM附属表的信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>删除成功返回True，删除失败返回False</returns>
        public bool DeletePertainProductBomInfo(string id, string productType, int assemblyFlag, 
            out DataTable returnTable, out string error)
        {
            returnTable = null;
            error = null;

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;
                Table<P_PertainProductBom> table = dataContxt.GetTable<P_PertainProductBom>();
                P_PertainProductBom delRow = (from c in table where c.ID.ToString() == id select c).First();

                table.DeleteOnSubmit(delRow);
                dataContxt.SubmitChanges();

                if (assemblyFlag == 0)
                {
                    GetPertainProductBomInfo("False", productType, out returnTable, out error);
                }
                else if (assemblyFlag == 1)
                {
                    GetPertainProductBomInfo("True", productType, out returnTable, out error);
                }
            }
            catch (Exception err)
            {
                return SetReturnError(err, out returnTable, out error);
            }

            return true; 
        }

        /// <summary>
        /// 设置返回的错误信息
        /// </summary>
        /// <param name="err">出错时返回错误信息，无错时返回null</param>
        /// <param name="returnTable">返回NULL</param>
        /// <param name="error">返回Err的String型</param>
        /// <returns>返回False</returns>
        bool SetReturnError(object err, out DataTable returnTable, out string error)
        {
            returnTable = null;
            error = err.ToString();
            return false;
        }
    }
}

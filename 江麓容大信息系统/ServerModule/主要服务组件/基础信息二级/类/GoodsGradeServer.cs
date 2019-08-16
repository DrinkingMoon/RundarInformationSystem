/******************************************************************************
 * �版权所?(c) 2006-2010, �小康工业集团容大有限责任公司
 *
 * 文件名称:  GoodsGradeServer.cs
 * 作?   :  �夏石友    版本: v1.00    日期: 2010/07/17
 * 开发平?  Visual C# 2008
 * �用于    :  仓库管理软件
 *----------------------------------------------------------------------------
 * 描述 : 
 * 其它 :
 *----------------------------------------------------------------------------
 * 公共信息: 参见系统'类帮助文?
 *----------------------------------------------------------------------------
 * �历史记录:
 *     1. 日期时间: 2010/07/17 8:54:12 作? �夏石友 当前版本: V1.00
 *        修改说明: 创建
 ******************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using System.Collections;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using DBOperate;
using PlatformManagement;

namespace ServerModule
{
    /// <summary>
    /// 产品、零件等级划分信息管理类
    /// </summary>
    class GoodsGradeServer : IGoodsGradeServer
    {
        /// <summary>
        /// 数据库操作
        /// </summary>
        static IDBOperate m_dbOperate = CommentParameter.GetDBOperatorOfDepotManagement();

        /// <summary>
        /// 判断某零件是否属于AB类零件
        /// </summary>
        /// <param name="goodsCode">图号型号</param>
        /// <param name="spec">规格</param>
        /// <param name="returnTable">返回的AB类零件数据集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        public bool JudgeGoodsGrade(string goodsCode, string spec, out DataTable returnTable, out string error)
        {
            returnTable = null;
            error = null;

            Hashtable paramTable = new Hashtable();

            paramTable.Add("@GoodsCode", goodsCode);
            paramTable.Add("@Spec", spec);

            DataSet goodsGradeDataSet = new DataSet(); 

            Dictionary<OperateCMD, object> dicOperateCMD = 
                m_dbOperate.RunProc_CMD("JudgeGoodsGradeFromQ_GoodsGradeTable", goodsGradeDataSet, paramTable);

            if (!Convert.ToBoolean(dicOperateCMD[OperateCMD.Return_OperateResult]))
            {
                error = Convert.ToString(dicOperateCMD[OperateCMD.Return_Errmsg]);
                return false;
            }

            returnTable = goodsGradeDataSet.Tables[0];
            return true;
        }

        /// <summary>
        /// 获取AB类零件表信息
        /// </summary>
        /// <param name="returnTable">返回的AB零件表</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        public bool GetAllGoodsGradeTable(out IQueryable<View_Q_GoodsGradeTable> returnTable, out string error)
        {
            returnTable = null;
            error = null;

            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            try
            {
                Table<View_Q_GoodsGradeTable> table = dataContxt.GetTable<View_Q_GoodsGradeTable>();

                returnTable = from c in table orderby c.类别, c.图号型号 select c;
                return true;
            }
            catch (Exception err)
            {
                return SetReturnError(err, out returnTable, out error);
            }

        }

        /// <summary>
        /// 添加/修改AB类零件表信息
        /// </summary>
        /// <param name="goodsType">物品类型</param>
        /// <param name="goodsCode">图号型号</param>
        /// <param name="spec">规格</param>
        /// <param name="goodsName">物品名称</param>
        /// <param name="goodsGrade">物品等级</param>
        /// <param name="id">序号</param>
        /// <param name="returnTable">返回的AB零件表</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        public bool AddGoodsGrade(string goodsType, string goodsCode, string spec, string goodsName, 
            string goodsGrade, int id, out DataTable returnTable, out string error)
        {
            returnTable = null;
            error = null;

            Hashtable paramTable = new Hashtable();

            paramTable.Add("@GoodsType", goodsType);
            paramTable.Add("@GoodsCode", goodsCode);
            paramTable.Add("@Spec", spec);
            paramTable.Add("@GoodsName", goodsName);
            paramTable.Add("@GoodsGrade", goodsGrade);

            if (id == -1)
            {
                paramTable.Add("@ID", DBNull.Value);
            }
            else
            {
                paramTable.Add("@ID", id);
            }

            DataSet goodsGradeDataSet = new DataSet();

            Dictionary<OperateCMD, object> dicOperateCMD =
                m_dbOperate.RunProc_CMD("AddQ_GoodsGradeTable", goodsGradeDataSet, paramTable);

            if (!Convert.ToBoolean(dicOperateCMD[OperateCMD.Return_OperateResult]))
            {
                error = Convert.ToString(dicOperateCMD[OperateCMD.Return_Errmsg]);
                return false;
            }

            returnTable = goodsGradeDataSet.Tables[0];
            return true;
        }

        /// <summary>
        /// 删除AB类零件表信息
        /// </summary>
        /// <param name="id">序号</param>
        /// <param name="returnTable">返回的AB零件表</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        public bool DeleteGoodsGrade(int id, out IQueryable<View_Q_GoodsGradeTable> returnTable, out string error)
        {
            error = null;

            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            try
            {

                Table<Q_GoodsGradeTable> table = dataContxt.GetTable<Q_GoodsGradeTable>();

                var delRow = from c in table 
                             where c.ID == id 
                             select c;

                foreach (var ei in delRow)
                {
                    table.DeleteOnSubmit(ei);
                }

                dataContxt.SubmitChanges();

                GetAllGoodsGradeTable(out returnTable, out error);
                return true; 
            }
            catch (Exception err)
            {
                return SetReturnError(err, out returnTable, out error);
            }
        }

        /// <summary>
        /// 设置出错返回值
        /// </summary>
        /// <param name="err">传入的错误信息</param>
        /// <param name="returnTable">返回的信息</param>
        /// <param name="error">返回的错误信息</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        bool SetReturnError(object err, out IQueryable<View_Q_GoodsGradeTable> returnTable, out string error)
        {
            returnTable = null;
            error = err.ToString();

            return false;
        }
    }
}

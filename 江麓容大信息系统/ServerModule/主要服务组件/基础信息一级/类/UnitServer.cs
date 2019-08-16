/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  IUnitServer.cs
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
using System.Data.SqlClient;
using System.Data.Common;
using System.Collections;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace ServerModule
{
    /// <summary>
    /// 单位管理类
    /// </summary>
    class UnitServer : BasicServer, IUnitServer
    {
        /// <summary>
        /// 获得单位名称
        /// </summary>
        /// <param name="unitID">单位ID</param>
        /// <returns>单位名称</returns>
        public string GetUnitName(int unitID)
        {
            string strSql = "select UnitName from S_Unit where ID = " + unitID;

            return GlobalObject.DatabaseServer.QueryInfo(strSql).Rows[0][0].ToString();
        }

        /// <summary>
        /// 获取单位信息
        /// </summary>
        /// <param name="id">单位ID</param>
        /// <returns>成功返回获取到的单位信息, 失败返回null</returns>
        public View_S_Unit GetUnitInfo(int id)
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            var result = from r in dataContxt.View_S_Unit 
                         where r.序号 == id 
                         select r;

            if (result.Count() == 0)
            {
                return null;
            }
            else
            {
                return result.Single();
            }
        }

        /// <summary>
        /// 获取单位信息
        /// </summary>
        /// <param name="unit">单位ID</param>
        /// <returns>成功返回获取到的单位信息, 失败返回null</returns>
        public View_S_Unit GetUnitInfo(string unit)
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            var result = from r in dataContxt.View_S_Unit 
                         where r.单位 == unit 
                         select r;

            if (result.Count() == 0)
            {
                return null;
            }
            else
            {
                return result.Single();
            }
        }

        /// <summary>
        /// 获取单位信息
        /// </summary>
        /// <param name="returnUnit">单位信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功获取单位信息</returns>
        public bool GetAllUnit(out IQueryable<View_S_Unit> returnUnit, out string error)
        {
            error = null;

            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            try
            {
                Table<View_S_Unit> table = dataContxt.GetTable<View_S_Unit>();

                returnUnit = from c in table
                             where c.停用 == false
                             select c;

                return true;
            }
            catch(Exception err)
            {
                return SetReturnError(err, out returnUnit, out error);
            }
        }

        /// <summary>
        /// 添加单位
        /// </summary>
        /// <param name="unit">单位</param>
        /// <param name="spec">单位规格</param>
        /// <param name="isDisable">停用</param>
        /// <param name="returnUnit">单位信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功添加单位信息</returns>
        public bool AddUnit(string unit, string spec, bool isDisable, out IQueryable<View_S_Unit> returnUnit, 
                            out string error)
        {
            error = null;

            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            try
            {
                S_Unit unitObj = new S_Unit();

                unitObj.UnitName = unit;
                unitObj.UnitSpec = spec;
                unitObj.IsDisable = isDisable;

                dataContxt.S_Unit.InsertOnSubmit(unitObj);
                dataContxt.SubmitChanges();

                GetAllUnit(out returnUnit, out error);

                return true;
            }
            catch (Exception err)
            {
                return SetReturnError(err, out returnUnit, out error);
            }
        }

        /// <summary>
        /// 更新单位
        /// </summary>
        /// <param name="id">序号</param>
        /// <param name="unit">单位</param>
        /// <param name="spec">规格</param>
        /// <param name="isDisable">停用</param>
        /// <param name="returnUnit">单位信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功更新单位信息</returns>
        public bool UpdateUnit(int id, string unit, string spec, bool isDisable, out IQueryable<View_S_Unit> returnUnit, 
                               out string error)
        {
            error = null;

            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            try
            {
                S_Unit unitObj = (from c in dataContxt.S_Unit 
                                  where c.ID == id 
                                  select c).Single();
                unitObj.UnitName = unit;
                unitObj.UnitSpec = spec;
                unitObj.IsDisable = isDisable;

                dataContxt.SubmitChanges();

                GetAllUnit(out returnUnit, out error);

                return true;
            }
            catch (Exception err)
            {
                return SetReturnError(err, out returnUnit, out error);
            }
        }

        /// <summary>
        /// 删除单位
        /// </summary>
        /// <param name="id">序号</param>
        /// <param name="returnUnit">单位信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功删除单位信息</returns>
        public bool DeleteUnit(int id, out IQueryable<View_S_Unit> returnUnit, out string error)
        { 
            error = null;
            
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            try
            {
                var varData = from a in dataContxt.F_GoodsPlanCost
                              where a.UnitID == id
                              select a;

                if (varData.Count() > 0)
                {
                    error = "基础信息表内已经使用此单位，不能删除！";
                    GetAllUnit(out returnUnit, out error);
                    return false;
                }

                Table<S_Unit> table = dataContxt.GetTable<S_Unit>();

                var delRow = from c in table 
                             where c.ID == id 
                             select c;

                foreach (var ei in delRow)
                {
                    table.DeleteOnSubmit(ei);
                }

                dataContxt.SubmitChanges();

                GetAllUnit(out returnUnit, out error);

                return true; 
            }
            catch (Exception err)
            {
                return SetReturnError(err, out returnUnit, out error);
            }
        }

        /// <summary>
        /// 设置出错返回值
        /// </summary>
        /// <param name="err">传入的错误信息</param>
        /// <param name="returnUnit">单位信息</param>
        /// <param name="error">返回的错误信息</param>
        /// <returns></returns>
        bool SetReturnError(object err, out IQueryable<View_S_Unit> returnUnit, out string error)
        {
            returnUnit = null;
            error = err.ToString();

            return false;
        }
    }
}

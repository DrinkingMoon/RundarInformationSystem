/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  DepotTypeForPersonnel.cs
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
    /// 材料类别管理员服务组件
    /// </summary>
    class DepotTypeForPersonnel:BasicServer,IDepotTypeForPersonnel
    {
        /// <summary>
        /// 获得材料类别信息
        /// </summary>
        /// <param name="depotCode">材料类别编码</param>
        /// <returns>返回对象</returns>
        public S_Depot GetDepotInfo(string depotCode)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.S_Depot
                          where a.DepotCode == depotCode
                          select a;

            if (varData.Count() != 1)
            {
                return null;
            }
            else
            {
                return varData.Single();
            }
        }

        /// <summary>
        /// 获取材料类型
        /// </summary>
        /// <returns>返回数据集</returns>
        public IQueryable<S_Depot> GetDepotTypeBill()
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            return from c in dataContxt.S_Depot 
                   orderby c.DepotCode 
                   select c;
        }

        /// <summary>
        /// 获得材料类别关系表
        /// </summary>
        /// <returns>返回数据集</returns>
        public IQueryable<S_DepotTypeForPersonnel> GetDepotForPersonnel()
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            return from a in dataContxt.S_DepotTypeForPersonnel 
                   orderby a.ZlID 
                   select a;
        }

        /// <summary>
        /// 获得人员所管辖所有仓库的编码列表
        /// </summary>
        /// <param name="workID">人员工号</param>
        /// <returns>成功返回材料类别编码列表,失败返回null</returns>
        public List<string> GetDepotCodeForPersonnel(string workID)
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            IQueryable<S_DepotTypeForPersonnel> result = 
                from r in dataContxt.S_DepotTypeForPersonnel 
                where r.PersonnelID == workID 
                select r;

            if (result.Count() == 0)
            {
                return null;
            }

            List<string> lstDepotCode = (from r in dataContxt.S_DepotForDtp 
                                         where r.DtpCode == result.Single().ZlID 
                                         select r.DepotCode).ToList();

            if (lstDepotCode != null && lstDepotCode.Count == 0)
            {
                return null;
            }

            return lstDepotCode;
        }
        
        /// <summary>
        /// 获得材料类别与管理人关系表
        /// </summary>
        /// <param name="dtpCode">材料类别编码</param>
        /// <returns>返回材料类别与人员关系表</returns>
        public IQueryable<S_DepotForDtp> GetDtp(string dtpCode)
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            return from c in dataContxt.S_DepotForDtp 
                   where c.DtpCode == dtpCode 
                   select c;
        }

        /// <summary>
        /// 临时TABLE的创建（添加父节点字段）
        /// </summary>
        /// <param name="tempTable">临时表</param>
        /// <returns>返回创建好的Table</returns>
        public DataTable ChangeDataTable(DataTable tempTable)
        {
            tempTable.Columns.Remove("DepotGrade");
            tempTable.Columns.Remove("IsEnd");
            tempTable.Columns.Add("RootSign");
            
            for (int i = 0; i <= tempTable.Rows.Count - 1; i++)
            {
                int intSlength = tempTable.Rows[i]["DepotCode"].ToString().Length;

                if (intSlength > 2)
                {
                    tempTable.Rows[i]["RootSign"] = tempTable.Rows[i]["DepotCode"].ToString().Substring(0, intSlength - 2);
                }
                else 
                {
                    tempTable.Rows[i]["RootSign"] = "Root";
                }
            }

            return tempTable;
        }

        /// <summary>
        /// 寻找Dtp表是否有记录存在
        /// </summary>
        /// <param name="depotCode">材料类别编码</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        public bool FindMessgeForDtp(string depotCode, out string error)
        {
            try
            {
                error = null;

                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var varData = from c in dataContxt.S_DepotForDtp
                            where c.DepotCode == depotCode
                            select c;

                if (varData.Count() == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception exce)
            {
                error = exce.Message;
                return false;
            }
        }

        /// <summary>
        /// 删除材料类别表记录
        /// </summary>
        /// <param name="depotCode">材料类别编码</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        public bool DeleteBill(string depotCode, out string error)
        {
            try
            {
                error = null;

                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                Table<S_Depot> table = dataContxt.GetTable<S_Depot>();
                
                var delRow = from c in table 
                             where c.DepotCode == depotCode 
                             select c;

                var CheckStock = from a in dataContxt.View_S_Stock 
                                 where a.材料类别编码 == depotCode 
                                 select a;

                if(CheckStock.Count() != 0)
                {
                    error = "材料仍存有此类物品，请执行材料调拨后才可删除此类别！";
                    return false;
                }

                foreach (var ei in delRow)
                {
                    table.DeleteOnSubmit(ei);
                }

                dataContxt.SubmitChanges();

                return true;
            }
            catch (Exception exce)
            {
                error = exce.Message;
                return false;
            }
        }

        /// <summary>
        /// 修改材料类别表记录
        /// </summary>
        /// <param name="depot">Linq数据集</param>
        /// <param name="depotCode">材料类别编码</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        public bool UpdateBill(S_Depot depot,string depotCode, out string error)
        {
            error = null;

            try
            {
                if (DeleteBill(depotCode, out error))
                {
                    if (AddBill(depot, out error))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else 
                {
                    return false;
                }
            }
            catch (Exception exce)
            {
                error = exce.Message;
                return false;
            }
        }

        /// <summary>
        /// 添加材料类别表记录
        /// </summary>
        /// <param name="depot">要添加的材料类别信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        public bool AddBill(S_Depot depot,out string error)
        {
            try
            {
                error = null;

                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from c in dataContxt.S_Depot
                             where c.DepotCode == depot.DepotCode
                             select c;

                if (result.Count() == 0)
                {
                    dataContxt.S_Depot.InsertOnSubmit(depot);
                    dataContxt.SubmitChanges();
                }
                else
                {
                    error = string.Format("编码 {0} 的材料类别已经存在, 不允许重复添加", depot.DepotCode);
                    return false;
                }

                return true;
            }
            catch (Exception exce)
            {
                error = exce.Message;
                return false;
            }
        }

        /// <summary>
        /// 修改材料类型关系表记录
        /// </summary>
        /// <param name="depotForDtpList">要更新的材料类别信息列表</param>
        /// <param name="szLid">关系ID</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>错误信息</returns>
        public bool UpdateDtp(List<S_DepotForDtp> depotForDtpList, string szLid, out string error)
        {
            try
            {
                error = null;

                if (DeleteDtp(szLid, out error))
                {
                    if (AddDtp(depotForDtpList, out error))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception exce)
            {
                error = exce.Message;
                return false;
            }
        }

        /// <summary>
        /// 删除材料类型关系记录
        /// </summary>
        /// <param name="szLid">关系ID</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        public bool DeleteDtp(string szLid, out string error)
        {
            try
            {
                error = null;

                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                Table<S_DepotForDtp> S_table = dataContxt.GetTable<S_DepotForDtp>();

                var S_delRow = from a in S_table 
                               where a.DtpCode == szLid 
                               select a;

                foreach (var S_ei in S_delRow)
                {
                    S_table.DeleteOnSubmit(S_ei);
                }

                dataContxt.SubmitChanges();

                return true;
            }
            catch (Exception exce)
            {
                error = exce.Message;
                return false;
            }
        }

        /// <summary>
        /// 添加材料类型关系记录
        /// </summary>
        /// <param name="depotForDtpList">要添加的数据集合</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        public bool AddDtp(List<S_DepotForDtp> depotForDtpList,out string error)
        {
            try
            {
                error = null;

                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                dataContxt.S_DepotForDtp.InsertAllOnSubmit(depotForDtpList);
                dataContxt.SubmitChanges();

                return true;
            }
            catch (Exception exce)
            {
                error = exce.Message;
                return false;
            }
        }

        /// <summary>
        /// 添加材料类别编码表记录
        /// </summary>
        /// <param name="depotForPersonnel">要添加的数据集合</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        public bool AddDepotForPersonnel(S_DepotTypeForPersonnel depotForPersonnel, out string error)
        {
            try
            {
                error = null;

                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result_ZL = from c in dataContxt.S_DepotTypeForPersonnel 
                                where c.ZlID == depotForPersonnel.ZlID  
                                select c;

                var result_Personnel = from c in dataContxt.S_DepotTypeForPersonnel 
                                       where c.PersonnelID == depotForPersonnel.PersonnelID 
                                       select c;
                
                if (result_ZL.Count() == 0 && result_Personnel.Count() == 0)
                {

                    dataContxt.S_DepotTypeForPersonnel.InsertOnSubmit(depotForPersonnel);
                    dataContxt.SubmitChanges();
                }
                else if (result_ZL.Count() != 0)
                {
                    error = string.Format("材料类别编码 {0} 已经存在, 不允许重复添加", depotForPersonnel.ZlID);
                    return false;
                }
                else if (result_Personnel.Count() != 0)
                {
                    error = string.Format("材料管理人 {0} 已经存在, 不允许重复添加", depotForPersonnel.PersonnelName);
                    return false;
                }
                return true;
            }
            catch (Exception exce)
            {
                error = exce.Message;
                return false;
            }
        }

        /// <summary>
        /// 删除材料类别编码表记录
        /// </summary>
        /// <param name="szLid">关系ID</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        public bool DeleteDepotForPersonnel(string szLid, out string error)
        {
            try
            {
                error = null;

                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                Table<S_DepotTypeForPersonnel> table = dataContxt.GetTable<S_DepotTypeForPersonnel>();
                
                var delRow = from c in table 
                             where c.ZlID == szLid 
                             select c;

                foreach (var ei in delRow)
                {
                    table.DeleteOnSubmit(ei);
                }

                dataContxt.SubmitChanges();

                return true;
            }
            catch (Exception exce)
            {
                error = exce.Message;
                return false;
            }
        }

        /// <summary>
        /// 修改材料类型表记录
        /// </summary>
        /// <param name="depotForPersonnel">Linq数据集</param>
        /// <param name="szLid">关系ID</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        public bool UpdateDepotForPersonnel(S_DepotTypeForPersonnel depotForPersonnel, string szLid, out string error)
        {
            try
            {
                error = null;

                if (DeleteDepotForPersonnel(szLid, out error))
                {
                    if (AddDepotForPersonnel(depotForPersonnel,out error))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception exce)
            {
                error = exce.Message;
                return false;
            }
        }
    }
}

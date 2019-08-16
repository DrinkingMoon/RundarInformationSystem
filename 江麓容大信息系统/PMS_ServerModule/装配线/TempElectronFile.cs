using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using System.Collections;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using DBOperate;
using PlatformManagement;
using GlobalObject;

namespace ServerModule
{
    /// <summary>
    /// 临时电子档案数据服务
    /// </summary>
    class TempElectronFileServer : BasicServer, ServerModule.ITempElectronFileServer
    {
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="fieldName">列名</param>
        /// <param name="searchValue">检索值</param>
        /// <returns>返回获取到的数据</returns>
        public IQueryable<View_P_TempElectronFile> GetData(string fieldName, string searchValue)
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;
            IQueryable<View_P_TempElectronFile> result = null;

            switch (fieldName)
            {
                case "产品型号":
                    result = (from r in dataContxt.View_P_TempElectronFile
                              where r.产品型号.Contains(searchValue)
                              select r);
                    break;
                case "产品编码":
                    result = (from r in dataContxt.View_P_TempElectronFile
                              where r.产品编码.Contains(searchValue)
                              select r);
                    break;
                case "父总成编码":
                    result = (from r in dataContxt.View_P_TempElectronFile
                              where r.父总成编码.Contains(searchValue)
                              select r);
                    break;
                case "父总成名称":
                    result = (from r in dataContxt.View_P_TempElectronFile
                              where r.父总成名称.Contains(searchValue)
                              select r);
                    break;
                case "父总成扫描码":
                    result = (from r in dataContxt.View_P_TempElectronFile
                              where r.父总成扫描码.Contains(searchValue)
                              select r);
                    break;
                case "工位":
                    result = (from r in dataContxt.View_P_TempElectronFile
                              where r.工位.Contains(searchValue)
                              select r);
                    break;
                case "供应商":
                    result = (from r in dataContxt.View_P_TempElectronFile
                              where r.供应商.Contains(searchValue)
                              select r);
                    break;
                case "规格":
                    result = (from r in dataContxt.View_P_TempElectronFile
                              where r.规格.Contains(searchValue)
                              select r);
                    break;
                case "零部件编码":
                    result = (from r in dataContxt.View_P_TempElectronFile
                              where r.零部件编码.Contains(searchValue)
                              select r);
                    break;
                case "零部件名称":
                    result = (from r in dataContxt.View_P_TempElectronFile
                              where r.零部件名称.Contains(searchValue)
                              select r);
                    break;
                case "零件标识码":
                    result = (from r in dataContxt.View_P_TempElectronFile
                              where r.零件标识码.Contains(searchValue)
                              select r);
                    break;
                case "零件扫描码":
                    result = (from r in dataContxt.View_P_TempElectronFile
                              where r.零件扫描码.Contains(searchValue)
                              select r);
                    break;
                case "批次号":
                    result = (from r in dataContxt.View_P_TempElectronFile
                              where r.批次号.Contains(searchValue)
                              select r);
                    break;
                case "装配模式":
                    result = (from r in dataContxt.View_P_TempElectronFile
                              where r.装配模式.Contains(searchValue)
                              select r);
                    break;
                case "装配人员":
                    result = (from r in dataContxt.View_P_TempElectronFile
                              where r.装配人员.Contains(searchValue)
                              select r);
                    break;
                case "装配状态":
                    result = (from r in dataContxt.View_P_TempElectronFile
                              where r.装配状态.Contains(searchValue)
                              select r);
                    break;
                case "总成标志":
                    result = (from r in dataContxt.View_P_TempElectronFile
                              where r.总成标志 == Convert.ToInt32(searchValue)
                              select r);
                    break;
                default:
                    break;
            }

            return result;
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="beginDate">起始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <returns>返回获取到的数据</returns>
        public System.Linq.IQueryable<ServerModule.View_P_TempElectronFile> GetData(DateTime beginDate, DateTime endDate)
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            beginDate = beginDate.Date;
            endDate = new DateTime(endDate.Year, endDate.Month, endDate.Day, 23, 59, 59);

            IQueryable<View_P_TempElectronFile> result = from r in dataContxt.View_P_TempElectronFile
                                                         where Convert.ToDateTime(r.装配时间) >= beginDate && Convert.ToDateTime(r.装配时间) <= endDate
                                                         select r;
            return result;
        }

        /// <summary>
        /// 删除列表中的数据项
        /// </summary>
        /// <param name="lstGUID">GUID列表</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        public bool Delete(List<string> lstGUID, out string error)
        {
            error = null;

            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;
            IQueryable<P_TempElectronFile> result = null;

            try
            {

                result = from r in ctx.P_TempElectronFile
                         where lstGUID.Contains(r.ID.ToString())
                         select r;

                ctx.P_TempElectronFile.DeleteAllOnSubmit(result);
                ctx.SubmitChanges();
                return true;
            }
            catch (Exception exce)
            {
                error = exce.Message;
                return false;
            }
        }

        /// <summary>
        /// 更新零件标识码(旧标识码记录到备注中)
        /// 2012-07-19
        /// </summary>
        /// <param name="guid">要更新零件的GUID</param>
        /// <param name="partName">要更新零件的物品名称</param>
        /// <param name="newOnlyCode">新的零件标识码</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        public bool UpdateOnlyCode(string guid, string partName, string newOnlyCode, out string error)
        {
            try
            {
                error = null;

                Hashtable paramTable = new Hashtable();

                paramTable.Add("@ID", guid);
                paramTable.Add("@PartName", partName);
                paramTable.Add("@NewOnlyCode", newOnlyCode);
                paramTable.Add("@Personnel", BasicInfo.LoginID);

                Dictionary<OperateCMD, object> dicOperateCMD = m_dbOperate.RunProc_CMD("ZPX_UpdateOnlyCodeOfTempElectronFile", paramTable);

                if (!Convert.ToBoolean(dicOperateCMD[OperateCMD.Return_OperateResult]))
                {
                    error = Convert.ToString(dicOperateCMD[OperateCMD.Return_Errmsg]);
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
        /// 更换临时电子档案中分总成的编号(变更信息记录到备注中)
        /// </summary>
        /// <param name="oldParentScanCode">旧的分总成扫描码</param>
        /// <param name="newParentScanCode">新的分总成扫描码</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        public bool UpdateParentScanCode(string oldParentScanCode, string newParentScanCode, out string error)
        {
            try
            {
                error = null;

                Hashtable paramTable = new Hashtable();

                paramTable.Add("@OldParentScanCode", oldParentScanCode);
                paramTable.Add("@NewParentScanCode", newParentScanCode);
                paramTable.Add("@Personnel", BasicInfo.LoginID);

                Dictionary<OperateCMD, object> dicOperateCMD = m_dbOperate.RunProc_CMD("ZPX_UpdateParentScanCodeOfTempElectronFile", paramTable);

                if (!Convert.ToBoolean(dicOperateCMD[OperateCMD.Return_OperateResult]))
                {
                    error = Convert.ToString(dicOperateCMD[OperateCMD.Return_Errmsg]);
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
    }
}

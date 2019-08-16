using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using PlatformManagement;
using GlobalObject;

namespace ServerModule
{
    class TCURepairInfoServer : ServerModule.ITCURepairInfoServer
    {
        /// <summary>
        /// 查询结果过滤器
        /// </summary>
        string m_queryResultFilter = null;

        /// <summary>
        /// 获取或设置查询结果过滤器
        /// </summary>
        public string QueryResultFilter
        {
            get { return m_queryResultFilter; }
            set { m_queryResultFilter = value; }
        }

        /// <summary>
        /// 获取所有返修信息
        /// </summary>
        /// <param name="result">结果集</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool GetAllData(out IQueryResult result,out string error)
        {
            error = "";
            result = null;

            IAuthorization authorization = PlatformFactory.GetObject<IAuthorization>();
            IQueryResult qr = null;

            if (QueryResultFilter == null)
            {
                qr = authorization.Query("TCU返修信息管理", null);
            }
            else
            {
                qr = authorization.Query("TCU返修信息管理", null, QueryResultFilter);
            }

            if (!qr.Succeeded)
            {
                error = qr.Error;
                return false;
            }

            result = qr;
            return true;
        }

        /// <summary>
        /// 通过单号获取一条信息
        /// </summary>
        /// <param name="billNo">单号</param>
        /// <returns>成功返回TCU_RepairInfo数据集，失败返回null</returns>
        public TCU_RepairInfo GetDataByBillNo(string billNo)
        {
            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from a in dataContxt.TCU_RepairInfo
                             where a.ID == Convert.ToInt32(billNo)
                             select a;

                if (result.Count() > 0)
                {
                    return result.Single();
                }

                else
                    return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 添加一条TCU返修信息
        /// </summary>
        /// <param name="repairInfo">TCU返修信息数据集</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回true，失败返回false</returns>
        public bool InsertData(TCU_RepairInfo repairInfo,out string error)
        {
            error = "";

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from a in dataContxt.TCU_RepairInfo
                             where a.TCUVersions == repairInfo.TCUVersions && a.AssociatedBillNo == repairInfo.AssociatedBillNo
                             select a;

                if (result.Count() > 0)
                {
                    error = repairInfo.AssociatedBillNo + "关联单号的" + repairInfo.TCUVersions + "版本已经存在！";
                    return false;
                }

                dataContxt.TCU_RepairInfo.InsertOnSubmit(repairInfo);
                dataContxt.SubmitChanges();

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 通过单号修改TCU返修信息(先删后增)
        /// </summary>
        /// <param name="repairInfo">TCU返修信息数据集</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回true，失败返回false</returns>
        public bool UpdateData(TCU_RepairInfo repairInfo, out string error)
        {
            error = "";

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from a in dataContxt.TCU_RepairInfo
                             where a.ID == repairInfo.ID
                             select a;

                if (result.Count() > 0)
                {
                    dataContxt.TCU_RepairInfo.DeleteAllOnSubmit(result);
                }

                dataContxt.TCU_RepairInfo.InsertOnSubmit(repairInfo);
                dataContxt.SubmitChanges();

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 通过单号删除TCU返修信息
        /// </summary>
        /// <param name="billNo">单号</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回true，失败返回false</returns>
        public bool DeleteData(string billNo, out string error)
        {
            error = "";

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from a in dataContxt.TCU_RepairInfo
                             where a.ID == Convert.ToInt32(billNo)
                             select a;

                if (result.Count() > 0)
                {
                    dataContxt.TCU_RepairInfo.DeleteAllOnSubmit(result);
                    dataContxt.SubmitChanges();
                }

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 质管确认修改TCU返修信息
        /// </summary>
        /// <param name="billNo">单号</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回true，失败返回false</returns>
        public bool ConfirmUpdateData(string billNo,out string error)
        {
            error = "";

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from a in dataContxt.TCU_RepairInfo
                             where a.ID == Convert.ToInt32(billNo)
                             select a;

                if (result.Count() > 0)
                {
                    TCU_RepairInfo lnq = result.Single();

                    lnq.Confirmor = BasicInfo.LoginID;
                    lnq.ConfirmTime = ServerTime.Time;
                    lnq.Statua = "已完成";

                    dataContxt.SubmitChanges();
                }

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 获得最大编号
        /// </summary>
        /// <returns>成功返回最大编号，失败返回null</returns>
        public string GetMaxBillNo()
        {
            string sql = "select Max(ID) 序号 from TCU_RepairInfo";
            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            if (dt != null && dt.Rows.Count > 0)
            {
                return dt.Rows[0]["序号"].ToString();
            }

            return null;
        }
    }
}

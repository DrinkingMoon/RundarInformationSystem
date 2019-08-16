/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  DeviceMaintenanceBill.cs
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
using System.Data.Linq;
using System.Data.Linq.Mapping;
using DBOperate;
using PlatformManagement;
using GlobalObject;

namespace ServerModule
{
    /// <summary>
    /// 设备维修申请管理类
    /// </summary>
    class DeviceMaintenanceBill : BasicServer, IDeviceMaintenanceBill
    {
        /// <summary>
        /// 检查某业务是否存在
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="billNo">业务号</param>
        /// <returns>存在返回true</returns>
        public bool IsExist(DepotManagementDataContext ctx, string billNo)
        {
            var varData = from a in ctx.S_DeviceMaintenanceBill
                          where a.Bill_ID == billNo
                          select a;

            if (varData == null || varData.Count() == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 检查某单据是否存在
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <returns>存在返回true</returns>
        public bool IsExist(string billNo)
        {
            string sql = "SELECT * FROM [DepotManagement].[dbo].[S_DeviceMaintenanceBill] where Bill_ID = '" + billNo + "'";

            System.Data.DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            if (dt != null && dt.Rows.Count > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 获得全部信息
        /// </summary>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="billStatus">单据状态</param>
        /// <returns>返回Table</returns>
        public DataTable GetAllInfo(DateTime startTime,DateTime endTime, string billStatus)
        {
            string strSql = "select * from View_S_DeviceMaintenanceBill where 申请日期 >= '" + startTime + "' and 申请日期 <= '"+ endTime +"' ";

            if (billStatus != "全部")
            {
                strSql += " and 单据状态 = '"+ billStatus +"'";
            }

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        /// <summary>
        /// 删除单据
        /// </summary>
        /// <param name="billNo">单据号</param>
        public void DeleteBill(string billNo)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.S_DeviceMaintenanceBill
                          where a.Bill_ID == billNo
                          select a;

            ctx.S_DeviceMaintenanceBill.DeleteAllOnSubmit(varData);
            ctx.SubmitChanges();
        }

        /// <summary>
        /// 流程管理
        /// </summary>
        /// <param name="deviceMaintenanceBill">LINQ数据集</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool FlowInfo(S_DeviceMaintenanceBill deviceMaintenanceBill,out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

                var varData = from a in ctx.S_DeviceMaintenanceBill
                              where a.Bill_ID == deviceMaintenanceBill.Bill_ID
                              select a;

                S_DeviceMaintenanceBill lnqDeviceMaintenanceBill = new S_DeviceMaintenanceBill();

                if (varData.Count() == 0)
                {
                    lnqDeviceMaintenanceBill.Bill_ID = deviceMaintenanceBill.Bill_ID;
                    lnqDeviceMaintenanceBill.DeviceCode = deviceMaintenanceBill.DeviceCode;
                    lnqDeviceMaintenanceBill.DeviceName = deviceMaintenanceBill.DeviceName;
                    lnqDeviceMaintenanceBill.FaultDescription = deviceMaintenanceBill.FaultDescription;
                    lnqDeviceMaintenanceBill.UseDept = deviceMaintenanceBill.UseDept;
                    lnqDeviceMaintenanceBill.DeviceDamageTime = deviceMaintenanceBill.DeviceDamageTime;
                    lnqDeviceMaintenanceBill.BillStatus = "等待维修";
                    lnqDeviceMaintenanceBill.Proposer = BasicInfo.LoginName;
                    lnqDeviceMaintenanceBill.ProposerDate = ServerTime.Time;

                    ctx.S_DeviceMaintenanceBill.InsertOnSubmit(lnqDeviceMaintenanceBill);
                }
                else if (varData.Count() == 1)
                {
                    lnqDeviceMaintenanceBill = varData.Single();

                    switch (lnqDeviceMaintenanceBill.BillStatus)
                    {
                        case "等待维修":

                            lnqDeviceMaintenanceBill.BillStatus = "等待评价";
                            lnqDeviceMaintenanceBill.MaintenanceCondition = deviceMaintenanceBill.MaintenanceCondition;
                            lnqDeviceMaintenanceBill.ReplacementParts = deviceMaintenanceBill.ReplacementParts;
                            lnqDeviceMaintenanceBill.DeviceNormalUseTime = deviceMaintenanceBill.DeviceNormalUseTime;
                            lnqDeviceMaintenanceBill.Attendant = BasicInfo.LoginName;
                            lnqDeviceMaintenanceBill.AttendantDate = ServerTime.Time;
                            break;
                        case "等待评价":

                            lnqDeviceMaintenanceBill.BillStatus = "等待确认";
                            lnqDeviceMaintenanceBill.ServiceEvaluation = deviceMaintenanceBill.ServiceEvaluation;
                            lnqDeviceMaintenanceBill.Reviewers = BasicInfo.LoginName;
                            lnqDeviceMaintenanceBill.ReviewersDate = ServerTime.Time;
                            break;
                        case "等待确认":

                            lnqDeviceMaintenanceBill.BillStatus = "单据已完成";
                            lnqDeviceMaintenanceBill.Confirmor = BasicInfo.LoginName;
                            lnqDeviceMaintenanceBill.ConfirmorDate = ServerTime.Time;
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    error = "单据号重复";
                    return false;
                }

                ctx.SubmitChanges();
                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ServerModule;
using System.Data.Linq;

namespace Service_Peripheral_HR
{
    /// <summary>
    /// 部门类型管理类
    /// </summary>
    class DeptTypeServer : Service_Peripheral_HR.IDeptTypeServer
    {
        /// <summary>
        /// 获得所有部门类别
        /// </summary>
        /// <returns>返回部门类别数据集</returns>
        public DataTable GetAllDeptType()
        {
            string strSql = "select * from View_DeptType ";

            DataTable dtTemp = GlobalObject.DatabaseServer.QueryInfo(strSql);

            return dtTemp;
        }

        /// <summary>
        /// 添加部门类别信息
        /// </summary>
        /// <param name="deptType">部门类别数据集</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool AddDeptType(HR_DeptType deptType,out string error)
        {
            error = "";

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from a in dataContxt.HR_DeptType
                             where a.TypeName == deptType.TypeName || a.TypeID == deptType.TypeID
                             select a;

                if (result.Count() > 0)
                {
                    error = "请检查部门编号和部门名称，不能重复添加！";
                    return false;
                }

                dataContxt.HR_DeptType.InsertOnSubmit(deptType);
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
        /// 修改部门类别信息
        /// </summary>
        /// <param name="deptType">部门类别数据集</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool UpdateDeptType(HR_DeptType deptType, out string error)
        {
            error = "";

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from a in dataContxt.HR_DeptType
                             where a.TypeID == deptType.TypeID
                             select a;

                if (result.Count() != 1)
                {
                    error = "请检查填写信息！";
                    return false;
                }

                HR_DeptType deptTypeList = result.Single();

                deptTypeList.TypeName = deptType.TypeName;
                deptTypeList.Remark = deptType.Remark;
                deptTypeList.Recorder = deptType.Recorder;
                deptTypeList.RecordTime = deptType.RecordTime;

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
        /// 通过部门类别编号删除
        /// </summary>
        /// <param name="typeID">类别编号</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool DeleteDeptType(int typeID, out string error)
        {
            error = "";

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from a in dataContxt.HR_Dept
                             where a.DeptTypeID == typeID
                             select a;

                if (result.Count() > 0)
                {
                    error = "部门信息中关联有此部门类别，不能删除！";
                    return false;
                }

                Table<HR_DeptType> table = dataContxt.GetTable<HR_DeptType>();

                var delRow = from c in table
                             where c.TypeID == typeID
                             select c;

                foreach (var item in delRow)
                {
                    table.DeleteOnSubmit(item);
                }

                dataContxt.SubmitChanges();

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

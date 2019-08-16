using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServerModule;
using System.Data;
using GlobalObject;

namespace Service_Peripheral_HR
{
    /// <summary>
    /// 岗位管理类
    /// </summary>
    class OperatingPostServer : Service_Peripheral_HR.IOperatingPostServer
    {
        /// <summary>
        /// 获取岗位信息
        /// </summary>
        /// <param name="dept">所属部门</param>
        /// <returns>返回岗位信息</returns>
        public DataTable GetOperatingPost(string dept)
        {
            string sql = "select * from View_HR_OperatingPost";

            if (dept != null)
            {
                sql += " where 所属部门='" + dept + "'";
            }

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            return dt;
        }

        /// <summary>
        /// 获取岗位信息
        /// </summary>
        /// <returns>返回岗位信息</returns>
        public DataTable GetOperatingPost(int postID)
        {
            string sql = "select * from View_HR_OperatingPost where 岗位编号=" + postID;
            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            return dt;
        }

        /// <summary>
        /// 通过岗位名称获取岗位信息
        /// </summary>
        /// <param name="name">岗位名称</param>
        /// <returns>返回岗位信息</returns>
        public View_HR_OperatingPost GetOperatingPostByPostName(string name)
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            var result = from r in dataContxt.View_HR_OperatingPost
                         where r.岗位名称 == name
                         select r;

            if (result.Count() == 0)
            {
                return null;
            }

            return result.Single();
        }

        /// <summary>
        /// 通过岗位编号获取岗位信息
        /// </summary>
        /// <param name="postCode">岗位编号</param>
        /// <returns>返回岗位信息</returns>
        public string GetOperatingPostByPostCode(int postCode)
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            var result = from r in dataContxt.View_HR_OperatingPost
                         where r.岗位编号 == postCode
                         select r;

            if (result.Count() == 0)
            {
                return null;
            }

            return result.Single().岗位名称;
        }

        /// <summary>
        /// 添加岗位
        /// </summary>
        /// <param name="operatPost">岗位信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>成功返回True，失败返回false</returns>
        public bool AddPost(HR_OperatingPost operatPost, out string error)
        {
            error = null;

            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            try
            {
                var result = from d in dataContxt.HR_OperatingPost
                             where d.PostName == operatPost.PostName && d.Dept == operatPost.Dept
                             select d;

                if (result.Count() == 0)
                {
                    dataContxt.HR_OperatingPost.InsertOnSubmit(operatPost);
                    dataContxt.SubmitChanges();
                }
                else
                {
                    error = string.Format(" {0} 岗位已经存在, 不允许重复添加", operatPost.PostName);
                    return false;
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
        /// 修改岗位
        /// </summary>
        /// <param name="operatPost">岗位信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>成功返回True，失败返回false</returns>
        public bool UpdatePost(HR_OperatingPost operatPost, out string error)
        {
            error = null;

            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            try
            {
                var result = from e in dataContxt.HR_OperatingPost
                             where e.PostID == operatPost.PostID
                             select e;

                if (result.Count() == 1)
                {
                    HR_OperatingPost updatePost = result.Single();

                    updatePost.PostName = operatPost.PostName;
                    updatePost.PostStatement = operatPost.PostStatement;
                    updatePost.PostPrinciple = operatPost.PostPrinciple;
                    updatePost.IsCorePost = operatPost.IsCorePost;
                    updatePost.PostTypeID = operatPost.PostTypeID;
                    updatePost.Remark = operatPost.Remark;
                    updatePost.Recorder = operatPost.Recorder;
                    updatePost.RecordTime = operatPost.RecordTime;
                    updatePost.Dept = operatPost.Dept;
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

        /// <summary>
        /// 通过岗位编号删除岗位
        /// </summary>
        /// <param name="postID">岗位编号</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool DeletePost(int postID, out string error)
        {
            error = "";

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from a in dataContxt.HR_PersonnelArchive
                             where a.WorkPost == postID
                             select a;

                if (result.Count() > 0)
                {
                    error = "岗位已经使用，不能删除";
                    return false;
                }

                var resultType = from c in dataContxt.HR_OperatingPost
                                 where c.PostID == postID
                                 select c;

                foreach (var item in resultType)
                {
                    dataContxt.HR_OperatingPost.DeleteOnSubmit(item);
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

        /// <summary>
        /// 获取岗位类别信息
        /// </summary>
        /// <returns>返回岗位类别信息</returns>
        public DataTable GetPostType()
        {
            string sql = "select * from View_HR_PostType";
            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            return dt;
        }

        /// <summary>
        /// 通过类别名称获取岗位类别编号
        /// </summary>
        /// <param name="typeName">岗位类别名称</param>
        /// <returns>返回岗位类别编号</returns>
        public string GetPostTypeByTypeName(string typeName)
        {
            string sql = "select TypeID from HR_PostType where TypeName='" + typeName + "'";
            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            return dt.Rows[0]["TypeID"].ToString();
        }

        /// <summary>
        /// 添加岗位类别
        /// </summary>
        /// <param name="postType">岗位类别信息</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True 失败返回False</returns>
        public bool AddPostType(HR_PostType postType,out string error)
        {
            error = "";

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from a in dataContxt.HR_PostType
                             where a.TypeName == postType.TypeName || a.TypeID == postType.TypeID
                             select a;

                if (result.Count() > 0)
                {
                    error = string.Format(" {0} 岗位类别已经存在,请检查岗位编号和岗位名称, 不允许重复添加", postType.TypeName);
                    return false;
                }

                dataContxt.HR_PostType.InsertOnSubmit(postType);
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
        /// 修改岗位类别
        /// </summary>
        /// <param name="postType">岗位类别信息</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True ，失败返回False</returns>
        public bool UpdatePostType(HR_PostType postType, out string error)
        {
            error = "";

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from a in dataContxt.HR_PostType
                             where a.TypeID == postType.TypeID
                             select a;

                if (result.Count() == 0)
                {
                    error = "找不到相关记录，无法进行操作";
                    return false;
                }

                HR_PostType postTypeList = result.Single();

                postTypeList.TypeName = postType.TypeName;
                postTypeList.IsMiddleLevel = postType.IsMiddleLevel;
                postTypeList.IsHighLevel = postType.IsHighLevel;
                postTypeList.Remark = postType.Remark;
                postTypeList.Recorder = postType.Recorder;
                postTypeList.RecordTime = postType.RecordTime;

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
        /// 通过岗位编号删除岗位类别
        /// </summary>
        /// <param name="typeID">岗位编号</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool DeletePostType(int typeID, out string error)
        {
            error = "";

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from a in dataContxt.HR_OperatingPost
                             where a.PostTypeID == typeID
                             select a;

                if (result.Count() > 0)
                {
                    error = "岗位类别已经关联岗位信息，不能删除";
                    return false;
                }

                var resultType = from c in dataContxt.HR_PostType
                         where c.TypeID == typeID
                         select c;

                foreach (var item in resultType)
                {
                    dataContxt.HR_PostType.DeleteOnSubmit(item);
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

        /// <summary>
        /// 获得部门的总人数
        /// </summary>
        /// <param name="deptName">岗位名称</param>
        /// <returns>成功返回数据集，失败返回null</returns>
        public DataTable GetDeptCount(string deptName)
        {
            string sql = @"SELECT SUM(NumberOfPeople) AS NumberOfPeople, "+
                          " (select Count(dept) from HR_PersonnelArchive WHERE (HR_PersonnelArchive.dept like '" + deptName + "%') and HR_PersonnelArchive.PersonnelStatus <> '3') " +
                          " AS ExistAmount FROM HR_DeptPost "+
                          " WHERE (HR_DeptPost.deptCode like '"+deptName+"%')";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            return dt;
        }

        /// <summary>
        /// 获得岗位编制的信息
        /// </summary>
        /// <param name="postName">岗位名称</param>
        /// <returns>成功返回数据集，失败返回null</returns>
        public DataTable GetDeptPost(string postName)
        {
            string sql = "select * from View_HR_DeptPost";

            if (postName != null)
            {
                sql += " where 岗位名称='" + postName + "'";
            }

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            return dt;
        }

        /// <summary>
        /// 通过部门ID获得编制信息
        /// </summary>
        /// <param name="deptName">部门编号</param>
        /// <returns>返回岗位编制信息</returns>
        public IQueryable<View_HR_DeptPost> GetDeptPostByDeptCode(string deptName)
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            return from a in dataContxt.View_HR_DeptPost
                   where a.部门名称 == deptName
                   select a;
        }

        /// <summary>
        /// 添加/更新部门工作岗位
        /// </summary>
        /// <param name="deptPost">工作岗位信息</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool AddDeptPost(HR_DeptPost deptPost,out string error)
        {
            error = "";

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from a in dataContxt.HR_DeptPost
                             where a.PostID == deptPost.PostID && a.DeptCode == deptPost.DeptCode
                             select a;

                if (result.Count() > 0)
                {
                    HR_DeptPost deptPostList = result.Single();

                    deptPostList.NumberOfPeople = deptPost.NumberOfPeople;
                    deptPostList.Recorder = deptPost.Recorder;
                    deptPostList.RecordTime = deptPost.RecordTime;
                    deptPostList.Remark = deptPost.Remark;
                    deptPostList.ExistAmount = deptPost.NumberOfPeople;
                }
                else
                {
                    dataContxt.HR_DeptPost.InsertOnSubmit(deptPost);
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

        /// <summary>
        /// 批量添加/更新部门工作岗位
        /// </summary>
        /// <param name="deptPost">工作岗位信息</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool AddDeptPost(DataTable deptPost, out string error)
        {
            error = "";
            string strTemp = "";

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                for (int i = 0; i < deptPost.Rows.Count; i++)
                {
                    strTemp = deptPost.Rows[i]["班组"].ToString().Trim() + "  " + deptPost.Rows[i]["岗位名称"].ToString().Trim();

                    var result = from a in dataContxt.HR_DeptPost
                                 where a.PostID == GetOperatingPostByPostName(deptPost.Rows[i]["岗位名称"].ToString()).岗位编号
                                 && a.DeptCode == new OrganizationServer().GetDeptCode(deptPost.Rows[i]["班组"].ToString())
                                 select a;

                    if (result.Count() > 0)
                    {
                        HR_DeptPost deptPostList = result.Single();

                        deptPostList.NumberOfPeople = Convert.ToInt32(deptPost.Rows[i]["编制人数"].ToString());
                        deptPostList.Recorder = BasicInfo.LoginID;
                        deptPostList.RecordTime = ServerTime.Time;
                        deptPostList.Remark = deptPost.Rows[i]["备注"] == DBNull.Value ? "" : deptPost.Rows[i]["备注"].ToString();
                    }
                    else
                    {
                        HR_DeptPost deptPostList = new HR_DeptPost();

                        deptPostList.DeptCode = new OrganizationServer().GetDeptCode(deptPost.Rows[i]["班组"].ToString());
                        deptPostList.PostID = Convert.ToInt32(GetOperatingPostByPostName(deptPost.Rows[i]["岗位名称"].ToString()));
                        deptPostList.ExistAmount = 0;
                        deptPostList.NumberOfPeople = Convert.ToInt32(deptPost.Rows[i]["编制人数"].ToString());
                        deptPostList.Recorder = BasicInfo.LoginID;
                        deptPostList.RecordTime = ServerTime.Time;
                        deptPostList.Remark = deptPost.Rows[i]["备注"] == DBNull.Value ? "" : deptPost.Rows[i]["备注"].ToString();

                        dataContxt.HR_DeptPost.InsertOnSubmit(deptPostList);
                    }

                    strTemp = "";
                }

                dataContxt.SubmitChanges();
                return true;
            }
            catch (Exception ex)
            {
                error = strTemp + "; " + ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 修改添加部门岗位的在职人员数
        /// </summary>
        /// <param name="deptCode">部门编号</param>
        /// <param name="postCode">岗位编号</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool UpdateDeptPost(string deptCode,int postCode,out string error)
        {
            error = "";

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from a in dataContxt.HR_DeptPost
                             where a.PostID == postCode && a.DeptCode == deptCode
                             select a;

                if (result.Count() > 0)
                {
                    HR_DeptPost deptPostList = result.Single();

                    deptPostList.ExistAmount = result.Single().ExistAmount + 1;
                    deptPostList.NumberOfPeople = deptPostList.ExistAmount;

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
        /// 修改减少部门岗位的在职人员数
        /// </summary>
        /// <param name="deptCode">部门编号</param>
        /// <param name="postCode">岗位编号</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool UpdateLessDeptPost(string deptCode, int postCode, out string error)
        {
            error = "";

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from a in dataContxt.HR_DeptPost
                             where a.PostID == postCode && a.DeptCode == deptCode
                             select a;

                if (result.Count() > 0)
                {
                    HR_DeptPost deptPostList = result.Single();

                    deptPostList.ExistAmount = result.Single().ExistAmount - 1;

                    if (deptPostList.ExistAmount < 0)
                    {
                        deptPostList.ExistAmount = 0;
                    }

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
        /// 删除岗位编制信息
        /// </summary>
        /// <param name="deptPost">岗位编制信息</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回true，失败返回False</returns>
        public bool DeleteDeptPost(HR_DeptPost deptPost, out string error)
        {
            error = "";

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from a in dataContxt.HR_DeptPost
                             where a.PostID == deptPost.PostID && a.DeptCode == deptPost.DeptCode
                             select a;

                if (result.Count() != 1)
                {
                    error = "信息有误，请重新选择！";
                    return false;
                }

                dataContxt.HR_DeptPost.DeleteAllOnSubmit(result);
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

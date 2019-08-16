/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  SystemFileBasicInfo.cs
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
using ServerModule;
using System.Reflection;
using System.ComponentModel;
using UniversalControlLibrary;

namespace Service_Quality_File
{

    public enum ProcessType
    {
        审查,
        发布,
        发放,
        变更,
        回收,
        销毁
    }

    /// <summary>
    /// 体系文件接口服务
    /// </summary>
    class SystemFileBasicInfo : ISystemFileBasicInfo
    {
        /// <summary>
        /// FTP服务组件
        /// </summary>
        FileServiceSocket m_serverFTP = new FileServiceSocket(GlobalObject.GlobalParameter.FTPServerIP,
            GlobalObject.GlobalParameter.FTPServerAdvancedUser,
            GlobalObject.GlobalParameter.FTPServerAdvancedPassword);

        /// <summary>
        /// 添加文件信息
        /// </summary>
        /// <param name="fileInfo">LINQ文件信息</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True,否则返回False</returns>
        public bool AddFileList(FM_FileList fileInfo, out string error)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;
            error = null;
            try
            {
                var varSort = from a in ctx.FM_FileSort
                              where a.SortName.Contains("回收站")
                              select a;

                var varData = from a in ctx.FM_FileList
                              where (a.FileName == fileInfo.FileName || a.FileNo == fileInfo.FileNo)
                              && !varSort.Select(r => r.SortID).ToList().Contains(a.SortID)
                              select a;

                Guid guid = new Guid();

                if (varData.Count() > 0)
                {
                    FM_FileList lnqTemp = varData.Single();

                    guid = lnqTemp.FileUnique;
                    lnqTemp.Department = fileInfo.Department;
                    lnqTemp.FileName = fileInfo.FileName;
                    lnqTemp.FileNo = fileInfo.FileNo;
                    lnqTemp.SortID = fileInfo.SortID;
                    lnqTemp.Version = fileInfo.Version;
                    lnqTemp.FileUnique = fileInfo.FileUnique;
                }
                else
                {
                    ctx.FM_FileList.InsertOnSubmit(fileInfo);
                }

                ctx.SubmitChanges();

                if (varData.Count() > 0)
                {
                    UniversalControlLibrary.FileOperationService.File_Delete(guid,
                        GlobalObject.GeneralFunction.StringConvertToEnum<CE_CommunicationMode>(BasicInfo.BaseSwitchInfo[(int)GlobalObject.CE_SwitchName.文件传输方式]));
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
        /// 更新数据
        /// </summary>
        /// <param name="uniqueCode">唯一编码</param>
        /// <param name="fileType">文件类型</param>
        public void UpdateFile(Guid uniqueCode, string fileType)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.FM_FilePath
                          where a.FileUnique == uniqueCode
                          select a;

            if (varData.Count() == 1)
            {
                FM_FilePath lnqTemp = varData.Single();

                lnqTemp.FileType = fileType.ToLower();
                lnqTemp.OperationDate = ServerTime.Time;

                ctx.SubmitChanges();
            }
        }

        /// <summary>
        /// 添加文件
        /// </summary>
        /// <param name="uniqueCode">文件唯一编码</param>
        /// <param name="path">文件路径</param>
        /// <param name="fileType">文件类型</param>
        public void AddFile(Guid uniqueCode, string path, string fileType)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            FM_FilePath lnqTemp = new FM_FilePath();

            lnqTemp.FileUnique = uniqueCode;
            lnqTemp.FileType = fileType.ToLower();
            lnqTemp.FilePath = path;
            lnqTemp.OperationDate = ServerTime.Time;

            ctx.FM_FilePath.InsertOnSubmit(lnqTemp);
            ctx.SubmitChanges();
        }

        /// <summary>
        /// 通过唯一编码获得文件路径
        /// </summary>
        /// <param name="uniqueCode">文件唯一编码</param>
        /// <returns>返回LNQ数据集</returns>
        public FM_FilePath GetFilePathInfo(Guid uniqueCode)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.FM_FilePath
                          where a.FileUnique == uniqueCode
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
        /// 获得文件分类的文件路径
        /// </summary>
        /// <param name="sortID">类别ID</param>
        /// <returns>返回文件路径</returns>
        public string SortPath(int sortID)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.FM_FileSort
                          where a.SortID == sortID
                          select a;

            if (varData.Count() == 1)
            {
                FM_FileSort lnqSort = varData.Single();

                if (lnqSort.ParentID == 0)
                {
                    return "/" + lnqSort.SortName;
                }
                else
                {
                    return SortPath((int)lnqSort.ParentID) + "/" + lnqSort.SortName;
                }
            }
            else
            {
                return "";
            }

        }

        /// <summary>
        /// 获得单一文件信息
        /// </summary>
        /// <param name="fileID">文件ID</param>
        /// <returns>返回LNQ数据集</returns>
        public FM_FileList GetFile(int fileID)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.FM_FileList
                          where a.FileID == fileID
                          select a;

            if (varData.Count() == 1)
            {
                return varData.Single();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 对相关文件进行FTP中的操作
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="fileID">文件ID</param>
        /// <param name="sortID">类别ID</param>
        /// <param name="error">错误信息</param>
        public void OperatorFTPSystemFile(DepotManagementDataContext ctx, int fileID, int sortID)
        {

            FileServiceSocket serverFTP = new FileServiceSocket(GlobalObject.GlobalParameter.FTPServerIP,
                GlobalObject.GlobalParameter.FTPServerAdvancedUser,
                GlobalObject.GlobalParameter.FTPServerAdvancedPassword);

            var varFile = from a in ctx.FM_FileList
                          where a.FileID == fileID
                          select a;

            FM_FileList lnqChangeFile = varFile.Single();
            FM_FileSort lnqSort = SortInfo(sortID);

            if (lnqSort == null)
            {
                lnqChangeFile.DeleteFlag = true;
            }
            else
            {
                lnqChangeFile.SortID = sortID;
            }

            if (serverFTP.Errormessage.Length != 0)
            {
                throw new Exception(serverFTP.Errormessage);
            }
        }

        /// <summary>
        /// 获得文件信息
        /// </summary>
        /// <param name="fileNo">文件编号</param>
        /// <param name="fileName">文件名称</param>
        /// <returns>返回Table</returns>
        public System.Data.DataTable GetFilesInfo(string fileNo, string fileName)
        {
            string strSql = "select * from FM_FileList where 1 = 1 ";

            if (fileNo != null && fileNo.Length != 0)
            {
                strSql += " and FileNo = '"+ fileNo +"'";
            }

            if (fileName != null && fileName.Length != 0)
            {
                strSql += " and FileName = '"+ fileName +"'";
            }

            strSql += " order by [Version] desc";

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        /// <summary>
        /// 获得文件类别信息
        /// </summary>
        /// <param name="sortID">类别ID</param>
        /// <returns>返回类别信息</returns>
        public FM_FileSort SortInfo(int sortID)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.FM_FileSort
                          where a.SortID == sortID
                          select a;

            if (varData.Count() == 1)
            {
                return varData.Single();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获得体系文件结构树
        /// </summary>
        /// <param name="fileType">文件类别</param>
        /// <returns></returns>
        public System.Data.DataTable GetTreeInfo(CE_FileType fileType)
        {
            string strSql = " select ID,Name,ParentID,FileNo from ( "+
                            " select CAST( SortID as varchar(50)) as ID , SortName as Name,  "+
                            " CAST( ParentID as varchar(50)) as ParentID, '' as FileNo, FileType  "+
                            " from FM_FileSort Union  "+
                            " select 'File' + CAST( FileID as varchar(50)),[FileName] + '(' + FileNo + ')' as [FileName], "+
                            "  CAST( a.SortID as varchar(50)), FileNo, b.FileType "+
                            " from FM_FileList as a inner join FM_FileSort as b on a.SortID = b.SortID   "+
                            " where DeleteFlag = 0) as a where FileType = " + (int)fileType;

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        /// <summary>
        /// 获得一定数量的文件信息
        /// </summary>
        /// <param name="listFileID">文件ID列表</param>
        /// <returns>返回Table</returns>
        public System.Data.DataTable GetFilesInfo(List<int> listFileID)
        {
            string strIn = "";

            if (listFileID.Count == 0)
            {
                return null;
            }

            foreach (int fileID in listFileID)
            {
                strIn += fileID.ToString() + ",";
            }

            strIn = strIn.Substring(0, strIn.Length - 1);

            string strSql = " select * from View_FM_FileInfo where 文件ID in (" + strIn + ") ";

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        /// <summary>
        /// 获得所有
        /// </summary>
        /// <param name="fileType">文件类别</param>
        /// <returns></returns>
        public System.Data.DataTable GetAllInfo(CE_FileType fileType)
        {
            string strSql = "select a.SortName as 类别名称,b.SortName as 父级类别名称, a.SortID as 类别ID, "+
                            " b.SortID as 父级类别ID from FM_FileSort as a left join FM_FileSort as b on "+
                            " a.ParentID = b.SortID where a.FileType = " + (int)fileType;

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        /// <summary>
        /// 对体系文件类别进行操作
        /// </summary>
        /// <param name="mode">操作类别</param>
        /// <param name="sort">LINQ数据集</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True,失败返回False</returns>
        public bool Operator(CE_OperatorMode mode, FM_FileSort sort, out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

                FM_FileSort lnqTemp = new FM_FileSort();

                var varData = from a in ctx.FM_FileSort
                              select a;

                switch (mode)
                {
                    case CE_OperatorMode.添加:

                        varData = from a in ctx.FM_FileSort
                                  where a.ParentID == sort.ParentID
                                  && a.SortName == sort.SortName
                                  select a;

                        if (varData.Count() > 0)
                        {
                            error = "数据重复";
                            return false;
                        }
                        else
                        {
                            lnqTemp.SortName = sort.SortName;
                            lnqTemp.ParentID = sort.ParentID;
                            lnqTemp.FileType = sort.FileType;

                            ctx.FM_FileSort.InsertOnSubmit(sort);
                        }

                        break;
                    case CE_OperatorMode.修改:

                        varData = from a in ctx.FM_FileSort
                                  where a.SortID == sort.SortID
                                  select a;

                        if (varData.Count() != 1)
                        {
                            error = "数据错误";
                            return false;
                        }
                        else
                        {
                            lnqTemp = varData.Single();

                            lnqTemp.SortName = sort.SortName;
                            lnqTemp.ParentID = sort.ParentID;
                        }

                        break;
                    case CE_OperatorMode.删除:

                        varData = from a in ctx.FM_FileSort
                                  where a.SortID == sort.SortID
                                  select a;

                        ctx.FM_FileSort.DeleteAllOnSubmit(varData);

                        break;
                    default:
                        break;
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

        /// <summary>
        /// 文件上传业务
        /// </summary>
        /// <param name="localhostFilePath">本机文件路径</param>
        /// <param name="ftpFilePath">FTP文件路径</param>
        /// <param name="guid">文件唯一编码</param>
        /// <param name="fileType">文件类型</param>
        public void FileUpLoad(string localhostFilePath, string ftpFilePath, string guid, string fileType)
        {
            FileServiceFTP serverFTP = new FileServiceFTP(GlobalObject.GlobalParameter.FTPServerIP,
                GlobalObject.GlobalParameter.FTPServerAdvancedUser,
                GlobalObject.GlobalParameter.FTPServerAdvancedPassword);

            serverFTP.Upload(localhostFilePath, ftpFilePath + guid);

            try
            {
                //生成SWF文件,以便在线阅读
                GlobalObject.WebServiceHelper.FileParse(ftpFilePath + guid, fileType.Substring(1));
            }
            catch (Exception)
            {
                return;
            }
        }

        /// <summary>
        /// 修改文件信息
        /// </summary>
        /// <param name="fileInfo">文件信息</param>
        public void ModifyFileInfo(FM_FileList fileInfo)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.FM_FileList
                          where a.FileID == fileInfo.FileID
                          select a;

            if (varData.Count() != 1)
            {
                throw new Exception("文件ID不唯一");
            }

            FM_FileList lnqTemp = varData.Single();

            lnqTemp.Department = fileInfo.Department;
            lnqTemp.FileName = fileInfo.FileName;
            lnqTemp.FileNo = fileInfo.FileNo;
            lnqTemp.SortID = fileInfo.SortID;
            lnqTemp.Version = fileInfo.Version;

            ctx.SubmitChanges();
        }

        public void DeleteFile(int fileId)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.FM_FileList
                          where a.FileID == fileId
                          select a;

            if (varData.Count() != 1)
            {
                throw new Exception("数据不唯一");
            }

            FileOperationService.File_Delete(varData.Single().FileUnique, CE_CommunicationMode.FTP);

            ctx.FM_FileList.DeleteAllOnSubmit(varData);
            ctx.SubmitChanges();
        }
    }
}

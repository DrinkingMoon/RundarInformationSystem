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
using System.IO;

namespace ServerModule
{
    /// <summary>
    /// 质量数据库服务
    /// </summary>
    class QualitySystemDatabase : IQualitySystemDatabase
    {
        /// <summary>
        /// FTP服务组件
        /// </summary>
        FileServiceSocket m_serverFTP = new FileServiceSocket(GlobalObject.GlobalParameter.FTPServerIP,
            GlobalObject.GlobalParameter.FTPServerAdvancedUser,
            GlobalObject.GlobalParameter.FTPServerAdvancedPassword);

        /// <summary>
        /// 检查某业务是否存在
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="billNo">业务号</param>
        /// <returns>存在返回true</returns>
        public bool IsExist(DepotManagementDataContext ctx, string billNo)
        {
            var varData = from a in ctx.ZL_Database_Record
                          where a.BillNo == billNo
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
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.ZL_Database_Record
                          where a.BillNo == billNo
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
        /// 获得设置信息
        /// </summary>
        /// <param name="settingName">设置类型名称</param>
        /// <returns>返回List集合</returns>
        public List<ZL_Database_Settings> GetSettingInfo(string settingName)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.ZL_Database_Settings
                          where a.Name == settingName
                          select a;

            if (varData == null)
            {
                return null;
            }
            else
            {
                return varData.ToList();
            }
        }

        /// <summary>
        /// 获得单条业务信息
        /// </summary>
        /// <param name="billNo">业务编号</param>
        /// <returns>返回LNQ数据集</returns>
        public ZL_Database_Record GetSingleInfo(string billNo)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.ZL_Database_Record
                          where a.BillNo == billNo
                          select a;

            if (varData == null || varData.Count() != 1)
            {
                return null;
            }
            else
            {
                return varData.Single();
            }
        }

        /// <summary>
        /// 获得业务信息列表
        /// </summary>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns>返回LIST列表信息</returns>
        public List<View_ZL_Database_Record> GetListInfo(DateTime startTime, DateTime endTime)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.View_ZL_Database_Record
                          where a.创建时间 >= startTime && a.创建时间 <= endTime
                          select a;

            return varData.ToList();
        }

        /// <summary>
        /// 获得FTP错误信息
        /// </summary>
        bool GetError()
        {
            if (m_serverFTP.Errormessage.Length != 0)
            {
                throw new Exception(m_serverFTP.Errormessage);
            }
            else
            {
                return true;
            }
        }

        void InsertFile(string path, string billNo)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            try
            {
                DirectoryInfo mydir = new DirectoryInfo(path);

                if (!mydir.Exists)
                {
                    return;
                }

                FileInfo[] fileList = mydir.GetFiles();

                foreach (FileInfo file in fileList)
                {
                    string filePath = file.FullName;
                    Guid guid = Guid.NewGuid();

                    string fileName = filePath.Substring(filePath.LastIndexOf("\\") + 1,
                        filePath.LastIndexOf(".") - (filePath.LastIndexOf("\\") + 1));
                    string fileType = filePath.Substring(filePath.LastIndexOf("."));
                    string strFtpServerPath = "/" + ServerTime.Time.Year.ToString() + "/" + ServerTime.Time.Month.ToString() + "/";

                    m_serverFTP.Upload(filePath, strFtpServerPath + guid);

                    if (GetError())
                    {
                        FM_FilePath lnqTemp = new FM_FilePath();

                        lnqTemp.FileUnique = guid;
                        lnqTemp.FileType = fileType.ToLower();
                        lnqTemp.FilePath = strFtpServerPath + guid;
                        lnqTemp.OperationDate = ServerTime.Time;

                        ctx.FM_FilePath.InsertOnSubmit(lnqTemp);
                        ctx.SubmitChanges();

                        ZL_Database_FileStruct tempLnq = new ZL_Database_FileStruct();

                        tempLnq.BillNo = billNo;
                        tempLnq.CreationTime = ServerTime.Time;
                        tempLnq.FileUnique = guid;
                        tempLnq.FileName = fileName;
                        tempLnq.ID = Guid.NewGuid();
                        tempLnq.LastModifyTime = ServerTime.Time;
                        tempLnq.ParentID = null;

                        InsertFileStruct(tempLnq);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 批量导入数据
        /// </summary>
        /// <param name="listRecord">数据集</param>
        public void BatchInportInfo(List<ZL_Database_Record> listRecord)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;
            BillNumberControl billNoControl = new BillNumberControl(CE_BillTypeEnum.质量数据库.ToString(), this);

            try
            {
                foreach (ZL_Database_Record record in listRecord)
                {
                    string billNo = billNoControl.GetNewBillNo();
                    InsertFile(record.BillNo, billNo);
                    record.BillNo = billNo;
                    ctx.ZL_Database_Record.InsertOnSubmit(record);
                    ctx.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 编辑业务信息
        /// </summary>
        /// <param name="record">LNQ业务信息集合</param>
        public void EditInfo(ZL_Database_Record record)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            try
            {
                if (record == null)
                {
                    throw new Exception("录入数据为空");
                }

                var varData = from a in ctx.ZL_Database_Record
                              where a.BillNo == record.BillNo
                              select a;

                if (varData == null || varData.Count() == 0)
                {
                    record.CreationTime = ServerTime.Time;

                    ctx.ZL_Database_Record.InsertOnSubmit(record);
                }
                else if(varData.Count() == 1)
                {
                    ZL_Database_Record tempLnq = varData.Single();

                    tempLnq.AssemblyCartonNo = record.AssemblyCartonNo;
                    tempLnq.CauseAnalysis = record.CauseAnalysis;
                    tempLnq.FaultRatio = record.FaultRatio;
                    tempLnq.Provider = record.Provider;
                    tempLnq.FaultDescription = record.FaultDescription;
                    tempLnq.FaultType = record.FaultType;
                    tempLnq.Finder = record.Finder;
                    tempLnq.FindPlaces = record.FindPlaces;
                    tempLnq.FindRole = record.FindRole;
                    tempLnq.GoodsCode = record.GoodsCode;
                    tempLnq.GoodsName = record.GoodsName;
                    tempLnq.Spec = record.Spec;
                    tempLnq.Mileage = record.Mileage;
                    tempLnq.Model = record.Model;
                    tempLnq.OccurrenceTime = record.OccurrenceTime;
                    tempLnq.TreatmentCountermeasures = record.TreatmentCountermeasures;
                    tempLnq.Type = record.Type;
                    tempLnq.Version = record.Version;
                    
                }
                else
                {
                    throw new Exception("相应的单据信息不唯一");
                }

                ctx.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 删除单据
        /// </summary>
        /// <param name="billNo">单据号</param>
        public void DeleteInfo(string billNo)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.ZL_Database_Record
                          where a.BillNo == billNo
                          select a;

            ctx.ZL_Database_Record.DeleteAllOnSubmit(varData);

            var varFile = from a in ctx.ZL_Database_FileStruct
                          where a.BillNo == billNo
                          select a;

            foreach (var item in varFile)
            {
                DeleteFileStruct(ctx, item.ID.ToString());
            }

            ctx.SubmitChanges();
        }

        /// <summary>
        /// 编辑文件结构信息
        /// </summary>
        /// <param name="fileStruct">LNQ文件结构信息集合</param>
        public void EditFileStruct(ZL_Database_FileStruct fileStruct)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            try
            {
                if (fileStruct == null)
                {
                    throw new Exception("录入数据信息为空");
                }

                var varData = from a in ctx.ZL_Database_FileStruct
                              where a.ID == fileStruct.ID
                              select a;

                if (varData == null || varData.Count() == 0)
                {
                    fileStruct.CreationTime = ServerTime.Time;

                    ctx.ZL_Database_FileStruct.InsertOnSubmit(fileStruct);
                }
                else if (varData.Count() == 1)
                {
                    ZL_Database_FileStruct tempLnq = varData.Single();

                    tempLnq.BillNo = fileStruct.BillNo;
                    tempLnq.FileName = fileStruct.FileName;
                    tempLnq.FileUnique = fileStruct.FileUnique;
                    tempLnq.LastModifyTime = ServerTime.Time;
                    tempLnq.ParentID = fileStruct.ParentID;
                }
                else
                {
                    throw new Exception("文件结构信息数据不唯一");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 获得文件结构
        /// </summary>
        /// <param name="guid">GUID</param>
        /// <param name="billNo">单据号</param>
        /// <returns>返回Table</returns>
        public DataTable GetAllFileStruct(string guid, string billNo)
        {
            string strSql = " select * from ZL_Database_FileStruct as a left join FM_FilePath as b on a.FileUnique = b.FileUnique" +
                            " where BillNo = '" + billNo + "'";

            if (guid == null || guid == "")
            {
                strSql += " and ParentID is null";
            }
            else
            {
                strSql += " and ParentID = '" + guid + "'";
            }

            DataTable tempTable = GlobalObject.DatabaseServer.QueryInfo(strSql);
            return tempTable;
        }

        /// <summary>
        /// 获得单条文件信息
        /// </summary>
        /// <param name="guid"> 唯一编码</param>
        /// <returns>返回数据集</returns>
        public ZL_Database_FileStruct GetFileStructInfo(string guid)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.ZL_Database_FileStruct
                          where a.ID == new Guid(guid)
                          select a;

            if (varData == null || varData.Count() != 1)
            {
                return null;
            }
            else
            {
                return varData.Single();
            }
        }

        /// <summary>
        /// 插入一条文件信息
        /// </summary>
        /// <param name="fileStruct">文件信息数据集</param>
        public void InsertFileStruct(ZL_Database_FileStruct fileStruct)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.ZL_Database_FileStruct
                          where a.ID == fileStruct.ID
                          select a;

            if (varData != null && varData.Count() == 0)
            {
                ctx.ZL_Database_FileStruct.InsertOnSubmit(fileStruct);
                ctx.SubmitChanges();
            }
        }

        /// <summary>
        /// 更新一条文件信息
        /// </summary>
        /// <param name="fileStruct">文件信息数据集</param>
        public void UpdateFileStruct(ZL_Database_FileStruct fileStruct)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.ZL_Database_FileStruct
                          where a.ID == fileStruct.ID
                          select a;

            if (varData != null && varData.Count() == 1)
            {
                ZL_Database_FileStruct tempLnq = varData.Single();

                tempLnq.BillNo = fileStruct.BillNo;
                tempLnq.FileName = fileStruct.FileName;
                tempLnq.FileUnique = fileStruct.FileUnique;
                tempLnq.LastModifyTime = fileStruct.LastModifyTime;
                tempLnq.ParentID = fileStruct.ParentID;

                ctx.SubmitChanges();
            }
        }

        /// <summary>
        /// 删除文件结构
        /// </summary>
        /// <param name="guid">唯一编码</param>
        public void DeleteFilesStruct(string guid)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            List<string> listTemp = GetBelongFiles(ctx, guid);

            foreach (string item in listTemp)
            {
                DeleteFileStruct(ctx, item);
            }

            ctx.SubmitChanges();
        }

        /// <summary>
        /// 删除一条文件信息,并且删除文件记录信息、FTP文件实体
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="guid">唯一编码</param>
        void DeleteFileStruct(DepotManagementDataContext ctx, string guid)
        {
            var varData = from a in ctx.ZL_Database_FileStruct
                          where a.ID == new Guid(guid)
                          select a;

            foreach (ZL_Database_FileStruct item in varData)
            {
                if (item.FileUnique != null)
                {
                    var varFilePath = from a in ctx.FM_FilePath
                                      where a.FileUnique == item.FileUnique
                                      select a;

                    foreach (FM_FilePath path in varFilePath)
                    {
                        m_serverFTP.Delete(path.FilePath);
                    }

                    ctx.FM_FilePath.DeleteAllOnSubmit(varFilePath);
                }
            }

            ctx.ZL_Database_FileStruct.DeleteAllOnSubmit(varData);
        }

        /// <summary>
        /// 获得所属文件唯一编码列表
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="guid">唯一编码</param>
        /// <returns>返回唯一编码字符串列表</returns>
        List<string> GetBelongFiles(DepotManagementDataContext ctx, string guid)
        {
            List<string> result = new List<string>();

            result.Add(guid);

            var varData = from a in ctx.ZL_Database_FileStruct
                          where a.ParentID == new Guid(guid)
                          select a;
            foreach (ZL_Database_FileStruct item in varData)
            {
                result.AddRange(GetBelongFiles(ctx, item.ID.ToString()));
            }

            return result;
        }
    }
}

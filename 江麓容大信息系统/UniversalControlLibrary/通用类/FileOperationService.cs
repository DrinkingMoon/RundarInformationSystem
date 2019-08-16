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
using GlobalObject;
using ServerModule;
using System.IO;

namespace UniversalControlLibrary
{
    /// <summary>
    /// 文件管理服务
    /// </summary>
    public static class FileOperationService
    {
        /// <summary>
        /// 文件上传
        /// </summary>
        /// <param name="guid">唯一编码</param>
        /// <param name="systemPath">系统路径</param>
        /// <param name="type">操作文件访问方式</param>
        public static void File_UpLoad(Guid guid, string systemPath, CE_CommunicationMode type)
        {
            try
            {
                DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

                var varData = from a in ctx.FM_FilePath
                              where a.FileUnique == guid
                              select a;

                if (varData.Count() == 0)
                {
                    FM_FilePath fileInfo = new FM_FilePath();

                    fileInfo.FileUnique = guid;
                    fileInfo.FilePath = 
                        "/" + ServerTime.Time.Year.ToString() + "/" + ServerTime.Time.Month.ToString() + "/" + fileInfo.FileUnique.ToString();
                    fileInfo.FileType = systemPath.Substring(systemPath.LastIndexOf("."));
                    fileInfo.OperationDate = ServerTime.Time;

                    ctx.FM_FilePath.InsertOnSubmit(fileInfo);

                    DateTime tempdateTime = ServerTime.Time;

                    if (type == CE_CommunicationMode.Socket)
                    {
                        FileServiceSocket serverSocket = new FileServiceSocket(GlobalObject.GlobalParameter.FTPServerIP,
                            GlobalObject.GlobalParameter.FTPServerAdvancedUser,
                            GlobalObject.GlobalParameter.FTPServerAdvancedPassword);
                        serverSocket.Upload(systemPath, fileInfo.FilePath);
                    }
                    else if (type == CE_CommunicationMode.FTP)
                    {
                        FileServiceFTP serverFTP = new FileServiceFTP(GlobalObject.GlobalParameter.FTPServerIP,
                            GlobalObject.GlobalParameter.FTPServerAdvancedUser,
                            GlobalObject.GlobalParameter.FTPServerAdvancedPassword);

                        serverFTP.Upload(systemPath, fileInfo.FilePath);
                    }

                    TimeSpan span = ServerTime.Time - tempdateTime;

                    ctx.SubmitChanges();
                }
                else
                {
                    throw new Exception("文件唯一编码重复");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 文件下载
        /// </summary>
        /// <param name="guid">文件唯一编码</param>
        /// <param name="systemPath">系统路径</param>
        /// <param name="type">操作文件访问方式</param>
        public static void File_DownLoad(Guid guid, string systemPath, CE_CommunicationMode type)
        {
            try
            {
                DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

                var varData = from a in ctx.FM_FilePath
                              where a.FileUnique == guid
                              select a;

                if (varData.Count() == 0)
                {
                    throw new Exception("文件不存在");
                }
                else if (varData.Count() == 1)
                {
                    if (type == CE_CommunicationMode.Socket)
                    {
                        FileServiceSocket serverSocket = new FileServiceSocket(GlobalObject.GlobalParameter.FTPServerIP,
                            GlobalObject.GlobalParameter.FTPServerAdvancedUser,
                            GlobalObject.GlobalParameter.FTPServerAdvancedPassword);
                        serverSocket.Download(varData.Single().FilePath, systemPath + varData.Single().FileType);
                    }
                    else if (type == CE_CommunicationMode.FTP)
                    {
                        FileServiceFTP serverFTP = new FileServiceFTP(GlobalObject.GlobalParameter.FTPServerIP,
                            GlobalObject.GlobalParameter.FTPServerAdvancedUser,
                            GlobalObject.GlobalParameter.FTPServerAdvancedPassword);
                        serverFTP.Download(varData.Single().FilePath, systemPath + varData.Single().FileType);
                    }
                }
                else
                {
                    throw new Exception("文件唯一编码重复");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 文件删除
        /// </summary>
        /// <param name="guid">文件唯一编码</param>
        /// <param name="type">操作文件访问方式</param>
        public static void File_Delete(Guid guid, CE_CommunicationMode type)
        {
            try
            {
                DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

                var varData = from a in ctx.FM_FilePath
                              where a.FileUnique == guid
                              select a;

                if (varData.Count() == 1)
                {
                    ctx.FM_FilePath.DeleteAllOnSubmit(varData);

                    if (type == CE_CommunicationMode.Socket)
                    {
                        FileServiceSocket serverSocket = new FileServiceSocket(GlobalObject.GlobalParameter.FTPServerIP,
                            GlobalObject.GlobalParameter.FTPServerAdvancedUser,
                            GlobalObject.GlobalParameter.FTPServerAdvancedPassword);
                        serverSocket.Delete(varData.Single().FilePath);
                    }
                    else if (type == CE_CommunicationMode.FTP)
                    {
                        FileServiceFTP serverFTP = new FileServiceFTP(GlobalObject.GlobalParameter.FTPServerIP,
                            GlobalObject.GlobalParameter.FTPServerAdvancedUser,
                            GlobalObject.GlobalParameter.FTPServerAdvancedPassword);
                        serverFTP.Delete(varData.Single().FilePath);
                    }

                    ctx.SubmitChanges();
                }
                else if(varData.Count() == 0)
                {
                    return;
                }
                else
                {
                    throw new Exception("文件唯一编码重复");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 查看
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="type"></param>
        public static void File_Look(Guid guid, CE_CommunicationMode type)
        {
            try
            {
                DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

                var varData = from a in ctx.FM_FilePath
                              where a.FileUnique == guid
                              select a;

                if (varData.Count() == 1)
                {
                    FM_FilePath tempPath = varData.Single();
                    string temp = System.Environment.GetEnvironmentVariable("TEMP");
                    DirectoryInfo info = new DirectoryInfo(temp);
                    string guidFile = (Guid.NewGuid()).ToString() + "\\";
                    string filePath = info.FullName + "\\" + guidFile + tempPath.FileUnique + tempPath.FileType;

                    if (type == CE_CommunicationMode.Socket)
                    {
                        FileServiceSocket serverSocket = new FileServiceSocket(GlobalObject.GlobalParameter.FTPServerIP,
                            GlobalObject.GlobalParameter.FTPServerAdvancedUser,
                            GlobalObject.GlobalParameter.FTPServerAdvancedPassword);
                        serverSocket.Download(varData.Single().FilePath, filePath);
                    }
                    else if (type == CE_CommunicationMode.FTP)
                    {
                        FileServiceFTP serverFTP = new FileServiceFTP(GlobalObject.GlobalParameter.FTPServerIP,
                            GlobalObject.GlobalParameter.FTPServerAdvancedUser,
                            GlobalObject.GlobalParameter.FTPServerAdvancedPassword);
                        serverFTP.Download(varData.Single().FilePath, filePath);
                    }

                    System.Diagnostics.Process.Start(filePath);
                }
                else
                {
                    throw new Exception("文件唯一编码重复");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 写入TXT文本文件
        /// </summary>
        /// <param name="txt">文本内容</param>
        /// <param name="filePath">文件路径</param>
        public static void Write_TxtFile(string txt, string filePath)
        {
            string path = filePath.Substring(0, filePath.LastIndexOf("\\") + 1);
            string fileName = filePath.Substring(filePath.LastIndexOf("\\") + 1);

            if (fileName.Trim().Length == 0)
            {
                fileName = "TempName.txt";
            }

            if (!fileName.ToLower().Contains(".txt"))
            {
                fileName += ".txt";
            }

            if (!Directory.Exists(path))
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(path);
                directoryInfo.Create();
            }

            FileStream fs = new FileStream(path + fileName, FileMode.Create);
            //获得字节数组
            byte[] data = System.Text.Encoding.Default.GetBytes(txt);
            //开始写入
            fs.Write(data, 0, data.Length);
            //清空缓冲区、关闭流
            fs.Flush();
            fs.Close();
        }
    }
}

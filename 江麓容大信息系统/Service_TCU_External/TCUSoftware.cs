using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Net.Sockets;
using System.Collections;

namespace Service_TCU_External
{
    public class TCUSoftware
    {
        #region

        string _ConnetionString =
            "Password=meimima123;Persist Security Info=True;User ID=InfoSysUser;Initial Catalog=DepotManagement;Data Source=113.240.226.82,1467";

        string _ServerIP = "ftp://113.240.226.82:21/";
        string _UserName = "AdvUser_QualityFile";
        string _PassWord = "~!_qweASD98";

        /// <summary>
        /// 下载进度
        /// </summary>
        /// <param name="percent">进度数值</param>
        public delegate void DownloadPercent(float percent);

        /// <summary>
        /// 软件版本状态
        /// </summary>
        public enum VersionStatus
        {
            /// <summary>
            /// 空
            /// </summary>
            Null,

            /// <summary>
            /// 测试
            /// </summary>
            Test,

            /// <summary>
            /// 发布
            /// </summary>
            Release,
        }

        #endregion

        /// <summary>
        /// 是否服务器连接成功
        /// </summary>
        /// <returns>成功返回TRUE, 失败返回False</returns>
        public bool IsConnecting()
        {
            try
            {
                string[] s = _ConnetionString.Split(';');
                s = s[s.Length - 1].Split('=');
                //获取IP 
                string serverIP = s[1];

                //发送数据，判断是否连接到指定ip 
                if (!TestConnection(serverIP, 1433, 500))
                {
                    throw new Exception("Socket Link Failed");
                }

                if (!TestConnection(_ConnetionString))
                {
                    throw new Exception("Sql Link Failed");
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 程序下载
        /// </summary>
        /// <param name="modelCode">车型代码</param>
        /// <param name="status">软件版本状态</param>
        /// <param name="percent">下载进度</param>
        /// <returns>成功返回TRUE， 失败返回False</returns>
        public bool DownloadFile(string modelCode, VersionStatus status, DownloadPercent percent)
        {
            try
            {
                if (modelCode == null || status == VersionStatus.Null)
                {
                    throw new Exception("参数值无效");
                }

                string currerntFilePath = System.Environment.CurrentDirectory + "\\TCU程序\\" + modelCode + "\\" + status.ToString() + "\\";

                if (!Directory.Exists(currerntFilePath))
                {
                    DirectoryInfo directoryInfo = new DirectoryInfo(currerntFilePath);
                    directoryInfo.Create();
                }

                System.IO.DirectoryInfo dir = new DirectoryInfo(currerntFilePath);
                bool isDownload = false;

                if (dir.GetFiles().Length == 0)
                {
                    isDownload = true;
                }
                else
                {
                    List<FileInfo> lstFile = dir.GetFiles().Where(k => k.Name.Contains("TCU程序")).ToList();

                    if (lstFile.Count() == 0)
                    {
                        isDownload = true;
                    }
                    else
                    {
                        string fileName = lstFile[0].Name;

                        DataTable tempTable = GetSoftWareInfo(modelCode, status);

                        if (tempTable == null || tempTable.Rows.Count == 0)
                        {
                            throw new Exception("无法获得文件信息");
                        }

                        fileName = fileName.Substring(fileName.IndexOf("(") + 1, fileName.IndexOf(")") - fileName.IndexOf("(") - 1);
                        string version = tempTable.Rows[0]["Version"].ToString();

                        if (string.Compare(version, fileName) > 0)
                        {
                            isDownload = true;
                        }
                    }
                }

                if (isDownload)
                {
                    foreach (FileInfo fi in dir.GetFiles())
                    {
                        fi.Delete();
                    }

                    Download_TxtFile(modelCode, status, currerntFilePath);
                    Download_SoftFile(modelCode, status, currerntFilePath, percent);
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 获取文件存放路径
        /// </summary>
        /// <returns>返回文件存放路径</returns>
        public string GetFilePath(string modelCode, VersionStatus status)
        {
            try
            {
                string path = null;

                if (modelCode == null || status == VersionStatus.Null)
                {
                    throw new Exception("参数值无效");
                }

                path = System.Environment.CurrentDirectory + "\\TCU程序\\" + modelCode + "\\" + status.ToString() + "\\";

                return path;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #region 私有方法

        void DownloadFile(string filePathLocal, string path, DownloadPercent percent)
        {
            string filePath = filePathLocal.Substring(0, filePathLocal.LastIndexOf('\\') + 1);

            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            FileStream outputStream = new FileStream(filePathLocal, FileMode.Create);

            FtpWebRequest reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(_ServerIP + path.Replace("/", "//")));
            reqFTP.Credentials = new NetworkCredential(_UserName, _PassWord);
            reqFTP.Method = WebRequestMethods.Ftp.DownloadFile;
            reqFTP.UseBinary = true;
            reqFTP.UsePassive = false;

            FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
            Stream ftpStream = response.GetResponseStream();

            long cl = GetFileSize(path);
            long totalDownloadedByte = 0;
            int bufferSize = 1024;
            int readCount;

            byte[] buffer = new byte[bufferSize];

            readCount = ftpStream.Read(buffer, 0, bufferSize);

            while (readCount > 0)
            {
                totalDownloadedByte = readCount + totalDownloadedByte;
                outputStream.Write(buffer, 0, readCount);

                float fl = (float)totalDownloadedByte / (float)cl * 100;

                if (percent != null)
                {
                    percent(fl);
                }

                readCount = ftpStream.Read(buffer, 0, bufferSize);
            }

            ftpStream.Close();
            outputStream.Close();
            response.Close();
        }

        void Download_SoftFile(string modelCode, VersionStatus status, string filePathLocal, DownloadPercent percent)
        {
            try
            {
                DataTable infoTable = GetSoftWareInfo(modelCode, status);

                if (infoTable == null || infoTable.Rows.Count == 0)
                {
                    throw new Exception("未查询到相关信息");
                }

                string versionTxt = "";

                if (infoTable.Rows[0]["Version"].ToString().Trim().Length > 10)
                {
                    versionTxt = infoTable.Rows[0]["Version"].ToString().Trim().Substring(0, infoTable.Rows[0]["Version"].ToString().Trim().Length - 1) + ")"
                        + infoTable.Rows[0]["Version"].ToString().Trim().Substring(infoTable.Rows[0]["Version"].ToString().Trim().Length - 1);
                }
                else
                {
                    versionTxt = infoTable.Rows[0]["Version"].ToString().Trim() + ")";
                }

                DownloadFile(filePathLocal
                    + "TCU程序(" + versionTxt + infoTable.Rows[0]["FileType"].ToString(),
                    infoTable.Rows[0]["FilePath"].ToString(), percent);

                DownloadFile(filePathLocal 
                    + infoTable.Rows[0]["DLLName"].ToString() 
                    + infoTable.Rows[0]["DLLFileType"].ToString(), 
                    infoTable.Rows[0]["DLLFilePath"].ToString(), percent);
            }
            catch (Exception exec)
            {
                throw new Exception(string.Format("异常位置: {0}, 详细信息: {1}", exec.StackTrace, exec.Message));
            }
        }

        void Download_TxtFile(string modelCode, VersionStatus status, string filePath)
        {
            try
            {
                DataTable infoTable = GetSoftWareInfo(modelCode, status);

                if (infoTable == null || infoTable.Rows.Count == 0)
                {
                    throw new Exception("未查询到相关信息");
                }

                string versionTxt = "";

                if (infoTable.Rows[0]["Version"].ToString().Trim().Length > 10)
                {
                    versionTxt = infoTable.Rows[0]["Version"].ToString().Trim().Substring(0, 
                        infoTable.Rows[0]["Version"].ToString().Trim().Length - 1);
                }
                else
                {
                    versionTxt = infoTable.Rows[0]["Version"].ToString().Trim();
                }

                string QRCode = "";

                QRCode += "供应商代码," + infoTable.Rows[0]["QRCode_Provider"].ToString().Trim() + "\r\n";
                QRCode += "零部件种类状态代码," + infoTable.Rows[0]["QRCode_PartsType"].ToString().Trim() + "\r\n";
                QRCode += "容大零件图号," + infoTable.Rows[0]["QRCode_PartsCode"].ToString().Trim() + "\r\n";
                QRCode += "软件版本," + versionTxt + "\r\n";
                QRCode += "工厂代号匹配电阻," + infoTable.Rows[0]["QRCode_FactoryCode"].ToString().Trim() + "\r\n";

                Write_TxtFile(QRCode, filePath + "TCU二维码信息.txt");

                string markingCode = "";

                foreach (DataRow dr in infoTable.Rows)
                {
                    markingCode += dr["DID"].ToString() + "," + dr["DataContent"].ToString() + "\r\n";
                }

                Write_TxtFile(markingCode, filePath + "TCU数据标识符.txt");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        DataTable GetSoftWareInfo(string modelCode, VersionStatus status)
        {
            string error = null;

            Hashtable hstable = new Hashtable();

            hstable.Add("@CarModelNo", modelCode);
            DataTable tempTable = QueryInfoPro("TCU_CarModelInfo_Software_GetList", hstable, out error);

            if (tempTable == null || tempTable.Rows.Count == 0)
            {
                return null;
            }

            string versionStatus = null;

            switch (status)
            {
                case VersionStatus.Null:
                    versionStatus = null;
                    break;
                case VersionStatus.Test:
                    versionStatus = "测试版";
                    break;
                case VersionStatus.Release:
                    versionStatus = "正式版";
                    break;
                default:
                    break;
            }

            DataRow[] drArrary = tempTable.Select("版本状态 = '" + versionStatus + "'", "版本号");

            if (drArrary == null || drArrary.Count() == 0)
            {
                return null;
            }

            string billNo = drArrary[drArrary.Count() - 1]["业务编号"] == null ? null : drArrary[drArrary.Count() - 1]["业务编号"].ToString();

            string strSql = " select * from (select d.FilePath, d.FileType, a.Version, b.*, c.*, e.DLLFilePath, e.DLLName, e.DLLFileType " +
                            " from Business_Project_TCU_SoftwareUpdate as a "+
                            " inner join Business_Project_TCU_SoftwareUpdate_DID as b on a.BillNo = b.BillNo "+
                            " inner join TCU_CarModelInfo_Tradition as c on a.CarModelNo = c.CarModelNo "+
                            " inner join FM_FilePath as d on a.ProgramUnique = d.FileUnique "+
                            " inner join (select a.DLLName, b.FilePath as DLLFilePath, b.FileType as DLLFileType, a.CarModelNo "+
                            " from TCU_CarModelInfo as a inner join FM_FilePath as b on a.DLLFileUnique = b.FileUnique) as e "+
                            " on a.CarModelNo = e.CarModelNo) as a where a.BillNo = '" + billNo + "'";

            DataTable tempTable1 = QueryInfo(strSql);
            return tempTable1;
        }

        /// <summary>
        /// 获取文件大小
        /// </summary>
        /// <returns>文件大小</returns>
        Int64 GetFileSize(string path)
        {
            FtpWebRequest request;

            try
            {
                request = (FtpWebRequest)FtpWebRequest.Create(new Uri(_ServerIP + path.Replace("/", "//")));
                request.Method = WebRequestMethods.Ftp.DownloadFile;
                request.UseBinary = true;
                request.Credentials = new NetworkCredential(_UserName, _PassWord);
                request.Method = WebRequestMethods.Ftp.GetFileSize;

                long dataLength = (int)request.GetResponse().ContentLength;

                return dataLength;
            }
            catch (Exception ex)
            {
                Console.WriteLine("获取文件大小出错：" + ex.Message);
                return -1;
            }
        }

        /// <summary> 
        /// 数据库连接操作，可替换为你自己的程序 
        /// </summary> 
        /// <param name="ConnectionString">连接字符串</param> 
        /// <returns></returns> 
        bool TestConnection(string ConnectionString)
        {
            bool result = true;
            try
            {
                SqlConnection m_myConnection = new SqlConnection(ConnectionString);
                m_myConnection.Open();
                //数据库操作...... 
                m_myConnection.Close();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                result = false;
            }
            return result;
        }

        /// <summary> 
        /// 采用Socket方式，测试服务器连接 
        /// </summary> 
        /// <param name="host">服务器主机名或IP</param> 
        /// <param name="port">端口号</param> 
        /// <param name="millisecondsTimeout">等待时间：毫秒</param> 
        /// <returns></returns> 
        bool TestConnection(string host, int port, int millisecondsTimeout)
        {
            TcpClient client = new TcpClient();
            try
            {
                var ar = client.BeginConnect(host, port, null, null);
                ar.AsyncWaitHandle.WaitOne(millisecondsTimeout);
                return client.Connected;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                client.Close();
            }
        }

        /// <summary>
        ///  执行存储过程返回得到的数据库表
        /// </summary>
        /// <param name="sp_cmd">存储过程名称</param>
        /// <param name="sp_Input_Params">哈希数据集</param>
        /// <returns>成功返回查询到的数据表, 失败返回null</returns>
        DataTable QueryInfoPro(string sp_cmd, Hashtable sp_Input_Params, out string error)
        {
            error = null;

            try
            {
                DataTable tempTable = new DataTable();

                using (SqlConnection sqlconn = new SqlConnection(_ConnetionString))
                {
                    using (SqlDataAdapter da = new SqlDataAdapter(sp_cmd, sqlconn))
                    {
                        DataSet ds = new DataSet();

                        da.SelectCommand = new SqlCommand();

                        da.SelectCommand.CommandType = CommandType.StoredProcedure;
                        da.SelectCommand.CommandText = sp_cmd;
                        da.SelectCommand.Connection = sqlconn;
                        da.SelectCommand.CommandTimeout = 40000;

                        if (sp_Input_Params != null)
                        {
                            System.Collections.IDictionaryEnumerator InputEnume = sp_Input_Params.GetEnumerator();

                            while (InputEnume.MoveNext())
                            {
                                SqlParameter param = new SqlParameter();

                                param.Direction = ParameterDirection.Input;
                                param.ParameterName = InputEnume.Key.ToString();
                                param.Value = InputEnume.Value;

                                da.SelectCommand.Parameters.Add(param);
                            }
                        }

                        int rowcount = da.Fill(ds);

                        if (rowcount == 0 && ds != null && ds.Tables.Count == 0)
                        {
                            error = "没有找到任何数据";
                        }
                        else
                        {
                            tempTable = ds.Tables[ds.Tables.Count - 1];
                        }
                    }
                }

                return tempTable;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return null;
            }
        }

        /// <summary>
        /// 执行SQL查询语句返回得到的数据库表
        /// </summary>
        /// <param name="strSql">SQL查询语句</param>
        /// <returns>成功返回查询到的数据表, 失败返回null</returns>
        DataTable QueryInfo(string strSql)
        {
            if (strSql == null || strSql.Trim().Length < 6 || strSql.Trim().Substring(0, 6).ToLower() != "select")
            {
                return null;
            }

            try
            {
                using (SqlConnection sqlconn = new SqlConnection(_ConnetionString))
                {
                    SqlDataAdapter sqlda = new SqlDataAdapter(strSql, sqlconn.ConnectionString);

                    DataTable dt = new DataTable();
                    sqlda.Fill(dt);

                    return dt;
                }
            }
            catch (Exception exce)
            {
                Console.WriteLine(exce.Message);
                return null;
            }
        }

        void Write_TxtFile(string txt, string filePath)
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

        #endregion
    }
}

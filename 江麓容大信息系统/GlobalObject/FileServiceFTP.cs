using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;
using System.Threading;

namespace GlobalObject
{
    public class FileServiceFTP : IDisposable
    {
        /// <summary>
        /// 用户名
        /// </summary>
        private string m_userName;

        /// <summary>
        /// 密码
        /// </summary>
        private string m_pwd;

        /// <summary>
        /// 服务器IP地址
        /// </summary>
        private string m_serverIP = "";

        /// <summary>
        /// 线程是否运行的标志
        /// </summary>
        private bool m_runThread = false;

        /// <summary>
        /// 上传线程
        /// </summary>
        private Thread m_uploadThread;

        /// <summary>
        /// 文件信息对
        /// </summary>
        struct FileInfoPair
        {
            /// <summary>
            /// 本地文件
            /// </summary>
            public string SourceFileName;

            /// <summary>
            /// FTP 目录
            /// </summary>
            public string FtpPath;
        }

        /// <summary>
        /// 上传文件队列
        /// </summary>
        private Queue<FileInfoPair> m_queueUploadFile = new Queue<FileInfoPair>();

        /// <summary>
        /// 构造函数
        /// </summary>
        public FileServiceFTP()
        {
            m_userName = null;
            m_pwd = null;
            m_serverIP = null;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        public FileServiceFTP(string serverIP, string userName, string password)
        {
            if (GlobalObject.GeneralFunction.IsNullOrEmpty(serverIP))
            {
                throw new Exception("操作服务器IP地址不能为空");
            }

            if (GlobalObject.GeneralFunction.IsNullOrEmpty(userName) || GlobalObject.GeneralFunction.IsNullOrEmpty(password))
            {
                throw new Exception("FTP 文件操作时，用户名或密码不允许为空");
            }

            m_userName = userName;
            m_pwd = password;

            if (serverIP.Substring(0, 3) == "10.")
            {
                serverIP = "10.30.17.18";
            }

            m_serverIP = @"ftp://" + serverIP + ":" + GlobalParameter.FTPServerPort + "/";
        }

        /// <summary>  
        /// 取得本地目录的文件名列表
        /// </summary>  
        /// <param name="localPath">本地文件路径</param>  
        /// <returns>返回获取到的文件名数组</returns>  
        string[] GetLocalFileName(string localPath)
        {
            return Directory.GetFiles(localPath);
        }

        /// <summary>
        /// 判断文件的目录是否存,不存则创建
        /// </summary>
        /// <param name="destFilePath">目标文件目录</param>
        void IsExistsOfFTPDirectory(string destFilePath)
        {
            string fullDir = ParseFTPDirectory(destFilePath);
            string[] dirs = fullDir.Split('/');
            string curDir = "";

            for (int i = 0; i < dirs.Length; i++)
            {
                string dir = dirs[i];

                //如果是以/开始的路径,第一个为空    
                if (dir != null && dir.Length > 0)
                {
                    try
                    {
                        curDir += dir + "/";
                        MakeFTPDir(curDir);
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// 处理FTP目录，去掉最后‘/’后续的部分
        /// </summary>
        /// <param name="destFilePath">目录名称</param>
        /// <returns>返回处理后的目录名称</returns>
        string ParseFTPDirectory(string destFilePath)
        {
            int index = destFilePath.LastIndexOf("/");

            if (index == -1)
            {
                return destFilePath;
            }

            return destFilePath.Substring(0, destFilePath.LastIndexOf("/"));
        }

        /// <summary>
        /// 创建目录
        /// </summary>
        /// <param name="ftpPath">FTP 文件目录</param>
        /// <returns>成功返回true</returns>
        void MakeFTPDir(string ftpPath)
        {
            FtpWebRequest reqFTP = (FtpWebRequest)WebRequest.Create(m_serverIP + ftpPath);
            reqFTP.Credentials = new NetworkCredential(m_userName, m_pwd);
            reqFTP.Method = WebRequestMethods.Ftp.MakeDirectory;

            try
            {
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                response.Close();
            }
            catch (Exception)
            {
                reqFTP.Abort();
            }

            reqFTP.Abort();
        }

        /// <summary>
        /// 获取文件大小
        /// </summary>
        /// <param name="file">ip服务器下的相对路径</param>
        /// <returns>文件大小</returns>
        public int GetFileSize(string file)
        {
            StringBuilder result = new StringBuilder();
            FtpWebRequest request;
            try
            {
                request = (FtpWebRequest)FtpWebRequest.Create(new Uri(m_serverIP + file.Replace("/", "//")));
                request.UseBinary = true;
                request.Credentials = new NetworkCredential(m_userName, m_pwd);
                request.Method = WebRequestMethods.Ftp.GetFileSize;

                int dataLength = (int)request.GetResponse().ContentLength;

                return dataLength;
            }
            catch (Exception ex)
            {
                Console.WriteLine("获取文件大小出错：" + ex.Message);
                return -1;
            }
        }

        /// <summary>  
        /// 取得文件名  
        /// </summary>  
        /// <param name="ftpPath">ftp路径</param>  
        /// <returns>返回获取到的文件名数组</returns>  
        string[] GetFileName(string ftpPath)
        {
            StringBuilder result = new StringBuilder();
            FtpWebRequest reqFTP;

            try
            {
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(m_serverIP + ftpPath));
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(m_userName, m_pwd);
                reqFTP.Method = WebRequestMethods.Ftp.ListDirectory;
                reqFTP.UsePassive = false;

                WebResponse response = reqFTP.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());
                string line = reader.ReadLine();

                while (line != null)
                {
                    result.Append(line);
                    result.Append("\n");
                    line = reader.ReadLine();
                }

                result.Remove(result.ToString().LastIndexOf('\n'), 1);
                reader.Close();
                response.Close();
                return result.ToString().Split('\n');
            }
            catch (Exception exec)
            {
                throw new Exception(string.Format("异常位置: {0}, 详细信息: {1}", exec.StackTrace, exec.Message));
            }
        }
                
        /// <summary>
        /// 上传本地目录的所有文件到ftp目录（不对本地目录进行递归运算，无法上传子目录文件）
        /// </summary>
        /// <param name="localFilePath">本地目录</param>
        /// <param name="ftpPath">上传到的目标目录</param>
        public void UploadPath(string localFilePath, string ftpPath)
        {
            if (!Directory.Exists(localFilePath))
            {
                return;
            }

            string[] localFileList = GetLocalFileName(localFilePath);

            if (localFileList == null || localFileList.Length == 0)
            {
                return;
            }

            IsExistsOfFTPDirectory(ftpPath);

            foreach (string fileName in localFileList)
            {
                FileInfoPair fileInfo = new FileInfoPair();

                fileInfo.SourceFileName = fileName;
                fileInfo.FtpPath = ftpPath;

                m_queueUploadFile.Enqueue(fileInfo);
            }

            if (m_uploadThread == null)
            {
                //ParameterizedThreadStart ParStart = new ParameterizedThreadStart(UploadPathThread);
                //m_uploadThread = new Thread(ParStart);

                m_uploadThread = new Thread(new ThreadStart(this.UploadPathThread));
                m_uploadThread.IsBackground = true;

                m_runThread = true;

                m_uploadThread.Start();

                //string[] path = new string[] { localFilePath, ftpPath };          
                //thread.Start(path);
            }
        }
                
        /// <summary>
        /// 线程执行体：上传本地目录的所有文件到ftp目录（不对本地目录进行递归运算，无法上传子目录文件）
        /// </summary>
        private void UploadPathThread()
        {
            while (m_runThread)
            {
                if (m_queueUploadFile.Count == 0)
                {
                    Thread.Sleep(0);
                    continue;
                }

                DateTime dt = DateTime.Now;

                lock (m_queueUploadFile)
                {
                    FileInfoPair fileInfo = m_queueUploadFile.Peek();

                    Upload(fileInfo.SourceFileName, fileInfo.FtpPath);

                    File.Delete(fileInfo.SourceFileName);

                    m_queueUploadFile.Dequeue();

                    Thread.Sleep(0);
                }

                TimeSpan sp = DateTime.Now - dt;
                int a = 0;
                a++;
            }
        }

        /// <summary>
        /// ftp的上传功能
        /// </summary>
        /// <param name="filename">本地文件名</param>
        /// <param name="remotefilename">远程要覆盖的文件名</param>
        public void Upload(string filename, string remotefilename)
        {
            IsExistsOfFTPDirectory(remotefilename);
            FileInfo fileInf = new FileInfo(filename);
            FtpWebRequest reqFTP;

            string filePath = m_serverIP + remotefilename.Replace("/", "//");

            // 根据uri创建FtpWebRequest对象   
            reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(m_serverIP + remotefilename.Replace("/", "//")));
            reqFTP.Proxy = null;
            
            // ftp用户名和密码  
            reqFTP.Credentials = new NetworkCredential(m_userName, m_pwd);
            reqFTP.UsePassive = true;

            // 默认为true，连接不会被关闭  
            // 在一个命令之后被执行  
            reqFTP.KeepAlive = false;

            // 指定执行什么命令  
            reqFTP.Method = WebRequestMethods.Ftp.UploadFile;

            // 指定数据传输类型  
            reqFTP.UseBinary = true;

            // 上传文件时通知服务器文件的大小  
            reqFTP.ContentLength = fileInf.Length;

            // 缓冲大小设置为2kb  
            int buffLength = 2048;
            byte[] buff = new byte[buffLength];
            int contentLen;

            // 打开一个文件流 (System.IO.FileStream) 去读上传的文件  
            FileStream fs = fileInf.OpenRead();

            try
            {
                // 把上传的文件写入流  
                Stream strm = reqFTP.GetRequestStream();

                // 每次读文件流的2kb  
                contentLen = fs.Read(buff, 0, buffLength);

                // 流内容没有结束  
                while (contentLen != 0)
                {
                    // 把内容从file stream 写入 upload stream  
                    strm.Write(buff, 0, contentLen);
                    contentLen = fs.Read(buff, 0, buffLength);
                }

                // 关闭两个流  
                strm.Close();
                fs.Close();
            }
            catch (Exception exec)
            {
                throw new Exception(string.Format("异常位置: {0}, 详细信息: {1}", exec.StackTrace, exec.Message));
            }
        }

        /// <summary>
        /// 删除FTP目录上的指定文件
        /// </summary>
        /// <param name="ftpPath">FTP文件名称(包含路径)</param>
        public void Delete(string path)
        {
            try
            {
                FtpWebRequest reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(m_serverIP + path.Replace("/", "//")));
                reqFTP.Method = WebRequestMethods.Ftp.DeleteFile;
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(m_userName, m_pwd);
                reqFTP.UsePassive = false;

                FtpWebResponse listResponse = (FtpWebResponse)reqFTP.GetResponse();
                string sStatus = listResponse.StatusDescription;
            }
            catch (Exception exec)
            {
                throw new Exception(string.Format("异常位置: {0}, 详细信息: {1}", exec.StackTrace, exec.Message));
            }
        }

        /// <summary>
        /// 从ftp服务器上下载文件的功能  
        /// </summary>
        /// <param name="filename">远程文件名称</param>
        /// <param name="localfilename">本地文件名</param>
        public void Download(string remote_filename, string localfilename)
        {
            try
            {
                string filePath = localfilename.Substring(0, localfilename.LastIndexOf('\\') + 1);
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }

                FileStream outputStream = new FileStream(localfilename, FileMode.Create);

                FtpWebRequest reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(m_serverIP + remote_filename.Replace("/", "//")));
                reqFTP.Proxy = new WebProxy();
                reqFTP.Proxy = null;
                reqFTP.Method = WebRequestMethods.Ftp.DownloadFile;
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(m_userName, m_pwd);
                reqFTP.UsePassive = true;
                reqFTP.KeepAlive = true;

                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                Stream ftpStream = response.GetResponseStream();

                long cl = response.ContentLength;
                int bufferSize = 2048;
                int readCount;
                byte[] buffer = new byte[bufferSize];

                readCount = ftpStream.Read(buffer, 0, bufferSize);

                while (readCount > 0)
                {
                    outputStream.Write(buffer, 0, readCount);
                    readCount = ftpStream.Read(buffer, 0, bufferSize);
                }

                ftpStream.Close();
                outputStream.Close();
                response.Close();
            }
            catch (Exception exec)
            {
                throw new Exception(string.Format("异常位置: {0}, 详细信息: {1}", exec.StackTrace, exec.Message));
            }
        }

        #region IDisposable 成员

        /// <summary>
        /// 销毁对象
        /// </summary>
        public void Dispose()
        {
            m_runThread = false;

            Thread.Sleep(200);
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Expression;
using GlobalObject;
using UniversalControlLibrary.Properties;
using UniversalControlLibrary;
using System.IO;
using System.Diagnostics;
using System.Threading;

namespace UniversalControlLibrary
{
    public partial class 文件操作方式 : Form
    {
        /// <summary>
        /// 文件外部路径，文件内部路径，文件名
        /// </summary>
        string m_filePathExternal, m_filePathInternal, m_fileName;

        /// <summary>
        /// 后缀名
        /// </summary>
        string m_fileNameSuffix;

        /// <summary>
        /// 文件操作方式列表
        /// </summary>
        List<CE_FileOperatorType> m_listShow;

        /// <summary>
        /// 文件类型枚举
        /// </summary>
        CE_SystemFileType m_fileType = CE_SystemFileType.Miss;

        /// <summary>
        /// FTP服务组件
        /// </summary>
        FileServiceSocket m_serverFileFtp = new FileServiceSocket(GlobalObject.GlobalParameter.FTPServerIP,
            GlobalObject.GlobalParameter.FTPServerAdvancedUser,
            GlobalObject.GlobalParameter.FTPServerAdvancedPassword);

        public 文件操作方式(List<CE_FileOperatorType> listShow, 
            string filePathExternal, string filePathInternal, string fileType)
        {
            InitializeComponent();

            m_filePathExternal = filePathExternal;
            m_filePathInternal = filePathInternal;

            m_fileNameSuffix = fileType.Contains(".") ? fileType : "." + fileType;

            m_fileType = GetDocumentType(fileType);

            string temp = m_filePathInternal.Substring(m_filePathInternal.LastIndexOf(@"/") + 1);
            m_fileName = temp.Contains(".") ? temp.Substring(0, temp.LastIndexOf(".")) : temp;

            m_listShow = listShow;
        }

        private void 文件操作方式_Load(object sender, EventArgs e)
        {
            if (m_listShow.Count == 1)
            {
                switch (m_listShow[0])
                {
                    case CE_FileOperatorType.上传:
                        FileUpLoad();
                        break;
                    case CE_FileOperatorType.下载:
                        FileDownLoad();
                        break;
                    case CE_FileOperatorType.在线编辑:
                        FileEdit();
                        break;
                    case CE_FileOperatorType.在线阅读:
                        FileRead();
                        break;
                    case CE_FileOperatorType.无操作:
                        break;
                    default:
                        break;
                }

                this.Close();
            }
            else
            {
                foreach (CE_FileOperatorType fileOpType in m_listShow)
                {
                    switch (fileOpType)
                    {
                        case CE_FileOperatorType.上传:
                            btnUpLoad.Visible = true;
                            break;
                        case CE_FileOperatorType.下载:
                            btnDownLoad.Visible = true;
                            break;
                        case CE_FileOperatorType.在线编辑:
                            btnEdit.Visible = true;
                            break;
                        case CE_FileOperatorType.在线阅读:
                            btnRead.Visible = true;
                            break;
                        case CE_FileOperatorType.无操作:
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// 上传
        /// </summary>
        void FileUpLoad()
        {
            try
            {
                if (m_filePathExternal.Trim().Length == 0)
                {
                    if (openFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        m_filePathExternal = openFileDialog1.FileName;
                    }
                }

                if (m_filePathExternal.Trim().Length > 0)
                {
                    CursorControl.SetWaitCursor(this);
                    m_serverFileFtp.Upload(m_filePathExternal, m_filePathInternal);

                    this.Cursor = System.Windows.Forms.Cursors.Arrow;

                    if (GetError())
                    {
                        MessageDialog.ShowPromptMessage("上传成功");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageDialog.ShowErrorMessage(ex.Message);
                this.Cursor = System.Windows.Forms.Cursors.Arrow;
            }
        }

        /// <summary>
        /// 下载
        /// </summary>
        void FileDownLoad()
        {
            try
            {
                saveFileDialog1.Filter = "All files (*.*)|*.*";
                saveFileDialog1.FileName = "";

                if (m_filePathExternal.Trim().Length == 0)
                {
                    if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        m_filePathExternal = saveFileDialog1.FileName;
                    }
                }


                if (m_filePathExternal.Trim().Length > 0)
                {
                    BackgroundWorker worker = BackgroundWorkerTools.GetWorker("下载文件");
                    worker.RunWorkerAsync();

                    m_serverFileFtp.Download(m_filePathInternal, m_filePathExternal);
                    worker.CancelAsync();

                    if (GetError())
                    {
                        MessageDialog.ShowPromptMessage("下载成功");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageDialog.ShowErrorMessage(ex.Message);
                this.Cursor = System.Windows.Forms.Cursors.Arrow;
            }
        }

        /// <summary>
        /// 文件阅读
        /// </summary>
        void FileRead()
        {
            try
            {
                string winFilePath = Path.Combine(GlobalObject.GlobalParameter.FileTempPath, m_fileName + m_fileNameSuffix);

                CursorControl.SetWaitCursor(this);
                m_serverFileFtp.Download(m_filePathInternal, winFilePath);

                this.Cursor = System.Windows.Forms.Cursors.Arrow;

                if (GetError())
                {
                    switch (m_fileType)
                    {
                        case CE_SystemFileType.Word:
                        case CE_SystemFileType.Excel:
                            Office文件显示 frmOffice = new Office文件显示(winFilePath, m_fileType);
                            frmOffice.Show();
                            break;
                        default:
                            其他文件显示 frmNormal = new 其他文件显示(winFilePath);
                            frmNormal.Show();
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageDialog.ShowErrorMessage(ex.Message);
                this.Cursor = System.Windows.Forms.Cursors.Arrow;
            }
        }

        /// <summary>
        /// 进程方法
        /// </summary>
        /// <param name="winFilePath">临时文件路径</param>
        void ProcessMethod()
        {
            string winFilePath = Path.Combine(GlobalObject.GlobalParameter.FileTempPath, m_fileName + m_fileNameSuffix);
            System.Diagnostics.Process myProcess = Process.Start(@winFilePath);

            if (myProcess != null)
            {
                while (!myProcess.HasExited)
                {
                    Thread.Sleep(1000);
                }

                CursorControl.SetWaitCursor(this);
                m_serverFileFtp.Upload(winFilePath, m_filePathInternal);
                File.Delete(winFilePath);
                SetArrowCursor(System.Windows.Forms.Cursors.Arrow);
            }
        }

        void SetArrowCursor(Cursor cur)
        {
            if (this.InvokeRequired)
            {
                GlobalObject.DelegateCollection.SetCursorArrowCallback temp = 
                    new GlobalObject.DelegateCollection.SetCursorArrowCallback(SetArrowCursor);
                this.Invoke(temp, new object[] { cur });
            }
            else
            {
                this.Cursor = System.Windows.Forms.Cursors.Arrow;
            }
        }

        /// <summary>
        /// 文件编辑
        /// </summary>
        void FileEdit()
        {
            try
            {
                string winFilePath = Path.Combine(GlobalObject.GlobalParameter.FileTempPath, m_fileName + m_fileNameSuffix);
                m_serverFileFtp.Download(m_filePathInternal, winFilePath);

                Thread thread = new Thread(new ThreadStart(ProcessMethod));

                thread.IsBackground = true;
                thread.Start();
            }
            catch (Exception ex)
            {
                MessageDialog.ShowErrorMessage(ex.Message);
                this.Cursor = System.Windows.Forms.Cursors.Arrow;
            }
        }

        /// <summary>
        /// 获得FTP错误信息
        /// </summary>
        bool GetError()
        {
            if (m_serverFileFtp.Errormessage.Length != 0)
            {
                MessageBox.Show(m_serverFileFtp.Errormessage);
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 获得文件类型
        /// </summary>
        /// <param name="fileType">文件类型</param>
        /// <returns>返回文件类型枚举</returns>
        CE_SystemFileType GetDocumentType(string fileType)
        {
            if (fileType.Contains("doc"))
            {
                return CE_SystemFileType.Word;
            }
            else if (fileType.Contains("xls"))
            {
                return CE_SystemFileType.Excel;
            }
            else if (fileType.Contains("ppt"))
            {
                return CE_SystemFileType.PPT;
            }
            else if (fileType.Contains("pdf"))
            {
                return CE_SystemFileType.PDF;
            }
            else
            {
                return CE_SystemFileType.Miss;
            }
        }

        private void btnRead_Click(object sender, EventArgs e)
        {
            FileRead();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            FileEdit();
        }

        private void btnDownLoad_Click(object sender, EventArgs e)
        {
            FileDownLoad();
        }

        private void btnUpLoad_Click(object sender, EventArgs e)
        {
            FileUpLoad();
        }
    }
}

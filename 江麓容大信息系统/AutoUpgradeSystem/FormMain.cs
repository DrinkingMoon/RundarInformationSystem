using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;
namespace AutoUpgradeSystem
{
    /// <summary>
    /// 系统登录前用于检查软件文件版本的主界面
    /// </summary>
    public partial class FormMain : Form
    {
        /// <summary>
        /// 升级管理数据库操作对象
        /// </summary>
        UpgradeManagement m_upgradeManagement;

        /// <summary>
        /// XML配置文件操作类
        /// </summary>
        XmlParams m_xmlParams;

        /// <summary>
        /// 要升级的文件列表，包含文件名称与文件版本等信息
        /// </summary>
        Dictionary<string, object> m_dicUpdateSystem;

        /// <summary>
        /// 升级文件列表
        /// </summary>
        List<UpgradeFileInfoNoFileContent> m_upgradeFile = new List<UpgradeFileInfoNoFileContent>();

        /// <summary>
        /// 升级文件存储目录
        /// </summary>
        readonly string m_filePath = Environment.CurrentDirectory;

        /// <summary>
        /// 临时文件存放位置
        /// </summary>
        readonly string m_tempPath = Environment.CurrentDirectory;

        /// <summary>
        /// 升级线程
        /// </summary>
        Thread m_upgradeThread;

        /// <summary>
        /// 升级文件委托
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="rowIndex">更新到的列表行索引</param>
        delegate void UpgradeFileHandle(string message, int rowIndex);

        /// <summary>
        /// 升级时显示消息用的委托
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="color">消息显示颜色</param>
        delegate void UpgradeMessageHandle(string message, Color color);

        /// <summary>
        /// 退出软件委托
        /// </summary>
        delegate void ExitHandle();

        /// <summary>
        /// 等待光标
        /// </summary>
        string[] Cursor_Char = new string[] { "/", "―", "\\", "|" };

        /// <summary>
        /// 光标索引
        /// </summary>
        int m_cursorIndex;

        /// <summary>
        /// 检索数据库服务器线程
        /// </summary>
        Thread m_searchDbServerThead;

        /// <summary>
        /// 检查软件新版本的委托
        /// </summary>
        delegate void CheckNewVersionHandle();

        /// <summary>
        /// 错误标志
        /// </summary>
        bool m_error;

        /// <summary>
        /// 构造函数
        /// </summary>
        public FormMain()
        {
            InitializeComponent();

            if (m_tempPath.LastIndexOf('\\') != m_tempPath.Length - 1)
            {
                m_tempPath += '\\';
            }

            m_tempPath += "Temp\\";
        }

        /// <summary>
        ///// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormMain_Load(object sender, EventArgs e)
        {
            if (!Init())
            {
                this.Close();
                return;
            }

            timerCheckDbServer.Enabled = true;

            //System.Diagnostics.Process.Start(
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns></returns>
        public bool Init()
        {
            string fileName = m_filePath + "\\AutoUpdate.xml";

            if (!File.Exists(fileName))
            {
                MessageDialog.ShowErrorMessage("系统配置文件不存在，可能文件损坏，请与管理员联系！");
                return false;
            }

            m_xmlParams = new XmlParams(fileName);
            m_dicUpdateSystem = m_xmlParams.GetParams();

            if (m_dicUpdateSystem.Count == 0)
            {
                MessageDialog.ShowErrorMessage("配置文件不正确，没有包含所需的文件信息，可能文件损坏，请与管理员联系！");
                return false;
            }

            return true;
        }

        /// <summary>
        /// 检查是否存在新的软件版本
        /// </summary>
        /// <returns></returns>
        public bool FindNewSoftwareVersion()
        {
            try
            {
                IQueryable<UpgradeFileInfoNoFileContent> result = m_upgradeManagement.GetUpgradeInfoNoFileContent();
                List<string> removeFileList = new List<string>();

                m_upgradeFile.Clear();

                foreach (KeyValuePair<string, object> sysItem in m_dicUpdateSystem)
                {
                    Dictionary<string, string> dicFile = sysItem.Value as Dictionary<string, string>;

                    foreach (KeyValuePair<string, string> fileItem in dicFile)
                    {
                        IQueryable<UpgradeFileInfoNoFileContent> records = from r in result
                                                                           where r.软件系统名称 == sysItem.Key && r.文件名称 == fileItem.Key
                                                                           select r;

                        // 2019.5.29 11:15 夏石友修改，去除自动删除文件功能 --------------------------------------------<<<<
                        //    if (records.Count() == 0)
                        //    {
                        //        // 删除新版本不需要的软件
                        //        removeFileList.Add(fileItem.Key);
                        //    }
                        //    else if (records.Single().版本号 > Convert.ToDouble(fileItem.Value))
                        //    {
                        //        m_upgradeFile.Add(records.Single());
                        //    }

                        // 需要更新的文件列表
                        if (records.Count() > 0 && records.Single().版本号 > Convert.ToDouble(fileItem.Value))
                        {
                            m_upgradeFile.Add(records.Single());
                        }
                    }

                    //2019.5.29 11:15 夏石友，保留
                    //foreach (var fileName in removeFileList)
                    //{
                    //    File.Delete(string.Format("{0}\\{1}", m_filePath, fileName));
                    //    dicFile.Remove(fileName);
                    //}

                    // 2019.5.29 11:15 夏石友修改，去除自动删除文件功能 -------------------------------------------->>>>

                    IQueryable<UpgradeFileInfoNoFileContent> fileRecords = from r in result
                                                                           where r.软件系统名称 == sysItem.Key
                                                                           select r;

                    foreach (var item in fileRecords)
                    {
                        // 添加原本不存在的文件
                        if (!dicFile.ContainsKey(item.文件名称))
                        {
                            dicFile.Add(item.文件名称, item.版本号.ToString());
                            m_upgradeFile.Add(item);
                        }
                        else
                        {
                            // 以数据库中的版本为新版本号
                            dicFile[item.文件名称] = item.版本号.ToString();
                        }
                    }
                }

                if (m_upgradeFile.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception err)
            {
                MessageDialog.ShowErrorMessage(err.Message);
            }

            return false;
        }

        /// <summary>
        /// 退出软件
        /// </summary>
        private void Exit()
        {
            timerShowCursor.Enabled = false;
            lblWaitCursor.Visible = false;

            if (!m_error)
            {
                lblFindNewVersion.Text = "当前系统已经是最新版本，无需更新...";
                lblFindNewVersion.Visible = true;

                timerShowPrompt.Enabled = true;
            }
            else
            {
                timerExit.Enabled = true;
            }
        }

        /// <summary>
        /// 显示提示信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timerShowPrompt_Tick(object sender, EventArgs e)
        {
            timerShowPrompt.Enabled = false;

            lblFindNewVersion.Text = "系统已经就绪，加载核心程序集";
            lblFindNewVersion.ForeColor = Color.Blue;

            timerExit.Enabled = true;
        }

        /// <summary>
        /// 用于退出系统的定时器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timerExit_Tick(object sender, EventArgs e)
        {
            timerExit.Enabled = false;

            if (m_error)
            {
                this.DialogResult = DialogResult.Cancel;
            }
            else
            {
                this.DialogResult = DialogResult.OK;
            }
        }

        /// <summary>
        /// 显示升级过程中的提示
        /// </summary>
        /// <param name="message">显示的消息</param>
        /// <param name="color">消息颜色</param>
        void ShowUpgradePrompt(string message, Color color)
        {
            lblFindNewVersion.Text = message;
            lblFindNewVersion.ForeColor = color;
        }

        /// <summary>
        /// 显示升级过程中的状态
        /// </summary>
        void ShowUpgradeStatus(string message, int rowIndex)
        {
            if (message.Contains("正在更新"))
            {
                if (rowIndex > 0)
                {
                    listViewFileList.Items[rowIndex - 1].Selected = false;
                }

                listViewFileList.Items[rowIndex].Selected = true;

                this.listViewFileList.Items[rowIndex].EnsureVisible();

                listViewFileList.Items[rowIndex].SubItems[1].Text = message;
            }
            else
            {
                listViewFileList.Items[rowIndex].SubItems[1].Text = message;
                lblFileList.Text = string.Format("更新文件列表，需要更新数量：{0}，已更新数：{1}", m_upgradeFile.Count, rowIndex+1);
            }
        }

        /// <summary>
        /// 升级文件线程
        /// </summary>
        private void UpgradeFileThread()
        {
            try
            {
                if (!Directory.Exists(m_tempPath))
                {
                    Directory.CreateDirectory(m_tempPath);
                }

                for (int i = 0; i < m_upgradeFile.Count; i++)
                {
                    if (listViewFileList.InvokeRequired)
                    {
                        listViewFileList.Invoke(new UpgradeFileHandle(this.ShowUpgradeStatus), new object[] { "正在更新...", i });
                    }

                    Sys_AutoUpgrade upgradeInfo = m_upgradeManagement.GetUpgradeInfo(m_upgradeFile[i].序号);
                    
                    string tempFile = string.Format("{0}~dki390v{1}{2}.tmp",
                        m_tempPath, DateTime.Now.ToString("hhmmss"), DateTime.Now.Millisecond);

                    using (FileStream outStream = new FileStream(tempFile, FileMode.Create))
                    {
                        outStream.Write(upgradeInfo.文件内容.ToArray(), 0, upgradeInfo.文件内容.Length);
                        outStream.Close();
                    }

                    if (!File.Exists(tempFile))
                    {
                        throw new Exception(string.Format("无法创建临时文件, 更新 {0} 文件失败...", m_upgradeFile[i].文件名称));
                    }

                    PlatformManagement.ZipService.UnZipFile(tempFile, m_filePath, m_upgradeFile[i].文件名称, "pwd123!");

                    Thread.Sleep(160);

                    if (listViewFileList.InvokeRequired)
                    {
                        listViewFileList.Invoke(new UpgradeFileHandle(this.ShowUpgradeStatus), new object[] { "更新完毕...", i });
                    }

                    File.Delete(tempFile);

                    Thread.Sleep(100);
                }

                //2017-07-07，夏石友 修改
                m_xmlParams.SaveParams(m_dicUpdateSystem, true);
            }
            catch (Exception exce)
            {
                m_error = true;

                lblFindNewVersion.Invoke(new UpgradeMessageHandle(this.ShowUpgradePrompt),
                    new object[] { exce.Message, Color.Red });

                Thread.Sleep(3000);

                lblFindNewVersion.Invoke(new UpgradeMessageHandle(this.ShowUpgradePrompt),
                    new object[] { "更新失败，系统终止，请联系系统管理员", Color.Red });

                Thread.Sleep(3000);
            }
            finally
            {
                lblFindNewVersion.Invoke(new ExitHandle(this.Exit));

                // 删除临时文件夹
                Directory.Delete(m_tempPath, true);

                //2017-07-07，夏石友 修改
                //m_xmlParams.SaveParams(m_dicUpdateSystem, true);
            }
        }

        /// <summary>
        /// 检查数据库服务器连接
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timerCheckDbServer_Tick(object sender, EventArgs e)
        {
            timerCheckDbServer.Enabled = false;

            m_searchDbServerThead = new Thread(new ThreadStart(this.SearchDbServerThread));
            m_searchDbServerThead.IsBackground = true;
            m_searchDbServerThead.Start();
        }

        /// <summary>
        /// 显示等待光标
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timerShowCursor_Tick(object sender, EventArgs e)
        {
            lblWaitCursor.Text = Cursor_Char[m_cursorIndex++ % Cursor_Char.Length];
        }

        /// <summary>
        /// 搜索数据库服务器线程
        /// </summary>
        private void SearchDbServerThread()
        {
            FormConfigDataServer form = new FormConfigDataServer();

            if (!form.TestDataServer(GlobalParameter.DataServerIP, false))
            {
                MessageDialog.ShowPromptMessage("设置的数据库服务器不正确，无法建立连接，请重新配置，如果配置无效请与管理员联系！");
                
                if (form.ShowDialog() == DialogResult.Cancel)
                {
                    MessageDialog.ShowPromptMessage("您设置的数据库服务器不正确，无法建立连接！");

                    m_error = true;
                    lblFindNewVersion.Invoke(new ExitHandle(this.Exit));

                    return;
                }
            }

            if (lblMainPrompt.InvokeRequired)
            {
                lblMainPrompt.Invoke(new CheckNewVersionHandle(this.CheckNewVersion));
            }
            else
            {
                CheckNewVersion();
            }
        }

        /// <summary>
        /// 检查软件新版本
        /// </summary>
        private void CheckNewVersion()
        {
            m_upgradeManagement = new UpgradeManagement();

            if (!FindNewSoftwareVersion())
            {
                Exit();
                return;
            }

            lblFindNewVersion.Visible = true;

            lblFileList.Text = string.Format("更新文件列表，需要更新数量：{0}", m_upgradeFile.Count);
            lblFileList.Visible = true;

            foreach (var item in m_upgradeFile)
            {
                ListViewItem listItem = new ListViewItem(new string[] { item.序号.ToString(), "等待更新...", 
                    item.软件系统名称, item.文件名称, item.版本号.ToString(), item.文件大小 });
                listViewFileList.Items.Add(listItem);
            }

            listViewFileList.Visible = true;
            m_upgradeThread = new Thread(new ThreadStart(this.UpgradeFileThread));
            m_upgradeThread.IsBackground = true;

            m_upgradeThread.Start();
        }
    }
}

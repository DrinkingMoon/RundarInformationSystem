using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Design;
using Microsoft.Win32;
using Service_Quality_File;

namespace Form_Quality_File
{
    public partial class 文件显示窗体 : Form
    {
        public 文件显示窗体(string url)
        {
            InitializeComponent();
            webBrowser1.Navigate(url);
        }

        #region 注册表操作
        //private static string REGUSTRY_PATH = "SOFTWARE\\Microsoft\\Windows\\Shell\\AttachmentExecute\\{0002DF01-0000-0000-C000-000000000046}";

        ///// <summary>
        ///// 文件基础管理服务组件
        ///// </summary>
        //ISystemFileBasicInfo m_serverFile = ServerModuleFactory.GetServerModule<ISystemFileBasicInfo>();

        ///// <summary>
        ///// 记录添加的注册表信息
        ///// </summary>
        //List<string> m_listRegistry = new List<string>();

        ///// <summary>
        ///// 删除注册表信息
        ///// </summary>
        //void DeleteRegistryKey()
        //{
        //    RegistryKey rsg = Registry.CurrentUser.OpenSubKey(REGUSTRY_PATH, true);

        //    foreach (string item in m_listRegistry)
        //    {
        //        rsg.DeleteValue(item);
        //    }

        //    rsg.Close();
        //}

        ///// <summary>
        ///// 添加注册表信息
        ///// </summary>
        //void AddRegistryKey()
        //{
        //    RegistryKey rsg = null;

        //    if (Registry.CurrentUser.OpenSubKey(REGUSTRY_PATH, true) == null)
        //    {
        //        Registry.CurrentUser.CreateSubKey(REGUSTRY_PATH);
        //    }

        //    rsg = Registry.CurrentUser.OpenSubKey(REGUSTRY_PATH, true);

        //    Array ary = Enum.GetValues(typeof(GlobalObject.FileType));

        //    foreach (object item in ary)
        //    {
        //        DataTable dtTemp = m_serverFile.GetFileBasicTypeInfo(item.ToString());

        //        foreach (DataRow dr in dtTemp.Rows)
        //        {
        //            if (rsg.GetValue(dr["FileTypeRegistryName"].ToString()) == null)
        //            {
        //                rsg.SetValue(dr["FileTypeRegistryName"].ToString(), new byte[] { });
        //                m_listRegistry.Add(dr["FileTypeRegistryName"].ToString());
        //            }
        //        }
        //    }

        //    rsg.Close();   
        //}
        #endregion
    }
}

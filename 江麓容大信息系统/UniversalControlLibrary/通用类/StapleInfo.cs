using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServerModule;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace UniversalControlLibrary
{
    /// <summary>
    /// 主界面需要实现的接口
    /// </summary>
    public interface IMainForm
    {
        /// <summary>
        /// 显示单据类窗体
        /// </summary>
        /// <param name="billType">单据类型</param>
        /// <param name="billNo">单据编号</param>
        void ShowBillForm(string billType, string billNo);
                
        /// <summary>
        /// 显示单据类窗体
        /// </summary>
        /// <param name="billType">单据类型</param>
        /// <param name="billNo">单据编号</param>
        /// <param name="faceParams">
        /// 界面接收的参数对象, 只支持简单数据类型、List、全部数据类型为简单类型或列表的结构体或类(列表中的数据为简单类型)
        /// </param>
        void ShowBillForm(string billType, string billNo, object faceParams);
               
        /// <summary>
        /// 显示窗体
        /// </summary>
        /// <param name="formName">界面名称</param>
        /// <param name="msgID">消息编号</param>
        /// <param name="faceParams">
        /// 传送到界面的参数数据,只支持简单数据类型、List、全部数据类型为简单类型或列表的结构体或类(列表中的数据为简单类型)
        /// </param>
        void ShowForm(string formName, int msgID, object faceParams);
    }

    /// <summary>
    /// 常用的信息
    /// </summary>
    public class StapleInfo
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        static string m_err;

        /// <summary>
        /// 列宽服务
        /// </summary>
        static IFieldWidthServer m_fieldWidthServer;

        /// <summary>
        /// 用于分隔字符串的分隔符
        /// </summary>
        static char[] m_splitChar = { ',' };

        /// <summary>
        /// 获取用于分隔字符串的分隔符
        /// </summary>
        public static char[] SplitChar
        {
            get { return StapleInfo.m_splitChar; }
        }

        /// <summary>
        /// 获取或设置主窗体(FormMain)
        /// </summary>
        static public Form MainForm
        {
            get;
            set;
        }

        /// <summary>
        /// 获取或设置消息提示窗体(FormMessagePrompt)
        /// </summary>
        static public Form MessagePromptForm
        {
            get;
            set;
        }

        /// <summary>
        /// 实体打印机名称列表
        /// </summary>
        static private List<string> m_printList = new List<string>();

        /// <summary>
        /// 获取或设置实体打印机名称列表
        /// </summary>
        static public List<string> PrintList
        {
            get { return m_printList; }
        }

        /// <summary>
        /// 获得当前活动窗体
        /// </summary>
        /// <returns>返回获取到的活动窗体</returns>
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern IntPtr GetActiveWindow();

        /// <summary>
        /// 设置活动窗体
        /// </summary>
        /// <param name="hwnd">要置为活动的窗体</param>
        /// <returns></returns>
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern IntPtr SetActiveWindow(IntPtr hwnd);

        /// <summary>
        /// 获取计量单位
        /// </summary>
        static public string[] Units
        {
            get
            {
                IUnitServer unitServer = ServerModuleFactory.GetServerModule<IUnitServer>();
                IQueryable<View_S_Unit> queryResult = null;

                if (unitServer.GetAllUnit(out queryResult, out m_err))
                {
                    queryResult = from a in queryResult
                                 where a.停用 == false
                                 select a;

                    if (queryResult.Count() > 0)
                    {
                        return (from r in queryResult select r.单位).ToArray();
                    }
                }

                return null;
            }
        }

        /// <summary>
        /// 初始化计量单位ComboBox控件
        /// </summary>
        /// <param name="cmbUnit">要初始化的计量单位ComboBox控件</param>
        static public bool InitUnitComboBox(ComboBox cmbUnit)
        {
            IUnitServer unitServer = ServerModuleFactory.GetServerModule<IUnitServer>();
            IQueryable<View_S_Unit> queryResult = null;

            if (unitServer.GetAllUnit(out queryResult, out m_err))
            {
                queryResult = from a in queryResult
                              where a.停用 == false
                              select a;

                if (queryResult.Count() > 0)
                {
                    cmbUnit.DataSource = queryResult;
                    cmbUnit.DisplayMember = "单位";
                    cmbUnit.ValueMember = "序号";

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 初始化库存物品状态ComboBox控件
        /// </summary>
        /// <param name="cmbState">要初始化的库存物品状态ComboBox控件</param>
        static public bool InitStoreStateComboBox(ComboBox cmbState)
        {
            IStoreServer server = ServerModuleFactory.GetServerModule<IStoreServer>();
            IQueryable<S_StockStatus> queryResult = server.GetStoreStatus();

            if (queryResult.Count() > 0)
            {
                cmbState.DataSource = queryResult;
                cmbState.DisplayMember = "Description";
                cmbState.ValueMember = "ID";

                return true;
            }

            return false;
        }

        /// <summary>
        /// 获取字段宽度服务
        /// </summary>
        static public IFieldWidthServer FieldWidthServer
        {
            get
            {
                if (m_fieldWidthServer == null)
                {
                    m_fieldWidthServer = ServerModuleFactory.GetServerModule<IFieldWidthServer>();
                }

                return m_fieldWidthServer;
            }
        }

        #region 夏石友, 2014-03-07，获取未隐藏列

        /// <summary>
        /// 获取控件中第一个未隐藏列
        /// </summary>
        /// <param name="dv">数据显示控件</param>
        /// <returns>返回的列号</returns>
        static public int GetVisibleColumn(DataGridView dv)
        {
            if (dv != null)
            {
                for (int i = 0; i < dv.Columns.Count; i++)
                {
                    if (dv.Columns[i].Visible)
                    {
                        return i;
                    }
                }
            }

            return -1;
        }

        #endregion
    }
}

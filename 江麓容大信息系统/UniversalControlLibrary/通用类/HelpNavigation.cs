using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UniversalControlLibrary
{
    /// <summary>
    /// 帮助导航仪
    /// </summary>
    static public class HelpNavigation
    {
        /// <summary>
        /// 帮助提供者
        /// </summary>
        static HelpProvider m_hp = null;

        /// <summary>
        /// 默认关键字
        /// </summary>
        const string DefaultKeyword = "业务操作前言";

        /// <summary>
        /// 要显示帮助的关键字
        /// </summary>
        static string m_keyword = DefaultKeyword;

        /// <summary>
        /// 获取或设置要显示帮助的关键字
        /// </summary>
        public static string Keyword
        {
            get { return HelpNavigation.m_keyword; }
            set {
                if (GlobalObject.GeneralFunction.IsNullOrEmpty(value))
                {
                    HelpNavigation.m_keyword = DefaultKeyword;
                }
                else
                {
                    HelpNavigation.m_keyword = value;
                }
            }
        }

        /// <summary>
        /// 用于触发帮助的控件
        /// </summary>
        static Control m_helpControl;

        /// <summary>
        /// 获取或设置用于触发帮助的控件
        /// </summary>
        public static Control HelpControl
        {
            get { return HelpNavigation.m_helpControl; }
            set { HelpNavigation.m_helpControl = value; }
        }

        /// <summary>
        /// 显示帮助
        /// </summary>
        /// <param name="keyword">要显示帮助的关键字</param>
        static void Show(string keyword)
        {
            if (m_hp == null)
            {
                m_hp = new HelpProvider();
                m_hp.HelpNamespace = "进销存信息化系统操作手册.chm";
            }

            if (m_helpControl == null)
            {
                MessageDialog.ShowErrorMessage("用于触发帮助的控件为NULL，操作失败");
                return;
            }

            if (GlobalObject.GeneralFunction.IsNullOrEmpty(keyword))
            {
                MessageDialog.ShowErrorMessage("帮助关键字为空，操作失败");
                return;
            }

            Help.ShowHelp(m_helpControl, m_hp.HelpNamespace, HelpNavigator.KeywordIndex, keyword);
        }

        /// <summary>
        /// 显示帮助
        /// </summary>
        /// <param name="keyword">要显示帮助的关键字</param>
        static public void ShowHelp(string keyword)
        {
            if (GlobalObject.GeneralFunction.IsNullOrEmpty(keyword))
            {
                keyword = DefaultKeyword;
            }

            Show(keyword);
        }

        /// <summary>
        /// 显示帮助
        /// </summary>
        static public void ShowHelp()
        {
            Show(m_keyword);
        }
    }
}

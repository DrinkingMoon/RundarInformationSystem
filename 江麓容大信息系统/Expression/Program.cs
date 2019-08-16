using System;
using System.Collections.Generic;
using System.Windows.Forms;
using UniversalControlLibrary;
using System.Runtime.InteropServices;
using System.Threading;

namespace Expression
{
    static class Program
    {
        static bool createNewInstance = true;
        static System.Threading.Mutex instance = new System.Threading.Mutex(true, "仓库管理系统", out createNewInstance);


        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                //Application.ThreadException += new ThreadExceptionEventHandler(UIThreadException);
                //Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
                //AppDomain.CurrentDomain.UnhandledException +=
                //    new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

                // 虚拟打印机初始化
                VirtualPrint.Init();

                // 列宽控制类初始化
                ColumnWidthControl.Init();

                // 查询过滤器初始化
                QueryFilterControl.Init();

                GlobalObject.GlobalParameter.Init();

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                Application.Run(new FormMain());

                // 保存设置
                ColumnWidthControl.Save();
            }
            catch (Exception exce)
            {
                MessageDialog.ShowErrorMessage(exce.Message);
            }
        }
        private static void UIThreadException(object sender, ThreadExceptionEventArgs t)
        {
            try
            {
                string errorMsg = "Windows窗体线程异常 : \n\n";
                MessageBox.Show(errorMsg + t.Exception.Message + Environment.NewLine + t.Exception.StackTrace);
            }
            catch
            {
                MessageBox.Show("不可恢复的Windows窗体异常，应用程序将退出！");
            }
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                Exception ex = (Exception)e.ExceptionObject;
                string errorMsg = "非窗体线程异常 : \n\n";
                MessageBox.Show(errorMsg + ex.Message + Environment.NewLine + ex.StackTrace);
            }
            catch
            {
                MessageBox.Show("不可恢复的非Windows窗体线程异常，应用程序将退出！");
            }
        }
    }
}
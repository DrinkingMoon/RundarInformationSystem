/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  FileParse.cs
 * 作者    :  曹津彬    版本: v1.00    日期: 2014/01/22
 * 开发平台:  Visual C# 2008
 * 用于    :  仓库管理软件
 *----------------------------------------------------------------------------
 * 描述 : 
 * 其它 :
 *----------------------------------------------------------------------------
 * 公共信息: 参见系统'类帮助文档'
 *----------------------------------------------------------------------------
 * 历史记录:
 ******************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading;
using System.Reflection;
using System.IO;

namespace GlobalObject
{
    /// <summary>
    /// 文件转换
    /// </summary>
    public class FileParse
    {
        //private static string XPDF_LANG_PATH = "xpdf-chinese-simplified";
        //private static string FLASH_PRINTER_PATH = "FlashPrinter.exe";
        //private static string PDF2SWF_PATH = "pdf2swf.exe";

        ///// <summary>
        ///// 文档转swf
        ///// </summary>
        ///// <param name="inFilename"></param>
        ///// <param name="swfFilename"></param>
        //public static bool ConvertPdfToSwf(string inFilename, string swfFilename)
        //{
        //    bool isStart;
        //    try
        //    {
        //        string flashPrinter = string.Concat(AppDomain.CurrentDomain.BaseDirectory, FLASH_PRINTER_PATH);//FlashPrinter.exe  

        //        System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo(flashPrinter);

        //        startInfo.Arguments = string.Concat(inFilename, " -o ", swfFilename);

        //        System.Diagnostics.Process process = new System.Diagnostics.Process();
        //        process.StartInfo = startInfo;
        //        isStart = process.Start();

        //        process.WaitForExit();
        //        process.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return isStart;
        //}

        //#region 转HTML

        ///// <summary>   
        ///// WinExcel文件生成HTML并保存   
        ///// </summary>   
        ///// <param name="FilePath">需要生成的Excel文件的路径</param>   
        ///// <param name="saveFilePath">生成以后保存HTML文件的路径</param>   
        ///// <returns>是否生成成功，成功为true，反之为false</returns>   
        //public static bool GenerationExcelHTML(string FilePath, string saveFilePath)
        //{
        //    try
        //    {
        //        Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();
        //        app.Visible = false;
        //        Object o = Missing.Value;

        //        ///打开文件   
        //        /*下面是Microsoft Excel 9 Object Library的写法： */
        //        /*_Workbook xls = app.Workbooks.Open(FilePath, o, o, o, o, o, o, o, o, o, o, o, o);*/

        //        /*下面是Microsoft Excel 10 Object Library的写法： */
        //        _Workbook xls = app.Workbooks.Open(FilePath, o, o, o, o, o, o, o, o, o, o, o, o, o, o);

        //        ///转换格式，另存为 HTML   
        //        /*下面是Microsoft Excel 9 Object Library的写法： */
        //        /*xls.SaveAs(saveFilePath, Excel.XlFileFormat.xlHtml, o, o, o, o, XlSaveAsAccessMode.xlExclusive, o, o, o, o);*/

        //        /*下面是Microsoft Excel 10 Object Library的写法： */
        //        xls.SaveAs(saveFilePath, Microsoft.Office.Interop.Excel.XlFileFormat.xlHtml, o, o, o, o, XlSaveAsAccessMode.xlExclusive, o, o, o, o, o);

        //        ///退出 Excel   
        //        app.Quit();
        //        return true;
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //    finally
        //    {
        //        //最后关闭打开的excel 进程   
        //        Process[] myProcesses = Process.GetProcessesByName("EXCEL");
        //        foreach (Process myProcess in myProcesses)
        //        {
        //            myProcess.Kill();
        //        }
        //    }
        //}

        ///// <summary>   
        ///// WinWord文件生成HTML并保存   
        ///// </summary>   
        ///// <param name="FilePath">需要生成的word文件的路径</param>   
        ///// <param name="saveFilePath">生成以后保存HTML文件的路径</param>   
        ///// <returns>是否生成成功，成功为true，反之为false</returns>   
        //public static bool GenerationWordHTML(string FilePath, string saveFilePath)
        //{
        //    try
        //    {
        //        Microsoft.Office.Interop.Word.ApplicationClass word = new Microsoft.Office.Interop.Word.ApplicationClass();
        //        Type wordType = word.GetType();
        //        Microsoft.Office.Interop.Word.Documents docs = word.Documents;

        //        /// 打开文件    
        //        Type docsType = docs.GetType();
        //        Microsoft.Office.Interop.Word.Document doc = (Microsoft.Office.Interop.Word.Document)docsType.InvokeMember("Open", System.Reflection.BindingFlags.InvokeMethod, null, docs, new Object[] { FilePath, true, true });

        //        /// 转换格式，另存为 HTML    
        //        Type docType = doc.GetType();

        //        /*下面是Microsoft Word 9 Object Library的写法： */
        //        /*docType.InvokeMember("SaveAs", System.Reflection.BindingFlags.InvokeMethod, null, doc, new object[] { saveFilePath, Word.WdSaveFormat.wdFormatHTML });*/

        //        /*下面是Microsoft Word 10 Object Library的写法： */
        //        docType.InvokeMember("SaveAs", System.Reflection.BindingFlags.InvokeMethod,
        //        null, doc, new object[] { saveFilePath, Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatFilteredHTML });

        //        /// 退出 Word   
        //        wordType.InvokeMember("Quit", System.Reflection.BindingFlags.InvokeMethod, null, word, null);
        //        return true;
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //    finally
        //    {
        //        //最后关闭打开的winword 进程   
        //        Process[] myProcesses = Process.GetProcessesByName("WINWORD");
        //        foreach (Process myProcess in myProcesses)
        //        {
        //            myProcess.Kill();
        //        }
        //    }
        //}

        //#endregion

        //#region 转换文件组件

        ///// <summary>
        ///// 转化pdf为flash（使用pdf2swf）
        ///// </summary>
        ///// <param name="sourcePath"></param>
        ///// <param name="targetPath"></param>
        ///// <returns></returns>
        //public static bool ParseSWF(string sourcePath, string targetPath)
        //{
        //    bool isSuccess = false;

        //    string strCommand = String.Format("{0} -T 9 -p 1-3 -s languagedir={3} {1} -o {2}",
        //        GetPath(PDF2SWF_PATH), sourcePath, targetPath, GetPath(XPDF_LANG_PATH));
        //    double spanMilliseconds = RunShell(strCommand);

        //    if (IsParseSuccess(targetPath))
        //    {
        //        Console.WriteLine("转换Flash成功{0}，用时{1}毫秒，命令：{2}", targetPath, spanMilliseconds, strCommand);
        //        isSuccess = true;
        //    }
        //    else
        //    {
        //        Console.WriteLine("转换Flash失败{0}，用时{1}毫秒，命令：{2}", targetPath, spanMilliseconds, strCommand);
        //    }
        //    return isSuccess;
        //}

        ///// <summary>
        ///// 转化文档为pdf（使用FlashPrinter）
        ///// </summary>
        ///// <param name="sourcePath"></param>
        ///// <param name="targetPath"></param>
        ///// <returns></returns>
        //public static bool ParsePDFWithFlashPrinter(string sourcePath, string targetPath)
        //{
        //    bool isSuccess = false;

        //    string strCommand = String.Format("{0} {1} -o {2}",
        //        FLASH_PRINTER_PATH, sourcePath, targetPath);
        //    double spanMilliseconds = RunShell(strCommand);

        //    if (IsParseSuccess(targetPath))
        //    {
        //        Console.WriteLine("转换文档成功{0}，用时{1}毫秒，命令：{2}", targetPath, spanMilliseconds, strCommand);
        //        isSuccess = true;
        //    }
        //    else
        //    {
        //        Console.WriteLine("转换文档失败{0}，用时{1}毫秒，命令：{2}", targetPath, spanMilliseconds, strCommand);
        //    }

        //    return isSuccess;
        //}

        ///// <summary>
        ///// 重命名文件（用来检查文件是否生成完成）
        ///// </summary>
        ///// <param name="sourePath">源地址</param>
        ///// <param name="targetPath">目标地址</param>
        ///// <returns></returns>
        //private static bool RenameFile(string sourePath, string targetPath)
        //{
        //    bool isOpen = false;

        //    //如果是相同地址，直接移动检查是否文件已经生成，否则进行复制（因为目标文件存在的话会有问题）
        //    if (sourePath.Equals(targetPath))
        //    {
        //        try
        //        {
        //            //移动文件
        //            File.Move(sourePath, targetPath);
        //            isOpen = true;
        //        }
        //        catch (Exception e)
        //        {
        //            isOpen = false;
        //            throw new Exception(e.Message);
        //        }
        //    }
        //    else
        //    {
        //        bool isCopySuccess = false;
        //        try
        //        {
        //            //复制文件
        //            File.Copy(sourePath, targetPath, true);
        //            isCopySuccess = true;
        //        }
        //        catch (Exception e)
        //        {
        //            isOpen = false;
        //            throw new Exception(e.Message);
        //        }
        //        if (isCopySuccess)
        //        {
        //            //如果复制成功，删除源文件
        //            File.Delete(sourePath);
        //        }
        //    }

        //    return isOpen;
        //}

        ///// <summary>
        ///// 根据进程名称来关闭进程
        ///// </summary>
        ///// <param name="processName"></param>
        //private static void KillPrecess(string processName)
        //{
        //    foreach (Process p in Process.GetProcesses())
        //    {
        //        if (p.ProcessName == processName)
        //        {
        //            p.Kill();
        //        }
        //    }
        //}

        ///// <summary>
        ///// 检查是否转换成功（文件是否生成完毕）
        ///// </summary>
        ///// <param name="sourcePath">要检查文件地址</param>
        ///// <param name="targetPath">要复制到的地址（如果不需要真正复制，请跟sourcePath一致）</param>
        ///// <param name="Timeout">最大等待时间</param>
        ///// <returns></returns>
        //private static bool IsParseSuccess(string sourcePath, string targetPath, int Timeout)
        //{
        //    bool isSuccess = false;

        //    if (Timeout <= 0)
        //        Timeout = 30;

        //    int i = 0;
        //    while (!RenameFile(sourcePath, targetPath))
        //    {
        //        Thread.Sleep(1000);
        //        i++;
        //        if (i == Timeout)
        //            break;
        //    }
        //    if (i < Timeout)
        //        isSuccess = true;

        //    return isSuccess;
        //}

        ///// <summary>
        ///// 检查是否转换成功（文件是否生成完毕）
        ///// </summary>
        ///// <param name="targetPath"></param>
        ///// <returns></returns>
        //private static bool IsParseSuccess(string targetPath)
        //{
        //    return IsParseSuccess(targetPath, targetPath, 30);
        //}

        ///// <summary>
        ///// 运行命令
        ///// </summary>
        ///// <param name="strShellCommand">命令字符串</param>
        ///// <returns>命令运行时间</returns>
        //private static double RunShell(string strShellCommand)
        //{
        //    double spanMilliseconds = 0;
        //    DateTime beginTime = DateTime.Now;

        //    Process cmd = new Process();
        //    cmd.StartInfo.FileName = "cmd.exe";
        //    cmd.StartInfo.UseShellExecute = false;
        //    cmd.StartInfo.CreateNoWindow = true;
        //    cmd.StartInfo.Arguments = String.Format(@"/c {0}", strShellCommand);
        //    cmd.Start();
        //    cmd.WaitForExit();


        //    DateTime endTime = DateTime.Now;
        //    TimeSpan timeSpan = endTime - beginTime;
        //    spanMilliseconds = timeSpan.TotalMilliseconds;

        //    return spanMilliseconds;
        //}

        ///// <summary>
        ///// 获取文件全路径
        ///// </summary>
        ///// <param name="path"></param>
        ///// <returns></returns>
        //private static string GetPath(string path)
        //{
        //    //HttpContext.Current.Server.MapPath(path);
        //    return String.Format("{0}{1}", AppDomain.CurrentDomain.BaseDirectory, path);
        //}

        //#endregion
    }
}

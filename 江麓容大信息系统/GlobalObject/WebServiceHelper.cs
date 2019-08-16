/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  WebServiceHelper.cs
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
using System.IO;
using System.Net;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Web.Services.Description;
using Microsoft.CSharp;
using System.Net.Sockets;

namespace GlobalObject
{
    /// <summary>
    /// 动态调用WEBSERVICE
    /// </summary>
    public class WebServiceHelper
    {
        public static void FileParse(string filePath, string fileType)
        {
            try
            {
                int port = 9000;

                string host = GlobalObject.GlobalParameter.DataServerIP;

                IPAddress ip = IPAddress.Parse(host);
                IPEndPoint ipe = new IPEndPoint(ip, port);

                // 创建Socket并连接到服务器
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                // 连接到服务器
                socket.Connect(ipe);

                string sendStr = "convert " + "-" + fileType + " " + filePath + "<EOF>";

                // 把字符串编码为字节
                byte[] bs = Encoding.Unicode.GetBytes(sendStr);

                Console.WriteLine("Send message");

                // 发送信息
                socket.Send(bs, bs.Length, 0);

                // 接受从服务器返回的信息
                string recvStr = "";
                byte[] recvBytes = new byte[1024];
                int bytes;

                // 从服务器端接受返回信息
                bytes = socket.Receive(recvBytes, recvBytes.Length, 0);
                recvStr = Encoding.Unicode.GetString(recvBytes, 0, bytes);

                // 回显服务器的返回信息
                Console.WriteLine("client get message:{0}", recvStr);

                Console.ReadLine();

                if (recvStr.Contains("failed"))
                {
                    throw new Exception("文件转换失败");
                }

                // 用完Socket后关闭
                socket.Close();
            }
            catch (ArgumentException e)
            {
                throw new Exception(string.Format("argumentNullException:{0}", e.Message));
            }
            catch (SocketException e)
            {
                throw new Exception(string.Format("SocketException:{0}", e.Message));
            }


            //localhost.Service service = new localhost.Service();

            //string fileType = filePath.Substring(filePath.LastIndexOf(".") + 1).ToLower();

            //service.Parse_CMD_PDF2SWF(filePath, filePath.Substring(0, filePath.LastIndexOf(".")) + ".swf");

        }

        /// <summary>
        /// 动态调用WebService
        /// </summary>
        /// <param name="url">WebService地址</param>
        /// <param name="methodname">方法名(模块名)</param>
        /// <param name="args">参数列表,无参数为null</param>
        /// <returns>object</returns>
        public static object InvokeWebService(string url, string methodname, object[] args)
        {
            return InvokeWebService(url, null, methodname, args);
        }

        /// <summary>
        /// 动态调用WebService
        /// </summary>
        /// <param name="url">WebService地址</param>
        /// <param name="classname">类名</param>
        /// <param name="methodname">方法名(模块名)</param>
        /// <param name="args">参数列表</param>
        /// <returns>object</returns>
        public static object InvokeWebService(string url, string classname, string methodname, object[] args)
        {
            string @namespace = "fangqm.Netbank.WebService.webservice";

            if (classname == null || classname == "")
            {
                classname = WebServiceHelper.GetClassName(url);
            }

            //获取服务描述语言(WSDL)
            WebClient wc = new WebClient();
            Stream stream = wc.OpenRead(url + "?WSDL");//【1】
            ServiceDescription sd = ServiceDescription.Read(stream);//【2】
            ServiceDescriptionImporter sdi = new ServiceDescriptionImporter();//【3】
            sdi.AddServiceDescription(sd, "", "");
            CodeNamespace cn = new CodeNamespace(@namespace);//【4】

            //生成客户端代理类代码
            CodeCompileUnit ccu = new CodeCompileUnit();//【5】
            ccu.Namespaces.Add(cn);
            sdi.Import(cn, ccu);
            CSharpCodeProvider csc = new CSharpCodeProvider();//【6】
            CodeDomProvider icc = CodeDomProvider.CreateProvider("CSharp");//【7】

            //设定编译器的参数
            CompilerParameters cplist = new CompilerParameters();//【8】
            cplist.GenerateExecutable = false;
            cplist.GenerateInMemory = true;
            cplist.ReferencedAssemblies.Add("System.dll");
            cplist.ReferencedAssemblies.Add("System.XML.dll");
            cplist.ReferencedAssemblies.Add("System.Web.Services.dll");
            cplist.ReferencedAssemblies.Add("System.Data.dll");

            //编译代理类
            CompilerResults cr = icc.CompileAssemblyFromDom(cplist, ccu);//【9】
            if (true == cr.Errors.HasErrors)
            {
                System.Text.StringBuilder sb = new StringBuilder();
                foreach (CompilerError ce in cr.Errors)
                {
                    sb.Append(ce.ToString());
                    sb.Append(System.Environment.NewLine);
                }
                throw new Exception(sb.ToString());
            }

            //生成代理实例,并调用方法
            System.Reflection.Assembly assembly = cr.CompiledAssembly;
            Type t = assembly.GetType(@namespace + "." + classname, true, true);
            object obj = Activator.CreateInstance(t);//【10】
            System.Reflection.MethodInfo mi = t.GetMethod(methodname);//【11】
            return mi.Invoke(obj, args);

        }

        private static string GetClassName(string url)
        {
            //假如URL为"http://localhost/InvokeService/Service1.asmx"
            //最终的返回值为 Service1
            string[] parts = url.Split('/');
            string[] pps = parts[parts.Length - 1].Split('.');
            return pps[0];
        }
    }
}

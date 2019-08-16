/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  CommResponseServer.cs
 * 作者    :  夏石友    版本: v1.00    日期: 2010/07/17
 * 开发平台:  Visual C# 2008
 * 用于    :  仓库管理软件
 *----------------------------------------------------------------------------
 * 描述 : 
 * 其它 :
 *----------------------------------------------------------------------------
 * 公共信息: 参见系统'类帮助文档'
 *----------------------------------------------------------------------------
 * 历史记录:
 *     1. 日期时间: 2010/07/17 8:54:12 作者: 夏石友 当前版本: V1.00
 *        修改说明: 创建
 ******************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Net.Sockets;
using System.Net;
using ServerModule;
using AsynSocketService;
using SocketCommDefiniens;
using ServerRequestProcessorModule;

namespace Expression
{
    /// <summary>
    /// 服务器与终端通信响应类
    /// </summary>
    public class CommResponseServer
    {
        /// <summary>
        /// 服务器接口
        /// </summary>
        IAsynServer m_server;

        /// <summary>
        /// 连接标志
        /// </summary>
        bool m_beginFlag = false;

        /// <summary>
        /// 获取或设置产品类型表
        /// </summary>
        Dictionary<string, string> m_productTypeDic;

        /// <summary>
        /// 获取或设置连接标志
        /// </summary>
        public bool BeginFlag
        {
            get { return m_beginFlag; }
            set { m_beginFlag = value; }
        }

        /// <summary>
        /// 产品类型表
        /// </summary>
        public Dictionary<string, string> ProductTypeDic
        {
            get { return m_productTypeDic; }
            set { m_productTypeDic = value; }
        }
         
        public CommResponseServer()
        {
            m_server = AsynSocketFactory.GetSingletonServer(60000);
            m_server.OnConnected += new GlobalObject.DelegateCollection.SocketConnectEvent(asynServer_OnConnected);
            m_server.OnReceive += new ReceiveEventHandler(asynServer_OnReceive);
        }

        void asynServer_OnConnected(object sender, bool isCompleted)
        {
            Socket client = sender as Socket;
            string msg = null;
            IPEndPoint endPoint = (client.RemoteEndPoint as IPEndPoint);

            if (isCompleted)
            {
                msg = String.Format("{0}, {1}端口 连接上服务器。", endPoint.Address.ToString(), endPoint.Port);
                BeginFlag = true;
                Console.WriteLine(msg + ServerTime.Time.ToString());
                //MessageDialog.ShowPromptMessage(msg);
            }
            else
            {
                msg = String.Format("{0}, {1}端口 断开服务器。", endPoint.Address.ToString(), endPoint.Port);
                Console.WriteLine(msg + ServerTime.Time.ToString());
                //MessageDialog.ShowErrorMessage(msg);
            }

            return;
        }

        void asynServer_OnReceive(object sender, CommEventArgs args)
        {
            CommEventArgs commArgs = args;
            string address = commArgs.SourceAddress;
            commArgs.SourceAddress = args.TargetAddress;
            commArgs.TargetAddress = address;

            if (args.Params != null)
            {
                for (int i = 0; i < args.Params.Count; i++)
                {
                    if (commArgs.Params[i].CMD == CommCMD.用户登陆)
                    {
                        Socket_UserInfo userInfo = commArgs.Params[i].DataValue as Socket_UserInfo;
                        RequestUserProcessor userProcessor = new RequestUserProcessor();
                        commArgs.Params[i].DataValue = userProcessor.ReceiveUserInfo(userInfo);
                    }
                    else if (commArgs.Params[i].CMD == CommCMD.初始化)
                    {
                        string workbench = commArgs.Params[i].DataValue as string;
                        RequestWorkBenchInfo wbProcessor = new RequestWorkBenchInfo();

                        if (commArgs.Params[i].Code == TagCode.工位产品列表)
                        {
                            commArgs.Params[i].DataValue = wbProcessor.GetProductInfo(workbench);
                        }
                        else if (commArgs.Params[i].Code == TagCode.工位指定产品零件信息)
                        {
                            commArgs.Params[i].DataValue = wbProcessor.GetWorkBenchInfo(workbench);
                        }                       
                    }
                    else if (commArgs.Params[i].CMD == CommCMD.请求)
                    {
                        if (commArgs.Params[i].Code == TagCode.获取装配条形码对应零件信息)
                        {
                            Socket_FittingAccessoryInfo fittingAccessoryInfo = commArgs.Params[i].DataValue as Socket_FittingAccessoryInfo;
                            RequestFittingInfo fittingProcessor = new RequestFittingInfo();
                            commArgs.Params[i].DataValue = fittingProcessor.ReceiveReadBarCodeInfo(fittingAccessoryInfo);
                        }
                        else if (commArgs.Params[i].Code == TagCode.获取选配值)
                        {
                            string info = (string)commArgs.Params[i].DataValue;
                            RequestFittingInfo fittingProcessor = new RequestFittingInfo();
                            commArgs.Params[i].DataValue = fittingProcessor.ReceiveReadChoseMatchInfo(info);

                            //Socket_FittingAccessoryInfo fittingAccessoryInfo = commArgs.Params[i].DataValue as Socket_FittingAccessoryInfo;
                            //RequestFittingInfo fittingProcessor = new RequestFittingInfo();
                            //fittingAccessoryInfo = fittingProcessor.ReceiveReadBarCodeInfo(fittingAccessoryInfo);
                            //commArgs.Params[i].DataValue = fittingProcessor.ReceiveReadChoseMatchInfo(fittingAccessoryInfo);
                        }
                    }
                    else if (commArgs.Params[i].CMD == CommCMD.存储工位信息)
                    {
                        if (commArgs.Params[i].Code == TagCode.装配信息)
                        {
                            Socket_FittingAccessoryInfoSum fittingAccessoryInfoSum = commArgs.Params[i].DataValue as Socket_FittingAccessoryInfoSum;
                            fittingAccessoryInfoSum.Edition = GetProductType(fittingAccessoryInfoSum.ProductTypeName);
                            RequestFittingInfo fittingProcessor = new RequestFittingInfo();

                            commArgs.Params[i].DataValue = fittingProcessor.ReceiveSaveTempFittingInfo(fittingAccessoryInfoSum);

                            if (fittingAccessoryInfoSum.ProductTypeName.Contains("力帆") && fittingAccessoryInfoSum.WorkBench == "DB")
                            {
                                commArgs.Params[i].DataValue = fittingProcessor.ReceiveSaveFittingInfo(fittingAccessoryInfoSum.ProductCode);
                            }
                            else if (fittingAccessoryInfoSum.ProductTypeName.Contains("众泰") && fittingAccessoryInfoSum.WorkBench == "DB")
                            {
                                commArgs.Params[i].DataValue = fittingProcessor.ReceiveSaveFittingInfo(fittingAccessoryInfoSum.ProductCode);
                            }
                        }
                    }
                }
            }

            for (int i = 0; i < commArgs.Params.Count; i++)
            {
                commArgs.Params[i].CMD = CommCMD.应答;
            }

            string error;
            m_server.Send(commArgs, out error);
        }
        
        /// <summary>
        /// 获取产品信息
        /// </summary>
        /// <returns></returns>
        string GetProductType(string productTypeName)
        {
            foreach (var item in ProductTypeDic)
            {
                if (item.Key == productTypeName)
                {
                    return item.Value;
                }
            }

            return null;
        }

        public void StartServer()
        {
            if (!BeginFlag)
            {
                m_server.Begin();
                BeginFlag = true;
            }
        }

        public void CloseServer()
        {
            m_server.Close();
            BeginFlag = false;
        }
    }
}

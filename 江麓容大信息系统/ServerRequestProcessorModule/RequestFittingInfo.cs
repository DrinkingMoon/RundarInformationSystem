/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  RequestFittingProcessor.cs
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
using AsynSocketService;
using GlobalObject;
using SocketCommDefiniens;
using PlatformManagement;
using ServerModule;

namespace ServerRequestProcessorModule
{
    /// <summary>
    /// 响应装配请求处理器
    /// </summary>
    public class RequestFittingInfo
    {
        /// <summary>
        /// 服务组件
        /// </summary>
        IBarCodeServer m_barCodeServer = ServerModuleFactory.GetServerModule<IBarCodeServer>();

        /// <summary>
        /// 服务组件
        /// </summary>
        IChoseConfectServer m_choseConfectServer = ServerModuleFactory.GetServerModule<IChoseConfectServer>();

        /// <summary>
        /// 服务组件
        /// </summary>
        IElectronFileServer m_electronFileServer = ServerModuleFactory.GetServerModule<IElectronFileServer>();

        /// <summary>
        /// 接收事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="re">事件参数</param>
        public Socket_FittingAccessoryInfo ReceiveReadBarCodeInfo(Socket_FittingAccessoryInfo fittingAccessoryInfo)
        {
            DataTable table;
            string error;

            if (!m_barCodeServer.GetData(fittingAccessoryInfo.BarCode, out table, out error))
            {
                if (error.Contains(Socket_FittingAccessoryInfo.OperateStateEnum.条形码有误.ToString()))
                {
                    fittingAccessoryInfo.OperateState = Socket_FittingAccessoryInfo.OperateStateEnum.条形码有误;
                }
            }
            else
            {
                fittingAccessoryInfo.GoodsCode = table.Rows[0][0].ToString();
                fittingAccessoryInfo.Spec = table.Rows[0][1].ToString();
                fittingAccessoryInfo.GoodsName = table.Rows[0][2].ToString();
                fittingAccessoryInfo.Provider = table.Rows[0][3].ToString();
                fittingAccessoryInfo.BatchNo = table.Rows[0][4].ToString();
                fittingAccessoryInfo.OperateState = Socket_FittingAccessoryInfo.OperateStateEnum.操作成功;
            }

            return fittingAccessoryInfo;
        }

        /// <summary>
        /// 接收事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="re">事件参数</param>
        public Socket_FittingAccessoryInfo ReceiveReadChoseMatchInfo(Socket_FittingAccessoryInfo fittingAccessoryInfo)
        {
            DataTable table;
            string error;

            if (!m_choseConfectServer.GetAccessoryChoseConfectInfo(fittingAccessoryInfo.GoodsCode, "", out table, out error))
            {
                if (error.Contains(Socket_FittingAccessoryInfo.OperateStateEnum.获取选配信息失败.ToString()))
                {
                    fittingAccessoryInfo.OperateState = Socket_FittingAccessoryInfo.OperateStateEnum.获取选配信息失败;
                }
                else
                {
                    fittingAccessoryInfo.OperateState = Socket_FittingAccessoryInfo.OperateStateEnum.该零件非选配零件;
                }
            }
            else
            {
                fittingAccessoryInfo.ChoseMatchInfo = BuildChoseMatchInfoDic(table);
                fittingAccessoryInfo.OperateState = Socket_FittingAccessoryInfo.OperateStateEnum.操作成功;
            }

            return fittingAccessoryInfo;
        }

        /// <summary>
        /// 获取指定产品指定工位选配零件信息
        /// </summary>
        /// <param name="info">包含产品名称、工位号的输入参数</param>
        public Socket_FittingAccessoryInfo ReceiveReadChoseMatchInfo(string info)
        {
            string[] data = info.Split(new char[] { ',' });
            string productName = data[0];
            string workBench = data[1];

            Socket_FittingAccessoryInfo fittingAccessoryInfo = new Socket_FittingAccessoryInfo();

            IAssemblingBom server = ServerModuleFactory.GetServerModule<IAssemblingBom>();

            // 获取指定工位所有装配信息，包含多种产品
            IQueryable<View_P_AssemblingBom> queryResult = server.GetInfoOfWorkBench(workBench);
            List<View_P_AssemblingBom> choseMatchParts = (from r in queryResult 
                                                          where r.产品名称 == productName && r.是否选配零件 
                                                          select r).ToList();

            if (choseMatchParts == null || choseMatchParts.Count == 0)
            {
                fittingAccessoryInfo.OperateState = Socket_FittingAccessoryInfo.OperateStateEnum.该工位无选配零件;
                return fittingAccessoryInfo;
            }

            fittingAccessoryInfo.ParentCode = choseMatchParts[0].父总成编码;
            fittingAccessoryInfo.GoodsCode = choseMatchParts[0].零件编码;
            fittingAccessoryInfo.GoodsName = choseMatchParts[0].零件名称;
            fittingAccessoryInfo.Spec = choseMatchParts[0].规格;
            fittingAccessoryInfo.Counts = choseMatchParts[0].装配数量;
            fittingAccessoryInfo.AssemblyFlag = Convert.ToInt32(choseMatchParts[0].是否总成);
            
            DataTable table;
            string error;

            if (!m_choseConfectServer.GetAccessoryChoseConfectInfo(choseMatchParts[0].零件编码, "", out table, out error))
            {
                if (error.Contains(Socket_FittingAccessoryInfo.OperateStateEnum.获取选配信息失败.ToString()))
                {
                    fittingAccessoryInfo.OperateState = Socket_FittingAccessoryInfo.OperateStateEnum.获取选配信息失败;
                }
                else
                {
                    fittingAccessoryInfo.OperateState = Socket_FittingAccessoryInfo.OperateStateEnum.该零件非选配零件;
                }
            }
            else
            {
                fittingAccessoryInfo.ChoseMatchInfo = BuildChoseMatchInfoDic(table);
                fittingAccessoryInfo.OperateState = Socket_FittingAccessoryInfo.OperateStateEnum.操作成功;
            }

            return fittingAccessoryInfo;
        }

        /// <summary>
        /// 构建选配字典
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        Sock_ChoseMatchInfo BuildChoseMatchInfoDic(DataTable table)
        {
            Sock_ChoseMatchInfo choseMatchInfo = new Sock_ChoseMatchInfo();

            for (int i = 0; i < table.Rows.Count; i++)
            {
                string rangeData = table.Rows[i][1].ToString();
                int index = 0;

                for (int j = 0; j < rangeData.Length; j++)
                {
                    if (rangeData[j].ToString() == "至")
                    {
                        index = j;
                        break;
                    }
                }

                string strmin = rangeData.Substring(0, index);
                string strmax = rangeData.Substring(index + 1, rangeData.Length - (index + 1));

                if (choseMatchInfo.ChoseMatchData == null)
                {
                    choseMatchInfo.ChoseMatchData = new List<string>();
                }

                if (choseMatchInfo.MinDataList == null)
                {
                    choseMatchInfo.MinDataList = new List<double>();
                }

                if (choseMatchInfo.MaxDataList == null)
                {
                    choseMatchInfo.MaxDataList = new List<double>();
                }

                choseMatchInfo.ChoseMatchData.Add(table.Rows[i][2].ToString());
                choseMatchInfo.MinDataList.Add(Convert.ToDouble(strmin));
                choseMatchInfo.MaxDataList.Add(Convert.ToDouble(strmax));
            }

            return choseMatchInfo;
        }

        /// <summary>
        /// 接收装配信息到临时表事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="re">事件参数</param>
        public Socket_StateInfo ReceiveSaveTempFittingInfo(Socket_FittingAccessoryInfoSum fittingAccessoryInfoSum)
        {
            DateTime serverTime = ServerTime.Time;

            if (fittingAccessoryInfoSum.FittingTime != "")
            {
                fittingAccessoryInfoSum.FittingTime = serverTime.ToLongDateString() + " " + serverTime.ToLongTimeString();
            }

            Socket_StateInfo stateInfo = new Socket_StateInfo();
            //string error;
            List<Socket_FittingAccessoryInfo> fittingAccessoryInfoList = fittingAccessoryInfoSum.FittingAccessoryInfoList;

            for (int i = 0; i < fittingAccessoryInfoList.Count; i++)
            {
                string oldParentCode = "";

                if (fittingAccessoryInfoList[i].ParentCode != null && fittingAccessoryInfoList[i].ParentCode != "")
                {
                    if (fittingAccessoryInfoList[i].ParentCode.Length < 10)
                        oldParentCode = fittingAccessoryInfoList[i].ParentCode;
                    else
                        oldParentCode = fittingAccessoryInfoList[i].ParentCode.Substring(
                            0, fittingAccessoryInfoList[i].ParentCode.Length - 10);
                }

                if (!GlobalObject.GeneralFunction.IsNullOrEmpty(fittingAccessoryInfoList[i].FinishTime))
                {
                    fittingAccessoryInfoList[i].FinishTime = serverTime.ToLongDateString() + " " + serverTime.ToLongTimeString();
                }

                // 2014-02-21 删除m_electronFileServer中的方法SaveTempElectronFile
                //if (!m_electronFileServer.SaveTempElectronFile(
                //    fittingAccessoryInfoList[i].OverlayFlag.ToString(), fittingAccessoryInfoSum.ProductCode,
                //    fittingAccessoryInfoList[i].ParentCode, fittingAccessoryInfoList[i].GoodsCode, fittingAccessoryInfoList[i].Spec,
                //    fittingAccessoryInfoList[i].GoodsName, fittingAccessoryInfoList[i].GoodsOnlyCode, fittingAccessoryInfoList[i].Provider,
                //    fittingAccessoryInfoList[i].BatchNo, fittingAccessoryInfoSum.WorkBench, fittingAccessoryInfoList[i].CheckData,
                //    fittingAccessoryInfoList[i].FactData, fittingAccessoryInfoSum.FittingPersonnel,fittingAccessoryInfoSum.FittingTime, 
                //    fittingAccessoryInfoSum.AmendPersonnel, fittingAccessoryInfoSum.AmendTime, fittingAccessoryInfoList[i].Remark,
                //    fittingAccessoryInfoSum.Edition, fittingAccessoryInfoList[i].AssemblyFlag, fittingAccessoryInfoList[i].FinishTime, 
                //    oldParentCode, fittingAccessoryInfoList[i].Counts, out error))
                //{
                //    if (error.Contains(Socket_FittingAccessoryInfo.OperateStateEnum.无法获取Bom基数.ToString()))
                //    {
                //        stateInfo.StateInfo = Socket_FittingAccessoryInfo.OperateStateEnum.无法获取Bom基数;
                //        return stateInfo;
                //    }
                //    else if (error.Contains(Socket_FittingAccessoryInfo.OperateStateEnum.电子档案临时表中无此信息无法覆盖.ToString()))
                //    {
                //        stateInfo.StateInfo = Socket_FittingAccessoryInfo.OperateStateEnum.电子档案临时表中无此信息无法覆盖;
                //        return stateInfo;
                //    }
                //    else
                //    {
                //        stateInfo.StateInfo = Socket_FittingAccessoryInfo.OperateStateEnum.存储装配信息失败;
                //        stateInfo.Error = error;

                //        return stateInfo;
                //    }
                //}
            }

            stateInfo.StateInfo = Socket_FittingAccessoryInfo.OperateStateEnum.操作成功;
            stateInfo.WorkBench = fittingAccessoryInfoSum.WorkBench;

            return stateInfo;
        }

        /// <summary>
        /// 接收完成装配事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="re">事件参数</param>
        public Socket_StateInfo ReceiveSaveFittingInfo(string productCode)
        {
            Socket_StateInfo stateInfo = new Socket_StateInfo();

            if (!m_electronFileServer.SaveElectronFile(productCode))
            {
                stateInfo.StateInfo = Socket_FittingAccessoryInfo.OperateStateEnum.存储装配信息失败;
            }
            else
            {
                stateInfo.StateInfo = Socket_FittingAccessoryInfo.OperateStateEnum.操作成功;
            }

            return stateInfo;
        }
    }
}

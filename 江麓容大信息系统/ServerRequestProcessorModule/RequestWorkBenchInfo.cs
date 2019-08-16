using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SocketCommDefiniens;
using ServerModule;
using System.Data;

namespace ServerRequestProcessorModule
{
    /// <summary>
    /// 响应终端获取工位信息
    /// </summary>
    public class RequestWorkBenchInfo
    {
        /// <summary>
        /// 数据项顺序
        /// </summary>
        readonly string[] m_sortName = {
                                        "主动带轮轴总成", "被动带轮轴总成", "前壳体总成", "中壳体总成",
                                        "后壳体总成", "输入轴总成", "差速器总成", "后端盖总成", 
                                        "中间轴总成", "输出轴总成", "油底壳总成", "液压阀块总成",
                                        "行星轮合件" 
                                       };

        /// <summary>
        /// 零件编码映射(分总成零件名称与总成条形码生成规则表中的虚拟分总成零件编码构成的映射表)
        ///// 零件编码映射(真实分总成零件编码与总成条形码生成规则表中的虚拟分总成零件编码构成的映射表)
        /// </summary>
        static Dictionary<string, string> m_dicVirtualPartMapping = new Dictionary<string, string>();

        /// <summary>
        /// 产品名称与产品编码间的映射字典
        /// </summary>
        static Dictionary<string, string> m_dicProductInfoMapping = new Dictionary<string, string>();

        /// <summary>
        /// 生成真实分总成零件编码与虚拟分总成零件编码映射字典
        /// </summary>
        private void GeneratePartMapping()
        {
            //string sql = "SELECT distinct remark as PartCode, PartCode as virtualPartCode FROM [DepotManagement].[dbo].[P_AssemblyManagementBarcode] A inner join P_ProductInfo B on A.[ProductTypeID] = B.id where PartCode is not null";
            string sql = "SELECT distinct PartName, PartCode as virtualPartCode FROM"+
                         " [DepotManagement].[dbo].[P_AssemblyManagementBarcode] A inner join P_ProductInfo B "+
                         " on A.[ProductTypeID] = B.id where PartCode is not null";
            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            if (dt.Rows.Count == 0)
            {
                return;
            }

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (!m_dicVirtualPartMapping.ContainsKey(dt.Rows[i][0].ToString()))
                {
                    m_dicVirtualPartMapping.Add(dt.Rows[i][0].ToString(), dt.Rows[i][1].ToString());
                }
            }
        }

        /// <summary>
        /// 生成产品名称与产品编码间的映射字典
        /// </summary>
        /// <param name="productCode">产品编码</param>
        private void GenerateProductInfoMapping()
        {
            string sql = "SELECT ProductName, ProductCode FROM [DepotManagement].[dbo].[P_ProductInfo]";
            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            if (dt.Rows.Count == 0)
            {
                return;
            }

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (!m_dicProductInfoMapping.ContainsKey(dt.Rows[i][0].ToString()))
                {
                    m_dicProductInfoMapping.Add(dt.Rows[i][0].ToString(), dt.Rows[i][1].ToString());
                }
            }
        }

        /// <summary>
        /// 根据零件真实图号获取虚拟图号
        /// </summary>
        /// <param name="partCode">真实图号</param>
        /// <returns>成功则返回获取到的零件虚拟图号，失败返回真实图号本身</returns>
        private string GetVirtualPartCode(string partCode)
        {
            if (GlobalObject.GeneralFunction.IsNullOrEmpty(partCode))
            {
                return partCode;
            }

            if (m_dicVirtualPartMapping.ContainsKey(partCode))
            {
                return m_dicVirtualPartMapping[partCode];
            }

            return partCode;
        }

        /// <summary>
        /// 获取指定工位要装配的产品信息
        /// </summary>
        /// <param name="workBench">工位信息</param>
        /// <returns>返回获取到的产品信息</returns>
        public string GetProductInfo(string workBench)
        {
            try
            {
                if (m_dicProductInfoMapping.Count == 0)
                    GenerateProductInfoMapping();

                if (m_dicVirtualPartMapping.Count == 0)
                    GeneratePartMapping();

                IAssemblingBom server = ServerModuleFactory.GetServerModule<IAssemblingBom>();

                // 获取指定工位所有装配信息，包含多种产品
                IQueryable<View_P_AssemblingBom> queryResult = server.GetInfoOfWorkBench(workBench);

                // 提取指定工位所属的产品(哪些产品可以在此工位上装配零件)
                string[] productNames = (from r in queryResult select r.产品名称).Distinct().ToArray();

                StringBuilder sb = new StringBuilder();
                int index = 0;

                foreach (var name in productNames)
                {
                    if (index++ == 0)
                    {
                        sb.AppendFormat("{0}_{1}", name, m_dicProductInfoMapping[name]);
                    }
                    else
                    {
                        sb.AppendFormat(",{0}_{1}", name, m_dicProductInfoMapping[name]);
                    }
                }

                return sb.ToString();
            }
            catch (Exception exce)
            {
                Console.WriteLine(exce.Message);
                return null;
            }            
        }

        /// <summary>
        /// 获取指定工位装配信息
        /// </summary>
        /// <param name="workBench">工位信息</param>
        /// <returns>返回获取到的工位信息</returns>
        public Socket_WorkBenchInfo GetWorkBenchInfo(string workBenchAndProductName)
        {
            char[] splitChars = { ',' };
            string[] splitString = workBenchAndProductName.Split(splitChars);
            string workBench = splitString[0];
            string productName = splitString[1];

            Socket_WorkBenchInfo returnInfo = new Socket_WorkBenchInfo();

            returnInfo.WorkBench = workBench;

            if (workBench[0] == 'F')
            {
                returnInfo.WorkBenchType = WorkbenchTypeEnum.分装.ToString();
            }
            else
            {
                returnInfo.WorkBenchType = WorkbenchTypeEnum.总装.ToString();
            }

            try
            {
                //IProductInfoServer productInfoServer = ServerModuleFactory.GetServerModule<IProductInfoServer>();
                IAssemblingBom server = ServerModuleFactory.GetServerModule<IAssemblingBom>();

                // 获取指定工位所有装配信息，包含多种产品
                IQueryable<View_P_AssemblingBom> queryResult = server.GetInfoOfWorkBench(workBench);

                queryResult = from r in queryResult where r.产品名称 == productName orderby r.父总成编码 select r;

                List<string> parentNamesArray = (from r in queryResult select r.父总成名称).Distinct().ToList();

                List<string> parentNamesArray2 = (from r in queryResult where r.是否总成 == true select r.零件名称).Distinct().ToList();

                if (parentNamesArray2 != null && parentNamesArray2.Count > 0)
                {
                    foreach (var item in parentNamesArray2)
                    {
                        if (!parentNamesArray.Contains(item))
                        {
                            parentNamesArray.Add(item);
                        }
                    }
                }

                // 提取指定工位所属的分总成(哪些分总成可以在此工位上装配零件)
                List<string> parentNames = new List<string>(m_sortName);

                for (int i = 0; i < parentNames.Count; i++)
                {
                    if (!parentNamesArray.Contains(parentNames[i]))
                    {
                        parentNames.RemoveAt(i--);
                    }
                }

                returnInfo.ProductName = productName;

                #region 获取此工位父总成名称列表

                StringBuilder sb = new StringBuilder();
                int index = 0;

                foreach (var parentName in parentNames)
                {
                    if (index++ == 0)
                    {
                        sb.AppendFormat("{0}_{1}", parentName, m_dicVirtualPartMapping[parentName]);
                    }
                    else
                    {
                        sb.AppendFormat(",{0}_{1}", parentName, m_dicVirtualPartMapping[parentName]);
                    }
                }

                returnInfo.FZC = sb.ToString();
                sb.Remove(0, sb.Length);

                #endregion

                #region 获取此工位能够完成的分总成名称

                index = 0;

                foreach (var parentName in parentNames)
                {
                    IQueryable<View_P_AssemblingBom> childrenPart = server.GetChildrenPart(parentName);

                    // 指定产品指定父总成名称需经历的装配工位
                    List<string> parentWorkBench = (from r in childrenPart 
                                                    where r.产品名称 == productName orderby r.工位 
                                                    select r.工位).Distinct().ToList();

                    if (parentWorkBench.Count == 0)
                    {
                        continue;
                    }

                    if (workBench == parentWorkBench[parentWorkBench.Count - 1])
                    {
                        if (index++ == 0)
                        {
                            sb.AppendFormat("{0}", parentName);
                        }
                        else
                        {
                            sb.AppendFormat(",{0}", parentName);
                        }
                    }
                }

                returnInfo.FinishedFZC = sb.ToString();
                sb.Remove(0, sb.Length);

                #endregion

                #region 获取此工位可以装配的零件
                List<WorkbenchPartInfo> workBenchPartInfo = new List<WorkbenchPartInfo>();

                IQueryable<View_P_AssemblingBom> info = from r in queryResult where r.产品名称 == productName select r;

                foreach (var item in info)
                {
                    WorkbenchPartInfo partInfo = new WorkbenchPartInfo();

                    partInfo.ProduceName = item.产品名称;
                    partInfo.ParentName = item.父总成名称;
                    partInfo.ParentCode = item.父总成编码; //GetVirtualPartCode(item.父总成编码);
                    partInfo.PartCode = item.零件编码; //GetVirtualPartCode(item.零件编码);
                    partInfo.PartName = item.零件名称;
                    partInfo.Spec = item.规格;
                    partInfo.Amount = item.装配数量;
                    workBenchPartInfo.Add(partInfo);
                }
                 
                returnInfo.WorkBenchParts = workBenchPartInfo;
                #endregion

                if (workBench == "Z24")
                {
                    returnInfo.IsFirstWorkBenchInAssemblingLine = true;
                }
                else if (workBench == "DB")
                {
                    returnInfo.IsLastWorkBenchAssemblingLine = true;
                }

                returnInfo.OperateState = Socket_WorkBenchInfo.ReturnStateEnum.操作成功;
            }
            catch (Exception exce)
            {
                returnInfo.OperateState = Socket_WorkBenchInfo.ReturnStateEnum.获取工位初始化信息失败;
                returnInfo.ErrorInfo = "RequestWorkBenchInfo.GetWorkBenchInfo：" + exce.Message;
            }

            return returnInfo;
        }
    }
}

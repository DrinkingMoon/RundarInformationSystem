/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  ServerModuleFactory.cs
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
using System.Text;
using System.Collections;
using System.Data;

namespace ServerModule
{
    /// <summary>
    /// 生产管理系统服务类厂
    /// </summary>
    public class PMS_ServerFactory
    {
        /// <summary>
        /// 用于保证服务组件实例的唯一性
        /// </summary>
        static Hashtable m_hashTable = null;

        /// <summary>
        /// 获取服务组件
        /// </summary>
        /// <returns>返回组件接口</returns>
        public static T GetServerModule<T>()
        {
            string name = typeof(T).ToString();
                
            m_hashTable = new Hashtable();

            if (typeof(T) == typeof(IBarCodeServer))
            {
                IBarCodeServer serverModule = BasicServerFactory.GetServerModule<IBarCodeServer>();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IProductInfoServer))
            {
                IProductInfoServer serverModule = new ProductInfoServer();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IBomServer))
            {
                IBomServer serverModule = new BomServer();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IAssemblingBom))
            {
                IAssemblingBom serverModule = new AssemblingBom();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IBarcodeForAssemblyLine))
            {
                IBarcodeForAssemblyLine serverModule = new BarcodeForAssemblyLine();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IElectronFileServer))
            {
                IElectronFileServer serverModule = new ElectronFileServer();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(ITempElectronFileServer))
            {
                ITempElectronFileServer serverModule = new TempElectronFileServer();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IBomMappingServer))
            {
                IBomMappingServer serverModule = new BomMappingServer();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IConvertCVTNumber))
            {
                IConvertCVTNumber serverModule = new ConvertCVTNumber();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IMultiBatchPartServer))
            {
                IMultiBatchPartServer serverModule = new MultiBatchPartServer();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IPrintProductBarcodeInfo))
            {
                IPrintProductBarcodeInfo serverModule = new PrintProductBarcodeInfo();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IProductBarcodeServer))
            {
                IProductBarcodeServer serverModule = new ProductBarcodeServer();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IAssemblyInfoServer))
            {
                IAssemblyInfoServer serverModule = new AssemblyInfoServer();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IChoseConfectServer))
            {
                IChoseConfectServer serverModule = new ChoseConfectServer();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IReparativePartInfoServer))
            {
                IReparativePartInfoServer serverModule = new ReparativePartInfoServer();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IWorkbenchService))
            {
                IWorkbenchService serverModule = new WorkbenchServer();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IConfigManagement))
            {
                IConfigManagement serverModule = new ConfigManagement();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IOffLineTest))
            {
                IOffLineTest serverModule = new OffLineTest();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IProductReleases))
            {
                IProductReleases serverModule = new ProductReleases();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IFrockStandingBook))
            {
                IFrockStandingBook serverModule = new FrockStandingBook();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IInspectionSetInfo))
            {
                IInspectionSetInfo serverModule = new InspectionSetInfo();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(ICVTRepairInfoServer))
            {
                ICVTRepairInfoServer serverModule = new CVTRepairInfoServer();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IValveCheckDataService))
            {
                IValveCheckDataService serverModule = new ValveCheckDataService();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(ICvtCheckDataService))
            {
                ICvtCheckDataService serverModule = new CvtCheckDataService();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IPreventErrorServer))
            {
                IPreventErrorServer serverModule = new PreventErrorServer();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IReplaceWorkBenchServer))
            {
                IReplaceWorkBenchServer serverModule = new ReplaceWorkBenchServer();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IZPXProductionParams))
            {
                IZPXProductionParams serverModule = new ZPXProductionParams();
                m_hashTable.Add(name, serverModule);
            }
            
            
            if (m_hashTable.ContainsKey(name))
            {
                return (T)m_hashTable[name];
            }

            throw new Exception(string.Format("系统中未找到接口：{0}", typeof(T).FullName));
            //return default(T);
        }
    }
}

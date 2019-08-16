/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  CvtCheckDataService.cs
 * 作者    :  夏石友    版本: v1.00    日期: 2014/01/21
 * 开发平台:  vs2005(c#)
 * 用于    :  生产线管理信息系统
 *----------------------------------------------------------------------------
 * 描述 : 查询CVT总成检测数据的服务，数据来源从CVT下线试验台
 * 其它 :
 *----------------------------------------------------------------------------
 * 历史记录:
 *     1. 日期时间: 2014/01/21 13:35:08 作者: 夏石友 当前版本: V1.00
 *        修改说明: 创建
 ******************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using DBOperate;

namespace ServerModule
{
    /// <summary>
    /// CVT 试验信息
    /// </summary>
    public class CvtTestInfo
    {
        /// <summary>
        /// CVT编号
        /// </summary>
        public string CvtNumber;

        /// <summary>
        /// 试验设备(试验台名称等)
        /// </summary>
        public string TestDevice;

        /// <summary>
        /// 油品类型
        /// </summary>
        public string OilType;

        /// <summary>
        /// 试验结论
        /// </summary>
        public string TestResult;

        /// <summary>
        /// 试验人员工号
        /// </summary>
        public string Tester;

        /// <summary>
        /// 预装人员工号
        /// </summary>
        public string PreassemblyPersonnel;

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark;

        /// <summary>
        /// 试验数据集
        /// </summary>
        public DataTable TestData;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="cvtNumber">CVT编号</param>
        /// <param name="testDevice">试验设备</param>
        /// <param name="oilType">油品类型</param>
        /// <param name="testResult">试验结果</param>
        /// <param name="tester">试验人员</param>
        /// <param name="preassemblyPersonnel">预装人员</param>
        /// <param name="remark">备注</param>
        /// <param name="testData">试验数据集</param>
        public CvtTestInfo(
            string cvtNumber, string testDevice,
            string oilType, string testResult,
            string tester, string preassemblyPersonnel,
            string remark, DataTable testData)
        {
            CvtNumber = cvtNumber;
            TestDevice = testDevice;
            OilType = oilType;
            TestResult = testResult;
            Tester = tester;
            PreassemblyPersonnel = preassemblyPersonnel;
            Remark = remark;
            TestData = testData;
        }
    }

    /// <summary>
    /// CVT检测数据服务
    /// </summary>
    class CvtCheckDataService : ServerModule.ICvtCheckDataService
    {
        /// <summary>
        /// 获取指定日期范围内的CVT检测数据
        /// </summary>
        /// <param name="beginTime">起始日期</param>
        /// <param name="endTime">结束日期</param>
        /// <returns>返回获取到的数据</returns>
        public IQueryable<View_ZPX_CVTTestInfo> GetData(DateTime beginTime, DateTime endTime)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var result = from r in ctx.View_ZPX_CVTTestInfo
                         where r.试验日期 >= beginTime.Date && r.试验日期 <= endTime.Date
                         select r;

            return result;
        }

        /// <summary>
        /// 获取指定CVT编号检测数据
        /// </summary>
        /// <param name="cvtNumber">CVT编号</param>
        /// <returns>返回获取到的数据</returns>
        public IQueryable<View_ZPX_CVTTestInfo> GetData(string cvtNumber)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var result = from r in ctx.View_ZPX_CVTTestInfo
                         where (r.产品型号 + " " + r.产品编号) == cvtNumber
                         select r;

            return result;
        }

        /// <summary>
        /// 保存CVT下线试验数据
        /// </summary>
        /// <param name="testInfo">CVT下线试验信息</param>
        /// <returns>返回操作是否成功的标志</returns>
        public bool SaveCVTExpData(CvtTestInfo testInfo)
        {
            if (testInfo == null)
            {
                throw new ArgumentException("试验数据参数不允许为空");
            }

            if (GlobalObject.GeneralFunction.IsNullOrEmpty(testInfo.CvtNumber))
            {
                throw new Exception("CVT编号不允许为空");
            }

            string[] cvtNumber = testInfo.CvtNumber.Split(new char[] { ' ' });

            if (cvtNumber.Length != 2)
            {
                throw new Exception("CVT编号格式不正确");
            }

            if (GlobalObject.GeneralFunction.IsNullOrEmpty(testInfo.OilType))
            {
                throw new Exception("油品类型不允许为空");
            }

            if (GlobalObject.GeneralFunction.IsNullOrEmpty(testInfo.TestDevice))
            {
                throw new Exception("试验设备名称不允许为空");
            }

            if (GlobalObject.GeneralFunction.IsNullOrEmpty(testInfo.TestResult))
            {
                throw new Exception("试验结果不允许为空");
            }

            if (GlobalObject.GeneralFunction.IsNullOrEmpty(testInfo.Tester))
            {
                throw new Exception("实验员不允许为空");
            }

            if (GlobalObject.GeneralFunction.IsNullOrEmpty(testInfo.PreassemblyPersonnel))
            {
                throw new Exception("预装员不允许为空");
            }

            if (testInfo.TestData == null || testInfo.TestData.Rows.Count == 0)
            {
                throw new Exception("试验项目表不允许为空");
            }

            string error = null;

            Hashtable paramTable = new Hashtable();

            paramTable.Add("@ProductType", cvtNumber[0]);
            paramTable.Add("@CvtNumber", cvtNumber[1]);
            paramTable.Add("@TestDevice", testInfo.TestDevice);
            paramTable.Add("@OilType", testInfo.OilType);
            paramTable.Add("@TestResult", testInfo.TestResult);
            paramTable.Add("@Tester", testInfo.Tester);
            paramTable.Add("@PreassemblyPersonnel", testInfo.PreassemblyPersonnel);
            paramTable.Add("@Remark", testInfo.Remark);

            IDBOperate dbOperate = AccessDB.GetIDBOperate("DepotManagement");

            string[] lstCmd = new string[testInfo.TestData.Rows.Count + 1];
            Hashtable[] lstParam = new Hashtable[testInfo.TestData.Rows.Count + 1];
            Hashtable[] outParams = new Hashtable[testInfo.TestData.Rows.Count + 1];

            lstCmd[0] = "ZPX_InsertCVTTestInfo";
            lstParam[0] = paramTable;

            for (int i = 0; i < testInfo.TestData.Rows.Count; i++)
            {
                lstCmd[i + 1] = "ZPX_InsertCVTTestDataItems";

                paramTable = new Hashtable();

                paramTable.Add("@TestType", testInfo.TestData.Rows[i]["TestType"].ToString());
                paramTable.Add("@TestItemName", testInfo.TestData.Rows[i]["TestItemName"].ToString());
                paramTable.Add("@TestCondition", testInfo.TestData.Rows[i]["TestCondition"].ToString());
                paramTable.Add("@TestRequirement", testInfo.TestData.Rows[i]["TestRequirement"].ToString());
                paramTable.Add("@TestData", testInfo.TestData.Rows[i]["TestData"].ToString());
                paramTable.Add("@TestResult", testInfo.TestData.Rows[i]["TestResult"].ToString());

                lstParam[i + 1] = paramTable;
            }

            Dictionary<OperateCMD, object> dicOperateCMD = dbOperate.Transaction_CMD(lstCmd, lstParam, ref outParams);

            if (!AccessDB.GetResult(dicOperateCMD, out error))
            {
                throw new Exception(error);
            }
            else
            {
                return true;
            }
        }
    }
}

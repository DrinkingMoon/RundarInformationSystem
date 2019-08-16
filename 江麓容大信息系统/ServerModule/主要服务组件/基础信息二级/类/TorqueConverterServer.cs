using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ServerModule
{
    /// <summary>
    /// 液力变矩器标识码信息操作类
    /// </summary>
    class TorqueConverterInfoServer : BasicServer, ITorqueConverterInfoServer
    {
        /// <summary>
        /// 系统中是否已经存在现在导入的唯一编码
        /// </summary>
        /// <param name="importCode">要导入的唯一编码列表</param>
        /// <param name="existedCode">已经存在的唯一编号</param>
        /// <returns>存在返回true</returns>
        private bool IsExistsUniqueCode(List<string> importCode, out string existedCode)
        {
            existedCode = null;

            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            var result = from r in dataContxt.S_TorqueConverterInfo
                         where importCode.Contains(r.UniqueCode)
                         select r.UniqueCode;

            if (result.Count() == 0)
            {
                return false;
            }

            StringBuilder sb = new StringBuilder();

            foreach (var item in result)
            {
                sb.AppendFormat("{0},", item);
            }

            sb.Remove(sb.Length - 1, 1);

            existedCode = sb.ToString();
            return true;
        }

        /// <summary>
        /// 导入厂家提供的液力变矩器数据
        /// </summary>
        /// <param name="dt">数据表</param>
        /// <param name="billNo">报检单单据号</param>
        /// <param name="batchNo">批次号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        public bool ImportInfo(DataTable dt, string billNo, string batchNo, out string error)
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            if (dt == null || dt.Rows.Count == 0)
            {
                error = "没有包含所需的信息，无法导入";
                return false;
            }

            try
            {
                List<string> uniqueCodes = new List<string>();
                error = null;

                foreach (DataRow row in dt.Rows)
                {
                    if (row[0] != null && row[0] != DBNull.Value 
                        && !GlobalObject.GeneralFunction.IsNullOrEmpty(row[0].ToString().Trim()))
                        uniqueCodes.Add(row["变矩器出厂编号"].ToString());
                }

                string existedCode = null;

                if (IsExistsUniqueCode(uniqueCodes, out existedCode))
                {
                    error = "不允许导入重复信息，下列变矩器出厂编号已经存在：" + existedCode;
                    return false;
                }

                foreach (DataRow row in dt.Rows)
                {
                    if (row[0] == null || row[0] == DBNull.Value 
                        || GlobalObject.GeneralFunction.IsNullOrEmpty(row[0].ToString().Trim()))
                        break;

                    S_TorqueConverterInfo info = new S_TorqueConverterInfo();

                    info.GoodsCode = row["液力变矩器型号"].ToString();
                    info.LeaveFactoryDate = Convert.ToDateTime(row["出厂日期"]);
                    info.BoxNumber = row["箱号"].ToString();
                    info.TorqueConverterNumber = Convert.ToInt32(row["变矩器序号"]);
                    info.UniqueCode = row["变矩器出厂编号"].ToString();
                    info.BillNo = billNo;
                    info.BatchNo = batchNo;

                    dataContxt.S_TorqueConverterInfo.InsertOnSubmit(info);
                }

                dataContxt.SubmitChanges();
                return true;
            }
            catch (Exception exce)
            {
                error = exce.Message;
                return false;
            }
        }

        /// <summary>
        /// 删除指定单据的变矩器标识信息（当单据报废时进行此操作）
        /// </summary>
        /// <param name="billNo">单据编号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        public bool DeleteInfo(string billNo, out string error)
        {
            try
            {
                error = null;

                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from r in dataContxt.S_TorqueConverterInfo
                             where r.BillNo == billNo
                             select r;

                dataContxt.S_TorqueConverterInfo.DeleteAllOnSubmit(result);
                dataContxt.SubmitChanges();

                return true;
            }
            catch (Exception exce)
            {
                error = exce.Message;
                return false;
            }
        }
    }
}

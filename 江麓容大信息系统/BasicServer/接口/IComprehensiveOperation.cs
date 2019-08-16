using System;
using System.Linq;
using System.Data;

namespace ServerModule
{
    /// <summary>
    /// 综合性操作接口
    /// </summary>
    public interface IComprehensiveOperation
    {
        /// <summary>
        /// 操作隐藏字段数据集
        /// </summary>
        /// <param name="formName">窗体名称</param>
        /// <param name="dataGridViewName">数据显示控件名称</param>
        /// <param name="userCode">用户编码</param>
        /// <param name="dtHideFields">是否隐藏设置表</param>
        /// <param name="error">出错时，输出错误信息</param>
        /// <returns>操作是否成功的标志</returns>
        bool OperationHideFields(string formName, string dataGridViewName,
            string userCode, DataTable dtHideFields, out string error);
    }
}

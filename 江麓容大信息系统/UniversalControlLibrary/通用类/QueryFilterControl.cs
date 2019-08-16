using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GlobalObject;

namespace UniversalControlLibrary
{
    /// <summary>
    /// 数据查询过滤控制类
    /// </summary>
    public sealed class QueryFilterControl
    {
        /// <summary>
        /// 参数对象
        /// </summary>
        static Dictionary<string, object> m_dicFilterParams;

        /// <summary>
        /// XML配置文件操作类
        /// </summary>
        static MultiLevelXmlParams m_xmlParams;

        /// <summary>
        /// 过滤器字符串字典
        /// </summary>
        static Dictionary<string, string> m_dicFilterString = new Dictionary<string,string>();

        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns>初始化成功返回true</returns>
        static public bool Init()
        {
            try
            {
                m_xmlParams = new MultiLevelXmlParams(Environment.CurrentDirectory + "\\FilterParams.xml");
                m_dicFilterParams = m_xmlParams.GetParams();

                return true;
            }
            catch //(Exception err)
            {
                //MessageDialog.ShowErrorMessage(err.Message);
            }

            return false;
        }

        /// <summary>
        /// 保存数据到配置文件
        /// </summary>
        static public void Save()
        {
            if (m_xmlParams != null)
            {
                m_xmlParams.SaveParams(m_dicFilterParams, true);
            }
        }

        /// <summary>
        /// 生成过滤信息串
        /// </summary>
        /// <param name="filterName">过滤名</param>
        /// <returns>成功返回生成的过滤字符串，失败返回null</returns>
        static private string GenerateFilterString(string filterName)
        {
            List<FilterInfo> lstFilterInfo = GetFilterInfo(filterName);

            if (lstFilterInfo == null)
            {
                return null;
            }

            lstFilterInfo.Sort();

            StringBuilder sb = new StringBuilder();
            int index = 0;

            sb.Append(" (");

            foreach (var item in lstFilterInfo)
            {
                string operatorSymbol = item._operator1;

                if (operatorSymbol == "包含")
                {
                    operatorSymbol = "like";
                }

                sb.Append(item.LeftParentheses);

                if (item.DataType == typeof(DateTime).Name && operatorSymbol == "=")
                {
                    sb.AppendFormat("({0} >= '{1} 00:00:00' and {0} <= '{1} 00:00:00')", item.FieldName, item.DataValue);
                }
                else if (operatorSymbol == "like")
                {
                    sb.AppendFormat(" {0} {1} '%{2}%' ", item.FieldName, operatorSymbol, item.DataValue);
                }
                else if (operatorSymbol == "是" || operatorSymbol == "不是")
                {
                    if (operatorSymbol == "是")
                    {
                        operatorSymbol = "is";
                    }
                    else
                    {
                        operatorSymbol = "is not";
                    }

                    sb.AppendFormat(" {0} {1} {2} ", item.FieldName, operatorSymbol, "null"); //item.DataValue);
                }
                else if (item.DataType.ToLower() == typeof(string).Name.ToLower() 
                    || (item.DataType.ToLower() == typeof(DateTime).Name.ToLower()))
                {
                    sb.AppendFormat(" {0} {1} '{2}' ", item.FieldName, operatorSymbol, item.DataValue);
                }
                else
                {
                    sb.AppendFormat(" {0} {1} {2} ", item.FieldName, operatorSymbol, item.DataValue);
                }

                sb.Append(item.RghtParentheses);

                if (++index != lstFilterInfo.Count)
                {
                    sb.AppendFormat(" {0} ", item.Logic);
                }
            }

            sb.Append(" )");
            return sb.ToString();
        }

        /// <summary>
        /// 获取过滤信息
        /// </summary>
        /// <param name="filterName">过滤名</param>
        /// <returns>成功返回获取到的过滤字符串，失败返回null</returns>
        static public string GetFilterString(string filterName)
        {
            if (m_dicFilterString.ContainsKey(filterName))
            {
                return m_dicFilterString[filterName];
            }

            m_dicFilterString.Add(filterName, GenerateFilterString(filterName));
            return m_dicFilterString[filterName];
        }

        /// <summary>
        /// 获取过滤信息
        /// </summary>
        /// <param name="filterName">过滤名</param>
        /// <returns>成功返回获取到的过滤信息，失败返回null</returns>
        static public List<FilterInfo> GetFilterInfo(string filterName)
        {
            if (m_dicFilterParams == null)
            {
                //MessageDialog.ShowPromptMessage("载入过滤信息源未成功，无法获取当前过滤信息");
                return null;
            }

            if (!m_dicFilterParams.ContainsKey(filterName))
            {
                //MessageDialog.ShowPromptMessage(string.Format("未找到过滤信息：{0}", filterName));
                return null;
            }

            Dictionary<string, object> filterParams = m_dicFilterParams[filterName] as Dictionary<string, object>;
            Dictionary<string, string> fieldParams;
            List<FilterInfo> lstFilterInfo = new List<FilterInfo>();

            foreach (KeyValuePair<string, Object> item in filterParams)
            {
                fieldParams = item.Value as Dictionary<string, string>;

                FilterInfo info = new FilterInfo();

                info.OrderNo = Convert.ToInt32(fieldParams["OrderNo"]);
                info.FieldName = item.Key;
                info.DataValue = fieldParams["DataValue"];
                info.DataType = fieldParams["DataType"];
                info.Logic = fieldParams["Logic"];
                info._operator1 = fieldParams["Operator"];
                info.LeftParentheses = fieldParams["LeftParentheses"];
                info.RghtParentheses = fieldParams["RightParentheses"];

                lstFilterInfo.Add(info);
            }

            if (lstFilterInfo.Count == 0)
                return null;
            else 
                return lstFilterInfo;
        }

        /// <summary>
        /// 保存过滤参数
        /// </summary>
        /// <param name="filterName">过滤名</param>
        /// <param name="lstFilterInfo">过滤参数列表</param>
        static public void SaveFilter(string filterName, List<FilterInfo> lstFilterInfo)
        {
            if (m_dicFilterParams == null)
            {
                m_dicFilterParams = new Dictionary<string, object>();
                //MessageDialog.ShowPromptMessage("载入过滤信息源未成功，无法保存当前过滤信息");
                //return;
            }

            if (m_dicFilterParams.ContainsKey(filterName))
            {
                m_dicFilterParams.Remove(filterName);
            }

            Dictionary<string, object> filterParams = new Dictionary<string, object>();

            m_dicFilterParams.Add(filterName, filterParams);

            foreach (var item in lstFilterInfo)
            {
                Dictionary<string, string> fieldParams = new Dictionary<string, string>();

                filterParams.Add(item.FieldName, fieldParams);

                fieldParams["OrderNo"] = item.OrderNo.ToString();
                fieldParams["LeftParentheses"] = item.LeftParentheses;
                fieldParams["RightParentheses"] = item.RghtParentheses;
                fieldParams["Operator"] = item._operator1;
                fieldParams["DataType"] = item.DataType;
                fieldParams["DataValue"] = item.DataValue;
                
                fieldParams["Logic"] = item.Logic;
            }

            if (m_dicFilterString.ContainsKey(filterName))
            {
                m_dicFilterString[filterName] = GenerateFilterString(filterName);
            }
            else
            {
                m_dicFilterString.Add(filterName, GenerateFilterString(filterName));
            }
        }
    }
}

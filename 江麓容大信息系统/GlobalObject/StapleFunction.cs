using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Reflection;

namespace GlobalObject
{
    /// <summary>
    /// 通用功能
    /// </summary>
    public class StapleFunction
    {
        /// <summary>
        /// 是否是正确的手机号码
        /// </summary>
        /// <param name="handset">需要验证的手机号码</param>
        /// <returns>是返回true</returns>
        static public bool IsHandset(string handset)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(handset, @"^[1]+[3,4,5,8]+\d{9}");
        }

        /// <summary>
        /// 从字符串参数中提取非数字字符信息
        /// </summary>
        /// <param name="strData">包含数据字符信息的字符串</param>
        /// <returns>获取到的不包含数据字符信息的字符串</returns>
        static public string GetNonNumericString(string strData)
        {
            StringBuilder sb = new StringBuilder();
            string strSource = "0123456789.-+";

            for (int i = 0; i < strData.Length; i++)
            {
                if (strSource.IndexOf(strData[i]) == -1)
                {
                    sb.Append(strData[i]);
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// 是否汉字字符串
        /// </summary>
        /// <param name="strData">要判别的字符串</param>
        /// <returns>是汉字字符串返回true</returns>
        static public bool IsChineseCharacters(string strData)
        {
            string pat = @"[\u4e00-\u9fa5]";
            Regex rg = new Regex(pat);
            Match mh = rg.Match(strData);

            if (mh.Success)
            {
                return true;
            }
            else
            {
                // 判断是否包含了全角符号
                if (strData.Length != Encoding.Default.GetByteCount(strData))
                {
                    return true;
                }

                return false;
            }
        }

        /// <summary>
        /// 去空格
        /// </summary>
        /// <param name="strSur">需要去除空格的源字符串</param>
        /// <returns>将源字符串去除空格后得到的字符串</returns>
        static public string DeleteSpace(string strSur)
        {
            while (0 < 1)
            {
                string strOK = strSur;

                strSur = strSur.Replace("　", "");
                strSur = strSur.Replace(" ", "");

                if (strSur.Length == strOK.Length)
                {
                    return strSur;
                }
            }
        }

        /// <summary>
        /// 生成带分隔符的字符串
        /// 举例：分隔符','，数据1："a"，数据2："b"，生成的数据结果："a,b"
        /// </summary>
        /// <param name="split">分隔符</param>
        /// <param name="data">需加分隔符的数据数组</param>
        /// <returns>成功返回带分隔符的字符串，失败返回null</returns>
        static public string CreateSplitString(string split, List<string> data)
        {
            if (GlobalObject.GeneralFunction.IsNullOrEmpty(split) || data == null || data.Count == 0)
            {
                return null;
            }

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < data.Count; i++)
            {
                sb.AppendFormat("{0},", data[i]);
            }

            sb.Remove(sb.Length - 1, 1);

            return sb.ToString();
        }

        /// <summary>
        /// 简单比较2个对象的属性值是否一致
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="sur">要比较的源数据</param>
        /// <param name="tar">要比较的目标数据</param>
        /// <returns>属性值相同返回true</returns>
        static public bool SimpleEqual<T>(T sur, T tar)
        {
            var props = sur.GetType().GetProperties();

            foreach (PropertyInfo pi in props)
            {
                if (pi.GetValue(sur, null) == pi.GetValue(tar, null))
                {
                    continue;
                }

                if ((pi.GetValue(sur, null) == null && pi.GetValue(tar, null) != null)
                    || (pi.GetValue(sur, null) != null && pi.GetValue(tar, null) == null)
                    || (pi.GetValue(sur, null).ToString() != pi.GetValue(tar, null).ToString()))
                {
                    return false;
                }
            }

            return true;
        }
    }
}

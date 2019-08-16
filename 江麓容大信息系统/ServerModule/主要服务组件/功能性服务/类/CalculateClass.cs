/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  CalculateClass.cs
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

namespace ServerModule
{
    /// <summary>
    /// 金额转换类
    /// </summary>
    public class CalculateClass
    {
        private enum NumValues
        {
            零 = 0x00,

            壹 = 0x01,

            贰 = 0x02,

            叁 = 0x03,

            肆 = 0x04,

            伍 = 0x05,

            陆 = 0x06,

            柒 = 0x07,

            捌 = 0x08,

            玖 = 0x09
        }

        private enum UpValues
        {
            万 = 0x01,

            亿 = 0x02
        }

        private enum byteValues
        {
            拾 = 0x01,

            佰 = 0x02,

            仟 = 0x03
        }

        static string pre = null;

        /// <summary>
        /// 小数位大写金额
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetDecimalData(string value)
        {
            char[] tmp = value.ToCharArray();

            if (tmp.Length >= 2)
            {
                Array.Resize(ref tmp, 2);
            }

            Array.Reverse(tmp);

            System.Diagnostics.Debug.Assert(tmp != null);

            int len = tmp.Length;

            StringBuilder stu = new StringBuilder();

            for (int i = 0; i < 2; i++)
            {
                pre = null;

                if (i >= tmp.Length)
                {
                    break;
                }

                char si = tmp[i];

                string charValue = ((NumValues)(int.Parse(si.ToString()))).ToString();

                if (i == 0)
                {
                    stu.Insert(0, charValue + "分");
                }
                else if (i == 1)
                {
                    stu.Insert(0, charValue + "角");
                }

                pre = charValue;

            }

            //Console.WriteLine(stu.ToString());

            return stu.ToString();
        }

        /// <summary>
        /// 大写金额
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetTotalPrice(decimal value)
        {
            if (value == 0)
            {
                return "零元整";
            }

            long dec = (long)Math.Floor(value);

            char[] tmp = dec.ToString().ToCharArray();

            Array.Reverse(tmp);

            System.Diagnostics.Debug.Assert(tmp != null);

            int len = tmp.Length;

            int fa = (int)Math.Ceiling(len * 1.0 / 4);

            StringBuilder stu = new StringBuilder();

            int index;

            for (int i = 0; i < fa; i++)
            {
                if (i > 0)
                {
                    stu.Insert(0, ((UpValues)i).ToString());
                }

                pre = null;

                for (int j = 0; j < 4; j++)
                {
                    index = i * 4 + j;

                    if (index >= tmp.Length)
                    {
                        break;
                    }

                    char si = tmp[index];

                    string charValue = ((NumValues)(int.Parse(si.ToString()))).ToString();

                    if (charValue == pre && charValue == "零")
                    {
                    }
                    else
                    {
                        if (charValue != "零")
                        {
                            if (j != 0)
                            {
                                charValue += ((byteValues)j).ToString();
                            }

                            stu.Insert(0, charValue);
                        }
                        else
                        {
                            stu.Insert(0, charValue);
                        }

                        pre = charValue;
                    }
                }

            }

            //Console.WriteLine(stu.ToString());

            if (value > 1 && (decimal)value - (decimal)dec >= (decimal)0.01)
            {
                string xs = value.ToString().Remove(0, len + 1);

                if (xs.Length == 1)
                    xs = xs.Insert(1, "0");

                stu.Append("元" + GetDecimalData(xs));
            }
            else if ((decimal)value - (decimal)dec >= (decimal)0.01)
            {
                string xs = value.ToString().Remove(0, len + 1);

                if (xs.Length == 1)
                    xs = xs.Insert(1, "0");

                stu.Append(GetDecimalData(xs));
            }
            else
            {
                stu.Append("元整");
            }

            // 去除多除的万字
            string retValue = stu.ToString();

            retValue = retValue.Replace("零亿", "亿");
            retValue = retValue.Replace("零万", "万");
            retValue = retValue.Replace("零仟", "仟");
            retValue = retValue.Replace("零佰", "佰");
            retValue = retValue.Replace("零拾", "拾");
            retValue = retValue.Replace("零元", "元");
            retValue = retValue.Replace("零零", "零");
            retValue = retValue.Replace("零角", "零");
            retValue = retValue.Replace("零分", "");
            retValue = retValue.Replace("亿万", "亿");

            if (retValue.Substring(0,1) == "零")
            {
                retValue = retValue.Remove(0, 1);
            }

            //Console.WriteLine("{0}  {1}", value, retValue);
            return retValue;
        }
    }
}

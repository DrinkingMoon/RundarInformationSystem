using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace ServerModule
{
    /// <summary>
    /// 拼音码服务类
    /// </summary>
    public class PYMnumber
    {
        /// <summary>
        /// 获得拼音码
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>返回拼音码</returns>
        public static string GetPYString(string str)
        {
            string tempStr = "";

            foreach (char c in str)
            {
                if ((int)c >= 33 && (int)c <= 126)//  33--126是所有显示英文下的字符.字母和符号原样保留
                {
                    tempStr += c.ToString();
                }
                else
                {
                    tempStr += GetPYChar(c.ToString()); //累加拼音声母
                }
            }

            return tempStr;
        }
        /// <summary>
        /// 取单个字符的拼音声母
        /// </summary>
        /// <param name="str">要转换的单个汉字</param>
        /// <returns>拼音声母(I、U、V不能返回)</returns>
        public static string GetPYChar(string str)
        {
            byte[] array = new byte[2];
            array = System.Text.Encoding.Default.GetBytes(str);//把符字转为字符序列

            int intPYnumber = (short)(array[0] - '\0') * 256 + ((short)(array[1] - '\0'));

            if ((intPYnumber >= 45217) && (intPYnumber <= 45252))
            {
                return "A";
            }

            if ((intPYnumber >= 45253) && (intPYnumber <= 45760))
            {
                return "B";
            }

            if ((intPYnumber >= 45761) && (intPYnumber <= 46317))
            {
                return "C";
            }

            if ((intPYnumber >= 46318) && (intPYnumber <= 46825))
            {
                return "D";
            }

            if ((intPYnumber >= 46826) && (intPYnumber <= 47009))
            {
                return "E";
            }

            if ((intPYnumber >= 47010) && (intPYnumber <= 47296))
            {
                return "F";
            }

            if ((intPYnumber >= 47297) && (intPYnumber <= 47613))
            {
                return "G";
            }

            if ((intPYnumber >= 47614) && (intPYnumber <= 48118))
            {
                return "H";
            }

            if ((intPYnumber >= 48119) && (intPYnumber <= 49061))
            {
                return "J";
            }

            if ((intPYnumber >= 49062) && (intPYnumber <= 49323))
            {
                return "K";
            }

            if ((intPYnumber >= 49324) && (intPYnumber <= 49895))
            {
                return "L";
            }

            if ((intPYnumber >= 49896) && (intPYnumber <= 50370))
            {
                return "M";
            }

            if ((intPYnumber >= 50371) && (intPYnumber <= 50613))
            {
                return "N";
            }
            if ((intPYnumber >= 50614) && (intPYnumber <= 50621))
            {
                return "O";
            }

            if ((intPYnumber >= 50622) && (intPYnumber <= 50905))
            {
                return "P";
            }

            if ((intPYnumber >= 50906) && (intPYnumber <= 51386))
            {
                return "Q";
            }

            if ((intPYnumber >= 51387) && (intPYnumber <= 51445))
            {
                return "R";
            }

            if ((intPYnumber >= 51446) && (intPYnumber <= 52217))
            {
                return "S";
            }

            if ((intPYnumber >= 52218) && (intPYnumber <= 52697))
            {
                return "T";
            }

            if ((intPYnumber >= 52698) && (intPYnumber <= 52979))
            {
                return "W";
            }

            if ((intPYnumber >= 52980) && (intPYnumber <= 53640))
            {
                return "X";
            }

            if ((intPYnumber >= 53689) && (intPYnumber <= 54480))
            {
                return "Y";
            }
            if ((intPYnumber >= 54481) && (intPYnumber <= 55289))
            {
                return "Z";
            }

            return "*";
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace ServerModule
{
    /// <summary>
    /// 获取服务器时间的类
    /// </summary>
    public class ServerTime
    {
        /// <summary>
        /// 数据库上下文
        /// </summary>
        static DepotManagementDataContext m_context = CommentParameter.DepotDataContext;

        /// <summary>
        /// 获取服务器时间
        /// </summary>
        /// <returns>获取到的服务器当前时间</returns>
        static public DateTime Time
        {
            get
            {
                try
                {
                    System.Nullable<System.DateTime> curTime = m_context.Fun_get_ServerTime();
                    return (DateTime)curTime;
                }
                catch (Exception exce)
                {
                    Console.WriteLine("DateTime.Now：{0}", exce.Message);
                    return DateTime.Now;
                }
            }
        }

        /// <summary>
        /// 开始时间
        /// </summary>
        /// <param name="date">日期</param>
        /// <returns>返回DateTime</returns>
        static public DateTime StartTime(DateTime date)
        {
            try
            {
                return Convert.ToDateTime( date.ToShortDateString() + " 00:00:00");
            }
            catch (Exception exce)
            {
                Console.WriteLine("DateTime.Now：{0}", exce.Message);
                return DateTime.Now;
            }
        }

        /// <summary>
        /// 结束时间
        /// </summary>
        /// <param name="date">日期</param>
        /// <returns>返回DateTime</returns>
        static public DateTime EndTime(DateTime date)
        {
            try
            {
                return Convert.ToDateTime(date.ToShortDateString() + " 23:59:59");
            }
            catch (Exception exce)
            {
                Console.WriteLine("DateTime.Now：{0}", exce.Message);
                return DateTime.Now;
            }
        }

        /// <summary>
        /// 对小于10的月份 进行加0
        /// </summary>
        /// <returns>返回两位的字符串，月份的表示</returns>
        static public string GetMouth()
        {
            return ServerTime.Time.Month.ToString("D2");
        }

        /// <summary>
        /// 转换字符串为日期
        /// </summary>
        /// <param name="date">字符串日期</param>
        /// <returns>返回DateTime</returns>
        public static DateTime ConvertToDateTime(string date)
        {
            string strReturn = "";

            char[] chSplit = { '-', '.' };

            string[] strArray = date.Split(chSplit);

            switch (strArray.Length)
            {
                case 1:

                    if (date.Length == 6)
                    {
                        strReturn = date.Substring(0, 4) + "-" + date.Substring(4, 2) + "-1";
                    }
                    else if (date.Length == 8)
                    {
                        strReturn = date.Substring(0, 4) + "-" + date.Substring(4, 2)
                            + "-" + date.Substring(6, 2) + "";
                    }
                    else
                    {
                        strReturn = "2099-12-31";
                    }

                    break;
                case 2:
                    strReturn = strArray[0] + "-" + strArray[1] + "-1";
                    break;
                case 3:
                    strReturn = strArray[0] + "-" + strArray[1] + "-" + strArray[2] + "";
                    break;
                default:
                    strReturn = "2099-12-31";
                    break;
            }

            return Convert.ToDateTime(strReturn);
        }

        public static DateTime ConvertToDateTime(DateTime? date)
        {
            if (date == null)
            {
                throw new Exception("日期为NULL，转换失败");
            }

            DateTime result = 
                Convert.ToDateTime(Convert.ToDateTime(date).ToShortDateString() 
                + " " + Convert.ToDateTime(date).ToShortTimeString());

            return result;
        }

        /// <summary>
        /// 获得年月
        /// </summary>
        /// <param name="nowTime">当前时间</param>
        /// <param name="dayNode">节点日</param>
        /// <returns>返回字符串年月</returns>
        public static string GetMonthlyString(DateTime nowTime)
        {
            string strNy = "";

            int dayNode = Convert.ToInt32(GlobalObject.BasicInfo.BaseSwitchInfo[(int)GlobalObject.CE_SwitchName.月结日]);
            bool calculateFlag = Convert.ToBoolean(GlobalObject.BasicInfo.BaseSwitchInfo[(int)GlobalObject.CE_SwitchName.月结日推算方式]);

            if (calculateFlag)
            {
                if (nowTime.Day < dayNode)
                {
                    strNy = nowTime.Year.ToString() + (nowTime.Month + 1).ToString("D2");
                }
                else
                {
                    strNy = nowTime.Year.ToString() + nowTime.Month.ToString("D2");
                }
            }
            else
            {
                if (nowTime.Day >= dayNode)
                {
                    if (nowTime.Month == 12)
                    {
                        strNy = (nowTime.Year + 1).ToString() + "01";
                    }
                    else
                    {
                        strNy = nowTime.Year.ToString() + (nowTime.Month + 1).ToString("D2");
                    }
                }
                else
                {
                    strNy = nowTime.Year.ToString() + nowTime.Month.ToString("D2");
                }
            }

            return strNy;
        }

        /// <summary>
        /// 获得月结日期范围
        /// </summary>
        /// <param name="nowTime">当前时间</param>
        /// <param name="startTime">开始日期</param>
        /// <param name="endTime">结束日期</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>Bool型返回</returns>
        public static void GetMonthlyBalance(string strNy, out DateTime startTime, out DateTime endTime)
        {
            startTime = Convert.ToDateTime("2099-12-31 00:00:00");
            endTime = Convert.ToDateTime("2099-12-31 00:00:00");

            int dayNode = Convert.ToInt32(GlobalObject.BasicInfo.BaseSwitchInfo[(int)GlobalObject.CE_SwitchName.月结日]);
            bool calculateFlag = Convert.ToBoolean(GlobalObject.BasicInfo.BaseSwitchInfo[(int)GlobalObject.CE_SwitchName.月结日推算方式]);

            if (calculateFlag)
            {
                startTime = new DateTime(Convert.ToInt32(strNy.Substring(0, 4)), Convert.ToInt32(strNy.Substring(4, 2)), dayNode);
                endTime = startTime.AddMonths(1);
            }
            else
            {
                endTime = new DateTime(Convert.ToInt32(strNy.Substring(0, 4)), Convert.ToInt32(strNy.Substring(4, 2)), dayNode);
                startTime = endTime.AddMonths(-1);
            }
        }

        /// <summary>
        /// 获得月结日期范围
        /// </summary>
        /// <param name="nowTime">当前时间</param>
        /// <param name="startTime">开始日期</param>
        /// <param name="endTime">结束日期</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>Bool型返回</returns>
        public static void GetMonthlyBalance(DateTime nowTime, out DateTime startTime, out DateTime endTime)
        {
            startTime = Convert.ToDateTime("2099-12-31 00:00:00");
            endTime = Convert.ToDateTime("2099-12-31 00:00:00");

            GetMonthlyBalance(GetMonthlyString(nowTime), out startTime, out endTime);
        }
    }
}

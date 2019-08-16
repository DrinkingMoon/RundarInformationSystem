/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  Year.cs
 * 作者    :  曹津彬    版本: v1.00    日期: 2014/01/22
 * 开发平台:  Visual C# 2008
 * 用于    :  仓库管理软件
 *----------------------------------------------------------------------------
 * 描述 : 
 * 其它 :
 *----------------------------------------------------------------------------
 * 公共信息: 参见系统'类帮助文档'
 *----------------------------------------------------------------------------
 * 历史记录:
 ******************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GlobalObject
{
    /// <summary>
    /// 用于判断闰年、每月天数的类
    /// </summary>
    public class Year
    {
        /// <summary>
        /// 每个月天数
        /// </summary>
        static int[] m_days = new int[] { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };

        /// <summary>
        /// 判定公历闰年遵循的一般规律为：四年一闰，百年不闰，四百年再闰。
        /// 公历闰年的精确计算方法：（按一回归年365天5小时48分45.5秒）
        /// 普通年能被4整除而不能被100整除的为闰年。 （如2004年就是闰年，1900年不是闰年）
        /// 世纪年能被400整除而不能被3200整除的为闰年。 (如2000年是闰年，3200年不是闰年)
        /// 对于数值很大的年份能整除3200,但同时又能整除172800则又是闰年。(如172800年是闰年，86400年不是闰年）
        /// 
        /// 公元前闰年规则如下：
        /// 非整百年：年数除4余数为1是闰年，即公元前1、5、9……年；
        /// 整百年：年数除400余数为1是闰年，年数除3200余数为1，不是闰年,年数除172800余1又为闰年，即公元前401、801……年。
        /// </summary>
        /// <param name="yN">年份数字</param>
        /// <returns></returns>
        public static bool IsLeapYear(int year)
        {

            if ((year % 400 == 0 && year % 3200 != 0)
               || (year % 4 == 0 && year % 100 != 0)
               || (year % 3200 == 0 && year % 172800 == 0))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 获取指定年、月包含的天数
        /// </summary>
        /// <param name="year">年份</param>
        /// <param name="month">月份</param>
        /// <returns>成功返回获取到的天数, 失败返回-1</returns>
        public static int GetDays(int year, int month)
        {
            if (year < 0 || month < 1 || month > 12)
            {
                return -1;
            }

            if (month == 2)
            {
                if (IsLeapYear(year))
                {
                    return 29;
                }
                else
                {
                    return 28;
                }
            }
            else
            {
                return m_days[month-1];
            }
        }
                
        /// <summary>
        /// 获取指定日期所在月份包含的天数
        /// </summary>
        /// <param name="date">日期</param>
        /// <returns>成功返回获取到的天数, 失败返回-1</returns>
        public static int GetDays(DateTime date)
        {
            return GetDays(date.Year, date.Month);
        }
    }
}

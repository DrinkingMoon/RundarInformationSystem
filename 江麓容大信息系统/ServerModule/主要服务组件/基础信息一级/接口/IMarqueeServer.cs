using System;
namespace ServerModule
{
    /// <summary>
    /// 跑马灯的内容显示操作类
    /// </summary>
    public interface IMarqueeServer
    {
        /// <summary>
        /// 获得所有需要显示的内容
        /// </summary>
        /// <returns>返回数据集</returns>
        System.Data.DataTable GetMarquee();
    }
}

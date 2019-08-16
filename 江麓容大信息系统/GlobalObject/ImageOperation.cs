using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace GlobalObject
{
    public enum JoinMode
    {
        Horizontal,
        Vertical
    }

    public static class ImageOperation
    {
        public static Image JoinImage(List<Image> imageList, JoinMode jm)
        {
            //图片列表
            if (imageList.Count <= 0)
                return null;
            if (jm == JoinMode.Horizontal)
            {
                //横向拼接
                int width = 0;
                //计算总长度
                foreach (Image i in imageList)
                {
                    width += i.Width;
                }
                //高度不变
                int height = imageList.Max(x => x.Height);
                //构造最终的图片白板
                Bitmap tableChartImage = new Bitmap(width, height);
                Graphics graph = Graphics.FromImage(tableChartImage);
                //初始化这个大图
                graph.DrawImage(tableChartImage, width, height);
                //初始化当前宽
                int currentWidth = 0;
                foreach (Image i in imageList)
                {
                    //拼图
                    graph.DrawImage(i, currentWidth, 0);
                    //拼接改图后，当前宽度
                    currentWidth += i.Width;
                }
                return tableChartImage;
            }
            else if (jm == JoinMode.Vertical)
            {
                //纵向拼接
                int height = 0;
                //计算总长度
                foreach (Image i in imageList)
                {
                    height += i.Height;
                }
                //宽度不变
                int width = imageList.Max(x => x.Width);
                //构造最终的图片白板
                Bitmap tableChartImage = new Bitmap(width, height);
                Graphics graph = Graphics.FromImage(tableChartImage);
                //初始化这个大图
                graph.DrawImage(tableChartImage, width, height);
                //初始化当前宽
                int currentHeight = 0;
                foreach (Image i in imageList)
                {
                    //拼图
                    graph.DrawImage(i, 0, currentHeight);
                    //拼接改图后，当前宽度
                    currentHeight += i.Height;
                }
                return tableChartImage;
            }
            else
            {
                return null;
            }
        }
    }
}

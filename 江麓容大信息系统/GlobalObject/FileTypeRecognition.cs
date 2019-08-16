/******************************************************************************
 * 版权所有 (c) 2013-2016, 小康工业集团容大有限责任公司
 *
 * 文件名称:  FileTypeRecognition.cs
 * 作者    :  夏石友    版本: v1.00    日期: 2013/12/06
 * 开发平台:  Visual C# 2005
 * 用于    :  文档管理模块
 *----------------------------------------------------------------------------
 * 描述 : 判断文档类型
 * 其它 :
 *----------------------------------------------------------------------------
 * 公共信息: 无
 *----------------------------------------------------------------------------
 * 历史记录:
 *     1. 日期时间: 2013/12/06 14:17:00 作者: 夏石友 当前版本: V1.00
 *        修改说明: 创建
 ******************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace GlobalObject
{
    /// <summary>
    /// 文件类型识别器
    /// </summary>
    public static class FileTypeRecognition
    {
        /// <summary>
        /// WORD 2007前的文件头
        /// </summary>
        static readonly byte[] WORD_2003_FILE_HEAD = new byte[] { 0xD0, 0xCF, 0x11, 0xE0, 0xA1, 0xB1, 0x1A, 0xE1 };

        /// <summary>
        /// WORD 2007后的文件头
        /// </summary>
        static readonly byte[] WORD_2007_FILE_HEAD = new byte[] { 0x50, 0x4B, 0x03, 0x04, 0x14, 0x00, 0x06, 0x00 };

        /// <summary>
        /// 判断是否是WORD文档
        /// </summary>
        /// <param name="fileName">包含路径的文件名</param>
        /// <returns>是WORD文档返回true</returns>
        public static bool IsWordDocument(string fileName)
        {
            if (!File.Exists(fileName))
            {
                throw new Exception("不存在文件：" + fileName);
            }

            // 打开文件
            FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);

            if (fileStream.Length < 100)
                return false;

            byte[] buffer = new byte[WORD_2003_FILE_HEAD.Length];

            // 读取文件二进制数据
            fileStream.Read(buffer, 0, 8);

            fileStream.Close();

            bool isWord2003Doc = true;
            bool isWord2007Doc = true;

            for (int i = 0; i < buffer.Length; i++)
            {
                if (isWord2003Doc && buffer[i] != WORD_2003_FILE_HEAD[i])
                {
                    isWord2003Doc = false;
                }

                if (isWord2007Doc && buffer[i] != WORD_2007_FILE_HEAD[i])
                {
                    isWord2007Doc = false;
                }
            }

            return isWord2003Doc || isWord2007Doc;
        }
    }
}


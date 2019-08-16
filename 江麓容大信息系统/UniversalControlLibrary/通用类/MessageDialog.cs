using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace UniversalControlLibrary
{
    /// <summary>
    /// 消息对话框
    /// </summary>
    public static class MessageDialog
    {
        /// <summary>
        /// 显示提示信息
        /// </summary>
        /// <param name="msg">要显示的消息</param>
        /// <returns>消息框返回值</returns>
        public static DialogResult ShowPromptMessage(string msg)
        {
            return MessageBox.Show(msg, "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// 显示错误信息
        /// </summary>
        /// <param name="msg">要显示的消息</param>
        /// <returns>消息框返回值</returns>
        public static DialogResult ShowErrorMessage(string msg)
        {
            if (msg.Contains("SqlException"))
            {
                int index = msg.IndexOf("在 System.Data.SqlClient");

                if (index == -1)
                    index = msg.IndexOf("at System.Data.SqlClient");

                if (index != -1)
                {
                    msg = msg.Remove(index);
                    msg = msg.Remove(0, "System.Data.SqlClient.".Length);
                }
            }

            return MessageBox.Show(msg, "错误信息", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// 显示询问信息
        /// </summary>
        /// <param name="msg">要显示的消息</param>
        /// <returns>消息框返回值</returns>
        public static DialogResult ShowEnquiryMessage(string msg)
        {
            return MessageBox.Show(msg, "询问信息", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        }

    }
}

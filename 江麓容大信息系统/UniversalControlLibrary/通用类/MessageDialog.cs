using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace UniversalControlLibrary
{
    /// <summary>
    /// ��Ϣ�Ի���
    /// </summary>
    public static class MessageDialog
    {
        /// <summary>
        /// ��ʾ��ʾ��Ϣ
        /// </summary>
        /// <param name="msg">Ҫ��ʾ����Ϣ</param>
        /// <returns>��Ϣ�򷵻�ֵ</returns>
        public static DialogResult ShowPromptMessage(string msg)
        {
            return MessageBox.Show(msg, "��ʾ��Ϣ", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// ��ʾ������Ϣ
        /// </summary>
        /// <param name="msg">Ҫ��ʾ����Ϣ</param>
        /// <returns>��Ϣ�򷵻�ֵ</returns>
        public static DialogResult ShowErrorMessage(string msg)
        {
            if (msg.Contains("SqlException"))
            {
                int index = msg.IndexOf("�� System.Data.SqlClient");

                if (index == -1)
                    index = msg.IndexOf("at System.Data.SqlClient");

                if (index != -1)
                {
                    msg = msg.Remove(index);
                    msg = msg.Remove(0, "System.Data.SqlClient.".Length);
                }
            }

            return MessageBox.Show(msg, "������Ϣ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// ��ʾѯ����Ϣ
        /// </summary>
        /// <param name="msg">Ҫ��ʾ����Ϣ</param>
        /// <returns>��Ϣ�򷵻�ֵ</returns>
        public static DialogResult ShowEnquiryMessage(string msg)
        {
            return MessageBox.Show(msg, "ѯ����Ϣ", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        }

    }
}

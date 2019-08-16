using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace AutoUpgradeSystem
{
    /// <summary>
    /// ��Ϣ�Ի���
    /// </summary>
    static class MessageDialog
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

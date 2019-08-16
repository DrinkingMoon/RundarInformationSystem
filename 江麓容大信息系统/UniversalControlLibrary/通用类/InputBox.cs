using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UniversalControlLibrary
{
    /// <summary>
    /// 输入对话框
    /// </summary>
    public static class InputBox
    {
        /// <summary>
        /// 输入对话框
        /// </summary>
        /// <param name="caption">标题</param>
        /// <param name="message">提示信息</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>返回输入的信息</returns>
        public static string ShowDialog(string caption, string message, string defaultValue)
        {
            Form InputForm = new Form();

            InputForm.MinimizeBox = false;
            InputForm.MaximizeBox = false;
            InputForm.StartPosition = FormStartPosition.CenterScreen;
            InputForm.Width = 220;
            InputForm.Height = 150;
            InputForm.Text = caption;

            Label lbl = new Label();

            lbl.Text = message;
            lbl.Left = 10;
            lbl.Top = 20;
            lbl.Parent = InputForm;
            lbl.AutoSize = true;

            TextBox tb = new TextBox();

            tb.Left = 30;
            tb.Top = 45;
            tb.Width = 160;
            tb.Parent = InputForm;
            tb.Text = defaultValue;
            tb.SelectAll();

            Button btnOk = new Button();

            btnOk.Left = 30;
            btnOk.Top = 80;
            btnOk.Parent = InputForm;
            btnOk.Text = "确定";
            InputForm.AcceptButton = btnOk; //回车响应
            btnOk.DialogResult = DialogResult.OK;

            Button btnCancal = new Button();

            btnCancal.Left = 120;
            btnCancal.Top = 80;
            btnCancal.Parent = InputForm;
            btnCancal.Text = "取消";
            btnCancal.DialogResult = DialogResult.Cancel;

            try
            {
                if (InputForm.ShowDialog() == DialogResult.OK)
                {
                    return tb.Text;
                }
                else
                {
                    return "";
                }
            }
            finally
            {
                InputForm.Dispose();
            }

        }
    }
}

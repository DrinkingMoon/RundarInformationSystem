using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PlatformManagement;
using ServerModule;
using GlobalObject;

namespace Expression
{
    /// <summary>
    /// Word文档显示界面
    /// </summary>
    public partial class Word文档显示窗体 : Form
    {
        public Word文档显示窗体(string strfilename)
        {
            InitializeComponent();
            String extension = System.IO.Path.GetExtension(strfilename);

            if (extension == ".rtf")
            {
                richTextBox1.LoadFile(strfilename);
            }
            else if (extension == ".txt")
            {
                richTextBox1.LoadFile(strfilename, RichTextBoxStreamType.PlainText);
            }
            else if (extension == ".doc")
            {
                ForWord(strfilename);
            }
        }

        /// <summary>
        /// WORD文档处理
        /// </summary>
        /// <param name="strfilename">文件路径</param>
        void ForWord(string strfilename)
        {

            //建立Word类的实例,缺点:不能正确读取表格,图片等等的显示

            Microsoft.Office.Interop.Word.ApplicationClass app = new Microsoft.Office.Interop.Word.ApplicationClass();

            Microsoft.Office.Interop.Word.Document doc = null;

            object missing = System.Reflection.Missing.Value;
            object FileName = strfilename;
            object readOnly = false;
            object isVisible = true;
            object index = 0;

            try
            {
                doc = app.Documents.Open(ref FileName, ref missing, ref readOnly,
                ref missing, ref missing, ref missing, ref missing, ref missing,
                ref missing, ref missing, ref missing, ref isVisible, ref missing,
                ref missing, ref missing, ref missing);

                doc.ActiveWindow.Selection.WholeStory();
                doc.ActiveWindow.Selection.Copy();
                //从剪切板获取数据
                IDataObject data = Clipboard.GetDataObject();
                this.richTextBox1.Text = data.GetData(DataFormats.Text).ToString();
            }
            finally
            {
                if (app != null)
                {
                    app.Quit(ref missing, ref missing, ref missing);
                    app = null;
                }
            }
        }
    }
}

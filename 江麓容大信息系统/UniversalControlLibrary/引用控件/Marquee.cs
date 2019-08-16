using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace Expression
{
    public partial class Marquee : Label
    {
        ///// <summary>
        ///// 文本内容
        ///// </summary>
        //List<string> m_text = new List<string>();

        /// <summary>
        /// 文本内容
        /// </summary>
        string m_text;

        ///// <summary>
        ///// 显示位置
        ///// </summary>
        //public PointF m_pointF;

        ///// <summary>
        ///// 字体和大小
        ///// </summary>
        //public Font m_font;

        ///// <summary>
        ///// 颜色
        ///// </summary>
        //public Color m_color;

        /// <summary>
        /// 运行跑马灯的标志
        /// </summary>
        public bool Run
        {
            get { return timer1.Enabled; }

            set
            {
                //if (value)
                //{
                //    for (int i = 0; i < m_text.Count; i++)
                //    {
                //        m_text[i] = this.Text;
                //    }
                //}

                //timer1.Enabled = value;

                if (value)
                {
                    m_text = this.Text;
                    //m_color = this.ForeColor;
                    //m_font = this.Font;
                }

                timer1.Enabled = value; 
            }
        }

        public Marquee()
        {
            InitializeComponent();
        }

        public Marquee(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (this.Text.Length > 1)
            {
                this.Text = this.Text.Substring(1, this.Text.Length - 2);
            }
            else
            {
                //for (int i = 0; i < m_text.Count; i++)
                //{
                //    this.Text = m_text[i];
                //}
                this.Text = m_text;
            }
        }
    }
}

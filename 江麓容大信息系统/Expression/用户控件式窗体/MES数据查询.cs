using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PlatformManagement;
using ServerModule;
using GlobalObject;
using UniversalControlLibrary;
using System.Collections;


namespace Expression
{
    public partial class MES数据查询 : Form
    {
        public MES数据查询()
        {
            InitializeComponent();
        }

        private void MES数据查询_Load(object sender, EventArgs e)
        {
            string[] fileNameData = new string[] { "总成编码", "父级编码", "数据名", "数据值", "流程名", 
                "操作步骤", "操作内容", "工位", "记录时间", "版本号", "记录类型" };

            FormConditionFind fcfData = new FormConditionFind(this.数据信息, fileNameData, this.数据信息.Name, this.Name + this.数据信息.Name);

            fcfData.TopLevel = false;
            fcfData.Dock = DockStyle.Fill;
            fcfData.FormBorderStyle = FormBorderStyle.None;
            fcfData.Show();

            fcfData.Parent = this.数据信息;


            string[] fileNameGoods = new string[] { "总成编码", "流程名", "图号型号", "物品名称", "规格", 
                "供应商", "批次号", "耗用数", "工位", "操作时间" };

            FormConditionFind fcfGoods = new FormConditionFind(this.零件信息, fileNameGoods, this.零件信息.Name, this.Name + this.零件信息.Name);

            fcfGoods.TopLevel = false;
            fcfGoods.Dock = DockStyle.Fill;
            fcfGoods.FormBorderStyle = FormBorderStyle.None;
            fcfGoods.Show();

            fcfGoods.Parent = this.零件信息;

            string[] fileNameRepair = new string[] { "流程名", "总成编码", "记录时间", "记录员", "情况说明", "旧箱号"};

            FormConditionFind fcfRepair = new FormConditionFind(this.返修信息, fileNameRepair, this.返修信息.Name, this.Name + this.返修信息.Name);

            fcfRepair.TopLevel = false;
            fcfRepair.Dock = DockStyle.Fill;
            fcfRepair.FormBorderStyle = FormBorderStyle.None;
            fcfRepair.Show();

            fcfRepair.Parent = this.返修信息;
        }
    }
}

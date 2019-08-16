using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ServerModule;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using PlatformManagement;
using UniversalControlLibrary;

namespace Expression
{
    public partial class 装配线系统参数配置 : Form
    {
        /// <summary>
        /// 参数对象
        /// </summary>
        ZPX_ProductionParams _LnqProductionParams = new ZPX_ProductionParams();

        /// <summary>
        /// 配置参数服务
        /// </summary>
        ServerModule.IZPXProductionParams _serviceProductionParams = ServerModule.PMS_ServerFactory.GetServerModule<IZPXProductionParams>();

        public 装配线系统参数配置(int paramsID)
        {
            InitializeComponent();
            _LnqProductionParams = _serviceProductionParams.GetSingleInfo(paramsID);
        }

        void SetInfo()
        {
            txtDataType.Text = _LnqProductionParams.DataType;
            txtRangeOfValues.Text = _LnqProductionParams.RangeOfValues;
            txtRemark.Text = _LnqProductionParams.Remark;
            txtValue1.Text = _LnqProductionParams.Value1;
            txtValue2.Text = _LnqProductionParams.Value2;
            txtValue3.Text = _LnqProductionParams.Value3;
            txtValue4.Text = _LnqProductionParams.Value4;
            txtValue5.Text = _LnqProductionParams.Value5;
            txtValue6.Text = _LnqProductionParams.Value6;

            if (_LnqProductionParams.RangeOfValues.Substring(0, 4) == "选择模式")
            {
                TextBoxShow.FindType findType = GlobalObject.GeneralFunction.StringConvertToEnum<TextBoxShow.FindType>(
                    _LnqProductionParams.RangeOfValues.Substring(_LnqProductionParams.RangeOfValues.IndexOf("【") + 1,
                    _LnqProductionParams.RangeOfValues.IndexOf("】") - _LnqProductionParams.RangeOfValues.IndexOf("【") - 1));

                foreach (Control cl in this.Controls)
                {
                    if (cl is TextBoxShow && cl.Name.Contains("txtValue"))
                    {
                        ((TextBoxShow)cl).ShowResultForm = true;
                        ((TextBoxShow)cl).FindItem = findType;
                    }
                }
            }
        }

        void GetInfo()
        {
            _LnqProductionParams.DataType = txtDataType.Text;
            _LnqProductionParams.RangeOfValues = txtRangeOfValues.Text;
            _LnqProductionParams.Remark = txtRemark.Text;
            _LnqProductionParams.Value1 = txtValue1.Text;
            _LnqProductionParams.Value2 = txtValue2.Text;
            _LnqProductionParams.Value3 = txtValue3.Text;
            _LnqProductionParams.Value4 = txtValue4.Text;
            _LnqProductionParams.Value5 = txtValue5.Text;
            _LnqProductionParams.Value6 = txtValue6.Text;
        }

        private void 装配线系统参数配置_Load(object sender, EventArgs e)
        {
            SetInfo();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                GetInfo();
                _serviceProductionParams.SaveInfo(_LnqProductionParams);
                MessageBox.Show("保存成功");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data;
using GlobalObject;
using System.Reflection;

namespace UniversalControlLibrary
{
    public partial class CustomComboBox : ComboBox
    {
        int _MinYear = 0, _MaxYear = 0;

        public int MaxYear
        {
            get { return _MaxYear; }
            set { _MaxYear = value; }
        }

        public int MinYear
        {
            get { return _MinYear; }
            set { _MinYear = value; }
        }

        public CustomComboBox()
        {
            InitializeComponent();
        }

        public CustomComboBox(IContainer container)
        {
            container.Add(this);
            InitializeComponent();
        }

        public void Init<T>(List<T> obj, string nameWord, string valueWord)
        {
            List<string> lstSource = new List<string>();

            if (obj == null || nameWord == null || nameWord == "")
            {
                return;
            }

            DataTable tempTable = new DataTable();

            tempTable.Columns.Add("Name");
            tempTable.Columns.Add("Value");

            foreach (T item in obj)
            {
                DataRow dr = tempTable.NewRow();
                dr["Name"] = GlobalObject.GeneralFunction.GetItemValue<T>(item, nameWord);

                if (valueWord == null || valueWord == "")
                {
                    dr["Value"] = null;
                }
                else
                {
                    dr["Value"] = GlobalObject.GeneralFunction.GetItemValue<T>(item, nameWord);
                }

                tempTable.Rows.Add(dr);
            }

            this.DataSource = tempTable;
            this.DisplayMember = "Name";
            this.ValueMember = "Value";
            this.SelectedIndex = -1;
        }

        public void Init<T>(DataTable tb, string nameWord, string valueWord)
        {
            List<string> lstSource = new List<string>();

            if (tb == null || nameWord == null || nameWord == "")
            {
                return;
            }

            if (nameWord != null && nameWord != "")
            {
                if (!tb.Columns.Contains(nameWord))
                {
                    return;
                }
            }

            if (valueWord != null && valueWord != "")
            {
                if (!tb.Columns.Contains(valueWord))
                {
                    return;
                }
            }

            DataTable tempTable = new DataTable();

            tempTable.Columns.Add("Name");
            tempTable.Columns.Add("Value");

            foreach (DataRow item in tb.Rows)
            {
                DataRow dr = tempTable.NewRow();
                dr["Name"] = item[nameWord];

                if (valueWord == null || valueWord == "")
                {
                    dr["Value"] = null;
                }
                else
                {
                    dr["Value"] = item[valueWord];
                }

                tempTable.Rows.Add(dr);
            }

            this.DataSource = tempTable;
            this.DisplayMember = "Name";
            this.ValueMember = "Value";
            this.SelectedIndex = -1;
        }

        public void Init<T>(List<T> lstValue)
        {
            List<string> lstSource = new List<string>();

            if (lstValue == null)
            {
                return;
            }

            foreach (T item in lstValue)
            {
                lstSource.Add(item == null ? "" : item.ToString());
            }

            this.DataSource = lstSource;
            this.SelectedIndex = -1;
        }

        public void Init<T>()
        {
            if (!typeof(T).IsEnum)
            {
                return;
            }

            List<string> lstEnum = GlobalObject.GeneralFunction.GetEumnList(typeof(T));

            if (Enum.GetValues(typeof(T)).Length > 0)
            {
                DataTable tempTable = new DataTable();

                tempTable.Columns.Add("Name");
                tempTable.Columns.Add("Value");

                foreach (string item in lstEnum)
                {
                    DataRow dr = tempTable.NewRow();
                    dr["Name"] = item;
                    dr["Value"] = ((int)typeof(T).InvokeMember(item, BindingFlags.GetField, null, null, null)).ToString();  

                    tempTable.Rows.Add(dr);
                }

                this.DataSource = tempTable;
                this.DisplayMember = "Name";
                this.ValueMember = "Value";
                this.SelectedIndex = -1;
            }
            else
            {
                this.DataSource = lstEnum;
            }
        }

        public void Init()
        {
            if (_MinYear == 0 || _MaxYear == 0 || _MaxYear < _MinYear)
            {
                return;
            }

            List<string> lstTemp = new List<string>();

            for (int i = _MinYear; i <= _MaxYear; i++)
            {
                lstTemp.Add(i.ToString());
            }

            this.DataSource = lstTemp;
            this.SelectedIndex = -1;
        }
    }
}

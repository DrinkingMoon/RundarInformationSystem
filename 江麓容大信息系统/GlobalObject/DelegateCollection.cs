
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows.Forms;

namespace GlobalObject
{
    public static class DelegateCollection
    {
        public delegate void ButtonClick(object sender, EventArgs e);

        public delegate int DBAction(string sql, params IDataParameter[] parameters);

        public delegate void SetCursorArrowCallback(Cursor cur);

        public delegate void SetCursorWaitCallback(Form form, byte[] bt);

        public delegate void DataGridViewEditRow(ref DataGridViewRow dgvr);

        public delegate void FormInit(Form form);

        public delegate void NonArgumentHandle();

        public delegate void DataTableHandle(DataTable dtTemp);

        public delegate void ShowDataGirdViewInfo(DataGridView dgv, string keyColunmName);

        public delegate void MessageHandle(string message);

        public delegate void DelegateTask(List<string> listCheck);

        public delegate void DelegateClick(DataRow dr);

        public delegate void DelegateButtonAffrim(TreeView tree);

        public delegate void CloseFormDelegate(DialogResult dialogResult);

        public delegate void SocketConnectEvent(object sender, bool isCompleted);

        public delegate bool CheckSelectedPersonnel(List<PersonnelBasicInfo> listInfo);

        public delegate bool GetDataInfo(CE_FlowOperationType flowOperationType);

        public delegate DialogResult FormDataRodioDelegate(string info);

        public delegate DialogResult FormDataTableCheckDelegate(DataTable info);

        public delegate DialogResult DelegateFormQueryInfo(DataGridViewRow drInfo);

        public delegate DataTable FormDataTableCheckFindDelegate(DateTime startTime, DateTime endTime);

        public delegate DataTable FormDataTableDelegate();

        public delegate Dictionary<string, DataTable> DictionaryDelegate();
    
    }
}

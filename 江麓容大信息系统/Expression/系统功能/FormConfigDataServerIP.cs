using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using UniversalControlLibrary;

namespace Expression
{
    public partial class FormConfigDataServerIP : Form
    {
        public FormConfigDataServerIP()
        {
            InitializeComponent();

            string[] address = GlobalObject.GlobalParameter.DataServerIP.Split(new char[] { '.' });
            string ip = "";

            for (int i = 0; i < address.Length; i++)
            {
                if (address[i].Length < 3)
                {
                    ip += Convert.ToInt32(address[i]).ToString("D3");
                }
                else
                {
                    ip += address[i];
                }
            }

            txtServerIP.Text = ip;
            txtServerIP.KeyDown += new KeyEventHandler(txtServerIP_KeyDown);
            txtServerIP.Enter += new EventHandler(txtServerIP_Enter);            
            txtServerIP.Leave += new EventHandler(txtServerIP_Leave);  
        }

        void txtServerIP_Leave(object sender, EventArgs e)
        {
            // Resets the cursor when we leave the textbox   
            txtServerIP.SelectionStart = 0;

            // Enable the TabStop property so we can cycle through the form controls again   
            foreach (Control c in this.Controls)
                c.TabStop = true;
        }

        // Handle the Enter event   
        void txtServerIP_Enter(object sender, EventArgs e)
        {
            // Resets the cursor when we enter the textbox   
            txtServerIP.SelectionStart = 0;

            // Disable the TabStop property to prevent the form and its controls to catch the Tab key   
            foreach (Control c in this.Controls)
                c.TabStop = false;
        }   

        private void txtServerIP_KeyDown(object sender, KeyEventArgs e)
        {
            // Cycle through the mask fields   
            if (e.KeyCode == Keys.Tab)
            {
                int pos = txtServerIP.SelectionStart;
                int max = (txtServerIP.MaskedTextProvider.Length - txtServerIP.MaskedTextProvider.EditPositionCount);
                int nextField = 0;

                for (int i = 0; i < txtServerIP.MaskedTextProvider.Length; i++)
                {
                    if (!txtServerIP.MaskedTextProvider.IsEditPosition(i) && (pos + max) >= i)
                        nextField = i;
                }

                nextField += 1;

                // We're done, enable the TabStop property again   
                if (pos == nextField)
                    txtServerIP_Leave(this, e);

                txtServerIP.SelectionStart = nextField;
            }
        }

        /// <summary>
        /// Method to validate an IP address
        /// using regular expressions. The pattern
        /// being used will validate an ip address
        /// with the range of 1.0.0.0 to 255.255.255.255
        /// </summary>
        /// <param name="addr">Address to validate</param>
        /// <returns></returns>
        public bool IsValidIP(string addr)
        {
            //create our match pattern
            string pattern = @"\b(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\b";

            //create our Regular Expression object
            Regex check = new Regex(pattern);
            
            //boolean variable to hold the status
            bool valid = false;
            
            //check to make sure an ip address was provided
            if (addr == "")
            {
                //no address provided so return false
                valid = false;
            }
            else
            {
                //address provided so use the IsMatch Method
                //of the Regular Expression object
                valid = check.IsMatch(addr, 0);
            }

            //return the results
            return valid;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (IsValidIP(this.txtServerIP.Text))
            {
                GlobalObject.GlobalParameter.DataServerIP = 
                    GlobalObject.GlobalParameter.ConvertIPAddress(this.txtServerIP.Text.Replace(" ", ""));
                GlobalObject.GlobalParameter.Save();
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                MessageDialog.ShowPromptMessage("不正确的IP地址");
                txtServerIP.Focus();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        } 
    }
}

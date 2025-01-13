using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EmailDeleter
{
    public partial class ConnectForm : Form
    {
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public string IMAPServer { get; set; }
        public bool UseSSL { get; set; }
        public int IMAPPort { get; set; }
        public string LogonUsing { get; set; }

        public ConnectForm()
        {
            InitializeComponent();
            EmailAddress = string.Empty;
            Password = string.Empty;
            IMAPServer = string.Empty;
            UseSSL = true;
            IMAPPort = 0;
            LogonUsing = string.Empty;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            EmailAddress = tbEmailAddress.Text;
            Password = mbPassword.Text;
            IMAPServer = tbIMAPServer.Text;
            UseSSL = cbSSLRequired.Checked;
            IMAPPort = (int)nbIMAPPort.Value;
            LogonUsing = cbLogonUsing.Text;

            Properties.Settings.Default.EmailAddress = tbEmailAddress.Text;
            Properties.Settings.Default.IMAPServer = tbIMAPServer.Text;
            Properties.Settings.Default.UseSSL = cbSSLRequired.Checked;
            Properties.Settings.Default.IMAPPort = (int)nbIMAPPort.Value;
            Properties.Settings.Default.LogonUsing = cbLogonUsing.Text;
            Properties.Settings.Default.Save();

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void ConnectForm_Load(object sender, EventArgs e)
        {
            tbEmailAddress.Text = Properties.Settings.Default.EmailAddress;
            tbIMAPServer.Text = Properties.Settings.Default.IMAPServer;
            cbSSLRequired.Checked = Properties.Settings.Default.UseSSL;
            nbIMAPPort.Value = Properties.Settings.Default.IMAPPort;
            cbLogonUsing.Text = Properties.Settings.Default.LogonUsing;
        }
    }
}

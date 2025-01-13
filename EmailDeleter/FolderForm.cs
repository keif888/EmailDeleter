using MailKit;
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
    public partial class FolderForm : Form
    {
        public FolderForm()
        {
            InitializeComponent();
            folderPath = string.Empty;
            folder = null;
        }

        public string folderPath { get; internal set; }
        public IMailFolder? folder { get; internal set; }

        private Dictionary<string, IMailFolder> Items = new Dictionary<string, IMailFolder>();

        public void AddItem(string folderName, IMailFolder folder)
        {
            if (!Items.ContainsKey(folderName))
            {
                Items.Add(folderName, folder);
                lbFolders.Items.Add(folderName);
            }
        }

        public void ClearItems()
        {
            lbFolders.Items.Clear();
            Items.Clear();
        }

        private void btOK_Click(object sender, EventArgs e)
        {
            folderPath = lbFolders.Text;
            folder = Items[folderPath];
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}

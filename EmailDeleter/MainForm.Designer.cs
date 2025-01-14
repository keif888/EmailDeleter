namespace EmailDeleter
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            menuStrip1 = new MenuStrip();
            connectToolStripMenuItem = new ToolStripMenuItem();
            findToolStripMenuItem = new ToolStripMenuItem();
            folderToolStripMenuItem = new ToolStripMenuItem();
            searchToolStripMenuItem = new ToolStripMenuItem();
            deleteToolStripMenuItem = new ToolStripMenuItem();
            cancelToolStripMenuItem = new ToolStripMenuItem();
            exitToolStripMenuItem = new ToolStripMenuItem();
            statusStrip1 = new StatusStrip();
            tsslStatus = new ToolStripStatusLabel();
            tsRecords = new ToolStripStatusLabel();
            tsProgress = new ToolStripProgressBar();
            lvEmails = new ListView();
            chDate = new ColumnHeader();
            chFrom = new ColumnHeader();
            chTo = new ColumnHeader();
            chSubject = new ColumnHeader();
            menuStrip1.SuspendLayout();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { connectToolStripMenuItem, findToolStripMenuItem, deleteToolStripMenuItem, cancelToolStripMenuItem, exitToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(1084, 24);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // connectToolStripMenuItem
            // 
            connectToolStripMenuItem.Name = "connectToolStripMenuItem";
            connectToolStripMenuItem.Size = new Size(64, 20);
            connectToolStripMenuItem.Text = "&Connect";
            connectToolStripMenuItem.Click += connectToolStripMenuItem_Click;
            // 
            // findToolStripMenuItem
            // 
            findToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { folderToolStripMenuItem, searchToolStripMenuItem });
            findToolStripMenuItem.Enabled = false;
            findToolStripMenuItem.Name = "findToolStripMenuItem";
            findToolStripMenuItem.Size = new Size(42, 20);
            findToolStripMenuItem.Text = "&Find";
            // 
            // folderToolStripMenuItem
            // 
            folderToolStripMenuItem.Name = "folderToolStripMenuItem";
            folderToolStripMenuItem.Size = new Size(109, 22);
            folderToolStripMenuItem.Text = "F&older";
            folderToolStripMenuItem.Click += folderToolStripMenuItem_Click;
            // 
            // searchToolStripMenuItem
            // 
            searchToolStripMenuItem.Enabled = false;
            searchToolStripMenuItem.Name = "searchToolStripMenuItem";
            searchToolStripMenuItem.Size = new Size(109, 22);
            searchToolStripMenuItem.Text = "&Search";
            searchToolStripMenuItem.Click += searchToolStripMenuItem_Click;
            // 
            // deleteToolStripMenuItem
            // 
            deleteToolStripMenuItem.Enabled = false;
            deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            deleteToolStripMenuItem.Size = new Size(52, 20);
            deleteToolStripMenuItem.Text = "&Delete";
            deleteToolStripMenuItem.Click += deleteToolStripMenuItem_Click;
            // 
            // cancelToolStripMenuItem
            // 
            cancelToolStripMenuItem.Enabled = false;
            cancelToolStripMenuItem.Name = "cancelToolStripMenuItem";
            cancelToolStripMenuItem.Size = new Size(55, 20);
            cancelToolStripMenuItem.Text = "C&ancel";
            cancelToolStripMenuItem.Click += cancelToolStripMenuItem_Click;
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.Size = new Size(38, 20);
            exitToolStripMenuItem.Text = "E&xit";
            exitToolStripMenuItem.Click += exitToolStripMenuItem_Click;
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { tsslStatus, tsRecords, tsProgress });
            statusStrip1.Location = new Point(0, 428);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(1084, 22);
            statusStrip1.TabIndex = 1;
            statusStrip1.Text = "statusStrip1";
            // 
            // tsslStatus
            // 
            tsslStatus.Name = "tsslStatus";
            tsslStatus.Size = new Size(79, 17);
            tsslStatus.Text = "Disconnected";
            // 
            // tsRecords
            // 
            tsRecords.Name = "tsRecords";
            tsRecords.Padding = new Padding(20, 0, 0, 0);
            tsRecords.Size = new Size(177, 17);
            tsRecords.Text = "0 Records Listed for Deletion";
            // 
            // tsProgress
            // 
            tsProgress.Name = "tsProgress";
            tsProgress.Padding = new Padding(20, 0, 0, 0);
            tsProgress.Size = new Size(120, 16);
            // 
            // lvEmails
            // 
            lvEmails.Columns.AddRange(new ColumnHeader[] { chDate, chFrom, chTo, chSubject });
            lvEmails.Dock = DockStyle.Fill;
            lvEmails.FullRowSelect = true;
            lvEmails.Location = new Point(0, 24);
            lvEmails.Name = "lvEmails";
            lvEmails.Size = new Size(1084, 404);
            lvEmails.TabIndex = 2;
            lvEmails.UseCompatibleStateImageBehavior = false;
            lvEmails.View = View.Details;
            lvEmails.ColumnClick += lvEmails_ColumnClick;
            lvEmails.KeyDown += lvEmails_KeyDown;
            // 
            // chDate
            // 
            chDate.Text = "Date";
            chDate.Width = 140;
            // 
            // chFrom
            // 
            chFrom.Text = "From";
            chFrom.Width = 240;
            // 
            // chTo
            // 
            chTo.Text = "To";
            chTo.Width = 240;
            // 
            // chSubject
            // 
            chSubject.Text = "Subject";
            chSubject.Width = 480;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1084, 450);
            Controls.Add(lvEmails);
            Controls.Add(statusStrip1);
            Controls.Add(menuStrip1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = menuStrip1;
            Name = "MainForm";
            Text = "Email Deleter";
            FormClosing += MainForm_FormClosing;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip1;
        private ToolStripMenuItem connectToolStripMenuItem;
        private ToolStripMenuItem findToolStripMenuItem;
        private ToolStripMenuItem exitToolStripMenuItem;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel tsslStatus;
        private ToolStripMenuItem deleteToolStripMenuItem;
        private ListView lvEmails;
        private ColumnHeader chDate;
        private ColumnHeader chFrom;
        private ColumnHeader chTo;
        private ColumnHeader chSubject;
        private ToolStripMenuItem folderToolStripMenuItem;
        private ToolStripMenuItem searchToolStripMenuItem;
        private ToolStripProgressBar tsProgress;
        private ToolStripStatusLabel tsRecords;
        private ToolStripMenuItem cancelToolStripMenuItem;
    }
}

namespace EmailDeleter
{
    partial class FolderForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            panel1 = new Panel();
            btCancel = new Button();
            btOK = new Button();
            lbFolders = new ListBox();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(btCancel);
            panel1.Controls.Add(btOK);
            panel1.Dock = DockStyle.Bottom;
            panel1.Location = new Point(0, 410);
            panel1.Name = "panel1";
            panel1.Size = new Size(704, 40);
            panel1.TabIndex = 0;
            // 
            // btCancel
            // 
            btCancel.DialogResult = DialogResult.Cancel;
            btCancel.Location = new Point(617, 6);
            btCancel.Name = "btCancel";
            btCancel.Size = new Size(75, 23);
            btCancel.TabIndex = 1;
            btCancel.Text = "Cancel";
            btCancel.UseVisualStyleBackColor = true;
            // 
            // btOK
            // 
            btOK.DialogResult = DialogResult.OK;
            btOK.Location = new Point(536, 6);
            btOK.Name = "btOK";
            btOK.Size = new Size(75, 23);
            btOK.TabIndex = 0;
            btOK.Text = "OK";
            btOK.UseVisualStyleBackColor = true;
            btOK.Click += btOK_Click;
            // 
            // lbFolders
            // 
            lbFolders.Dock = DockStyle.Fill;
            lbFolders.FormattingEnabled = true;
            lbFolders.ItemHeight = 15;
            lbFolders.Location = new Point(0, 0);
            lbFolders.Name = "lbFolders";
            lbFolders.ScrollAlwaysVisible = true;
            lbFolders.Size = new Size(704, 410);
            lbFolders.TabIndex = 1;
            // 
            // FolderForm
            // 
            AcceptButton = btOK;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btCancel;
            ClientSize = new Size(704, 450);
            Controls.Add(lbFolders);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FolderForm";
            Text = "Folders";
            panel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Button btCancel;
        private Button btOK;
        private ListBox lbFolders;
    }
}
namespace EmailDeleter
{
    partial class ConnectForm
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
            label1 = new Label();
            tbEmailAddress = new TextBox();
            mbPassword = new MaskedTextBox();
            label2 = new Label();
            tbIMAPServer = new TextBox();
            label3 = new Label();
            nbIMAPPort = new NumericUpDown();
            label4 = new Label();
            cbSSLRequired = new CheckBox();
            cbLogonUsing = new ComboBox();
            label5 = new Label();
            btnOK = new Button();
            btnCancel = new Button();
            ((System.ComponentModel.ISupportInitialize)nbIMAPPort).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(81, 15);
            label1.TabIndex = 0;
            label1.Text = "Email Address";
            // 
            // tbEmailAddress
            // 
            tbEmailAddress.Location = new Point(166, 6);
            tbEmailAddress.Name = "tbEmailAddress";
            tbEmailAddress.Size = new Size(242, 23);
            tbEmailAddress.TabIndex = 1;
            // 
            // mbPassword
            // 
            mbPassword.Location = new Point(166, 35);
            mbPassword.Name = "mbPassword";
            mbPassword.PasswordChar = '*';
            mbPassword.Size = new Size(242, 23);
            mbPassword.TabIndex = 2;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 38);
            label2.Name = "label2";
            label2.Size = new Size(57, 15);
            label2.TabIndex = 3;
            label2.Text = "Password";
            // 
            // tbIMAPServer
            // 
            tbIMAPServer.Location = new Point(166, 64);
            tbIMAPServer.Name = "tbIMAPServer";
            tbIMAPServer.Size = new Size(242, 23);
            tbIMAPServer.TabIndex = 3;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(12, 67);
            label3.Name = "label3";
            label3.Size = new Size(71, 15);
            label3.TabIndex = 5;
            label3.Text = "IMAP Server";
            // 
            // nbIMAPPort
            // 
            nbIMAPPort.Location = new Point(166, 93);
            nbIMAPPort.Maximum = new decimal(new int[] { 60000, 0, 0, 0 });
            nbIMAPPort.Name = "nbIMAPPort";
            nbIMAPPort.Size = new Size(85, 23);
            nbIMAPPort.TabIndex = 5;
            nbIMAPPort.Value = new decimal(new int[] { 993, 0, 0, 0 });
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(12, 95);
            label4.Name = "label4";
            label4.Size = new Size(61, 15);
            label4.TabIndex = 7;
            label4.Text = "IMAP Port";
            // 
            // cbSSLRequired
            // 
            cbSSLRequired.AutoSize = true;
            cbSSLRequired.Checked = true;
            cbSSLRequired.CheckState = CheckState.Checked;
            cbSSLRequired.Location = new Point(414, 66);
            cbSSLRequired.Name = "cbSSLRequired";
            cbSSLRequired.Size = new Size(99, 19);
            cbSSLRequired.TabIndex = 4;
            cbSSLRequired.Text = "SSL Required?";
            cbSSLRequired.UseVisualStyleBackColor = true;
            // 
            // cbLogonUsing
            // 
            cbLogonUsing.FormattingEnabled = true;
            cbLogonUsing.Items.AddRange(new object[] { "Clear text authentication", "Outlook OAUTH", "Google OAUTH" });
            cbLogonUsing.Location = new Point(166, 122);
            cbLogonUsing.Name = "cbLogonUsing";
            cbLogonUsing.Size = new Size(242, 23);
            cbLogonUsing.TabIndex = 6;
            cbLogonUsing.Text = "Clear text authentication";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(12, 125);
            label5.Name = "label5";
            label5.Size = new Size(76, 15);
            label5.TabIndex = 10;
            label5.Text = "Log on using";
            // 
            // btnOK
            // 
            btnOK.Location = new Point(354, 151);
            btnOK.Name = "btnOK";
            btnOK.Size = new Size(75, 23);
            btnOK.TabIndex = 7;
            btnOK.Text = "OK";
            btnOK.UseVisualStyleBackColor = true;
            btnOK.Click += btnOK_Click;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(435, 151);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(75, 23);
            btnCancel.TabIndex = 8;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            // 
            // ConnectForm
            // 
            AcceptButton = btnOK;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancel;
            ClientSize = new Size(522, 181);
            Controls.Add(btnCancel);
            Controls.Add(btnOK);
            Controls.Add(label5);
            Controls.Add(cbLogonUsing);
            Controls.Add(cbSSLRequired);
            Controls.Add(label4);
            Controls.Add(nbIMAPPort);
            Controls.Add(label3);
            Controls.Add(tbIMAPServer);
            Controls.Add(label2);
            Controls.Add(mbPassword);
            Controls.Add(tbEmailAddress);
            Controls.Add(label1);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "ConnectForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Connection";
            Load += ConnectForm_Load;
            ((System.ComponentModel.ISupportInitialize)nbIMAPPort).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox tbEmailAddress;
        private MaskedTextBox mbPassword;
        private Label label2;
        private TextBox tbIMAPServer;
        private Label label3;
        private NumericUpDown nbIMAPPort;
        private Label label4;
        private CheckBox cbSSLRequired;
        private ComboBox cbLogonUsing;
        private Label label5;
        private Button btnOK;
        private Button btnCancel;
    }
}
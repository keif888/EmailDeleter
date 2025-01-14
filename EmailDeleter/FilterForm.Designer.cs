namespace EmailDeleter
{
    partial class FilterForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FilterForm));
            dtFromDate = new DateTimePicker();
            dtToDate = new DateTimePicker();
            tbFromContains = new TextBox();
            tbToContains = new TextBox();
            tbSubjectContains = new TextBox();
            tbBodyContains = new TextBox();
            btOK = new Button();
            btCancel = new Button();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            label6 = new Label();
            btClear = new Button();
            SuspendLayout();
            // 
            // dtFromDate
            // 
            dtFromDate.CustomFormat = "yyyy-MM-dd  hh:mm:ss tt";
            dtFromDate.Format = DateTimePickerFormat.Custom;
            dtFromDate.Location = new Point(138, 12);
            dtFromDate.Name = "dtFromDate";
            dtFromDate.Size = new Size(294, 23);
            dtFromDate.TabIndex = 0;
            dtFromDate.Value = new DateTime(1753, 1, 1, 0, 0, 0, 0);
            // 
            // dtToDate
            // 
            dtToDate.CustomFormat = "yyyy-MM-dd  hh:mm:ss tt";
            dtToDate.Format = DateTimePickerFormat.Custom;
            dtToDate.Location = new Point(138, 41);
            dtToDate.Name = "dtToDate";
            dtToDate.Size = new Size(294, 23);
            dtToDate.TabIndex = 1;
            dtToDate.Value = new DateTime(9998, 12, 31, 0, 0, 0, 0);
            // 
            // tbFromContains
            // 
            tbFromContains.Location = new Point(138, 70);
            tbFromContains.Name = "tbFromContains";
            tbFromContains.Size = new Size(294, 23);
            tbFromContains.TabIndex = 2;
            // 
            // tbToContains
            // 
            tbToContains.Location = new Point(138, 99);
            tbToContains.Name = "tbToContains";
            tbToContains.Size = new Size(294, 23);
            tbToContains.TabIndex = 3;
            // 
            // tbSubjectContains
            // 
            tbSubjectContains.Location = new Point(138, 128);
            tbSubjectContains.Name = "tbSubjectContains";
            tbSubjectContains.Size = new Size(294, 23);
            tbSubjectContains.TabIndex = 4;
            // 
            // tbBodyContains
            // 
            tbBodyContains.Location = new Point(138, 157);
            tbBodyContains.Name = "tbBodyContains";
            tbBodyContains.Size = new Size(294, 23);
            tbBodyContains.TabIndex = 5;
            // 
            // btOK
            // 
            btOK.Location = new Point(276, 186);
            btOK.Name = "btOK";
            btOK.Size = new Size(75, 23);
            btOK.TabIndex = 6;
            btOK.Text = "OK";
            btOK.UseVisualStyleBackColor = true;
            btOK.Click += btOK_Click;
            // 
            // btCancel
            // 
            btCancel.Location = new Point(357, 186);
            btCancel.Name = "btCancel";
            btCancel.Size = new Size(75, 23);
            btCancel.TabIndex = 7;
            btCancel.Text = "Cancel";
            btCancel.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 18);
            label1.Name = "label1";
            label1.Size = new Size(62, 15);
            label1.TabIndex = 8;
            label1.Text = "From Date";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 47);
            label2.Name = "label2";
            label2.Size = new Size(46, 15);
            label2.TabIndex = 9;
            label2.Text = "To Date";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(12, 73);
            label3.Name = "label3";
            label3.Size = new Size(85, 15);
            label3.TabIndex = 10;
            label3.Text = "From Contains";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(12, 102);
            label4.Name = "label4";
            label4.Size = new Size(69, 15);
            label4.TabIndex = 11;
            label4.Text = "To Contains";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(12, 131);
            label5.Name = "label5";
            label5.Size = new Size(96, 15);
            label5.TabIndex = 12;
            label5.Text = "Subject Contains";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(12, 160);
            label6.Name = "label6";
            label6.Size = new Size(84, 15);
            label6.TabIndex = 13;
            label6.Text = "Body Contains";
            // 
            // btClear
            // 
            btClear.Location = new Point(195, 186);
            btClear.Name = "btClear";
            btClear.Size = new Size(75, 23);
            btClear.TabIndex = 14;
            btClear.Text = "Clear";
            btClear.UseVisualStyleBackColor = true;
            btClear.Click += btClear_Click;
            // 
            // FilterForm
            // 
            AcceptButton = btOK;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btCancel;
            ClientSize = new Size(447, 219);
            Controls.Add(btClear);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(btCancel);
            Controls.Add(btOK);
            Controls.Add(tbBodyContains);
            Controls.Add(tbSubjectContains);
            Controls.Add(tbToContains);
            Controls.Add(tbFromContains);
            Controls.Add(dtToDate);
            Controls.Add(dtFromDate);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FilterForm";
            Text = "Filter";
            Load += FilterForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DateTimePicker dtFromDate;
        private DateTimePicker dtToDate;
        private TextBox tbFromContains;
        private TextBox tbToContains;
        private TextBox tbSubjectContains;
        private TextBox tbBodyContains;
        private Button btOK;
        private Button btCancel;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Button btClear;
    }
}
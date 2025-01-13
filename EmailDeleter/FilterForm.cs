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
    public partial class FilterForm : Form
    {
        public string BodyContains { get; private set; }
        public string FromContains { get; private set; }
        public string ToContains { get; private set; }
        public string SubjectContains { get; private set; }
        public DateTime FromDate { get; private set; }
        public DateTime ToDate { get; private set; }
        public bool FromDateIsNull { get; private set; }
        public bool ToDateIsNull { get; private set; }

        public FilterForm()
        {
            InitializeComponent();
            BodyContains = tbBodyContains.Text;
            FromContains = tbFromContains.Text;
            ToContains = tbToContains.Text;
            SubjectContains = tbSubjectContains.Text;
            FromDate = dtFromDate.Value;
            ToDate = dtToDate.Value;
            FromDateIsNull = true;
            ToDateIsNull = true;
        }

        private void btClear_Click(object sender, EventArgs e)
        {
            tbBodyContains.Text = string.Empty;
            tbFromContains.Text = string.Empty;
            tbToContains.Text = string.Empty;
            tbSubjectContains.Text = string.Empty;
            dtFromDate.Value = dtFromDate.MinDate;
            dtToDate.Value = dtToDate.MaxDate;
        }

        private void FilterForm_Load(object sender, EventArgs e)
        {
            tbBodyContains.Text = Properties.Settings.Default.BodyContains;
            tbFromContains.Text = Properties.Settings.Default.FromContains;
            tbToContains.Text = Properties.Settings.Default.ToContains;
            tbSubjectContains.Text = Properties.Settings.Default.SubjectContains;
            dtFromDate.Value = Properties.Settings.Default.FromDate;
            dtToDate.Value = Properties.Settings.Default.ToDate;
        }

        private void btOK_Click(object sender, EventArgs e)
        {
            BodyContains = tbBodyContains.Text;
            FromContains = tbFromContains.Text;
            ToContains = tbToContains.Text;
            SubjectContains = tbSubjectContains.Text;
            FromDate = dtFromDate.Value;
            ToDate = dtToDate.Value;
            FromDateIsNull = dtFromDate.Value == dtFromDate.MinDate;
            ToDateIsNull = dtToDate.Value == dtToDate.MaxDate;

            Properties.Settings.Default.BodyContains = tbBodyContains.Text;
            Properties.Settings.Default.FromContains = tbFromContains.Text;
            Properties.Settings.Default.ToContains = tbToContains.Text;
            Properties.Settings.Default.SubjectContains = tbSubjectContains.Text;
            Properties.Settings.Default.FromDate = dtFromDate.Value;
            Properties.Settings.Default.ToDate = dtToDate.Value;
            Properties.Settings.Default.Save();

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}

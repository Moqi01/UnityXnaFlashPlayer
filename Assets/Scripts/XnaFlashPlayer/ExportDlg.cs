using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace XnaFlashPlayer
{
    public partial class ExportDlg : Form
    {
        public int SkipFrames { get; private set; }
        public int ExportFrames { get; private set; }
        public bool Transparent { get; private set; }

        public ExportDlg()
        {
            InitializeComponent();
        }

        private void Ok_Click(object sender, EventArgs e)
        {
            SkipFrames = (int)skipFrames.Value;
            ExportFrames = (int)exportFrames.Value;
            Transparent = transparent.Checked;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void Storno_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}

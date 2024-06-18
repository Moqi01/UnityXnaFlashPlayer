using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace XnaFlashPlayer
{
    public partial class MainForm : Form
    {
        private FormWindowState puvodniStav = FormWindowState.Normal;
        private Rectangle puvodniOblast;

        public MainForm()
        {
            InitializeComponent();
            //this.Icon = Properties.Resources.Player_Play;
            this.puvodniOblast = Bounds;
            //this.středníKvalitaToolStripMenuItem.PerformClick();
            //this.opakovatToolStripMenuItem.PerformClick();
            this.přehráváníToolStripMenuItem.Enabled = false;
            this.exportToolStripMenuItem.Enabled = false;
        }

        internal void HideMainMenu()
        {
            this.mainMenu.Hide();
        }

        internal void ShowMainMenu()
        {
            this.mainMenu.Show();
            mainMenu.Visible = true;
        }

        internal void Play()
        {
            flashPlayer.Play();
        }

        internal void Up()
        {
            openSwf.fileRenderer.Previous();
        }

        internal void Down()
        {
            openSwf.fileRenderer.Down();
        }

        internal void R()
        {
            openSwf.ButtonBack();
        }

        internal void L()
        {
            openSwf.ButtonUp();
        }

        public void OpenFile()
        {
            openSwf.OpenFile();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            int msaa = flashPlayer.GraphicsDevice.PresentationParameters.MultiSampleCount;
            středníKvalitaToolStripMenuItem.Enabled = msaa >= 4;
            vysokáKvalitaToolStripMenuItem.Enabled = msaa >= 8;
            nejvyššíKvalitaToolStripMenuItem.Enabled = msaa >= 16;
        }
        private void NastavCelouObrazovku(bool celaObrazovka)
        {
            if (celaObrazovka)
            {
                if (this.FormBorderStyle != System.Windows.Forms.FormBorderStyle.None)
                {
                    this.puvodniOblast = Bounds;
                    this.puvodniStav = this.WindowState;
                    this.TopMost = true;
                    this.mainMenu.Visible = false;
                    this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                    this.WindowState = FormWindowState.Maximized;
                }
            }
            else if (this.FormBorderStyle == System.Windows.Forms.FormBorderStyle.None)
            {
                this.TopMost = false;
                this.mainMenu.Visible = true;
                this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
                this.WindowState = this.puvodniStav;
                this.Bounds = this.puvodniOblast;
            }
        }

        public void ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openSwf.ShowDialog(Open) != System.Windows.Forms.DialogResult.OK)
                return;
        }

        public string FileName => openSwf?.FileName;

        void Open(Form form, DialogResult dialogResult)
        {
            if(dialogResult== DialogResult.OK)
            {
                exportToolStripMenuItem.Enabled =
                přehráváníToolStripMenuItem.Enabled = flashPlayer.Open(openSwf.FileName);
                DrawGL.ins.SetFileText( openSwf.FileName);
                StartScript.ins.RefreshLocalScale();
            }
        }

        private void zavřítToolStripMenuItem_Click(object sender, EventArgs e)
        {
            flashPlayer.Close();
            exportToolStripMenuItem.Enabled =
            přehráváníToolStripMenuItem.Enabled = false;
        }
        private void uložitSnímekObrazovkyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveJpeg.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                return;

            flashPlayer.Snapshot(saveJpeg.FileName);
        }
        private void konecToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void celáObrazovkaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            celáObrazovkaToolStripMenuItem.Checked = !celáObrazovkaToolStripMenuItem.Checked;
            NastavCelouObrazovku(celáObrazovkaToolStripMenuItem.Checked);
        }
        private void nízkáKvalitaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            flashPlayer.SetQuality(0);
            nízkáKvalitaToolStripMenuItem.Checked = true;
            středníKvalitaToolStripMenuItem.Checked = false;
            vysokáKvalitaToolStripMenuItem.Checked = false;
            nejvyššíKvalitaToolStripMenuItem.Checked = false;
        }
        private void středníKvalitaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            flashPlayer.SetQuality(1);
            nízkáKvalitaToolStripMenuItem.Checked = false;
            středníKvalitaToolStripMenuItem.Checked = true;
            vysokáKvalitaToolStripMenuItem.Checked = false;
            nejvyššíKvalitaToolStripMenuItem.Checked = false;
        }
        private void vysokáKvalitaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            flashPlayer.SetQuality(2);
            nízkáKvalitaToolStripMenuItem.Checked = false;
            středníKvalitaToolStripMenuItem.Checked = false;
            vysokáKvalitaToolStripMenuItem.Checked = true;
            nejvyššíKvalitaToolStripMenuItem.Checked = false;
        }
        private void nejvyššíKvalitaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            flashPlayer.SetQuality(3);
            nízkáKvalitaToolStripMenuItem.Checked = false;
            středníKvalitaToolStripMenuItem.Checked = false;
            vysokáKvalitaToolStripMenuItem.Checked = false;
            nejvyššíKvalitaToolStripMenuItem.Checked = true;
        }

        private void pozastavitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pozastavitToolStripMenuItem.Checked = !pozastavitToolStripMenuItem.Checked;
            flashPlayer.Pause(pozastavitToolStripMenuItem.Checked);
        }
        private void návratToolStripMenuItem_Click(object sender, EventArgs e)
        {
            flashPlayer.Rewind();
        }
        public void předchozíSnímekToolStripMenuItem_Click(object sender, EventArgs e)
        {
            flashPlayer.PrevFrame();
        }
        public void dalšíSnímekToolStripMenuItem_Click(object sender, EventArgs e)
        {
            flashPlayer.NextFrame();
        }
        private void opakovatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            opakovatToolStripMenuItem.Checked = !opakovatToolStripMenuItem.Checked;
            flashPlayer.SetLooping(opakovatToolStripMenuItem.Checked);
        }

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var dlg = new ExportDlg())
            {
                if (dlg.ShowDialog() != DialogResult.OK)
                    return;

                if (folderBrowserDialog.ShowDialog() != DialogResult.OK)
                    return;

                mainMenu.Enabled = false;
                try
                {
                    flashPlayer.ExportAnimation(dlg.SkipFrames, dlg.ExportFrames, dlg.Transparent, folderBrowserDialog.SelectedPath);
                    MessageBox.Show("Export dokončen!", "Požadované snímky byly exportovány!");//, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Export selhal!", ex.Message);//, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    mainMenu.Enabled = true;
                }
            }
        }
    }
}

namespace XnaFlashPlayer
{
    partial class MainForm
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
            this.mainMenu = new System.Windows.Forms.MenuStrip();
            this.souborToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.zavřítToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.uložitSnímekObrazovkyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.konecToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pohledToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.celáObrazovkaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.nízkáKvalitaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.středníKvalitaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.vysokáKvalitaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nejvyššíKvalitaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.přehráváníToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pozastavitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.návratToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.předchozíSnímekToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dalšíSnímekToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.opakovatToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.flashPlayer = new XnaFlashPlayer.FlashPlayerControl();
            this.openSwf = new System.Windows.Forms.OpenFileDialog();
            this.saveJpeg = new System.Windows.Forms.SaveFileDialog();
            //this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.folderBrowserDialog = new System.Windows.Forms.OpenFileDialog();
            this.mainMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainMenu
            // 
            this.mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.souborToolStripMenuItem,
            this.pohledToolStripMenuItem,
            this.přehráváníToolStripMenuItem});
            this.mainMenu.Location = new System.Drawing.Point(0, 0);
            this.mainMenu.Name = "mainMenu";
            this.mainMenu.Size = new System.Drawing.Size(684, 24);
            this.mainMenu.TabIndex = 0;
            this.mainMenu.Text = "menuStrip1";
            // 
            // souborToolStripMenuItem
            // 
            this.souborToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItem,
            this.zavřítToolStripMenuItem,
            this.toolStripSeparator1,
            this.uložitSnímekObrazovkyToolStripMenuItem,
            this.toolStripSeparator2,
            this.konecToolStripMenuItem,
            this.exportToolStripMenuItem});
            this.souborToolStripMenuItem.Name = "souborToolStripMenuItem";
            this.souborToolStripMenuItem.Size = new System.Drawing.Size(57, 20);
            this.souborToolStripMenuItem.Text = "S";
            // 
            // ToolStripMenuItem
            // 
            this.ToolStripMenuItem.Name = "ToolStripMenuItem";
            this.ToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.ToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.ToolStripMenuItem.Text = "O";
            this.ToolStripMenuItem.Click += new System.EventHandler(this.ToolStripMenuItem_Click);
            // 
            // zavřítToolStripMenuItem
            // 
            this.zavřítToolStripMenuItem.Name = "zavřítToolStripMenuItem";
            this.zavřítToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.W)));
            this.zavřítToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.zavřítToolStripMenuItem.Text = "Z";
            this.zavřítToolStripMenuItem.Click += new System.EventHandler(this.zavřítToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(182, 6);
            // 
            // uložitSnímekObrazovkyToolStripMenuItem
            // 
            this.uložitSnímekObrazovkyToolStripMenuItem.Name = "uložitSnímekObrazovkyToolStripMenuItem";
            this.uložitSnímekObrazovkyToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.uložitSnímekObrazovkyToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.uložitSnímekObrazovkyToolStripMenuItem.Text = "&Uložit snímek";
            this.uložitSnímekObrazovkyToolStripMenuItem.Click += new System.EventHandler(this.uložitSnímekObrazovkyToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(182, 6);
            // 
            // konecToolStripMenuItem
            // 
            this.konecToolStripMenuItem.Name = "konecToolStripMenuItem";
            this.konecToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.konecToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.konecToolStripMenuItem.Text = "K";
            this.konecToolStripMenuItem.Click += new System.EventHandler(this.konecToolStripMenuItem_Click);
            // 
            // exportToolStripMenuItem
            // 
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.exportToolStripMenuItem.ShortcutKeyDisplayString = "";
            this.exportToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.E)));
            this.exportToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.exportToolStripMenuItem.Text = "Export";
            this.exportToolStripMenuItem.Click += new System.EventHandler(this.exportToolStripMenuItem_Click);
            // 
            // pohledToolStripMenuItem
            // 
            this.pohledToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.celáObrazovkaToolStripMenuItem,
            this.toolStripSeparator3,
            this.nízkáKvalitaToolStripMenuItem,
            this.středníKvalitaToolStripMenuItem,
            this.vysokáKvalitaToolStripMenuItem,
            this.nejvyššíKvalitaToolStripMenuItem});
            this.pohledToolStripMenuItem.Name = "pohledToolStripMenuItem";
            this.pohledToolStripMenuItem.Size = new System.Drawing.Size(56, 20);
            this.pohledToolStripMenuItem.Text = "P";
            // 
            // celáObrazovkaToolStripMenuItem
            // 
            this.celáObrazovkaToolStripMenuItem.Name = "celáObrazovkaToolStripMenuItem";
            this.celáObrazovkaToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
            this.celáObrazovkaToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            this.celáObrazovkaToolStripMenuItem.Text = "C";
            this.celáObrazovkaToolStripMenuItem.Click += new System.EventHandler(this.celáObrazovkaToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(197, 6);
            // 
            // nízkáKvalitaToolStripMenuItem
            // 
            this.nízkáKvalitaToolStripMenuItem.Name = "nízkáKvalitaToolStripMenuItem";
            this.nízkáKvalitaToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F1)));
            this.nízkáKvalitaToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            this.nízkáKvalitaToolStripMenuItem.Text = "N";
            this.nízkáKvalitaToolStripMenuItem.Click += new System.EventHandler(this.nízkáKvalitaToolStripMenuItem_Click);
            // 
            // středníKvalitaToolStripMenuItem
            // 
            this.středníKvalitaToolStripMenuItem.Name = "středníKvalitaToolStripMenuItem";
            this.středníKvalitaToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F2)));
            this.středníKvalitaToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            this.středníKvalitaToolStripMenuItem.Text = "&Střední kvalita";
            this.středníKvalitaToolStripMenuItem.Click += new System.EventHandler(this.středníKvalitaToolStripMenuItem_Click);
            // 
            // vysokáKvalitaToolStripMenuItem
            // 
            this.vysokáKvalitaToolStripMenuItem.Name = "vysokáKvalitaToolStripMenuItem";
            this.vysokáKvalitaToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F3)));
            this.vysokáKvalitaToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            this.vysokáKvalitaToolStripMenuItem.Text = "V";
            this.vysokáKvalitaToolStripMenuItem.Click += new System.EventHandler(this.vysokáKvalitaToolStripMenuItem_Click);
            // 
            // nejvyššíKvalitaToolStripMenuItem
            // 
            this.nejvyššíKvalitaToolStripMenuItem.Name = "nejvyššíKvalitaToolStripMenuItem";
            this.nejvyššíKvalitaToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F4)));
            this.nejvyššíKvalitaToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            this.nejvyššíKvalitaToolStripMenuItem.Text = "N";
            this.nejvyššíKvalitaToolStripMenuItem.Click += new System.EventHandler(this.nejvyššíKvalitaToolStripMenuItem_Click);
            // 
            // přehráváníToolStripMenuItem
            // 
            this.přehráváníToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pozastavitToolStripMenuItem,
            this.návratToolStripMenuItem,
            this.toolStripSeparator4,
            this.předchozíSnímekToolStripMenuItem,
            this.dalšíSnímekToolStripMenuItem,
            this.toolStripSeparator5,
            this.opakovatToolStripMenuItem});
            this.přehráváníToolStripMenuItem.Name = "přehráváníToolStripMenuItem";
            this.přehráváníToolStripMenuItem.Size = new System.Drawing.Size(75, 20);
            this.přehráváníToolStripMenuItem.Text = "P";
            // 
            // pozastavitToolStripMenuItem
            // 
            this.pozastavitToolStripMenuItem.Name = "pozastavitToolStripMenuItem";
            this.pozastavitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Return)));
            this.pozastavitToolStripMenuItem.Size = new System.Drawing.Size(221, 22);
            this.pozastavitToolStripMenuItem.Text = "Pozastavit";
            this.pozastavitToolStripMenuItem.Click += new System.EventHandler(this.pozastavitToolStripMenuItem_Click);
            // 
            // návratToolStripMenuItem
            // 
            this.návratToolStripMenuItem.Name = "návratToolStripMenuItem";
            this.návratToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
            this.návratToolStripMenuItem.Size = new System.Drawing.Size(221, 22);
            this.návratToolStripMenuItem.Text = "N";
            this.návratToolStripMenuItem.Click += new System.EventHandler(this.návratToolStripMenuItem_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(218, 6);
            // 
            // předchozíSnímekToolStripMenuItem
            // 
            this.předchozíSnímekToolStripMenuItem.Name = "předchozíSnímekToolStripMenuItem";
            this.předchozíSnímekToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Left)));
            this.předchozíSnímekToolStripMenuItem.Size = new System.Drawing.Size(221, 22);
            this.předchozíSnímekToolStripMenuItem.Text = "P";
            this.předchozíSnímekToolStripMenuItem.Click += new System.EventHandler(this.předchozíSnímekToolStripMenuItem_Click);
            // 
            // dalšíSnímekToolStripMenuItem
            // 
            this.dalšíSnímekToolStripMenuItem.Name = "dalšíSnímekToolStripMenuItem";
            this.dalšíSnímekToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Right)));
            this.dalšíSnímekToolStripMenuItem.Size = new System.Drawing.Size(221, 22);
            this.dalšíSnímekToolStripMenuItem.Text = "D";
            this.dalšíSnímekToolStripMenuItem.Click += new System.EventHandler(this.dalšíSnímekToolStripMenuItem_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(218, 6);
            // 
            // opakovatToolStripMenuItem
            // 
            this.opakovatToolStripMenuItem.Name = "opakovatToolStripMenuItem";
            this.opakovatToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.L)));
            this.opakovatToolStripMenuItem.Size = new System.Drawing.Size(221, 22);
            this.opakovatToolStripMenuItem.Text = "O";
            this.opakovatToolStripMenuItem.Click += new System.EventHandler(this.opakovatToolStripMenuItem_Click);

            
            // 
            // flashPlayer
            // 
            //this.flashPlayer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flashPlayer.Location = new System.Drawing.Point(0, 24);
            this.flashPlayer.Name = "flashPlayer";
            this.flashPlayer.Size = new System.Drawing.Size(684, 438);
            this.flashPlayer.TabIndex = 1;
            this.flashPlayer.Text = "flashPlayerControl1";
            //this.flashPlayer.VectorDevice = null;
            // 
            // openSwf
            // 
            this.openSwf.DefaultExt = "swf";
            this.openSwf.Filter = "Klipy SWF|*.swf";
            this.openSwf.Title = "Otevřít klip";
            // 
            // saveJpeg
            // 
            this.saveJpeg.DefaultExt = "jpg";
            this.saveJpeg.Filter = "Obrázek JPEG|*.jpg";
            this.saveJpeg.Title = "Uložit snímek";
            // 
            // folderBrowserDialog
            // 
            this.folderBrowserDialog.Description = "Zvolte adresář pro export";

            // 
            // MainForm
            // 
            //this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            //this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            //this.ClientSize = new System.Drawing.Size(684, 462);

            
            this.Location = new System.Drawing.Point(-100, -20);
            this.ClientSize = new System.Drawing.Size(24, 24);

            this.Controls.Add(this.flashPlayer);
            this.Controls.Add(this.mainMenu);
            this.MainMenuStrip = this.mainMenu;
            this.Name = "MainForm";
            this.Text = "XnaFlashPlayer";
            this.mainMenu.ResumeLayout(false);
            this.mainMenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip mainMenu;
        private System.Windows.Forms.ToolStripMenuItem souborToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem zavřítToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem uložitSnímekObrazovkyToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem konecToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pohledToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem celáObrazovkaToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem nízkáKvalitaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem středníKvalitaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem vysokáKvalitaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem nejvyššíKvalitaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem přehráváníToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pozastavitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem návratToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem dalšíSnímekToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem předchozíSnímekToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem opakovatToolStripMenuItem;
        private FlashPlayerControl flashPlayer;
        private System.Windows.Forms.OpenFileDialog openSwf;
        private System.Windows.Forms.SaveFileDialog saveJpeg;
        private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
        //private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.FileDialog folderBrowserDialog;
    }
}


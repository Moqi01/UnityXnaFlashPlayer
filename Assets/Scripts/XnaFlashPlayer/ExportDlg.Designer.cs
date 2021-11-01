namespace XnaFlashPlayer
{
    partial class ExportDlg
    {
        ///// <summary>
        ///// Required designer variable.
        ///// </summary>
        //private System.ComponentModel.IContainer components = null;

        ///// <summary>
        ///// Clean up any resources being used.
        ///// </summary>
        ///// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing && (components != null))
        //    {
        //        components.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}

        //#region Windows Form Designer generated code

        ///// <summary>
        ///// Required method for Designer support - do not modify
        ///// the contents of this method with the code editor.
        ///// </summary>
        //private void InitializeComponent()
        //{
        //    this.ok = new System.Windows.Forms.Button();
        //    this.storno = new System.Windows.Forms.Button();
        //    this.exportFrames = new System.Windows.Forms.NumericUpDown();
        //    this.skipFrames = new System.Windows.Forms.NumericUpDown();
        //    this.label1 = new System.Windows.Forms.Label();
        //    this.label2 = new System.Windows.Forms.Label();
        //    this.transparent = new System.Windows.Forms.CheckBox();
        //    this.label3 = new System.Windows.Forms.Label();
        //    ((System.ComponentModel.ISupportInitialize)(this.exportFrames)).BeginInit();
        //    ((System.ComponentModel.ISupportInitialize)(this.skipFrames)).BeginInit();
        //    this.SuspendLayout();
        //    // 
        //    // ok
        //    // 
        //    this.ok.Location = new System.Drawing.Point(122, 81);
        //    this.ok.Name = "ok";
        //    this.ok.Size = new System.Drawing.Size(75, 23);
        //    this.ok.TabIndex = 0;
        //    this.ok.Text = "OK";
        //    this.ok.UseVisualStyleBackColor = true;
        //    this.ok.Click += new System.EventHandler(this.Ok_Click);
        //    // 
        //    // storno
        //    // 
        //    this.storno.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        //    this.storno.Location = new System.Drawing.Point(203, 81);
        //    this.storno.Name = "storno";
        //    this.storno.Size = new System.Drawing.Size(75, 23);
        //    this.storno.TabIndex = 1;
        //    this.storno.Text = "Storno";
        //    this.storno.UseVisualStyleBackColor = true;
        //    this.storno.Click += new System.EventHandler(this.Storno_Click);
        //    // 
        //    // exportFrames
        //    // 
        //    this.exportFrames.Location = new System.Drawing.Point(122, 32);
        //    this.exportFrames.Maximum = new decimal(new int[] {
        //    999999,
        //    0,
        //    0,
        //    0});
        //    this.exportFrames.Minimum = new decimal(new int[] {
        //    1,
        //    0,
        //    0,
        //    0});
        //    this.exportFrames.Name = "exportFrames";
        //    this.exportFrames.Size = new System.Drawing.Size(156, 20);
        //    this.exportFrames.TabIndex = 2;
        //    this.exportFrames.Value = new decimal(new int[] {
        //    1,
        //    0,
        //    0,
        //    0});
        //    // 
        //    // skipFrames
        //    // 
        //    this.skipFrames.Location = new System.Drawing.Point(122, 7);
        //    this.skipFrames.Maximum = new decimal(new int[] {
        //    999999,
        //    0,
        //    0,
        //    0});
        //    this.skipFrames.Name = "skipFrames";
        //    this.skipFrames.Size = new System.Drawing.Size(156, 20);
        //    this.skipFrames.TabIndex = 3;
        //    // 
        //    // label1
        //    // 
        //    this.label1.AutoSize = true;
        //    this.label1.Location = new System.Drawing.Point(12, 9);
        //    this.label1.Name = "label1";
        //    this.label1.Size = new System.Drawing.Size(93, 13);
        //    this.label1.TabIndex = 4;
        //    this.label1.Text = "Přeskočit snímků:";
        //    // 
        //    // label2
        //    // 
        //    this.label2.AutoSize = true;
        //    this.label2.Location = new System.Drawing.Point(12, 34);
        //    this.label2.Name = "label2";
        //    this.label2.Size = new System.Drawing.Size(99, 13);
        //    this.label2.TabIndex = 5;
        //    this.label2.Text = "Exportovat snímků:";
        //    // 
        //    // transparentne
        //    // 
        //    this.transparent.AutoSize = true;
        //    this.transparent.Checked = true;
        //    this.transparent.CheckState = System.Windows.Forms.CheckState.Checked;
        //    this.transparent.Location = new System.Drawing.Point(122, 58);
        //    this.transparent.Name = "transparentne";
        //    this.transparent.Size = new System.Drawing.Size(15, 14);
        //    this.transparent.TabIndex = 6;
        //    this.transparent.UseVisualStyleBackColor = true;
        //    // 
        //    // label3
        //    // 
        //    this.label3.AutoSize = true;
        //    this.label3.Location = new System.Drawing.Point(12, 59);
        //    this.label3.Name = "label3";
        //    this.label3.Size = new System.Drawing.Size(94, 13);
        //    this.label3.TabIndex = 7;
        //    this.label3.Text = "Průhledné pozadí:";
        //    // 
        //    // ExportDlg
        //    // 
        //    this.AcceptButton = this.ok;
        //    this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        //    this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        //    this.CancelButton = this.storno;
        //    this.ClientSize = new System.Drawing.Size(291, 113);
        //    this.Controls.Add(this.label3);
        //    this.Controls.Add(this.transparent);
        //    this.Controls.Add(this.label2);
        //    this.Controls.Add(this.label1);
        //    this.Controls.Add(this.skipFrames);
        //    this.Controls.Add(this.exportFrames);
        //    this.Controls.Add(this.storno);
        //    this.Controls.Add(this.ok);
        //    this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
        //    this.MaximizeBox = false;
        //    this.MinimizeBox = false;
        //    this.Name = "ExportDlg";
        //    this.ShowIcon = false;
        //    this.ShowInTaskbar = false;
        //    this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
        //    this.Text = "Export";
        //    ((System.ComponentModel.ISupportInitialize)(this.exportFrames)).EndInit();
        //    ((System.ComponentModel.ISupportInitialize)(this.skipFrames)).EndInit();
        //    this.ResumeLayout(false);
        //    this.PerformLayout();

        //}

        //#endregion

        //private System.Windows.Forms.Button ok;
        //private System.Windows.Forms.Button storno;
        //private System.Windows.Forms.NumericUpDown exportFrames;
        //private System.Windows.Forms.NumericUpDown skipFrames;
        //private System.Windows.Forms.Label label1;
        //private System.Windows.Forms.Label label2;
        //private System.Windows.Forms.CheckBox transparent;
        //private System.Windows.Forms.Label label3;
    }
}
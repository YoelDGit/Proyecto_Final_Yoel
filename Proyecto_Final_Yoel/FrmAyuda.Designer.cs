namespace Proyecto_Final_Yoel
{
    partial class FrmAyuda
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.panelCabecera = new System.Windows.Forms.Panel();
            this.labelTitulo = new System.Windows.Forms.Label();
            this.rtbManual = new System.Windows.Forms.RichTextBox();
            this.panelCabecera.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelCabecera
            // 
            this.panelCabecera.Controls.Add(this.labelTitulo);
            this.panelCabecera.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelCabecera.Location = new System.Drawing.Point(0, 0);
            this.panelCabecera.Name = "panelCabecera";
            this.panelCabecera.Size = new System.Drawing.Size(560, 50);
            this.panelCabecera.TabIndex = 0;
            // 
            // labelTitulo
            // 
            this.labelTitulo.AutoSize = true;
            this.labelTitulo.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this.labelTitulo.Location = new System.Drawing.Point(15, 13);
            this.labelTitulo.Name = "labelTitulo";
            this.labelTitulo.Size = new System.Drawing.Size(67, 25);
            this.labelTitulo.TabIndex = 0;
            this.labelTitulo.Text = "Ayuda";
            // 
            // rtbManual
            // 
            this.rtbManual.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtbManual.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbManual.Location = new System.Drawing.Point(0, 50);
            this.rtbManual.Name = "rtbManual";
            this.rtbManual.ReadOnly = true;
            this.rtbManual.Size = new System.Drawing.Size(557, 348);
            this.rtbManual.TabIndex = 1;
            this.rtbManual.Text = "";
            this.rtbManual.TextChanged += new System.EventHandler(this.rtbManual_TextChanged);
            // 
            // FrmAyuda
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(560, 411);
            this.Controls.Add(this.rtbManual);
            this.Controls.Add(this.panelCabecera);
            this.Name = "FrmAyuda";
            this.Text = "Ayuda";
            this.Load += new System.EventHandler(this.FrmAyuda_Load);
            this.panelCabecera.ResumeLayout(false);
            this.panelCabecera.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelCabecera;
        private System.Windows.Forms.Label labelTitulo;
        private System.Windows.Forms.RichTextBox rtbManual;
    }
}

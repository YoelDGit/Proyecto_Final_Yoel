namespace Proyecto_Final_Yoel
{
    partial class FrmCambiarUsuario
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
            this.flowPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.panelCabecera.SuspendLayout();
            this.SuspendLayout();
            //
            // panelCabecera
            //
            this.panelCabecera.Controls.Add(this.labelTitulo);
            this.panelCabecera.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelCabecera.Location = new System.Drawing.Point(0, 0);
            this.panelCabecera.Name = "panelCabecera";
            this.panelCabecera.Size = new System.Drawing.Size(500, 50);
            this.panelCabecera.TabIndex = 0;
            //
            // labelTitulo
            //
            this.labelTitulo.AutoSize = true;
            this.labelTitulo.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this.labelTitulo.Location = new System.Drawing.Point(15, 13);
            this.labelTitulo.Name = "labelTitulo";
            this.labelTitulo.Size = new System.Drawing.Size(220, 24);
            this.labelTitulo.TabIndex = 0;
            this.labelTitulo.Text = "Cambiar de usuario";
            //
            // flowPanel
            //
            this.flowPanel.AutoScroll = true;
            this.flowPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowPanel.Location = new System.Drawing.Point(0, 50);
            this.flowPanel.Name = "flowPanel";
            this.flowPanel.Padding = new System.Windows.Forms.Padding(10);
            this.flowPanel.Size = new System.Drawing.Size(500, 400);
            this.flowPanel.TabIndex = 1;
            this.flowPanel.WrapContents = false;
            //
            // FrmCambiarUsuario
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(500, 450);
            this.Controls.Add(this.flowPanel);
            this.Controls.Add(this.panelCabecera);
            this.Name = "FrmCambiarUsuario";
            this.Text = "Cambiar de usuario";
            this.Load += new System.EventHandler(this.FrmCambiarUsuario_Load);
            this.panelCabecera.ResumeLayout(false);
            this.panelCabecera.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel panelCabecera;
        private System.Windows.Forms.Label labelTitulo;
        private System.Windows.Forms.FlowLayoutPanel flowPanel;
    }
}

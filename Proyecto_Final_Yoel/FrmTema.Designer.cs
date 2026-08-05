namespace Proyecto_Final_Yoel
{
    partial class FrmTema
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.btnPredeterminado = new System.Windows.Forms.Button();
            this.btnClaro = new System.Windows.Forms.Button();
            this.btnOscuro = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            //
            // panel1
            //
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.btnOscuro);
            this.panel1.Controls.Add(this.btnClaro);
            this.panel1.Controls.Add(this.btnPredeterminado);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(340, 220);
            this.panel1.TabIndex = 0;
            //
            // label1
            //
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(15, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(140, 21);
            this.label1.TabIndex = 0;
            this.label1.Text = "Apariencia";
            //
            // btnPredeterminado
            //
            this.btnPredeterminado.Location = new System.Drawing.Point(15, 55);
            this.btnPredeterminado.Name = "btnPredeterminado";
            this.btnPredeterminado.Size = new System.Drawing.Size(300, 40);
            this.btnPredeterminado.TabIndex = 1;
            this.btnPredeterminado.Text = "Predeterminado";
            this.btnPredeterminado.UseVisualStyleBackColor = true;
            this.btnPredeterminado.Click += new System.EventHandler(this.btnPredeterminado_Click);
            //
            // btnClaro
            //
            this.btnClaro.Location = new System.Drawing.Point(15, 105);
            this.btnClaro.Name = "btnClaro";
            this.btnClaro.Size = new System.Drawing.Size(300, 40);
            this.btnClaro.TabIndex = 2;
            this.btnClaro.Text = "Tema Claro";
            this.btnClaro.UseVisualStyleBackColor = true;
            this.btnClaro.Click += new System.EventHandler(this.btnClaro_Click);
            //
            // btnOscuro
            //
            this.btnOscuro.Location = new System.Drawing.Point(15, 155);
            this.btnOscuro.Name = "btnOscuro";
            this.btnOscuro.Size = new System.Drawing.Size(300, 40);
            this.btnOscuro.TabIndex = 3;
            this.btnOscuro.Text = "Tema Oscuro";
            this.btnOscuro.UseVisualStyleBackColor = true;
            this.btnOscuro.Click += new System.EventHandler(this.btnOscuro_Click);
            //
            // FrmTema
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(340, 220);
            this.Controls.Add(this.panel1);
            this.Name = "FrmTema";
            this.Text = "Apariencia";
            this.Load += new System.EventHandler(this.FrmTema_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnPredeterminado;
        private System.Windows.Forms.Button btnClaro;
        private System.Windows.Forms.Button btnOscuro;
    }
}

namespace Proyecto_Final_Yoel
{
    partial class FrmExportar
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
            this.btnHistorialPdf = new System.Windows.Forms.Button();
            this.labelInfo = new System.Windows.Forms.Label();
            this.btnTransacciones = new System.Windows.Forms.Button();
            this.btnStock = new System.Windows.Forms.Button();
            this.btnClientes = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.btnHistorialPdf);
            this.panel1.Controls.Add(this.labelInfo);
            this.panel1.Controls.Add(this.btnTransacciones);
            this.panel1.Controls.Add(this.btnStock);
            this.panel1.Controls.Add(this.btnClientes);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(400, 320);
            this.panel1.TabIndex = 0;
            // 
            // btnHistorialPdf
            // 
            this.btnHistorialPdf.Location = new System.Drawing.Point(15, 201);
            this.btnHistorialPdf.Name = "btnHistorialPdf";
            this.btnHistorialPdf.Size = new System.Drawing.Size(320, 40);
            this.btnHistorialPdf.TabIndex = 5;
            this.btnHistorialPdf.Text = "Historial (PDF)";
            this.btnHistorialPdf.UseVisualStyleBackColor = true;
            this.btnHistorialPdf.Click += new System.EventHandler(this.btnHistorialPdf_Click);
            // 
            // labelInfo
            // 
            this.labelInfo.AutoSize = true;
            this.labelInfo.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.labelInfo.Location = new System.Drawing.Point(12, 260);
            this.labelInfo.MaximumSize = new System.Drawing.Size(320, 0);
            this.labelInfo.Name = "labelInfo";
            this.labelInfo.Size = new System.Drawing.Size(291, 30);
            this.labelInfo.TabIndex = 4;
            this.labelInfo.Text = "Elige qué datos exportar. Te pedirá dónde guardar el archivo .xlsx y podrás abrir" +
    "lo directamente al terminar.";
            // 
            // btnTransacciones
            // 
            this.btnTransacciones.Location = new System.Drawing.Point(15, 155);
            this.btnTransacciones.Name = "btnTransacciones";
            this.btnTransacciones.Size = new System.Drawing.Size(320, 40);
            this.btnTransacciones.TabIndex = 3;
            this.btnTransacciones.Text = "Exportar Transacciones";
            this.btnTransacciones.UseVisualStyleBackColor = true;
            this.btnTransacciones.Click += new System.EventHandler(this.btnTransacciones_Click);
            // 
            // btnStock
            // 
            this.btnStock.Location = new System.Drawing.Point(15, 105);
            this.btnStock.Name = "btnStock";
            this.btnStock.Size = new System.Drawing.Size(320, 40);
            this.btnStock.TabIndex = 2;
            this.btnStock.Text = "Exportar Stock";
            this.btnStock.UseVisualStyleBackColor = true;
            this.btnStock.Click += new System.EventHandler(this.btnStock_Click);
            // 
            // btnClientes
            // 
            this.btnClientes.Location = new System.Drawing.Point(15, 55);
            this.btnClientes.Name = "btnClientes";
            this.btnClientes.Size = new System.Drawing.Size(320, 40);
            this.btnClientes.TabIndex = 1;
            this.btnClientes.Text = "Exportar Clientes";
            this.btnClientes.UseVisualStyleBackColor = true;
            this.btnClientes.Click += new System.EventHandler(this.btnClientes_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(15, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(132, 21);
            this.label1.TabIndex = 0;
            this.label1.Text = "Exportar a Excel";
            // 
            // FrmExportar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(400, 320);
            this.Controls.Add(this.panel1);
            this.Name = "FrmExportar";
            this.Text = "Exportar a Excel";
            this.Load += new System.EventHandler(this.FrmExportar_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnClientes;
        private System.Windows.Forms.Button btnStock;
        private System.Windows.Forms.Button btnTransacciones;
        private System.Windows.Forms.Label labelInfo;
        private System.Windows.Forms.Button btnHistorialPdf;
    }
}
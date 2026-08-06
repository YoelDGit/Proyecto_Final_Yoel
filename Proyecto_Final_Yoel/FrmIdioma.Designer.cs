namespace Proyecto_Final_Yoel
{
    partial class FrmIdioma
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
            this.btnPortugues = new System.Windows.Forms.Button();
            this.btnAleman = new System.Windows.Forms.Button();
            this.btnFrances = new System.Windows.Forms.Button();
            this.btnIngles = new System.Windows.Forms.Button();
            this.btnEspanol = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            //
            // panel1
            //
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.btnPortugues);
            this.panel1.Controls.Add(this.btnAleman);
            this.panel1.Controls.Add(this.btnFrances);
            this.panel1.Controls.Add(this.btnIngles);
            this.panel1.Controls.Add(this.btnEspanol);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(340, 320);
            this.panel1.TabIndex = 0;
            //
            // label1
            //
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(15, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 21);
            this.label1.TabIndex = 0;
            this.label1.Text = "Idioma";
            //
            // btnEspanol
            //
            this.btnEspanol.Location = new System.Drawing.Point(15, 55);
            this.btnEspanol.Name = "btnEspanol";
            this.btnEspanol.Size = new System.Drawing.Size(300, 40);
            this.btnEspanol.TabIndex = 1;
            this.btnEspanol.Text = "Español";
            this.btnEspanol.UseVisualStyleBackColor = true;
            this.btnEspanol.Click += new System.EventHandler(this.btnEspanol_Click);
            //
            // btnIngles
            //
            this.btnIngles.Location = new System.Drawing.Point(15, 105);
            this.btnIngles.Name = "btnIngles";
            this.btnIngles.Size = new System.Drawing.Size(300, 40);
            this.btnIngles.TabIndex = 2;
            this.btnIngles.Text = "English";
            this.btnIngles.UseVisualStyleBackColor = true;
            this.btnIngles.Click += new System.EventHandler(this.btnIngles_Click);
            //
            // btnFrances
            //
            this.btnFrances.Location = new System.Drawing.Point(15, 155);
            this.btnFrances.Name = "btnFrances";
            this.btnFrances.Size = new System.Drawing.Size(300, 40);
            this.btnFrances.TabIndex = 3;
            this.btnFrances.Text = "Français";
            this.btnFrances.UseVisualStyleBackColor = true;
            this.btnFrances.Click += new System.EventHandler(this.btnFrances_Click);
            //
            // btnAleman
            //
            this.btnAleman.Location = new System.Drawing.Point(15, 205);
            this.btnAleman.Name = "btnAleman";
            this.btnAleman.Size = new System.Drawing.Size(300, 40);
            this.btnAleman.TabIndex = 4;
            this.btnAleman.Text = "Deutsch";
            this.btnAleman.UseVisualStyleBackColor = true;
            this.btnAleman.Click += new System.EventHandler(this.btnAleman_Click);
            //
            // btnPortugues
            //
            this.btnPortugues.Location = new System.Drawing.Point(15, 255);
            this.btnPortugues.Name = "btnPortugues";
            this.btnPortugues.Size = new System.Drawing.Size(300, 40);
            this.btnPortugues.TabIndex = 5;
            this.btnPortugues.Text = "Português";
            this.btnPortugues.UseVisualStyleBackColor = true;
            this.btnPortugues.Click += new System.EventHandler(this.btnPortugues_Click);
            //
            // FrmIdioma
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(340, 320);
            this.Controls.Add(this.panel1);
            this.Name = "FrmIdioma";
            this.Text = "Idioma";
            this.Load += new System.EventHandler(this.FrmIdioma_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnEspanol;
        private System.Windows.Forms.Button btnIngles;
        private System.Windows.Forms.Button btnFrances;
        private System.Windows.Forms.Button btnAleman;
        private System.Windows.Forms.Button btnPortugues;
    }
}

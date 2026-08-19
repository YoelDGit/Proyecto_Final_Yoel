
namespace Proyecto_Final_Yoel
{
    partial class Login
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Login));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonIniciarLogin = new System.Windows.Forms.Button();
            this.txtUsuario = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtContrasena = new System.Windows.Forms.TextBox();
            this.buttonCancelarLogin = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.LabelTime = new System.Windows.Forms.Label();
            this.LabelFecha = new System.Windows.Forms.Label();
            this.buttonCrearLogin = new System.Windows.Forms.Button();
            this.MouseEventArg = new System.Windows.Forms.PictureBox();
            this.labelIdioma = new System.Windows.Forms.Label();
            this.comboIdioma = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MouseEventArg)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(236, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(201, 177);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Usuario:";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // buttonIniciarLogin
            // 
            this.buttonIniciarLogin.Location = new System.Drawing.Point(24, 112);
            this.buttonIniciarLogin.Name = "buttonIniciarLogin";
            this.buttonIniciarLogin.Size = new System.Drawing.Size(58, 23);
            this.buttonIniciarLogin.TabIndex = 2;
            this.buttonIniciarLogin.Text = "Iniciar";
            this.buttonIniciarLogin.UseVisualStyleBackColor = true;
            this.buttonIniciarLogin.Click += new System.EventHandler(this.buttonIniciarLogin_Click);
            // 
            // txtUsuario
            // 
            this.txtUsuario.Location = new System.Drawing.Point(24, 28);
            this.txtUsuario.Name = "txtUsuario";
            this.txtUsuario.Size = new System.Drawing.Size(198, 20);
            this.txtUsuario.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(21, 61);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Contraseña:";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // txtContrasena
            // 
            this.txtContrasena.Location = new System.Drawing.Point(24, 77);
            this.txtContrasena.Name = "txtContrasena";
            this.txtContrasena.Size = new System.Drawing.Size(174, 20);
            this.txtContrasena.TabIndex = 5;
            this.txtContrasena.UseSystemPasswordChar = true;
            this.txtContrasena.TextChanged += new System.EventHandler(this.txtContrasena_TextChanged);
            // 
            // buttonCancelarLogin
            // 
            this.buttonCancelarLogin.Location = new System.Drawing.Point(152, 112);
            this.buttonCancelarLogin.Name = "buttonCancelarLogin";
            this.buttonCancelarLogin.Size = new System.Drawing.Size(58, 23);
            this.buttonCancelarLogin.TabIndex = 6;
            this.buttonCancelarLogin.Text = "Cancelar";
            this.buttonCancelarLogin.UseVisualStyleBackColor = true;
            this.buttonCancelarLogin.Click += new System.EventHandler(this.buttonCancelarLogin_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(28, 152);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(33, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Hora:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(21, 176);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(40, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Fecha:";
            // 
            // LabelTime
            // 
            this.LabelTime.AutoSize = true;
            this.LabelTime.Location = new System.Drawing.Point(67, 152);
            this.LabelTime.Name = "LabelTime";
            this.LabelTime.Size = new System.Drawing.Size(13, 13);
            this.LabelTime.TabIndex = 9;
            this.LabelTime.Text = "_";
            this.LabelTime.Click += new System.EventHandler(this.label5_Click);
            // 
            // LabelFecha
            // 
            this.LabelFecha.AutoSize = true;
            this.LabelFecha.Location = new System.Drawing.Point(67, 176);
            this.LabelFecha.Name = "LabelFecha";
            this.LabelFecha.Size = new System.Drawing.Size(13, 13);
            this.LabelFecha.TabIndex = 10;
            this.LabelFecha.Text = "_";
            // 
            // buttonCrearLogin
            // 
            this.buttonCrearLogin.Location = new System.Drawing.Point(88, 112);
            this.buttonCrearLogin.Name = "buttonCrearLogin";
            this.buttonCrearLogin.Size = new System.Drawing.Size(58, 23);
            this.buttonCrearLogin.TabIndex = 11;
            this.buttonCrearLogin.Text = "Crear";
            this.buttonCrearLogin.UseVisualStyleBackColor = true;
            this.buttonCrearLogin.Click += new System.EventHandler(this.button3_Click);
            // 
            // MouseEventArg
            // 
            this.MouseEventArg.BackColor = System.Drawing.SystemColors.Control;
            this.MouseEventArg.Cursor = System.Windows.Forms.Cursors.Hand;
            this.MouseEventArg.Image = ((System.Drawing.Image)(resources.GetObject("MouseEventArg.Image")));
            this.MouseEventArg.Location = new System.Drawing.Point(204, 77);
            this.MouseEventArg.Name = "MouseEventArg";
            this.MouseEventArg.Size = new System.Drawing.Size(26, 20);
            this.MouseEventArg.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.MouseEventArg.TabIndex = 12;
            this.MouseEventArg.TabStop = false;
            this.MouseEventArg.Click += new System.EventHandler(this.pictureBox2_Click);
            // 
            // labelIdioma
            // 
            this.labelIdioma.AutoSize = true;
            this.labelIdioma.Location = new System.Drawing.Point(21, 205);
            this.labelIdioma.Name = "labelIdioma";
            this.labelIdioma.Size = new System.Drawing.Size(41, 13);
            this.labelIdioma.TabIndex = 13;
            this.labelIdioma.Text = "Idioma:";
            // 
            // comboIdioma
            // 
            this.comboIdioma.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboIdioma.FormattingEnabled = true;
            this.comboIdioma.Items.AddRange(new object[] {
            "Español",
            "English",
            "Français",
            "Deutsch",
            "Português"});
            this.comboIdioma.Location = new System.Drawing.Point(88, 202);
            this.comboIdioma.Name = "comboIdioma";
            this.comboIdioma.Size = new System.Drawing.Size(142, 21);
            this.comboIdioma.TabIndex = 14;
            this.comboIdioma.SelectedIndexChanged += new System.EventHandler(this.comboIdioma_SelectedIndexChanged);
            // 
            // Login
            // 
            this.AcceptButton = this.buttonIniciarLogin;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(444, 235);
            this.Controls.Add(this.comboIdioma);
            this.Controls.Add(this.labelIdioma);
            this.Controls.Add(this.MouseEventArg);
            this.Controls.Add(this.buttonCrearLogin);
            this.Controls.Add(this.LabelFecha);
            this.Controls.Add(this.LabelTime);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.buttonCancelarLogin);
            this.Controls.Add(this.txtContrasena);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtUsuario);
            this.Controls.Add(this.buttonIniciarLogin);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Login";
            this.Text = "Login";
            this.Load += new System.EventHandler(this.Login_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MouseEventArg)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonIniciarLogin;
        private System.Windows.Forms.TextBox txtUsuario;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtContrasena;
        private System.Windows.Forms.Button buttonCancelarLogin;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label LabelTime;
        private System.Windows.Forms.Label LabelFecha;
        private System.Windows.Forms.Button buttonCrearLogin;
        private System.Windows.Forms.PictureBox MouseEventArg;
        private System.Windows.Forms.Label labelIdioma;
        private System.Windows.Forms.ComboBox comboIdioma;
    }
}


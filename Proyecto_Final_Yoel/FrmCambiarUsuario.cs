using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace Proyecto_Final_Yoel
{
    public partial class FrmCambiarUsuario : Form
    {
        private ConexionDBDataContext db = new ConexionDBDataContext();

        public FrmCambiarUsuario()
        {
            InitializeComponent();
            this.AutoScaleMode = AutoScaleMode.Inherit;
        }

        private void FrmCambiarUsuario_Load(object sender, EventArgs e)
        {
            EstiloModerno.AplicarTema(this);
            CargarUsuarios();
        }

        private void CargarUsuarios()
        {
            flowPanel.Controls.Clear();

            var todos = db.Inicio_Sesion
                .OrderBy(u => u.Usuario)
                .ToList();

            var administradores = todos.Where(u => u.EsAdministrador).ToList();
            var secundarios = todos.Where(u => !u.EsAdministrador).ToList();

            // Si el usuario activo NO es administrador, ni siquiera se le muestra
            // la sección de administradores (no puede verla ni intentar entrar
            // con esas cuentas desde aquí).
            if (SesionActual.EsAdministrador)
            {
                flowPanel.Controls.Add(CrearEncabezadoSeccion("Administradores"));
                flowPanel.Controls.Add(CrearBotonNuevoAdmin());

                if (administradores.Count == 0)
                {
                    flowPanel.Controls.Add(CrearMensajeVacio("No hay administradores creados."));
                }
                else
                {
                    foreach (var usuario in administradores)
                    {
                        flowPanel.Controls.Add(CrearTarjetaUsuario(usuario));
                    }
                }
            }

            flowPanel.Controls.Add(CrearEncabezadoSeccion("Usuarios secundarios"));

            if (secundarios.Count == 0)
            {
                flowPanel.Controls.Add(CrearMensajeVacio("No hay usuarios secundarios creados."));
            }
            else
            {
                foreach (var usuario in secundarios)
                {
                    flowPanel.Controls.Add(CrearTarjetaUsuario(usuario));
                }
            }
        }

        private Label CrearEncabezadoSeccion(string texto)
        {
            return new Label
            {
                Text = texto,
                AutoSize = true,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = EstiloModerno.Primario,
                Margin = new Padding(6, 14, 6, 4)
            };
        }

        private Label CrearMensajeVacio(string texto)
        {
            return new Label
            {
                Text = texto,
                AutoSize = true,
                Font = new Font("Segoe UI", 9F),
                ForeColor = EstiloModerno.TextoSecundario,
                Margin = new Padding(6, 0, 6, 8)
            };
        }

        private Panel CrearTarjetaUsuario(Inicio_Sesion usuario)
        {
            Panel tarjeta = new Panel
            {
                Width = Math.Max(280, flowPanel.ClientSize.Width - 25),
                Height = 64,
                Margin = new Padding(8, 4, 8, 4),
                BackColor = EstiloModerno.FondoTarjeta,
                Cursor = Cursors.Hand
            };

            tarjeta.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (GraphicsPath ruta = RutaRedondeada(tarjeta.Width, tarjeta.Height, 12))
                {
                    tarjeta.Region = new Region(ruta);
                    using (SolidBrush brush = new SolidBrush(EstiloModerno.FondoTarjeta))
                    {
                        e.Graphics.FillPath(brush, ruta);
                    }
                }
            };

            Label avatar = new Label
            {
                Text = string.IsNullOrEmpty(usuario.Usuario) ? "?" : usuario.Usuario.Substring(0, 1).ToUpper(),
                Size = new Size(40, 40),
                Location = new Point(12, 12),
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = usuario.EsAdministrador ? EstiloModerno.PrimarioOscuro : EstiloModerno.Primario,
                Cursor = Cursors.Hand
            };
            avatar.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (GraphicsPath circulo = new GraphicsPath())
                {
                    circulo.AddEllipse(0, 0, avatar.Width - 1, avatar.Height - 1);
                    avatar.Region = new Region(circulo);
                }
            };

            Label nombreUsuario = new Label
            {
                Text = usuario.Usuario + (usuario.Usuario == SesionActual.UsuarioActual ? "  (sesión actual)" : ""),
                Location = new Point(64, 10),
                AutoSize = true,
                Font = new Font("Segoe UI", 10.5F, FontStyle.Bold),
                ForeColor = EstiloModerno.TextoPrincipal,
                Cursor = Cursors.Hand
            };

            Label rolLabel = new Label
            {
                Text = usuario.EsAdministrador ? "Administrador" : "Usuario secundario",
                Location = new Point(64, 32),
                AutoSize = true,
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = EstiloModerno.TextoSecundario,
                Cursor = Cursors.Hand
            };

            // Cualquier parte de la tarjeta es clicable, no solo un botón concreto
            EventHandler alClicar = (s, e) => IntentarCambiarA(usuario);
            tarjeta.Click += alClicar;
            avatar.Click += alClicar;
            nombreUsuario.Click += alClicar;
            rolLabel.Click += alClicar;

            tarjeta.Controls.Add(avatar);
            tarjeta.Controls.Add(nombreUsuario);
            tarjeta.Controls.Add(rolLabel);

            return tarjeta;
        }

        private Button CrearBotonNuevoAdmin()
        {
            Button boton = new Button
            {
                Text = "+ Crear nuevo administrador",
                AutoSize = true,
                Padding = new Padding(10, 6, 10, 6),
                Margin = new Padding(6, 0, 6, 10),
                FlatStyle = FlatStyle.Flat,
                BackColor = EstiloModerno.Primario,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            boton.FlatAppearance.BorderSize = 0;
            boton.Click += (s, e) => CrearNuevoAdministrador();
            return boton;
        }

        private void CrearNuevoAdministrador()
        {
            if (!PedirNuevoUsuario(out string usuario, out string password))
            {
                return; // Canceló
            }

            if (string.IsNullOrWhiteSpace(usuario) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Usuario y contraseña son obligatorios.", "Campos incompletos",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (usuario.Length > 10 || password.Length > 10)
            {
                MessageBox.Show("El usuario y la contraseña deben tener 10 caracteres como máximo.",
                    "Demasiado largo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (db.Inicio_Sesion.Any(u => u.Usuario == usuario))
            {
                MessageBox.Show("Ya existe un usuario con ese nombre.", "Usuario duplicado",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                Inicio_Sesion nuevoAdmin = new Inicio_Sesion
                {
                    Usuario = usuario,
                    Contrasena = password,
                    Fecha = DateTime.Now.Date,
                    Hora = DateTime.Now.TimeOfDay,
                    EsAdministrador = true // La diferencia clave con el "Crear" del Login
                };

                db.Inicio_Sesion.InsertOnSubmit(nuevoAdmin);
                db.SubmitChanges();

                MessageBox.Show($"Administrador \"{usuario}\" creado correctamente.", "Éxito",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                CargarUsuarios();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al crear el administrador: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Cuadro de diálogo modal para pedir usuario + contraseña de la cuenta
        // nueva (solo se usa para crear administradores, desde aquí)
        private bool PedirNuevoUsuario(out string usuario, out string password)
        {
            usuario = "";
            password = "";

            using (Form dialogo = new Form())
            {
                dialogo.Text = "Nuevo administrador";
                dialogo.FormBorderStyle = FormBorderStyle.FixedDialog;
                dialogo.StartPosition = FormStartPosition.CenterParent;
                dialogo.MinimizeBox = false;
                dialogo.MaximizeBox = false;
                dialogo.ClientSize = new Size(300, 160);
                dialogo.BackColor = EstiloModerno.FondoBase;
                dialogo.Font = new Font("Segoe UI", 9.5F);

                Label etiquetaUsuario = new Label
                {
                    Text = "Nombre de usuario (máx. 10):",
                    Location = new Point(15, 15),
                    AutoSize = true,
                    ForeColor = EstiloModerno.TextoPrincipal
                };

                TextBox txtUsuario = new TextBox
                {
                    Location = new Point(15, 38),
                    Width = 270,
                    MaxLength = 10
                };

                Label etiquetaPassword = new Label
                {
                    Text = "Contraseña (máx. 10):",
                    Location = new Point(15, 68),
                    AutoSize = true,
                    ForeColor = EstiloModerno.TextoPrincipal
                };

                TextBox txtPassword = new TextBox
                {
                    Location = new Point(15, 91),
                    Width = 270,
                    MaxLength = 10,
                    UseSystemPasswordChar = true
                };

                Button btnOk = new Button
                {
                    Text = "Crear",
                    Location = new Point(110, 122),
                    Size = new Size(85, 30),
                    DialogResult = DialogResult.OK,
                    BackColor = EstiloModerno.Primario,
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat
                };
                btnOk.FlatAppearance.BorderSize = 0;

                Button btnCancelar = new Button
                {
                    Text = "Cancelar",
                    Location = new Point(200, 122),
                    Size = new Size(85, 30),
                    DialogResult = DialogResult.Cancel,
                    FlatStyle = FlatStyle.Flat
                };

                dialogo.Controls.Add(etiquetaUsuario);
                dialogo.Controls.Add(txtUsuario);
                dialogo.Controls.Add(etiquetaPassword);
                dialogo.Controls.Add(txtPassword);
                dialogo.Controls.Add(btnOk);
                dialogo.Controls.Add(btnCancelar);
                dialogo.AcceptButton = btnOk;
                dialogo.CancelButton = btnCancelar;

                DialogResult resultado = dialogo.ShowDialog(this);

                if (resultado == DialogResult.OK)
                {
                    usuario = txtUsuario.Text.Trim();
                    password = txtPassword.Text;
                    return true;
                }

                return false;
            }
        }

        // Pide la contraseña de ese usuario y, si es correcta, cambia la sesión activa
        private void IntentarCambiarA(Inicio_Sesion usuario)
        {
            if (!PedirPassword(usuario.Usuario, out string password))
            {
                return; // El usuario canceló el cuadro de contraseña
            }

            if (usuario.Contrasena != password)
            {
                MessageBox.Show("Contraseña incorrecta.", "Acceso denegado",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Registramos este acceso, igual que hace el Login normal
            var usuarioEnBd = db.Inicio_Sesion.FirstOrDefault(u => u.IdUsuario == usuario.IdUsuario);
            if (usuarioEnBd != null)
            {
                usuarioEnBd.Fecha = DateTime.Now.Date;
                usuarioEnBd.Hora = DateTime.Now.TimeOfDay;
                db.SubmitChanges();
            }

            SesionActual.IniciarSesion(usuario.Usuario, usuario.EsAdministrador);
            SesionActual.RefrescarPermisosGlobal();

            MessageBox.Show($"Ahora has entrado como: {usuario.Usuario} ({(usuario.EsAdministrador ? "Administrador" : "Usuario secundario")})",
                "Usuario cambiado", MessageBoxButtons.OK, MessageBoxIcon.Information);

            CargarUsuarios(); // Refresca la lista (por si ahora ya no eres admin y hay que ocultar esa sección)
        }

        // Pequeño cuadro de diálogo modal para pedir la contraseña, construido
        // en código (no necesita su propio Designer.cs)
        private bool PedirPassword(string nombreUsuario, out string password)
        {
            password = "";

            using (Form dialogo = new Form())
            {
                dialogo.Text = "Entrar como " + nombreUsuario;
                dialogo.FormBorderStyle = FormBorderStyle.FixedDialog;
                dialogo.StartPosition = FormStartPosition.CenterParent;
                dialogo.MinimizeBox = false;
                dialogo.MaximizeBox = false;
                dialogo.ClientSize = new Size(300, 120);
                dialogo.BackColor = EstiloModerno.FondoBase;
                dialogo.Font = new Font("Segoe UI", 9.5F);

                Label etiqueta = new Label
                {
                    Text = "Contraseña de " + nombreUsuario + ":",
                    Location = new Point(15, 15),
                    AutoSize = true,
                    ForeColor = EstiloModerno.TextoPrincipal
                };

                TextBox txtPassword = new TextBox
                {
                    Location = new Point(15, 40),
                    Width = 270,
                    UseSystemPasswordChar = true
                };

                Button btnOk = new Button
                {
                    Text = "Entrar",
                    Location = new Point(110, 75),
                    Size = new Size(85, 30),
                    DialogResult = DialogResult.OK,
                    BackColor = EstiloModerno.Primario,
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat
                };
                btnOk.FlatAppearance.BorderSize = 0;

                Button btnCancelar = new Button
                {
                    Text = "Cancelar",
                    Location = new Point(200, 75),
                    Size = new Size(85, 30),
                    DialogResult = DialogResult.Cancel,
                    FlatStyle = FlatStyle.Flat
                };

                dialogo.Controls.Add(etiqueta);
                dialogo.Controls.Add(txtPassword);
                dialogo.Controls.Add(btnOk);
                dialogo.Controls.Add(btnCancelar);
                dialogo.AcceptButton = btnOk;
                dialogo.CancelButton = btnCancelar;

                DialogResult resultado = dialogo.ShowDialog(this);

                if (resultado == DialogResult.OK)
                {
                    password = txtPassword.Text;
                    return true;
                }

                return false;
            }
        }

        private GraphicsPath RutaRedondeada(int width, int height, int radio)
        {
            GraphicsPath path = new GraphicsPath();
            path.AddArc(0, 0, radio, radio, 180, 90);
            path.AddArc(width - radio, 0, radio, radio, 270, 90);
            path.AddArc(width - radio, height - radio, radio, radio, 0, 90);
            path.AddArc(0, height - radio, radio, radio, 90, 90);
            path.CloseAllFigures();
            return path;
        }
    }
}

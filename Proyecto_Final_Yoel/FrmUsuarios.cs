using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace Proyecto_Final_Yoel
{
    public partial class FrmUsuarios : Form
    {
        private ConexionDBDataContext db = new ConexionDBDataContext();

        public FrmUsuarios()
        {
            InitializeComponent();
            this.AutoScaleMode = AutoScaleMode.Inherit;
        }

        private void FrmUsuarios_Load(object sender, EventArgs e)
        {
            EstiloModerno.AplicarTema(this);
            CargarUsuarios();
        }

        // Pinta una tarjeta por usuario, apiladas una debajo de otra (estilo
        // selector de cuenta de Google), ordenadas por el último acceso más reciente
        private void CargarUsuarios()
        {
            flowPanel.Controls.Clear();

            var usuarios = db.Inicio_Sesion
                .OrderByDescending(u => u.Fecha)
                .ThenByDescending(u => u.Hora)
                .ToList();

            if (usuarios.Count == 0)
            {
                Label vacio = new Label
                {
                    Text = "No hay usuarios creados todavía.",
                    AutoSize = true,
                    ForeColor = EstiloModerno.TextoSecundario,
                    Font = new Font("Segoe UI", 10F),
                    Margin = new Padding(10, 20, 10, 10)
                };
                flowPanel.Controls.Add(vacio);
                return;
            }

            foreach (var usuario in usuarios)
            {
                flowPanel.Controls.Add(CrearTarjetaUsuario(usuario));
            }
        }

        private Panel CrearTarjetaUsuario(Inicio_Sesion usuario)
        {
            Panel tarjeta = new Panel
            {
                Width = Math.Max(280, flowPanel.ClientSize.Width - 25),
                Height = 90,
                Margin = new Padding(8, 6, 8, 6),
                BackColor = EstiloModerno.FondoTarjeta
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

            // Avatar circular con la inicial del usuario
            Label avatar = new Label
            {
                Text = string.IsNullOrEmpty(usuario.Usuario) ? "?" : usuario.Usuario.Substring(0, 1).ToUpper(),
                Size = new Size(48, 48),
                Location = new Point(12, 21),
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = EstiloModerno.Primario
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
                Text = usuario.Usuario,
                Location = new Point(72, 10),
                AutoSize = true,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = EstiloModerno.TextoPrincipal
            };

            Label rolLabel = new Label
            {
                Text = usuario.EsAdministrador ? "Administrador" : "Usuario secundario",
                Location = new Point(72, 32),
                AutoSize = true,
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                ForeColor = usuario.EsAdministrador ? EstiloModerno.Primario : EstiloModerno.TextoSecundario
            };

            // Fecha (solo día) + Hora (solo hora) se guardan por separado en la
            // tabla; los combinamos aquí para mostrar el último acceso completo
            DateTime ultimoAcceso = usuario.Fecha.Date + usuario.Hora;

            Label ultimoLabel = new Label
            {
                Text = "Último acceso: " + ultimoAcceso.ToString("dd/MM/yyyy - HH:mm:ss"),
                Location = new Point(72, 54),
                AutoSize = true,
                Font = new Font("Segoe UI", 9F),
                ForeColor = EstiloModerno.TextoSecundario
            };

            tarjeta.Controls.Add(avatar);
            tarjeta.Controls.Add(nombreUsuario);
            tarjeta.Controls.Add(rolLabel);
            tarjeta.Controls.Add(ultimoLabel);

            return tarjeta;
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

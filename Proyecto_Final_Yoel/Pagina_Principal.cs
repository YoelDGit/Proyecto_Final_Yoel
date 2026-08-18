using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Proyecto_Final_Yoel
{
    public partial class Pagina_Principal : Form
    {
        public Pagina_Principal()
        {
            InitializeComponent();
        }

        private void AbrirFormularioEnPanel(Form formHijo)
        {
            // 1. Limpia cualquier control previo en el panel
            if (panelContenedor.Controls.Count > 0)
            {
                panelContenedor.Controls.Clear();
            }

            // 2. Configura el formulario para que actúe como un control interno
            formHijo.TopLevel = false;
            formHijo.FormBorderStyle = FormBorderStyle.None;

            // IMPORTANTE: Forzar el tamaño inicial del hijo al del contenedor
            formHijo.Size = panelContenedor.ClientSize;

            // Ahora sí, acoplamos al 100% del panel
            formHijo.Dock = DockStyle.Fill;

            // 3. Añade el formulario al panel y muéstralo
            panelContenedor.Controls.Add(formHijo);
            panelContenedor.Tag = formHijo;
            formHijo.Show();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Pagina_Principal_Load(object sender, EventArgs e)
        {
            // ¡La magia ocurre aquí! Pasa este formulario como parámetro
            EstiloModerno.AplicarTema(this);
            Idiomas.AplicarIdioma(this);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            AbrirFormularioEnPanel(new Cliente());
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            AbrirFormularioEnPanel(new FrmStock());
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            AbrirFormularioEnPanel(new FormTransacciones());
        }

        private void button5_Click(object sender, EventArgs e)
        {
            AbrirFormularioEnPanel(new Configuración());
        }

        private void button4_Click(object sender, EventArgs e) // "USUARIOS"
        {
            AbrirFormularioEnPanel(new FrmUsuarios());
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e) // "LOGIN"
        {
            AbrirFormularioEnPanel(new FrmCambiarUsuario());
        }
    }
}

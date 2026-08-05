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
    public partial class Configuración : Form
    {
        public Configuración()
        {
            InitializeComponent();
        }

        private void AbrirFormularioEnPanel(Form formHijo)
        {
            if (panel2.Controls.Count > 0)
            {
                panel2.Controls.Clear();
            }

            formHijo.TopLevel = false;
            formHijo.FormBorderStyle = FormBorderStyle.None;
            formHijo.Size = panel2.ClientSize;
            formHijo.Dock = DockStyle.Fill;

            panel2.Controls.Add(formHijo);
            formHijo.Show();
        }

        private void button1_Click(object sender, EventArgs e) // "Modificar Categorías"
        {
            AbrirFormularioEnPanel(new FrmCategorias());
        }

        private void button2_Click(object sender, EventArgs e) // "Diseño"
        {
            AbrirFormularioEnPanel(new FrmTema());
        }

        private void Configuración_Load(object sender, EventArgs e)
        {
            EstiloModerno.AplicarTema(this);
        }
    }
}

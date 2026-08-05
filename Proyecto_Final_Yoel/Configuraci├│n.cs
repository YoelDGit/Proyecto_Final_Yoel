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

            this.AutoScaleMode = AutoScaleMode.Inherit;
        }

        private void Configuración_Load(object sender, EventArgs e)
        {
            EstiloModerno.AplicarTema(this);
        }

        // Mismo patrón que AbrirFormularioEnPanel de Pagina_Principal, pero
        // aquí el contenedor es flowLayoutPanel1 (el panel derecho de Configuración)
        private void AbrirFormularioEnPanel(Form formHijo)
        {
            if (flowLayoutPanel1.Controls.Count > 0)
            {
                flowLayoutPanel1.Controls.Clear();
            }

            formHijo.TopLevel = false;
            formHijo.FormBorderStyle = FormBorderStyle.None;
            formHijo.Size = flowLayoutPanel1.ClientSize;
            formHijo.Dock = DockStyle.Fill;

            flowLayoutPanel1.Controls.Add(formHijo);
            formHijo.Show();
        }

        private void button1_Click(object sender, EventArgs e) // "Modificar Categorías"
        {
            AbrirFormularioEnPanel(new FrmCategorias());
        }
    }
}

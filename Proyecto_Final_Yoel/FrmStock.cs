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
    public partial class FrmStock : Form
    {

        private void FrmStock_Load(object sender, EventArgs e)
        {
            // ¡La magia ocurre aquí! Pasa este formulario como parámetro
            EstiloModerno.AplicarTema(this);
        }
        public FrmStock()
        {
            InitializeComponent();

            // FUERZA AL HIJO A HEREDAR LA ESCALA DEL PANEL DE LA PÁGINA PRINCIPAL
            this.AutoScaleMode = AutoScaleMode.Inherit;
        }



        private void label1_Click(object sender, EventArgs e)
        {
        }
    }
}
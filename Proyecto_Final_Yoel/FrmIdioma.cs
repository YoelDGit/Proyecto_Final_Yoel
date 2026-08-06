using System;
using System.Windows.Forms;

namespace Proyecto_Final_Yoel
{
    public partial class FrmIdioma : Form
    {
        public FrmIdioma()
        {
            InitializeComponent();
            this.AutoScaleMode = AutoScaleMode.Inherit;
        }

        private void FrmIdioma_Load(object sender, EventArgs e)
        {
            EstiloModerno.AplicarTema(this);
            Idiomas.AplicarIdioma(this);
            ActualizarSeleccionVisual();
        }

        private void btnEspanol_Click(object sender, EventArgs e)
        {
            Idiomas.CambiarIdioma(IdiomaApp.Espanol);
            ActualizarSeleccionVisual();
        }

        private void btnIngles_Click(object sender, EventArgs e)
        {
            Idiomas.CambiarIdioma(IdiomaApp.Ingles);
            ActualizarSeleccionVisual();
        }

        private void btnFrances_Click(object sender, EventArgs e)
        {
            Idiomas.CambiarIdioma(IdiomaApp.Frances);
            ActualizarSeleccionVisual();
        }

        private void btnAleman_Click(object sender, EventArgs e)
        {
            Idiomas.CambiarIdioma(IdiomaApp.Aleman);
            ActualizarSeleccionVisual();
        }

        private void btnPortugues_Click(object sender, EventArgs e)
        {
            Idiomas.CambiarIdioma(IdiomaApp.Portugues);
            ActualizarSeleccionVisual();
        }

        // Marca con un check (✓) el botón del idioma activo ahora mismo
        private void ActualizarSeleccionVisual()
        {
            btnEspanol.Text = "Español" + (Idiomas.IdiomaActual == IdiomaApp.Espanol ? "  ✓" : "");
            btnIngles.Text = "English" + (Idiomas.IdiomaActual == IdiomaApp.Ingles ? "  ✓" : "");
            btnFrances.Text = "Français" + (Idiomas.IdiomaActual == IdiomaApp.Frances ? "  ✓" : "");
            btnAleman.Text = "Deutsch" + (Idiomas.IdiomaActual == IdiomaApp.Aleman ? "  ✓" : "");
            btnPortugues.Text = "Português" + (Idiomas.IdiomaActual == IdiomaApp.Portugues ? "  ✓" : "");
        }
    }
}

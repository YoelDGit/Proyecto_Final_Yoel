using System;
using System.Windows.Forms;

namespace Proyecto_Final_Yoel
{
    public partial class FrmTema : Form
    {
        public FrmTema()
        {
            InitializeComponent();
            this.AutoScaleMode = AutoScaleMode.Inherit;
        }

        private void FrmTema_Load(object sender, EventArgs e)
        {
            EstiloModerno.AplicarTema(this);
            ActualizarSeleccionVisual();
        }

        private void btnPredeterminado_Click(object sender, EventArgs e)
        {
            EstiloModerno.CambiarTema(TemaApp.Predeterminado);
            ActualizarSeleccionVisual();
        }

        private void btnClaro_Click(object sender, EventArgs e)
        {
            EstiloModerno.CambiarTema(TemaApp.Claro);
            ActualizarSeleccionVisual();
        }

        private void btnOscuro_Click(object sender, EventArgs e)
        {
            EstiloModerno.CambiarTema(TemaApp.Oscuro);
            ActualizarSeleccionVisual();
        }

        // Marca con un check (✓) el botón del tema que está activo ahora mismo
        private void ActualizarSeleccionVisual()
        {
            btnPredeterminado.Text = "Predeterminado" + (EstiloModerno.TemaActual == TemaApp.Predeterminado ? "  ✓" : "");
            btnClaro.Text = "Tema Claro" + (EstiloModerno.TemaActual == TemaApp.Claro ? "  ✓" : "");
            btnOscuro.Text = "Tema Oscuro" + (EstiloModerno.TemaActual == TemaApp.Oscuro ? "  ✓" : "");
        }
    }
}

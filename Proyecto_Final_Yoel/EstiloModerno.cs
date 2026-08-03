using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Proyecto_Final_Yoel
{
    public static class EstiloModerno
    {
        // Colores de la paleta extraídos de tu imagen
        public static Color FondoBase = Color.FromArgb(10, 15, 26);
        public static Color FondoTarjeta = Color.FromArgb(20, 27, 45);
        public static Color TextoPrincipal = Color.FromArgb(240, 245, 255);
        public static Color TextoSecundario = Color.FromArgb(120, 135, 165);
        public static Color NeonCian = Color.FromArgb(0, 212, 255);
        public static Color NeonVerde = Color.FromArgb(46, 213, 115);

        /// <summary>
        /// Aplica el tema oscuro futurista al formulario y a todos sus controles hijos
        /// </summary>
        public static void AplicarTema(Form formulario)
        {
            formulario.BackColor = FondoBase;
            formulario.ForeColor = TextoPrincipal;
            formulario.Font = new Font("Segoe UI UI Semibold", 9.5F, FontStyle.Regular, GraphicsUnit.Point);

            // Recorrer los paneles y elementos del formulario
            foreach (Control control in formulario.Controls)
            {
                EstilizarControl(control);
            }
        }

        private static void EstilizarControl(Control control)
        {
            // Si es un panel, actúa como las "tarjetas" redondeadas de la imagen
            if (control is Panel)
            {
                control.BackColor = FondoTarjeta;
                control.ForeColor = TextoPrincipal;

                // Forzar el redibujado para hacer las esquinas redondeadas
                control.Paint += (s, e) => DibujarEsquinasRedondeadas(control, e.Graphics, 15, FondoTarjeta);
            }
            // Si es un botón, lo hacemos plano con estilo de cápsula futurista
            else if (control is Button btn)
            {
                btn.FlatStyle = FlatStyle.Flat;
                btn.BackColor = Color.FromArgb(32, 42, 68);
                btn.ForeColor = NeonCian;
                btn.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                btn.FlatAppearance.BorderSize = 1;
                btn.FlatAppearance.BorderColor = NeonCian;
                btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(0, 212, 255);
                btn.Cursor = Cursors.Hand;
            }
            // Si es una etiqueta de texto
            else if (control is Label lbl)
            {
                lbl.ForeColor = lbl.Name.Contains("Titulo") ? NeonCian : TextoPrincipal;
            }
            // Cajas de texto (Inputs) oscuros integrados
            else if (control is TextBox txt)
            {
                txt.BackColor = Color.FromArgb(15, 20, 35);
                txt.BorderStyle = BorderStyle.FixedSingle;
                txt.ForeColor = TextoPrincipal;
            }

            // Si el control tiene subcontroles (como un panel con botones dentro), los estiliza también
            if (control.HasChildren)
            {
                foreach (Control subControl in control.Controls)
                {
                    EstilizarControl(subControl);
                }
            }
        }

        /// <summary>
        /// Dibuja bordes suaves y redondeados imitando el estilo de la imagen
        /// </summary>
        private static void DibujarEsquinasRedondeadas(Control control, Graphics g, int radio, Color colorFondo)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;

            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddArc(0, 0, radio, radio, 180, 90);
                path.AddArc(control.Width - radio, 0, radio, radio, 270, 90);
                path.AddArc(control.Width - radio, control.Height - radio, radio, radio, 0, 90);
                path.AddArc(0, control.Height - radio, radio, radio, 90, 90);
                path.CloseAllFigures();

                control.Region = new Region(path);

                using (SolidBrush brush = new SolidBrush(colorFondo))
                {
                    g.FillPath(brush, path);
                }
            }
        }
    }
}
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Proyecto_Final_Yoel
{
    public static class EstiloModerno
    {
        // Paleta turquesa/teal extraída de la referencia
        public static Color FondoBase = Color.FromArgb(245, 247, 248);   // gris casi blanco (fondo general)
        public static Color FondoTarjeta = Color.FromArgb(240, 244, 248);  // tarjetas/paneles
        public static Color TextoPrincipal = Color.FromArgb(34, 40, 49);   // gris oscuro (texto principal)
        public static Color TextoSecundario = Color.FromArgb(107, 114, 128); // gris medio (texto secundario)
        public static Color Primario = Color.FromArgb(23, 163, 152);       // teal principal (cabeceras, botones)
        public static Color PrimarioOscuro = Color.FromArgb(14, 122, 114); // teal oscuro (hover/pulsado)
        public static Color BordeSuave = Color.FromArgb(224, 230, 231);    // gris muy suave (bordes, líneas)

        /// <summary>
        /// Aplica el tema turquesa al formulario y a todos sus controles hijos
        /// </summary>
        public static void AplicarTema(Form formulario)
        {
            formulario.BackColor = FondoBase;
            formulario.ForeColor = TextoPrincipal;
            formulario.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point);

            foreach (Control control in formulario.Controls)
            {
                EstilizarControl(control);
            }
        }

        private static void EstilizarControl(Control control)
        {
            // Paneles y GroupBox actúan como las "tarjetas" blancas sobre fondo gris claro
            if (control is Panel || control is GroupBox)
            {
                control.BackColor = FondoTarjeta;
                control.ForeColor = TextoPrincipal;

                if (control is Panel)
                {
                    control.Paint += (s, e) => DibujarEsquinasRedondeadas(control, e.Graphics, 12, FondoTarjeta);
                }
            }
            // Botones: relleno teal sólido con texto blanco (como el botón "Guardar" de la referencia)
            else if (control is Button btn)
            {
                btn.FlatStyle = FlatStyle.Flat;
                btn.BackColor = Primario;
                btn.ForeColor = Color.White;
                btn.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                btn.FlatAppearance.BorderSize = 0;
                btn.FlatAppearance.MouseOverBackColor = PrimarioOscuro;
                btn.FlatAppearance.MouseDownBackColor = PrimarioOscuro;
                btn.Cursor = Cursors.Hand;
            }
            else if (control is Label lbl)
            {
                lbl.ForeColor = lbl.Name.Contains("Titulo") ? Primario : TextoPrincipal;
            }
            // Cajas de texto claras con borde suave, integradas en el fondo blanco
            else if (control is TextBox txt)
            {
                txt.BackColor = Color.White;
                txt.BorderStyle = BorderStyle.FixedSingle;
                txt.ForeColor = TextoPrincipal;
            }
            else if (control is DataGridView grid)
            {
                grid.BackgroundColor = Color.White;
                grid.BorderStyle = BorderStyle.None;
                grid.GridColor = BordeSuave;
                grid.EnableHeadersVisualStyles = false;
                grid.ColumnHeadersDefaultCellStyle.BackColor = Primario;
                grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                grid.DefaultCellStyle.BackColor = Color.White;
                grid.DefaultCellStyle.ForeColor = TextoPrincipal;
                grid.DefaultCellStyle.SelectionBackColor = Primario;
                grid.DefaultCellStyle.SelectionForeColor = Color.White;
                grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 248, 247); // tinte teal muy suave
                grid.RowHeadersVisible = false;
            }

            if (control.HasChildren)
            {
                foreach (Control subControl in control.Controls)
                {
                    EstilizarControl(subControl);
                }
            }
        }

        /// <summary>
        /// Dibuja bordes suaves y redondeados para las tarjetas/paneles
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

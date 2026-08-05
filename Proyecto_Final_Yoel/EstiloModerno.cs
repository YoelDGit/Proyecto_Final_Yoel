using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;

namespace Proyecto_Final_Yoel
{
    public enum TemaApp
    {
        Predeterminado,
        Claro,
        Oscuro
    }

    public static class EstiloModerno
    {
        // Ruta donde se guarda el tema elegido, para que se recuerde entre ejecuciones
        private static readonly string RutaConfigTema =
            Path.Combine(Application.StartupPath, "tema.cfg");

        public static TemaApp TemaActual { get; private set; } = TemaApp.Predeterminado;

        // ---------- PALETA ACTIVA (estos son los valores que usa EstilizarControl) ----------
        public static Color FondoBase;
        public static Color FondoTarjeta;
        public static Color FondoCampo;      // fondo de textboxes / celdas de grid
        public static Color FilaAlterna;     // fila alterna del DataGridView
        public static Color TextoPrincipal;
        public static Color TextoSecundario;
        public static Color Primario;
        public static Color PrimarioOscuro;
        public static Color BordeSuave;

        static EstiloModerno()
        {
            TemaActual = CargarTemaGuardado();
            AplicarPaleta(TemaActual);
        }

        // ---------- CAMBIAR DE TEMA ----------

        /// <summary>
        /// Cambia el tema activo, lo guarda para la próxima vez que se abra la app,
        /// y refresca todos los formularios que estén abiertos ahora mismo.
        /// </summary>
        public static void CambiarTema(TemaApp nuevoTema)
        {
            TemaActual = nuevoTema;
            AplicarPaleta(nuevoTema);
            GuardarTema(nuevoTema);
            AplicarTemaGlobal();
        }

        private static void AplicarPaleta(TemaApp tema)
        {
            switch (tema)
            {
                case TemaApp.Claro:
                    FondoBase = Color.FromArgb(255, 255, 255);
                    FondoTarjeta = Color.FromArgb(250, 250, 251);
                    FondoCampo = Color.White;
                    FilaAlterna = Color.FromArgb(245, 247, 250);
                    TextoPrincipal = Color.FromArgb(40, 40, 40);
                    TextoSecundario = Color.FromArgb(120, 120, 120);
                    Primario = Color.FromArgb(0, 120, 215);      // azul Windows
                    PrimarioOscuro = Color.FromArgb(0, 90, 170);
                    BordeSuave = Color.FromArgb(225, 225, 225);
                    break;

                case TemaApp.Oscuro:
                    FondoBase = Color.FromArgb(24, 26, 32);
                    FondoTarjeta = Color.FromArgb(34, 37, 46);
                    FondoCampo = Color.FromArgb(44, 47, 56);
                    FilaAlterna = Color.FromArgb(40, 43, 52);
                    TextoPrincipal = Color.FromArgb(230, 232, 235);
                    TextoSecundario = Color.FromArgb(150, 155, 165);
                    Primario = Color.FromArgb(23, 163, 152);     // mismo teal de marca
                    PrimarioOscuro = Color.FromArgb(16, 130, 120);
                    BordeSuave = Color.FromArgb(55, 58, 68);
                    break;

                case TemaApp.Predeterminado:
                default:
                    // El tema turquesa/teal que ya tenías por defecto, sin tocar
                    FondoBase = Color.FromArgb(245, 247, 248);
                    FondoTarjeta = Color.FromArgb(240, 244, 248);
                    FondoCampo = Color.White;
                    FilaAlterna = Color.FromArgb(240, 248, 247);
                    TextoPrincipal = Color.FromArgb(34, 40, 49);
                    TextoSecundario = Color.FromArgb(107, 114, 128);
                    Primario = Color.FromArgb(23, 163, 152);
                    PrimarioOscuro = Color.FromArgb(14, 122, 114);
                    BordeSuave = Color.FromArgb(224, 230, 231);
                    break;
            }
        }

        // ---------- PERSISTENCIA (archivo de texto simple, sin tocar Settings.settings) ----------

        private static TemaApp CargarTemaGuardado()
        {
            try
            {
                if (File.Exists(RutaConfigTema))
                {
                    string texto = File.ReadAllText(RutaConfigTema).Trim();
                    if (Enum.TryParse(texto, out TemaApp temaGuardado))
                    {
                        return temaGuardado;
                    }
                }
            }
            catch
            {
                // Si falla la lectura (permisos, archivo corrupto...) simplemente
                // seguimos con el tema Predeterminado sin romper el arranque de la app.
            }

            return TemaApp.Predeterminado;
        }

        private static void GuardarTema(TemaApp tema)
        {
            try
            {
                File.WriteAllText(RutaConfigTema, tema.ToString());
            }
            catch
            {
                // Igual que arriba: si no se puede guardar, la app sigue funcionando,
                // simplemente no recordará el tema la próxima vez que se abra.
            }
        }

        // ---------- APLICAR EL TEMA A UN FORMULARIO ----------

        /// <summary>
        /// Aplica el tema activo al formulario y a todos sus controles hijos
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

        /// <summary>
        /// Reaplica el tema a TODOS los formularios abiertos ahora mismo, incluidos
        /// los que están embebidos dentro de un panel (Clientes, Stock, Transacciones,
        /// Configuración, Categorías...), para que el cambio se vea al instante sin
        /// tener que cerrar y volver a abrir cada pantalla.
        /// </summary>
        public static void AplicarTemaGlobal()
        {
            foreach (Form formularioAbierto in Application.OpenForms)
            {
                AplicarTema(formularioAbierto);

                var embebidos = new List<Form>();
                BuscarFormulariosEmbebidos(formularioAbierto, embebidos);

                foreach (Form embebido in embebidos)
                {
                    AplicarTema(embebido);
                }
            }
        }

        // Recorre el árbol de controles buscando formularios embebidos (los que se
        // añaden con TopLevel = false dentro de un panel, como hacemos en
        // Pagina_Principal y en Configuración)
        private static void BuscarFormulariosEmbebidos(Control contenedor, List<Form> encontrados)
        {
            foreach (Control hijo in contenedor.Controls)
            {
                if (hijo is Form formHijo)
                {
                    encontrados.Add(formHijo);
                }

                if (hijo.HasChildren)
                {
                    BuscarFormulariosEmbebidos(hijo, encontrados);
                }
            }
        }

        private static void EstilizarControl(Control control)
        {
            // Paneles y GroupBox actúan como las "tarjetas" sobre el fondo general
            if (control is Panel || control is GroupBox)
            {
                control.BackColor = FondoTarjeta;
                control.ForeColor = TextoPrincipal;

                if (control is Panel)
                {
                    control.Paint += (s, e) => DibujarEsquinasRedondeadas(control, e.Graphics, 12, FondoTarjeta);
                }
            }
            // Botones: relleno sólido con el color primario del tema
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
            else if (control is TextBox txt)
            {
                txt.BackColor = FondoCampo;
                txt.BorderStyle = BorderStyle.FixedSingle;
                txt.ForeColor = TextoPrincipal;
            }
            else if (control is ComboBox combo)
            {
                combo.BackColor = FondoCampo;
                combo.ForeColor = TextoPrincipal;
                combo.FlatStyle = FlatStyle.Flat;
            }
            else if (control is DataGridView grid)
            {
                grid.BackgroundColor = FondoTarjeta;
                grid.BorderStyle = BorderStyle.None;
                grid.GridColor = BordeSuave;
                grid.EnableHeadersVisualStyles = false;
                grid.ColumnHeadersDefaultCellStyle.BackColor = Primario;
                grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                grid.DefaultCellStyle.BackColor = FondoCampo;
                grid.DefaultCellStyle.ForeColor = TextoPrincipal;
                grid.DefaultCellStyle.SelectionBackColor = Primario;
                grid.DefaultCellStyle.SelectionForeColor = Color.White;
                grid.AlternatingRowsDefaultCellStyle.BackColor = FilaAlterna;
                grid.AlternatingRowsDefaultCellStyle.ForeColor = TextoPrincipal;
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

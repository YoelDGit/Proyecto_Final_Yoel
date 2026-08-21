using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Proyecto_Final_Yoel
{
    public partial class FrmExportar : Form
    {
        private ConexionDBDataContext db = new ConexionDBDataContext();

        public FrmExportar()
        {
            InitializeComponent();
            this.AutoScaleMode = AutoScaleMode.Inherit;
        }

        private void FrmExportar_Load(object sender, EventArgs e)
        {
            EstiloModerno.AplicarTema(this);
        }

        // ---------- CLIENTES ----------

        private void btnClientes_Click(object sender, EventArgs e)
        {
            var encabezados = new List<string> { "IdCliente", "Nombre", "Apellidos", "Direccion", "Telefono" };

            var filas = db.Clientes
                .OrderBy(c => c.IdCliente)
                .AsEnumerable()
                .Select(c => new object[] { c.IdCliente, c.Nombre, c.Apellidos, c.Direccion, c.Telefono })
                .ToList();

            Exportar("Clientes", encabezados, filas);
        }

        // ---------- STOCK ----------

        private void btnStock_Click(object sender, EventArgs e)
        {
            var encabezados = new List<string> { "IdStock", "Nombre", "Descripcion", "Categoria", "Precio", "Cantidad" };

            var filas = db.Stock
                .OrderBy(s => s.IdStock)
                .AsEnumerable()
                .Select(s => new object[] { s.IdStock, s.Nombre, s.Descripcion, s.Categorias?.Nombre, s.Precio, s.Cantidad })
                .ToList();

            Exportar("Stock", encabezados, filas);
        }

        // ---------- TRANSACCIONES ----------

        private void btnTransacciones_Click(object sender, EventArgs e)
        {
            var encabezados = new List<string>
            {
                "IdTransaccion", "Fecha", "Tipo", "Cliente", "Producto", "Cantidad", "PrecioUnitario", "Subtotal"
            };

            var filas = db.DetalleTransaccion
                .OrderByDescending(d => d.Transacciones.Fecha)
                .AsEnumerable()
                .Select(d => new object[]
                {
                    d.IdTransaccion,
                    d.Transacciones.Fecha.ToString("dd/MM/yyyy HH:mm"),
                    d.Transacciones.Tipo,
                    d.Transacciones.Clientes.Nombre + " " + d.Transacciones.Clientes.Apellidos,
                    d.Stock.Nombre,
                    d.Cantidad,
                    d.PrecioUnitario,
                    (d.Transacciones.Tipo == "Devolucion" ? -1 : 1) * (d.PrecioUnitario * d.Cantidad) // Subtotal
                })
                .ToList();

            Exportar("Transacciones", encabezados, filas);
        }

        // ---------- LÓGICA COMÚN ----------

        private void Exportar(string nombreSugerido, List<string> encabezados, List<object[]> filas)
        {
            if (filas.Count == 0)
            {
                MessageBox.Show("No hay datos para exportar todavía.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (SaveFileDialog dialogo = new SaveFileDialog())
            {
                dialogo.Filter = "Libro de Excel (*.xlsx)|*.xlsx";
                dialogo.FileName = nombreSugerido + "_" + DateTime.Now.ToString("yyyyMMdd") + ".xlsx";

                if (dialogo.ShowDialog(this) != DialogResult.OK)
                {
                    return; // El usuario canceló
                }

                try
                {
                    ExcelExportHelper.ExportarAExcel(dialogo.FileName, nombreSugerido, encabezados, filas);

                    var abrir = MessageBox.Show(
                        $"Se exportaron {filas.Count} fila(s) a:\n{dialogo.FileName}\n\n¿Quieres abrir el archivo ahora?",
                        "Exportación completada", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                    if (abrir == DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start(dialogo.FileName);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al exportar: " + ex.Message, "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // ================================================================
        // SUSTITUYE tu método btnHistorialPdf_Click por este completo
        // ================================================================

        private void btnHistorialPdf_Click(object sender, EventArgs e)
        {
            var resumen = db.Transacciones
                .OrderByDescending(t => t.Fecha)
                .AsEnumerable()
                .Select(t => new
                {
                    t.IdTransaccion,
                    t.Fecha,
                    Cliente = t.Clientes != null ? (t.Clientes.Nombre + " " + t.Clientes.Apellidos) : "(cliente eliminado)",
                    t.Tipo,
                    // Una Devolución resta del total (es dinero que sale de caja),
                    // una Salida (venta) suma normalmente
                    Total = (t.Tipo == "Devolucion" ? -1 : 1) *
                            (t.DetalleTransaccion.Sum(d => (decimal?)(d.Cantidad * d.PrecioUnitario)) ?? 0)
                })
                .ToList();

            if (resumen.Count == 0)
            {
                MessageBox.Show("No hay transacciones registradas todavía.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var encabezados = new[] { "ID", "Fecha", "Cliente", "Tipo", "Total" };
            var anchosColumnas = new double[] { 0, 45, 140, 330, 430 };

            // Formato de moneda español: "150,00 €" en vez de "150.00"
            var culturaEuro = new System.Globalization.CultureInfo("es-ES");

            var filas = resumen
                .Select(r => new[]
                {
            r.IdTransaccion.ToString(),
            r.Fecha.ToString("dd/MM/yyyy HH:mm"),
            r.Cliente,
            r.Tipo,
            r.Total.ToString("C2", culturaEuro)
                })
                .ToList();

            decimal totalGeneral = resumen.Sum(r => r.Total);
            string piePagina = $"Total general: {totalGeneral.ToString("C2", culturaEuro)}   —   {resumen.Count} transacción(es)";

            // Ruta del logo: se copia junto al .exe, dentro de la carpeta Resources
            // (igual que ya haces con tu otra imagen rtgeth.png)
            string rutaLogo = System.IO.Path.Combine(Application.StartupPath, "Resources", "logo_ticket.jpg");

            using (SaveFileDialog dialogo = new SaveFileDialog())
            {
                dialogo.Filter = "Documento PDF (*.pdf)|*.pdf";
                dialogo.FileName = "Historial_Transacciones_" + DateTime.Now.ToString("yyyyMMdd") + ".pdf";

                if (dialogo.ShowDialog(this) != DialogResult.OK)
                {
                    return;
                }

                try
                {
                    PdfExportHelper.ExportarTablaPdf(
                        dialogo.FileName,
                        "Historial resumen de transacciones",
                        encabezados,
                        anchosColumnas,
                        filas,
                        piePagina,
                        rutaLogo,
                        "StockWise");

                    var abrir = MessageBox.Show(
                        $"Se exportaron {resumen.Count} transacción(es) a:\n{dialogo.FileName}\n\n¿Quieres abrir el archivo ahora?",
                        "Exportación completada", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                    if (abrir == DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start(dialogo.FileName);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al exportar: " + ex.Message, "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


    }
}
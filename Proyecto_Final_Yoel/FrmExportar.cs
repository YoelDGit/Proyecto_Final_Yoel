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
                    d.PrecioUnitario * d.Cantidad
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
    }
}
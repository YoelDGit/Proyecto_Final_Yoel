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
    public partial class FormTransacciones : Form
    {
        private ConexionDBDataContext db = new ConexionDBDataContext();

        // Cliente actualmente seleccionado para la venta en curso
        private string clienteSeleccionadoId = null;

        // Carrito de la venta en curso (aún no guardado en la base de datos)
        private BindingList<ItemCarrito> carrito = new BindingList<ItemCarrito>();

        // --- Igual que arriba, pero para la pestaña "Devueltos" ---
        private string clienteDevolucionId = null;
        private BindingList<ItemCarrito> carritoDevolucion = new BindingList<ItemCarrito>();

        public FormTransacciones()
        {
            InitializeComponent();
            this.AutoScaleMode = AutoScaleMode.Inherit;
        }

        private void FormTransacciones_Load(object sender, EventArgs e)
        {
            EstiloModerno.AplicarTema(this);

            textBox2.ReadOnly = true; // Nombre del cliente: solo lectura, viene de la búsqueda
            textBox3.ReadOnly = true; // Apellido del cliente: solo lectura

            textBox7.ReadOnly = true; // Nombre del cliente (Devueltos)
            textBox5.ReadOnly = true; // Apellido del cliente (Devueltos)

            ConfigurarGrids();
            CargarProductosDisponibles();
        }

        // ---------- CONFIGURACIÓN DE GRIDS ----------

        private void ConfigurarGrids()
        {
            dataGridView1.AutoGenerateColumns = true;
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.MultiSelect = false;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            dataGridView2.AutoGenerateColumns = true;
            dataGridView2.ReadOnly = true;
            dataGridView2.AllowUserToAddRows = false;
            dataGridView2.MultiSelect = false;
            dataGridView2.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView2.DataSource = carrito;

            // --- Devueltos ---
            dataGridView4.AutoGenerateColumns = true;
            dataGridView4.ReadOnly = true;
            dataGridView4.AllowUserToAddRows = false;
            dataGridView4.MultiSelect = false;
            dataGridView4.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            dataGridView3.AutoGenerateColumns = true;
            dataGridView3.ReadOnly = true;
            dataGridView3.AllowUserToAddRows = false;
            dataGridView3.MultiSelect = false;
            dataGridView3.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView3.DataSource = carritoDevolucion;
        }

        // Lista de productos disponibles (columna izquierda / superior), con búsqueda opcional
        private void CargarProductosDisponibles(string filtro = "")
        {
            var query = db.Stock.AsQueryable();

            if (!string.IsNullOrWhiteSpace(filtro))
            {
                query = query.Where(i => i.Nombre.Contains(filtro) || i.IdStock.Contains(filtro));
            }

            dataGridView1.DataSource = query
                .OrderBy(i => i.IdStock)
                .Select(i => new
                {
                    i.IdStock,
                    i.Nombre,
                    i.Descripcion,
                    i.Precio,
                    CantidadDisponible = i.Cantidad
                })
                .ToList();
        }

        // ---------- BUSCAR CLIENTE ----------

        private void button1_Click(object sender, EventArgs e) // Buscar (cliente)
        {
            string idInput = textBox1.Text.Trim();

            if (string.IsNullOrWhiteSpace(idInput))
            {
                MessageBox.Show("Escribe un Cliente ID para buscar.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Permite buscar tanto "115" como "0000115": si es numérico, lo
            // rellenamos con ceros para que coincida con el formato CHAR(7).
            string idBuscado = idInput;
            if (int.TryParse(idInput, out int idNumero))
            {
                idBuscado = idNumero.ToString().PadLeft(7, '0');
            }

            var cliente = db.Clientes.FirstOrDefault(c => c.IdCliente == idBuscado);

            if (cliente == null)
            {
                MessageBox.Show("No se encontró ningún cliente con ese ID.", "Cliente no encontrado",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                clienteSeleccionadoId = null;
                textBox2.Clear();
                textBox3.Clear();
                return;
            }

            clienteSeleccionadoId = cliente.IdCliente;
            textBox1.Text = cliente.IdCliente;
            textBox2.Text = cliente.Nombre;
            textBox3.Text = cliente.Apellidos;
        }

        // ---------- BUSCAR PRODUCTO ----------

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            CargarProductosDisponibles(textBox6.Text.Trim());
        }

        // ---------- AGREGAR ÍTEM AL CARRITO ----------

        private void button6_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Selecciona un producto de la lista.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string idItem = dataGridView1.CurrentRow.Cells["IdStock"].Value.ToString();
            string nombre = dataGridView1.CurrentRow.Cells["Nombre"].Value.ToString();
            decimal precio = Convert.ToDecimal(dataGridView1.CurrentRow.Cells["Precio"].Value);
            int disponible = Convert.ToInt32(dataGridView1.CurrentRow.Cells["CantidadDisponible"].Value);

            var enCarrito = carrito.FirstOrDefault(c => c.IdItem == idItem);

            int cantidadIntroducida;
            if (!int.TryParse(textBox9.Text, out cantidadIntroducida) || cantidadIntroducida <= 0)
            {
                // Si usuario no introduce cantidad válida, por defecto 1
                cantidadIntroducida = 1;
            }

            int cantidadYaEnCarrito = enCarrito?.Cantidad ?? 0;

            if (cantidadYaEnCarrito + cantidadIntroducida > disponible)
            {
                MessageBox.Show($"No hay suficiente stock. Disponible: {disponible}.", "Sin stock",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (enCarrito != null)
            {
                enCarrito.Cantidad += cantidadIntroducida;
            }
            else
            {
                carrito.Add(new ItemCarrito
                {
                    IdItem = idItem,
                    Nombre = nombre,
                    Cantidad = cantidadIntroducida,
                    PrecioUnitario = precio
                });
            }

            dataGridView2.Refresh();
        }

        // ---------- ELIMINAR ÍTEM DEL CARRITO (antes de guardar) ----------

        private void button3_Click(object sender, EventArgs e) // Eliminar
        {
            if (dataGridView2.CurrentRow == null)
            {
                MessageBox.Show("Selecciona un ítem del carrito para quitarlo.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var item = (ItemCarrito)dataGridView2.CurrentRow.DataBoundItem;
            carrito.Remove(item);
        }

        // ---------- VER LISTA (resumen del carrito actual) ----------

        private void button2_Click(object sender, EventArgs e) // Ver Lista
        {
            if (carrito.Count == 0)
            {
                MessageBox.Show("El carrito está vacío.", "Lista de productos añadidos",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            decimal total = carrito.Sum(c => c.Subtotal);
            MessageBox.Show($"{carrito.Count} producto(s) en la venta.\nTotal: {total:C2}",
                "Lista de productos añadidos", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // ---------- GUARDAR LA VENTA ----------

        private void button5_Click(object sender, EventArgs e) // Guardar
        {
            if (string.IsNullOrWhiteSpace(clienteSeleccionadoId))
            {
                MessageBox.Show("Busca y selecciona un cliente antes de guardar.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (carrito.Count == 0)
            {
                MessageBox.Show("Añade al menos un producto antes de guardar.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                Transacciones nuevaTransaccion = new Transacciones
                {
                    IdCliente = clienteSeleccionadoId,
                    Fecha = DateTime.Now,
                    Tipo = "Salida"
                };

                db.Transacciones.InsertOnSubmit(nuevaTransaccion);
                db.SubmitChanges(); // Necesario ya aquí para obtener el IdTransaccion autogenerado

                foreach (var item in carrito)
                {
                    DetalleTransaccion detalle = new DetalleTransaccion
                    {
                        IdTransaccion = nuevaTransaccion.IdTransaccion,
                        IdItem = item.IdItem,
                        Cantidad = item.Cantidad,
                        PrecioUnitario = item.PrecioUnitario
                    };
                    db.DetalleTransaccion.InsertOnSubmit(detalle);
                }

                db.SubmitChanges(); // Aquí se disparan los triggers que descuentan el stock

                MessageBox.Show("Venta guardada correctamente.", "Éxito",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                LimpiarTodo();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar la venta: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ---------- SALIR / LIMPIAR ----------

        private void button4_Click(object sender, EventArgs e) // Salir
        {
            LimpiarTodo();
        }

        private void LimpiarTodo()
        {
            clienteSeleccionadoId = null;
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            carrito.Clear();
            CargarProductosDisponibles();
        }

        // ================== PESTAÑA "DEVUELTOS" ==================
        //
        // Solo se puede devolver lo que el cliente ya compró (Tipo="Salida")
        // y que no haya devuelto ya antes. Por eso dataGridView4 no muestra
        // todo el Stock (como en Salidas), sino solo lo que ESE cliente
        // tiene pendiente de devolver.

        // ---------- BUSCAR CLIENTE (Devueltos) ----------

        private void button12_Click(object sender, EventArgs e) // Buscar (cliente, Devueltos)
        {
            string idInput = textBox8.Text.Trim();

            if (string.IsNullOrWhiteSpace(idInput))
            {
                MessageBox.Show("Escribe un Cliente ID para buscar.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string idBuscado = idInput;
            if (int.TryParse(idInput, out int idNumero))
            {
                idBuscado = idNumero.ToString().PadLeft(7, '0');
            }

            var cliente = db.Clientes.FirstOrDefault(c => c.IdCliente == idBuscado);

            if (cliente == null)
            {
                MessageBox.Show("No se encontró ningún cliente con ese ID.", "Cliente no encontrado",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                clienteDevolucionId = null;
                textBox7.Clear();
                textBox5.Clear();
                dataGridView4.DataSource = null;
                return;
            }

            clienteDevolucionId = cliente.IdCliente;
            textBox8.Text = cliente.IdCliente;
            textBox7.Text = cliente.Nombre;
            textBox5.Text = cliente.Apellidos;

            CargarProductosDevolvibles(clienteDevolucionId);
        }

        // Solo productos que el cliente compró y aún no ha devuelto del todo
        private void CargarProductosDevolvibles(string idCliente, string filtro = "")
        {
            if (string.IsNullOrWhiteSpace(idCliente))
            {
                dataGridView4.DataSource = null;
                return;
            }

            var comprado = db.DetalleTransaccion
                .Where(d => d.Transacciones.IdCliente == idCliente && d.Transacciones.Tipo == "Salida")
                .GroupBy(d => d.IdItem)
                .Select(g => new { IdItem = g.Key, Cantidad = g.Sum(x => x.Cantidad) })
                .ToList();

            var devuelto = db.DetalleTransaccion
                .Where(d => d.Transacciones.IdCliente == idCliente && d.Transacciones.Tipo == "Devolucion")
                .GroupBy(d => d.IdItem)
                .Select(g => new { IdItem = g.Key, Cantidad = g.Sum(x => x.Cantidad) })
                .ToList();

            var idsComprados = comprado.Select(c => c.IdItem).ToList();
            var infoItems = db.Stock.Where(s => idsComprados.Contains(s.IdStock)).ToList();

            var disponibles = comprado
                .Select(c => new
                {
                    c.IdItem,
                    CantidadDisponibleParaDevolver = c.Cantidad - (devuelto.FirstOrDefault(d => d.IdItem == c.IdItem)?.Cantidad ?? 0)
                })
                .Where(x => x.CantidadDisponibleParaDevolver > 0)
                .Join(infoItems, x => x.IdItem, s => s.IdStock, (x, s) => new
                {
                    s.IdStock,
                    s.Nombre,
                    s.Precio,
                    x.CantidadDisponibleParaDevolver
                });

            if (!string.IsNullOrWhiteSpace(filtro))
            {
                disponibles = disponibles.Where(d => d.Nombre.Contains(filtro) || d.IdStock.Contains(filtro));
            }

            dataGridView4.DataSource = disponibles.OrderBy(d => d.IdStock).ToList();
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            CargarProductosDevolvibles(clienteDevolucionId, textBox4.Text.Trim());
        }

        // ---------- AGREGAR ÍTEM AL CARRITO DE DEVOLUCIÓN ----------

        private void button11_Click(object sender, EventArgs e) // Agregar (Devueltos)
        {
            if (dataGridView4.CurrentRow == null)
            {
                MessageBox.Show("Selecciona un producto de la lista.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string idItem = dataGridView4.CurrentRow.Cells["IdStock"].Value.ToString();
            string nombre = dataGridView4.CurrentRow.Cells["Nombre"].Value.ToString();
            decimal precio = Convert.ToDecimal(dataGridView4.CurrentRow.Cells["Precio"].Value);
            int disponibleParaDevolver = Convert.ToInt32(dataGridView4.CurrentRow.Cells["CantidadDisponibleParaDevolver"].Value);

            var enCarrito = carritoDevolucion.FirstOrDefault(c => c.IdItem == idItem);
            int yaEnCarrito = enCarrito?.Cantidad ?? 0;

            if (yaEnCarrito + 1 > disponibleParaDevolver)
            {
                MessageBox.Show($"No puedes devolver más de lo comprado. Disponible para devolver: {disponibleParaDevolver}.",
                    "Cantidad excedida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (enCarrito != null)
            {
                enCarrito.Cantidad += 1;
                dataGridView3.Refresh();
            }
            else
            {
                carritoDevolucion.Add(new ItemCarrito
                {
                    IdItem = idItem,
                    Nombre = nombre,
                    Cantidad = 1,
                    PrecioUnitario = precio
                });
            }
        }

        // ---------- ELIMINAR ÍTEM DEL CARRITO DE DEVOLUCIÓN ----------

        private void button8_Click(object sender, EventArgs e) // Eliminar (Devueltos)
        {
            if (dataGridView3.CurrentRow == null)
            {
                MessageBox.Show("Selecciona un ítem de la lista para quitarlo.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var item = (ItemCarrito)dataGridView3.CurrentRow.DataBoundItem;
            carritoDevolucion.Remove(item);
        }

        // ---------- VER LISTA (Devueltos) ----------

        private void button9_Click(object sender, EventArgs e) // Ver Lista (Devueltos)
        {
            if (carritoDevolucion.Count == 0)
            {
                MessageBox.Show("No hay productos en la devolución.", "Lista de productos a devolver",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            decimal total = carritoDevolucion.Sum(c => c.Subtotal);
            MessageBox.Show($"{carritoDevolucion.Count} producto(s) a devolver.\nTotal a reembolsar: {total:C2}",
                "Lista de productos a devolver", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // ---------- GUARDAR LA DEVOLUCIÓN ----------

        private void button10_Click(object sender, EventArgs e) // Guardar (Devueltos)
        {
            if (string.IsNullOrWhiteSpace(clienteDevolucionId))
            {
                MessageBox.Show("Busca y selecciona un cliente antes de guardar.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (carritoDevolucion.Count == 0)
            {
                MessageBox.Show("Añade al menos un producto a devolver.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                Transacciones nuevaDevolucion = new Transacciones
                {
                    IdCliente = clienteDevolucionId,
                    Fecha = DateTime.Now,
                    Tipo = "Devolucion"
                };

                db.Transacciones.InsertOnSubmit(nuevaDevolucion);
                db.SubmitChanges(); // Necesario para obtener el IdTransaccion autogenerado

                foreach (var item in carritoDevolucion)
                {
                    DetalleTransaccion detalle = new DetalleTransaccion
                    {
                        IdTransaccion = nuevaDevolucion.IdTransaccion,
                        IdItem = item.IdItem,
                        Cantidad = item.Cantidad,
                        PrecioUnitario = item.PrecioUnitario
                    };
                    db.DetalleTransaccion.InsertOnSubmit(detalle);
                }

                db.SubmitChanges(); // El trigger (ya corregido, ver LEEME) SUMA el stock de vuelta

                MessageBox.Show("Devolución guardada correctamente.", "Éxito",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                LimpiarDevolucion();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar la devolución: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ---------- SALIR / LIMPIAR (Devueltos) ----------

        private void button7_Click(object sender, EventArgs e) // Salir (Devueltos)
        {
            LimpiarDevolucion();
        }

        private void LimpiarDevolucion()
        {
            clienteDevolucionId = null;
            textBox8.Clear();
            textBox7.Clear();
            textBox5.Clear();
            carritoDevolucion.Clear();
            dataGridView4.DataSource = null;
        }
    }

    // Clase auxiliar: representa cada línea del carrito antes de guardarla en la base de datos
    public class ItemCarrito
    {
        public string IdItem { get; set; }
        public string Nombre { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal => Cantidad * PrecioUnitario;
    }
}

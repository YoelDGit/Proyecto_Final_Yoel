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
    public partial class Cliente : Form
    {
        // Contexto de datos: la "puerta de entrada" para consultar y guardar en SQL Server
        private ConexionDBDataContext db = new ConexionDBDataContext();

        public Cliente()
        {
            InitializeComponent();

            // FUERZA AL HIJO A HEREDAR LA ESCALA DEL PANEL DE LA PÁGINA PRINCIPAL
            this.AutoScaleMode = AutoScaleMode.Inherit;
        }

        private void Cliente_Load(object sender, EventArgs e)
        {
            // ¡La magia ocurre aquí! Pasa este formulario como parámetro
            EstiloModerno.AplicarTema(this);

            // textBox1 (Cliente ID) nunca se edita a mano: lo generamos nosotros
            textBox1.ReadOnly = true;

            ConfigurarGrid();
            CargarClientes();
            PrepararNuevoRegistro();
        }

        // ---------- CARGA Y VISUALIZACIÓN ----------

        private void ConfigurarGrid()
        {
            dataGridView1.AutoGenerateColumns = true;
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.MultiSelect = false;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            
        }

        // Recarga el grid desde la base de datos, opcionalmente filtrado por texto de búsqueda
        private void CargarClientes(string filtro = "")
        {
            var query = db.Clientes.AsQueryable();

            if (!string.IsNullOrWhiteSpace(filtro))
            {
                query = query.Where(c =>
                    c.Nombre.Contains(filtro) ||
                    c.Apellidos.Contains(filtro) ||
                    c.IdCliente.Contains(filtro));
            }

            dataGridView1.DataSource = query
                .OrderBy(c => c.IdCliente)
                .Select(c => new
                {
                    c.IdCliente,
                    c.Nombre,
                    c.Apellidos,
                    c.Direccion,
                    c.Telefono
                })
                .ToList();
        }

        // Al hacer clic en una fila del grid, cargamos esos datos en el formulario para editar
        private void DataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null) return;

            var fila = dataGridView1.CurrentRow;

            textBox1.Text = fila.Cells["IdCliente"].Value?.ToString();
            textBox2.Text = fila.Cells["Nombre"].Value?.ToString();
            textBox3.Text = fila.Cells["Direccion"].Value?.ToString();
            textBox4.Text = fila.Cells["Apellidos"].Value?.ToString();
            textBox5.Text = fila.Cells["Telefono"].Value?.ToString();
        }

        // ---------- GENERACIÓN DE ID (CHAR(7) con ceros a la izquierda) ----------

        private string GenerarNuevoId()
        {
            // Cogemos todos los Id existentes, los convertimos a número y buscamos el mayor
            var maxId = db.Clientes
                .AsEnumerable()
                .Select(c => int.TryParse(c.IdCliente, out int n) ? n : 0)
                .DefaultIfEmpty(0)
                .Max();

            int siguiente = maxId + 1;
            return siguiente.ToString().PadLeft(7, '0'); // ej: 0000115
        }

        private void PrepararNuevoRegistro()
        {
            textBox1.Text = GenerarNuevoId();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
        }

        // ---------- VALIDACIÓN ----------

        private bool DatosValidos()
        {
            if (string.IsNullOrWhiteSpace(textBox2.Text) || string.IsNullOrWhiteSpace(textBox4.Text))
            {
                MessageBox.Show("El nombre y los apellidos son obligatorios.", "Campos incompletos",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!string.IsNullOrWhiteSpace(textBox5.Text) &&
                (!textBox5.Text.All(char.IsDigit) || textBox5.Text.Length != 9))
            {
                MessageBox.Show("El teléfono debe tener 9 dígitos numéricos (o dejarse vacío).", "Teléfono inválido",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        // ---------- BOTONES: AGREGAR / MODIFICAR / ELIMINAR / SALIR ----------

        private void button1_Click(object sender, EventArgs e) // Agregar
        {
            if (!DatosValidos()) return;

            try
            {
                Clientes nuevo = new Clientes
                {
                    IdCliente = textBox1.Text,
                    Nombre = textBox2.Text.Trim(),
                    Apellidos = textBox4.Text.Trim(),
                    Direccion = textBox3.Text.Trim(),
                    Telefono = textBox5.Text.Trim()
                };

                db.Clientes.InsertOnSubmit(nuevo);
                db.SubmitChanges();

                MessageBox.Show("Cliente añadido correctamente.", "Éxito",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                CargarClientes();
                PrepararNuevoRegistro();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al añadir el cliente: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e) // Modificar
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("Selecciona primero un cliente de la lista.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!DatosValidos()) return;

            try
            {
                var cliente = db.Clientes.FirstOrDefault(c => c.IdCliente == textBox1.Text);

                if (cliente == null)
                {
                    MessageBox.Show("Ese cliente ya no existe en la base de datos.", "Aviso",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                cliente.Nombre = textBox2.Text.Trim();
                cliente.Apellidos = textBox4.Text.Trim();
                cliente.Direccion = textBox3.Text.Trim();
                cliente.Telefono = textBox5.Text.Trim();

                db.SubmitChanges();

                MessageBox.Show("Cliente actualizado correctamente.", "Éxito",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                CargarClientes();
                PrepararNuevoRegistro();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al modificar el cliente: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button3_Click(object sender, EventArgs e) // Eliminar
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text) || dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Selecciona primero un cliente de la lista.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirmar = MessageBox.Show(
                $"¿Seguro que quieres eliminar al cliente {textBox2.Text} {textBox4.Text}?",
                "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirmar != DialogResult.Yes) return;

            try
            {
                var cliente = db.Clientes.FirstOrDefault(c => c.IdCliente == textBox1.Text);

                if (cliente == null)
                {
                    MessageBox.Show("Ese cliente ya no existe en la base de datos.", "Aviso",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                db.Clientes.DeleteOnSubmit(cliente);
                db.SubmitChanges();

                MessageBox.Show("Cliente eliminado.", "Éxito",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                CargarClientes();
                PrepararNuevoRegistro();
            }
            catch (Exception ex)
            {
                // Si el cliente tiene transacciones asociadas, SQL Server rechazará el borrado
                // por la clave foránea (integridad referencial) — esto es correcto y esperado.
                MessageBox.Show("No se pudo eliminar: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button4_Click(object sender, EventArgs e) // Salir / Limpiar formulario
        {
            // Al estar embebido dentro de Pagina_Principal (no es un formulario de nivel superior),
            // "Salir" limpia el formulario en vez de cerrar la ventana.
            PrepararNuevoRegistro();
            dataGridView1.ClearSelection();
        }

        // ---------- BÚSQUEDA ----------

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            CargarClientes(textBox6.Text.Trim());
        }

        // ---------- Eventos vacíos ya existentes en el diseñador (los dejamos, sin lógica) ----------

        private void label1_Click(object sender, EventArgs e)
        {
        }

        private void label2_Click(object sender, EventArgs e)
        {
        }

        private void label7_Click(object sender, EventArgs e)
        {
        }
    }
}

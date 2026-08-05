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
        private ConexionDBDataContext db = new ConexionDBDataContext();

        public FrmStock()
        {
            InitializeComponent();

            // FUERZA AL HIJO A HEREDAR LA ESCALA DEL PANEL DE LA PÁGINA PRINCIPAL
            this.AutoScaleMode = AutoScaleMode.Inherit;
        }

        private void FrmStock_Load(object sender, EventArgs e)
        {
            // ¡La magia ocurre aquí! Pasa este formulario como parámetro
            EstiloModerno.AplicarTema(this);

            textBox1.ReadOnly = true; // Item ID: autogenerado, nunca a mano

            ConfigurarGrid();
            CargarCategorias();
            CargarStock();
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
            dataGridView1.SelectionChanged += DataGridView1_SelectionChanged;
        }

        // Llena el combo de categorías desde la tabla Categorias
        private void CargarCategorias()
        {
            var categorias = db.Categorias.OrderBy(c => c.Nombre).ToList();

            comboBox2.DataSource = categorias;
            comboBox2.DisplayMember = "Nombre";
            comboBox2.ValueMember = "IdCategoria";
            comboBox2.SelectedIndex = -1; // arranca sin selección
        }

        private void CargarStock(string filtro = "")
        {
            var query = db.Stock.AsQueryable();

            if (!string.IsNullOrWhiteSpace(filtro))
            {
                query = query.Where(i =>
                    i.Nombre.Contains(filtro) ||
                    i.IdStock.Contains(filtro) ||
                    i.Descripcion.Contains(filtro));
            }

            dataGridView1.DataSource = query
                .OrderBy(i => i.IdStock)
                .Select(i => new
                {
                    i.IdStock,
                    i.Nombre,
                    i.Descripcion,
                    Categoria = i.Categorias.Nombre,
                    i.Precio,
                    i.Cantidad
                })
                .ToList();
        }

        private void DataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null) return;

            string id = dataGridView1.CurrentRow.Cells["IdStock"].Value?.ToString();
            var item = db.Stock.FirstOrDefault(i => i.IdStock == id);
            if (item == null) return;

            textBox1.Text = item.IdStock;
            textBox2.Text = item.Nombre;
            textBox3.Text = item.Descripcion;
            textBox5.Text = item.Precio.ToString();
            textBox4.Text = item.Cantidad.ToString();
            comboBox2.SelectedValue = item.IdCategoria ?? 0;
        }

        // ---------- GENERACIÓN DE ID (ej: A000015) ----------

        private string GenerarNuevoId()
        {
            var maxNumero = db.Stock
                .AsEnumerable()
                .Select(i =>
                {
                    string soloNumeros = new string(i.IdStock.Where(char.IsDigit).ToArray());
                    return int.TryParse(soloNumeros, out int n) ? n : 0;
                })
                .DefaultIfEmpty(0)
                .Max();

            int siguiente = maxNumero + 1;
            return "A" + siguiente.ToString().PadLeft(6, '0'); // ej: A000015
        }

        private void PrepararNuevoRegistro()
        {
            textBox1.Text = GenerarNuevoId();
            textBox2.Clear();
            textBox3.Clear();
            textBox5.Clear();
            textBox4.Clear();
            comboBox2.SelectedIndex = -1;
        }

        // ---------- VALIDACIÓN ----------

        private bool DatosValidos(out decimal precio, out int cantidad)
        {
            precio = 0;
            cantidad = 0;

            if (string.IsNullOrWhiteSpace(textBox2.Text))
            {
                MessageBox.Show("El nombre del ítem es obligatorio.", "Campos incompletos",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (comboBox2.SelectedValue == null)
            {
                MessageBox.Show("Selecciona una categoría.", "Campos incompletos",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!decimal.TryParse(textBox5.Text, out precio) || precio < 0)
            {
                MessageBox.Show("El precio debe ser un número válido.", "Precio inválido",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!int.TryParse(textBox4.Text, out cantidad) || cantidad < 0)
            {
                MessageBox.Show("La cantidad debe ser un número entero válido.", "Cantidad inválida",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        // ---------- BOTONES: AGREGAR / MODIFICAR / ELIMINAR / CERRAR ----------

        private void button5_Click(object sender, EventArgs e) // Agregar
        {
            if (!DatosValidos(out decimal precio, out int cantidad)) return;

            try
            {
                Stock nuevo = new Stock
                {
                    IdStock = textBox1.Text,
                    Nombre = textBox2.Text.Trim(),
                    Descripcion = textBox3.Text.Trim(),
                    IdCategoria = (int)comboBox2.SelectedValue,
                    Precio = precio,
                    Cantidad = cantidad
                };

                db.Stock.InsertOnSubmit(nuevo);
                db.SubmitChanges();

                MessageBox.Show("Ítem añadido correctamente.", "Éxito",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                CargarStock();
                PrepararNuevoRegistro();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al añadir el ítem: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e) // Modificar
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("Selecciona primero un ítem de la lista.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!DatosValidos(out decimal precio, out int cantidad)) return;

            try
            {
                var item = db.Stock.FirstOrDefault(i => i.IdStock == textBox1.Text);

                if (item == null)
                {
                    MessageBox.Show("Ese ítem ya no existe en la base de datos.", "Aviso",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                item.Nombre = textBox2.Text.Trim();
                item.Descripcion = textBox3.Text.Trim();
                item.IdCategoria = (int)comboBox2.SelectedValue;
                item.Precio = precio;
                item.Cantidad = cantidad;

                db.SubmitChanges();

                MessageBox.Show("Ítem actualizado correctamente.", "Éxito",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                CargarStock();
                PrepararNuevoRegistro();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al modificar el ítem: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e) // Eliminar
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text) || dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Selecciona primero un ítem de la lista.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirmar = MessageBox.Show(
                $"¿Seguro que quieres eliminar el ítem {textBox2.Text}?",
                "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirmar != DialogResult.Yes) return;

            try
            {
                var item = db.Stock.FirstOrDefault(i => i.IdStock == textBox1.Text);

                if (item == null)
                {
                    MessageBox.Show("Ese ítem ya no existe en la base de datos.", "Aviso",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                db.Stock.DeleteOnSubmit(item);
                db.SubmitChanges();

                MessageBox.Show("Ítem eliminado.", "Éxito",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                CargarStock();
                PrepararNuevoRegistro();
            }
            catch (Exception ex)
            {
                // Si el ítem tiene ventas asociadas (DetalleTransaccion), SQL Server
                // rechazará el borrado por la clave foránea - esto es correcto y esperado.
                MessageBox.Show("No se pudo eliminar: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button3_Click(object sender, EventArgs e) // Cerrar / Limpiar formulario
        {
            // Al estar embebido dentro de Pagina_Principal (no es una ventana independiente),
            // "Cerrar" limpia el formulario en vez de cerrar la ventana.
            PrepararNuevoRegistro();
            dataGridView1.ClearSelection();
        }

        // ---------- BÚSQUEDA ----------

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            CargarStock(textBox6.Text.Trim());
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}

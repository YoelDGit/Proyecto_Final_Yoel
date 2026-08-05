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
    // NOTA: este formulario se llamaba "Categorias", igual que la clase que genera
    // LINQ to SQL para la tabla Categorias (en ConexionDB.designer.cs). Dos clases
    // no pueden compartir nombre en el mismo namespace, así que lo renombré a
    // FrmCategorias (siguiendo el mismo patrón que ya usabas en FrmStock).
    public partial class FrmCategorias : Form
    {
        private ConexionDBDataContext db = new ConexionDBDataContext();

        public FrmCategorias()
        {
            InitializeComponent();

            // Igual que en Cliente/FrmStock: hereda la escala del panel que lo contiene
            this.AutoScaleMode = AutoScaleMode.Inherit;
        }

        private void FrmCategorias_Load(object sender, EventArgs e)
        {
            EstiloModerno.AplicarTema(this);
            CargarCategorias();
        }

        // ---------- CARGA ----------

        private void CargarCategorias()
        {
            var categorias = db.Categorias.OrderBy(c => c.Nombre).ToList();

            comboBox1.DataSource = categorias;
            comboBox1.DisplayMember = "Nombre";
            comboBox1.ValueMember = "IdCategoria";
            comboBox1.SelectedIndex = -1;

            textBox2.Clear();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem == null) return;

            var seleccionada = (Categorias)comboBox1.SelectedItem;
            textBox2.Text = seleccionada.Nombre;
        }

        // ---------- BOTONES: AGREGAR / MODIFICAR / ELIMINAR ----------

        private void button5_Click(object sender, EventArgs e) // Agregar
        {
            string nombre = textBox2.Text.Trim();

            if (string.IsNullOrWhiteSpace(nombre))
            {
                MessageBox.Show("Escribe un nombre de categoría.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            bool yaExiste = db.Categorias.Any(c => c.Nombre.ToLower() == nombre.ToLower());
            if (yaExiste)
            {
                MessageBox.Show("Ya existe una categoría con ese nombre.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                Categorias nueva = new Categorias { Nombre = nombre };
                db.Categorias.InsertOnSubmit(nueva);
                db.SubmitChanges();

                MessageBox.Show("Categoría añadida.", "Éxito",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                CargarCategorias();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al añadir la categoría: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e) // Modificar
        {
            if (comboBox1.SelectedValue == null)
            {
                MessageBox.Show("Selecciona primero una categoría de la lista.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string nuevoNombre = textBox2.Text.Trim();
            if (string.IsNullOrWhiteSpace(nuevoNombre))
            {
                MessageBox.Show("Escribe un nombre de categoría.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                int id = (int)comboBox1.SelectedValue;
                var categoria = db.Categorias.FirstOrDefault(c => c.IdCategoria == id);

                if (categoria == null)
                {
                    MessageBox.Show("Esa categoría ya no existe.", "Aviso",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                categoria.Nombre = nuevoNombre;
                db.SubmitChanges();

                MessageBox.Show("Categoría actualizada.", "Éxito",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                CargarCategorias();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al modificar la categoría: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e) // Eliminar
        {
            if (comboBox1.SelectedValue == null)
            {
                MessageBox.Show("Selecciona primero una categoría de la lista.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirmar = MessageBox.Show(
                $"¿Seguro que quieres eliminar la categoría \"{textBox2.Text}\"?",
                "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirmar != DialogResult.Yes) return;

            try
            {
                int id = (int)comboBox1.SelectedValue;
                var categoria = db.Categorias.FirstOrDefault(c => c.IdCategoria == id);

                if (categoria == null)
                {
                    MessageBox.Show("Esa categoría ya no existe.", "Aviso",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                db.Categorias.DeleteOnSubmit(categoria);
                db.SubmitChanges();

                MessageBox.Show("Categoría eliminada.", "Éxito",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                CargarCategorias();
            }
            catch (Exception ex)
            {
                // Si hay ítems de Stock usando esta categoría, SQL Server rechaza el
                // borrado por la clave foránea - correcto y esperado.
                MessageBox.Show(
                    "No se pudo eliminar: probablemente hay ítems de Stock usando esta categoría.\n\n" + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

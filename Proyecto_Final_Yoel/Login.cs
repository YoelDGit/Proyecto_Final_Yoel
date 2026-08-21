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
    public partial class Login : Form
    {
        // Se activa justo antes de un cierre LEGÍTIMO (login correcto, o el
        // usuario ya confirmó salir por otra vía). Login_FormClosing solo
        // pregunta "¿seguro que quieres salir?" cuando esta bandera está en false.
        private bool salidaAutorizada = false;

        public Login()
        {
            InitializeComponent();
            this.FormClosing += Login_FormClosing;
            InicializarTimer();
        }

        ConexionDBDataContext ConexionData = new ConexionDBDataContext();
        private Timer timer;

        private void Login_Load(object sender, EventArgs e)
        {
            // ¡La magia ocurre aquí! Pasa este formulario como parámetro
            EstiloModerno.AplicarTema(this);
            Idiomas.AplicarIdioma(this);

            comboIdioma.SelectedIndex = (int)Idiomas.IdiomaActual;
        }

        private void comboIdioma_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboIdioma.SelectedIndex < 0) return;

            IdiomaApp seleccionado = (IdiomaApp)comboIdioma.SelectedIndex;

            if (seleccionado != Idiomas.IdiomaActual)
            {
                Idiomas.CambiarIdioma(seleccionado);
            }
        }

        private void InicializarTimer()
        {
            // Crear e inicializar el Timer
            timer = new Timer();
            timer.Interval = 1000; // 1 segundo
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            try
            {
                // Mostrar fecha y hora por separado
                LabelFecha.Text = DateTime.Now.ToString("dd/MM/yyyy"); // Formato de fecha
                LabelTime.Text = DateTime.Now.ToString("HH:mm:ss");    // Formato de hora 24h
            }
            catch (Exception ex)
            {
                // Manejo de errores (opcional)
                timer.Stop();
                MessageBox.Show("Error al actualizar fecha/hora: " + ex.Message);
            }
        }

        // NOTA: este método existía con un usuario/contraseña fijos en el código
        // ("admin"/"1234"), a modo de acceso paralelo que no pasaba por la
        // validación real ni por SesionActual. Ahora simplemente redirige al
        // botón de login de verdad, por si algún control viejo sigue llamando
        // a este método - así queda inutilizado como "puerta trasera" sin
        // arriesgarnos a tocar el Designer, que no tenemos delante.
        private void btnLogin_Click(object sender, EventArgs e)
        {
            buttonIniciarLogin_Click(sender, e);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            // 1. Validar que los campos no estén vacíos
            if (string.IsNullOrWhiteSpace(txtUsuario.Text) || string.IsNullOrWhiteSpace(txtContrasena.Text))
            {
                MessageBox.Show("Por favor, rellene todos los campos.", "Campos vacíos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (var db = new ConexionDBDataContext())
                {
                    // 2. Instanciamos el objeto con la 'S' y la 's' del plural de LINQ
                    Inicio_Sesion nuevoUsuario = new Inicio_Sesion();

                    // Asignamos las propiedades
                    nuevoUsuario.Usuario = txtUsuario.Text.Trim();
                    nuevoUsuario.Contrasena = txtContrasena.Text.Trim();
                    nuevoUsuario.Fecha = DateTime.Now.Date;
                    nuevoUsuario.Hora = DateTime.Now.TimeOfDay;
                    nuevoUsuario.EsAdministrador = false; // Los usuarios que se crean aquí son siempre secundarios

                    // 3. Añadimos el objeto a la colección en plural
                    db.Inicio_Sesion.InsertOnSubmit(nuevoUsuario);

                    // 4. Guardamos en la Base de Datos
                    db.SubmitChanges();

                    MessageBox.Show("Usuario creado exitosamente con LINQ.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Limpiamos los campos
                    txtUsuario.Clear();
                    txtContrasena.Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar el registro: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtContrasena_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

            // Si la contraseña está oculta, la mostramos
            if (txtContrasena.UseSystemPasswordChar == true)
            {
                txtContrasena.UseSystemPasswordChar = false;

                // OPCIONAL: Si tienes otra imagen del ojo abierto en tus Recursos, cámbiala aquí:
                // picOjo.Image = Properties.Resources.ojo_abierto; 
            }
            // Si ya se estaba mostrando, la volvemos a ocultar
            else
            {
                txtContrasena.UseSystemPasswordChar = true;

                // picOjo.Image = Properties.Resources.ojo_tachado;
            }

        }

        private void buttonIniciarLogin_Click(object sender, EventArgs e)
        {
            // 1. Validar primero que los campos no estén vacíos en la interfaz
            if (string.IsNullOrWhiteSpace(txtUsuario.Text) || string.IsNullOrWhiteSpace(txtContrasena.Text))
            {
                MessageBox.Show("Por favor, rellene todos los campos.", "Campos vacíos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (var db = new ConexionDBDataContext())
                {
                    string usuarioIntroducido = txtUsuario.Text.Trim();
                    string contrasenaIntroducida = txtContrasena.Text.Trim();

                    // 2. Buscamos el registro completo (no solo si existe) para
                    // poder actualizar su Fecha/Hora de último acceso
                    var usuarioEncontrado = db.Inicio_Sesion.FirstOrDefault(u =>
                        u.Usuario == usuarioIntroducido && u.Contrasena == contrasenaIntroducida);

                    if (usuarioEncontrado != null)
                    {
                        // Registramos el momento exacto de este inicio de sesión
                        usuarioEncontrado.Fecha = DateTime.Now.Date;
                        usuarioEncontrado.Hora = DateTime.Now.TimeOfDay;
                        db.SubmitChanges();

                        // Guardamos quién ha entrado y si es administrador, para
                        // que el resto de la app (Pagina_Principal, FormTransacciones...)
                        // sepa qué puede hacer y qué no
                        SesionActual.IniciarSesion(usuarioEncontrado.Usuario, usuarioEncontrado.EsAdministrador);

                        MessageBox.Show("¡Bienvenido al sistema!", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // 3. Indicamos al Program.cs que la validación fue un éxito
                        this.DialogResult = DialogResult.OK;

                        // Este cierre es legítimo (login correcto): no debe
                        // disparar la pregunta de "¿seguro que quieres salir?"
                        salidaAutorizada = true;

                        // 4. Cerramos este formulario para destruirlo y liberar memoria RAM
                        this.Close();
                    }
                    else
                    {
                        // Si los datos no coinciden
                        MessageBox.Show("Usuario o contraseña incorrectos.", "Error de acceso", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                // Por si falla la comunicación con SQL Server
                MessageBox.Show("Error al conectar con la base de datos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Login_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (salidaAutorizada)
            {
                return; // Es un cierre legítimo (login correcto, o ya confirmado), no preguntamos nada
            }

            if (e.CloseReason == CloseReason.UserClosing)
            {
                var respuesta = MessageBox.Show(
                    "¿Seguro que quieres salir de la aplicación?",
                    "Confirmar salida",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (respuesta == DialogResult.No)
                {
                    e.Cancel = true;
                }
            }
        }

        private void buttonCancelarLogin_Click(object sender, EventArgs e)
        {
            var respuesta = MessageBox.Show(
                "¿Seguro que quieres salir de la aplicación?",
                "Confirmar salida",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (respuesta == DialogResult.Yes)
            {
                // Ya hemos preguntado aquí: que Login_FormClosing no vuelva a preguntar
                salidaAutorizada = true;
                this.Close();
            }
        }
    }
}

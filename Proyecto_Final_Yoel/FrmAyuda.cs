using System;
using System.Drawing;
using System.Windows.Forms;

namespace Proyecto_Final_Yoel
{
    public partial class FrmAyuda : Form
    {
        public FrmAyuda()
        {
            InitializeComponent();
            this.AutoScaleMode = AutoScaleMode.Inherit;
        }

        private void FrmAyuda_Load(object sender, EventArgs e)
        {
            EstiloModerno.AplicarTema(this);

            // El RichTextBox lleva su propio contenido con formato (negrita en
            // los títulos), así que lo dejamos con fondo blanco y texto oscuro
            // fijos, independientemente del tema activo, para que siempre se
            // lea bien.
            rtbManual.BackColor = Color.White;
            rtbManual.ForeColor = Color.Black;

            ConstruirManual();
        }

        private void AppendTitulo(string texto)
        {
            rtbManual.SelectionStart = rtbManual.TextLength;
            rtbManual.SelectionFont = new Font("Segoe UI", 13F, FontStyle.Bold);
            rtbManual.SelectionColor = EstiloModerno.Primario;
            rtbManual.AppendText(texto + "\n");
        }

        private void AppendSubtitulo(string texto)
        {
            rtbManual.SelectionStart = rtbManual.TextLength;
            rtbManual.SelectionFont = new Font("Segoe UI", 10.5F, FontStyle.Bold);
            rtbManual.SelectionColor = Color.Black;
            rtbManual.AppendText(texto + "\n");
        }

        private void AppendTexto(string texto)
        {
            rtbManual.SelectionStart = rtbManual.TextLength;
            rtbManual.SelectionFont = new Font("Segoe UI", 10F, FontStyle.Regular);
            rtbManual.SelectionColor = Color.Black;
            rtbManual.AppendText(texto + "\n\n");
        }

        private void ConstruirManual()
        {
            rtbManual.Clear();

            AppendTitulo("Manual de uso — Sistema de Gestión de Inventarios");
            AppendTexto(
                "Este manual explica cómo usar cada pantalla de la aplicación. " +
                "Puedes desplazarte hacia abajo para ver todos los apartados."
            );

            AppendSubtitulo("1. Iniciar sesión");
            AppendTexto(
                "Al abrir la aplicación se muestra la pantalla de Login. Escribe tu Usuario y " +
                "Contraseña y pulsa \"Iniciar\". Si no tienes una cuenta, puedes crear una nueva " +
                "desde el propio Login \n (las cuentas creadas así son siempre de tipo Usuario " +
                "secundario). Arriba a la derecha puedes elegir el idioma de la aplicación " +
                "(Español, English, Français, Deutsch, Português)."
            );

            AppendSubtitulo("2. Página principal");
            AppendTexto(
                "Tras iniciar sesión verás el menú principal con estos botones: TRANSACCIONES, " +
                "STOCK, CLIENTES, USUARIOS, LOGIN y CONF. Al pulsar cualquiera de ellos, la " +
                "pantalla \n correspondiente se abre dentro de esta misma ventana, sin necesidad de " +
                "abrir ventanas nuevas."
            );

            AppendSubtitulo("3. Clientes");
            AppendTexto(
                "Permite dar de alta, modificar, eliminar y buscar clientes. El Cliente ID se " +
                "genera automáticamente al crear uno nuevo (no se edita a mano). Para modificar " +
                "o eliminar \n un cliente, selecciónalo primero en la lista de abajo; sus datos " +
                "aparecerán en el formulario. El cuadro \"Buscar\" filtra la lista mientras " +
                "escribes, por nombre, apellidos o ID."
            );

            AppendSubtitulo("4. Stock");
            AppendTexto(
                "Gestión de los productos del almacén (alta, modificación, eliminación y " +
                "búsqueda). El Item ID también se \n autogenera. La Categoría se elige de un " +
                "desplegable: si la categoría que \n necesitas no existe todavía, créala primero " +
                "desde CONF → Modificar Categorías."
            );

            AppendSubtitulo("5. Transacciones — pestaña Salidas (ventas)");
            AppendTexto(
                "Paso 1: busca al cliente escribiendo su ID, nombre o apellidos y pulsando " +
                "\"Buscar\". Al encontrarlo, verás su historial de compras y devoluciones en la " +
                "tabla central.\n" +
                "Paso 2: en la lista de productos, selecciona el que quieres vender, escribe la " +
                "cantidad en el cuadro  junto a \"Agregar\" (si lo dejas vacío, se añade 1 " +
                "unidad) \n \t y pulsa \"Agregar\". El producto  pasa al carrito de la venta.\n" +
                "Paso 3: repite el paso 2 con todos los productos que necesites. Puedes quitar " +
                "un producto del carrito con \"Eliminar\", o ver un resumen con \"Ver Lista\".\n" +
                "Paso 4: pulsa \"Guardar\" para completar la venta. El stock de los productos " +
                "vendidos se descuenta automáticamente."
            );

            AppendSubtitulo("6. Transacciones — pestaña Devueltos");
            AppendTexto(
                "Solo visible para usuarios Administrador. Funciona igual que Salidas, pero solo " +
                "permite devolver productos que ese \n cliente ya haya comprado y no haya devuelto " +
                "todavía en su totalidad. Al guardar la devolución, el stock se suma de vuelta " +
                "automáticamente."
            );

            AppendSubtitulo("7. Usuarios");
            AppendTexto(
                "Pantalla de solo consulta: muestra una tarjeta por cada cuenta de la " +
                "aplicación, con \n su rol (Administrador o Usuario secundario) y la fecha/hora de " +
                "su último acceso, de la más reciente a la más antigua."
            );

            AppendSubtitulo("8. Cambiar de usuario (botón LOGIN)");
            AppendTexto(
                "Desde el botón LOGIN del menú principal puedes cambiar de cuenta sin cerrar la " +
                "aplicación. Verás las cuentas \n agrupadas en \"Administradores\" y \"Usuarios " +
                "secundarios\" (la sección de Administradores solo aparece si tu cuenta actual " +
                "ya es administradora). Pulsa sobre \n una tarjeta e introduce su contraseña para " +
                "entrar con ella. Si eres administrador, también verás aquí el botón " +
                "\"+ Crear nuevo administrador\"."
            );

            AppendSubtitulo("9. Configuración");
            AppendTexto(
                "Desde el botón CONF accedes a tres apartados:\n" +
                "• Modificar Categorías: crea, edita o elimina las categorías de producto que " +
                "luego aparecen en el desplegable de Stock.\n" +
                "• Diseño: elige el aspecto de la aplicación entre tres temas (Predeterminado, " +
                "Claro u Oscuro). El cambio se aplica al instante en toda la aplicación.\n" +
                "• Lenguaje: cambia el idioma de la aplicación (igual que desde el Login).\n" +
                "• Ayuda: esta misma pantalla que estás leyendo ahora."
            );

            AppendSubtitulo("10. Roles de usuario");
            AppendTexto(
                "Administrador: acceso completo a toda la aplicación, incluida la pestaña " +
                "Devueltos y la creación de otras cuentas administradoras.\n" +
                "Usuario secundario: acceso a Clientes, Stock, Categorías y Salidas; no puede " +
                "acceder a Devueltos ni ver o usar las cuentas de administrador."
            );

            AppendSubtitulo("11. Exportar a Excel");
            AppendTexto(
                "Desde CONF → Exportación puedes sacar tus datos a un archivo Excel " +
                "(.xlsx) para consultarlos o compartirlos fuera de la aplicación. Hay " +
                "tres botones, uno por cada tipo de dato:\n" +
                "• Exportar Clientes: todos los clientes con sus datos de contacto.\n" +
                "• Exportar Stock: todos los productos del almacén, con su categoría, " +
                "precio y cantidad disponible.\n" +
                "• Exportar Transacciones: el detalle de todas las ventas y " +
                "devoluciones realizadas (una fila por producto, con fecha, tipo, " +
                "cliente, cantidad y precio).\n" +
                "Al pulsar cualquiera de los tres, la aplicación te pedirá dónde " +
                "guardar el archivo y con qué nombre. Al terminar, te preguntará si " +
                "quieres abrirlo directamente."
            );

            rtbManual.SelectionStart = 0;
            rtbManual.ScrollToCaret();
        }

        private void rtbManual_TextChanged(object sender, EventArgs e)
        {

        }
    }
}

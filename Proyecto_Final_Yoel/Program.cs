using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Proyecto_Final_Yoel
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // 0. Antes de nada: nos aseguramos de que la base de datos LocalDB
            // existe y tiene todas las tablas. La primera vez que se instale
            // en un PC nuevo, esto la crea sola (tarda 1-2 segundos).
            DatabaseInitializer.AsegurarBaseDatosCreada();

            // 1. Instanciamos el formulario de Login
            Login frmLogin = new Login();

            // 2. Lo mostramos como un diálogo modal. 
            // El código de Main se detendrá aquí hasta que el Login se oculte o se cierre.
            if (frmLogin.ShowDialog() == DialogResult.OK)
            {
                // 3. Si el Login devuelve "OK", arrancamos la aplicación de verdad
                // usando la Pagina_Principal como el formulario raíz.
                Application.Run(new Pagina_Principal());
            }
        }
    }
}

namespace Proyecto_Final_Yoel
{
    // Guarda quién ha iniciado sesión ahora mismo en la aplicación y si tiene
    // permisos de administrador. La consultan Pagina_Principal, FrmUsuarios,
    // FrmCambiarUsuario y FormTransacciones (para bloquear "Devueltos").
    public static class SesionActual
    {
        public static string UsuarioActual { get; private set; }
        public static bool EsAdministrador { get; private set; }

        public static void IniciarSesion(string usuario, bool esAdministrador)
        {
            UsuarioActual = usuario;
            EsAdministrador = esAdministrador;
        }

        public static void CerrarSesion()
        {
            UsuarioActual = null;
            EsAdministrador = false;
        }

        // Recorre todos los formularios abiertos (incluidos los embebidos dentro
        // de un panel) y le pide a cada FormTransacciones que se reevalúe si
        // puede mostrar la pestaña "Devueltos" o no, según el usuario activo.
        public static void RefrescarPermisosGlobal()
        {
            foreach (System.Windows.Forms.Form formularioAbierto in System.Windows.Forms.Application.OpenForms)
            {
                AplicarPermisosRecursivo(formularioAbierto);
            }
        }

        private static void AplicarPermisosRecursivo(System.Windows.Forms.Control contenedor)
        {
            if (contenedor is FormTransacciones ft)
            {
                ft.AplicarPermisos();
            }

            foreach (System.Windows.Forms.Control hijo in contenedor.Controls)
            {
                AplicarPermisosRecursivo(hijo);
            }
        }
    }
}

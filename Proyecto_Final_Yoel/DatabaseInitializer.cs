using System;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Proyecto_Final_Yoel
{
    // Se encarga de que la base de datos exista antes de que arranque el resto
    // de la aplicación. Usa SQL Server LocalDB, que no requiere instalar ni
    // configurar ningún servidor: LocalDB gestiona sus propios archivos de
    // base de datos automáticamente, por usuario, sin pedir nada.
    //
    // La primera vez que alguien ejecuta la aplicación en un PC nuevo:
    //   1. Se crea la base de datos (si no existe)
    //   2. Se crean todas las tablas y el trigger de control de stock (si no existen)
    //   3. Se crea un usuario Administrador de partida, para poder entrar sin
    //      tener que tocar SQL a mano
    //
    // En arranques posteriores, todo esto se comprueba y se salta al instante
    // porque ya existe.
    public static class DatabaseInitializer
    {
        private const string NombreBaseDatos = "ProyectoFinalYoel";

        // (localdb)\MSSQLLocalDB es la instancia de LocalDB que trae Visual
        // Studio instalada por defecto en cualquier PC con VS 2019/2022
        private const string ConexionMaster =
            @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True";

        public static string ObtenerConnectionString()
        {
            return $@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog={NombreBaseDatos};Integrated Security=True";
        }

        /// <summary>
        /// Comprueba que la base de datos y las tablas existen, y las crea si
        /// hace falta. Hay que llamarlo una vez, al arrancar la aplicación,
        /// antes de mostrar el Login.
        /// </summary>
        public static void AsegurarBaseDatosCreada()
        {
            try
            {
                CrearBaseDatosSiNoExiste();
                CrearTablasSiNoExisten();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "No se pudo preparar la base de datos automáticamente.\n\n" +
                    "Detalle: " + ex.Message + "\n\n" +
                    "Comprueba que SQL Server LocalDB está instalado (viene incluido " +
                    "con Visual Studio). Si el problema persiste, contacta con el " +
                    "desarrollador.",
                    "Error al iniciar la base de datos",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

                Environment.Exit(1);
            }
        }

        private static void CrearBaseDatosSiNoExiste()
        {
            using (SqlConnection conexion = new SqlConnection(ConexionMaster))
            {
                conexion.Open();

                bool existe;
                using (SqlCommand cmd = new SqlCommand(
                    "SELECT COUNT(*) FROM sys.databases WHERE name = @nombre", conexion))
                {
                    cmd.Parameters.AddWithValue("@nombre", NombreBaseDatos);
                    existe = (int)cmd.ExecuteScalar() > 0;
                }

                if (!existe)
                {
                    using (SqlCommand cmdCrear = new SqlCommand(
                        $"CREATE DATABASE [{NombreBaseDatos}]", conexion))
                    {
                        cmdCrear.ExecuteNonQuery();
                    }
                }
            }
        }

        private static void CrearTablasSiNoExisten()
        {
            using (SqlConnection conexion = new SqlConnection(ObtenerConnectionString()))
            {
                conexion.Open();

                bool tablasExisten;
                using (SqlCommand cmd = new SqlCommand(
                    "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Inicio_Sesion'", conexion))
                {
                    tablasExisten = (int)cmd.ExecuteScalar() > 0;
                }

                if (!tablasExisten)
                {
                    EjecutarScript(conexion, ScriptCreacionCompleto);
                }
            }
        }

        // SqlCommand no entiende la palabra "GO" (es una instrucción de SSMS,
        // no de T-SQL real), así que partimos el script por cada línea que
        // contenga solo "GO" y ejecutamos cada trozo por separado.
        private static void EjecutarScript(SqlConnection conexion, string script)
        {
            string[] lotes = Regex.Split(script, @"^\s*GO\s*$",
                RegexOptions.Multiline | RegexOptions.IgnoreCase);

            foreach (string lote in lotes)
            {
                string sql = lote.Trim();
                if (string.IsNullOrWhiteSpace(sql)) continue;

                using (SqlCommand cmd = new SqlCommand(sql, conexion))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private const string ScriptCreacionCompleto = @"
CREATE TABLE Inicio_Sesion (
    IdUsuario INT IDENTITY(1,1) PRIMARY KEY,
    Usuario VARCHAR(10) NOT NULL UNIQUE,
    Contrasena VARCHAR(10) NOT NULL,
    Fecha DATE NOT NULL,
    Hora TIME NOT NULL,
    EsAdministrador BIT NOT NULL DEFAULT 0
);
GO

CREATE TABLE Clientes (
    IdCliente CHAR(7) PRIMARY KEY,
    Nombre VARCHAR(20) NOT NULL,
    Apellidos VARCHAR(20) NOT NULL,
    Direccion VARCHAR(50),
    Telefono VARCHAR(9)
);
GO

CREATE TABLE Categorias (
    IdCategoria INT IDENTITY(1,1) PRIMARY KEY,
    Nombre VARCHAR(30) NOT NULL
);
GO

CREATE TABLE Stock (
    IdStock VARCHAR(10) PRIMARY KEY,
    Nombre VARCHAR(20) NOT NULL,
    Descripcion VARCHAR(200),
    IdCategoria INT FOREIGN KEY REFERENCES Categorias(IdCategoria),
    Precio DECIMAL(10,2) NOT NULL,
    Cantidad INT NOT NULL DEFAULT 0
);
GO

CREATE TABLE Transacciones (
    IdTransaccion INT IDENTITY(1,1) PRIMARY KEY,
    IdCliente CHAR(7) FOREIGN KEY REFERENCES Clientes(IdCliente),
    Fecha DATETIME NOT NULL DEFAULT GETDATE(),
    Tipo VARCHAR(10) NOT NULL DEFAULT 'Salida'
);
GO

CREATE TABLE DetalleTransaccion (
    IdDetalle INT IDENTITY(1,1) PRIMARY KEY,
    IdTransaccion INT FOREIGN KEY REFERENCES Transacciones(IdTransaccion),
    IdItem VARCHAR(10) FOREIGN KEY REFERENCES Stock(IdStock),
    Cantidad INT NOT NULL,
    PrecioUnitario DECIMAL(10,2) NOT NULL
);
GO

CREATE TRIGGER trg_DescontarStock
ON DetalleTransaccion
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Stock
    SET Stock.Cantidad = Stock.Cantidad - i.Cantidad
    FROM Stock
    INNER JOIN inserted i ON Stock.IdStock = i.IdItem
    INNER JOIN Transacciones t ON t.IdTransaccion = i.IdTransaccion
    WHERE t.Tipo = 'Salida';

    UPDATE Stock
    SET Stock.Cantidad = Stock.Cantidad + i.Cantidad
    FROM Stock
    INNER JOIN inserted i ON Stock.IdStock = i.IdItem
    INNER JOIN Transacciones t ON t.IdTransaccion = i.IdTransaccion
    WHERE t.Tipo = 'Devolucion';
END;
GO

-- Categorías de ejemplo, para que Stock tenga algo donde elegir desde el primer momento
INSERT INTO Categorias (Nombre) VALUES ('Motor'), ('Frenos'), ('Suspension'), ('Electrico'), ('Accesorios');
GO

-- Usuario administrador de partida: admin / admin
-- (para poder entrar en la aplicación recién instalada sin tocar SQL a mano)
INSERT INTO Inicio_Sesion (Usuario, Contrasena, Fecha, Hora, EsAdministrador)
VALUES ('admin', 'admin', CAST(GETDATE() AS DATE), CAST(GETDATE() AS TIME), 1);
GO
";
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Proyecto_Final_Yoel
{
    public enum IdiomaApp
    {
        Espanol,
        Ingles,
        Frances,
        Aleman,
        Portugues
    }

    public static class Idiomas
    {
        private static readonly string RutaConfigIdioma =
            Path.Combine(Application.StartupPath, "idioma.cfg");

        public static IdiomaApp IdiomaActual { get; private set; } = IdiomaApp.Espanol;

        static Idiomas()
        {
            IdiomaActual = CargarIdiomaGuardado();
        }

        public static void CambiarIdioma(IdiomaApp nuevoIdioma)
        {
            IdiomaActual = nuevoIdioma;
            GuardarIdioma(nuevoIdioma);
            AplicarIdiomaGlobal();
        }

        private static IdiomaApp CargarIdiomaGuardado()
        {
            try
            {
                if (File.Exists(RutaConfigIdioma))
                {
                    string texto = File.ReadAllText(RutaConfigIdioma).Trim();
                    if (Enum.TryParse(texto, out IdiomaApp idiomaGuardado))
                    {
                        return idiomaGuardado;
                    }
                }
            }
            catch
            {
                // Si falla la lectura, seguimos en Español sin romper el arranque.
            }

            return IdiomaApp.Espanol;
        }

        private static void GuardarIdioma(IdiomaApp idioma)
        {
            try
            {
                File.WriteAllText(RutaConfigIdioma, idioma.ToString());
            }
            catch
            {
                // Si no se puede guardar, la app sigue funcionando; solo no lo recordará.
            }
        }

        // ---------- DICCIONARIO DE TRADUCCIONES ----------
        // Estructura: [Nombre de la clase del formulario][Nombre del control] -> textos
        // en el orden del enum IdiomaApp (Español, Inglés, Francés, Alemán, Portugués)
        //
        // Para añadir un formulario nuevo a la traducción: añade una entrada aquí con
        // el nombre exacto de la clase (form.GetType().Name) y, dentro, una línea por
        // cada control cuyo texto quieras traducir (form.Text = título de la ventana).

        private static readonly Dictionary<string, Dictionary<string, string[]>> Traducciones =
            new Dictionary<string, Dictionary<string, string[]>>
        {
            ["Login"] = new Dictionary<string, string[]>
            {
                ["__FormTitle__"] = new[] { "Login", "Login", "Connexion", "Anmeldung", "Login" },
                ["label1"] = new[] { "Usuario:", "Username:", "Nom d'utilisateur :", "Benutzername:", "Usuário:" },
                ["label2"] = new[] { "Contraseña:", "Password:", "Mot de passe :", "Passwort:", "Senha:" },
                ["label3"] = new[] { "Hora:", "Time:", "Heure :", "Uhrzeit:", "Hora:" },
                ["label4"] = new[] { "Fecha:", "Date:", "Date :", "Datum:", "Data:" },
                ["buttonIniciarLogin"] = new[] { "Iniciar", "Login", "Connexion", "Anmelden", "Entrar" },
                ["buttonCancelarLogin"] = new[] { "Cancelar", "Cancel", "Annuler", "Abbrechen", "Cancelar" },
                ["buttonCrearLogin"] = new[] { "Crear", "Create", "Créer", "Erstellen", "Criar" },
                ["labelIdioma"] = new[] { "Idioma:", "Language:", "Langue :", "Sprache:", "Idioma:" },
            },

            ["Pagina_Principal"] = new Dictionary<string, string[]>
            {
                ["label1"] = new[]
                {
                    "Sistema de Gestion de Inventarios", "Inventory Management System",
                    "Système de Gestion des Stocks", "Bestandsverwaltungssystem",
                    "Sistema de Gestão de Inventário"
                },
                ["button6"] = new[] { "LOGIN", "LOGIN", "CONNEXION", "ANMELDEN", "LOGIN" },
                ["button5"] = new[] { "CONF", "SETTINGS", "CONFIG", "EINSTELL.", "CONFIG" },
                ["button1"] = new[] { "TRANSACCIONES", "TRANSACTIONS", "TRANSACTIONS", "TRANSAKTIONEN", "TRANSAÇÕES" },
                ["button4"] = new[] { "USUARIOS", "USERS", "UTILISATEURS", "BENUTZER", "USUÁRIOS" },
                ["buttonClientes"] = new[] { "CLIENTES", "CUSTOMERS", "CLIENTS", "KUNDEN", "CLIENTES" },
                ["button2"] = new[] { "STOCK", "STOCK", "STOCK", "BESTAND", "ESTOQUE" },
            },

            ["Configuración"] = new Dictionary<string, string[]>
            {
                ["button4"] = new[] { "Ayuda", "Help", "Aide", "Hilfe", "Ajuda" },
                ["button3"] = new[] { "Lenguaje", "Language", "Langue", "Sprache", "Idioma" },
                ["button2"] = new[] { "Diseño", "Appearance", "Apparence", "Design", "Aparência" },
                ["button1"] = new[]
                {
                    "Modificar\r\nCategorías", "Edit\r\nCategories", "Modifier\r\nCatégories",
                    "Kategorien\r\nbearbeiten", "Editar\r\nCategorias"
                },
            },

            ["FrmIdioma"] = new Dictionary<string, string[]>
            {
                ["__FormTitle__"] = new[] { "Idioma", "Language", "Langue", "Sprache", "Idioma" },
                ["label1"] = new[] { "Idioma", "Language", "Langue", "Sprache", "Idioma" },
            },
        };

        // ---------- APLICAR TRADUCCIÓN ----------

        public static void AplicarIdioma(Form formulario)
        {
            string nombreClase = formulario.GetType().Name;

            if (!Traducciones.TryGetValue(nombreClase, out var textosFormulario))
            {
                return; // Este formulario aún no está en el diccionario; se deja tal cual
            }

            int idx = (int)IdiomaActual;

            if (textosFormulario.TryGetValue("__FormTitle__", out var tituloVentana))
            {
                formulario.Text = tituloVentana[idx];
            }

            AplicarATodosLosControles(formulario, textosFormulario, idx);
        }

        private static void AplicarATodosLosControles(Control contenedor, Dictionary<string, string[]> textos, int idx)
        {
            foreach (Control control in contenedor.Controls)
            {
                if (textos.TryGetValue(control.Name, out var traduccion))
                {
                    control.Text = traduccion[idx];
                }

                if (control.HasChildren)
                {
                    AplicarATodosLosControles(control, textos, idx);
                }
            }
        }

        /// <summary>
        /// Reaplica el idioma activo a todos los formularios abiertos ahora mismo,
        /// incluidos los embebidos dentro de un panel, para que el cambio se vea
        /// al instante (mismo patrón que EstiloModerno.AplicarTemaGlobal).
        /// </summary>
        public static void AplicarIdiomaGlobal()
        {
            foreach (Form formularioAbierto in Application.OpenForms)
            {
                AplicarIdioma(formularioAbierto);

                var embebidos = new List<Form>();
                BuscarFormulariosEmbebidos(formularioAbierto, embebidos);

                foreach (Form embebido in embebidos)
                {
                    AplicarIdioma(embebido);
                }
            }
        }

        private static void BuscarFormulariosEmbebidos(Control contenedor, List<Form> encontrados)
        {
            foreach (Control hijo in contenedor.Controls)
            {
                if (hijo is Form formHijo)
                {
                    encontrados.Add(formHijo);
                }

                if (hijo.HasChildren)
                {
                    BuscarFormulariosEmbebidos(hijo, encontrados);
                }
            }
        }
    }
}

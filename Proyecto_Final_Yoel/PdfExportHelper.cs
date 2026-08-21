using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace Proyecto_Final_Yoel
{
    // Genera archivos PDF "a mano", escribiendo directamente la estructura del
    // formato PDF (objetos, flujo de contenido, tabla de referencias cruzadas...).
    // No depende de ninguna librería externa (nada de iTextSharp, PdfSharp...).
    public static class PdfExportHelper
    {
        private const double ANCHO_PAGINA = 612; // Tamaño Carta (Letter), en puntos
        private const double ALTO_PAGINA = 792;
        private const double MARGEN = 40;
        private const double ALTO_LINEA = 16;
        private const int FILAS_POR_PAGINA = 28; // un poco menos que antes, para dejar sitio a la cabecera

        // Los caracteres especiales (como €, á, ñ...) hay que codificarlos con
        // Windows-1252, que es la codificación que corresponde a /WinAnsiEncoding
        // en el PDF. ISO-8859-1 NO tiene el símbolo € (por eso salía mal antes).
        private static readonly Encoding CP1252 = Encoding.GetEncoding(1252);

        public static void ExportarTablaPdf(
            string rutaArchivo,
            string titulo,
            string[] encabezados,
            double[] anchosColumnas,
            List<string[]> filas,
            string piePagina = null,
            string rutaLogoJpg = null,
            string nombreApp = null)
        {
            byte[] logoBytes = null;
            int logoAnchoPx = 0, logoAltoPx = 0;
            if (!string.IsNullOrEmpty(rutaLogoJpg) && File.Exists(rutaLogoJpg))
            {
                logoBytes = File.ReadAllBytes(rutaLogoJpg);
                LeerDimensionesJpeg(logoBytes, out logoAnchoPx, out logoAltoPx);
            }

            var paginas = PaginarFilas(filas);
            var buffer = new List<byte>();
            var offsets = new Dictionary<int, long>();

            void EscribirObjeto(int numero, string contenido)
            {
                offsets[numero] = buffer.Count;
                buffer.AddRange(CP1252.GetBytes($"{numero} 0 obj\n{contenido}\nendobj\n"));
            }

            void EscribirObjetoStream(int numero, string streamContenido)
            {
                offsets[numero] = buffer.Count;
                var streamBytes = CP1252.GetBytes(streamContenido);
                buffer.AddRange(CP1252.GetBytes($"{numero} 0 obj\n<< /Length {streamBytes.Length} >>\nstream\n"));
                buffer.AddRange(streamBytes);
                buffer.AddRange(CP1252.GetBytes("\nendstream\nendobj\n"));
            }

            void EscribirImagen(int numero, byte[] jpegBytes, int anchoPx, int altoPx)
            {
                offsets[numero] = buffer.Count;
                var cabecera = $"{numero} 0 obj\n<< /Type /XObject /Subtype /Image /Width {anchoPx} /Height {altoPx} " +
                               $"/ColorSpace /DeviceRGB /BitsPerComponent 8 /Filter /DCTDecode /Length {jpegBytes.Length} >>\nstream\n";
                buffer.AddRange(CP1252.GetBytes(cabecera));
                buffer.AddRange(jpegBytes); // Los bytes JPEG se incrustan tal cual, el PDF los soporta de forma nativa
                buffer.AddRange(CP1252.GetBytes("\nendstream\nendobj\n"));
            }

            // Cabecera del PDF (la línea con bytes altos es el marcador estándar
            // que indica a los lectores que el archivo tiene contenido binario)
            buffer.AddRange(Encoding.ASCII.GetBytes("%PDF-1.4\n"));
            buffer.AddRange(new byte[] { 0x25, 0xE2, 0xE3, 0xCF, 0xD3, 0x0A });

            int numPaginas = Math.Max(paginas.Count, 1);
            const int objCatalogo = 1;
            const int objPages = 2;
            const int objFontRegular = 3;
            const int objFontBold = 4;
            int objImagen = 5;
            bool hayLogo = logoBytes != null;
            int primerObjPagina = hayLogo ? 6 : 5;
            int primerObjContenido = primerObjPagina + numPaginas;

            var contenidosPaginas = new List<string>();
            for (int p = 0; p < numPaginas; p++)
            {
                var filasPagina = p < paginas.Count ? paginas[p] : new List<string[]>();
                contenidosPaginas.Add(ConstruirContenidoPagina(
                    titulo, encabezados, anchosColumnas, filasPagina, p + 1, numPaginas, piePagina,
                    p == 0 && hayLogo, logoAnchoPx, logoAltoPx, nombreApp));
            }

            EscribirObjeto(objCatalogo, $"<< /Type /Catalog /Pages {objPages} 0 R >>");

            var kids = new StringBuilder();
            for (int p = 0; p < numPaginas; p++) kids.Append($"{primerObjPagina + p} 0 R ");
            EscribirObjeto(objPages, $"<< /Type /Pages /Kids [{kids.ToString().Trim()}] /Count {numPaginas} >>");

            EscribirObjeto(objFontRegular, "<< /Type /Font /Subtype /Type1 /BaseFont /Helvetica /Encoding /WinAnsiEncoding >>");
            EscribirObjeto(objFontBold, "<< /Type /Font /Subtype /Type1 /BaseFont /Helvetica-Bold /Encoding /WinAnsiEncoding >>");

            if (hayLogo)
            {
                EscribirImagen(objImagen, logoBytes, logoAnchoPx, logoAltoPx);
            }

            for (int p = 0; p < numPaginas; p++)
            {
                int numPag = primerObjPagina + p;
                int numContenido = primerObjContenido + p;
                string recursos = hayLogo
                    ? $"/Resources << /Font << /F1 {objFontRegular} 0 R /F2 {objFontBold} 0 R >> /XObject << /Im1 {objImagen} 0 R >> >>"
                    : $"/Resources << /Font << /F1 {objFontRegular} 0 R /F2 {objFontBold} 0 R >> >>";
                string dic = $"<< /Type /Page /Parent {objPages} 0 R /MediaBox [0 0 {Fmt(ANCHO_PAGINA)} {Fmt(ALTO_PAGINA)}] " +
                             $"{recursos} /Contents {numContenido} 0 R >>";
                EscribirObjeto(numPag, dic);
            }

            for (int p = 0; p < numPaginas; p++)
            {
                EscribirObjetoStream(primerObjContenido + p, contenidosPaginas[p]);
            }

            int totalObjetos = primerObjContenido + numPaginas;
            long offsetXref = buffer.Count;

            var xref = new StringBuilder();
            xref.Append("xref\n");
            xref.Append($"0 {totalObjetos}\n");
            xref.Append("0000000000 65535 f \n");
            for (int i = 1; i < totalObjetos; i++)
            {
                xref.Append(offsets[i].ToString("D10") + " 00000 n \n");
            }
            buffer.AddRange(CP1252.GetBytes(xref.ToString()));

            buffer.AddRange(CP1252.GetBytes(
                $"trailer\n<< /Size {totalObjetos} /Root {objCatalogo} 0 R >>\nstartxref\n{offsetXref}\n%%EOF"));

            if (File.Exists(rutaArchivo)) File.Delete(rutaArchivo);
            File.WriteAllBytes(rutaArchivo, buffer.ToArray());
        }

        private static List<List<string[]>> PaginarFilas(List<string[]> filas)
        {
            var paginas = new List<List<string[]>>();
            for (int i = 0; i < filas.Count; i += FILAS_POR_PAGINA)
            {
                paginas.Add(filas.GetRange(i, Math.Min(FILAS_POR_PAGINA, filas.Count - i)));
            }
            return paginas;
        }

        private static string ConstruirContenidoPagina(
            string titulo, string[] encabezados, double[] anchosColumnas,
            List<string[]> filasPagina, int numPagina, int totalPaginas, string piePagina,
            bool conCabeceraTicket, int logoAnchoPx, int logoAltoPx, string nombreApp)
        {
            var sb = new StringBuilder();
            double y = ALTO_PAGINA - MARGEN;

            if (conCabeceraTicket)
            {
                // ---- Cabecera tipo "ticket": logo + nombre de la app, grande y resaltado ----
                double logoAltoPt = 46;
                double logoAnchoPt = logoAltoPx > 0 ? logoAltoPt * logoAnchoPx / logoAltoPx : logoAltoPt;

                double yLogo = y - logoAltoPt;
                sb.Append($"q {Fmt(logoAnchoPt)} 0 0 {Fmt(logoAltoPt)} {Fmt(MARGEN)} {Fmt(yLogo)} cm /Im1 Do Q\n");

                if (!string.IsNullOrEmpty(nombreApp))
                {
                    double yTextoNombre = y - (logoAltoPt / 2) - 8; // centrado verticalmente con el logo
                    sb.Append(TextoEn(MARGEN + logoAnchoPt + 14, yTextoNombre, nombreApp, "F2", 20));
                }

                y -= logoAltoPt + 14;
                sb.Append(LineaHorizontal(MARGEN, y, ANCHO_PAGINA - MARGEN));
                y -= 26;

                sb.Append(TextoEn(MARGEN, y, titulo, "F2", 14));
                y -= 20;
                sb.Append(TextoEn(MARGEN, y, "Generado el " + DateTime.Now.ToString("dd/MM/yyyy HH:mm"), "F1", 9));
                y -= 24;
            }
            else if (numPagina == 1)
            {
                sb.Append(TextoEn(MARGEN, y, titulo, "F2", 16));
                y -= 22;
                sb.Append(TextoEn(MARGEN, y, "Generado el " + DateTime.Now.ToString("dd/MM/yyyy HH:mm"), "F1", 9));
                y -= 26;
            }
            else
            {
                sb.Append(TextoEn(MARGEN, y, titulo + " (continuación)", "F2", 13));
                y -= 26;
            }

            for (int c = 0; c < encabezados.Length; c++)
            {
                sb.Append(TextoEn(MARGEN + anchosColumnas[c], y, encabezados[c], "F2", 10));
            }
            y -= 6;
            sb.Append(LineaHorizontal(MARGEN, y, ANCHO_PAGINA - MARGEN));
            y -= ALTO_LINEA;

            foreach (var fila in filasPagina)
            {
                for (int c = 0; c < fila.Length && c < anchosColumnas.Length; c++)
                {
                    sb.Append(TextoEn(MARGEN + anchosColumnas[c], y, fila[c], "F1", 9));
                }
                y -= ALTO_LINEA;
            }

            if (!string.IsNullOrEmpty(piePagina) && numPagina == totalPaginas)
            {
                y -= 10;
                sb.Append(LineaHorizontal(MARGEN, y, ANCHO_PAGINA - MARGEN));
                y -= ALTO_LINEA;
                sb.Append(TextoEn(MARGEN, y, piePagina, "F2", 11));
            }

            sb.Append(TextoEn(ANCHO_PAGINA - MARGEN - 70, MARGEN / 2, $"Página {numPagina} de {totalPaginas}", "F1", 8));

            return sb.ToString();
        }

        private static string TextoEn(double x, double y, string texto, string fuente, double tamano)
        {
            string escapado = EscaparPdf(texto ?? "");
            return $"BT /{fuente} {Fmt(tamano)} Tf 1 0 0 1 {Fmt(x)} {Fmt(y)} Tm ({escapado}) Tj ET\n";
        }

        private static string LineaHorizontal(double x1, double y, double x2)
        {
            return $"{Fmt(x1)} {Fmt(y)} m {Fmt(x2)} {Fmt(y)} l S\n";
        }

        private static string EscaparPdf(string texto)
        {
            return texto.Replace("\\", "\\\\").Replace("(", "\\(").Replace(")", "\\)");
        }

        private static string Fmt(double valor)
        {
            return valor.ToString(CultureInfo.InvariantCulture);
        }

        // Lee el ancho/alto en píxeles directamente de la cabecera del JPEG,
        // sin necesitar System.Drawing.Image (para no arrastrar dependencias
        // extra solo por esto).
        private static void LeerDimensionesJpeg(byte[] datos, out int ancho, out int alto)
        {
            ancho = 0;
            alto = 0;
            int i = 2; // los 2 primeros bytes son el marcador SOI (0xFFD8)
            while (i < datos.Length - 9)
            {
                if (datos[i] != 0xFF) { i++; continue; }
                byte marcador = datos[i + 1];

                // Marcadores SOF0..SOF3, SOF5..SOF7, etc. contienen las dimensiones
                bool esSOF = marcador >= 0xC0 && marcador <= 0xCF && marcador != 0xC4 && marcador != 0xC8 && marcador != 0xCC;

                if (esSOF)
                {
                    alto = (datos[i + 5] << 8) + datos[i + 6];
                    ancho = (datos[i + 7] << 8) + datos[i + 8];
                    return;
                }

                int longitudSegmento = (datos[i + 2] << 8) + datos[i + 3];
                i += 2 + longitudSegmento;
            }
        }
    }
}

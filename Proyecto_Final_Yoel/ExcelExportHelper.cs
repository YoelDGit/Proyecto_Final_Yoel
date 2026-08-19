using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Packaging;
using System.Text;

namespace Proyecto_Final_Yoel
{
    // Genera archivos .xlsx "a mano" (formato OpenXML / SpreadsheetML), usando
    // System.IO.Packaging, que ya viene incluido en .NET Framework (WindowsBase).
    // No hace falta instalar ninguna librería de terceros (EPPlus, ClosedXML...).
    //
    // Si al compilar da un error de que no encuentra "System.IO.Packaging",
    // añade la referencia: clic derecho en el proyecto -> Agregar -> Referencia
    // -> Ensamblados -> Framework -> marca "WindowsBase".
    public static class ExcelExportHelper
    {
        public static void ExportarAExcel(string rutaArchivo, string nombreHoja, List<string> encabezados, List<object[]> filas)
        {
            if (File.Exists(rutaArchivo))
            {
                File.Delete(rutaArchivo);
            }

            using (Package paquete = Package.Open(rutaArchivo, FileMode.Create))
            {
                Uri uriWorkbook = new Uri("/xl/workbook.xml", UriKind.Relative);
                PackagePart partWorkbook = paquete.CreatePart(
                    uriWorkbook,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet.main+xml");

                string workbookXml =
                    "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>" +
                    "<workbook xmlns=\"http://schemas.openxmlformats.org/spreadsheetml/2006/main\" " +
                    "xmlns:r=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships\">" +
                    "<sheets><sheet name=\"" + EscaparXml(LimitarNombreHoja(nombreHoja)) +
                    "\" sheetId=\"1\" r:id=\"rIdSheet1\"/></sheets>" +
                    "</workbook>";
                EscribirTexto(partWorkbook, workbookXml);

                Uri uriSheet = new Uri("/xl/worksheets/sheet1.xml", UriKind.Relative);
                PackagePart partSheet = paquete.CreatePart(
                    uriSheet,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.worksheet+xml");
                EscribirTexto(partSheet, ConstruirHojaXml(encabezados, filas));

                Uri uriStyles = new Uri("/xl/styles.xml", UriKind.Relative);
                PackagePart partStyles = paquete.CreatePart(
                    uriStyles,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.styles+xml");
                EscribirTexto(partStyles, ConstruirStylesXml());

                // Relaciones: workbook -> hoja y workbook -> estilos
                partWorkbook.CreateRelationship(uriSheet, TargetMode.Internal,
                    "http://schemas.openxmlformats.org/officeDocument/2006/relationships/worksheet", "rIdSheet1");
                partWorkbook.CreateRelationship(uriStyles, TargetMode.Internal,
                    "http://schemas.openxmlformats.org/officeDocument/2006/relationships/styles", "rIdStyles1");

                // Relación raíz del paquete -> workbook (esto es lo que hace que
                // Excel sepa cuál es el archivo principal al abrir el .xlsx)
                paquete.CreateRelationship(uriWorkbook, TargetMode.Internal,
                    "http://schemas.openxmlformats.org/officeDocument/2006/relationships/officeDocument",
                    "rIdWorkbook1");
            }
        }

        private static string ConstruirHojaXml(List<string> encabezados, List<object[]> filas)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>");
            sb.Append("<worksheet xmlns=\"http://schemas.openxmlformats.org/spreadsheetml/2006/main\">");
            sb.Append("<sheetData>");

            // Fila 1: encabezados, en negrita (estilo s="1", definido en styles.xml)
            sb.Append("<row r=\"1\">");
            for (int col = 0; col < encabezados.Count; col++)
            {
                string celda = ColumnaLetra(col) + "1";
                sb.Append("<c r=\"" + celda + "\" t=\"inlineStr\" s=\"1\"><is><t>" +
                    EscaparXml(encabezados[col]) + "</t></is></c>");
            }
            sb.Append("</row>");

            // Resto de filas: los datos
            for (int fila = 0; fila < filas.Count; fila++)
            {
                int numFila = fila + 2; // la fila 1 es la de encabezados
                sb.Append("<row r=\"" + numFila + "\">");

                object[] valores = filas[fila];
                for (int col = 0; col < valores.Length; col++)
                {
                    string celda = ColumnaLetra(col) + numFila;
                    object valor = valores[col];

                    if (valor == null)
                    {
                        sb.Append("<c r=\"" + celda + "\"/>");
                    }
                    else if (EsNumerico(valor))
                    {
                        string numTexto = Convert.ToDouble(valor, CultureInfo.InvariantCulture)
                            .ToString(CultureInfo.InvariantCulture);
                        sb.Append("<c r=\"" + celda + "\"><v>" + numTexto + "</v></c>");
                    }
                    else
                    {
                        sb.Append("<c r=\"" + celda + "\" t=\"inlineStr\"><is><t>" +
                            EscaparXml(valor.ToString()) + "</t></is></c>");
                    }
                }

                sb.Append("</row>");
            }

            sb.Append("</sheetData></worksheet>");
            return sb.ToString();
        }

        private static string ConstruirStylesXml()
        {
            // Estilo 0 = normal, estilo 1 = negrita (usado en la fila de encabezados)
            return
                "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>" +
                "<styleSheet xmlns=\"http://schemas.openxmlformats.org/spreadsheetml/2006/main\">" +
                "<fonts count=\"2\">" +
                "<font><sz val=\"11\"/><name val=\"Calibri\"/></font>" +
                "<font><b/><sz val=\"11\"/><name val=\"Calibri\"/></font>" +
                "</fonts>" +
                "<fills count=\"1\"><fill><patternFill patternType=\"none\"/></fill></fills>" +
                "<borders count=\"1\"><border/></borders>" +
                "<cellStyleXfs count=\"1\"><xf/></cellStyleXfs>" +
                "<cellXfs count=\"2\">" +
                "<xf fontId=\"0\" xfId=\"0\"/>" +
                "<xf fontId=\"1\" xfId=\"0\" applyFont=\"1\"/>" +
                "</cellXfs>" +
                "<cellStyles count=\"1\"><cellStyle name=\"Normal\" xfId=\"0\" builtinId=\"0\"/></cellStyles>" +
                "</styleSheet>";
        }

        private static void EscribirTexto(PackagePart parte, string contenido)
        {
            using (Stream stream = parte.GetStream(FileMode.Create, FileAccess.Write))
            using (StreamWriter writer = new StreamWriter(stream, new UTF8Encoding(false)))
            {
                writer.Write(contenido);
            }
        }

        private static bool EsNumerico(object valor)
        {
            return valor is int || valor is long || valor is short ||
                   valor is decimal || valor is double || valor is float;
        }

        private static string ColumnaLetra(int indiceCero)
        {
            int n = indiceCero + 1;
            string letra = "";
            while (n > 0)
            {
                int resto = (n - 1) % 26;
                letra = (char)('A' + resto) + letra;
                n = (n - 1) / 26;
            }
            return letra;
        }

        private static string LimitarNombreHoja(string nombre)
        {
            // Excel no permite nombres de hoja de más de 31 caracteres ni
            // ciertos símbolos (: \ / ? * [ ])
            string limpio = nombre;
            foreach (char c in new[] { ':', '\\', '/', '?', '*', '[', ']' })
            {
                limpio = limpio.Replace(c.ToString(), "");
            }
            return limpio.Length > 31 ? limpio.Substring(0, 31) : limpio;
        }

        private static string EscaparXml(string texto)
        {
            return texto
                .Replace("&", "&amp;")
                .Replace("<", "&lt;")
                .Replace(">", "&gt;")
                .Replace("\"", "&quot;")
                .Replace("'", "&apos;");
        }
    }
}

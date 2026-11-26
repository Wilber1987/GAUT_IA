using System;
using System.Text;
using UglyToad.PdfPig;

namespace BusinessLogic.IAModule.Utils
{
    public static class PdfTextExtractor
    {
        /// <summary>
        /// Extrae el texto de un PDF recibido en base64.
        /// Solo funciona con PDFs con texto embebido (no escaneados/imagen).
        /// </summary>
        public static string ExtractText(string base64Pdf)
        {
            byte[] pdfBytes = Convert.FromBase64String(base64Pdf);

            using var pdf = PdfDocument.Open(pdfBytes);
            var sb = new StringBuilder();

            foreach (var page in pdf.GetPages())
            {
                sb.AppendLine(page.Text);
            }

            return sb.ToString();
        }
    }
}
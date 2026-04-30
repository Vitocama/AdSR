using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using AdSR.Models;

namespace AdSR.Common
{
    public class Stampa
    {
        /// <summary>
        /// Genera un file HTML con i dati del Protocollo (Marsau)
        /// e lo apre nel browser predefinito. Il file viene salvato
        /// in %TEMP% con nome basato sulla ditta.
        /// </summary>
        public void GeneraEApri(Protocolli p)
        {
            if (p == null)
                throw new ArgumentNullException("p");

            string nomeDitta = string.IsNullOrWhiteSpace(p.Ditta) ? "SenzaDitta" : p.Ditta;
            string nomeFileSicuro = SanitizzaNomeFile(nomeDitta);

            string nomeFile = string.Format("Marsau_{0}_{1:yyyyMMdd_HHmmss}.html",
                                            nomeFileSicuro, DateTime.Now);
            string percorso = Path.Combine(Path.GetTempPath(), nomeFile);

            string html = CostruisciHtml(p);
            File.WriteAllText(percorso, html, Encoding.UTF8);

            Process.Start(new ProcessStartInfo(percorso) { UseShellExecute = true });
        }

        private string SanitizzaNomeFile(string nome)
        {
            char[] invalidi = Path.GetInvalidFileNameChars();
            StringBuilder sb = new StringBuilder();
            foreach (char c in nome)
            {
                if (Array.IndexOf(invalidi, c) < 0 && c != ' ')
                    sb.Append(c);
                else
                    sb.Append('_');
            }
            return sb.ToString();
        }

        private string CostruisciHtml(Protocolli p)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<!DOCTYPE html>");
            sb.AppendLine("<html lang=\"it\">");
            sb.AppendLine("<head>");
            sb.AppendLine("<meta charset=\"UTF-8\">");
            sb.AppendFormat("<title>Marsau - {0}</title>\n", Esc(p.Ditta));
            sb.AppendLine("<style>");
            sb.AppendLine(@"
    body { font-family: 'Segoe UI', Arial, sans-serif; background: #fff;
           color: #000000; margin: 0; padding: 30px; }
    .header { text-align: center; padding: 10px 0 20px 0; }
    .header h1 { margin: 0; font-size: 22px; font-weight: 600;
                 letter-spacing: 2px; color: #000000; }
    .lower { text-align: center; padding: 0 0 20px 0; }
h2.argomento {
    display: block;
    max-width: 90%;
    margin: 20px auto;
    padding: 14px 20px;
    background: #ffffff;
    border: 1px solid #000000;
    border-left: 4px solid #000000;
    border-radius: 3px;
    color: #000000;
    font-size: 15px;
    font-weight: 500;
    line-height: 1.5;
    text-align: left;
    text-transform: none;
    letter-spacing: 0;
    word-wrap: break-word;
    overflow-wrap: break-word;
    text-transform: uppercase;
}
    h2.lower { margin: 0; font-size: 26px; font-weight: 700;
               color: #000000; letter-spacing: 2px;
               text-transform: uppercase;
               border: none; padding: 0; }
    .section { padding: 16px 0; border-bottom: 1px solid #000000; }
    .section h2 { color: #000000; font-size: 14px; text-transform: uppercase;
                  border-left: 4px solid #C9A84C; padding-left: 10px;
                  margin: 0 0 12px 0; letter-spacing: 1px; }
    table { width: 100%; border-collapse: collapse; }
    table td { padding: 7px 10px; vertical-align: top; font-size: 14px; }
    table td.label { width: 230px; font-weight: 600; color: #2A456E;
                     background: #f9f6ed; }
    table td.value { color: #333; border-bottom: 1px solid #eee; }
    .footer { padding: 12px 0; font-size: 11px; color: #888;
              text-align: right; border-top: 1px solid #000000; }
    .toolbar { text-align: right; margin: 0 0 12px 0; }
    .toolbar button { background: #C9A84C; color: #0F2044;
                      border: none; padding: 8px 18px; font-weight: 600;
                      font-family: 'Segoe UI'; font-size: 13px;
                      cursor: pointer; border-radius: 3px; }
    .toolbar button:hover { background: #E8C97A; }
    @media print {
        body { padding: 0; }
        .toolbar { display: none; }
    }
");
            sb.AppendLine("</style>");
            sb.AppendLine("</head>");
            sb.AppendLine("<body>");

            sb.AppendLine("<div class=\"toolbar\">");
            sb.AppendLine("<button onclick=\"window.print()\">Stampa</button>");
            sb.AppendLine("</div>");

            sb.AppendLine("<div class=\"container\">");

            // Header
            sb.AppendLine("<div class=\"header\">");
            sb.AppendLine("<h1>ARSENALE MILITARE MARITTIMO</h1>");
            sb.AppendLine("<h1>AUGUSTA</h1>");
            sb.AppendLine("<h1>SETTORE SALA RICEZIONE</h1>");
            sb.AppendFormat("<h2>DITTA</h2>\n");
            sb.AppendFormat("<h2 class=\"lower\">{0} del {1}</h2>\n", Esc(p.Ditta), Esc(p.MarsauData));
            sb.AppendFormat("<h2 class=\"lower\">Richiesta {0}</h2>\n", Esc(p.Richiesta));
            sb.AppendFormat("<h2 class=\"lower\">ATTO :{0} </h2>\n", Esc(p.Atto));
            sb.AppendFormat("<h2 class=\"lower\"> ARGOMENTO: </h2>\n");
            sb.AppendLine("<div style=\"text-align:center;\">");
            sb.AppendFormat("<h2 class=\"argomento\">{0}</h2>\n", Esc(p.Argomento));
            sb.AppendLine("</div>");
            sb.AppendLine("</div>");
            sb.AppendLine("</body>");
            sb.AppendLine("</html>");

            return sb.ToString();
        }

        private void Riga(StringBuilder sb, string etichetta, object valore)
        {
            string val = valore == null ? "" : valore.ToString();
            sb.AppendFormat("<tr><td class=\"label\">{0}</td><td class=\"value\">{1}</td></tr>\n",
                            Esc(etichetta), Esc(val));
        }

        private string Esc(string s)
        {
            if (string.IsNullOrEmpty(s)) return "&nbsp;";
            return s.Replace("&", "&amp;")
                    .Replace("<", "&lt;")
                    .Replace(">", "&gt;")
                    .Replace("\"", "&quot;")
                    .Replace("\n", "<br/>");
        }
    }
}
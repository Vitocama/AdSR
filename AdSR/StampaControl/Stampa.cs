using AdSR.Models;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace AdSR.StampaControl
{
    /// <summary>Quale foglio stampare.</summary>
    public enum TipoStampa
    {
        Collaudo,      // pulsante STAMPA
        Trasmissione   // pulsante TRASMISSIONE
    }

    /// <summary>
    /// Crea i fogli di stampa (FlowDocument WPF) e li invia alla stampante.
    /// </summary>
    public class Stampa
    {
        // ╔══════════════════════════════════════════════════════════╗
        // ║  IMPOSTAZIONI  —  MODIFICA QUI dimensioni e testi fissi  ║
        // ╚══════════════════════════════════════════════════════════╝

        // ── Generale ──
        private const string Carattere = "Segoe UI";
        private const double TestoNormale = 12;   // dimensione base di tutto il testo
        private const double SpazioTraRighe = 8;    // spazio sotto ogni riga

        // ── Foglio STAMPA (collaudo) ──
        private const double Col_Ditta = 28;
        private const double Col_Marsau = 24;
        private const double Col_Argomento = 26;
        private const double Col_Collaudato = 18;
        private const double Col_Introduzione = 18;
        private const double Col_Firma = 16;

        // ── Foglio TRASMISSIONE ──
        private const double Tra_Titolo = 16;
        private const double Tra_Ditta = 20;
        private const double Tra_Marsau = 15;
        private const double Tra_DatiGrandi = 14;  // Capitolo, gg, Introduzione, RIC, Fornitura, BDC
        private const double Tra_Firma = 12;

        // ── Testi fissi ──
        private const string Ente = "ARSENALE MILITARE MARITTIMO";
        private const string Ufficio = "Sala Ricezione";
        private const string Telefoni = "Telefono 46653 - 46648 - 46654";
        private const string Citta = "Cap 96011 Augusta";
        private const string TitoloTrasm = "TRASMISSIONE BOLLA DI CARICO";
        private const string Destinatario = "AL MARINARSEN Augusta - Economo";
        private const string SettoreFirma = "2° SETTORE ASS. AMM.VO";

        private static string Oggi => DateTime.Today.ToString("dd/MM/yyyy");


        // ╔══════════════════════════════════════════════════════════╗
        // ║  FOGLIO STAMPA (collaudo)  —  una riga = una voce        ║
        // ╚══════════════════════════════════════════════════════════╝
        private void CreaCollaudo(FlowDocument doc, Protocolli p, DatiStampa dati)
        {
            Riga(doc, Col_Ditta, "Ditta: ", p.Ditta);
            Riga(doc, Col_Marsau, "MARSAU ", p.Marsau, " del ", p.MarsauData);
            RigaACapo(doc, Col_Argomento, "Argomento:", p.Argomento);

            // Mat. collaudato su tre righe
            RigaACapo(doc, Col_Collaudato, "Mat. collaudato",
                Casella(dati?.Parziale == true) + "Parziale",
                Casella(dati?.Completo == true) + "Completo");

            // Introduzione del ...  colli n. ... (solo se colli > 0)
            if (Colli(dati) > 0)
                Riga(doc, Col_Introduzione, "Introduzione del ", Linea(p.DataIntroduzione), "   colli n. ", Colli(dati).ToString());
            else
                Riga(doc, Col_Introduzione, "Introduzione del ", Linea(p.DataIntroduzione));

            Spazio(doc, 60);
            Riga(doc, Col_Firma, "Data: ", Oggi);
            Spazio(doc, 40);
            Riga(doc, Col_Firma, "Firma ______________________________");
        }


        // ╔══════════════════════════════════════════════════════════╗
        // ║  FOGLIO TRASMISSIONE  —  una riga = una voce             ║
        // ╚══════════════════════════════════════════════════════════╝
        private void CreaTrasmissione(FlowDocument doc, Protocolli p, DatiStampa dati)
        {
            Intestazione(doc);

            Titolo(doc, Tra_Titolo, TitoloTrasm);
            Riga(doc, TestoNormale, Destinatario);

            // Ditta grande e indirizzo sotto (indirizzo inserito dall'utente)
            RigaACapo(doc, Tra_Ditta, Valore(p.Ditta), Valore(dati?.Indirizzo));

            Riga(doc, Tra_Marsau, "Marsau: ", p.Marsau, "     Data Marsau: ", p.MarsauData);
            Riga(doc, Tra_DatiGrandi, "Capitolo: ", p.Capitolo, "     gg: ", dati?.Giorni);
            Riga(doc, Tra_DatiGrandi, "Introduzione: ", p.DataIntroduzione, "     RIC: ", p.Ric);
            Riga(doc, Tra_DatiGrandi, "Fornitura: ", p.Forniture, "     BDC: ", p.Bdc, "   del ", p.DataBdc);

            Riga(doc, TestoNormale, "Argomento: ", p.Argomento);

            Riga(doc, TestoNormale, "DDT 1: ", p.Ddt, "   del ", p.DataDdt, "   pervenuto il ", p.Pervenuto);
            Riga(doc, TestoNormale, "DDT 2: ", p.Ddt2, "   del ", p.DataDdt2, "   pervenuto il ", p.Pervenuto2);
            Riga(doc, TestoNormale, "DDT 3: ", p.Ddt3, "   del ", p.DataDdt3, "   pervenuto il ", p.Pervenuto3);

            Riga(doc, TestoNormale, "Fattura: ", p.Fattura, "   del ", p.DataFattura, "     CRE: ", p.Cre, "   del ", p.DataCre);
            Riga(doc, TestoNormale, "Note: ", p.Note);

            // Firma al centro (nome inserito dall'utente)
            Spazio(doc, 30);
            Titolo(doc, Tra_Firma, SettoreFirma);
            Titolo(doc, Tra_Firma, Linea(dati?.NomeFirmatario), grassetto: false);
        }

        /// <summary>Intestazione: a sinistra Ente/Ufficio/Telefoni, a destra Città e data.</summary>
        private void Intestazione(FlowDocument doc)
        {
            var sinistra = new Paragraph { Margin = new Thickness(0) };
            sinistra.Inlines.Add(new Bold(new Run(Ente)));
            sinistra.Inlines.Add(new LineBreak());
            sinistra.Inlines.Add(new Run(Ufficio));
            sinistra.Inlines.Add(new LineBreak());
            sinistra.Inlines.Add(new Run(Telefoni));

            var destra = new Paragraph(new Run($"{Citta}, {Oggi}"))
            {
                Margin = new Thickness(0),
                TextAlignment = TextAlignment.Right
            };

            var tabella = new Table { CellSpacing = 0, Margin = new Thickness(0, 0, 0, 20) };
            tabella.Columns.Add(new TableColumn { Width = new GridLength(1, GridUnitType.Star) });
            tabella.Columns.Add(new TableColumn { Width = new GridLength(1, GridUnitType.Star) });
            var riga = new TableRow();
            riga.Cells.Add(new TableCell(sinistra));
            riga.Cells.Add(new TableCell(destra));
            var gruppo = new TableRowGroup();
            gruppo.Rows.Add(riga);
            tabella.RowGroups.Add(gruppo);
            doc.Blocks.Add(tabella);
        }


        // ╔══════════════════════════════════════════════════════════╗
        // ║  STRUMENTI per scrivere le righe (di solito non serve    ║
        // ║  modificarli)                                            ║
        // ╚══════════════════════════════════════════════════════════╝

        /// <summary>
        /// Una riga di testo. I pezzi si alternano: etichetta (grassetto), valore, etichetta, valore...
        /// Esempio: Riga(doc, 14, "DDT: ", p.Ddt, " del ", p.DataDdt)
        /// </summary>
        private static void Riga(FlowDocument doc, double dimensione, params string[] pezzi)
        {
            var paragrafo = new Paragraph { FontSize = dimensione, Margin = new Thickness(0, 0, 0, SpazioTraRighe) };
            for (int i = 0; i < pezzi.Length; i++)
            {
                var run = new Run(Valore(pezzi[i]));
                if (i % 2 == 0)
                    paragrafo.Inlines.Add(new Bold(run));   // etichetta
                else
                    paragrafo.Inlines.Add(run);             // valore
            }
            doc.Blocks.Add(paragrafo);
        }

        /// <summary>
        /// Titolo in grassetto e le righe successive a capo, sotto.
        /// Esempio: RigaACapo(doc, 26, "Argomento:", p.Argomento)
        /// </summary>
        private static void RigaACapo(FlowDocument doc, double dimensione, string titolo, params string[] righe)
        {
            var paragrafo = new Paragraph { FontSize = dimensione, Margin = new Thickness(0, 0, 0, SpazioTraRighe) };
            paragrafo.Inlines.Add(new Bold(new Run(Valore(titolo))));
            foreach (var r in righe)
            {
                paragrafo.Inlines.Add(new LineBreak());
                paragrafo.Inlines.Add(new Run(Valore(r)));
            }
            doc.Blocks.Add(paragrafo);
        }

        /// <summary>Testo centrato (titoli, firma).</summary>
        private static void Titolo(FlowDocument doc, double dimensione, string testo, bool grassetto = true)
        {
            doc.Blocks.Add(new Paragraph(new Run(Valore(testo)))
            {
                FontSize = dimensione,
                FontWeight = grassetto ? FontWeights.Bold : FontWeights.Normal,
                TextAlignment = TextAlignment.Center,
                Margin = new Thickness(0, 0, 0, SpazioTraRighe)
            });
        }

        /// <summary>Spazio vuoto verticale (in punti).</summary>
        private static void Spazio(FlowDocument doc, double altezza)
        {
            doc.Blocks.Add(new Paragraph { Margin = new Thickness(0, altezza, 0, 0), FontSize = 1 });
        }

        private static string Casella(bool segnata) => segnata ? "( X ) " : "(    ) ";

        private static int Colli(DatiStampa dati) =>
            int.TryParse(dati?.NumeroColli, out int n) ? n : 0;

        private static string Valore(string v) => string.IsNullOrWhiteSpace(v) ? "" : v;

        private static string Linea(string v) => string.IsNullOrWhiteSpace(v) ? "__________" : v;


        // ══════════════════════════════════════════════════════════
        // Creazione documento e stampa (non serve modificarli)
        // ══════════════════════════════════════════════════════════

        public FlowDocument CreaDocumentoAnteprima(Protocolli p, TipoStampa tipo, DatiStampa dati = null)
        {
            var doc = CreaDocumento(p, tipo, dati);
            doc.PagePadding = new Thickness(50);
            return doc;
        }

        public void Stampa_Documento(Protocolli p, TipoStampa tipo, DatiStampa dati = null)
        {
            if (p == null)
                return;

            var dialog = new PrintDialog();
            if (dialog.ShowDialog() != true)
                return;

            var doc = CreaDocumento(p, tipo, dati);
            doc.PageWidth = dialog.PrintableAreaWidth;
            doc.PageHeight = dialog.PrintableAreaHeight;
            doc.PagePadding = new Thickness(60);
            doc.ColumnGap = 0;
            doc.ColumnWidth = dialog.PrintableAreaWidth;

            string titolo = tipo == TipoStampa.Trasmissione ? TitoloTrasm : "Stampa Marsau";
            IDocumentPaginatorSource sorgente = doc;
            dialog.PrintDocument(sorgente.DocumentPaginator, titolo);
        }

        private FlowDocument CreaDocumento(Protocolli p, TipoStampa tipo, DatiStampa dati)
        {
            var doc = new FlowDocument
            {
                FontFamily = new FontFamily(Carattere),
                FontSize = TestoNormale,
                Foreground = Brushes.Black,
                Background = Brushes.White,
                ColumnWidth = double.PositiveInfinity
            };

            if (p == null)
                return doc;

            if (tipo == TipoStampa.Trasmissione)
                CreaTrasmissione(doc, p, dati);
            else
                CreaCollaudo(doc, p, dati);

            return doc;
        }
    }
}
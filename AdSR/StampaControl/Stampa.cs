using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using AdSR.Models;

namespace AdSR.Common
{
    public class Stampa
    {
        /// <summary>
        /// Genera un FlowDocument con i dati del Protocollo (Marsau)
        /// e apre la finestra di stampa di Windows.
        /// </summary>
        public void Stampa_Documento(Protocolli p)
        {
            if (p == null)
                throw new ArgumentNullException(nameof(p));

            FlowDocument documento = CostruisciDocumento(p);

            PrintDialog printDialog = new PrintDialog();

            if (printDialog.ShowDialog() == true)
            {
                documento.PageWidth = printDialog.PrintableAreaWidth;
                documento.PageHeight = printDialog.PrintableAreaHeight;
                documento.PagePadding = new Thickness(40);
                documento.ColumnWidth = printDialog.PrintableAreaWidth;

                IDocumentPaginatorSource paginatorSource = documento;
                printDialog.PrintDocument(paginatorSource.DocumentPaginator, "Marsau - " + p.Ditta);
            }
        }

        /// <summary>
        /// Costruisce il FlowDocument per l'anteprima (usato dal ViewModel per il binding).
        /// </summary>
        public FlowDocument CreaDocumentoAnteprima(Protocolli p)
        {
            if (p == null)
                throw new ArgumentNullException(nameof(p));

            return CostruisciDocumento(p);
        }

        private FlowDocument CostruisciDocumento(Protocolli p)
        {
            var doc = new FlowDocument
            {
                FontFamily = new FontFamily("Segoe UI"),
                FontSize = 13,
                PagePadding = new Thickness(40),
                Foreground = Brushes.Black
            };

            var header1 = new Paragraph(new Run("ARSENALE MILITARE MARITTIMO"))
            {
                FontSize = 20,
                FontWeight = FontWeights.SemiBold,
                TextAlignment = TextAlignment.Center,
                Margin = new Thickness(0, 0, 0, 2)
            };
            var header2 = new Paragraph(new Run("AUGUSTA"))
            {
                FontSize = 20,
                FontWeight = FontWeights.SemiBold,
                TextAlignment = TextAlignment.Center,
                Margin = new Thickness(0, 0, 0, 2)
            };
            var header3 = new Paragraph(new Run("SETTORE SALA RICEZIONE"))
            {
                FontSize = 20,
                FontWeight = FontWeights.SemiBold,
                TextAlignment = TextAlignment.Center,
                Margin = new Thickness(0, 0, 0, 20)
            };

            doc.Blocks.Add(header1);
            doc.Blocks.Add(header2);
            doc.Blocks.Add(header3);

            doc.Blocks.Add(CreaParagrafoCentrato("DITTA", 14, FontWeights.SemiBold));
            doc.Blocks.Add(CreaParagrafoCentrato(
                $"{Val(p.Ditta)} del {Val(p.MarsauData)}", 22, FontWeights.Bold));
            doc.Blocks.Add(CreaParagrafoCentrato(
                $"Richiesta {Val(p.Richiesta)}", 22, FontWeights.Bold));
            doc.Blocks.Add(CreaParagrafoCentrato(
                $"ATTO: {Val(p.Atto)}", 22, FontWeights.Bold));
            doc.Blocks.Add(CreaParagrafoCentrato("ARGOMENTO:", 22, FontWeights.Bold));

            var argomentoParagraph = new Paragraph(new Run(Val(p.Argomento).ToUpperInvariant()))
            {
                FontSize = 15,
                FontWeight = FontWeights.Medium,
                Padding = new Thickness(14, 12, 14, 12),
                Margin = new Thickness(20, 15, 20, 15),
                TextAlignment = TextAlignment.Left
            };

            var argomentoTable = new Table();
            var col = new TableColumn();
            argomentoTable.Columns.Add(col);
            var rowGroup = new TableRowGroup();
            var row = new TableRow();
            var cell = new TableCell(argomentoParagraph)
            {
                BorderBrush = Brushes.Black,
                BorderThickness = new Thickness(1, 1, 1, 1),
                Padding = new Thickness(0)
            };
            row.Cells.Add(cell);
            rowGroup.Rows.Add(row);
            argomentoTable.RowGroups.Add(rowGroup);
            argomentoTable.Margin = new Thickness(30, 10, 30, 20);

            doc.Blocks.Add(argomentoTable);

            return doc;
        }

        private Paragraph CreaParagrafoCentrato(string testo, double fontSize, FontWeight peso)
        {
            return new Paragraph(new Run(testo))
            {
                FontSize = fontSize,
                FontWeight = peso,
                TextAlignment = TextAlignment.Center,
                Margin = new Thickness(0, 0, 0, 6)
            };
        }

        private string Val(string s)
        {
            return string.IsNullOrWhiteSpace(s) ? " " : s;
        }
    }
}
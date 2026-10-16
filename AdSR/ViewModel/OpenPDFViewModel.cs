using AdSR.Common;
using AdSR.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace AdSR.OpenPDFControl
{
    public class OpenPDFViewModel : INotifyPropertyChanged
    {
        private readonly string _cartella;

        public OpenPDFViewModel(string cartella, string titolo)
        {
            _cartella = cartella;
            Titolo = titolo;

            AggiornaCommand = new RelayCommand(CaricaDocumenti);
            ApriEsternoCommand = new RelayCommand(ApriEsterno);
            ApriEsploraCommand = new RelayCommand(ApriEsplora);

            CaricaDocumenti();
        }

        public string Titolo { get; }
        public string Cartella => _cartella;

        public ObservableCollection<DocumentoPdf> Documenti { get; } = new ObservableCollection<DocumentoPdf>();

        private DocumentoPdf _documentoSelezionato;
        public DocumentoPdf DocumentoSelezionato
        {
            get => _documentoSelezionato;
            set
            {
                if (_documentoSelezionato == value) return;
                _documentoSelezionato = value;
                OnPropertyChanged(nameof(DocumentoSelezionato));

                // WebView2 non accetta Source = null.
                // UriBuilder: il frammento "#view=FitH" resta un parametro e non entra nel nome file
                PdfUri = value != null && File.Exists(value.Percorso)
                    ? new UriBuilder(new Uri(value.Percorso)) { Fragment = "view=FitH" }.Uri
                    : new Uri("about:blank");
            }
        }

        private Uri _pdfUri = new Uri("about:blank");
        public Uri PdfUri
        {
            get => _pdfUri;
            private set { _pdfUri = value; OnPropertyChanged(nameof(PdfUri)); }
        }

        public ICommand AggiornaCommand { get; }
        public ICommand ApriEsternoCommand { get; }
        public ICommand ApriEsploraCommand { get; }

        private void CaricaDocumenti()
        {
            Documenti.Clear();
            if (!Directory.Exists(_cartella)) return;

            string radice = _cartella.TrimEnd('\\');

            foreach (var f in TrovaPdf(new DirectoryInfo(_cartella))
                                 .OrderByDescending(f => f.LastWriteTime))
            {
                var rel = f.DirectoryName.Length > radice.Length
                    ? f.DirectoryName.Substring(radice.Length).TrimStart('\\')
                    : string.Empty;

                Documenti.Add(new DocumentoPdf
                {
                    Nome = f.Name,
                    Percorso = f.FullName,
                    Sottocartella = rel,
                    DataModifica = f.LastWriteTime
                });
            }

            OnPropertyChanged(nameof(Documenti));
            DocumentoSelezionato = Documenti.FirstOrDefault();
        }

        // Ricorsione manuale: una sottocartella senza permessi non blocca le altre
        private static IEnumerable<FileInfo> TrovaPdf(DirectoryInfo dir)
        {
            var risultato = new List<FileInfo>();
            try
            {
                risultato.AddRange(dir.GetFiles("*.pdf"));
                foreach (var sub in dir.GetDirectories())
                    risultato.AddRange(TrovaPdf(sub));
            }
            catch (UnauthorizedAccessException) { }
            catch (IOException) { }
            return risultato;
        }

        private void ApriEsterno()
        {
            if (DocumentoSelezionato == null) return;
            Avvia(DocumentoSelezionato.Percorso);
        }

        private void ApriEsplora()
        {
            try
            {
                if (DocumentoSelezionato != null && File.Exists(DocumentoSelezionato.Percorso))
                    Process.Start("explorer.exe", $"/select,\"{DocumentoSelezionato.Percorso}\"");
                else
                    Avvia(_cartella);
            }
            catch (Exception ex)
            {
                MarinaMessageBox.Show($"Impossibile aprire Esplora risorse:\n{ex.Message}", "Errore",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private static void Avvia(string percorso)
        {
            try
            {
                Process.Start(new ProcessStartInfo { FileName = percorso, UseShellExecute = true });
            }
            catch (Exception ex)
            {
                MarinaMessageBox.Show($"Impossibile aprire:\n{ex.Message}", "Errore",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
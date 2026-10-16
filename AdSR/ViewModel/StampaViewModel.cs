using AdSR.Common;
using AdSR.Models;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;

namespace AdSR.StampaControl
{
    public class StampaViewModel : INotifyPropertyChanged
    {
        private readonly Stampa _stampa;
        private readonly TipoStampa _tipo;
        private Window _window;
        private Protocolli _marsauSelezionato;
        private FlowDocument _documentoStampa;
        private DatiStampa _datiStampa;

        public FlowDocument DocumentoStampa
        {
            get => _documentoStampa;
            set
            {
                if (_documentoStampa != value)
                {
                    _documentoStampa = value;
                    OnPropertyChanged(nameof(DocumentoStampa));
                }
            }
        }

        /// <summary>Titolo mostrato nella barra dell'anteprima.</summary>
        public string Titolo => _tipo == TipoStampa.Trasmissione
            ? "Anteprima trasmissione bolla di carico"
            : "Anteprima di stampa";

        public ICommand StampaCommand { get; }
        public ICommand ChiudiCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        public StampaViewModel() : this(TipoStampa.Collaudo)
        {
        }

        public StampaViewModel(TipoStampa tipo)
        {
            _tipo = tipo;
            _stampa = new Stampa();

            StampaCommand = new RelayCommand(EseguiStampa);
            ChiudiCommand = new RelayCommand(Chiudi);
        }

        public void SetWindow(Window window)
        {
            _window = window;
        }

        public void SetMarsau(Protocolli marsau)
        {
            _marsauSelezionato = marsau;

            // Valori proposti nella finestra dati
            _datiStampa = new DatiStampa
            {
                DataIntroduzione = marsau?.DataIntroduzione,
                Giorni = marsau?.Giorni
            };

            DocumentoStampa = _stampa.CreaDocumentoAnteprima(marsau, _tipo, _datiStampa);
        }

        private void EseguiStampa()
        {
            if (_marsauSelezionato == null)
                return;

            // 1. L'utente inserisce i dati richiesti dal foglio
            var view = new DatiStampaView();
            var vm = new DatiStampaViewModel(_datiStampa, _tipo);
            vm.SetWindow(view);
            view.DataContext = vm;
            view.Owner = _window;

            if (view.ShowDialog() != true || vm.Risultato == null)
                return; // annullato

            _datiStampa = vm.Risultato;

            // 2. Aggiorna l'anteprima con i dati scelti
            DocumentoStampa = _stampa.CreaDocumentoAnteprima(_marsauSelezionato, _tipo, _datiStampa);

            // 3. Stampa
            try
            {
                _stampa.Stampa_Documento(_marsauSelezionato, _tipo, _datiStampa);
            }
            catch (Exception ex)
            {
                MarinaMessageBox.Show("Errore durante la stampa: " + ex.Message,
                    "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Chiudi()
        {
            _window?.Close();
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
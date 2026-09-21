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
        private Window _window;
        private Protocolli _marsauSelezionato;
        private FlowDocument _documentoStampa;

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

        public ICommand StampaCommand { get; }
        public ICommand ChiudiCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        public StampaViewModel()
        {
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
            DocumentoStampa = _stampa.CreaDocumentoAnteprima(marsau);
        }

        private void EseguiStampa()
        {
            if (_marsauSelezionato == null)
                return;

            try
            {
                _stampa.Stampa_Documento(_marsauSelezionato);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Errore durante la stampa: " + ex.Message,
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
using AdSR.Common;
using AdSR.Models;
using AdSR.ServiceRepository;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace AdSR.ModificaControl
{
    public class ModificaViewModel : INotifyPropertyChanged
    {
        private readonly IProtocolliRepository _protocolliRepository;
        private Window _window;
        private Protocolli _marsauSelezionato;

        public Protocolli MarsauSelezionato
        {
            get => _marsauSelezionato;
            set
            {
                if (_marsauSelezionato != value)
                {
                    _marsauSelezionato = value;
                    OnPropertyChanged(nameof(MarsauSelezionato));
                    OnPropertyChanged(nameof(Argomento));
                }
            }
        }

        public string Argomento
        {
            get => _marsauSelezionato?.Argomento;
            set
            {
                if (_marsauSelezionato != null && _marsauSelezionato.Argomento != value)
                {
                    _marsauSelezionato.Argomento = value;
                    OnPropertyChanged(nameof(Argomento));
                }
            }
        }

        public ICommand SalvaCommand { get; }
        public ICommand AnnullaCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        public ModificaViewModel(IProtocolliRepository protocolliRepository)
        {
            _protocolliRepository = protocolliRepository;

            SalvaCommand = new RelayCommand(Salva);
            AnnullaCommand = new RelayCommand(Annulla);
        }

        public void SetWindow(Window window)
        {
            _window = window;
        }

        public void SetMarsau(Protocolli marsau)
        {
            MarsauSelezionato = marsau;
        }

        private void Salva()
        {
            if (MarsauSelezionato == null)
                return;

            try
            {
                _protocolliRepository.ProtocolliUpdate(MarsauSelezionato);
                MessageBox.Show("Modifiche salvate correttamente.", "Successo",
                    MessageBoxButton.OK, MessageBoxImage.Information);

                _window?.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Errore durante il salvataggio: " + ex.Message,
                    "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Annulla()
        {
            _window?.Close();
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
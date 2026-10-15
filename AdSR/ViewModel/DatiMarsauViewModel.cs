using AdSR.Common;
using AdSR.Models;
using AdSR.ModificaControl;
using AdSR.ServiceRepository;
using AdSR.StampaControl;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace AdSR.DatiMarsau
{
    public class DatiMarsauViewModel : INotifyPropertyChanged
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

        public ICommand ChiudiCommand { get; }
        public ICommand OpenModificaCommand { get; }
        public ICommand OpenCartellaCommand { get; }
        public ICommand StampaCommand { get; }
        public ICommand TrasmissioneCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        public DatiMarsauViewModel(IProtocolliRepository protocolliRepository)
        {
            _protocolliRepository = protocolliRepository;

            ChiudiCommand = new RelayCommand(Chiudi);
            OpenModificaCommand = new RelayCommand(ApriModifica);
            OpenCartellaCommand = new RelayCommand(ApriCartella);
            StampaCommand = new RelayCommand(Stampa);
            TrasmissioneCommand = new RelayCommand(Trasmissione);
        }

        public void SetWindow(Window window)
        {
            _window = window;
        }

        public void SetMarsau(Protocolli marsau)
        {
            if (marsau == null)
                return;

            try
            {
                var refreshed = _protocolliRepository.ProtocolliGetAll()
                    .FirstOrDefault(p => p.Marsau == marsau.Marsau);

                // Forza il refresh dei binding anche se l'istanza è la stessa
                _marsauSelezionato = null;
                MarsauSelezionato = refreshed ?? marsau;
            }
            catch
            {
                _marsauSelezionato = null;
                MarsauSelezionato = marsau;
            }
        }

        private void Stampa()
        {
            if (MarsauSelezionato == null)
                return;

            var stampaWindow = new StampaView(MarsauSelezionato);
            stampaWindow.Owner = _window;
            stampaWindow.ShowDialog();
        }

        private void Trasmissione()
        {
            if (MarsauSelezionato == null)
                return;

            var stampaWindow = new StampaView(MarsauSelezionato, TipoStampa.Trasmissione);
            stampaWindow.Owner = _window;
            stampaWindow.ShowDialog();
        }

        private void ApriCartella()
        {
            if (MarsauSelezionato == null)
                return;

            string cartella = MarsauSelezionato.Cartella;

            if (string.IsNullOrWhiteSpace(cartella))
            {
                MessageBox.Show("Nessun percorso cartella associato a questo protocollo.",
                    "Attenzione", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!Directory.Exists(cartella))
            {
                MessageBox.Show($"La cartella non esiste o non è raggiungibile:\n{cartella}",
                    "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = cartella,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Impossibile aprire la cartella:\n{ex.Message}",
                    "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ApriModifica()
        {
            if (MarsauSelezionato == null)
                return;

            var view = new ModificaView();
            var vm = new ModificaViewModel(_protocolliRepository);

            vm.SetWindow(view);                 // necessario per chiudere da Salva/Annulla
            vm.SetMarsau(MarsauSelezionato);    // il VM lavora su una copia

            view.DataContext = vm;
            view.Owner = _window;
            view.ShowDialog();

            // Ricarica i dati (se si è salvato, l'originale è aggiornato;
            // se si è annullato, l'originale è rimasto intatto)
            SetMarsau(MarsauSelezionato);
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
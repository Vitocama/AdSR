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
        private IProtocolliRepository _protocolliRepository;
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
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MarsauSelezionato)));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Argomento)));
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
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Argomento)));
                }
            }
        }

        public ICommand ChiudiCommand { get; }
        public ICommand OpenModificaCommand { get; }
        public ICommand OpenCartellaCommand { get; }
        public ICommand StampaCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        public DatiMarsauViewModel(IProtocolliRepository protocolliRepository)
        {
            _protocolliRepository = protocolliRepository;
            ChiudiCommand = new RelayCommand(Chiudi);
            OpenModificaCommand = new RelayCommand(ApriModifica);
            OpenCartellaCommand = new RelayCommand(ApriCartella);
            StampaCommand = new RelayCommand(Stampa);
        }

        private void Stampa()
        {
            if (MarsauSelezionato == null)
                return;

            var stampaWindow = new StampaView(MarsauSelezionato);
            stampaWindow.ShowDialog();
          
        }

        private void ApriCartella()
        {
            if (MarsauSelezionato == null) return;

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
            vm.SetMarsau(MarsauSelezionato);
            view.DataContext = vm;
            view.ShowDialog();

            SetMarsau(MarsauSelezionato);
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
                MarsauSelezionato = refreshed ?? marsau;
            }
            catch
            {
                MarsauSelezionato = marsau;
            }
        }

        private void Chiudi()
        {
            _window?.Close();
        }
    }
}
using AdSR.Common;
using AdSR.MarsauControl;
using AdSR.Models;
using AdSR.ServiceRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace AdSR
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public MainViewModel()
        {
            VerificaCommand = new RelayCommand(async () => await VerificaAsync());
            EntraCommand = new RelayCommand(Entra);

            // La verifica parte dal ViewModel alla creazione, non dal code-behind
            _ = VerificaAsync();
        }

        private string _percorsoDb;
        public string PercorsoDb
        {
            get => _percorsoDb;
            private set { _percorsoDb = value; OnPropertyChanged(nameof(PercorsoDb)); }
        }

        private bool _connesso;
        public bool Connesso
        {
            get => _connesso;
            private set { _connesso = value; OnPropertyChanged(nameof(Connesso)); }
        }

        private bool _inVerifica;
        public bool InVerifica
        {
            get => _inVerifica;
            private set { _inVerifica = value; OnPropertyChanged(nameof(InVerifica)); }
        }

        private string _stato;
        public string Stato
        {
            get => _stato;
            private set { _stato = value; OnPropertyChanged(nameof(Stato)); }
        }

        public ICommand VerificaCommand { get; }
        public ICommand EntraCommand { get; }

        public async Task VerificaAsync()
        {
            if (InVerifica) return;

            InVerifica = true;
            Connesso = false;
            Stato = "Verifica connessione in corso...";

            try
            {
                var esito = await Task.Run(() =>
                {
                    using (var ctx = new AdSRContext())
                    {
                        string percorso = ctx.Database.GetDbConnection().DataSource;

                        if (!ctx.Database.CanConnect())
                            return (percorso, ok: false, totale: 0);

                        int totale = ctx.Protocolli.Count();
                        return (percorso, ok: true, totale);
                    }
                });

                PercorsoDb = esito.percorso;
                Connesso = esito.ok;
                Stato = esito.ok
                    ? $"{esito.totale} protocolli presenti."
                    : "Database non raggiungibile. Verificare la rete e premere RIPROVA.";
            }
            catch (Exception ex)
            {
                Stato = $"Errore di connessione: {ex.GetBaseException().Message}";
            }
            finally
            {
                InVerifica = false;
            }
        }

        // CONSULTA DATABASE → apre la pagina Marsau
        private void Entra()
        {
            try
            {
                IProtocolliRepository repo = new ProtocolliService();

                var view = new MarsauView();
                var vm = new MarsauViewModel(repo);

                view.DataContext = vm;
                view.Owner = Application.Current.MainWindow;
                view.ShowDialog();
            }
            catch (Exception ex)
            {
                MarinaMessageBox.Show($"Impossibile aprire la consultazione:\n{ex.GetBaseException().Message}",
                    "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
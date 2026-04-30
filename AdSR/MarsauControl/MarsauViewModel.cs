using AdSR.Common;
using AdSR.DatiMarsau;
using AdSR.Models;
using AdSR.ServiceRepository;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace AdSR.MarsauControl
{
    public class MarsauViewModel : INotifyPropertyChanged
    {
        private readonly IProtocolliRepository _protocolliRepository;
        private ObservableCollection<Protocolli> _marsauLista;
        private ObservableCollection<Protocolli> _marsauFiltratiLista;
        private string _filtroRicerca = string.Empty;
        private Protocolli _marsauSelezionato;
        private bool _isFormVisible = false;
        private Protocolli _nuovoMarsau;

        public ICommand SearchCommand { get; }
        public ICommand AggiungiCommand { get; }
        public ICommand SalvaCommand { get; }
        public ICommand AnnullaCommand { get; }
        public ICommand apriDatiMarsau { get; }
        public ICommand SfogliaCartellaCommand { get; }

        public string CartellaPath
        {
            get => NuovoMarsau?.Cartella;
            set
            {
                if (NuovoMarsau != null && NuovoMarsau.Cartella != value)
                {
                    NuovoMarsau.Cartella = value;
                    RaisePropertyChanged();
                }
            }
        }

        public MarsauViewModel(IProtocolliRepository protocolliRepository)
        {
            _protocolliRepository = protocolliRepository;
            MarsauLista = new ObservableCollection<Protocolli>(_protocolliRepository.ProtocolliGetAll());
            MarsauFiltratiLista = new ObservableCollection<Protocolli>(MarsauLista);
            NuovoMarsau = new Protocolli();

            SfogliaCartellaCommand = new RelayCommand(SfogliaCartella);
            SearchCommand = new RelayCommand(Filter);
            AggiungiCommand = new RelayCommand(Aggiungi);
            SalvaCommand = new RelayCommand(Salva);
            AnnullaCommand = new RelayCommand(Annulla);
            apriDatiMarsau = new RelayCommand(ApriDatiMarsau);
        }

        private void SfogliaCartella()
        {
            var dialog = new Microsoft.Win32.SaveFileDialog
            {
                Title = "Seleziona la cartella dei documenti del Marsau",
                FileName = "Seleziona cartella",  // nome suggerito, l'utente lo ignora
                Filter = "Cartella|*.this.directory",
                CheckFileExists = false,
                CheckPathExists = true
            };

            if (!string.IsNullOrWhiteSpace(NuovoMarsau.Cartella) &&
                System.IO.Directory.Exists(NuovoMarsau.Cartella))
            {
                dialog.InitialDirectory = NuovoMarsau.Cartella;
            }

            if (dialog.ShowDialog() == true)
            {
                // Estrai solo il percorso della cartella, ignorando il nome file
                string percorsoCartella = System.IO.Path.GetDirectoryName(dialog.FileName);
                NuovoMarsau.Cartella = percorsoCartella;
                RaisePropertyChanged(nameof(CartellaPath));
            }
        }

        private void ApriDatiMarsau()
        {
            if (MarsauSelezionato == null)
            {
                MessageBox.Show("Nessun Marsau selezionato!");
                return;
            }

            var view = new DatiMarsauView();
            var vm = new DatiMarsauViewModel(_protocolliRepository);
            vm.SetWindow(view);
            vm.SetMarsau(MarsauSelezionato);
            view.DataContext = vm;

            view.ShowDialog();

            // Dopo la chiusura, ricarica il DataGrid per mostrare eventuali modifiche
            RicaricaLista();
        }

        private void RicaricaLista()
        {
            MarsauLista = new ObservableCollection<Protocolli>(_protocolliRepository.ProtocolliGetAll());
            MarsauFiltratiLista = new ObservableCollection<Protocolli>(MarsauLista);
        }

        private void Filter()
        {
            MarsauFiltratiLista.Clear();

            if (string.IsNullOrWhiteSpace(FiltroRicerca))
            {
                foreach (var marsau in MarsauLista)
                    MarsauFiltratiLista.Add(marsau);
            }
            else
            {
                var filtro = FiltroRicerca.ToLower();
                var risultati = MarsauLista.Where(p =>
                    (p.Marsau != null && p.Marsau.ToLower().Contains(filtro)) ||
                    (p.Rdo != null && p.Rdo.ToLower().Contains(filtro)) ||
                    (p.Atto != null && p.Atto.ToLower().Contains(filtro)) ||
                    (p.Contratto != null && p.Contratto.ToLower().Contains(filtro)) ||
                    (p.Importo != null && p.Importo.ToLower().Contains(filtro)) ||
                    (p.Note != null && p.Note.ToLower().Contains(filtro)) ||
                    (p.Ditta != null && p.Ditta.ToLower().Contains(filtro)) ||
                    (p.Argomento != null && p.Argomento.ToLower().Contains(filtro)) ||
                    (p.Lotto != null && p.Lotto.ToLower().Contains(filtro)) ||
                    (p.PdC != null && p.PdC.ToLower().Contains(filtro))
                );

                foreach (var marsau in risultati)
                    MarsauFiltratiLista.Add(marsau);
            }
        }

        private void Aggiungi()
        {
            IsFormVisible = true;
        }

        private void Salva()
        {
            string random = new Random(1000).Next(10000, 99999).ToString();

            if (string.IsNullOrWhiteSpace(NuovoMarsau.Marsau) &&
                string.IsNullOrWhiteSpace(NuovoMarsau.Rdo) &&
                string.IsNullOrWhiteSpace(NuovoMarsau.Atto))
            {
                MessageBox.Show("Inserisci almeno Marsau, RDO o Atto!", "Avviso",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                // Gestione sicura del campo Giorni (può essere null o stringa breve)
                string giorni = NuovoMarsau.Giorni;
                if (!string.IsNullOrWhiteSpace(giorni) && giorni.Length >= 10)
                {
                    giorni = giorni.Substring(0, 10).Replace("/", ".");
                }

                var nuovoMarsau = new Protocolli
                {
                    // ─── DATI IDENTIFICATIVI ───
                    Marsau = $"{NuovoMarsau.Marsau}-{random}",
                    MarsauData = NuovoMarsau.MarsauData,
                    Ditta = NuovoMarsau.Ditta,
                    Rdo = NuovoMarsau.Rdo,
                    Argomento = NuovoMarsau.Argomento,
                    Atto = NuovoMarsau.Atto,
                    Contratto = NuovoMarsau.Contratto,
                    DataIntroduzione = NuovoMarsau.DataIntroduzione,

                    // ─── RICHIESTA E IMPORTI ───
                    Richiesta = NuovoMarsau.Richiesta,
                    Lotto = NuovoMarsau.Lotto,
                    Importo = NuovoMarsau.Importo,
                    Giorni = giorni,
                    Ric = NuovoMarsau.Ric,

                    // ─── FORNITURE E SISTEMA ───
                    Forniture = NuovoMarsau.Forniture,
                    Siga45 = NuovoMarsau.Siga45,

                    // ─── CRE / DDT / JOSH ───
                    Cre = NuovoMarsau.Cre,
                    DataCre = NuovoMarsau.DataCre,
                    Josh = NuovoMarsau.Josh,
                    Ddt = NuovoMarsau.Ddt,
                    DataDdt = NuovoMarsau.DataDdt,

                    // ─── FATTURAZIONE ───
                    Fattura = NuovoMarsau.Fattura,
                    DataFattura = NuovoMarsau.DataFattura,
                    ImportoFattura = NuovoMarsau.ImportoFattura,

                    // ─── BDC / COLLAUDO ───
                    Bdc = NuovoMarsau.Bdc,
                    DataBdc = NuovoMarsau.DataBdc,
                    Collaudo = NuovoMarsau.Collaudo,

                    // ─── ALTRO ───
                    PdC = NuovoMarsau.PdC,
                    Cartella = NuovoMarsau.Cartella,
                    Note = NuovoMarsau.Note
                };

                _protocolliRepository.ProtocolliInsert(nuovoMarsau);

                // Ricarica il DataGrid
                RicaricaLista();

                // Nascondi il form
                IsFormVisible = false;

                // Resetta il form
                NuovoMarsau = new Protocolli();

                MessageBox.Show("Marsau salvato con successo!", "Successo",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Errore: " + ex.Message, "Errore",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Annulla()
        {
            IsFormVisible = false;
            NuovoMarsau = new Protocolli();
        }

        public ObservableCollection<Protocolli> MarsauLista
        {
            get { return _marsauLista; }
            set
            {
                if (_marsauLista != value)
                {
                    _marsauLista = value;
                    RaisePropertyChanged();
                }
            }
        }

        public ObservableCollection<Protocolli> MarsauFiltratiLista
        {
            get { return _marsauFiltratiLista; }
            set
            {
                if (_marsauFiltratiLista != value)
                {
                    _marsauFiltratiLista = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string FiltroRicerca
        {
            get { return _filtroRicerca; }
            set
            {
                if (_filtroRicerca != value)
                {
                    _filtroRicerca = value;
                    RaisePropertyChanged();
                }
            }
        }

        public Protocolli MarsauSelezionato
        {
            get { return _marsauSelezionato; }
            set
            {
                if (_marsauSelezionato != value)
                {
                    _marsauSelezionato = value;
                    RaisePropertyChanged();
                }
            }
        }

        public bool IsFormVisible
        {
            get { return _isFormVisible; }
            set
            {
                if (_isFormVisible != value)
                {
                    _isFormVisible = value;
                    RaisePropertyChanged();
                }
            }
        }

        public Protocolli NuovoMarsau
        {
            get { return _nuovoMarsau; }
            set
            {
                if (_nuovoMarsau != value)
                {
                    _nuovoMarsau = value;
                    RaisePropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
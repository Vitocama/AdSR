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

            SearchCommand = new RelayCommand(Filter);
            AggiungiCommand = new RelayCommand(Aggiungi);
            SalvaCommand = new RelayCommand(Salva);
            AnnullaCommand = new RelayCommand(Annulla);
            apriDatiMarsau = new RelayCommand(ApriDatiMarsau);
        }

        

        private void ApriDatiMarsau()
        {
            if (MarsauSelezionato == null)
            {
                MarinaMessageBox.Show("Nessun Marsau selezionato!");
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
                    (p.Capitolo != null && p.Capitolo.ToLower().Contains(filtro)) ||
                    (p.PdC != null && p.PdC.ToLower().Contains(filtro)) ||
                    (p.PdG != null && p.PdG.ToLower().Contains(filtro)) ||
                    (p.Rup != null && p.Rup.ToLower().Contains(filtro)) ||
                    (p.Dec != null && p.Dec.ToLower().Contains(filtro))
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
            string random = Guid.NewGuid().ToString("N").Substring(0, 5).ToUpper();

            if (string.IsNullOrWhiteSpace(NuovoMarsau.Marsau) &&
                string.IsNullOrWhiteSpace(NuovoMarsau.Rdo) &&
                string.IsNullOrWhiteSpace(NuovoMarsau.Atto))
            {
                MarinaMessageBox.Show("Inserisci almeno Marsau, RDO o Atto!", "Avviso",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                // Gestione sicura del campo Giorni
                string giorni = NuovoMarsau.Giorni;
                if (!string.IsNullOrWhiteSpace(giorni) && giorni.Length >= 10)
                {
                    giorni = giorni.Substring(0, 10).Replace("/", ".");
                }

                var nuovoMarsau = new Protocolli
                {
                    Marsau = $"{NuovoMarsau.Marsau}-{random}",
                    MarsauData = NuovoMarsau.MarsauData,
                    Ditta = NuovoMarsau.Ditta,
                    Rdo = NuovoMarsau.Rdo,
                    Argomento = NuovoMarsau.Argomento,
                    Atto = NuovoMarsau.Atto,
                    Contratto = NuovoMarsau.Contratto,
                    DataIntroduzione = NuovoMarsau.DataIntroduzione,
                    Richiesta = NuovoMarsau.Richiesta,
                    Lotto = NuovoMarsau.Lotto,
                    Capitolo = NuovoMarsau.Capitolo,
                    Importo = NuovoMarsau.Importo,
                    Giorni = giorni,
                    Ric = NuovoMarsau.Ric,
                    Dec = NuovoMarsau.Dec,
                    Rup = NuovoMarsau.Rup,
                    AA = NuovoMarsau.AA,
                    Ad = NuovoMarsau.Ad,
                    Forniture = NuovoMarsau.Forniture,
                    Siga45 = NuovoMarsau.Siga45,
                    Cre = NuovoMarsau.Cre,
                    DataCre = NuovoMarsau.DataCre,
                    Josh = NuovoMarsau.Josh,
                    Ddt = NuovoMarsau.Ddt,
                    DataDdt = NuovoMarsau.DataDdt,
                    Pervenuto = NuovoMarsau.Pervenuto,
                    Ddt2 = NuovoMarsau.Ddt2,
                    DataDdt2 = NuovoMarsau.DataDdt2,
                    Pervenuto2 = NuovoMarsau.Pervenuto2,
                    Ddt3 = NuovoMarsau.Ddt3,
                    DataDdt3 = NuovoMarsau.DataDdt3,
                    Pervenuto3 = NuovoMarsau.Pervenuto3,
                    Trasmesso = NuovoMarsau.Trasmesso,
                    Fattura = NuovoMarsau.Fattura,
                    DataFattura = NuovoMarsau.DataFattura,
                    ImportoFattura = NuovoMarsau.ImportoFattura,
                    Bdc = NuovoMarsau.Bdc,
                    DataBdc = NuovoMarsau.DataBdc,
                    Collaudo = NuovoMarsau.Collaudo,
                    PdC = NuovoMarsau.PdC,
                    PdG = NuovoMarsau.PdG,
                    Cartella = NuovoMarsau.Cartella,
                    Note = NuovoMarsau.Note
                };

                _protocolliRepository.ProtocolliInsert(nuovoMarsau);

                RicaricaLista();
                IsFormVisible = false;
                NuovoMarsau = new Protocolli();

                MarinaMessageBox.Show("Marsau salvato con successo!", "Successo",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                string messaggio = "Errore: " + ex.Message;
                if (ex.InnerException != null)
                    messaggio += "\n\nDettaglio: " + ex.InnerException.Message;

                MarinaMessageBox.Show(messaggio, "Errore",
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
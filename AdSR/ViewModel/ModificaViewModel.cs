using AdSR.Common;
using AdSR.Models;
using AdSR.ServiceRepository;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace AdSR.ModificaControl
{
    public class ModificaViewModel : INotifyPropertyChanged
    {
        private readonly IProtocolliRepository _protocolliRepository;
        private Window _window;

        // Oggetto originale (eventualmente tracciato da EF): viene toccato solo su Salva
        private Protocolli _originale;

        // Copia di lavoro su cui agiscono i binding della view
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
            if (marsau == null)
                return;

            _originale = marsau;
            MarsauSelezionato = Clona(marsau);
        }

        private void Salva()
        {
            if (MarsauSelezionato == null || _originale == null)
                return;

            try
            {
                // Applica le modifiche della copia all'originale (che conserva l'Id) e salva
                CopiaValori(MarsauSelezionato, _originale);
                _protocolliRepository.ProtocolliUpdate(_originale);

                MarinaMessageBox.Show("Modifiche salvate correttamente.", "Successo",
                    MessageBoxButton.OK, MessageBoxImage.Information);

                _window?.Close();
            }
            catch (Exception ex)
            {
                string messaggio = "Errore durante il salvataggio: " + ex.Message;
                if (ex.InnerException != null)
                    messaggio += "\n\nDettaglio: " + ex.InnerException.Message;

                MarinaMessageBox.Show(messaggio, "Errore",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Annulla()
        {
            // La copia viene scartata: l'originale resta invariato
            _window?.Close();
        }

        private static Protocolli Clona(Protocolli origine)
        {
            var copia = new Protocolli();
            CopiaValori(origine, copia);
            return copia;
        }

        /// <summary>
        /// Copia esplicita di tutti i campi modificabili (Id escluso:
        /// resta quello dell'originale, così l'update colpisce il record giusto).
        /// Se aggiungi un campo al modello, aggiungilo anche qui.
        /// </summary>
        private static void CopiaValori(Protocolli da, Protocolli a)
        {
            // Dati identificativi
            a.Marsau = da.Marsau;
            a.MarsauData = da.MarsauData;
            a.Ditta = da.Ditta;
            a.Rdo = da.Rdo;
            a.Argomento = da.Argomento;
            a.Atto = da.Atto;
            a.Contratto = da.Contratto;

            // Richiesta e importi
            a.Richiesta = da.Richiesta;
            a.Lotto = da.Lotto;
            a.Capitolo = da.Capitolo;
            a.Importo = da.Importo;
            a.Giorni = da.Giorni;
            a.Ric = da.Ric;
            a.Dec = da.Dec;
            a.Rup = da.Rup;
            a.AA = da.AA;
            a.Ad = da.Ad;

            // Forniture e sistema
            a.Forniture = da.Forniture;
            a.Siga45 = da.Siga45;

            // CRE / DDT / Josh
            a.Cre = da.Cre;
            a.DataCre = da.DataCre;
            a.Josh = da.Josh;
            a.Ddt = da.Ddt;
            a.DataDdt = da.DataDdt;
            a.Pervenuto = da.Pervenuto;
            a.Ddt2 = da.Ddt2;
            a.DataDdt2 = da.DataDdt2;
            a.Pervenuto2 = da.Pervenuto2;
            a.Ddt3 = da.Ddt3;
            a.DataDdt3 = da.DataDdt3;
            a.Pervenuto3 = da.Pervenuto3;
            a.DataIntroduzione = da.DataIntroduzione;
            a.Trasmesso = da.Trasmesso;

            // Fatturazione
            a.Fattura = da.Fattura;
            a.DataFattura = da.DataFattura;
            a.ImportoFattura = da.ImportoFattura;

            // BDC / Collaudo
            a.Bdc = da.Bdc;
            a.DataBdc = da.DataBdc;
            a.Collaudo = da.Collaudo;

            // Altro
            a.PdC = da.PdC;
            a.PdG = da.PdG;
            a.Cartella = da.Cartella;
            a.Note = da.Note;
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
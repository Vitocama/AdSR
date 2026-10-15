using AdSR.Common;
using AdSR.Models;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace AdSR.StampaControl
{
    /// <summary>
    /// Finestra che chiede i dati all'utente prima di stampare.
    /// Collaudo:     Parziale/Completo, numero colli (data introduzione presa dal DB).
    /// Trasmissione: indirizzo, gg, nome firmatario.
    /// </summary>
    public class DatiStampaViewModel : INotifyPropertyChanged
    {
        private readonly TipoStampa _tipo;
        private Window _window;

        private string _numeroColli;
        private bool _parziale;
        private string _dataIntroduzione;
        private string _indirizzo;
        private string _giorni;
        private string _nomeFirmatario;

        public bool IsCollaudo => _tipo == TipoStampa.Collaudo;
        public bool IsTrasmissione => _tipo == TipoStampa.Trasmissione;

        // ── Collaudo ──
        public string NumeroColli
        {
            get => _numeroColli;
            set { _numeroColli = value; OnPropertyChanged(nameof(NumeroColli)); }
        }

        public bool Parziale
        {
            get => _parziale;
            set
            {
                _parziale = value;
                OnPropertyChanged(nameof(Parziale));
                OnPropertyChanged(nameof(Completo));
            }
        }

        public bool Completo
        {
            get => !_parziale;
            set { Parziale = !value; }
        }

        public string DataIntroduzione
        {
            get => _dataIntroduzione;
            set { _dataIntroduzione = value; OnPropertyChanged(nameof(DataIntroduzione)); }
        }

        // ── Trasmissione ──
        public string Indirizzo
        {
            get => _indirizzo;
            set { _indirizzo = value; OnPropertyChanged(nameof(Indirizzo)); }
        }

        public string Giorni
        {
            get => _giorni;
            set { _giorni = value; OnPropertyChanged(nameof(Giorni)); }
        }

        public string NomeFirmatario
        {
            get => _nomeFirmatario;
            set { _nomeFirmatario = value; OnPropertyChanged(nameof(NomeFirmatario)); }
        }

        /// <summary>Valori confermati (null se l'utente annulla).</summary>
        public DatiStampa Risultato { get; private set; }

        public ICommand ConfermaCommand { get; }
        public ICommand AnnullaCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        public DatiStampaViewModel(DatiStampa iniziali) : this(iniziali, TipoStampa.Collaudo)
        {
        }

        public DatiStampaViewModel(DatiStampa iniziali, TipoStampa tipo)
        {
            _tipo = tipo;

            _numeroColli = iniziali?.NumeroColli ?? "";
            _parziale = iniziali?.Parziale ?? false;
            _dataIntroduzione = string.IsNullOrWhiteSpace(iniziali?.DataIntroduzione)
                ? DateTime.Today.ToString("dd/MM/yyyy")
                : iniziali.DataIntroduzione;
            _indirizzo = iniziali?.Indirizzo ?? "";
            _giorni = iniziali?.Giorni ?? "";
            _nomeFirmatario = iniziali?.NomeFirmatario ?? "";

            ConfermaCommand = new RelayCommand(Conferma);
            AnnullaCommand = new RelayCommand(Annulla);
        }

        public void SetWindow(Window window)
        {
            _window = window;
        }

        private void Conferma()
        {
            int colli = 0;

            if (IsCollaudo)
            {
                // Vuoto = 0 colli (la scritta "colli n." non viene stampata)
                if (string.IsNullOrWhiteSpace(NumeroColli))
                    NumeroColli = "0";

                if (!int.TryParse(NumeroColli.Trim(), out colli) || colli < 0)
                {
                    MessageBox.Show("Inserisci un numero di colli valido (0 o più).",
                        "Dati stampa", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            }

            Risultato = new DatiStampa
            {
                NumeroColli = IsCollaudo ? colli.ToString() : NumeroColli,
                Parziale = Parziale,
                Completo = Completo,
                DataIntroduzione = DataIntroduzione?.Trim(),
                Indirizzo = Indirizzo?.Trim(),
                Giorni = Giorni?.Trim(),
                NomeFirmatario = NomeFirmatario?.Trim()
            };

            if (_window != null)
            {
                _window.DialogResult = true;
                _window.Close();
            }
        }

        private void Annulla()
        {
            Risultato = null;
            if (_window != null)
            {
                _window.DialogResult = false;
                _window.Close();
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
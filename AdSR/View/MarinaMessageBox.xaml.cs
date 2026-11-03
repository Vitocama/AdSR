using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace AdSR.Common
{
    public partial class MarinaMessageBox : Window
    {
        private MessageBoxResult _risultato;
        private readonly MessageBoxResult _risultatoEsc;

        private MarinaMessageBox(string messaggio, string titolo,
                                 MessageBoxButton pulsanti, MessageBoxImage icona)
        {
            InitializeComponent();

            TxtTitolo.Text = titolo;
            TxtMessaggio.Text = messaggio;

            // Risultato se si preme Esc o si chiude con Alt+F4
            switch (pulsanti)
            {
                case MessageBoxButton.OKCancel: _risultatoEsc = MessageBoxResult.Cancel; break;
                case MessageBoxButton.YesNo: _risultatoEsc = MessageBoxResult.No; break;
                case MessageBoxButton.YesNoCancel: _risultatoEsc = MessageBoxResult.Cancel; break;
                default: _risultatoEsc = MessageBoxResult.OK; break;
            }
            _risultato = _risultatoEsc;

            ImpostaIcona(icona);
            CreaPulsanti(pulsanti);
        }

        /// <summary>Stessa firma di MarinaMessageBox.Show.</summary>
        public static MessageBoxResult Show(string messaggio,
                                            string titolo = "AdSR",
                                            MessageBoxButton pulsanti = MessageBoxButton.OK,
                                            MessageBoxImage icona = MessageBoxImage.Information)
        {
            var dlg = new MarinaMessageBox(messaggio, titolo, pulsanti, icona);

            var owner = Application.Current?.Windows.OfType<Window>().FirstOrDefault(w => w.IsActive)
                        ?? Application.Current?.MainWindow;

            if (owner != null && owner.IsLoaded)
                dlg.Owner = owner;
            else
                dlg.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            dlg.ShowDialog();
            return dlg._risultato;
        }

        private void ImpostaIcona(MessageBoxImage icona)
        {
            string glifo;
            Color colore;
            Brush testo;

            switch (icona)
            {
                case MessageBoxImage.Warning:     // = Exclamation
                    glifo = "!"; colore = Color.FromRgb(0xE8, 0xA3, 0x3C); testo = new SolidColorBrush(Color.FromRgb(0x0F, 0x20, 0x44));
                    break;
                case MessageBoxImage.Error:       // = Hand, Stop
                    glifo = "✕"; colore = Color.FromRgb(0xE0, 0x4B, 0x4B); testo = Brushes.White;
                    break;
                case MessageBoxImage.Question:
                    glifo = "?"; colore = Color.FromRgb(0x3A, 0x6B, 0xA4); testo = Brushes.White;
                    break;
                case MessageBoxImage.Information: // = Asterisk
                    glifo = "i"; colore = Color.FromRgb(0xC9, 0xA8, 0x4C); testo = new SolidColorBrush(Color.FromRgb(0x0F, 0x20, 0x44));
                    break;
                default:
                    IconaBox.Visibility = Visibility.Collapsed;
                    return;
            }

            IconaBox.Background = new SolidColorBrush(colore);
            IconaGlow.Color = colore;
            IconaGlifo.Text = glifo;
            IconaGlifo.Foreground = testo;
        }

        private void CreaPulsanti(MessageBoxButton pulsanti)
        {
            switch (pulsanti)
            {
                case MessageBoxButton.OKCancel:
                    Aggiungi("ANNULLA", MessageBoxResult.Cancel, primario: false);
                    Aggiungi("OK", MessageBoxResult.OK, primario: true);
                    break;
                case MessageBoxButton.YesNo:
                    Aggiungi("NO", MessageBoxResult.No, primario: false);
                    Aggiungi("SÌ", MessageBoxResult.Yes, primario: true);
                    break;
                case MessageBoxButton.YesNoCancel:
                    Aggiungi("ANNULLA", MessageBoxResult.Cancel, primario: false);
                    Aggiungi("NO", MessageBoxResult.No, primario: false);
                    Aggiungi("SÌ", MessageBoxResult.Yes, primario: true);
                    break;
                default:
                    Aggiungi("OK", MessageBoxResult.OK, primario: true);
                    break;
            }
        }

        private void Aggiungi(string testo, MessageBoxResult risultato, bool primario)
        {
            var b = new Button
            {
                Content = testo,
                Style = (Style)FindResource(primario ? "MsgPrimario" : "MsgSecondario"),
                IsDefault = primario,               // Invio
                Margin = new Thickness(10, 0, 0, 0)
            };
            b.Click += (s, e) => { _risultato = risultato; Close(); };
            PannelloPulsanti.Children.Add(b);

            if (primario)
                Loaded += (s, e) => b.Focus();
        }

        private void Header_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
                DragMove();
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                _risultato = _risultatoEsc;
                Close();
            }
        }
    }
}
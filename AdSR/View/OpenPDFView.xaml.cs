using System;
using System.IO;
using System.Windows;

namespace AdSR.OpenPDFControl
{
    public partial class OpenPDFView : Window
    {
        public OpenPDFView(string cartella, string titolo)
        {
            // Cache WebView2 in LocalAppData: evita la finestra bianca se l'exe è su share di rete
            Environment.SetEnvironmentVariable("WEBVIEW2_USER_DATA_FOLDER",
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                             "AdSR", "WebView2"));

            InitializeComponent();
            DataContext = new OpenPDFViewModel(cartella, titolo);
        }
    }
}
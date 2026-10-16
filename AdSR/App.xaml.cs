using AdSR.Common;
using System.ComponentModel;
using System.Windows;

namespace AdSR
{
    /// <summary>
    /// Logica di interazione per App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var main = new MainWindow();
            main.Closing += MainWindow_Closing;

            MainWindow = main;
            main.Show();
        }

        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            var risposta = MarinaMessageBox.Show(
                "Vuoi chiudere AdSR?",
                "Conferma chiusura",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (risposta != MessageBoxResult.Yes)
                e.Cancel = true;   // annulla la chiusura
        }
    }
}
using AdSR.Models;
using System.Windows;

namespace AdSR.StampaControl
{
    public partial class StampaView : Window
    {
        // Pulsante STAMPA (foglio collaudo)
        public StampaView(Protocolli marsau) : this(marsau, TipoStampa.Collaudo)
        {
        }

        // Pulsante TRASMISSIONE oppure STAMPA
        public StampaView(Protocolli marsau, TipoStampa tipo)
        {
            InitializeComponent();

            var vm = new StampaViewModel(tipo);
            vm.SetWindow(this);
            vm.SetMarsau(marsau);
            DataContext = vm;
        }
    }
}
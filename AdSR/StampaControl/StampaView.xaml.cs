using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AdSR.StampaControl
{
    /// <summary>
    /// Logica di interazione per StampaView.xaml
    /// </summary>
    public partial class StampaView : Window
    {
        public StampaView(Models.Protocolli marsauSelezionato)
        {
            InitializeComponent();
            DataContext = new StampaViewModel();
        }
    }
}

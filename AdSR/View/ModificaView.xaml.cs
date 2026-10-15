using AdSR.ServiceRepository;
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

namespace AdSR.ModificaControl
{
    /// <summary>
    /// Logica di interazione per ModificaView.xaml
    /// </summary>
    public partial class ModificaView : Window
    {
        public ModificaView()
        {
            InitializeComponent();
            DataContext=new ModificaViewModel(new ProtocolliService());
        }
    }
}

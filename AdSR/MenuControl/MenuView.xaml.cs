using AdSR.ServiceRepository;
using System.Windows.Controls;

namespace AdSR.MenuControl
{
    public partial class MenuView : UserControl
    {
        public MenuView()
        {
            InitializeComponent();
            DataContext=new  MenuViewModel(new ProtocolliService());
        }
    }
}
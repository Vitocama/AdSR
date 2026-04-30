using AdSR.ServiceRepository;
using System.Windows;

namespace AdSR.MarsauControl
{
    public partial class MarsauView : Window
    {
        public MarsauView()
        {
            InitializeComponent();
            DataContext = new MarsauViewModel(new ProtocolliService());
        }
    }
}

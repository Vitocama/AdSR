using AdSR.Common;
using AdSR.MarsauControl;
using AdSR.ServiceRepository;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace AdSR.MenuControl
{
    public class MenuViewModel : INotifyPropertyChanged
    {
        private MarsauView _marsauView;
        private readonly IProtocolliRepository _protocolliRepository;

        public ICommand OpenMarsauCommand { get; }

        public ICommand OpenModificaCommand { get; }

        public MenuViewModel(IProtocolliRepository protocolliRepository)
        {
            _protocolliRepository = protocolliRepository;
            OpenMarsauCommand = new RelayCommand(OpenMarsau);
            OpenModificaCommand = new RelayCommand(ApriModifica);
        }

        private void ApriModifica()
{
  

    var view = new ModificaControl.ModificaView();
    var vm = new ModificaControl.ModificaViewModel(_protocolliRepository);
   
    view.DataContext = vm;

    view.ShowDialog();

    
}


        private void OpenMarsau()
        {
            if (_marsauView == null || !_marsauView.IsVisible)
            {
                _marsauView = new MarsauView();
                _marsauView.Closed += (s, e) => _marsauView = null;
                _marsauView.Show();
            }
            else
            {
                if (_marsauView.WindowState == WindowState.Minimized)
                    _marsauView.WindowState = WindowState.Normal;
                _marsauView.Activate();
                _marsauView.Focus();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace  AdSR.Common
{
    /// <summary>
    /// This class implements the INotifyPropertyChanged event
    /// </summary>
    public class CommonBase : INotifyPropertyChanged
    {
        
        public event PropertyChangedEventHandler PropertyChanged;
        
    protected void RaisePropertyChanged(string propertyName)
        {
            // Grab a handler
            PropertyChangedEventHandler handler = this.PropertyChanged;
            // Only raise event if handler is connected
            if (handler != null)
            {
                PropertyChangedEventArgs args =
                new PropertyChangedEventArgs(propertyName);
                // Raise the PropertyChanged event.
                handler(this, args);
            }
        }
    }
}


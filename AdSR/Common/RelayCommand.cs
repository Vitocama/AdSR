using System;
using System.Windows.Input;

namespace AdSR.Common
{
    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Action _executeNoArgs;
        private readonly Func<object, bool> _canExecute;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        // Costruttore con parametro
        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        // Costruttore senza parametri
        public RelayCommand(Action execute, Func<bool> canExecute = null)
        {
            _executeNoArgs = execute ?? throw new ArgumentNullException(nameof(execute));
            
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            if (_execute != null)
                _execute(parameter);
            else if (_executeNoArgs != null)
                _executeNoArgs();
        }

        public void RaiseCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace eFitnessApp.Helpers
{
    /// <summary>
    /// This is an implementation of ICommand to take as parameter int or string
    /// </summary>
    public class StringToIntCommand : ICommand
    {
        private readonly Action<int> _execute;

        public StringToIntCommand(Action<int> execute)
        {
            _execute = execute;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            // If the parameter is int
            if(parameter is int intValue1)
            {
                _execute?.Invoke(intValue1);
            }
            // If the parameter is string
            else if (parameter is string stringValue)
            {
                if(int.TryParse(stringValue, out int intValue2))
                    _execute?.Invoke(intValue2);
            }
        }
    }
}

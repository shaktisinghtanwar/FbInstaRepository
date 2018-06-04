using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Fb_InstaWpf.Helper
{
  public  class DelegateCommand : ICommand
    {
      private readonly Predicate<object> _canExecute;
      private readonly Action<object> _execute;
      
      public bool CanExecute(object parameter)
      {
          if (_canExecute == null)
          {
              return true;
          }

          return _canExecute(parameter);
      }
      public DelegateCommand(Action<object> execute)
          : this(execute, null)
      {
      }
      public DelegateCommand(Action<object> execute,
                  Predicate<object> canExecute)
      {
          _execute = execute;
          _canExecute = canExecute;
      }
      public void Execute(object parameter)
      {
          _execute(parameter);
      }

      public event EventHandler CanExecuteChanged;
      public void RaiseCanExecuteChanged()
      {
          if (CanExecuteChanged != null)
          {
              CanExecuteChanged(this, EventArgs.Empty);
          }
      }
    }
}

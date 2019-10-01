using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace UrcieSln.WpfUI.Common
{
	public class BaseCommand : ICommand
	{
		readonly Action<object> execute;
		readonly Predicate<object> canExecute;

		public BaseCommand(Action<object> executeDelegate, Predicate<object> canExecuteDelegate)
		{
			execute = executeDelegate;
			canExecute = canExecuteDelegate;
		}

		event EventHandler ICommand.CanExecuteChanged
		{
			add { CommandManager.RequerySuggested += value; }
			remove { CommandManager.RequerySuggested -= value; }
		}

		bool ICommand.CanExecute(object parameter)
		{
			return canExecute == null ? true : canExecute(parameter);
		}

		void ICommand.Execute(object parameter)
		{
			execute(parameter);
		}
	}
}

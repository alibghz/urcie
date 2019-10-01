using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using UrcieSln.Domain;

namespace UrcieSln.WpfUI.Common
{
	public class BaseViewModel : INotifyPropertyChanged
	{
		public BaseViewModel () { }

		public BaseViewModel(FileStorage storage)
		{
			Storage = storage;
		}

		protected FileStorage Storage { get; set; }

		#region INotifyPropertyChanged

		public event PropertyChangedEventHandler PropertyChanged;
		protected void OnPropertyChanged(string name)
		{
			PropertyChangedEventHandler handler = PropertyChanged;
			if(handler != null)
			{
				handler(this, new PropertyChangedEventArgs(name));
			}
		}

		#endregion INotifyPropertyChanged

		#region CloseRequestEventHandler

		public delegate void RequestCloseEventHandler(object sender, CloseEventHandlerArgs args);
		public event RequestCloseEventHandler CloseRequested;
		protected virtual void OnCloseRequested(CloseEventHandlerArgs args)
		{
			RequestCloseEventHandler handler = CloseRequested;
			if(handler != null)
			{
				handler(this, args);
			}
		}
		
		#endregion CloseRequestEventHandler

		#region CloseCommand

		private ICommand closeCommand;
		public ICommand CloseCommand
		{
			get
			{
				if (closeCommand == null)
				{
					closeCommand = new BaseCommand(i => this.Close(), null);
				}
				return closeCommand;
			}
		}

		protected virtual void Close()
		{
			OnCloseRequested(new CloseEventHandlerArgs(this));
		}

		#endregion
	}
}
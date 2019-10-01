using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using UrcieSln.Domain;

namespace UrcieSln.WpfUI.Common
{
	public abstract class BaseMasterDetailViewModel<T> : BaseViewModel where T : BaseEntityViewModel
	{
		public BaseMasterDetailViewModel(FileStorage storage)
			: base(storage)
		{
		}

		private bool collectionModified = false;
		public bool CollectionModified
		{
			get { return collectionModified; }
			set
			{
				if (collectionModified != value)
				{
					collectionModified = value;
					OnPropertyChanged("CollectionModified");
				}
			}
		}


		private T selectedItem;
		public T SelectedItem
		{
			get { return selectedItem; }
			set
			{
				if (selectedItem != value)
				{
					selectedItem = value;
					OnPropertyChanged("SelectedItem");
				}
			}
		}

		private ObservableCollection<T> items;
		public ObservableCollection<T> Items
		{
			get { return items; }
			set
			{
				items = new ObservableCollection<T>();
				if (value != null)
				{
					foreach (T item in value)
					{
						item.PropertyChanged += Item_PropertyChanged;
						items.Add(item);
					}
					items.CollectionChanged += Items_CollectionChanged;
					OnPropertyChanged("Items");
				}
				else
				{
					items = null;
					OnPropertyChanged("Items");
				}
			}
		}
		private void Item_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			CollectionModified = true;
		}
		private void Items_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (e.OldItems != null)
			{
				foreach (T vm in e.OldItems)
				{
					vm.PropertyChanged -= Item_PropertyChanged;
				}
			}

			if (e.NewItems != null)
			{
				foreach (T vm in e.NewItems)
				{
					vm.PropertyChanged += Item_PropertyChanged;
				}
			}

			CollectionModified = true;
		}

		#region AddCommand

		private ICommand addCommand;
		public ICommand AddCommand
		{
			get
			{
				if (addCommand == null)
				{
					addCommand = new BaseCommand(i => this.Add(), i => this.AddCanExecute());
				}
				return addCommand;
			}
		}
		protected abstract void Add();
		protected virtual bool AddCanExecute()
		{
			return true;
		}

		#endregion

		#region SaveCommand

		private ICommand saveCommand;
		public ICommand SaveCommand
		{
			get
			{
				if(saveCommand == null)
				{
					saveCommand = new BaseCommand(i => this.Save(), i => this.CanSaveExecute());
				}
				return saveCommand;
			}
		}
		protected abstract void Save();
		protected virtual bool CanSaveExecute()
		{
			return CollectionModified;
		}

		#endregion

		#region DeleteCommand

		private ICommand deleteCommand;
		public ICommand DeleteCommand
		{
			get
			{
				if(deleteCommand == null)
				{
					deleteCommand = new BaseCommand(i => this.Delete(), i => this.CanDeleteExecute());
				}
				return deleteCommand;
			}
		}
		protected virtual void Delete()
		{
			MessageBoxResult result = MessageBox.Show(
				"Are you sure you want to delete the selected item and save changes?", "Confirm",
				MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

			if (result == MessageBoxResult.Yes)
			{
				Items.Remove(SelectedItem);
				Save();
			}
		}
		protected virtual bool CanDeleteExecute()
		{
			return SelectedItem != null;
		}
		
		#endregion

		#region CancelCommand

		private ICommand cancelCommand;
		public ICommand CancelCommand
		{
			get
			{
				if(cancelCommand == null)
				{
					cancelCommand = new BaseCommand(i => this.Cancel(), i => this.CanCancelExecute());
				}
				return cancelCommand;
			}
		}
		protected virtual void Cancel()
		{
			MessageBoxResult result = MessageBox.Show(
				"Are you sure you want to discard changes?", "Confirm",
				MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

			if (result == MessageBoxResult.Yes) Restore();
		}
		protected virtual bool CanCancelExecute()
		{
			return CollectionModified;
		}

		#endregion CancelCommand

		#region RefreshCommand

		private ICommand refreshCommand;
		public ICommand RefreshCommand
		{
			get
			{
				if(refreshCommand == null)
				{
					refreshCommand = new BaseCommand(i => this.Refresh(), i => this.CanRefreshExecute());
				}
				return refreshCommand;
			}
		}
		protected abstract void Refresh();
		protected virtual bool CanRefreshExecute()
		{
			return true;
		}

		#endregion

		protected override void OnCloseRequested(CloseEventHandlerArgs args)
		{
			if (CollectionModified)
			{
				MessageBoxResult result = MessageBox.Show(
					"Do you want to save the changes before closing?", "Save changes",
					MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

				switch (result)
				{
					case MessageBoxResult.Cancel:
						return;
					case MessageBoxResult.Yes:
						Save();
						break;
					case MessageBoxResult.No:
						Restore();
						break;
				}
			}
			base.OnCloseRequested(args);
		}	
		protected virtual void Restore()
		{
			foreach (T t in Items) if (t.Original != null) t.Restore();

			Items.ToList().FindAll(i => i.Original == null).ForEach(i => Items.Remove(i));
			SelectedItem = null;
			CollectionModified = false;
		}
	}
}

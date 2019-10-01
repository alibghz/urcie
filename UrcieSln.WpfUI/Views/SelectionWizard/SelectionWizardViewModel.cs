using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using UrcieSln.Domain;
using UrcieSln.Domain.Entities;
using UrcieSln.WpfUI.Common;
using UrcieSln.WpfUI.Extensions;
using UrcieSln.WpfUI.Views.Materials;

namespace UrcieSln.WpfUI.Views
{
	public class SelectionWizardViewModel : BaseViewModel
	{
		private ObservableCollection<AccessoryType> allItems = new ObservableCollection<AccessoryType>();
		public ObservableCollection<AccessoryType> AllItems
		{
			get { return allItems; }
			set
			{
				if(allItems != value)
				{
					allItems = value;
					OnPropertyChanged("AllItems");
				}
			}
		}


		private AccessoryType allItemsSelectedItem;
		public AccessoryType AllItemsSelectedItem
		{
			get { return allItemsSelectedItem; }
			set
			{
				if (allItemsSelectedItem == null || !allItemsSelectedItem.Equals(value))
				{
					allItemsSelectedItem = value;
					OnPropertyChanged("AllItemsSelectedItem");
				}
			}
		}


		private ObservableCollection<AccessoryViewModel> selectedItems = new ObservableCollection<AccessoryViewModel>();
		public ObservableCollection<AccessoryViewModel> SelectedItems
		{
			get { return selectedItems; }
			set
			{
				if(selectedItems != value)
				{
					selectedItems = value;
					OnPropertyChanged("SelectedItems");
				}
			}
		}
		
		
		private AccessoryViewModel selectedItemsSelectedItem;
		public AccessoryViewModel SelectedItemsSelectedItem
		{
			get { return selectedItemsSelectedItem; }
			set
			{
				if(selectedItemsSelectedItem == null || !selectedItemsSelectedItem.Equals(value))
				{
					selectedItemsSelectedItem = value;
					OnPropertyChanged("SelectedItemsSelectedItem");
				}
			}
		}

		
		private ICommand deleteCommand;
		public ICommand DeleteCommand
		{
			get
			{
				if(deleteCommand == null)
				{
					deleteCommand = new BaseCommand(i => this.Delete(), i => this.SelectedItemsSelectedItem != null);
				}
				return deleteCommand;
			}
		}
		private void Delete()
		{
			var item = AllItems.ToList().Find(i => i.Equals(SelectedItemsSelectedItem.ToModel().AccessoryType));
			if (item == null)
			{
				AllItems.Add(SelectedItemsSelectedItem.AccessoryType.ToModel());
			}
			SelectedItems.Remove(SelectedItemsSelectedItem);
			SelectedItemsSelectedItem = null;
		}


		private ICommand addCommand;
		public ICommand AddCommand
		{
			get
			{
				if(addCommand == null)
				{
					addCommand = new BaseCommand(i => this.Add(), i => this.CanAdd());
				}
				return addCommand;
			}
		}

		private bool CanAdd()
		{
			if (this.AllItemsSelectedItem == null) return false;
			if (selectedItems == null || selectedItems.Count == 0) return true;
			var item = SelectedItems.ToList().Find(i => i.AccessoryType.ToModel().Equals(AllItemsSelectedItem));
			if (item != null) return false;
			return true;
		}
		
		private void Add()
		{	
			AccessoryViewModel accessory = new AccessoryViewModel();
			accessory.AccessoryType = new AccessoryTypeViewModel(AllItemsSelectedItem);
			accessory.Code = "";
			accessory.Quantity = 1;
			SelectedItems.Add(accessory);
			AllItems.Remove(AllItemsSelectedItem);
			AllItemsSelectedItem = null;
			SelectedItemsSelectedItem = accessory;
		}
	}
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using UrcieSln.WpfUI.Common;

namespace UrcieSln.WpfUI.ViewModels
{
	public class TabControlViewModel : BaseViewModel
	{
		private TabControl _tabControl;
		private TabItem _selectedTabItem;
		private ObservableCollection<TabItem> _openTabItems;

		public TabControlViewModel()
		{
			OpenTabItems = new ObservableCollection<TabItem>();
		}

		public TabControl TabControl {
			get { return _tabControl; }
			set
			{
				if(_tabControl != value)
				{
					_tabControl = value;

					var openTabsBinding = new Binding("OpenTabItems");
					openTabsBinding.Source = this;
					_tabControl.SetBinding(TabControl.ItemsSourceProperty, openTabsBinding);

					var selectedTabBinding = new Binding("SelectedTabItem");
					selectedTabBinding.Source = this;
					_tabControl.SetBinding(TabControl.SelectedItemProperty, selectedTabBinding);
				}
			}
		}

		public ObservableCollection<TabItem> OpenTabItems
		{
			get { return _openTabItems; }
			set
			{
				if(_openTabItems != value)
				{
					_openTabItems = value;
					OnPropertyChanged("OpenTabItems");
				}
			}
		}

		public TabItem SelectedTabItem
		{
			get { return _selectedTabItem; }
			set
			{
				if(_selectedTabItem != value)
				{
					_selectedTabItem = value;
					OnPropertyChanged("SelectedTabItem");
				}
			}
		}

		public void OpenTab(Control control, string header, object tag, object dataContext)
		{
			if (tag == null) throw new ArgumentNullException("tag");
			
			TabItem tabItem = GetTabFor(tag);

			if(tabItem == null)
			{
				tabItem = new TabItem();
				tabItem.Header = header;
				tabItem.Tag = tag;
				tabItem.DataContext = dataContext;
				tabItem.Content = control;
				tabItem.IsSelected = true;
				OpenTabItems.Add(tabItem);

				if(dataContext is BaseViewModel)
				{
					((BaseViewModel)dataContext).CloseRequested += TabControlViewModel_CloseRequested;
				}
			}
			SelectedTabItem = tabItem;
		}

		void TabControlViewModel_CloseRequested(object sender, CloseEventHandlerArgs args)
		{
			BaseViewModel viewModel = sender as BaseViewModel;
			OpenTabItems.Remove(GetTabFor(viewModel));
		}

		private TabItem GetTabFor(object tag)
		{
			if (tag == null) return null;

			foreach(TabItem tabItem in OpenTabItems)
			{
				if(tabItem.Tag == tag) return tabItem;
			}
			return null;
		}

		internal void SelectTab(BaseViewModel viewModel)
		{
			if(IsOpen(viewModel))
			{
				SelectedTabItem = OpenTabItems.First(i => i.DataContext == viewModel);
			}
		}

		internal bool IsOpen(BaseViewModel viewModel)
		{
			foreach(TabItem tabItem in OpenTabItems)
			{
				if(tabItem.DataContext == viewModel
					|| tabItem.Tag == viewModel)
				{
					return true;
				}
			}
			return false;
		}
	}
}
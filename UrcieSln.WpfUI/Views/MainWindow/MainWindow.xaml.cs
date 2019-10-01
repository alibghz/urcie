using Microsoft.Practices.Unity;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using UrcieSln.WpfUI.Common;
using UrcieSln.WpfUI.ViewModels;
using UrcieSln.WpfUI.Views;

namespace UrcieSln.WpfUI
{
	public partial class MainWindow : Window
	{
		public MainWindow(MainWindowViewModel viewModel)
		{
			ViewModel = viewModel;
			InitializeComponent();
			TabControl.DataContext = ViewModel.TabControlViewModel;
			ViewModel.TabControlViewModel.TabControl = TabControl;
			ViewModel.PropertyChanged += ViewModel_PropertyChanged;
            
		}

		private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName != "SelectedProject") return;
			if (ViewModel.SelectedProject == null)
				UnitsPanel.Visibility = System.Windows.Visibility.Collapsed;
			else
				UnitsPanel.Visibility = System.Windows.Visibility.Visible;
		}

		public MainWindowViewModel ViewModel
		{
			get { return (MainWindowViewModel)DataContext; }
			set { DataContext = value; }
		}

		private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			Close();
		}

		private void contactBtn_Click(object sender, RoutedEventArgs e)
		{
			System.Diagnostics.Process.Start("https://linkedin.com/in/baratali");
		}

		private void helpBtn_Click(object sender, RoutedEventArgs e)
		{
			System.Diagnostics.Process.Start("https://github.com/alibghz");
		}

		private void UnitsBoxView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			if(ViewModel.EditUnitCommand.CanExecute(null))
			{
				ViewModel.EditUnitCommand.Execute(null);
			}
		}

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (App.Args != null && App.Args.Length > 0)
            {
                ViewModel.OpenProject(App.Args[0]);
            }
        }
	}
}
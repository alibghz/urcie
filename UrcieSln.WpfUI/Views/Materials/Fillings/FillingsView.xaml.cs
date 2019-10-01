using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using UrcieSln.WpfUI.Extensions;

namespace UrcieSln.WpfUI.Views.Materials
{
	public partial class FillingsView : UserControl
	{
		public FillingsView(FillingsViewViewModel viewModel)
		{
			InitializeComponent();
			ViewModel = viewModel;
		}

		public FillingsViewViewModel ViewModel
		{
			get { return (FillingsViewViewModel)DataContext; }
			set { DataContext = value; }
		}

		private void SaveButton_Click(object sender, RoutedEventArgs e)
		{
			if (!this.IsValid()) return;
			ViewModel.SaveCommand.Execute(null);
		}

		#region UX event handlers

		private void SelectAll(object sender, KeyboardFocusChangedEventArgs e)
		{
			TextBox tb = (sender as TextBox);
			if (tb != null)
			{
				tb.SelectAll();
			}
		}

		private void SelectivelyIgnoreMouseButton(object sender, MouseButtonEventArgs e)
		{
			TextBox tb = (sender as TextBox);
			if (tb != null)
			{
				if (!tb.IsKeyboardFocusWithin)
				{
					e.Handled = true;
					tb.Focus();
				}
			}
		}

		private void SelectAll(object sender, MouseButtonEventArgs e)
		{
			TextBox tb = (sender as TextBox);
			if (tb != null)
			{
				tb.SelectAll();
			}
		}

		#endregion
	}
}

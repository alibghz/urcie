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
using System.Windows.Shapes;
using UrcieSln.Domain;

namespace UrcieSln.WpfUI.Views
{
	public partial class SelectionWizardView : Window
	{
		public SelectionWizardView(SelectionWizardViewModel viewModel)
		{
			InitializeComponent();
			ViewModel = viewModel;
		}

		public SelectionWizardViewModel ViewModel
		{
			get { return (SelectionWizardViewModel)DataContext; }
			set { DataContext = value; }
		}

		private void OkButton_Click(object sender, RoutedEventArgs e)
		{
			DialogResult = true;
		}

		private void CancelButton_Click(object sender, RoutedEventArgs e)
		{
			DialogResult = false;
		}

		private void TextBox_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
			{
				float val = 0;
				if (float.TryParse(QuantityTextBox.Text, out val))
				{
					ViewModel.SelectedItemsSelectedItem.Quantity = val;
				}
				else
				{
					QuantityTextBox.Text = ViewModel.SelectedItemsSelectedItem.Quantity.ToString();
				}
			}
		}
	}
}

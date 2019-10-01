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
using UrcieSln.Domain.Entities;

namespace UrcieSln.WpfUI.Views
{
	public partial class EditUnitView : Window
	{
		public EditUnitView(UnitViewModel viewModel)
		{
			ViewModel = viewModel;
			InitializeComponent();
		}
		public UnitViewModel ViewModel
		{
			get { return (UnitViewModel)DataContext; }
			set { DataContext = value; }
		}

		#region OkCancel event handlers

		private void OkButton_Click(object sender, RoutedEventArgs e)
		{
			if (!ValidateCode()) return;
			if (!ValidateQuantity()) return;
			if (!ValidateWidth()) return;
			if (!ValidateHeight()) return;

			if (!IsValid(this)) return;
			DialogResult = true;
		}
		private void CancelButton_Click(object sender, RoutedEventArgs e)
		{
			this.DialogResult = false;
		}

		#endregion

		#region Validation

		bool IsValid(DependencyObject node)
		{
			if (node != null)
			{
				bool isValid = !Validation.GetHasError(node);
				if (!isValid)
				{
					if (node is IInputElement) Keyboard.Focus((IInputElement)node);
					return false;
				}
			}

			foreach (object subnode in LogicalTreeHelper.GetChildren(node))
			{
				if (subnode is DependencyObject)
				{
					if (IsValid((DependencyObject)subnode) == false) return false;
				}
			}

			return true;
		}
		bool ValidateCode()
		{
			if(string.IsNullOrEmpty(ViewModel.Code) || string.IsNullOrWhiteSpace(ViewModel.Code))
			{
				DialogError("Code field is required.", "Error");
				return false;
			}
			return true;
		}
		bool ValidateQuantity()
		{
			if (ViewModel.Quantity <= 0)
			{
				DialogError("Quantity value is invalid.", "Error");
				return false;
			}
			return true;
		}
		bool ValidateWidth()
		{
			if(ViewModel.Width <= 0)
			{
				DialogError("Width value is invalid.", "Error");
				return false;
			}
			return true;
		}
		bool ValidateHeight()
		{
			if(ViewModel.Height <= 0)
			{
				DialogError("Height value is invalid.", "Error");
				return false;
			}
			return true;
		}

		#endregion

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

		private void DialogError(string message, string title)
		{
			MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error);
		}
	}
}

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

namespace UrcieSln.WpfUI.Views.Materials
{
	public partial class AccessoriesView : UserControl
	{
		public AccessoriesView(AccessoriesViewViewModel viewModel)
		{
			InitializeComponent();
			ViewModel = viewModel;
		}

		public AccessoriesViewViewModel ViewModel
		{
			get { return (AccessoriesViewViewModel)DataContext; }
			set { DataContext = value; }
		}
	}
}

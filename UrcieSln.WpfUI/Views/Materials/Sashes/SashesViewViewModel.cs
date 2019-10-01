using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using UrcieSln.Domain;
using UrcieSln.Domain.Entities;
using UrcieSln.WpfUI.Common;
using UrcieSln.WpfUI.Extensions;

namespace UrcieSln.WpfUI.Views.Materials
{
	public class SashesViewViewModel : BaseMasterDetailViewModel<SashTypeViewModel>
	{
		public SashesViewViewModel(FileStorage storage) : base(storage)
		{
			Refresh();
		}

		private List<ProfileTypeViewModel> profileTypes;
		public List<ProfileTypeViewModel> ProfileTypes
		{
			get { return profileTypes; }
			set
			{
				if (profileTypes != value)
				{
					profileTypes = value;
					OnPropertyChanged("ProfileTypes");
				}
			}
		}

		protected override void Add()
		{
			SashTypeViewModel svm = new SashTypeViewModel();
			svm.Name = "Sash Name";
			Items.Add(svm);
			SelectedItem = svm;
		}
		protected override void Save()
		{
			if(CollectionModified)
			{
				foreach (SashTypeViewModel svm in Items)
					if (svm.ProfileType == null)
					{
						MessageBox.Show("Please select a profile type.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
						SelectedItem = svm;
						return;
					}
				Storage.SaveAll<SashType>(Items.ToList().ToModels());
				Refresh();
			}
		}
		protected override void Refresh()
		{
			ProfileTypes = new List<ProfileTypeViewModel>(Storage.GetAll<ProfileType>().ToViewModels());
			Items = new ObservableCollection<SashTypeViewModel>(Storage.GetAll<SashType>().ToViewModels());
			SelectedItem = null;
			CollectionModified = false;
		}
		protected override bool AddCanExecute()
		{
			return ProfileTypes.Count > 0;
		}
	}
}

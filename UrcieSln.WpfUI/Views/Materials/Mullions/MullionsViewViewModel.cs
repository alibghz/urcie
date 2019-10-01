using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using UrcieSln.Domain;
using UrcieSln.Domain.Entities;
using UrcieSln.WpfUI.Common;
using UrcieSln.WpfUI.Extensions;

namespace UrcieSln.WpfUI.Views.Materials.Mullions
{
	public class MullionsViewViewModel : BaseMasterDetailViewModel<MullionTypeViewModel>
	{
		public MullionsViewViewModel(FileStorage storage) : base(storage)
		{
			Refresh();
		}

		private List<ProfileTypeViewModel> profileTypes;
		public List<ProfileTypeViewModel> ProfileTypes
		{
			get { return profileTypes; }
			set
			{
				if(profileTypes != value)
				{
					profileTypes = value;
					OnPropertyChanged("ProfileTypes");
				}
			}
		}

		protected override void Add()
		{
			MullionTypeViewModel mvm = new MullionTypeViewModel();
			mvm.Name = "Mullion name";

			Items.Add(mvm);
			SelectedItem = mvm;
		}
		protected override void Save()
		{
			if(CollectionModified)
			{
				foreach(MullionTypeViewModel mvm in Items)
					if(mvm.ProfileType == null)
					{
						MessageBox.Show("Please select a profile type.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
						SelectedItem = mvm;
						return;
					}
				Storage.SaveAll<MullionType>(Items.ToList().ToModels());
				Refresh();
			}
		}
		protected override void Refresh()
		{
			ProfileTypes = Storage.GetAll<ProfileType>().ToViewModels();
			Items = new ObservableCollection<MullionTypeViewModel>(Storage.GetAll<MullionType>().ToViewModels());
			SelectedItem = null;
			CollectionModified = false;
		}
		protected override bool AddCanExecute()
		{
			return ProfileTypes.Count > 0;
		}
	}
}

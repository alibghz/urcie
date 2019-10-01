using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
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
	public class ProfilesViewViewModel : BaseMasterDetailViewModel<ProfileTypeViewModel>
	{
		public ProfilesViewViewModel(FileStorage storage) : base(storage)
		{
			Refresh();
		}

		protected override void Add()
		{
			ProfileTypeViewModel pvm = new ProfileTypeViewModel();
			pvm.Name = "Profile Name";
			Items.Add(pvm);
			SelectedItem = pvm;
		}
		protected override void Save()
		{
			if (CollectionModified)
			{
				Storage.SaveAll<ProfileType>(Items.ToList().ToModels());
				Refresh();
			}
		}
		protected override void Refresh()
		{
			Items = new ObservableCollection<ProfileTypeViewModel>(Storage.GetAll<ProfileType>().ToViewModels());
			SelectedItem = null;
			CollectionModified = false;
		}
	}
}
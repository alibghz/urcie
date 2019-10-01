using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrcieSln.Domain;
using UrcieSln.Domain.Entities;
using UrcieSln.WpfUI.Common;
using UrcieSln.WpfUI.Extensions;

namespace UrcieSln.WpfUI.Views.Materials
{
	public class FillingsViewViewModel : BaseMasterDetailViewModel<FillingTypeViewModel>
	{
		public FillingsViewViewModel(FileStorage storage) : base(storage)
		{
			Refresh();
		}
		protected override void Add()
		{
			FillingTypeViewModel mvm = new FillingTypeViewModel();
			mvm.Name = "Filling name";
			Items.Add(mvm);
			SelectedItem = mvm;
		}
		protected override void Save()
		{
			if(CollectionModified)
			{
				Storage.SaveAll<FillingType>(Items.ToList().ToModels());
				Refresh();
			}
		}
		protected override void Refresh()
		{
			Items = new ObservableCollection<FillingTypeViewModel>(Storage.GetAll<FillingType>().ToViewModels());
			SelectedItem = null;
			CollectionModified = false;
		}
	}
}

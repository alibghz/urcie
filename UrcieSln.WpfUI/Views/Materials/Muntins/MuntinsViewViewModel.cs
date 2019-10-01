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
	public class MuntinsViewViewModel : BaseMasterDetailViewModel<MuntinTypeViewModel>
	{
		public MuntinsViewViewModel(FileStorage storage) : base(storage)
		{
			Refresh();
		}
		protected override void Add()
		{
			MuntinTypeViewModel mvm = new MuntinTypeViewModel();
			Items.Add(mvm);
			SelectedItem = mvm;
		}
		protected override void Save()
		{
			if(CollectionModified)
			{
				Storage.SaveAll<MuntinType>(Items.ToList().ToModels());
				Refresh();
			}
		}
		protected override void Refresh()
		{
			Items = new ObservableCollection<MuntinTypeViewModel>(Storage.GetAll<MuntinType>().ToViewModels());
			SelectedItem = null;
			CollectionModified = false;
		}
	}
}

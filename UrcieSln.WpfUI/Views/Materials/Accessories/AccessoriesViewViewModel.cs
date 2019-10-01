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
	public class AccessoriesViewViewModel : BaseMasterDetailViewModel<AccessoryTypeViewModel>
	{
		public AccessoriesViewViewModel(FileStorage storage) : base(storage)
		{
			Refresh();
		}

		protected override void Add()
		{
			AccessoryTypeViewModel avm = new AccessoryTypeViewModel();
			Items.Add(avm);
			SelectedItem = avm;
		}
		protected override void Save()
		{
			if(CollectionModified)
			{
				Storage.SaveAll<AccessoryType>(Items.ToList().ToModels());
				Refresh();
			}
		}
		protected override void Refresh()
		{
			Items = new ObservableCollection<AccessoryTypeViewModel>(Storage.GetAll<AccessoryType>().ToViewModels());
			SelectedItem = null;
			CollectionModified = false;
		}
		protected override void Restore()
		{
			foreach (AccessoryTypeViewModel avm in Items)
				if (avm.Original != null) avm.Restore();

			Items.ToList().RemoveAll(i => i.Original == null);

			SelectedItem = null;
			CollectionModified = false;
		}
	}
}

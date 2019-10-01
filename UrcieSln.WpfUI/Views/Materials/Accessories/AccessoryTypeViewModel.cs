using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrcieSln.Domain.Entities;
using UrcieSln.WpfUI.Common;

namespace UrcieSln.WpfUI.Views.Materials
{
	public class AccessoryTypeViewModel : BaseEntityViewModel
	{
		private string name;
		private string category;
		private string description;
		private Price price;

		public AccessoryTypeViewModel()
		{
			Id = Guid.NewGuid();
		}
		public AccessoryTypeViewModel(AccessoryType accessoryType)
		{
			Original = accessoryType;
			Restore();
		}
		public override void Restore()
		{
			AccessoryType original = Original as AccessoryType;
			if (original == null) 
				throw new InvalidOperationException(
				"View model does not have an original value.");

			Id = original.Id;
			Name = original.Name;
			Category = original.Category;
			Description = original.Description;
			Price = original.Price;
		}
		public string Name
		{
			get { return name; }
			set
			{
				if(name != value)
				{
					name = value;
					OnPropertyChanged("Name");
				}
			}
		}
		public string Category
		{
			get { return category; }
			set
			{
				if(category != value)
				{
					category = value;
					OnPropertyChanged("Category");
				}
			}
		}
		public string Description
		{
			get { return description; }
			set
			{
				if(description != value)
				{
					description = value;
					OnPropertyChanged("Description");
				}
			}
		}
		public Price Price
		{
			get { return price; }
			set
			{
				if(price != value)
				{
					price = value;
					OnPropertyChanged("Price");
				}
			}
		}
		
		public override string ToString()
		{
			return Name;
		}
		public override bool Equals(object obj)
		{
			AccessoryTypeViewModel ac = obj as AccessoryTypeViewModel;
			if (ac == null) return false;

			if (ac.Id != Id) return false;
			if (ac.Name != Name) return false;
			if (ac.Category != Category) return false;
			if (ac.Description != Description) return false;
			if (ac.Price != null && !Price.Equals(ac.Price)) return false;

			return true;
		}
	}
}

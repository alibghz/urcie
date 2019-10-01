using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrcieSln.Domain.Entities;
using UrcieSln.WpfUI.Common;

namespace UrcieSln.WpfUI.Views.Materials
{
	public class MuntinTypeViewModel : BaseEntityViewModel
	{
		private string name;
		private float width;
		private float height;
		private float weight;
		private Price price;
		private float tolerance;
		private string description;

		public MuntinTypeViewModel ()
		{
			Id = Guid.NewGuid();
		}
		public MuntinTypeViewModel(MuntinType muntinType)
		{
			Original = muntinType;
			Restore();
		}
		public override void Restore()
		{
			MuntinType original = Original as MuntinType;
			if (original == null)
				throw new InvalidOperationException(
				"View model does not have an original value.");
			Id = original.Id;
			Name = original.Name;
			Width = original.Width;
			Weight = original.Weight;
			Height = original.Height;
			Price = original.Price;
			Tolerance = original.Tolerance;
			Description = original.Description;
		}
	
		public virtual string Name
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
		public float Width
		{
			get { return width; }
			set
			{
				if(width != value)
				{
					width = value;
					OnPropertyChanged("Width");
				}
			}
		}
		public float Height
		{
			get { return height; }
			set
			{
				if(height != value)
				{
					height = value;
					OnPropertyChanged("Height");
				}
			}
		}
		public float Weight
		{
			get { return weight; }
			set
			{
				if(weight != value)
				{
					weight = value;
					OnPropertyChanged("Weight");
				}
			}
		}
		public virtual Price Price
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
		public float Tolerance
		{
			get { return tolerance; }
			set
			{
				if(tolerance != value)
				{
					tolerance = value;
					OnPropertyChanged("Tolerance");
				}
			}
		}
		public virtual string Description
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
		public override string ToString()
		{
			return Name;
		}

		public override bool Equals(object obj)
		{
			MuntinTypeViewModel m = obj as MuntinTypeViewModel;
			if (m == null) return false;
			if (m.Id != Id) return false;
			if (m.Name != Name) return false;
			if (m.Width != Width) return false;
			if (m.Height != Height) return false;
			if (m.Price != null && !Price.Equals(m.Price)) return false;
			if (m.Tolerance != Tolerance) return false;
			if (m.Description != Description) return false;
			return true;
		}
	}
}

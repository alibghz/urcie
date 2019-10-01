using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrcieSln.Domain.Entities;
using UrcieSln.WpfUI.Common;

namespace UrcieSln.WpfUI.Views.Materials
{
	public class FillingTypeViewModel : BaseEntityViewModel
	{
		private string name;
		private bool glass;
		private string description;
		private float width;
		private float height;
		private float weight;
		private float tolerance;
		private Price price;

		public FillingTypeViewModel()
		{
			Id = Guid.NewGuid();
		}
		public FillingTypeViewModel(FillingType fillingType)
		{
			Original = fillingType;
			Restore();
		}
		public override void Restore()
		{
			FillingType original = Original as FillingType;

			if (original == null)
				throw new InvalidOperationException(
				"View model does not have an original value.");


			Id = original.Id;
			Name = original.Name;
			Glass = original.Glass;
			Width = original.Width;
			Height = original.Height;
			Weight = original.Weight;
			Price = original.Price;
			Tolerance = original.Tolerance;
			Description = original.Description;
		}
		
		public virtual string Name
		{
			get { return name; }
			set
			{
				if (name != value)
				{
					name = value;
					OnPropertyChanged("Name");
				}
			}
		}
		public bool Glass
		{
			get { return glass; }
			set
			{
				if(glass != value)
				{
					glass = value;
					OnPropertyChanged("Glass");
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
				if (weight != value)
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
		public float Tolerance {
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
			FillingTypeViewModel f = obj as FillingTypeViewModel;

			if (f == null) return false;

			if (f.Id != Id) return false;
			if (f.Name != Name) return false;
			if (f.Glass != Glass) return false;
			if (f.Width != Width) return false;
			if (f.Height != Height) return false;
			if (f.Price != null && !Price.Equals(f.Price)) return false;
			if (f.Tolerance != Tolerance) return false;
			if (f.Description != Description) return false;

			return true;
		}
	}
}

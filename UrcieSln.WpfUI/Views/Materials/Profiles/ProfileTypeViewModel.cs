using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrcieSln.Domain.Entities;
using UrcieSln.WpfUI.Common;
using UrcieSln.WpfUI.Extensions;

namespace UrcieSln.WpfUI.Views.Materials
{
	public class ProfileTypeViewModel : BaseEntityViewModel
	{
		private string name;
		private float thickness;
		private ProfileShape profileShape;
		private float length;
		private float weight;
		private Price price;
		private float tolerance;
		private string description;

		public ProfileTypeViewModel()
		{
			Id = Guid.NewGuid();
		}
		public ProfileTypeViewModel(ProfileType profileType)
		{
			Original = profileType;
			Restore();
		}
		public override void Restore()
		{
			ProfileType original = Original as ProfileType;
			if (original == null) 
				throw new InvalidOperationException(
				"View model does not have an original value.");

			Id = original.Id;
			Name = original.Name;
			Thickness = original.Thickness;
			Shape = original.Shape;
			Length = original.Length;
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
		public float Thickness { 
			get { return thickness; }
			set
			{
				if(thickness != value)
				{
					thickness = value;
					OnPropertyChanged("Thickness");
				}
			}
		}
		public virtual ProfileShape Shape
		{
			get { return profileShape; }
			set
			{
				if (profileShape != value)
				{
					profileShape = value;
					OnPropertyChanged("Shape");
				}
			}
		}
		public float Length
		{
			get { return length; }
			set
			{
				if (length != value)
				{
					length = value;
					OnPropertyChanged("Length");
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
				if (price != value)
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
				if (tolerance != value)
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
				if (description != value)
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
			ProfileTypeViewModel pvm = obj as ProfileTypeViewModel;
			if (pvm == null) return false;

			if (pvm.Id != Id) return false;

			if (pvm.Name != Name) return false;
			if (pvm.Thickness != Thickness) return false;
			if (pvm.Shape != Shape) return false;
			if (pvm.Length != Length) return false;
			if (pvm.Weight != Weight) return false;
			if (pvm.Price != null && !Price.Equals(pvm.Price)) return false;
			if (pvm.Tolerance != Tolerance) return false;
			if (pvm.Description != Description) return false;

			return true;
		}
	}
}

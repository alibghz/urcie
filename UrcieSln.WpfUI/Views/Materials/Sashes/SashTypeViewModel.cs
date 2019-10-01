using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrcieSln.Domain.Entities;
using UrcieSln.WpfUI.Common;
using UrcieSln.WpfUI.Extensions;

namespace UrcieSln.WpfUI.Views.Materials
{
	public class SashTypeViewModel : BaseEntityViewModel
	{
		private string name;
		private ProfileTypeViewModel profileType;
		private float tolerance;
		private string description;

		public SashTypeViewModel()
		{
			Id = Guid.NewGuid();
		}
		public SashTypeViewModel(SashType sashType)
		{
			Original = sashType;
			Restore();
		}
		public override void Restore()
		{
			SashType original = Original as SashType;
			if (original == null)
				throw new InvalidOperationException(
				"View model does not have an original value.");
			Id = original.Id;
			Name = original.Name;
			ProfileType = new ProfileTypeViewModel(original.ProfileType);
			Tolerance = original.Tolerance;
			Description = original.Description;
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
		public ProfileTypeViewModel ProfileType
		{
			get { return profileType; }
			set
			{
				if(profileType == null || !profileType.Equals(value))
				{
					profileType = value;
					OnPropertyChanged("ProfileType");
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

		public override string ToString()
		{
			return Name;
		}
		public override bool Equals(object obj)
		{
			SashTypeViewModel s = obj as SashTypeViewModel;
			if (s == null) return false;

			if (s.Id != Id) return false;
			if (s.Name != Name) return false;
			if (s.ProfileType != null && !s.ProfileType.Equals(ProfileType)) return false;
			if (s.Tolerance != Tolerance) return false;
			if (s.Description != Description) return false;

			return true;
		}
	}
}

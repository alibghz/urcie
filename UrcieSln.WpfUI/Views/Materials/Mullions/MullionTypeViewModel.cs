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
	public class MullionTypeViewModel : BaseEntityViewModel
	{
		private string name;
		private ProfileTypeViewModel profileType;

		public MullionTypeViewModel() {
			Id = Guid.NewGuid();
		}
		public MullionTypeViewModel(MullionType mullionType)
		{
			Original = mullionType;
			Restore();
		}
		public override void Restore()
		{
			MullionType original = Original as MullionType;
			if (original == null)
				throw new InvalidOperationException(
				"View model does not have an original value.");

			Id = original.Id;
			Name = original.Name;
			ProfileType = new ProfileTypeViewModel(original.ProfileType);
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
		public override string ToString()
		{
			return Name;
		}
		public override bool Equals(object obj)
		{
			MullionTypeViewModel mvm = obj as MullionTypeViewModel;
			if (mvm == null) return false;

			if (mvm.Id != Id) return false;
			if (mvm.Name != Name) return false;
			if (mvm.ProfileType != null && !mvm.ProfileType.Equals(ProfileType)) return false;
			
			return true;
		}
	}
}

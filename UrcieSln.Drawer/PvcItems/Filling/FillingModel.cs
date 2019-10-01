using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using UrcieSln.Domain;
using UrcieSln.Domain.Entities;

namespace UrcieSln.Drawer.PvcItems
{
	[Serializable()]
	public class FillingModel : PVCModel, IFilling, ISerializable
	{
		[NonSerialized()]
		public const int CODE_PROPERTY_CODE = 1;
		[NonSerialized()]
		public const int FILLING_TYPE_PROPERTY_CODE = 2;
		[NonSerialized()]
		public const int PARENT_PROPERTY_CODE = 3;
		[NonSerialized()]

		private string code;
		private FillingType fillingType;
		private ProfileType profileType;
		private Surface parent;

		public FillingModel() { }
		public FillingModel(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			code = (string)info.GetValue("Code", typeof(string));
			fillingType = (FillingType)info.GetValue("FillingType", typeof(FillingType));
			profileType = (ProfileType)info.GetValue("ProfileType", typeof(ProfileType));
			parent = (Surface)info.GetValue("SurfaceParent", typeof(Surface));
		}
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue("Code", Code);
			info.AddValue("FillingType", FillingType);
			info.AddValue("ProfileType", ProfileType);
			info.AddValue("SurfaceParent", SurfaceParent);
		}
		public string Code
		{
			get { return code; }
			set
			{
				if (value != code)
				{
					code = value;
					OnPropertyChanged(new PVCModelPropertyChangedEventArgs(CODE_PROPERTY_CODE));
				}
			}
		}
		public FillingType FillingType
		{
			get { return fillingType; }
			set
			{
				if (value != fillingType)
				{
					fillingType = value;
					OnPropertyChanged(new PVCModelPropertyChangedEventArgs(FILLING_TYPE_PROPERTY_CODE));
				}
			}
		}
		public ProfileType ProfileType
		{
			get { return profileType; }
			set
			{
				if (profileType != value)
				{
					profileType = value;
				}
			}
		}
		public Surface SurfaceParent
		{
			get { return parent; }
			set
			{
				if (parent == null)
				{
					parent = value;
					OnPropertyChanged(new PVCModelPropertyChangedEventArgs(PARENT_PROPERTY_CODE));
				}
			}
		}
		public ISurface Parent
		{
			get
			{
				if (parent != null) return parent.Model;
				return null;
			}
		}
	}
}

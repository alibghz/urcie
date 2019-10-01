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
	public class PVCFrameModel : PVCModel, IFrame, ISerializable
	{
		[NonSerialized()]
		public const int WIDTH_PROPERTY_CODE = 1;
		[NonSerialized()]
		public const int HEIGHT_PROPERTY_CODE = 2;
		[NonSerialized()]
		public const int PROFILE_PROPERTY_CODE = 3;
		[NonSerialized()]
		public const int CODE_PROPERTY_CODE = 4;
		[NonSerialized()]
		public const int COLOR_PROPERTY_CODE = 5;
		[NonSerialized()]
		public const int BORDER_COLOR_PROPERTY_CODE = 6;

		private string code = "";
		private float width = 0;
		private float height = 0;
		private ProfileType profileType;
		private ProfileType uProfileType;
		private Surface child;

		public PVCFrameModel() { }
		public PVCFrameModel(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			code = (string)info.GetValue("Code", typeof(string));
			width = (float)info.GetValue("Width", typeof(float));
			height = (float)info.GetValue("Height", typeof(float));
			profileType = (ProfileType)info.GetValue("ProfileType", typeof(ProfileType));
			uProfileType = (ProfileType)info.GetValue("UProfileType", typeof(ProfileType));
			child = (Surface)info.GetValue("SurfaceChild", typeof(Surface));
		}
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue("Code", Code);
			info.AddValue("Width", Width);
			info.AddValue("Height", Height);
			info.AddValue("ProfileType", ProfileType);
			info.AddValue("UProfileType", UProfileType);
			info.AddValue("SurfaceChild", SurfaceChild);
		}
		public string Code
		{
			get { return code; }
			set
			{
				if (code != value)
				{
					code = value;
					OnPropertyChanged(new PVCModelPropertyChangedEventArgs(CODE_PROPERTY_CODE));
				}
			}
		}
		public ProfileType ProfileType
		{
			get { return profileType; }

			set
			{
				if (value == null) return;
				if (profileType == null || profileType.Id != value.Id)
				{
					profileType = value;
					OnPropertyChanged(new PVCModelPropertyChangedEventArgs(PROFILE_PROPERTY_CODE));
				}
			}
		}
		public ProfileType UProfileType
		{
			get { return uProfileType; }
			set
			{
				if (value == null)
				{
					uProfileType = null;
				}
				else if (uProfileType == null || uProfileType.Id != value.Id)
				{
					uProfileType = value;
				}
			}
		}
		public float Width
		{
			get
			{
				if (width == 0 && ProfileType != null)
					return ProfileType.Thickness * 2;

				return width;
			}
			set
			{
				if (width != value)
				{
					width = value;
					OnPropertyChanged(new PVCModelPropertyChangedEventArgs(WIDTH_PROPERTY_CODE));
				}
			}
		}
		public float Height
		{
			get
			{
				if (height == 0 && ProfileType != null)
					return ProfileType.Thickness * 2;

				return height;
			}
			set
			{
				if (value != height)
				{
					height = value;
					PVCModelPropertyChangedEventArgs args = new PVCModelPropertyChangedEventArgs(HEIGHT_PROPERTY_CODE);
					OnPropertyChanged(args);
				}
			}
		}
		public Surface SurfaceChild
		{
			get { return child; }
			set
			{
				if (child != value)
				{
					child = value;
				}
			}
		}
		public ISurface Child
		{
			get { return SurfaceChild.Model; }
		}
		public IList<Accessory> Accessories { get; set; }
		public IList<IMuntin> Muntins { get; set; }
		public bool IsCornered { get; set; }
	}
}

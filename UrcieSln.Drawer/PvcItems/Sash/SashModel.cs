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
	public class SashModel : PVCModel, ISash, ISerializable
	{
		[NonSerialized()]
		public const int CODE_PROPERTY_CODE = 1;
		[NonSerialized()]
		public const int SASH_TYPE_PROPERTY_CODE = 2;
		[NonSerialized()]
		public const int PARENT_PROPERTY_CODE = 3;
		[NonSerialized()]
		public const int CHILD_PROPERTY_CODE = 4;

		private string code;
		private SashType sashType;
		private Surface parent;
		private Surface child;

		public SashModel() { }

		public SashModel(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			code = (string)info.GetValue("Code", typeof(string));
			sashType = (SashType)info.GetValue("SashType", typeof(SashType));
			parent = (Surface)info.GetValue("SurfaceParent", typeof(Surface));
			child = (Surface)info.GetValue("SurfaceChild", typeof(Surface));
		}

		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue("Code", Code);
			info.AddValue("SashType", SashType);
			info.AddValue("SurfaceParent", SurfaceParent);
			info.AddValue("SurfaceChild", SurfaceChild);
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

		public SashType SashType
		{
			get { return sashType; }
			set
			{
				if (sashType != value)
				{
					sashType = value;
					OnPropertyChanged(new PVCModelPropertyChangedEventArgs(SASH_TYPE_PROPERTY_CODE));
				}
			}
		}

		public Surface SurfaceParent
		{
			get { return parent; }
			set
			{
				if (parent != value)
				{
					parent = value;
					OnPropertyChanged(new PVCModelPropertyChangedEventArgs(PARENT_PROPERTY_CODE));
				}
			}
		}

		public ISurface Parent
		{
			get { return SurfaceParent.Model; }
		}

		public Surface SurfaceChild
		{
			get { return child; }
			set
			{
				if (child != value)
				{
					child = value;
					OnPropertyChanged(new PVCModelPropertyChangedEventArgs(CHILD_PROPERTY_CODE));
				}
			}
		}

		public ISurface Child
		{
			get { return SurfaceChild.Model; }
		}

		public bool IsCornered { get; set; }
	}
}

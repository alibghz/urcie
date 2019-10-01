using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using UMD.HCIL.Piccolo;
using UrcieSln.Domain;
using UrcieSln.Domain.Entities;

namespace UrcieSln.Drawer.PvcItems
{
	[Serializable()]
	public class SurfaceModel : PVCModel, ISurface, ISerializable
	{
		[NonSerialized()]
		public const int WIDTH_PROPERTY_CODE = 1;
		[NonSerialized()]
		public const int HEIGHT_PROPERTY_CODE = 2;
		[NonSerialized()]
		public const int X_PROPERTY_CODE = 3;
		[NonSerialized()]
		public const int Y_PROPERTY_CODE = 4;
		[NonSerialized()]
		public const int BOUNDS_PROPERTY_CODE = 5;
		[NonSerialized()]
		public const int CODE_PROPERTY_CODE = 6;

		private string code;
		private RectangleF bounds = RectangleF.Empty;
		private Surface surfaceParent;
		private Sash sashParent;
		private PVCFrame frameParent;
		private object parentModel;
		private Surface surface;

		public SurfaceModel(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			surface = (Surface)info.GetValue("Surface", typeof(Surface));
			code = (string)info.GetValue("Code", typeof(string));
			bounds = (RectangleF)info.GetValue("Bounds", typeof(RectangleF));
			surfaceParent = (Surface)info.GetValue("SurfaceParent", typeof(Surface));
			sashParent = (Sash)info.GetValue("SashParent", typeof(Sash));
			frameParent = (PVCFrame)info.GetValue("FrameParent", typeof(PVCFrame));
		}

		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue("Surface", surface);
			info.AddValue("Code", Code);
			info.AddValue("Bounds", Bounds);
			info.AddValue("SurfaceParent", SurfaceParent);
			info.AddValue("SashParent", SashParent);
			info.AddValue("FrameParent", FrameParent);
		}

		public SurfaceModel(Surface surface)
		{
			this.surface = surface;
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

		public object Parent { get { return parentModel; } }

		public Surface SurfaceParent
		{
			get { return surfaceParent; }
			set
			{
				surfaceParent = value;
				parentModel = surfaceParent.Model;
			}
		}

		public Sash SashParent
		{
			get { return sashParent; }
			set
			{
				sashParent = value;
				parentModel = sashParent.Model;
			}
		}

		public PVCFrame FrameParent
		{
			get { return frameParent; }
			set
			{
				frameParent = value;
				parentModel = frameParent.Model;
			}
		}

		public Orientation Orientation { get; set; }

		public IList<object> Children
		{
			get
			{
				List<object> children = new List<object>();

				foreach (PNode child in surface.ChildrenReference)
				{
					if (child is Surface)
						children.Add(((Surface)child).Model);
					else if (child is Mullion)
						children.Add(((Mullion)child).Model);
					else if (child is Sash)
						children.Add(((Sash)child).Model);
					else if (child is Filling)
						children.Add(((Filling)child).Model);
				}
				return children;
			}
			set { }
		}

		public IMullion Previous { get; set; }

		public IMullion Next { get; set; }

		public float X
		{
			get { return bounds.X; }
			set
			{
				if (value != bounds.X)
				{
					bounds.X = value;
					OnPropertyChanged(new PVCModelPropertyChangedEventArgs(X_PROPERTY_CODE));
				}
			}
		}

		public float Y
		{
			get { return bounds.Y; }
			set
			{
				if (value != bounds.Y)
				{
					bounds.Y = value;
					OnPropertyChanged(new PVCModelPropertyChangedEventArgs(Y_PROPERTY_CODE));
				}
			}
		}

		public float Width
		{
			get { return bounds.Width; }
			set
			{
				if (value != bounds.Width)
				{
					bounds.Width = value;
					OnPropertyChanged(new PVCModelPropertyChangedEventArgs(WIDTH_PROPERTY_CODE));
				}
			}
		}

		public float Height
		{
			get { return bounds.Height; }
			set
			{
				if (value != bounds.Height)
				{
					bounds.Height = value;
					OnPropertyChanged(new PVCModelPropertyChangedEventArgs(HEIGHT_PROPERTY_CODE));
				}
			}
		}

		public RectangleF Bounds
		{
			get { return bounds; }
			set
			{
				if (!value.Equals(bounds))
				{
					bounds = value;
					OnPropertyChanged(new PVCModelPropertyChangedEventArgs(BOUNDS_PROPERTY_CODE));
				}
			}
		}
	}
}

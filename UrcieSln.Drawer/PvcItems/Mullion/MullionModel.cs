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
	public class MullionModel : PVCModel, IMullion, ISerializable
	{
		[NonSerialized()]
		public const int CODE_PROPERTY_CODE = 1;
		[NonSerialized()]
		public const int LENGTH_PROPERTY_CODE = 2;
		[NonSerialized()]
		public const int MIDDLE_POINT_PROPERTY_CODE = 3;
		[NonSerialized()]
		public const int MULLION_TYPE_PROPERTY_CODE = 4;
		[NonSerialized()]
		public const int ORIENTATION_PROPERTY_CODE = 5;
		[NonSerialized()]
		public const int X_PROPERTY_CODE = 6;
		[NonSerialized()]
		public const int Y_PROPERTY_CODE = 7;
		[NonSerialized()]
		public const int VIRTUAL_PROPERTY_CODE = 8;

		private string code;
		private float length;
		private MullionType mullionType;
		private Orientation orientation;
		private float x;
		private float y;
		private Mullion mullion;
		private float middlePoint;
		private bool _virtual = false;

		public MullionModel(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			mullion = (Mullion)info.GetValue("mullion", typeof(Mullion));
			code = (string)info.GetValue("Code", typeof(string));
			length = (float)info.GetValue("Length", typeof(float));
			middlePoint = (float)info.GetValue("MiddlePoint", typeof(float));
			mullionType = (MullionType)info.GetValue("MullionType", typeof(MullionType));
			orientation = (Orientation)info.GetValue("Orientation", typeof(Orientation));
			x = (float)info.GetValue("X", typeof(float));
			y = (float)info.GetValue("Y", typeof(float));
			_virtual = (bool)info.GetValue("IsVirtual", typeof(bool));
		}

		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue("mullion", mullion);
			info.AddValue("Code", Code);
			info.AddValue("Length", Length);
			info.AddValue("MiddlePoint", MiddlePoint);
			info.AddValue("MullionType", MullionType);
			info.AddValue("Orientation", Orientation);
			info.AddValue("X", X);
			info.AddValue("Y", Y);
			info.AddValue("IsVirtual", IsVirtual);
		}
		public MullionModel(Mullion mullion) : base()
		{
			this.mullion = mullion;
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
		public float Length
		{
			get { return length; }
			set
			{
				if (value != length)
				{
					length = value;
					OnPropertyChanged(new PVCModelPropertyChangedEventArgs(LENGTH_PROPERTY_CODE));
				}
			}
		}
		public float MiddlePoint
		{
			get { return middlePoint; }

			set
			{
				if (value != middlePoint)
				{
					middlePoint = value;
					if (mullion.Frame != null)
						if (orientation == Orientation.Horizontal)
						{
							y = value - mullion.Thickness / 2 + mullion.Frame.Y;
						}
						else
						{
							x = value - mullion.Thickness / 2 + mullion.Frame.X;
						}
					OnPropertyChanged(new PVCModelPropertyChangedEventArgs(MIDDLE_POINT_PROPERTY_CODE));
				}
			}
		}
		public MullionType MullionType
		{
			get { return mullionType; }
			set
			{
				if (mullionType == null || !mullionType.Equals(value))
				{
					mullionType = value;
					OnPropertyChanged(new PVCModelPropertyChangedEventArgs(MULLION_TYPE_PROPERTY_CODE));
					OnPropertyChanged(new PVCModelPropertyChangedEventArgs(MIDDLE_POINT_PROPERTY_CODE));
				}
			}
		}
		public Orientation Orientation
		{
			get { return orientation; }
			set
			{
				if (value != orientation)
				{
					orientation = value;
					OnPropertyChanged(new PVCModelPropertyChangedEventArgs(ORIENTATION_PROPERTY_CODE));
				}
			}
		}
		public float X
		{
			get
			{
				return x;
			}

			set
			{
				if (value != x)
				{
					x = value;
					OnPropertyChanged(new PVCModelPropertyChangedEventArgs(X_PROPERTY_CODE));
					if (mullion.Frame != null)
						if (orientation == Orientation.Vertical)
							middlePoint = x + (mullion.Thickness / 2) - mullion.Frame.X;
				}
			}
		}
		public float Y
		{
			get
			{
				return y;
			}

			set
			{
				if (value != y)
				{
					y = value;
					OnPropertyChanged(new PVCModelPropertyChangedEventArgs(Y_PROPERTY_CODE));
					if ((mullion.Frame != null))
						if (orientation == Orientation.Horizontal)
							middlePoint = y + (mullion.Thickness / 2) - mullion.Frame.Y;
				}
			}
		}
		public float Width
		{
			get
			{
				if (Orientation == Orientation.Horizontal)
					return Length;
				else if (MullionType != null && MullionType.ProfileType != null)
					return MullionType.ProfileType.Thickness;

				throw new InvalidOperationException("Mullion is not truly initialized.");
			}
		}
		public float Height
		{
			get
			{
				if (Orientation == Orientation.Vertical)
					return Length;
				else if (MullionType != null && MullionType.ProfileType != null)
					return MullionType.ProfileType.Thickness;

				throw new InvalidOperationException("Mullion is not truly initialized.");
			}
		}
		public float Offset
		{
			get
			{
				if (Orientation == Orientation.Horizontal)
				{
					int index = mullion.Frame.HorizontalMullions().IndexOf(mullion);
					if (index == 0)
						return MiddlePoint;
					else
					{
						Mullion previous = mullion.Frame.HorizontalMullions().ElementAt(--index);
						float prevMidPoint = previous.Model.MiddlePoint;
						return MiddlePoint - previous.Model.MiddlePoint;
					}
				}
				else if (Orientation == Orientation.Vertical)
				{
					int index = mullion.Frame.VerticalMullions().IndexOf(mullion);
					if (index == 0)
						return MiddlePoint;
					else
					{
						Mullion previous = mullion.Frame.VerticalMullions().ElementAt(--index);
						return MiddlePoint - previous.Model.MiddlePoint;
					}
				}
				else
				{
					throw new InvalidOperationException("Invalid operation called.");
				}
			}
		}
		public float OffsetLeft
		{
			get
			{
				Surface prevSurf = mullion.PreviousSurface();
				Mullion prevMull = prevSurf.PreviousMullion();
				float middlePoint = -1;
				if (prevMull != null)
				{
					middlePoint = MiddlePoint - prevMull.Model.MiddlePoint;
				}
				else
				{
					SurfaceLayout parentLayout = (Orientation == Orientation.Vertical) ?
						SurfaceLayout.VERTICAL : SurfaceLayout.HORIZONTAL;
					Surface parent = (Surface)mullion.Parent;
					if (parent.Layout != parentLayout)
					{
						if (parent.Parent is Surface)
						{
							parent = (Surface)parent.Parent;
							if (parent.Layout != parentLayout)
							{
								throw new InvalidOperationException("Invalid Parent");
							}
							prevMull = parent.PreviousMullion();
							if (prevMull != null)
							{
								middlePoint = MiddlePoint - prevMull.Model.MiddlePoint;
							}
							else
							{
								middlePoint = MiddlePoint;
							}
						}
						else if (parent.Parent is PVCFrame)
						{
							middlePoint = MiddlePoint;
						}
						else if (parent.Parent is Sash)
						{
							Sash sashParent = (Sash)parent.Parent;
							if (orientation == Domain.Entities.Orientation.Vertical)
							{
								middlePoint = MiddlePoint - sashParent.X;
							}
							else if (orientation == Domain.Entities.Orientation.Horizontal)
							{
								middlePoint = MiddlePoint - sashParent.Y;
							}
						}
					}
					else
					{
						throw new InvalidOperationException("Invalid Parent given.");
					}
				}
				return middlePoint;
			}
		}
		public bool IsVirtual
		{
			get { return _virtual; }
			set
			{
				if(_virtual != value)
				{
					_virtual = value;

					OnPropertyChanged(new PVCModelPropertyChangedEventArgs(VIRTUAL_PROPERTY_CODE));
				}
			}
		}
		public ISurface Next { get; set; }
		public ISurface Parent { get { return ((Surface)mullion.Parent).Model; } }
		public ISurface Previous { get; set; }
	}
}

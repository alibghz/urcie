using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using UMD.HCIL.Piccolo;
using UrcieSln.Domain.Entities;

namespace UrcieSln.Drawer.PvcItems
{
	[Serializable()]
	public class Mullion : PNode, ISerializable
	{
		public Mullion(Orientation orientation, MullionType mullionType)
			: base()
		{
			Model = new MullionModel(this);
			if (orientation == Orientation.Null)
			{
				orientation = Orientation.Vertical;
			}

			Model.Orientation = orientation;
			Model.MullionType = mullionType;
			Model.PropertyChanged += Model_PropertyChanged;
			FullBoundsChanged += Mullion_FullBoundsChanged;
		}

		public Mullion(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			FullBoundsChanged += Mullion_FullBoundsChanged;
		}

		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
		}

		void Model_PropertyChanged(object sender, PVCModelPropertyChangedEventArgs e)
		{
			switch (e.PropertyCode)
			{
				case MullionModel.LENGTH_PROPERTY_CODE:
				case MullionModel.MIDDLE_POINT_PROPERTY_CODE:
				case MullionModel.MULLION_TYPE_PROPERTY_CODE:
				case MullionModel.ORIENTATION_PROPERTY_CODE:
				case MullionModel.X_PROPERTY_CODE:
				case MullionModel.Y_PROPERTY_CODE:
					if (!SetBounds(Model.X, Model.Y, Model.Width, Model.Height))
					{
						Model.X = X;
						Model.Y = Y;
						if (Model.Orientation == Orientation.Horizontal)
							Model.Length = Width;
						else
							Model.Length = Height;
					}
					else if (Frame != null)
					{
						Frame.OnDimensionChanged();
					}
					break;
				case MullionModel.CODE_PROPERTY_CODE:
					Label = Model.Code;
					InvalidatePaint();
					break;
				case MullionModel.VIRTUAL_PROPERTY_CODE:
					Model.MiddlePoint += 1;
					Model.MiddlePoint -= 1;
					Repaint();
					break;
			}
		}

		public PVCFrame Frame { get; set; }

		public MullionModel Model { get; set; }

		protected String Label { get; set; }

		public float Thickness
		{
			get
			{
				if (Model != null)
				{
					if (Model.IsVirtual)
						return 2;
					else
						if (Model.MullionType != null && Model.MullionType.ProfileType != null)
							return Model.MullionType.ProfileType.Thickness;
				}
				return 10;
			}
		}

		public override void ParentBoundsChanged()
		{
			Surface parent = (Surface)Parent;
			Surface prevSurface = PreviousSurface();
			Surface nextSurface = NextSurface();
			RectangleF nextBounds = nextSurface.Model.Bounds;
			RectangleF previousBounds = prevSurface.Model.Bounds;
			RectangleF mullinBounds = FullBounds;

			if (Model.Orientation == Orientation.Vertical)
			{
				mullinBounds.Y = Parent.Y;
				mullinBounds.Height = Parent.Height;
				Model.X = mullinBounds.X;
				Model.Y = mullinBounds.Y;
				Model.Length = mullinBounds.Height;
				previousBounds.Y = Y;
				previousBounds.Height = Height;
				nextBounds.Y = Y;
				nextBounds.Height = Height;
			}
			else
			{

				mullinBounds.X = Parent.X;
				mullinBounds.Y = prevSurface.Height + prevSurface.Y;
				mullinBounds.Width = parent.Width;

				Model.X = mullinBounds.X;
				Model.Y = mullinBounds.Y;
				Model.Length = mullinBounds.Width;
				previousBounds.X = X;
				previousBounds.Width = Width;

				nextBounds.Width = Width;
				nextBounds.X = X;
			}
			nextSurface.Model.Bounds = nextBounds;
			prevSurface.Model.Bounds = previousBounds;
		}

		public void Mullion_FullBoundsChanged(object sender, UMD.HCIL.Piccolo.Event.PPropertyEventArgs e)
		{
			Surface previousSurface = PreviousSurface();
			Surface nextSurface = NextSurface();
			if (previousSurface == null || nextSurface == null) return;

			RectangleF nextBounds = nextSurface.Model.Bounds;
			if (Model.Orientation == Orientation.Vertical)
			{
				float oldX = nextSurface.Model.X;
				nextBounds.X = Model.X + Thickness;
				nextBounds.Width = nextBounds.Width + (oldX - (Model.X + Thickness));
				previousSurface.Model.Width = Model.X - previousSurface.Model.X;

				nextSurface.Model.Bounds = nextBounds;
			}
			else if (Model.Orientation == Orientation.Horizontal)
			{
				float oldY = nextSurface.Model.Y;
				nextBounds.Y = Model.Y + Thickness;
				nextBounds.Height = nextBounds.Height + (oldY - (Model.Y + Thickness));
				previousSurface.Model.Height = Model.Y - previousSurface.Model.Y;

				nextSurface.Model.Bounds = nextBounds;
			}

		}
		public override bool SetBounds(float x, float y, float width, float height)
		{
			if (Model.Orientation == Orientation.Vertical)
			{
				float minX = MinX();
				float maxX = MaxX();
				if ((minX == -1 || x > minX) && (maxX == -1 || x < maxX))
					return base.SetBounds(x, y, Thickness, height);
			}
			else
			{
				float minY = MinY();
				float maxY = MaxY();
				if ((minY == -1 || y > minY) && (maxY == -1 || y < maxY))
					return base.SetBounds(x, y, width, Thickness);
			}
			return false;
		}
		public int Index()
		{
			if (Parent != null) return Parent.IndexOfChild(this);
			return -1;
		}
		public float MinX()
		{
			Surface previous = PreviousSurface();
			if (previous != null)
				return previous.X + previous.MinWidth();

			return -1;
		}
		public float MaxX()
		{
			Surface next = NextSurface();
			if (next != null)
				return next.MaxX() - Model.MullionType.ProfileType.Thickness;

			return -1;
		}
		public float MinY()
		{
			Surface previous = PreviousSurface();
			if (previous != null)
				return previous.Y + previous.MinHeight();

			return -1;
		}
		public float MaxY()
		{
			Surface next = NextSurface();
			if (next != null)
				return next.MaxY() - Model.MullionType.ProfileType.Thickness;

			return -1;
		}
		public Surface NextSurface()
		{
			int index = Index();
			if (index++ != -1 && index < Parent.ChildrenCount)
				return (Surface)Parent.GetChild(index);

			return null;
		}
		public Surface PreviousSurface()
		{
			int index = Index();
			if (index-- != -1 && index < Parent.ChildrenCount)
				return (Surface)Parent.GetChild(index);

			return null;
		}

		#region Overriden methods
		public override void AddChild(int index, PNode child) { }
		public override void AddChild(PNode child) { }
		public override void AddChildren(UMD.HCIL.Piccolo.Util.PNodeList nodes) { }
		#endregion

		protected override void Paint(UMD.HCIL.Piccolo.Util.PPaintContext paintContext)
		{
			Graphics graphics = paintContext.Graphics;
			graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

			if (Model != null && Model.IsVirtual)
			{
				graphics.FillRectangle(Brushes.Red, Bounds);
				return;
			}

			Pen pen = new Pen(Brushes.Black, 2);
			graphics.FillRectangle(Brushes.WhiteSmoke, Model.X, Model.Y, Model.Width, Model.Height);
			if (Model.Orientation == Orientation.Horizontal)
			{
				graphics.DrawLine(pen, X, Y, X, Y + Thickness);
				graphics.DrawLine(pen, X, Y, X + Width, Y);
				graphics.DrawLine(pen, X, Y + Thickness, X + Width, Y + Thickness);
			}
			else if (Model.Orientation == Orientation.Vertical)
			{
				graphics.DrawLine(pen, X, Y, X + Thickness, Y);
				graphics.DrawLine(pen, X, Y, X, Y + Height);
				graphics.DrawLine(pen, X + Thickness, Y, X + Thickness, Y + Height);
			}

			if (Label != null && Frame.ShowCodes)
			{
				graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
				Font font = new Font("Arial", Frame.FontSize, FontStyle.Regular);
				if (Model.Orientation == Orientation.Horizontal)
				{
					graphics.DrawString(Label, font, Brushes.Black,
						new PointF(Model.X + Model.Length / 2 - (Label.Length * font.Size) / 2, Model.Y - font.Height / 2 + Thickness / 2));
				}
				else
				{
					graphics.DrawString(Label, font, Brushes.Black,
						new PointF(Model.X + Thickness / 2 - (font.Height / 2), Model.Y + Model.Length / 2 - (Label.Length * font.Size) / 2),
						new StringFormat(StringFormatFlags.DirectionVertical));
				}
			}
		}
	}
}

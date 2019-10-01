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
	public class Sash : PNode, ISerializable
	{
		private Surface surfaceParent;
		private string direction;
		private bool _fixed = false;

		public Sash(Surface surfaceParent, SashType sashType)
			: base()
		{
			if (surfaceParent.ChildrenCount > 0)
				throw new ArgumentException("Invalid surface parent provided.");

			this.surfaceParent = surfaceParent;
			Model = new SashModel();
			Model.SashType = sashType;

			if (surfaceParent.Model.Width < MinWidth() || surfaceParent.Model.Height < MinHeight())
				throw new ArgumentException("Too small parent.");

			Surface = new Surface();
			Model.PropertyChanged += Model_PropertyChanged;
			Model.SurfaceParent = this.surfaceParent;
			SurfaceParent().AddChild(this);
			Model.SurfaceChild = Surface;
			Surface.Model.SashParent = this;
		}
		public Sash(SerializationInfo info, StreamingContext context) : base(info, context) { }
		public string Direction
		{
			get { return direction; }
			set
			{
				direction = value;
				Repaint();
			}
		}
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
		}
		void Model_PropertyChanged(object sender, PVCModelPropertyChangedEventArgs e)
		{
			switch (e.PropertyCode)
			{
				case SashModel.PARENT_PROPERTY_CODE:
					Bounds = SurfaceParent().Model.Bounds;
					break;
				case SashModel.SASH_TYPE_PROPERTY_CODE:
					Repaint();
					break;
				case SashModel.CHILD_PROPERTY_CODE:
					RemoveAllChildren();
					Surface = Model.SurfaceChild;
					Surface.Frame = SurfaceParent().Frame;
					Surface.Model.Bounds = InnerBounds;
					AddChild(Surface);
					break;
				case SashModel.CODE_PROPERTY_CODE:
					Repaint();
					break;
			}
			Surface.ParentBoundsChanged();
		}
		public Surface SurfaceParent()
		{
			return surfaceParent;
		}
		public float Thickness
		{
			get
			{
				if (Model != null && Model.SashType != null)
					return Model.SashType.ProfileType.Thickness;
				return 0;
			}
		}
		public float MinWidth()
		{
			if (ChildrenCount > 0)
			{
				return ((Surface)GetChild(0)).MinWidth() + Thickness * 2;
			}
			return Thickness * 2;
		}
		public float MinHeight()
		{
			if (ChildrenCount > 0)
			{
				return ((Surface)GetChild(0)).MinHeight() + Thickness * 2;
			}
			return Thickness * 2;
		}
		public override bool SetBounds(float x, float y, float width, float height)
		{
			if (width < MinWidth()) return false;
			if (height < MinHeight()) return false;

			return base.SetBounds(x, y, width, height);
		}
		public Surface Surface { get; set; }
		public SashModel Model { get; set; }
		public RectangleF InnerBounds
		{
			get
			{
				if (Model != null && Model.SashType != null && Model.SashType.ProfileType != null)
				{
					float thickness = Model.SashType.ProfileType.Thickness;
					return new
					RectangleF(X + thickness, Y + thickness, Width - 2 * thickness, Height - 2 * thickness);
				}
				return RectangleF.Empty;
			}
		}
		public override void ParentBoundsChanged()
		{
			Bounds = Parent.Bounds;
		}
		protected override void Paint(UMD.HCIL.Piccolo.Util.PPaintContext paintContext)
		{
			base.Paint(paintContext);
			Graphics graphics = paintContext.Graphics;
			graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

			Pen pen = new Pen(Brushes.Black, 2);

			RectangleF innerBounds = InnerBounds;
			float thickness = Thickness;

			graphics.FillRectangle(Brushes.WhiteSmoke, X, Y, Width, thickness);
			graphics.FillRectangle(Brushes.WhiteSmoke, X, Y, thickness, Height);
			graphics.FillRectangle(Brushes.WhiteSmoke, X + Width - thickness, Y, thickness, Height);
			graphics.FillRectangle(Brushes.WhiteSmoke, X, Y + Height - thickness, Width, thickness);

			graphics.FillRectangle(Brushes.White, innerBounds);

			graphics.DrawRectangle(pen, X, Y, Width, Height);
			graphics.DrawRectangle(pen, innerBounds.X, innerBounds.Y, innerBounds.Width, innerBounds.Height);


			graphics.DrawLine(pen, X, Y, innerBounds.X, innerBounds.Y);
			graphics.DrawLine(pen, X + Width, Y, innerBounds.X + innerBounds.Width, innerBounds.Y);
			graphics.DrawLine(pen, X + Width, Y + Height, innerBounds.X + innerBounds.Width, innerBounds.Y + innerBounds.Height);
			graphics.DrawLine(pen, X, Y + Height, innerBounds.X, innerBounds.Y + innerBounds.Height);

			if (!string.IsNullOrEmpty(Direction))
			{
				pen.Color = Color.Black;
				if (direction.Equals("Top"))
				{
					graphics.DrawLine(pen, innerBounds.X + innerBounds.Width / 2, innerBounds.Y, innerBounds.X, innerBounds.Y + innerBounds.Height);
					graphics.DrawLine(pen, innerBounds.X + innerBounds.Width / 2, innerBounds.Y, innerBounds.X + innerBounds.Width, innerBounds.Y + innerBounds.Height);
				}
				else if (direction.Equals("Right"))
				{
					graphics.DrawLine(pen, innerBounds.X, innerBounds.Y, innerBounds.X + innerBounds.Width, innerBounds.Y + innerBounds.Height / 2);
					graphics.DrawLine(pen, innerBounds.X + innerBounds.Width, innerBounds.Y + innerBounds.Height / 2, innerBounds.X, innerBounds.Y + innerBounds.Height);
				}
				else if (direction.Equals("Left"))
				{
					graphics.DrawLine(pen, innerBounds.X + innerBounds.Width, innerBounds.Y, innerBounds.X, innerBounds.Y + innerBounds.Height / 2);
					graphics.DrawLine(pen, innerBounds.X, innerBounds.Y + innerBounds.Height / 2, innerBounds.X + innerBounds.Width, innerBounds.Y + innerBounds.Height);
				}
				else if (direction.Equals("Bottom"))
				{
					graphics.DrawLine(pen, innerBounds.X, innerBounds.Y, innerBounds.X + innerBounds.Width / 2, innerBounds.Y + innerBounds.Height);
					graphics.DrawLine(pen, innerBounds.X + innerBounds.Width / 2, innerBounds.Height + innerBounds.Y, innerBounds.X + innerBounds.Width, innerBounds.Y);
				}
			}

			if (_fixed && !string.IsNullOrEmpty(Direction))
			{
				float accessoryThickness = 16;
				float accessoryLength = 60;
				pen.Color = Color.Black;
				if (direction.Equals("Top"))
				{
					graphics.FillRectangle(Brushes.Gray, X, Y + (innerBounds.Y - Y) / 2 - accessoryThickness / 2, accessoryLength, accessoryThickness);
					graphics.DrawRectangle(pen, X, Y + (innerBounds.Y - Y) / 2 - accessoryThickness / 2, accessoryLength, accessoryThickness);
					graphics.FillRectangle(Brushes.Gray, X + Width - accessoryLength, Y + (innerBounds.Y - Y) / 2 - accessoryThickness / 2, accessoryLength, accessoryThickness);
					graphics.DrawRectangle(pen, X + Width - accessoryLength, Y + (innerBounds.Y - Y) / 2 - accessoryThickness / 2, accessoryLength, accessoryThickness);
				}
				else if (direction.Equals("Right"))
				{
					graphics.FillRectangle(Brushes.Gray, X + Width - (innerBounds.X - X) / 2 - (accessoryThickness / 2), Y, accessoryThickness, accessoryLength);
					graphics.DrawRectangle(pen, X + Width - (innerBounds.X - X) / 2 - (accessoryThickness / 2), Y, accessoryThickness, accessoryLength);
					graphics.FillRectangle(Brushes.Gray, X + Width - (innerBounds.X - X) / 2 - (accessoryThickness / 2), Y + Height - accessoryLength, accessoryThickness, accessoryLength);
					graphics.DrawRectangle(pen, X + Width - (innerBounds.X - X) / 2 - (accessoryThickness / 2), Y + Height - accessoryLength, accessoryThickness, accessoryLength);
				}
				else if (direction.Equals("Left"))
				{
					graphics.FillRectangle(Brushes.Gray, X + (innerBounds.X - X) / 2 - accessoryThickness / 2, Y, accessoryThickness, accessoryLength);
					graphics.DrawRectangle(pen, X + (innerBounds.X - X) / 2 - accessoryThickness / 2, Y, accessoryThickness, accessoryLength);
					graphics.FillRectangle(Brushes.Gray, X + (innerBounds.X - X) / 2 - accessoryThickness / 2, Y + Height - accessoryLength, accessoryThickness, accessoryLength);
					graphics.DrawRectangle(pen, X + (innerBounds.X - X) / 2 - accessoryThickness / 2, Y + Height - accessoryLength, accessoryThickness, accessoryLength);
				}
				else if (direction.Equals("Bottom"))
				{
					graphics.FillRectangle(Brushes.Gray, X, Y + Height - (innerBounds.Y - Y) / 2 - accessoryThickness / 2, accessoryLength, accessoryThickness);
					graphics.DrawRectangle(pen, X, Y + Height - (innerBounds.Y - Y) / 2 - accessoryThickness / 2, accessoryLength, accessoryThickness);
					graphics.FillRectangle(Brushes.Gray, X + Width - accessoryLength, Y + Height - (innerBounds.Y - Y) / 2 - accessoryThickness / 2, accessoryLength, accessoryThickness);
					graphics.DrawRectangle(pen, X + Width - accessoryLength, Y + Height - (innerBounds.Y - Y) / 2 - accessoryThickness / 2, accessoryLength, accessoryThickness);
				}
			}


			if (!string.IsNullOrEmpty(Direction) && !_fixed)
			{
				float accessoryThickness = 16;
				float accessoryLength = 60;
				pen.Color = Color.Black;
				if (direction.Equals("Top"))
				{
					graphics.FillRectangle(Brushes.Gray, X + Width / 2 - (accessoryLength / 2), Y + Model.SashType.ProfileType.Thickness / 2 - (accessoryThickness / 2), accessoryLength, accessoryThickness);
					graphics.DrawRectangle(pen, X + Width / 2 - (accessoryLength / 2), Y + Model.SashType.ProfileType.Thickness / 2 - (accessoryThickness / 2), accessoryLength, accessoryThickness);
				}
				else if (direction.Equals("Right"))
				{
					graphics.FillRectangle(Brushes.Gray, X + Width - (Model.SashType.ProfileType.Thickness / 2) - (accessoryThickness / 2), Y + (Height / 2) - (accessoryLength / 2), accessoryThickness, accessoryLength);
					graphics.DrawRectangle(pen, X + Width - (Model.SashType.ProfileType.Thickness / 2) - (accessoryThickness / 2), Y + (Height / 2) - (accessoryLength / 2), accessoryThickness, accessoryLength);
				}
				else if (direction.Equals("Left"))
				{
					graphics.FillRectangle(Brushes.Gray, X + Model.SashType.ProfileType.Thickness - (Model.SashType.ProfileType.Thickness / 2) - (accessoryThickness / 2), Y + (Height / 2) - (accessoryLength / 2), accessoryThickness, accessoryLength);
					graphics.DrawRectangle(pen, X + Model.SashType.ProfileType.Thickness - (Model.SashType.ProfileType.Thickness / 2) - (accessoryThickness / 2), Y + (Height / 2) - (accessoryLength / 2), accessoryThickness, accessoryLength);
				}
				else if (direction.Equals("Bottom"))
				{
					graphics.FillRectangle(Brushes.Gray, X + Width / 2 - (accessoryLength / 2), Y + Height - (Model.SashType.ProfileType.Thickness / 2) - (accessoryThickness / 2), accessoryLength, accessoryThickness);
					graphics.DrawRectangle(pen, X + Width / 2 - (accessoryLength / 2), Y + Height - (Model.SashType.ProfileType.Thickness / 2) - (accessoryThickness / 2), accessoryLength, accessoryThickness);
				}
			}

			if (Model.Code != null && Model.SurfaceParent.Frame.ShowCodes)
			{
				Font font = new Font("Arial", Model.SurfaceParent.Frame.FontSize, FontStyle.Regular);
				PointF labelPosition;
				if (!string.IsNullOrEmpty(Direction) && Direction.Equals("Top"))
				{
					labelPosition = new PointF(X + Width / 2 - (font.Size * Model.Code.Length) / 2, Y + Height - Model.SashType.ProfileType.Thickness + (innerBounds.Y - Y) / 2 - font.Height / 2);
				}
				else
				{
					labelPosition = new PointF(X + Width / 2 - (font.Size * Model.Code.Length) / 2, Y + (innerBounds.Y - Y) / 2 - font.Height / 2);
				}

				graphics.DrawString(Model.Code, font, Brushes.Black, labelPosition);
			}
		}
		public float MaxX()
		{
			return ((Surface)GetChild(0)).MaxX() - Thickness;
		}
		public float MaxY()
		{
			return ((Surface)GetChild(0)).MaxY() - Thickness;
		}
		public bool Fixed
		{
			get { return _fixed; }
			set
			{
				if (_fixed != value)
				{
					_fixed = value;
					Repaint();
				}
			}
		}
	}
}

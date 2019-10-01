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
	public class PVCFrame : PNode, ISerializable
	{
		private float fontSize = 16;
		private float MINX = 50;
		private float MINY = 50;
		public delegate void DimensionChangedEventHandler();
		public event DimensionChangedEventHandler DimensionChanged;
		protected List<Mullion> verticalMullions = new List<Mullion>();
		protected List<Mullion> horizontalMullions = new List<Mullion>();
		protected List<Sash> sashes = new List<Sash>();
		protected List<Filling> fillings = new List<Filling>();
		protected Surface surface;
		protected bool showAxis = false;
		private bool showChodes = false;

		public PVCFrame(float width, float height, ProfileType profileType)
			: base()
		{
			Brush = new SolidBrush(Color.White);

			Model = new PVCFrameModel();
			Model.PropertyChanged += Model_PropertyChanged;

			Model.ProfileType = profileType; // Setting profile type is done in the very first step always
			Model.Width = width;
			Model.Height = height;
			Model.Code = "A0";
			Surface = new Surface();
			Surface.Frame = this;
            Surface.Model.FrameParent = this;

            Arc arc = new Arc(this);
		}

		/// <summary>
		/// Default constructor for deserialization
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		public PVCFrame(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			
		}

		/// <summary>
		/// Used for serialization
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
		}

		/// <summary>
		/// Gets or sets the PVCFrameModel
		/// </summary>
		public PVCFrameModel Model { get; set; }

		/// <summary>
		/// Returns the profile type thickness
		/// </summary>
		public float FrameThickness
		{
			get { return Model.ProfileType.Thickness; }
		}

		/// <summary>
		/// The surface which holds the aFrame items in it's children reference
		/// </summary>
		public Surface Surface
		{
			get { return surface; }
			set
			{
				if (!value.IsDescendentOf(this))
				{
					RemoveAllChildren();
					surface = value;
					LayoutChildren();
					AddChild(surface);
					Model.SurfaceChild = surface;
				}
			}
		}

		/// <summary>
		/// The innerbounds based on the profile type thickness
		/// </summary>
		public RectangleF InnerBounds
		{
			get
			{
				return new RectangleF(
					X + FrameThickness,
					Y + FrameThickness,
					(Width) - FrameThickness * 2,
					(Height) - FrameThickness * 2);
			}
		}

		/// <summary>
		/// The label of this aFrame (It's code)
		/// </summary>
		protected String Label { get; set; }

		public float FontSize
		{
			get
			{
				return fontSize;
			}
			set
			{
				if (value != fontSize)
				{
					fontSize = value;
					Repaint();
				}
			}
		}

		/// <summary>
		/// Gets or Sets a value indicating whether surfaces show the axis lines or not
		/// </summary>
		public bool ShowAxis
		{
			get { return showAxis; }
			set
			{
				if (value != showAxis)
				{
					showAxis = value;
					Repaint();
				}
			}
		}

		/// <summary>
		/// Gets or sets a value indicating weather codes be printed on canvas or not
		/// </summary>
		public bool ShowCodes
		{
			get { return showChodes; }
			set
			{
				if (value != showChodes)
				{
					showChodes = value;
					Repaint();
				}
			}
		}

		/// <summary>
		/// Updates the aFrame based on the Model values
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void Model_PropertyChanged(object sender, PVCModelPropertyChangedEventArgs e)
		{
			switch (e.PropertyCode)
			{
				case PVCFrameModel.WIDTH_PROPERTY_CODE:
				case PVCFrameModel.HEIGHT_PROPERTY_CODE:
					if (!SetBounds(MINX, MINY, Model.Width, Model.Height))
					{
						Model.Width = Width;
						Model.Height = Height;
					}
					else
					{
						OnDimensionChanged();
					}
					break;
				case PVCFrameModel.CODE_PROPERTY_CODE:
					Label = Model.Code;
					ChildrenManager.UpdateFillingsCode(this);
					ChildrenManager.UpdateSashesCode(this);
					ChildrenManager.UpdateMullionsCode(this, Orientation.Horizontal);
					ChildrenManager.UpdateMullionsCode(this, Orientation.Vertical);
					InvalidatePaint();
					break;
				case PVCFrameModel.PROFILE_PROPERTY_CODE:
					InvalidateLayout();
					break;
				case PVCFrameModel.COLOR_PROPERTY_CODE:
				case PVCFrameModel.BORDER_COLOR_PROPERTY_CODE:
					InvalidatePaint();
					break;
			}
		}

		/// <summary>
		/// Notifies the event subscribers of a change in aFrame's dimension
		/// </summary>
		public void OnDimensionChanged()
		{
			DimensionChangedEventHandler handler = DimensionChanged;
			if (handler != null)
			{
				handler();
			}
			Repaint();
		}

		/// <summary>
		/// Overriden, If parameters are valid values applies them
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <returns></returns>
		public override bool SetBounds(float x, float y, float width, float height)
		{
			float minWidth = (float)MinWidth();
			float minHeight = (float)MinHeight();

			if (width < minWidth) return false;
			if (height < minHeight) return false;
			return base.SetBounds(MINX, MINY, width, height);
		}

		/// <summary>
		/// Minimum width of the aFrame
		/// </summary>
		/// <returns></returns>
		public double MinWidth()
		{
			if (Surface != null)
			{
				return FrameThickness * 2 + Surface.MinWidth();
			}
			else
				return FrameThickness * 2;
		}

		/// <summary>
		/// Minimum height of the aFrame
		/// </summary>
		/// <returns></returns>
		public double MinHeight()
		{
			if (Surface != null)
			{
				return FrameThickness * 2 + Surface.MinHeight();
			}
			else
				return FrameThickness * 2;
		}

		/// <summary>
		/// The maximum X coordinate of the aFrame
		/// </summary>
		/// <returns></returns>
		public double MaxX()
		{
			return Surface.MaxX() - FrameThickness - 1;
		}

		/// <summary>
		/// The maximum Y coordinate of the aFrame
		/// </summary>
		/// <returns></returns>
		public double MaxY()
		{
			return Surface.MaxY() - FrameThickness - 1;
		}

		/// <summary>
		/// Adjusts it's surface's bounds to it's innerBounds
		/// </summary>
		public override void LayoutChildren()
		{
			if (surface != null) surface.Model.Bounds = InnerBounds;
		}

		/// <summary>
		/// Gets a list of Horizontal mullions
		/// Caution: Do not modify this list, The aFrame object manages this
		/// by itself
		/// </summary>
		/// <returns></returns>
		public List<Mullion> HorizontalMullions()
		{
			horizontalMullions = horizontalMullions.OrderBy(o => o.Y).ToList();
			return horizontalMullions;
		}

		/// <summary>
		/// Gets a list of Vertical mullions
		/// </summary>
		/// <returns></returns>
		public List<Mullion> VerticalMullions()
		{
			verticalMullions = verticalMullions.OrderBy(o => o.X).ToList();
			return verticalMullions;
		}

		/// <summary>
		/// Adds a mullion reference to this aFrame object
		/// </summary>
		/// <param name="mullion"></param>
		internal void AddMullion(Mullion mullion)
		{
			if (mullion.Model.Orientation == Orientation.Vertical)
			{
				if (!verticalMullions.Contains(mullion))
				{
					verticalMullions.Add(mullion);
					ChildrenManager.UpdateMullionsCode(this, Orientation.Vertical);
				}
			}
			else if (mullion.Model.Orientation == Orientation.Horizontal)
			{
				if (!horizontalMullions.Contains(mullion))
				{
					horizontalMullions.Add(mullion);
					ChildrenManager.UpdateMullionsCode(this, Orientation.Horizontal);
				}
			}
		}

		/// <summary>
		/// Removes a mullion reference from this aFrame object
		/// </summary>
		/// <param name="mullion"></param>
		internal void RemoveMullion(Mullion mullion)
		{
			if (mullion.Model.Orientation == Orientation.Horizontal)
			{
				horizontalMullions.Remove(mullion);
				ChildrenManager.UpdateMullionsCode(this, Orientation.Horizontal);
			}
			else if (mullion.Model.Orientation == Orientation.Vertical)
			{
				verticalMullions.Remove(mullion);
				ChildrenManager.UpdateMullionsCode(this, Orientation.Vertical);
			}
			OnDimensionChanged();
		}

		/// <summary>
		/// Returns the list of dSash children in this aFrame
		/// </summary>
		/// <returns></returns>
		public List<Sash> Sashes()
		{
			return sashes;
		}

		/// <summary>
		/// Adds a Sash reference to this aFrame object
		/// </summary>
		/// <param name="dSash"></param>
		internal void AddSash(Sash sash)
		{
			if (!sashes.Contains(sash))
			{
				sashes.Add(sash);
				ChildrenManager.UpdateSashesCode(this);
			}
		}

		/// <summary>
		/// Removes a Sash reference from this aFrame object
		/// </summary>
		/// <param name="dSash"></param>
		internal void RemoveSash(Sash sash)
		{
			if (sashes.Contains(sash))
			{
				sashes.Remove(sash);
				ChildrenManager.UpdateSashesCode(this);
			}
		}

		/// <summary>
		/// Returns a list of filling children of this aFrame object
		/// </summary>
		/// <returns></returns>
		public List<Filling> Fillings()
		{
			return fillings;
		}

		/// <summary>
		/// Add a filling child reference to this aFrame object
		/// </summary>
		/// <param name="filling"></param>
		internal void AddFilling(Filling filling)
		{
			if (!fillings.Contains(filling))
			{
				fillings.Add(filling);
				ChildrenManager.UpdateFillingsCode(this);
			}
		}

		/// <summary>
		/// Removes a filling child reference from this aFrame object
		/// </summary>
		/// <param name="filling"></param>
		internal void RemoveFilling(Filling filling)
		{
			if (fillings.Contains(filling))
			{
				fillings.Remove(filling);
				ChildrenManager.UpdateFillingsCode(this);
			}
		}

		internal void ValidateChildren()
		{
			List<Mullion> invalidMullions = new List<Mullion>();
			foreach (Mullion mullion in HorizontalMullions())
				if (!mullion.IsDescendentOf(this)) invalidMullions.Add(mullion);

			foreach (Mullion mullion in invalidMullions)
				horizontalMullions.Remove(mullion);

			invalidMullions.Clear();

			foreach (Mullion mullion in VerticalMullions())
				if (!mullion.IsDescendentOf(this)) invalidMullions.Add(mullion);

			foreach (Mullion mullion in invalidMullions)
				verticalMullions.Remove(mullion);

			invalidMullions.Clear();

			Sashes().RemoveAll(s => s.IsDescendentOf(this) == false);
			Fillings().RemoveAll(f => f.IsDescendentOf(this) == false);

			OnDimensionChanged();
		}

		/// <summary>
		/// Paints the aFrame object
		/// </summary>
		/// <param name="paintContext"></param>
		protected override void Paint(UMD.HCIL.Piccolo.Util.PPaintContext paintContext)
		{
			Graphics graphics = paintContext.Graphics;
			graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
			graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

			RectangleF inBnds = InnerBounds;
			Pen pen = new Pen(Brushes.Black, 2);

			float x = X;
			float y = Y;
			float width = Width;
			float height = Height;
			float inX = inBnds.X;
			float inY = inBnds.Y;
			float inWidth = inBnds.Width;
			float inHeight = inBnds.Height;

			graphics.FillRectangle(Brushes.WhiteSmoke, x, y, width, height);
			graphics.FillRectangle(Brushes.White, inX, inY, inWidth, inHeight);
			graphics.DrawRectangle(pen, x, y, width, height);
			graphics.DrawRectangle(pen, inX, inY, inWidth, inHeight);
			graphics.DrawLine(pen, x, y, inX, inY);
			graphics.DrawLine(pen, x + width, y, inX + inWidth, inY);
			graphics.DrawLine(pen, x, y + height, inX, inY + inHeight);
			graphics.DrawLine(pen, x + width, y + height, inX + inWidth, inY + inHeight);

			if (ShowCodes && Label != null)
			{
				Font font = new Font("Arial", FontSize, FontStyle.Regular);
				PointF labelPosition = new PointF(x + Width / 2 - (Label.Length * font.Size) / 2, y + (inY - y) / 2 - font.Height / 2);
				graphics.DrawString(Label, font, Brushes.Black, labelPosition);
			}
		}
	}

	/// <summary>
	/// Utility class used for managing children codes
	/// </summary>
	static class ChildrenManager
	{
		internal static void UpdateMullionsCode(PVCFrame frame, Orientation orienation)
		{
			frame.ValidateChildren();
			List<Mullion> items = frame.HorizontalMullions(); ;
			string prefix = ".MH";

			if (orienation == Orientation.Vertical)
			{
				items = frame.VerticalMullions();
				prefix = ".MV";
			}

			for (int i = 0; i < items.Count; i++) items[i].Model.Code = frame.Model.Code + prefix + (i + 1).ToString();
		}

		internal static void UpdateSashesCode(PVCFrame frame)
		{
			frame.ValidateChildren();
			List<Sash> items = frame.Sashes();
			for (int i = 0; i < items.Count; i++)
				items[i].Model.Code = frame.Model.Code + ".SH" + (i + 1).ToString();
		}

		internal static void UpdateFillingsCode(PVCFrame frame)
		{
			frame.ValidateChildren();
			List<Filling> items = frame.Fillings();
			for (int i = 0; i < items.Count; i++)
				items[i].Model.Code = frame.Model.Code + ".FL" + (i + 1).ToString();
		}
	}
}

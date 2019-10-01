using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMD.HCIL.Piccolo;
using UMD.HCIL.Piccolo.Event;
using UMD.HCIL.Piccolo.Util;

namespace UrcieSln.Drawer.PvcItems
{
	public class FrameDecorator
	{
		private PVCFrame frame;
		private PLayer layer;
		private WidthAxis widthAxis;
		private HeightAxis heightAxis;

		public FrameDecorator(PVCFrame frame, PLayer layer)
		{
			this.frame = frame;
			this.layer = layer;
		}
		public void DisableFrameDecorator()
		{
			layer.RemoveChild(widthAxis);
			layer.RemoveChild(heightAxis);
			widthAxis.Reset();
			heightAxis.Reset();
			widthAxis = null;
			heightAxis = null;
		}
		public void EnableFrameDecorator()
		{
			widthAxis = new WidthAxis(this.frame, layer);
			heightAxis = new HeightAxis(this.frame, layer);
			layer.AddChild(widthAxis);
			layer.AddChild(heightAxis);
		}
		public RectangleF FullBounds()
		{
			if (widthAxis == null || heightAxis == null)
			{
				return RectangleF.Empty;
			}
			return new RectangleF(
					frame.X - 5,
					frame.Y - 5,
					heightAxis.DefaultX + heightAxis.DefaultWidth + 10,
					widthAxis.DefaultY + widthAxis.DefaultHeight + 10
				);
		}
	}

	public class WidthAxis : PNode
	{
		private PointF center = new PointF();

		private PCamera camera;
		private PLayer layer;
		private PVCFrame frame;

		private Brush fontBrush = Brushes.Black;
		private Pen pen = new Pen(Brushes.Black, 2);

		public WidthAxis(PVCFrame aFrame, PLayer aLayer)
		{
			layer = aLayer;
			frame = aFrame;
			camera = layer.GetCamera(0);
			camera.ViewTransformChanged += CameraVisibleChanged;
			frame.DimensionChanged += FrameDimensionChanged;
			frame.ChildrenChanged += FrameChildrenChanged;

			SetBounds(DefaultX, DefaultY, DefaultWidth, DefaultHeight);
		}
		public float DefaultHeight
		{
			get
			{
				return frame.FontSize * 5;
			}
		}
		public float DefaultWidth
		{
			get
			{
				if (frame != null) return frame.Model.Width;

				return 0;
			}
		}
		public float DefaultX
		{
			get
			{
				if (frame != null) return frame.X;
				return 0;
			}
		}
		public float DefaultY
		{
			get
			{
				if (frame != null) return frame.Y + frame.Model.Height + OFFSET / 2;
				return 0;
			}
		}
		public float OFFSET
		{
			get
			{
				return frame.FontSize * 3;
			}
		}
		private PointF Center
		{
			get
			{
				center.X = X + Width / 2 - (Width.ToString().Length * 7) / 2;
				center.Y = Y + (DefaultHeight - OFFSET + 5);
				return center;
			}
		}
		private PointF LabelPosition(float middlePoint, float offset)
		{
			return new PointF(middlePoint + X - (offset / 2) - (offset.ToString().Length * 7) / 2, Y + 5);
		}
		public void Reset()
		{
			frame.DimensionChanged -= FrameDimensionChanged;
			frame.ChildrenChanged -= FrameChildrenChanged;
			camera.VisibleChanged -= CameraVisibleChanged;
		}
		private void CameraVisibleChanged(object sender, PPropertyEventArgs e)
		{
			SetBounds(DefaultX, DefaultY, DefaultWidth, DefaultHeight);
		}
		private void FrameChildrenChanged(object sender, PPropertyEventArgs e)
		{
			FrameDimensionChanged();
		}
		public void FrameDimensionChanged()
		{
			SetBounds(DefaultX, DefaultY, DefaultWidth, DefaultHeight);
			Repaint();
		}
		protected override void Paint(PPaintContext paintContext)
		{
			Graphics graphics = paintContext.Graphics;
			graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			graphics.FillRectangle(Brushes.White, X, Y, Width, Height);

			float cap = 3;
			if (paintContext.Canvas != null && paintContext.Camera.ViewScale < 1)
				cap = cap * (1 + (1 - paintContext.Camera.ViewScale) * 2);

			float W = X + Width;
			float H = Y + cap;

			PointF[] points = new PointF[] { new PointF(X, H), new PointF(W, H) };
			graphics.DrawLines(pen, points);

			List<Mullion> mullions = frame.VerticalMullions();

			points[0].X = points[1].X = X;
			points[0].Y -= cap;
			points[1].Y += cap;
			graphics.DrawLines(pen, points);

			Font font = new Font("Arial", frame.FontSize, FontStyle.Regular);

			float offset = 0;
			float middlePoint = 0;



			foreach (Mullion mullion in mullions)
			{
				offset = mullion.Model.Offset;
				middlePoint = mullion.Model.MiddlePoint;

				if (offset != 0)
					graphics.DrawString(offset.ToString(), font, fontBrush, LabelPosition(middlePoint, offset));

				// Draw the cap
				points[0].X = points[1].X = middlePoint + frame.X;
				graphics.DrawLines(pen, points);
			}
			offset = Width - middlePoint;
			middlePoint = Width;
			graphics.DrawString(offset.ToString(), font, fontBrush, LabelPosition(middlePoint, offset));
			points[0].X = points[1].X = W;
			graphics.DrawLines(pen, points);

			// Main axis
			H = Y + DefaultHeight - OFFSET;
			points = new PointF[] { 
				new PointF(X, H),
				new PointF(X, H + cap * 2),
				new PointF(X, H + cap),
				new PointF(W, H + cap),
				new PointF(W, H),
				new PointF(W, H + cap * 2)
			};
			graphics.DrawLines(pen, points);
			graphics.DrawString(Width.ToString(), font, fontBrush, Center);
		}
	}

	public class HeightAxis : PNode
	{
		private PLayer layer;
		private PCamera camera;
		private PVCFrame frame;

		private Pen pen = new Pen(Brushes.Black, 2);
		private Brush fontBrush = Brushes.Black;

		public HeightAxis(PVCFrame aFrame, PLayer aLayer)
		{
			this.frame = aFrame;
			this.layer = aLayer;

			layer = aLayer;
			frame = aFrame;
			camera = layer.GetCamera(0);
			camera.ViewTransformChanged += CameraVisibleChanged;
			frame.DimensionChanged += FrameDimensionChanged;
			frame.ChildrenChanged += FrameChildrenChanged;

			SetBounds(DefaultX, DefaultY, DefaultWidth, DefaultHeight);
		}

		public float DefaultHeight
		{
			get
			{
				if (frame != null) return frame.Model.Height;

				return 0;
			}
		}
		public float DefaultWidth
		{
			get
			{
				return frame.FontSize * 5;
			}
		}
		private float OFFSET
		{
			get
			{
				return frame.FontSize * 3;
			}
		}
		public float DefaultX
		{
			get
			{
				if (frame != null) return frame.X + frame.Model.Width + OFFSET / 2;
				return 0;
			}
		}
		public float DefaultY
		{
			get
			{
				if (frame != null) return frame.Y;
				return 0;
			}
		}
		private PointF Center
		{
			get
			{
				return new PointF(X + (DefaultWidth - OFFSET + 5), Y + Height / 2 - (Height.ToString().Length * 7) / 2);
			}
		}
		private PointF LabelPosition(float middlePoint, float offset)
		{
			return new PointF(X + 5, middlePoint + Y - (offset / 2) - (offset.ToString().Length * 7) / 2);
		}
		public void Reset()
		{
			frame.DimensionChanged -= FrameDimensionChanged;
			frame.ChildrenChanged -= FrameChildrenChanged;
			camera.VisibleChanged -= CameraVisibleChanged;
		}
		private void CameraVisibleChanged(object sender, PPropertyEventArgs e)
		{
			SetBounds(DefaultX, DefaultY, DefaultWidth, DefaultHeight);
		}
		private void FrameChildrenChanged(object sender, PPropertyEventArgs e)
		{
			FrameDimensionChanged();
		}
		public void FrameDimensionChanged()
		{
			SetBounds(DefaultX, DefaultY, DefaultWidth, DefaultHeight);
			Repaint();
		}
		protected override void Paint(PPaintContext paintContext)
		{
			Graphics graphics = paintContext.Graphics;
			graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;


			graphics.FillRectangle(Brushes.White, X, Y, Width, Height);
			float cap = 3;
			if (paintContext.Canvas != null && paintContext.Camera.ViewScale < 1) cap = cap * (1 + (1 - paintContext.Camera.ViewScale) * 2);
			float W = X + cap;
			float H = Y + Height;



			PointF[] points = new PointF[] { new PointF(W, Y), new PointF(W, H) };
			graphics.DrawLines(pen, points);

			points[0].X = W - cap;
			points[0].Y = Y;
			points[1].X = W + cap;
			points[1].Y = Y;
			graphics.DrawLines(pen, points);

			StringFormat stringFormat = new StringFormat(StringFormatFlags.DirectionVertical);
			Font font = new Font("Arial", frame.FontSize, FontStyle.Regular);

			float offset = 0;
			float middlePoint = 0;

			foreach (Mullion mullion in frame.HorizontalMullions())
			{
				offset = mullion.Model.Offset;
				middlePoint = mullion.Model.MiddlePoint;
				if (offset != 0)
					graphics.DrawString(offset.ToString(), font, fontBrush, LabelPosition(middlePoint, offset), stringFormat);
				// Draw the cap
				points[0].Y = points[1].Y = middlePoint + frame.Y;
				graphics.DrawLines(pen, points);
			}
			offset = Height - middlePoint;
			middlePoint = Height;
			graphics.DrawString(offset.ToString(), font, fontBrush, LabelPosition(middlePoint, offset), stringFormat);
			points[0].Y = points[1].Y = H;
			graphics.DrawLines(pen, points);

			points[0].Y = points[1].Y = H;
			graphics.DrawLines(pen, points);

			W = X + Width - OFFSET;
			H = Y + Height;
			points = new PointF[] { 
				new PointF(W, Y),
				new PointF(W + cap * 2, Y),
				new PointF(W + cap, Y),
				new PointF(W + cap, H),
				new PointF(W, H),
				new PointF(W + cap * 2, H)
			};
			graphics.DrawLines(pen, points);
			graphics.DrawString(Height.ToString(), font, fontBrush, Center, stringFormat);
		}
	}
}

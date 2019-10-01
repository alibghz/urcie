using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using UMD.HCIL.Piccolo;
using UMD.HCIL.Piccolo.Util;

namespace UrcieSln.Drawer.PvcItems
{
	[Serializable]
	public class Arc : PNode, ISerializable
	{
		private float defaultHeight = 400;
		private PVCFrame frame;

		public Arc(PVCFrame frame)
		{
			this.frame = frame;
			frame.AddChild(this);
			frame.DimensionChanged += frame_DimensionChanged;
			SetBounds(frame.X, frame.Y - defaultHeight, frame.Width, defaultHeight);
		}

		public Arc(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		public RectangleF InnerBounds
		{
			get
			{
				return new RectangleF(
					X + frame.Model.ProfileType.Thickness,
					Y + frame.Model.ProfileType.Thickness,
					Width - frame.Model.ProfileType.Thickness * 2,
					Height - frame.Model.ProfileType.Thickness * 2
				);
			}
		}

		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
		}

		void frame_DimensionChanged()
		{
			SetBounds(frame.X, frame.Y - Height, frame.Width, Height);
		}

		public override bool SetBounds(float x, float y, float width, float height)
		{
			float thickness = frame.Model.ProfileType.Thickness;
			if ((height - thickness) <= 0) return false;
			return base.SetBounds(x, y, width, height);
		}

		protected override void Paint(PPaintContext paintContext)
		{
			Graphics g = paintContext.Graphics;
			float thickness = frame.Model.ProfileType.Thickness;
			Image image = new Bitmap((int)Width, (int)Height * 2);
			Graphics imgGraphics = Graphics.FromImage(image);
			imgGraphics.FillEllipse(Brushes.WhiteSmoke, 0, 0, Width, Height * 2);
			imgGraphics.FillEllipse(Brushes.White, thickness, thickness, Width - (2 * thickness), Height * 2 - (2 * thickness));
			Rectangle rect = new Rectangle((int)X, (int)Y, (int)Width, (int)Height);
			Pen p = new Pen(Brushes.Black, 2);
			g.DrawImageUnscaledAndClipped(image, rect);
			g.DrawArc(p, X, Y, Width, Height * 2, 0, -180);
			g.DrawArc(p, X + thickness, Y + thickness, Width - (2 * thickness), Height * 2 - (2 * thickness), 0, -180);

			float _break = frame.FontSize;

			g.DrawLine(p,
						X + Width - 5 - (InnerBounds.X - X) / 2,
						Y + 10,
						X + Width + 5 - (InnerBounds.X - X) / 2,
						Y + 10);
			g.DrawLine(p,
						X + Width - 5 - (InnerBounds.X - X) / 2,
						Y + Height - 10,
						X + Width + 5 - (InnerBounds.X - X) / 2,
						Y + Height - 10);

			g.DrawLine(p,
						X + Width - (InnerBounds.X - X) / 2,
						Y + 10,
						X + Width - (InnerBounds.X - X) / 2, 
						Y + (Height / 2) - _break);
			g.DrawLine(p,
						X + Width - (InnerBounds.X - X) / 2,
						Y + (Height / 2) + _break / 2,
						X + Width - (InnerBounds.X - X) / 2, 
						Y + Height - 10);

			g.DrawString(
				Height.ToString(), new Font("Arial", frame.FontSize), Brushes.Black,
				new PointF(X + Width - (Height.ToString().Length * _break) / 2 - (InnerBounds.X - X) / 2, Y + Height / 2 - _break));
		}
	}
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using UMD.HCIL.Piccolo;

namespace UrcieSln.Drawer.PvcItems
{
	[Serializable()]
	public class Muntin : PNode, ISerializable
	{
		public Muntin() : base()
		{
		}

		public Muntin(SerializationInfo info, StreamingContext context) : base()
		{ }

		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
		}

		protected override void Paint(UMD.HCIL.Piccolo.Util.PPaintContext paintContext)
		{
			Graphics graphics = paintContext.Graphics;
			graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

			Pen pen = new Pen(Brushes.Red, 1);
			pen.DashStyle = System.Drawing.Drawing2D.DashStyle.DashDotDot;
			graphics.DrawRectangle(pen, X, Y, Width, Height);

			for (float i = X; i <= Width; i += 30)
			{
				graphics.DrawLine(pen, i, Y, i, Y + Height);
			}

			for (float j = Y; j <= Height; j += 30)
			{
				graphics.DrawLine(pen, X, j, X + Width, j);
			}
		}
	}
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMD.HCIL.Piccolo.Event;
using UrcieSln.Domain.Entities;

namespace UrcieSln.Drawer.PvcItems
{
	public class DivisionEventHandler : PBasicInputEventHandler
	{
		private Surface surface;

		public DivisionEventHandler(Surface surface) : base()
		{
			this.surface = surface;
		}

		public Orientation Orientation { get; set; }
		public MullionType MullionType { get; set; }
		public override void OnClick(object sender, PInputEventArgs e)
		{
			base.OnClick(sender, e);

			Point position = new Point((int)e.Position.X, (int)e.Position.Y);
			//if (e.PickedNode is Surface && MullionType != null
			//	&& !(e.PickedNode.Parent is Sash))
			if (e.PickedNode is Surface && MullionType != null)
			{
				RectangleF mBounds = new RectangleF(
				position.X, position.Y,
				MullionType.ProfileType.Thickness,
				MullionType.ProfileType.Thickness
				);

				if (Orientation == Orientation.Horizontal)
				{
					mBounds.Y = mBounds.Y - mBounds.Height / 2;
					if (e.PickedNode.Bounds.Contains(mBounds))
						SurfaceLayoutManager.AddChild(surface, position, SurfaceLayout.VERTICAL, MullionType);
				}
				else
				{
					mBounds.X = mBounds.X - mBounds.Height / 2;
					if (e.PickedNode.Bounds.Contains(mBounds))
						SurfaceLayoutManager.AddChild(surface, position, SurfaceLayout.HORIZONTAL, MullionType);
				}
			}
		}
	}
}

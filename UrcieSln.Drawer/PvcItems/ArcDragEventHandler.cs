using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMD.HCIL.Piccolo;
using UMD.HCIL.Piccolo.Event;

namespace UrcieSln.Drawer.PvcItems
{
	public class ArcDragEventHandler : PDragSequenceEventHandler
	{
		PCanvas canvas;
		PVCFrame frame;
		Arc arc;

		public ArcDragEventHandler(PCanvas canvas, PVCFrame frame)
		{
			this.canvas = canvas;
			this.frame = frame;
		}

		protected override bool ShouldStartDragInteraction(PInputEventArgs e)
		{
			if (e.PickedNode is Arc)
				return base.ShouldStartDragInteraction(e);

			return false;
		}

		protected override void OnStartDrag(object sender, PInputEventArgs e)
		{
			base.OnStartDrag(sender, e);

			arc = e.PickedNode as Arc;
		}

		protected override void OnDrag(object sender, PInputEventArgs e)
		{
			base.OnDrag(sender, e);

			SizeF delta = e.Delta;
			arc.SetBounds(arc.X, arc.Y + (int)delta.Height, arc.Width, arc.Height - (int)delta.Height);
		}
	}
}

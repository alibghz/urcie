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
	public class MullionDragEventHandler : PDragSequenceEventHandler
	{
		PCanvas canvas;
		PVCFrame frame;
		Mullion mullion;

		public MullionDragEventHandler(PCanvas canvas, PVCFrame frame)
		{
			this.canvas = canvas;
			this.frame = frame;
		}

		protected override bool ShouldStartDragInteraction(PInputEventArgs e)
		{
			if(e.PickedNode is Mullion)
				return base.ShouldStartDragInteraction(e);
			
			return false;
		}

		protected override void OnStartDrag(object sender, PInputEventArgs e)
		{
			base.OnStartDrag(sender, e);

			mullion = e.PickedNode as Mullion;
		}

		protected override void OnDrag(object sender, PInputEventArgs e)
		{
			base.OnDrag(sender, e);

			SizeF delta = e.Delta;

			if(mullion.Model.Orientation == Domain.Entities.Orientation.Vertical)
			{
				mullion.Model.MiddlePoint += (int)delta.Width;
			}
			else
			{
				mullion.Model.MiddlePoint += (int)delta.Height;
			}
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMD.HCIL.Piccolo.Event;
using UrcieSln.Domain.Entities;

namespace UrcieSln.Drawer.PvcItems
{
	public class SashCreationEventHandler : PBasicInputEventHandler
	{
		private Surface surface;
		public SashCreationEventHandler(Surface surface)
		{
			this.surface = surface;
		}

		public SashType SashType { get; set; }

		public string Direction { get; set; }

		public override void OnClick(object sender, PInputEventArgs e)
		{
			if (e.Button != System.Windows.Forms.MouseButtons.Left) return;
			base.OnClick(sender, e);
			e.Handled = true;
			if (e.PickedNode is Surface && e.PickedNode.ChildrenCount == 0
			&& SashType != null && !(e.PickedNode.Parent is Sash))
			{
				try
				{
					Surface sashParent = (Surface)e.PickedNode;
					Sash sash = new Sash(sashParent, SashType);
					sash.Direction = Direction;
					sashParent.Frame.AddSash(sash);
				}
				catch (ArgumentException ex)
				{
					Console.WriteLine(ex.Message);
				}
			}
		}
	}
}

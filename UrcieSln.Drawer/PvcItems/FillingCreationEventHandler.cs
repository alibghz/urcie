using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMD.HCIL.Piccolo.Event;
using UrcieSln.Domain.Entities;

namespace UrcieSln.Drawer.PvcItems
{
	public class FillingCreationEventHandler : PBasicInputEventHandler
	{
		private Surface surface;

		public FillingCreationEventHandler(Surface surface)
		{
			this.surface = surface;
		}

		public FillingType FillingType { get; set; }
        public ProfileType ProfileType { get; set; }

		public override void OnClick(object sender, PInputEventArgs e)
		{
			if (e.Button != System.Windows.Forms.MouseButtons.Left) return;
			e.Handled = true;

			base.OnClick(sender, e);

			if (e.PickedNode is Surface && e.PickedNode.ChildrenCount == 0 && FillingType != null)
			{
				Surface fillingParent = (Surface)e.PickedNode;
                Filling filling = new Filling(fillingParent, FillingType, ProfileType);
				fillingParent.Frame.AddFilling(filling);
			}
		}
	}
}

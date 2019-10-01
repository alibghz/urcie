using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UMD.HCIL.Piccolo;
using UMD.HCIL.Piccolo.Event;

namespace UrcieSln.Drawer.PvcItems
{
	public class SelectionEventHandler : PBasicInputEventHandler
	{
		public event PPropertyEventHandler SelectionChanged;
		private PVCFrame frame;
		private PNode selectedNode;

		public SelectionEventHandler(PVCFrame frame) : base()
		{
			this.frame = frame;
		}
		
		public PNode SelectedNode
		{
			get { return selectedNode; }
			set
			{
				if (selectedNode != value)
				{
					var oldValue = selectedNode;
					selectedNode = value;
					OnSelectionChanged(oldValue);
				}
			}
		}
		
		//public override void OnClick(object sender, PInputEventArgs e)
		//{
		//	base.OnClick(sender, e);
		//	if (e.PickedNode == frame || e.PickedNode.IsDescendentOf(frame))
		//	{
		//		SelectedNode = e.PickedNode;
		//	}
		//}
		public override void OnMouseDown(object sender, PInputEventArgs e)
		{
			base.OnMouseDown(sender, e);
			if (e.PickedNode == frame || e.PickedNode.IsDescendentOf(frame))
			{
				SelectedNode = e.PickedNode;
			}
		}
		public void OnSelectionChanged(PNode oldValue)
		{
			PPropertyEventHandler handler = SelectionChanged;
			if (handler != null)
			{
				handler(this, new PPropertyEventArgs(oldValue, selectedNode));
			}
		}
	}
}

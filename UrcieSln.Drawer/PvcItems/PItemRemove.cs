using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMD.HCIL.Piccolo;
using UMD.HCIL.Piccolo.Util;

namespace UrcieSln.Drawer.PvcItems
{
	public class PItemRemove
	{
		public static void RemoveShiftRightMullion(Mullion mullion)
		{
			if (mullion == null || mullion.NextSurface() == null)
				throw new ArgumentException("Invalid argument provided.");

			if (mullion.Parent.ChildrenCount == 3)
			{
				mullion.Parent.RemoveAllChildren();
				return;
			}
			Surface previousSurface = mullion.PreviousSurface();
			Surface nextSurface = mullion.NextSurface();

			float x = previousSurface.Model.X;
			float y = previousSurface.Model.Y;
			float width = previousSurface.Model.Width;
			float height = previousSurface.Model.Height;

			if (mullion.Model.Orientation == Domain.Entities.Orientation.Vertical)
			{
				width += nextSurface.Model.X + nextSurface.Model.Width - mullion.Model.X;
			}
			else if (mullion.Model.Orientation == Domain.Entities.Orientation.Horizontal)
			{
				height += nextSurface.Model.Y + nextSurface.Model.Height - mullion.Model.Y;
			}
			PNodeList nodeList = new PNodeList();
			nodeList.Add(mullion);
			nodeList.Add(nextSurface);
			mullion.Parent.RemoveChildren(nodeList);

			previousSurface.Model.Bounds = new System.Drawing.RectangleF(x, y, width, height);

			mullion.Frame.RemoveMullion(mullion);
		}

		public static void RemoveShiftLeftMullion(Mullion mullion)
		{
			if (mullion == null || mullion.PreviousSurface() == null)
				throw new ArgumentException("Invalid argument provided.");

			if (mullion.Parent.ChildrenCount == 3)
			{
				mullion.Parent.RemoveAllChildren();
				return;
			}
			Surface previousSurface = mullion.PreviousSurface();
			Surface nextSurface = mullion.NextSurface();

			float x = nextSurface.Model.X;
			float y = nextSurface.Model.Y;
			float width = nextSurface.Model.Width;
			float height = nextSurface.Model.Height;

			if (mullion.Model.Orientation == Domain.Entities.Orientation.Vertical)
			{
				x = previousSurface.Model.X;
			}
			else if (mullion.Model.Orientation == Domain.Entities.Orientation.Horizontal)
			{
				y = previousSurface.Model.Y;
			}
			PNodeList nodeList = new PNodeList();
			nodeList.Add(mullion);
			nodeList.Add(previousSurface);
			mullion.Parent.RemoveChildren(nodeList);

			nextSurface.Model.Bounds = new System.Drawing.RectangleF(x, y, width, height);
			nextSurface.ParentBoundsChanged();
			Mullion nextMullion = nextSurface.NextMullion();
			Mullion prevMullion = nextSurface.PreviousMullion();
			if (nextMullion != null)
			{
				nextMullion.ParentBoundsChanged();
			}
			else if (prevMullion != null)
			{
				prevMullion.ParentBoundsChanged();
			}
		}

		public static void RemoveItem(PNode item)
		{
			if (item == null || item.Parent == null)
				throw new ArgumentNullException("Null item provided.");


			item.Parent.RemoveChild(item.Parent.IndexOfChild(item));

			if (item is Sash)
			{
				Sash sash = (Sash)item;
				sash.SurfaceParent().Frame.RemoveSash(sash);
			}
			else if (item is Filling)
			{
				Filling filling = (Filling)item;
				filling.SurfaceParent().Frame.RemoveFilling(filling);
			}
		}
	}
}

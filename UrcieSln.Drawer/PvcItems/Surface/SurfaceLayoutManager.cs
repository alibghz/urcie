using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMD.HCIL.Piccolo;
using UrcieSln.Domain.Entities;

namespace UrcieSln.Drawer.PvcItems
{
	public class SurfaceLayoutManager
	{
		public static Surface AddChild(Surface parent)
		{
			if (parent.Frame == null)
				throw new ArgumentException("Illegal surface provided.");

			Surface child = new Surface();
			child.Frame = parent.Frame;

			if (parent.Layout == SurfaceLayout.HORIZONTAL)
				child.Layout = SurfaceLayout.VERTICAL;
			else
				child.Layout = SurfaceLayout.HORIZONTAL;

			child.Model.Bounds = new RectangleF(parent.Model.X, parent.Model.Y, parent.Model.Width, parent.Model.Height);
			parent.AddChild(child);

			child.Model.SurfaceParent = parent;

			return child;
		}

		public static Surface AddChild(Surface parent, PointF position, SurfaceLayout layout, MullionType mullionType)
		{
			if (parent.Frame == null)
				throw new ArgumentException("Illegal surface provided.");

			if (parent.ChildrenCount == 0)
			{
				parent.Layout = layout;
				return Divide(AddChild(parent), position, mullionType);
			}
			Surface newChild = null;

			foreach (PNode child in parent.ChildrenReference)
			{
				if (child is Mullion) continue;

				if (child.Bounds.Contains(position))
				{
					if (child is Surface)
					{
						Surface surfChild = (Surface)child;
						if (surfChild.ChildrenCount == 0)
						{
							if (layout == parent.Layout)
								newChild = Divide(surfChild, position, mullionType);
							else
								newChild = Divide(AddChild(surfChild), position, mullionType);
						}
						else
						{
							newChild = AddChild(surfChild, position, layout, mullionType);
						}
					}
					else if (child is Sash)
					{
						Sash sashChild = (Sash)child;
						newChild = AddChild(sashChild.Surface, position, layout, mullionType);
					}
					break;
				}
			}
			return newChild;
		}

		private static Surface Divide(Surface surface, PointF position, MullionType mullionType)
		{
			if (surface.Frame == null)
				throw new ArgumentException("Illegal surface provided.");

			Surface parent = (Surface)surface.Parent;
			if (!surface.Bounds.Contains(position)) return null;


			Surface next = new Surface(surface.Layout);
			Mullion mullion;
			RectangleF parentBounds = parent.Model.Bounds;
			RectangleF nextBounds = next.Bounds;
			RectangleF surfaceBounds = surface.Model.Bounds;

			if (parent.Layout == SurfaceLayout.VERTICAL)
			{
				mullion = new Mullion(Orientation.Horizontal, mullionType);
				mullion.Frame = surface.Frame;
				mullion.Model.MiddlePoint = position.Y - surface.Frame.Y;
				mullion.Model.X = surfaceBounds.X;
				mullion.Model.Length = surface.Width;

				nextBounds.X = surfaceBounds.X;
				nextBounds.Y = mullion.Model.Y + mullion.Thickness;
				nextBounds.Width = surfaceBounds.Width;
				nextBounds.Height = surfaceBounds.Height + surfaceBounds.Y - mullion.Model.Y - mullion.Thickness;

				surfaceBounds.Height = mullion.Model.Y - surfaceBounds.Y;
			}
			else if (parent.Layout == SurfaceLayout.HORIZONTAL)
			{
				mullion = new Mullion(Orientation.Vertical, mullionType);
				mullion.Frame = surface.Frame;
				mullion.Model.MiddlePoint = position.X - surface.Frame.X;
				mullion.Model.Y = surfaceBounds.Y;
				mullion.Model.Length = surfaceBounds.Height;

				nextBounds.X = mullion.Model.X + mullion.Thickness;
				nextBounds.Y = surfaceBounds.Y;
				nextBounds.Width = surfaceBounds.Width + surfaceBounds.X - mullion.Model.X - mullion.Thickness;
				nextBounds.Height = surfaceBounds.Height;

				surfaceBounds.Width = mullion.Model.X - surfaceBounds.X;
			}
			else
				throw new ArgumentException("Illegal surface layout value.");


			next.Model.SurfaceParent = parent;
			next.Frame = surface.Frame;
			next.Model.Bounds = nextBounds;

			int index = surface.Index();
			parent.AddChild(index + 1, mullion);
			parent.AddChild(index + 2, next);

			mullion.Frame.AddMullion(mullion);

			// Because after setting bounds the surface will update herself with
			// her next mullion, So this setting MUST happen after adding mullion
			// and the next surface
			if (parent.Layout == SurfaceLayout.VERTICAL)
				surface.Model.Height = mullion.Model.Y - surface.Model.Y;
			else
				surface.Model.Width = mullion.Model.X - surface.Model.X;

			return next;
		}
	}
}

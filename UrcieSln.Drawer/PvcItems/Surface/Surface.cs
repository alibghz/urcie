using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using UMD.HCIL.Piccolo;
using UMD.HCIL.Piccolo.Event;
using UMD.HCIL.Piccolo.Util;

namespace UrcieSln.Drawer.PvcItems
{
	[Serializable()]
	public enum SurfaceLayout
	{
		VERTICAL, HORIZONTAL
	}

	[Serializable()]
	public class Surface : PNode, ISerializable
	{
		public List<Mullion> MullionChildrenReference = new List<Mullion>();
		public List<Surface> SurfaceChildrenReference = new List<Surface>();
		public List<Sash> SashChildrenReference = new List<Sash>();

		public Surface() : this(SurfaceLayout.HORIZONTAL) { }

		public Surface(SurfaceLayout layout) : base()
		{
			Layout = layout;
			Model = new SurfaceModel(this);
			Model.PropertyChanged += Model_PropertyChanged;
			BoundsChanged += Surface_BoundsChanged;
		}

		public Surface(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			BoundsChanged += Surface_BoundsChanged;
		}

		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
		}

		void Surface_BoundsChanged(object sender, PPropertyEventArgs e)
		{
			Mullion next = NextMullion();
			Mullion previous = PreviousMullion();

			if (next != null)
			{
				if (next.Model.Orientation == Domain.Entities.Orientation.Vertical)
				{
					if (Model.Width + Model.X != next.Model.X) Model.Width += next.Model.X - (Model.X + Model.Width);
				}
				else if (next.Model.Orientation == Domain.Entities.Orientation.Horizontal)
				{
					if (Model.Height + Model.Y != next.Model.Y) Model.Height += next.Model.Y - (Model.Y + Model.Height);
				}
			}
			else if (previous != null)
			{
				Surface parent = (Surface)Parent;
				if (previous.Model.Orientation == Domain.Entities.Orientation.Vertical)
				{
					if (Model.Width + Model.X != (parent.Model.Width + parent.Model.X))
					{
						Model.Width += parent.Model.Width + parent.Model.X - (Model.Width + Model.X);
					}
				}
				else if (previous.Model.Orientation == Domain.Entities.Orientation.Horizontal)
				{
					if (Model.Height + Model.Y != (parent.Model.Y + parent.Model.Height))
					{
						Model.Height += parent.Model.Height + parent.Model.Y - (Model.Height + Model.Y);
					}
				}
			}
		}

		public PVCFrame Frame { get; set; }

		public SurfaceModel Model { get; set; }

		public void Model_PropertyChanged(object sender, PVCModelPropertyChangedEventArgs e)
		{
			switch (e.PropertyCode)
			{
				case SurfaceModel.X_PROPERTY_CODE:
				case SurfaceModel.WIDTH_PROPERTY_CODE:
				case SurfaceModel.HEIGHT_PROPERTY_CODE:
				case SurfaceModel.Y_PROPERTY_CODE:
				case SurfaceModel.BOUNDS_PROPERTY_CODE:
					if (Parent == null)
					{
						Bounds = Model.Bounds;
					}
					else if (!SetBounds(Model.X, Model.Y, Model.Width, Model.Height))
					{
						Model.Bounds = Bounds;
					}
					break;
				default:
					break;
			}
		}

		public float MaxX()
		{
			float MAX = Model.X + Model.Width - 1;

			if (ChildrenCount == 1 && GetChild(0) is Sash)
			{
				return ((Sash)GetChild(0)).MaxX();
			}

			Surface firstChild;

			if (ChildrenCount == 0)
				return MAX;
			else if ((firstChild = FirstChild()) != null)
			{
				if (Layout == SurfaceLayout.HORIZONTAL)
					return firstChild.MaxX();
				else
				{
					float maxX;
					foreach (Surface child in SurfaceChildrenReference)
						if ((maxX = child.MaxX()) < MAX) { MAX = maxX; }
				}

			}
			return MAX;
		}

		public float MaxY()
		{
			if (ChildrenCount == 1 && GetChild(0) is Sash)
			{
				return ((Sash)GetChild(0)).MaxY();
			}

			float MAX = Model.Y + Model.Height - 1;
			Surface firstChild;

			if (ChildrenCount == 0)
				return MAX;
			else if ((firstChild = FirstChild()) != null)
			{
				if (Layout == SurfaceLayout.VERTICAL)
					return firstChild.MaxY();
				else
				{
					float maxY;
					foreach (Surface child in SurfaceChildrenReference)
						if ((maxY = child.MaxY()) < MAX) { MAX = maxY; }
				}
			}
			return MAX;
		}

		public float MinWidth()
		{
			float MINW = 1;
			if (ChildrenCount == 0) return MINW;

			Surface firstChild = FirstChild();

			if (ChildrenCount == 1)
			{
				if (firstChild != null)
					MINW = firstChild.MinWidth();
				else
				{
					PNode nodeChild = GetChild(0);
					if (nodeChild is Sash)
					{
						MINW = ((Sash)nodeChild).MinWidth();
					}
				}
			}
			else if (Layout == SurfaceLayout.HORIZONTAL)
			{
				foreach (Surface child in SurfaceChildrenReference)
				{
					Mullion nextMullion;
					if ((nextMullion = child.NextMullion()) != null)
						MINW += child.Model.Width + nextMullion.Thickness;
					else
						MINW += child.MinWidth();
				}
			}
			else
			{
				float minWidth;
				foreach (Surface child in SurfaceChildrenReference)
					if ((minWidth = child.MinWidth()) > MINW) MINW = minWidth;
			}


			return MINW;
		}
		public float MinHeight()
		{
			float MINH = 1;
			if (ChildrenCount == 0) return MINH;

			Surface firstChild = FirstChild();

			if (ChildrenCount == 1)
			{
				if (firstChild != null)
					MINH = firstChild.MinHeight();
				else
				{
					PNode nodeChild = GetChild(0);
					if (nodeChild is Sash)
					{
						MINH = ((Sash)nodeChild).MinHeight();
					}
				}
			}
			else if (Layout == SurfaceLayout.VERTICAL)
			{
				foreach (Surface child in SurfaceChildrenReference)
				{
					Mullion nextMullion;
					if ((nextMullion = child.NextMullion()) != null)
						MINH += child.Model.Height + nextMullion.Thickness;
					else
						MINH += child.MinHeight();
				}
			}
			else
			{
				float minHeight;
				foreach (Surface child in SurfaceChildrenReference)
					if ((minHeight = child.MinHeight()) > MINH) MINH = minHeight;
			}
			return MINH;
		}

		public float MaxWidth()
		{
			float MAX = -1;

			if (Parent == null) return MAX;
			if (Parent is PVCFrame) return ((PVCFrame)Parent).InnerBounds.Width;
			if (!(Parent is Surface)) return MAX;

			Surface parent = (Surface)Parent;

			Mullion previousMullion = PreviousMullion();
			Mullion nextMullion = NextMullion();

			if (parent.Layout == SurfaceLayout.HORIZONTAL)
			{
				if (nextMullion != null)
					return nextMullion.MaxX() - X;
				else if (previousMullion != null)
				{
					return Parent.Width + Parent.X - (previousMullion.MinX() + previousMullion.Thickness);
				}
				else
					return Parent.Width;
			}
			else
				return MAX;
		}

		public float MaxHeight()
		{
			float MAX = -1;

			if (Parent == null) return MAX;
			if (Parent is PVCFrame) return ((PVCFrame)Parent).InnerBounds.Height;
			if (!(Parent is Surface)) return MAX;

			Surface parent = (Surface)Parent;

			Mullion previousMullion = PreviousMullion();
			Mullion nextMullion = NextMullion();

			if (parent.Layout == SurfaceLayout.VERTICAL)
			{
				if (nextMullion != null)
					return nextMullion.MaxY() - Model.Y;
				else if (previousMullion != null)
				{
					return Parent.Height + Parent.Y - (previousMullion.MinY() + previousMullion.Thickness);
				}
				else
					return Parent.Height;
			}
			else
				return MAX;
		}

		public override void ParentBoundsChanged()
		{
			if (Parent is PVCFrame)
			{
				RectangleF parentInnerBounds = ((PVCFrame)Parent).InnerBounds;
				Model.X = parentInnerBounds.X;
				Model.Y = parentInnerBounds.Y;
				Model.Width = parentInnerBounds.Width;
				Model.Height = parentInnerBounds.Height;
			}
			else if (Parent is Sash)
			{
				Model.Bounds = ((Sash)Parent).InnerBounds;
			}
			else if (Parent is Surface)
			{
				Surface parent = (Surface)Parent;
				RectangleF parentBounds = parent.Model.Bounds;
				RectangleF myBounds = Model.Bounds;

				if (parent.Layout == SurfaceLayout.HORIZONTAL)
				{
					myBounds.Y = parent.Model.Y;
					myBounds.Height = parent.Model.Height;
					if (parent.FirstChild() == this)
					{
						myBounds.X = parent.Model.X;
						myBounds.Width = NextMullion().Model.X - parent.Model.X;
					}
					else if (parent.LastChild() == this)
					{
						Mullion prevMullion = PreviousMullion();
						myBounds.X = prevMullion.Model.X + prevMullion.Thickness;
						myBounds.Width = parent.Model.Width + parent.Model.X - myBounds.X;
					}
				}
				else
				{
					myBounds.X = parent.Model.X;
					myBounds.Width = parent.Model.Width;
					if (parent.FirstChild() == this)
					{
						myBounds.Y = parent.Model.Y;
						if (parent.ChildrenCount > 1)
						{
							myBounds.Height = NextMullion().Model.Y - parent.Model.Y;
						}
						else
						{
							myBounds.Height = parent.Model.Height;
						}
					}
					else if (parent.LastChild() == this)
					{
						Mullion prevMullion = PreviousMullion();
						myBounds.Y = prevMullion.Model.Y + prevMullion.Thickness;
						myBounds.Height = parent.Model.Height + parent.Model.Y - myBounds.Y;
					}
				}
				Model.Bounds = myBounds;
			}
		}

		public SurfaceLayout Layout { get; set; }

		public Mullion NextMullion()
		{
			int index = Index();
			if (++index > 0 && index < Parent.ChildrenCount) return Parent.GetChild(index) as Mullion;
			return null;
		}

		public Mullion PreviousMullion()
		{
			int index = Index();
			if (--index > 0) return Parent.GetChild(index) as Mullion;
			return null;
		}

		public Surface FirstChild()
		{
			PNode firstChild;
			return (ChildrenCount > 0 && (firstChild = GetChild(0)) is Surface) ? firstChild as Surface: null;
		}

		public Surface LastChild()
		{
			PNode lastChild;
			return (ChildrenCount > 0 && (lastChild = GetChild(ChildrenCount - 1)) is Surface) ? lastChild as Surface: null;
		}

		public int Index()
		{
			if (Parent != null)
				return Parent.IndexOfChild(this);
			return -1;
		}

		public override bool SetBounds(float x, float y, float width, float height)
		{
			if (Parent == null) return base.SetBounds(x, y, width, height);
			if (Parent.ChildrenCount > 1 && x > MaxX()) return false;
			if (Parent.ChildrenCount > 1 && y > MaxY()) return false;
			if (Parent.ChildrenCount > 1 && width < MinWidth()) return false;
			if (Parent.ChildrenCount > 1 && height < MinHeight()) return false;

			float maxWidth = (float)MaxWidth();
			float maxHeight = (float)MaxHeight();

			if (maxWidth != -1 && width > maxWidth) return false;
			if (maxHeight != -1 && height > maxHeight) return false;

			return base.SetBounds(x, y, width, height);
		}

		public override void AddChild(int index, PNode child)
		{
			AddChildReference(child);
			base.AddChild(index, child);
		}

		public override void AddChild(PNode child)
		{
			if (child is Sash)
			{
				if (ChildrenCount > 0) return;

			}
			AddChildReference(child);
			base.AddChild(child);
		}

		protected void AddChildReference(PNode child)
		{
			if (child.IsDescendentOf(this) || IsAncestorOf(child)) return;

			if (child is Surface)
				if (SurfaceChildrenReference.Contains((Surface)child)) return;
				else SurfaceChildrenReference.Add((Surface)child);
			else if (child is Mullion)
				if (MullionChildrenReference.Contains((Mullion)child)) return;
				else MullionChildrenReference.Add((Mullion)child);
			else if (child is Sash)
				if (SashChildrenReference.Contains((Sash)child)) return;
				else SashChildrenReference.Add((Sash)child);
		}

		public override void RemoveAllChildren()
		{
			base.RemoveAllChildren();

			MullionChildrenReference.Clear();
			SurfaceChildrenReference.Clear();
			SashChildrenReference.Clear();

			Frame.ValidateChildren();
		}

		public override PNode RemoveChild(int index)
		{
			PNode removedChild = base.RemoveChild(index);
			RemoveChildReference(removedChild);
			return removedChild;
		}

		public override void RemoveChildren(PNodeList childrenNodes)
		{
			base.RemoveChildren(childrenNodes);
			foreach (PNode child in childrenNodes)
			{
				RemoveChildReference(child);
			}
		}

		public override PNode RemoveChild(PNode child)
		{
			PNode removedChild = base.RemoveChild(child);
			RemoveChildReference(removedChild);
			return removedChild;
		}

		protected void RemoveChildReference(PNode child)
		{
			if (child == null) return;

			if (child is Mullion)
			{
				MullionChildrenReference.Remove((Mullion)child);
			}
			else if (child is Surface)
			{
				SurfaceChildrenReference.Remove((Surface)child);
			}
			else if (child is Sash)
			{
				SashChildrenReference.Remove((Sash)child);
			}
			Frame.ValidateChildren();
		}

		protected override void Paint(UMD.HCIL.Piccolo.Util.PPaintContext paintContext)
		{
			Graphics graphics = paintContext.Graphics;
			Brush brush = Brush;

			if (brush == null)
			{
				if (ChildrenCount == 0)
				{
					brush = new SolidBrush(Color.FromArgb(170, Color.White));
				}
				else
				{
					brush = new SolidBrush(Color.FromArgb(0, Color.White));
				}
			}
			brush = new SolidBrush(Color.FromArgb(0, Color.White));
			float x = Model.X;
			float y = Model.Y;
			float width = Model.Width;
			float height = Model.Height;

			graphics.FillRectangle(brush, x, y, width, height);

			Pen pen = new Pen(Brushes.Gray, 1);

			pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;

			if (Frame.ShowAxis)
			{
				pen.Brush = Brushes.SkyBlue;
				pen.Width = 1;
				pen.DashOffset = 30;
				graphics.DrawLine(pen, x, y, x + width, y + height);
				graphics.DrawLine(pen, x, y + height, x + width, y);
			}
		}
	}
}

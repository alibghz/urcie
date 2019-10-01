using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using UMD.HCIL.Piccolo;
using UrcieSln.Domain.Entities;
using UrcieSln.Drawer.Properties;

namespace UrcieSln.Drawer.PvcItems
{
	[Serializable()]
	public class Filling : PNode, ISerializable
	{
		public Filling(Surface surfaceParent, FillingType fillingType, ProfileType profileType)
			: base()
		{
			if (surfaceParent.ChildrenCount > 0)
				throw new ArgumentException("Invalid surface parent provided.");

			Model = new FillingModel();
			Model.PropertyChanged += Model_PropertyChanged;
			Model.SurfaceParent = surfaceParent;
			Model.FillingType = fillingType;
            Model.ProfileType = profileType;
			BoundsChanged += Filling_BoundsChanged;
		}

		public Filling(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{ }

		public Surface SurfaceParent()
		{
			return Model.SurfaceParent;
		}

		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
		}

		void Filling_BoundsChanged(object sender, UMD.HCIL.Piccolo.Event.PPropertyEventArgs e)
		{
			// Just to ensure that this never gonna get different bounds other than
			// the parent's ones
			if (Model.SurfaceParent != null) Bounds = Model.SurfaceParent.Model.Bounds;
		}

		public override void ParentBoundsChanged()
		{
			Bounds = Model.SurfaceParent.Model.Bounds;
		}

		public void Model_PropertyChanged(object sender, PVCModelPropertyChangedEventArgs e)
		{
			switch (e.PropertyCode)
			{
				case FillingModel.PARENT_PROPERTY_CODE:
					if (Model.SurfaceParent != null && Model.SurfaceParent.ChildrenCount == 0)
					{
						Model.SurfaceParent.AddChild(this);
						Bounds = Model.SurfaceParent.Model.Bounds;
					}
					else
						Model.SurfaceParent = null;
					break;
				case FillingModel.FILLING_TYPE_PROPERTY_CODE:
					break;
				case FillingModel.CODE_PROPERTY_CODE:
					break;
			}
			Repaint();
		}

		public FillingModel Model { get; set; }

		protected override void Paint(UMD.HCIL.Piccolo.Util.PPaintContext paintContext)
		{
			Graphics graphics = paintContext.Graphics;

			if (!Model.FillingType.Glass)
			{
				graphics.FillRectangle(Brushes.White, Bounds);
				object image = Resources.ResourceManager.GetObject("board");
				using (TextureBrush brush = new TextureBrush((Image)image, WrapMode.Tile))
				{
					graphics.FillRectangle(brush, Bounds);
				}
			}
			else
			{
				graphics.FillRectangle(new SolidBrush(Color.FromArgb(100, Color.SkyBlue)), Bounds);
			}

			if (Model.Code != null && Model.SurfaceParent.Frame.ShowCodes)
			{
				Font font = new Font("Arial", Model.SurfaceParent.Frame.FontSize, FontStyle.Regular);
				graphics.DrawString(Model.Code, font, Brushes.Black,
					X + Width / 2 - (Model.Code.Length * font.Size) / 2, Y + (Height / 2) - (font.Height / 2));
			}
		}
	}
}

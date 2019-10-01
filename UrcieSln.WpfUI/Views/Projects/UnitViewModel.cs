using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using UrcieSln.Domain.Entities;
using UrcieSln.WpfUI.Common;
using UrcieSln.WpfUI.Extensions;
using UrcieSln.WpfUI.Views.Materials;

namespace UrcieSln.WpfUI.Views
{
	public class UnitViewModel : BaseEntityViewModel
	{
		private bool modified = false;
		private int quantity;
		private string code;
		private float width;
		private float height;
		private byte[] frame;
		private object image; // = new byte[0];
		private string description;
		private string versionInfo;
		private ObservableCollection<AccessoryViewModel> accessories = new ObservableCollection<AccessoryViewModel>();

		public UnitViewModel(Unit unit)
		{
			Original = unit;
			Restore();
		}
		public override void Restore()
		{
			Unit original = Original as Unit;
			if (Original == null)
				throw new InvalidOperationException(
				"View model does not have an original value.");

			if (original.Id == Guid.Empty) Id = Guid.NewGuid();
			else Id = original.Id;

			Quantity = original.Quantity;
			Code = original.Code;
			Width = original.Width;
			Height = original.Height;
			Frame = original.Frame;
			Accessories = new ObservableCollection<AccessoryViewModel>(original.Accessories.ToViewModels());
			Image = original.Image;
			Description = original.Description;
			VersionInfo = original.VersionInfo;
		}

		public int Quantity
		{
			get { return quantity; }
			set
			{
				if(quantity != value)
				{
					quantity = value;
					OnPropertyChanged("Quantity");
				}
			}
		}
		public string Code
		{
			get { return code; }
			set
			{
				if(code != value)
				{
					code = value;
					OnPropertyChanged("Code");
				}
			}
		}
		public float Width
		{
			get { return width; }
			set
			{
				if(width != value)
				{
					width = value;
					OnPropertyChanged("Width");
				}
			}
		}
		public float Height
		{
			get { return height; }
			set
			{
				if(height != value)
				{
					height = value;
					OnPropertyChanged("Height");
				}
			}
		}
		public byte[] Frame
		{
			get { return frame; }
			set
			{
				//if(frame != value)
				//{
					frame = value;
					OnPropertyChanged("Frame");
				//}
			}
		}
		public object Image
		{
			get { return image; }
			set
			{
				if(image != value)
				{
					image = value;
					OnPropertyChanged("Image");
				}
			}
		}
		public virtual string Description
		{
			get { return description; }
			set
			{
				if(description != value)
				{
					description = value;
					OnPropertyChanged("Description");
				}
			}
		}
		public string VersionInfo
		{
			get { return versionInfo; }
			set
			{
				if(versionInfo != value)
				{
					versionInfo = value;
					OnPropertyChanged("VersionInfo");
				}
			}
		}
		public ObservableCollection<AccessoryViewModel> Accessories
		{
			get { return accessories; }
			set
			{
				if(accessories != value)
				{
					accessories = value;
					OnPropertyChanged("Accessories");
					if(accessories != null)
					{
						accessories.CollectionChanged += accessories_CollectionChanged;
					}
				}
			}
		}

		void accessories_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			Modified = true;
		}
		public bool Modified
		{
			get { return modified; }
			set
			{
				if(modified != value)
				{
					modified = value;
					OnPropertyChanged("Modified");
				}
			}
		}
		public override string ToString()
		{
			return Code;
		}
		public override bool Equals(object obj)
		{
			UnitViewModel u = obj as UnitViewModel;
			if (u == null) return false;
			if (u.Id != Id) return false;
			if (u.Quantity != Quantity) return false;
			if (u.Code != Code) return false;
			if (u.Width != Width) return false;
			if (u.Height != Height) return false;
			if (u.Frame != Frame) return false;
			if (u.Image != Image) return false;
			if (u.Modified != Modified) return false;
			if (u.Description != Description) return false;
			if (u.VersionInfo != VersionInfo) return false;
			return true;
		}

		protected override void OnCloseRequested(CloseEventHandlerArgs args)
		{
			if (!Modified)
			{
				base.OnCloseRequested(args);
				return;
			}

			MessageBoxResult result = System.Windows.MessageBox.Show(
				"Do you want to save changes before closing?", "Confirm",
				MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
			if (result == MessageBoxResult.Yes)
			{
				Modified = true;
			}
			else if (result == MessageBoxResult.No)
			{
				Restore();
				Modified = false;
			}
			else if (result == MessageBoxResult.Cancel)
			{
				return;
			}
			base.OnCloseRequested(args);
		}
	}
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrcieSln.Domain.Entities;
using UrcieSln.WpfUI.Common;

namespace UrcieSln.WpfUI.Views.Materials
{
	public class AccessoryViewModel : BaseEntityViewModel
	{
		private AccessoryTypeViewModel accessoryType;
		private string code;
		private float quantity;

		public AccessoryViewModel() { }

		public AccessoryViewModel(Accessory accessory)
		{
			Original = accessory;
			Restore();
		}

		public override void Restore()
		{
			Accessory original = Original as Accessory;
			if(original == null)
				throw new InvalidOperationException(
				"View model does not have an original value.");

			if (original.Id == Guid.Empty) Id = Guid.NewGuid();
			else Id = original.Id;

			Code = original.Code;
			AccessoryType = new AccessoryTypeViewModel(original.AccessoryType);
			Quantity = original.Quantity;
		}

		public AccessoryTypeViewModel AccessoryType
		{
			get { return accessoryType; }
			set
			{
				if (accessoryType == null || !accessoryType.Equals(value))
				{
					accessoryType = value;
					OnPropertyChanged("AccessoryType");
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
		public float Quantity
		{
			get { return quantity; }
			set
			{
				if (quantity != value)
				{
					quantity = value;
					OnPropertyChanged("Quantity");
				}
			}
		}

		public override string ToString()
		{
			if (AccessoryType != null) return AccessoryType.ToString();
			return base.ToString();
		}
		public override bool Equals(object obj)
		{
			AccessoryViewModel avm = obj as AccessoryViewModel;
			if (avm == null) return false;
			if (avm.Id != Id) return false;
			if (avm.Code != Code) return false;
			if (avm.AccessoryType == null || !avm.AccessoryType.Equals(AccessoryType)) return false;
			return true;
		}
	}
}

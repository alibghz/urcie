using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace UrcieSln.Domain.Entities
{
	[Serializable()]
	public class ProfileType : ISerializable, Entity
	{
		public ProfileType() {}
		public ProfileType(SerializationInfo info, StreamingContext context)
		{
			Id = (Guid)info.GetValue("Id", typeof(Guid));
			Name = (string)info.GetValue("Name", typeof(string));
			Thickness = (float)info.GetValue("Thickness", typeof(float));
			Shape = (ProfileShape)info.GetValue("Shape", typeof(ProfileShape));
			Length = (float)info.GetValue("Length", typeof(float));
			Weight = (float)info.GetValue("Weight", typeof(float));
			Price = (Price)info.GetValue("Price", typeof(Price));
			Tolerance = (float)info.GetValue("Tolerance", typeof(float));
			Description = (string)info.GetValue("Description", typeof(string));
		}
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("Id", Id);
			info.AddValue("Name", Name);
			info.AddValue("Thickness", Thickness);
			info.AddValue("Shape", Shape);
			info.AddValue("Length", Length);
			info.AddValue("Weight", Weight);
			info.AddValue("Price", Price);
			info.AddValue("Tolerance", Tolerance);
			info.AddValue("Description", Description);
		}

		public Guid Id { get; set; }
		public virtual string Name { get; set; }
		public float Thickness { get; set; }
		public virtual ProfileShape Shape { get; set; }
		public float Length { get; set; }
		public float Weight { get; set; }
		public virtual Price Price { get; set; }
		public float Tolerance { get; set; }
		public virtual string Description { get; set; }

		public override string ToString()
		{
			return Name;
		}

		#region Others
		public float WeightPerLength { get { return (Length > 0 && Weight > 0) ? Weight / Length : 0; } }
		public Price PricePerLength { get { return (Length > 0 && Price.Sale > 0) ? Price / Length : new Price { Currency = Price.Currency }; } }
		public Price PricePerWeight { get { return (Weight > 0 && Price.Sale > 0) ? Price / Weight : new Price { Currency = Price.Currency }; } }
		#endregion

		public override bool Equals(object obj)
		{
			ProfileType p = obj as ProfileType;
			if (p == null) return false;

			if (p.Id != Id) return false;
			if (p.Name != Name) return false;
			if (p.Thickness != Thickness) return false;
			if (p.Shape != Shape) return false;
			if (p.Length != Length) return false;
			if (p.Weight != Weight) return false;
			if (p.Price != null && !Price.Equals(p.Price)) return false;
			if (p.Tolerance != Tolerance) return false;
			if (p.Description != Description) return false;

			return true;
		}
	}
}

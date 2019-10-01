using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace UrcieSln.Domain.Entities
{
	[Serializable()]
	public class MuntinType : ISerializable, Entity
	{
		public MuntinType(SerializationInfo info, StreamingContext context)
		{
			Id = (Guid)info.GetValue("Id", typeof(Guid));
			Name = (string)info.GetValue("Name", typeof(string));
			Width = (float)info.GetValue("Width", typeof(float));
			Height = (float)info.GetValue("Height", typeof(float));
			Weight = (float)info.GetValue("Weight", typeof(float));
			Price = (Price)info.GetValue("Price", typeof(Price));
			Tolerance = (float)info.GetValue("Tolerance", typeof(float));
			Description = (string)info.GetValue("Description", typeof(string));
		}

		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("Id", Id);
			info.AddValue("Name", Name);
			info.AddValue("Width", Width);
			info.AddValue("Height", Height);
			info.AddValue("Weight", Weight);
			info.AddValue("Price", Price);
			info.AddValue("Tolerance", Tolerance);
			info.AddValue("Description", Description);
		}

		public MuntinType() { }

		public Guid Id { get; set; }
		public virtual string Name { get; set; }
		public float Width { get; set; }
		public float Height { get; set; }
		public float Weight { get; set; }
		public virtual Price Price { get; set; }
		public float Tolerance { get; set; }
		public virtual string Description { get; set; }
		public float Square { get { return Width * Height; } }
		public float WeightPerSquare { get { return (Square > 0 && Weight > 0) ? Weight / Square : 0; } }
		public virtual Price PricePerSquare { get { return (Square > 0 && Price.Sale > 0) ? Price / Square : new Price { Currency = Price.Currency }; } }
		public virtual Price PricePerWeight { get { return (Weight > 0 && Price.Sale > 0) ? Price / Weight : new Price { Currency = Price.Currency }; } }
		public override string ToString()
		{
			return Name;
		}
		public override bool Equals(object obj)
		{
			MuntinType m = obj as MuntinType;
			if (m == null) return false;
			if (m.Id != Id) return false;
			if (m.Name != Name) return false;
			if (m.Width != Width) return false;
			if (m.Height != Height) return false;
			if (m.Price != null && !Price.Equals(m.Price)) return false;
			if (m.Tolerance != Tolerance) return false;
			if (m.Description != Description) return false;
			return true;
		}
	}
}
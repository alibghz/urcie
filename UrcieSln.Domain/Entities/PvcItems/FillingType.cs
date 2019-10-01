using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace UrcieSln.Domain.Entities
{
	[Serializable()]
	public class FillingType : ISerializable, Entity
	{
		public FillingType() { }

		public FillingType(SerializationInfo info, StreamingContext context)
		{
			Id = (Guid)info.GetValue("Id", typeof(Guid));
			Name = (string)info.GetValue("Name", typeof(string));
			Glass = (bool)info.GetValue("Glass", typeof(bool));
			Width = (float)info.GetValue("Width", typeof(float));
			Height = (float)info.GetValue("Height", typeof(float));
			Weight = (float)info.GetValue("Weight", typeof(float));
			Tolerance = (float)info.GetValue("Tolerance", typeof(float));
			Description = (string)info.GetValue("Description", typeof(string));
			Price = (Price)info.GetValue("Price", typeof(Price));
		}

		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("Id", Id);
			info.AddValue("Name", Name);
			info.AddValue("Glass", Glass);
			info.AddValue("Width", Width);
			info.AddValue("Height", Height);
			info.AddValue("Weight", Weight);
			info.AddValue("Tolerance", Tolerance);
			info.AddValue("Description", Description);
			info.AddValue("Price", Price);
		}

		public Guid Id
		{
			get;
			set;
		}
		public bool Glass { get; set; }
		public virtual string Name { get; set; }
		public float Width { get; set; }
		public float Height { get; set; }
		public float Weight { get; set; }
		public virtual Price Price { get; set; }
		public float Tolerance { get; set; }
		public virtual string Description { get; set; }
		public float Square { get { return (Width > 0 && Height > 0) ? Width * Height : 0; } }
		public float WeightPerSquare { get { return (Square > 0 && Weight > 0) ? Weight / Square : 0; } }
		public virtual Price PricePerWeight { get { return (Price.Sale > 0 && Weight > 0) ? Price / Weight : new Price { Currency = Price.Currency }; } }
		public virtual Price PricePerSquare { get { return (Square > 0 && Price.Sale > 0) ? Price / Square : new Price { Currency = Price.Currency }; } }

		public override string ToString()
		{
			return Name;
		}

		public override bool Equals(object obj)
		{
			FillingType f = obj as FillingType;
			if (f == null) return false;

			if (f.Id != Id) return false;
			if (f.Name != Name) return false;
			if (f.Width != Width) return false;
			if (f.Height != Height) return false;
			if (f.Glass != Glass) return false;
			if (f.Price != null && !Price.Equals(f.Price)) return false;
			if (f.Tolerance != Tolerance) return false;
			if (f.Description != Description) return false;

			return true;
		}
	}
}

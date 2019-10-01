using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace UrcieSln.Domain.Entities
{
	[Serializable()]
	public class AccessoryType : Entity, ISerializable 
	{
		public AccessoryType() { }
		public AccessoryType(SerializationInfo info, StreamingContext context)
		{
			Id = (Guid)info.GetValue("Id", typeof(Guid));
			Name = (string)info.GetValue("Name", typeof(string));
			Category = (string)info.GetValue("Category", typeof(string));
			Price = (Price)info.GetValue("Price", typeof(Price));
			Description = (string)info.GetValue("Description", typeof(string));
		}
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("Id", Id);
			info.AddValue("Name", Name);
			info.AddValue("Category", Category);
			info.AddValue("Price", Price);
			info.AddValue("Description", Description);
		}

		public Guid Id { get; set; }
		public virtual string Name { get; set; }
		public string Category { get; set; }
		public virtual string Description { get; set; }
		public virtual Price Price { get; set; }
		public string Unit { get; set; }
		public override string ToString()
		{
			return Name;
		}
		public override bool Equals(object obj)
		{
			AccessoryType ac = obj as AccessoryType;
			if (ac == null) return false;

			if (ac.Id != Id) return false;
			if (ac.Name != Name) return false;
			if (ac.Category != Category) return false;
			if (ac.Description != Description) return false;
			if (ac.Price != null && !Price.Equals(ac.Price)) return false;
			if (ac.Unit != Unit) return false;

			return true;
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace UrcieSln.Domain.Entities
{
	[Serializable()]
	public class SashType : ISerializable, Entity
	{
		public SashType(SerializationInfo info, StreamingContext context)
		{
			Id = (Guid)info.GetValue("Id", typeof(Guid));
			Name = (string)info.GetValue("Name", typeof(string));
			ProfileType = (ProfileType)info.GetValue("ProfileType", typeof(ProfileType));
			Tolerance = (float)info.GetValue("Tolerance", typeof(float));
			Description = (string)info.GetValue("Description", typeof(string));
		}
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("Id", Id);
			info.AddValue("Name", Name);
			info.AddValue("ProfileType", ProfileType);
			info.AddValue("Tolerance", Tolerance);
			info.AddValue("Description", Description);
		}
		public SashType()
		{
		}

		public Guid Id { get; set; }
		public virtual string Name { get; set; }
		public virtual ProfileType ProfileType { get; set; }
		public float Tolerance { get; set; }
		public virtual string Description { get; set; }
		public virtual IList<Accessory> Accessories { get; set; }

		public override string ToString()
		{
			return Name.ToString();
		}
		public override bool Equals(object obj)
		{
			SashType s = obj as SashType;
			if (s == null) return false;

			if (s.Id != Id) return false;
			if (s.Name != Name) return false;
			if (s.ProfileType != null && !s.ProfileType.Equals(ProfileType)) return false;
			if (s.Tolerance != Tolerance) return false;
			if (s.Description != Description) return false;

			return true;
		}
	}
}

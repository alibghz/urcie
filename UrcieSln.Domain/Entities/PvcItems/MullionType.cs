using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace UrcieSln.Domain.Entities
{
	[Serializable]
	public class MullionType : ISerializable, Entity
	{
		public MullionType() { }

		public MullionType(SerializationInfo info, StreamingContext context)
		{
			Id = (Guid)info.GetValue("Id", typeof(Guid));
			Name = (string)info.GetValue("Name", typeof(string));
			ProfileType = (ProfileType)info.GetValue("ProfileType", typeof(ProfileType));
		}

		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("Id", Id);
			info.AddValue("Name", Name);
			info.AddValue("ProfileType", ProfileType);
		}

		public Guid Id { get; set; }
		public virtual string Name { get; set; }
		public virtual ProfileType ProfileType { get; set; }
		public override string ToString()
		{
			return Name;
		}

		public override bool Equals(object obj)
		{
			MullionType m = obj as MullionType;
			if (m == null) return false;

			if (m.Id != Id) return false;
			if (m.Name != Name) return false;
			if (m.ProfileType != null && !m.ProfileType.Equals(ProfileType)) return false;

			return true;
		}
	}
}

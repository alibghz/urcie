using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace UrcieSln.Domain.Entities
{
	[Serializable()]
	public class Project : Entity, ISerializable
	{
		public Project()
		{
			Units = new List<Unit>();
		}
		public Project(SerializationInfo info, StreamingContext context)
		{
			Id = (Guid)info.GetValue("Id", typeof(Guid));
			Path = (string)info.GetValue("Path", typeof(string));
			Code = (string)info.GetValue("Code", typeof(string));
			Units = (List<Unit>)info.GetValue("Units", typeof(List<Unit>));
		}
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("Id", Id);
			info.AddValue("Path", Path);
			info.AddValue("Code", Code);
			info.AddValue("Units", Units);
		}

		public Guid Id { get; set; }
		public string Path { get; set; }
		public string Code { get; set; }
		public virtual List<Unit> Units { get; set; }
		public override bool Equals(object obj)
		{
			Project p = obj as Project;
			if (p == null) return false;
			if (p.Id != Id) return false;
			if (p.Path != Path) return false;
			if (p.Code != Code) return false;
			if (p.Units != Units) return false;
			return true;
		}
		public override string ToString()
		{
			return Code;
		}
	}
}
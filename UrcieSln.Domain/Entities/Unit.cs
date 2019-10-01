using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace UrcieSln.Domain.Entities
{
	[Serializable()]
	public class Unit : Entity, ISerializable
	{
		public Unit() {
			Accessories = new List<Accessory>();
		}
		public Unit(SerializationInfo info, StreamingContext context)
		{
			Id = (Guid)info.GetValue("Id", typeof(Guid));
			Project = (Project)info.GetValue("Project", typeof(Project));
			Quantity = (int)info.GetValue("Quantity", typeof(int));
			Code = (string)info.GetValue("Code", typeof(string));
			Width = (float)info.GetValue("Width", typeof(float));
			Height = (float)info.GetValue("Height", typeof(float));
			Frame = (byte[])info.GetValue("Frame", typeof(byte[]));
			Accessories = (List<Accessory>)info.GetValue("Accessories", typeof(List<Accessory>));
			Image = (object)info.GetValue("Image", typeof(object));
			Description = (string)info.GetValue("Description", typeof(string));
			VersionInfo = (string)info.GetValue("VersionInfo", typeof(string));
		}
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("Id", Id);
			info.AddValue("Project", Project);
			info.AddValue("Quantity", Quantity);
			info.AddValue("Code", Code);
			info.AddValue("Width", Width);
			info.AddValue("Height", Height);
			info.AddValue("Frame", Frame);
			info.AddValue("Accessories", Accessories);
			info.AddValue("Image", Image);
			info.AddValue("Description", Description);
			info.AddValue("VersionInfo", VersionInfo);
		}

		public Guid Id { get; set; }
		public virtual Project Project { get; set; }
		public int Quantity { get; set; }
		public string Code { get; set; }
		public float Width { get; set; }
		public float Height { get; set; }
		public byte[] Frame { get; set; }
		public object Image { get; set; }
		public virtual string Description { get; set; }
		public float SquareMeter { get { return (Width * Height) / 1000000; } }
		public string VersionInfo { get; set; }
		public List<Accessory> Accessories { get; set; }
		public override string ToString()
		{
			return Code;
		}
		public override bool Equals(object obj)
		{
			Unit u = obj as Unit;
			if (u == null) return false;
			if (u.Id != Id) return false;
			if (u.Project == null || !u.Project.Equals(Project)) return false;
			if (u.Quantity != Quantity) return false;
			if (u.Code != Code) return false;
			if (u.Width != Width) return false;
			if (u.Height != Height) return false;
			if (u.Frame != Frame) return false;
			if (u.Image != Image) return false;
			if (u.Description != Description) return false;
			if (u.VersionInfo != VersionInfo) return false;
			return true;
		}
	}
}

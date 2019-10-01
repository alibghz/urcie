using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace UrcieSln.Domain.Entities
{
	[Serializable()]
	public class Accessory : ISerializable
	{
		public Accessory() { }

		public Accessory(SerializationInfo info, StreamingContext context)
		{
			Id = (Guid)info.GetValue("Id", typeof(Guid));
			Code = (string)info.GetValue("Code", typeof(string));
			AccessoryType = (AccessoryType)info.GetValue("AccessoryType", typeof(AccessoryType));
			Quantity = (float)info.GetValue("Quantity", typeof(float));
		}
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("Id", Id);
			info.AddValue("Code", Code);
			info.AddValue("AccessoryType", AccessoryType);
			info.AddValue("Quantity", Quantity);
		}
		public Guid Id { get; set; }
		public string Code { get; set; }
		public AccessoryType AccessoryType { get; set; }
		public float Quantity { get; set; }

		public override string ToString()
		{
			return AccessoryType.ToString() + " " + Quantity.ToString();
		}
	}
}

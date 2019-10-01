using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UrcieSln.Domain.Entities
{
	[Serializable()]
	public class Customer
	{
		public Guid Id { get; set; }
		public virtual string Name { get; set; }
		public virtual string Address { get; set; }
		public virtual string Address2 { get; set; }
		public string Email { get; set; }
		public string Email2 { get; set; }
		public string Phone { get; set; }
		public string Phone2 { get; set; }
		public virtual string Description { get; set; }
		public override string ToString()
		{
			return Name.ToString();
		}
	}
}

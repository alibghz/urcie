using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrcieSln.Domain.Entities;

namespace UrcieSln.Domain.Models
{
	public class Muntin : IMuntin
	{
		public string Code { get; set; }
		public MuntinType MuntinType { get; set; }
		public float X { get; set; }
		public float Y { get; set; }
		public float Width { get; set; }
		public float Height { get; set; }
		public override string ToString()
		{
			return Code;
		}
	}
}

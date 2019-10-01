using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrcieSln.Domain.Entities;

namespace UrcieSln.Domain.Models
{
	public class Surface : ISurface
	{
		public Surface()
		{
			Children = new List<object>();
		}
		public string Code { get; set; }
		public object Parent { get; set; }
		public Orientation Orientation { get; set; }
		public IList<object> Children { get; set; }
		public IMullion Previous { get; set; }
		public IMullion Next { get; set; }
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

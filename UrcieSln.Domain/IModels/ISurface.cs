using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrcieSln.Domain.Entities;

namespace UrcieSln.Domain
{
	public interface ISurface
	{
		string Code { get; set; }
		object Parent { get; }
		Orientation Orientation { get; set; }
		IList<object> Children { get; set; }
		IMullion Previous { get; set; }
		IMullion Next { get; set; }
		float X { get; set; }
		float Y { get; set; }
		float Width { get; set; }
		float Height { get; set; }
	}
}

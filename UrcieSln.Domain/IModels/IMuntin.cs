using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrcieSln.Domain.Entities;

namespace UrcieSln.Domain
{
	public interface IMuntin
	{
		string Code { get; set; }
		MuntinType MuntinType { get; set; }
		float X { get; set; }
		float Y { get; set; }
		float Width { get; set; }
		float Height { get; set; }
	}
}

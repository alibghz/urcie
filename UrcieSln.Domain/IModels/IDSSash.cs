using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrcieSln.Domain.Entities;

namespace UrcieSln.Domain
{
	public interface IDSash
	{
		string Code { get; set; }
		bool IsCornered { get; set; }
		SashType SashType { get; set; }
		ISurface Parent { get; }
		IList<ISurface> Children { get; }
		Orientation Orientation { get; set; }
		MullionType MullionType { get; set; }
		ISurface Previous { get; set; }
		ISurface Next { get; set; }
		float Length { get; set; }
		float MiddlePoint { get; set; }
	}
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrcieSln.Domain.Entities;

namespace UrcieSln.Domain
{
	public interface IMullion
	{
		string Code { get; set; }
		bool IsVirtual { get; set; }
		ISurface Parent { get; }
		Orientation Orientation { get; set; }
		MullionType MullionType { get; set; }
		ISurface Previous { get; set; }
		ISurface Next { get; set; }
		float Length { get; set; }
		float MiddlePoint { get; set; }
	}
}

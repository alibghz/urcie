using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrcieSln.Domain.Entities;

namespace UrcieSln.Domain
{
	public interface ISash
	{
		string Code { get; set; }
		bool IsCornered { get; set; }
		SashType SashType { get; set; }
		ISurface Parent { get; }
		ISurface Child { get; }
	}
}

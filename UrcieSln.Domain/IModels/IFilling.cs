using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrcieSln.Domain.Entities;

namespace UrcieSln.Domain
{
	public interface IFilling
	{
		string Code { get; set; }
		FillingType FillingType { get; set; }
		ProfileType ProfileType { get; set; }
		ISurface Parent { get; }
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrcieSln.Domain.Entities;

namespace UrcieSln.Domain.Models
{
	public class Filling : IFilling
	{
		public string Code { get; set; }
		public FillingType FillingType { get; set; }
		public ProfileType ProfileType { get; set; }
		public ISurface Parent { get; set; }
		public override string ToString()
		{
			return Code;
		}
	}
}

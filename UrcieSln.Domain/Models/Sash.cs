using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrcieSln.Domain.Entities;

namespace UrcieSln.Domain.Models
{
	public class Sash : ISash
	{
		public Sash()
		{
			IsCornered = true;
		}
		public string Code { get; set; }
		public bool IsCornered { get; set; }
		public SashType SashType { get; set; }
		public ISurface Parent { get; set; }
		public ISurface Child { get; set; }
		public override string ToString()
		{
			return Code;
		}
	}
}

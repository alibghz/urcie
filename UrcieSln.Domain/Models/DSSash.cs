using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrcieSln.Domain.Entities;

namespace UrcieSln.Domain.Models
{
	public class DSash : IDSash
	{
		public DSash()
		{
			IsCornered = true;
		}
		public string Code { get; set; }
		public bool IsCornered { get; set; }
		public SashType SashType { get; set; }
		public ISurface Parent { get; set; }
		public IList<ISurface> Children { get; set; }
		public Orientation Orientation { get; set; }
		public MullionType MullionType { get; set; }
		public ISurface Previous { get; set; }
		public ISurface Next { get; set; }
		public float Length { get; set; }
		public float MiddlePoint { get; set; }
		public override string ToString()
		{
			return Code;
		}
	}
}

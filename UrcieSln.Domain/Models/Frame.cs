using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrcieSln.Domain.Entities;

namespace UrcieSln.Domain.Models
{
	public class Frame : IFrame
	{
		public Frame()
		{
			Accessories = new List<Accessory>();
			Muntins = new List<IMuntin>();
			IsCornered = true;
		}
		public string Code { get; set; }
		public ProfileType ProfileType { get; set; }
		public ProfileType UProfileType { get; set; }
		public bool IsCornered { get; set; }
		public float Width { get; set; }
		public float Height { get; set; }
		public ISurface Child { get; set; }
		public IList<Accessory> Accessories { get; set; }
		public IList<IMuntin> Muntins { get; set; }
		public override string ToString()
		{
			return Code;
		}
	}
}

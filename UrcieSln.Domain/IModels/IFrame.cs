using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrcieSln.Domain.Entities;

namespace UrcieSln.Domain
{
	public interface IFrame
	{
		string Code { get; set; }
		ProfileType ProfileType { get; set; }
		ProfileType UProfileType { get; set; }
		bool IsCornered { get; set; }
		float Width { get; set; }
		float Height { get; set; }
		ISurface Child { get; }
		IList<Accessory> Accessories { get; set; }
		IList<IMuntin> Muntins { get; set; }
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrcieSln.Domain.Entities;

namespace UrcieSln.Domain
{
	public interface IProfile
	{
		string Code { get; set; }
		Orientation Orientation { get; set; }
		ProfileType ProfileType { get; set; }
		float Length { get; set; }
		float RightAngle { get; set; }
		float LeftAngle { get; set; }
	}
}

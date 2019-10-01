using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrcieSln.Domain.Entities;

namespace UrcieSln.Domain.Models
{
	public class Profile : IProfile
	{
		public string Code { get; set; }
		public Orientation Orientation { get; set; }
		public ProfileType ProfileType { get; set; }
		public float Length { get; set; }
		public float RightAngle { get; set; }
		public float LeftAngle { get; set; }
		public override string ToString()
		{
			return Code;
		}
	}
}

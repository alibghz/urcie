using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UrcieSln.Domain.Entities
{
	[Serializable()]
	public enum ProjectStatus
	{
		Started,
		Completed,
		InProgress,
		Pending,
		Suspended,
		Installing,
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UrcieSln.WpfUI.Common
{
	public class CloseEventHandlerArgs : EventArgs
	{
		public CloseEventHandlerArgs(object owner) : base()
		{
			RequestOwner = owner;
		}

		public object RequestOwner
		{
			get;
			set;
		}
	}
}
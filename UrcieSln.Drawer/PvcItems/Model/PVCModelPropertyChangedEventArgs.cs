using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UrcieSln.Drawer.PvcItems
{
	public class PVCModelPropertyChangedEventArgs : EventArgs
	{
		public PVCModelPropertyChangedEventArgs() : base() { }

		public PVCModelPropertyChangedEventArgs(int propertyCode)
			: base()
		{
			PropertyCode = propertyCode;
		}

		public int PropertyCode { get; set; }
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace UrcieSln.Drawer.PvcItems
{
	[Serializable()]
	public class PVCModel : ISerializable
	{
		public event PropertyChangedEventHandler PropertyChanged;
		public delegate void PropertyChangedEventHandler(Object sender, PVCModelPropertyChangedEventArgs e);

		public void OnPropertyChanged(PVCModelPropertyChangedEventArgs e)
		{
			PropertyChangedEventHandler handler = PropertyChanged;
			if (handler != null)
			{
				handler(this, e);
			}
		}

		public PVCModel() { }

		protected PVCModel(SerializationInfo info, StreamingContext context)
		{
			PropertyChanged = (PropertyChangedEventHandler)info.GetValue("PropertyChanged", typeof(PropertyChangedEventHandler));
		}

		public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("PropertyChanged", PropertyChanged);
		}
	}
}

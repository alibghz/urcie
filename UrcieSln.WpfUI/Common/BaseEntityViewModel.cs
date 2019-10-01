using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UrcieSln.Domain;

namespace UrcieSln.WpfUI.Common
{
	public abstract class BaseEntityViewModel : BaseViewModel
	{
		public abstract void Restore();

		private Guid id = Guid.Empty;
		public virtual Guid Id
		{
			get { return id; }
			set
			{
				if(id == Guid.Empty && value != Guid.Empty)
				{
					id = value;
					OnPropertyChanged("Id");
				}
			}
		}
		public virtual object Original
		{
			get;
			set;
		}
	}
}
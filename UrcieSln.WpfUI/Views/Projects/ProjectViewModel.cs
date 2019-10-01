using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrcieSln.Domain.Entities;
using UrcieSln.WpfUI.Common;
using UrcieSln.WpfUI.Extensions;

namespace UrcieSln.WpfUI.Views
{
	public class ProjectViewModel : BaseEntityViewModel
	{
		private string code;
		private ObservableCollection<UnitViewModel> units;
		private string path;

		public ProjectViewModel()
		{
			Id = Guid.NewGuid();
			units = new ObservableCollection<UnitViewModel>();
		}
		public ProjectViewModel(Project project)
		{
			Original = project;
			Restore();
		}
		public override void Restore()
		{
			Project original = Original as Project;
			if (original == null)
				throw new InvalidOperationException(
				"View model does not have an original value.");
			Id = original.Id;
			Path = original.Path;
			Code = original.Code;
			Units = new ObservableCollection<UnitViewModel>(original.Units.ToViewModels());
		}

		public string Path
		{
			get { return path; }
			set
			{
				if (path != value)
				{
					path = value;
					OnPropertyChanged("Path");
				}
			}
		}
		public string Code
		{
			get { return code; }
			set
			{
				if(code != value)
				{
					code = value;
					OnPropertyChanged("Code");
				}
			}
		}
		public virtual ObservableCollection<UnitViewModel> Units
		{
			get { return units; }
			set
			{
				if(units != value)
				{
					units = value;
					OnPropertyChanged("Units");
				}
			}
		}
		public override bool Equals(object obj)
		{
			ProjectViewModel p = obj as ProjectViewModel;
			if (p == null) return false;
			if (p.Id != Id) return false;
			if (p.Path != Path) return false;
			if (p.Code != Code) return false;
			return true;
		}
		public override string ToString()
		{
			return Code;
		}
	}
}

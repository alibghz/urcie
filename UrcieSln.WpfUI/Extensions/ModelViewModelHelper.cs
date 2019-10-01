using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using UMD.HCIL.Piccolo.Util;
using UrcieSln.Domain.Entities;
using UrcieSln.Drawer.PvcItems;
using UrcieSln.WpfUI.Views;
using UrcieSln.WpfUI.Views.Materials;

namespace UrcieSln.WpfUI.Extensions
{
	public static class ModelViewModelHelper
	{
		#region ProfileType

		public static List<ProfileTypeViewModel> ToViewModels(this List<ProfileType> profileTypes)
		{
			if (profileTypes == null) throw new ArgumentNullException("profileTypes");
			List<ProfileTypeViewModel> viewModels = new List<ProfileTypeViewModel>();
			profileTypes.ForEach(i => viewModels.Add(new ProfileTypeViewModel(i)));
			return viewModels;
		}

		public static List<ProfileType> ToModels(this List<ProfileTypeViewModel> profileTypeViewModels)
		{
			if (profileTypeViewModels == null) throw new ArgumentNullException("profileTypeViewModels");
			List<ProfileType> profileTypes = new List<ProfileType>();
			profileTypeViewModels.ForEach(i => profileTypes.Add(i.ToModel()));
			return profileTypes;
		}

		public static ProfileType ToModel(this ProfileTypeViewModel profileTypeViewModel)
		{
			if (profileTypeViewModel == null) throw new ArgumentNullException("profileTypeViewModel");
			ProfileType profileType = new ProfileType();
			profileType.Id = profileTypeViewModel.Id;
			profileType.Name = profileTypeViewModel.Name;
			profileType.Thickness = profileTypeViewModel.Thickness;
			profileType.Shape = profileTypeViewModel.Shape;
			profileType.Length = profileTypeViewModel.Length;
			profileType.Weight = profileTypeViewModel.Weight;
			profileType.Price = profileTypeViewModel.Price;
			profileType.Tolerance = profileTypeViewModel.Tolerance;
			profileType.Description = profileTypeViewModel.Description;
			return profileType;
		}

		#endregion

		#region MullionType

		public static List<MullionTypeViewModel> ToViewModels(this List<MullionType> mullionTypes)
		{
			if (mullionTypes == null)
				throw new ArgumentException("mullionTypes");

			List<MullionTypeViewModel> viewModels = new List<MullionTypeViewModel>();
			mullionTypes.ForEach(i => viewModels.Add(new MullionTypeViewModel(i)));
			return viewModels;
		}

		public static List<MullionType> ToModels(this List<MullionTypeViewModel> mullionTypeViewModels)
		{
			if (mullionTypeViewModels == null)
				throw new ArgumentNullException("mullionTypeViewModels");

			List<MullionType> mullionTypes = new List<MullionType>();
			mullionTypeViewModels.ForEach(i => mullionTypes.Add(i.ToModel()));
			return mullionTypes;
		}

		public static MullionType ToModel(this MullionTypeViewModel mullionTypeViewModel)
		{
			if (mullionTypeViewModel == null)
				throw new ArgumentNullException("mullionTypeViewModel");

			MullionType mullionType = new MullionType();

			mullionType.Id = mullionTypeViewModel.Id;
			mullionType.Name = mullionTypeViewModel.Name;
			mullionType.ProfileType = mullionTypeViewModel.ProfileType.ToModel();
			return mullionType;
		}

		#endregion

		#region FillingType

		public static List<FillingTypeViewModel> ToViewModels(this List<FillingType> fillingTypes)
		{
			if (fillingTypes == null) throw new ArgumentException("fillingTypes");
			List<FillingTypeViewModel> viewModels = new List<FillingTypeViewModel>();
			fillingTypes.ForEach(i => viewModels.Add(new FillingTypeViewModel(i)));
			return viewModels;
		}

		public static List<FillingType> ToModels(this List<FillingTypeViewModel> fillingTypeViewModels)
		{
			if (fillingTypeViewModels == null) throw new ArgumentNullException("fillingTypeViewModels");
			List<FillingType> fillingTypes = new List<FillingType>();
			fillingTypeViewModels.ForEach(i => fillingTypes.Add(i.ToModel()));
			return fillingTypes;
		}

		public static FillingType ToModel(this FillingTypeViewModel fillingTypeViewModel)
		{
			if (fillingTypeViewModel == null) throw new ArgumentNullException("fillingTypeViewModel");
			FillingType fillingType = new FillingType();
			fillingType.Id = fillingTypeViewModel.Id;
			fillingType.Glass = fillingTypeViewModel.Glass;
			fillingType.Name = fillingTypeViewModel.Name;
			fillingType.Description = fillingTypeViewModel.Description;
			fillingType.Width = fillingTypeViewModel.Width;
			fillingType.Height = fillingTypeViewModel.Height;
			fillingType.Weight = fillingTypeViewModel.Weight;
			fillingType.Tolerance = fillingTypeViewModel.Tolerance;
			fillingType.Price = fillingTypeViewModel.Price;
			return fillingType;
		}

		#endregion

		#region SashType

		public static List<SashTypeViewModel> ToViewModels(this List<SashType> sashTypes)
		{
			if (sashTypes == null) throw new ArgumentException("sashTypes");
			List<SashTypeViewModel> viewModels = new List<SashTypeViewModel>();
			sashTypes.ForEach(i => viewModels.Add(new SashTypeViewModel(i)));
			return viewModels;
		}

		public static List<SashType> ToModels(this List<SashTypeViewModel> sashTypeViewModels)
		{
			if (sashTypeViewModels == null) throw new ArgumentNullException("sashTypeViewModels");
			List<SashType> sashTypes = new List<SashType>();
			sashTypeViewModels.ForEach(i => sashTypes.Add(i.ToModel()));
			return sashTypes;
		}

		public static SashType ToModel(this SashTypeViewModel sashTypeViewModel)
		{
			if (sashTypeViewModel == null) throw new ArgumentNullException("sashTypeViewModel");
			SashType sashType = new SashType();
			sashType.Id = sashTypeViewModel.Id;
			sashType.Name = sashTypeViewModel.Name;
			sashType.ProfileType = sashTypeViewModel.ProfileType.ToModel();
			sashType.Description = sashTypeViewModel.Description;
			sashType.Tolerance = sashTypeViewModel.Tolerance;
			return sashType;
		}

		#endregion

		#region MuntinType

		public static List<MuntinTypeViewModel> ToViewModels(this List<MuntinType> muntinTypes)
		{
			if (muntinTypes == null) throw new ArgumentException("muntinTypes");
			List<MuntinTypeViewModel> viewModels = new List<MuntinTypeViewModel>();
			muntinTypes.ForEach(i => viewModels.Add(new MuntinTypeViewModel(i)));
			return viewModels;
		}

		public static List<MuntinType> ToModels(this List<MuntinTypeViewModel> muntinTypeViewModels)
		{
			if (muntinTypeViewModels == null) throw new ArgumentNullException("muntinTypeViewModels");
			List<MuntinType> muntinTypes = new List<MuntinType>();
			muntinTypeViewModels.ForEach(i => muntinTypes.Add(i.ToModel()));
			return muntinTypes;
		}

		public static MuntinType ToModel(this MuntinTypeViewModel muntinTypeViewModel)
		{
			if (muntinTypeViewModel == null) throw new ArgumentNullException("muntinTypeViewModel");
			MuntinType muntinType = new MuntinType();
			muntinType.Id = muntinTypeViewModel.Id;
			muntinType.Name = muntinTypeViewModel.Name;
			muntinType.Width = muntinTypeViewModel.Width;
			muntinType.Weight = muntinTypeViewModel.Weight;
			muntinType.Height = muntinTypeViewModel.Height;
			muntinType.Price = muntinTypeViewModel.Price;
			muntinType.Description = muntinTypeViewModel.Description;
			muntinType.Tolerance = muntinTypeViewModel.Tolerance;
			return muntinType;
		}

		#endregion

		#region AccessoryType

		public static List<AccessoryTypeViewModel> ToViewModels(this List<AccessoryType> accessoryTypes)
		{
			if (accessoryTypes == null) throw new ArgumentException("accessoryTypes");
			List<AccessoryTypeViewModel> viewModels = new List<AccessoryTypeViewModel>();
			accessoryTypes.ForEach(i => viewModels.Add(new AccessoryTypeViewModel(i)));
			return viewModels;
		}

		public static List<AccessoryType> ToModels(this List<AccessoryTypeViewModel> accessoryTypeViewModels)
		{
			if (accessoryTypeViewModels == null) throw new ArgumentNullException("accessoryTypeViewModels");
			List<AccessoryType> accessoryTypes = new List<AccessoryType>();
			accessoryTypeViewModels.ForEach(i => accessoryTypes.Add(i.ToModel()));
			return accessoryTypes;
		}

		public static AccessoryType ToModel(this AccessoryTypeViewModel accessoryTypeViewModel)
		{
			if (accessoryTypeViewModel == null) throw new ArgumentNullException("accessoryTypeViewModel");
			AccessoryType accessoryType = new AccessoryType();
			accessoryType.Id = accessoryTypeViewModel.Id;
			accessoryType.Name = accessoryTypeViewModel.Name;
			accessoryType.Category = accessoryTypeViewModel.Category;
			accessoryType.Description = accessoryTypeViewModel.Description;
			accessoryType.Price = accessoryTypeViewModel.Price;

			return accessoryType;
		}

		#endregion

		#region Accessory

		public static List<AccessoryViewModel> ToViewModels(this List<Accessory> accessories)
		{
			if (accessories == null) throw new ArgumentException("accessories");
			List<AccessoryViewModel> viewModels = new List<AccessoryViewModel>();
			accessories.ForEach(i => viewModels.Add(new AccessoryViewModel(i)));
			return viewModels;
		}

		public static List<Accessory> ToModels(this List<AccessoryViewModel> accessoryViewModels)
		{
			if (accessoryViewModels == null) throw new ArgumentNullException("accessoryViewModels");
			List<Accessory> accessories = new List<Accessory>();
			accessoryViewModels.ForEach(i => accessories.Add(i.ToModel()));
			return accessories;
		}

		public static Accessory ToModel(this AccessoryViewModel accessoryViewModel)
		{
			Accessory accessory = new Accessory();
			accessory.Id = accessoryViewModel.Id;
			accessory.Code = accessoryViewModel.Code;
			accessory.Quantity = accessoryViewModel.Quantity;
			if (accessoryViewModel.AccessoryType != null)
				accessory.AccessoryType = accessoryViewModel.AccessoryType.ToModel();
			return accessory;
		}

		#endregion

		#region Project

		public static List<ProjectViewModel> ToViewModels(this List<Project> projects)
		{
			if (projects == null) throw new ArgumentException("projects");
			List<ProjectViewModel> viewModels = new List<ProjectViewModel>();
			projects.ForEach(i => viewModels.Add(new ProjectViewModel(i)));
			return viewModels;
		}

		public static List<Project> ToModels(this List<ProjectViewModel> projectViewModels)
		{
			if (projectViewModels == null) throw new ArgumentNullException("projectViewModels");
			List<Project> projects = new List<Project>();
			projectViewModels.ForEach(i => projects.Add(i.ToModel()));
			return projects;
		}

		public static Project ToModel(this ProjectViewModel projectViewModel)
		{
			if (projectViewModel == null) throw new ArgumentNullException("projectViewModel");
			Project project = new Project();
			project.Id = projectViewModel.Id;
			project.Path = projectViewModel.Path;
			project.Units = projectViewModel.Units.ToList().ToModels();
			project.Code = projectViewModel.Code;
			project.Units.ForEach(i => i.Project = project);
			return project;
		}

		#endregion

		#region Unit

		public static List<UnitViewModel> ToViewModels(this List<Unit> units)
		{
			if (units == null) throw new ArgumentException("units");
			List<UnitViewModel> viewModels = new List<UnitViewModel>();
			units.ForEach(i => viewModels.Add(new UnitViewModel(i)));
			return viewModels;
		}
		public static List<Unit> ToModels(this List<UnitViewModel> unitViewModels)
		{
			if (unitViewModels == null) throw new ArgumentNullException("unitViewModels");
			List<Unit> units = new List<Unit>();
			unitViewModels.ForEach(i => units.Add(i.ToModel()));
			return units;
		}
		public static Unit ToModel(this UnitViewModel unitViewModel)
		{
			if (unitViewModel == null) throw new ArgumentNullException("unitViewModel");
			Unit unit = new Unit();
			if (unitViewModel.Original != null)
			{
				Project p = ((Unit)unitViewModel.Original).Project as Project;
				unit.Project = p;
			}

			unit.Code = unitViewModel.Code;
			unit.Description = unitViewModel.Description;
			unit.Frame = unitViewModel.Frame;
			unit.Height = unitViewModel.Height;
			unit.Id = unitViewModel.Id;
			unit.Image = unitViewModel.Image;
			unit.Quantity = unitViewModel.Quantity;
			unit.VersionInfo = unitViewModel.VersionInfo;
			unit.Width = unitViewModel.Width;
			unit.Accessories = unitViewModel.Accessories.ToList().ToModels();

			return unit;
		}

		#endregion

		#region PVCFrame

		public static byte[] ToBytes(this PVCFrame frame)
		{
			IFormatter formatter = new BinaryFormatter();
            //IFormatter formatter = new JsonNetFormatter();
            formatter.SurrogateSelector = PUtil.FrameworkSurrogateSelector;
			MemoryStream stream = new MemoryStream();
			PStream pStream = new PStream(stream);
			pStream.WriteObjectTree(formatter, frame);
			return stream.ToArray();
		}

		public static PVCFrame ToFrame(this byte[] byteArray)
		{
			PVCFrame frame = null;
            IFormatter formatter = new BinaryFormatter();
            //IFormatter formatter = new JsonNetFormatter();
            formatter.SurrogateSelector = PUtil.FrameworkSurrogateSelector;
			MemoryStream stream = new MemoryStream(byteArray);
			PStream pStream = new PStream(stream);
			frame = (PVCFrame)pStream.ReadObjectTree(formatter);
			stream.Close();
			return frame;
		}

		#endregion
	}
}

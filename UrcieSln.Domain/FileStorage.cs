using Modules.JsonNet;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using UrcieSln.Domain.Entities;

namespace UrcieSln.Domain
{
	public class FileStorage
	{
        private static string confFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments); //Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\KBITS\Urcie";
		private Dictionary<Type, string> pathes = new Dictionary<Type, string>
		{
			{	typeof(ProfileType),	confFolder + @"\profiletypes.dat"	},
			{	typeof(FillingType),	confFolder + @"\fillingTypes.dat"	},
			{	typeof(MullionType),	confFolder + @"\mulliontypes.dat"	},
			{	typeof(SashType),		confFolder + @"\sashtypes.dat"		},
			{	typeof(AccessoryType),	confFolder + @"\accessorytypes.dat"	},
			{	typeof(MuntinType),		confFolder + @"\muntinstype.dat"	},
		};
		#region Instantiation

		private static FileStorage instance;
		private FileStorage() {}
		public static FileStorage GetInstance()
		{
			if(instance == null)
				instance = new FileStorage();

			return instance;
		}

		#endregion

		public T GetById<T>(Guid id) where T : Entity
		{
			string path = GetPath<T>();

			List<T> types = GetAll<T>();
			return types.Find(i => i.Id == id);
		}

		public List<T> GetAll<T>() where T : Entity
		{
			string path = GetPath<T>();

			List<T> types = new List<T>();
			try
			{
				using (Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
				{
                    //IFormatter formatter = new BinaryFormatter();
                    IFormatter formatter = new JsonNetFormatter();
                    types = (List<T>)formatter.Deserialize(stream);
				}
			}
			catch (Exception e)
			{
				Console.WriteLine("TODO: Log the exception!");
				Console.WriteLine(e.Message);
			}

			return types;
		}

		public void Add<T>(T type) where T : Entity
		{
			if (type == null) throw new ArgumentNullException("type");

			List<T> allTypes = GetAll<T>();

			if (allTypes.Find(t => t.Id == type.Id) == null)
			{
				allTypes.Add(type);
				SaveAll<T>(allTypes);
			}
			else
			{
				throw new InvalidOperationException("type is already exist in the storage.");
			}
		}

		public void Update<T>(T type) where T : Entity
		{
			if (type == null) throw new ArgumentNullException("type");

			List<T> allTypes = GetAll<T>();
			T oldValue = allTypes.Find(t => t.Id == type.Id);

			if(oldValue == null)
			{
				throw new InvalidOperationException("Type was not found in the storage.");
			}

			int index = allTypes.IndexOf(oldValue);
			if (index != -1) allTypes[index] = type;
			
			SaveAll<T>(allTypes);
		}

		public void Delete<T>(T type) where T : Entity
		{
			if (type == null) throw new ArgumentNullException("type");

			List<T> allTypes = GetAll<T>();
			int index = allTypes.IndexOf(type);
			if (index != -1)
			{
				allTypes.RemoveAt(index);
				SaveAll<T>(allTypes);
			}
			else
				throw new InvalidOperationException("type does not exist in the storage.");
		}
		
		public void SaveAll<T>(List<T> types) where T : Entity 
		{
			if (types == null) throw new ArgumentNullException("types");
			string path = GetPath<T>();

			using(Stream stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None))
			{
				//IFormatter formatter = new BinaryFormatter();
                IFormatter formatter = new JsonNetFormatter();
                formatter.Serialize(stream, types);
			}
		}

		public void SaveProject(Project project)
		{
			if (project == null) throw new ArgumentNullException("project");

			string path = Path.GetFullPath(project.Path);
			using(Stream stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None))
			{
                IFormatter formatter = new BinaryFormatter();
                //IFormatter formatter = new JsonNetFormatter();
                formatter.Serialize(stream, project);
			}
		}
		
		public void SaveUnit(Unit unit)
		{
			if (unit == null) throw new ArgumentNullException("unit");
			if (unit.Project == null) throw new ArgumentException("Invalid unit argument");

			string path = Path.GetFullPath(unit.Project.Path);
			Project project = GetProject(path);
			Unit originalUnit = project.Units.Find(i => i.Id == unit.Id);
			if(originalUnit == null)
			{
				project.Units.Add(unit);
			}
			else
			{
				project.Units[project.Units.IndexOf(originalUnit)] = unit;
			}
			SaveProject(project);
		}

		public void DeleteUnit(Unit unit)
		{
			if (unit == null) throw new ArgumentNullException("unit");
			if (unit.Project == null) throw new ArgumentException("Invalid unit argument.");

			string path = Path.GetFullPath(unit.Project.Path);
			Project project = GetProject(path);
			var originalUnit = project.Units.Find(i => i.Id == unit.Id);
			if (originalUnit == null)
			{
				return;
			}
			project.Units.Remove(originalUnit);
			SaveProject(project);
		}

		private string GetPath<T>() where T : Entity
		{
			if (!pathes.ContainsKey(typeof(T))) throw new ArgumentException("Invalid type parameter.");

			return pathes.Where(i => i.Key == typeof(T)).First().Value;
		}

		public Project GetProject(string path)
		{
			Project project = null;
			using(Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
			{
				try
				{
                    IFormatter formatter = new BinaryFormatter();
                    //IFormatter formatter = new JsonNetFormatter();
                    project = (Project)formatter.Deserialize(stream);
				}
				catch (Exception e)
				{
					Console.WriteLine("TODO: Log the exception.");
					Console.WriteLine(e.Message);
				}
			}
			return project;
		}
	}
}
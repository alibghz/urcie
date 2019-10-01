using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrcieSln.Domain;
using UrcieSln.Domain.Models;

namespace UrcieSln.Domain.Extensions
{
	public static class IExtensionMethods
	{
		public static Filling ToFilling(this IFilling item)
		{
			var val = new Filling();

			if (item.FillingType != null)
				val.FillingType = item.FillingType;
			if (item.Parent != null)
				val.Parent = item.Parent;
			if (item.Code != null)
				val.Code = item.Code;
			if (item.ProfileType != null)
				val.ProfileType = item.ProfileType;

			return val;
		}

		public static Frame ToFrame(this IFrame item)
		{
			var val = new Frame();

			if (item.Accessories != null)
				val.Accessories = item.Accessories;
			if (item.Child != null)
				val.Child = item.Child;
			if (item.Code != null)
				val.Code = item.Code;
			if (item.Muntins != null)
				val.Muntins = item.Muntins;
			if (item.ProfileType != null)
				val.ProfileType = item.ProfileType;
			if (item.UProfileType != null)
				val.UProfileType = item.UProfileType;
			val.Height = item.Height;
			val.Width = item.Width;

			return val;
		}

		public static Mullion ToMullion(this IMullion item)
		{
			var val = new Mullion();

			if (item.MullionType != null)
				val.MullionType = item.MullionType;
			if (item.Parent != null)
				val.Parent = item.Parent;
			if (item.Code != null)
				val.Code = item.Code;
			if (item.Next != null)
				val.Next = item.Next;
			if (item.Previous != null)
				val.Previous = item.Previous;
			val.IsVirtual = item.IsVirtual;
			val.Orientation = item.Orientation;
			val.Length = item.Length;
			val.MiddlePoint = item.MiddlePoint;

			return val;
		}

		public static Muntin ToMuntin(this IMuntin item)
		{
			var val = new Muntin();

			if (item.Code != null)
				val.Code = item.Code;
			if (item.MuntinType != null)
				val.MuntinType = item.MuntinType;
			val.X = item.X;
			val.Y = item.Y;
			val.Width = item.Width;
			val.Height = item.Height;

			return val;
		}

		public static Profile ToProfile(this IProfile item)
		{
			var val = new Profile();

			if (item.Code != null)
				val.Code = item.Code;
			val.LeftAngle = item.LeftAngle;
			val.Length = item.Length;
			val.Orientation = item.Orientation;
			val.ProfileType = item.ProfileType;
			val.RightAngle = item.RightAngle;

			return val;
		}

		public static Sash ToSash(this ISash item)
		{
			var val = new Sash();

			if (item.Code != null)
				val.Code = item.Code;
			if (item.Child != null)
				val.Child = item.Child;
			if (item.Parent != null)
				val.Parent = item.Parent;
			if (item.SashType != null)
				val.SashType = item.SashType;

			return val;
		}

		public static DSash ToDSash(this IDSash item)
		{
			var val = new DSash();

			if (item.Code != null)
				val.Code = item.Code;
			if (item.Children != null)
				val.Children = item.Children;
			if (item.Parent != null)
				val.Parent = item.Parent;
			if (item.SashType != null)
				val.SashType = item.SashType;
			if (item.MullionType != null)
				val.MullionType = item.MullionType;
			if (item.Code != null)
				val.Code = item.Code;
			if (item.Next != null)
				val.Next = item.Next;
			if (item.Previous != null)
				val.Previous = item.Previous;
			val.IsCornered = item.IsCornered;
			val.Orientation = item.Orientation;
			val.Length = item.Length;
			val.MiddlePoint = item.MiddlePoint;

			return val;
		}

		public static Surface ToSurface(this ISurface item)
		{
			var val = new Surface();

			if (item.Code != null)
				val.Code = item.Code;
			if (item.Children != null)
				val.Children = item.Children;
			if (item.Parent != null)
				val.Parent = item.Parent;
			if (item.Next != null)
				val.Next = item.Next;
			if (item.Previous != null)
				val.Previous = item.Previous;
			val.Orientation = item.Orientation;
			val.Height = item.Height;
			val.Width = item.Width;
			val.X = item.X;
			val.Y = item.Y;

			return val;
		}

		public static IList<Filling> ToFillings(this IList<IFilling> items)
		{
			var val = new List<Filling>();

			foreach (var item in items)
			{
				val.Add(ToFilling(item));
			}

			return val;
		}

		public static IList<Frame> ToFrames(this IList<IFrame> items)
		{
			var val = new List<Frame>();

			foreach (var item in items)
			{
				val.Add(ToFrame(item));
			}

			return val;
		}

		public static IList<Mullion> ToMullions(this IList<IMullion> items)
		{
			var val = new List<Mullion>();

			foreach (var item in items)
			{
				val.Add(ToMullion(item));
			}

			return val;
		}

		public static IList<Muntin> ToMuntins(this IList<IMuntin> items)
		{
			var val = new List<Muntin>();

			foreach (var item in items)
			{
				val.Add(ToMuntin(item));
			}

			return val;
		}

		public static IList<Profile> ToProfiles(this IList<IProfile> items)
		{
			var val = new List<Profile>();

			foreach (var item in items)
			{
				val.Add(ToProfile(item));
			}

			return val;
		}

		public static IList<Sash> ToSashes(this IList<ISash> items)
		{
			var val = new List<Sash>();

			foreach (var item in items)
			{
				val.Add(ToSash(item));
			}

			return val;
		}

		public static IList<Surface> ToSurfaces(this IList<ISurface> items)
		{
			var val = new List<Surface>();

			foreach (var item in items)
			{
				val.Add(ToSurface(item));
			}

			return val;
		}
	}
}

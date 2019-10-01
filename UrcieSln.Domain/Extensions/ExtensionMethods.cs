using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using UrcieSln.Domain.Entities;
using UrcieSln.Domain.Models;

namespace UrcieSln.Domain.Extensions
{
	public static class ExtensionMethods
	{
		#region GetObjects

		#region GetProfiles
		#region Frame

		public static IList<Profile> GetProfiles(this Frame frame, bool includeTolerance = true)
		{
			IList<Profile> val = new List<Profile>();
			float width = frame.Width, height = frame.Height;
			if (includeTolerance)
			{
				float tmpProfTol = frame.ProfileType.Tolerance;
				width += tmpProfTol;
				height += tmpProfTol;
			}

			val.Add(new Profile { Code = frame.Code + "-01", LeftAngle = 45, RightAngle = 45, Length = width, Orientation = Orientation.Horizontal, ProfileType = frame.ProfileType });
			val.Add(new Profile { Code = frame.Code + "-02", LeftAngle = 45, RightAngle = 45, Length = height, Orientation = Orientation.Vertical, ProfileType = frame.ProfileType });
			val.Add(new Profile { Code = frame.Code + "-03", LeftAngle = 45, RightAngle = 45, Length = width, Orientation = Orientation.Horizontal, ProfileType = frame.ProfileType });
			val.Add(new Profile { Code = frame.Code + "-04", LeftAngle = 45, RightAngle = 45, Length = height, Orientation = Orientation.Vertical, ProfileType = frame.ProfileType });

			if (frame.UProfileType != null)
			{
				if (includeTolerance)
				{
					float tmpProfThick = frame.ProfileType.Thickness;
					float tmpUProfTol = frame.UProfileType.Tolerance;
					width = frame.Width + tmpUProfTol - 2 * tmpProfThick;
					height = frame.Height + tmpUProfTol - 2 * tmpProfThick;
				}
				val.Add(new Profile { Code = frame.Code + "-U01", LeftAngle = 90, RightAngle = 90, Length = width, Orientation = Orientation.Horizontal, ProfileType = frame.UProfileType });
				val.Add(new Profile { Code = frame.Code + "-U02", LeftAngle = 90, RightAngle = 90, Length = height, Orientation = Orientation.Vertical, ProfileType = frame.UProfileType });
				val.Add(new Profile { Code = frame.Code + "-U03", LeftAngle = 90, RightAngle = 90, Length = width, Orientation = Orientation.Horizontal, ProfileType = frame.UProfileType });
				val.Add(new Profile { Code = frame.Code + "-U04", LeftAngle = 90, RightAngle = 90, Length = height, Orientation = Orientation.Vertical, ProfileType = frame.UProfileType });
			}
			return val;
		}

		public static IList<Profile> GetProfiles(this Frame frame, bool includeTolerance, bool includeChildren)
		{
			IList<Profile> val = new List<Profile>(), childVal = new List<Profile>();

			val = GetProfiles(frame, includeTolerance);
			childVal = frame.Child.GetProfiles(includeTolerance, includeChildren);

			if (childVal != null) foreach (Profile item in childVal) val.Add(item);

			return val;
		}

		#endregion

		#region Object
		public static IList<Profile> GetProfiles(this object obj, bool includeTolerance = true)
		{
			IList<Profile> val = new List<Profile>();
			if (obj is Frame)
			{
				val = GetProfiles((Frame)obj, includeTolerance);
			}
			else if (obj is Mullion)
			{
				val = GetProfiles((Mullion)obj, includeTolerance);
			}
			else if (obj is Sash)
			{
				val = GetProfiles((Sash)obj, includeTolerance);
			}
			else if (obj is DSash)
			{
				val = GetProfiles((DSash)obj, includeTolerance);
			}
			else if (obj is Filling)
			{
				val = GetProfiles((Filling)obj, includeTolerance);
			}
			else if (obj is Surface)
			{
				val = GetProfiles((Surface)obj, includeTolerance);
			}
			else if (obj is IFrame)
			{
				val = GetProfiles(((IFrame)obj).ToFrame(), includeTolerance);
			}
			else if (obj is IMullion)
			{
				val = GetProfiles(((IMullion)obj).ToMullion(), includeTolerance);
			}
			else if (obj is ISash)
			{
				val = GetProfiles(((ISash)obj).ToSash(), includeTolerance);
			}
			else if (obj is IDSash)
			{
				val = GetProfiles(((IDSash)obj).ToDSash(), includeTolerance);
			}
			else if (obj is IFilling)
			{
				val = GetProfiles(((IFilling)obj).ToFilling(), includeTolerance);
			}
			else if (obj is ISurface)
			{
				val = GetProfiles(((ISurface)obj).ToSurface(), includeTolerance);
			}

			return val;
		}
		public static IList<Profile> GetProfiles(this object obj, bool includeTolerance, bool includeChildren)
		{
			IList<Profile> val = new List<Profile>();
			if (obj is Frame)
			{
				val = GetProfiles((Frame)obj, includeChildren, includeTolerance);
			}
			else if (obj is Mullion)
			{
				val = GetProfiles((Mullion)obj, includeTolerance, includeChildren);
			}
			else if (obj is Sash)
			{
				val = GetProfiles((Sash)obj, includeTolerance, includeChildren);
			}
			else if (obj is DSash)
			{
				val = GetProfiles((DSash)obj, includeTolerance, includeChildren);
			}
			else if (obj is Filling)
			{
				val = GetProfiles((Filling)obj, includeTolerance, includeChildren);
			}
			else if (obj is Surface)
			{
				val = GetProfiles((Surface)obj, includeTolerance, includeChildren);
			}
			else if (obj is IFrame)
			{
				val = GetProfiles(((IFrame)obj).ToFrame(), includeTolerance, includeChildren);
			}
			else if (obj is IMullion)
			{
				val = GetProfiles(((IMullion)obj).ToMullion(), includeTolerance, includeChildren);
			}
			else if (obj is ISash)
			{
				val = GetProfiles(((ISash)obj).ToSash(), includeTolerance, includeChildren);
			}
			else if (obj is IDSash)
			{
				val = GetProfiles(((IDSash)obj).ToDSash(), includeTolerance, includeChildren);
			}
			else if (obj is IFilling)
			{
				val = GetProfiles(((IFilling)obj).ToFilling(), includeTolerance, includeChildren);
			}
			else if (obj is ISurface)
			{
				val = GetProfiles(((ISurface)obj).ToSurface(), includeTolerance, includeChildren);
			}

			return val;
		}
		#endregion

		#region Surface
		public static IList<Profile> GetProfiles(this Surface surface, bool includeTolerance = true)
		{
			IList<Profile> val = new List<Profile>();

			//val = GetProfiles(surface, includeTolerance);

			return val;
		}
		public static IList<Profile> GetProfiles(this Surface surface, bool includeTolerance, bool includeChildren)
		{
			IList<Profile> val = new List<Profile>();
			val = GetProfiles(surface, includeTolerance);
			if (surface.Children != null) foreach (object item in surface.Children)
				{
					IList<Profile> tmp = new List<Profile>();
					tmp = GetProfiles(item, includeTolerance, includeChildren);
					if (tmp != null) foreach (Profile tmpItem in tmp) val.Add(tmpItem);
				}

			return val;
		}
		#endregion

		#region Mullion
		public static IList<Profile> GetProfiles(this Mullion mullion, bool includeTolerance = true)
		{
			IList<Profile> val = new List<Profile>();
			float length = mullion.Length;
			if (includeTolerance) length += mullion.MullionType.ProfileType.Tolerance;
			if (!mullion.IsVirtual)
			val.Add(new Profile { Code = mullion.Code, LeftAngle = 90, RightAngle = 90, Length = length, Orientation = mullion.Orientation, ProfileType = mullion.MullionType.ProfileType });

			return val;
		}
		public static IList<Profile> GetProfiles(this Mullion mullion, bool includeTolerance, bool includeChildren)
		{
			IList<Profile> val = new List<Profile>();

			val = GetProfiles(mullion, includeTolerance);

			return val;
		}
		#endregion

		#region Sash
		public static IList<Profile> GetProfiles(this Sash sash, bool includeTolerance = true)
		{
			IList<Profile> val = new List<Profile>();
			float width = sash.Parent.Width, height = sash.Parent.Height;
			if (includeTolerance)
			{
				float tmpProfTol = sash.SashType.ProfileType.Tolerance;
				float tmpSashTol = sash.SashType.Tolerance;
				width += tmpProfTol + tmpSashTol;
				height += tmpProfTol + tmpSashTol;
			}

			val.Add(new Profile { Code = sash.Code + "-01", LeftAngle = 45, RightAngle = 45, Length = width, Orientation = Orientation.Horizontal, ProfileType = sash.SashType.ProfileType });
			val.Add(new Profile { Code = sash.Code + "-02", LeftAngle = 45, RightAngle = 45, Length = height, Orientation = Orientation.Vertical, ProfileType = sash.SashType.ProfileType });
			val.Add(new Profile { Code = sash.Code + "-03", LeftAngle = 45, RightAngle = 45, Length = width, Orientation = Orientation.Horizontal, ProfileType = sash.SashType.ProfileType });
			val.Add(new Profile { Code = sash.Code + "-04", LeftAngle = 45, RightAngle = 45, Length = height, Orientation = Orientation.Vertical, ProfileType = sash.SashType.ProfileType });

			return val;
		}
		public static IList<Profile> GetProfiles(this Sash sash, bool includeTolerance, bool includeChildren)
		{
			IList<Profile> val = new List<Profile>(), childVal = new List<Profile>();

			val = GetProfiles(sash, includeTolerance);
			childVal = sash.Child.GetProfiles(includeTolerance, includeChildren);

			if (childVal != null) foreach (Profile item in childVal) val.Add(item);

			return val;
		}
		#endregion

		#region DSash
		public static IList<Profile> GetProfiles(this DSash dSash, bool includeTolerance = true)
		{
			IList<Profile> val = new List<Profile>();
			float width = dSash.Parent.Width, height = dSash.Parent.Height;
			if (dSash.Orientation == Orientation.Horizontal)
				width = dSash.MiddlePoint - dSash.Parent.X;
			if (dSash.Orientation == Orientation.Vertical)
				height = dSash.MiddlePoint - dSash.Parent.Y;
			if (includeTolerance)
			{
				float tmpProfTol = dSash.SashType.ProfileType.Tolerance;
				float tmpSashTol = dSash.SashType.Tolerance;
				width += tmpProfTol + tmpSashTol;
				height += tmpProfTol + tmpSashTol;
			}

			val.Add(new Profile { Code = dSash.Code + "-A-01", LeftAngle = 45, RightAngle = 45, Length = width, Orientation = Orientation.Horizontal, ProfileType = dSash.SashType.ProfileType });
			val.Add(new Profile { Code = dSash.Code + "-A-02", LeftAngle = 45, RightAngle = 45, Length = height, Orientation = Orientation.Vertical, ProfileType = dSash.SashType.ProfileType });
			val.Add(new Profile { Code = dSash.Code + "-A-03", LeftAngle = 45, RightAngle = 45, Length = width, Orientation = Orientation.Horizontal, ProfileType = dSash.SashType.ProfileType });
			val.Add(new Profile { Code = dSash.Code + "-A-04", LeftAngle = 45, RightAngle = 45, Length = height, Orientation = Orientation.Vertical, ProfileType = dSash.SashType.ProfileType });

			width = dSash.Parent.Width; height = dSash.Parent.Height;
			if (dSash.Orientation == Orientation.Horizontal)
				width = dSash.Parent.X + dSash.Parent.Width - dSash.MiddlePoint;
			if (dSash.Orientation == Orientation.Vertical)
				height = dSash.Parent.Y + dSash.Parent.Height - dSash.MiddlePoint;
			if (includeTolerance)
			{
				float tmpProfTol = dSash.SashType.ProfileType.Tolerance;
				float tmpSashTol = dSash.SashType.Tolerance;
				width += tmpProfTol + tmpSashTol;
				height += tmpProfTol + tmpSashTol;
			}

			val.Add(new Profile { Code = dSash.Code + "-B-01", LeftAngle = 45, RightAngle = 45, Length = width, Orientation = Orientation.Horizontal, ProfileType = dSash.SashType.ProfileType });
			val.Add(new Profile { Code = dSash.Code + "-B-02", LeftAngle = 45, RightAngle = 45, Length = height, Orientation = Orientation.Vertical, ProfileType = dSash.SashType.ProfileType });
			val.Add(new Profile { Code = dSash.Code + "-B-03", LeftAngle = 45, RightAngle = 45, Length = width, Orientation = Orientation.Horizontal, ProfileType = dSash.SashType.ProfileType });
			val.Add(new Profile { Code = dSash.Code + "-B-04", LeftAngle = 45, RightAngle = 45, Length = height, Orientation = Orientation.Vertical, ProfileType = dSash.SashType.ProfileType });

			return val;
		}
		public static IList<Profile> GetProfiles(this DSash dSash, bool includeTolerance, bool includeChildren)
		{
			IList<Profile> val = new List<Profile>(), childVal = new List<Profile>();

			val = GetProfiles(dSash, includeTolerance);
			childVal = dSash.Children.GetProfiles(includeTolerance, includeChildren);

			if (childVal != null) foreach (Profile item in childVal) val.Add(item);

			return val;
		}
		#endregion

		#region Filling
		public static IList<Profile> GetProfiles(this Filling filling, bool includeTolerance = true)
		{
			IList<Profile> val = new List<Profile>();

			if (filling.ProfileType != null)
			{
				float width = filling.Parent.Width, height = filling.Parent.Height;
				if (includeTolerance)
				{
					float tmpProfTol = filling.ProfileType.Tolerance;
					width += tmpProfTol;
					height += tmpProfTol;
				}

				val.Add(new Profile { Code = filling.Code + "-01", LeftAngle = 45, RightAngle = 45, Length = width, Orientation = Orientation.Horizontal, ProfileType = filling.ProfileType });
				val.Add(new Profile { Code = filling.Code + "-02", LeftAngle = 45, RightAngle = 45, Length = height, Orientation = Orientation.Vertical, ProfileType = filling.ProfileType });
				val.Add(new Profile { Code = filling.Code + "-03", LeftAngle = 45, RightAngle = 45, Length = width, Orientation = Orientation.Horizontal, ProfileType = filling.ProfileType });
				val.Add(new Profile { Code = filling.Code + "-04", LeftAngle = 45, RightAngle = 45, Length = height, Orientation = Orientation.Vertical, ProfileType = filling.ProfileType });
			}

			return val;
		}
		public static IList<Profile> GetProfiles(this Filling filling, bool includeTolerance, bool includeChildren)
		{
			IList<Profile> val = new List<Profile>();

			val = GetProfiles(filling, includeTolerance);

			return val;
		}
		#endregion
		#endregion

		#region GetAccessories
		#region Frame
		public static IList<Accessory> GetAccessories(this Frame frame)
		{
			IList<Accessory> val = new List<Accessory>();

			if (frame.Accessories != null)
				foreach (var item in frame.Accessories)
				{ item.Code = frame.Code; val.Add(item); }

			return val;
		}

		public static IList<Accessory> GetAccessories(this Frame frame, bool includeChildren)
		{
			IList<Accessory> val = new List<Accessory>(), childVal = new List<Accessory>();

			val = GetAccessories(frame);
			childVal = frame.Child.GetAccessories(includeChildren);

			if (childVal != null) foreach (Accessory item in childVal) val.Add(item);

			return val;
		}
		#endregion

		#region object
		public static IList<Accessory> GetAccessories(this object obj)
		{
			IList<Accessory> val = new List<Accessory>();

			if (obj is Frame)
			{
				val = GetAccessories((Frame)obj);
			}
			else if (obj is Mullion)
			{
				val = GetAccessories((Mullion)obj);
			}
			else if (obj is Sash)
			{
				val = GetAccessories((Sash)obj);
			}
			else if (obj is Filling)
			{
				val = GetAccessories((Filling)obj);
			}
			else if (obj is Surface)
			{
				val = GetAccessories((Surface)obj);
			}
			else if (obj is IFrame)
			{
				val = GetAccessories(((IFrame)obj).ToFrame());
			}
			else if (obj is IMullion)
			{
				val = GetAccessories(((IMullion)obj).ToMullion());
			}
			else if (obj is ISash)
			{
				val = GetAccessories(((ISash)obj).ToSash());
			}
			else if (obj is IFilling)
			{
				val = GetAccessories(((IFilling)obj).ToFilling());
			}
			else if (obj is ISurface)
			{
				val = GetAccessories(((ISurface)obj).ToSurface());
			}

			return val;
		}

		public static IList<Accessory> GetAccessories(this object obj, bool includeChildren)
		{
			IList<Accessory> val = new List<Accessory>();

			if (obj is Frame)
			{
				val = GetAccessories((Frame)obj, includeChildren);
			}
			else if (obj is Mullion)
			{
				val = GetAccessories((Mullion)obj, includeChildren);
			}
			else if (obj is Sash)
			{
				val = GetAccessories((Sash)obj, includeChildren);
			}
			else if (obj is Filling)
			{
				val = GetAccessories((Filling)obj, includeChildren);
			}
			else if (obj is Surface)
			{
				val = GetAccessories((Surface)obj, includeChildren);
			}
			else if (obj is IFrame)
			{
				val = GetAccessories(((IFrame)obj).ToFrame(), includeChildren);
			}
			else if (obj is IMullion)
			{
				val = GetAccessories(((IMullion)obj).ToMullion(), includeChildren);
			}
			else if (obj is ISash)
			{
				val = GetAccessories(((ISash)obj).ToSash(), includeChildren);
			}
			else if (obj is IFilling)
			{
				val = GetAccessories(((IFilling)obj).ToFilling(), includeChildren);
			}
			else if (obj is ISurface)
			{
				val = GetAccessories(((ISurface)obj).ToSurface(), includeChildren);
			}

			return val;
		}
		#endregion

		#region Surface
		public static IList<Accessory> GetAccessories(this Surface surface)
		{
			IList<Accessory> val = new List<Accessory>();

			return val;
		}

		public static IList<Accessory> GetAccessories(this Surface surface, bool includeChildren)
		{
			IList<Accessory> val = new List<Accessory>(), childVal = new List<Accessory>();
			val = GetAccessories(surface);
			if (surface.Children != null) foreach (object item in surface.Children)
				{
					IList<Accessory> tmp = new List<Accessory>();
					tmp = GetAccessories(item, includeChildren);
					if (tmp != null) foreach (Accessory tmpItem in tmp) val.Add(tmpItem);
				}

			return val;
		}
		#endregion

		#region Mullion
		public static IList<Accessory> GetAccessories(this Mullion mullion)
		{
			IList<Accessory> val = new List<Accessory>();

			return val;
		}

		public static IList<Accessory> GetAccessories(this Mullion mullion, bool includeChildren)
		{
			IList<Accessory> val = new List<Accessory>();

			val = GetAccessories(mullion);

			return val;
		}
		#endregion

		#region Sash
		public static IList<Accessory> GetAccessories(this Sash sash)
		{
			IList<Accessory> val = new List<Accessory>();

			if (sash.SashType.Accessories != null)
				foreach (var item in sash.SashType.Accessories)
				{ item.Code = sash.Code; val.Add(item); }

			return val;
		}

		public static IList<Accessory> GetAccessories(this Sash sash, bool includeChildren)
		{
			IList<Accessory> val = new List<Accessory>(), childVal = new List<Accessory>();

			val = GetAccessories(sash);
			childVal = sash.Child.GetAccessories(includeChildren);

			if (childVal != null) foreach (Accessory item in childVal) val.Add(item);

			return val;
		}
		#endregion

		#region Filling
		public static IList<Accessory> GetAccessories(this Filling filling)
		{
			IList<Accessory> val = new List<Accessory>();

			return val;
		}

		public static IList<Accessory> GetAccessories(this Filling filling, bool includeChildren)
		{
			IList<Accessory> val = new List<Accessory>();

			val = GetAccessories(filling);

			return val;
		}
		#endregion
		#endregion

		#region GetFillings
		#region Frame
		public static IList<Filling> GetFillings(this Frame frame)
		{
			IList<Filling> val = new List<Filling>();

			return val;
		}

		public static IList<Filling> GetFillings(this Frame frame, bool includeChildren)
		{
			IList<Filling> val = new List<Filling>(), childVal = new List<Filling>();

			val = GetFillings(frame);
			childVal = frame.Child.GetFillings(includeChildren);

			if (childVal != null) foreach (Filling item in childVal) val.Add(item);

			return val;
		}
		#endregion

		#region object
		public static IList<Filling> GetFillings(this object obj)
		{
			IList<Filling> val = new List<Filling>();

			if (obj is Frame)
			{
				val = GetFillings((Frame)obj);
			}
			else if (obj is Mullion)
			{
				val = GetFillings((Mullion)obj);
			}
			else if (obj is Sash)
			{
				val = GetFillings((Sash)obj);
			}
			else if (obj is Filling)
			{
				val = GetFillings((Filling)obj);
			}
			else if (obj is Surface)
			{
				val = GetFillings((Surface)obj);
			}
			else if (obj is IFrame)
			{
				val = GetFillings(((IFrame)obj).ToFrame());
			}
			else if (obj is IMullion)
			{
				val = GetFillings(((IMullion)obj).ToMullion());
			}
			else if (obj is ISash)
			{
				val = GetFillings(((ISash)obj).ToSash());
			}
			else if (obj is IFilling)
			{
				val = GetFillings(((IFilling)obj).ToFilling());
			}
			else if (obj is ISurface)
			{
				val = GetFillings(((ISurface)obj).ToSurface());
			}

			return val;
		}

		public static IList<Filling> GetFillings(this object obj, bool includeChildren)
		{
			IList<Filling> val = new List<Filling>();

			if (obj is Frame)
			{
				val = GetFillings((Frame)obj, includeChildren);
			}
			else if (obj is Mullion)
			{
				val = GetFillings((Mullion)obj, includeChildren);
			}
			else if (obj is Sash)
			{
				val = GetFillings((Sash)obj, includeChildren);
			}
			else if (obj is Filling)
			{
				val = GetFillings((Filling)obj, includeChildren);
			}
			else if (obj is Surface)
			{
				val = GetFillings((Surface)obj, includeChildren);
			}
			else if (obj is IFrame)
			{
				val = GetFillings(((IFrame)obj).ToFrame(), includeChildren);
			}
			else if (obj is IMullion)
			{
				val = GetFillings(((IMullion)obj).ToMullion(), includeChildren);
			}
			else if (obj is ISash)
			{
				val = GetFillings(((ISash)obj).ToSash(), includeChildren);
			}
			else if (obj is IFilling)
			{
				val = GetFillings(((IFilling)obj).ToFilling(), includeChildren);
			}
			else if (obj is ISurface)
			{
				val = GetFillings(((ISurface)obj).ToSurface(), includeChildren);
			}

			return val;
		}
		#endregion

		#region Surface
		public static IList<Filling> GetFillings(this Surface surface)
		{
			IList<Filling> val = new List<Filling>();

			return val;
		}

		public static IList<Filling> GetFillings(this Surface surface, bool includeChildren)
		{
			IList<Filling> val = new List<Filling>(), childVal = new List<Filling>();
			val = GetFillings(surface);
			if (surface.Children != null) foreach (object item in surface.Children)
				{
					IList<Filling> tmp = new List<Filling>();
					tmp = GetFillings(item, includeChildren);
					if (tmp != null) foreach (Filling tmpItem in tmp) val.Add(tmpItem);
				}

			return val;
		}
		#endregion

		#region Mullion
		public static IList<Filling> GetFillings(this Mullion mullion)
		{
			IList<Filling> val = new List<Filling>();

			return val;
		}

		public static IList<Filling> GetFillings(this Mullion mullion, bool includeChildren)
		{
			IList<Filling> val = new List<Filling>();

			val = GetFillings(mullion);

			return val;
		}
		#endregion

		#region Sash
		public static IList<Filling> GetFillings(this Sash sash)
		{
			IList<Filling> val = new List<Filling>();

			return val;
		}

		public static IList<Filling> GetFillings(this Sash sash, bool includeChildren)
		{
			IList<Filling> val = new List<Filling>(), childVal = new List<Filling>();

			val = GetFillings(sash);
			childVal = sash.Child.GetFillings(includeChildren);

			if (childVal != null) foreach (Filling item in childVal) val.Add(item);

			return val;
		}
		#endregion

		#region Filling
		public static IList<Filling> GetFillings(this Filling filling)
		{
			IList<Filling> val = new List<Filling>();

			val.Add(filling);

			return val;
		}

		public static IList<Filling> GetFillings(this Filling filling, bool includeChildren)
		{
			IList<Filling> val = new List<Filling>();

			val = GetFillings(filling);

			return val;
		}
		#endregion
		#endregion

		#region GetSashes
		#region Frame
		public static IList<Sash> GetSashes(this Frame frame)
		{
			IList<Sash> val = new List<Sash>();

			return val;
		}

		public static IList<Sash> GetSashes(this Frame frame, bool includeChildren)
		{
			IList<Sash> val = new List<Sash>(), childVal = new List<Sash>();

			val = GetSashes(frame);
			childVal = frame.Child.GetSashes(includeChildren);

			if (childVal != null) foreach (Sash item in childVal) val.Add(item);

			return val;
		}
		#endregion

		#region object
		public static IList<Sash> GetSashes(this object obj)
		{
			IList<Sash> val = new List<Sash>();

			if (obj is Frame)
			{
				val = GetSashes((Frame)obj);
			}
			else if (obj is Mullion)
			{
				val = GetSashes((Mullion)obj);
			}
			else if (obj is Sash)
			{
				val = GetSashes((Sash)obj);
			}
			else if (obj is Filling)
			{
				val = GetSashes((Filling)obj);
			}
			else if (obj is Surface)
			{
				val = GetSashes((Surface)obj);
			}
			else if (obj is IFrame)
			{
				val = GetSashes(((IFrame)obj).ToFrame());
			}
			else if (obj is IMullion)
			{
				val = GetSashes(((IMullion)obj).ToMullion());
			}
			else if (obj is ISash)
			{
				val = GetSashes(((ISash)obj).ToSash());
			}
			else if (obj is IFilling)
			{
				val = GetSashes(((IFilling)obj).ToFilling());
			}
			else if (obj is ISurface)
			{
				val = GetSashes(((ISurface)obj).ToSurface());
			}

			return val;
		}

		public static IList<Sash> GetSashes(this object obj, bool includeChildren)
		{
			IList<Sash> val = new List<Sash>();

			if (obj is Frame)
			{
				val = GetSashes((Frame)obj, includeChildren);
			}
			else if (obj is Mullion)
			{
				val = GetSashes((Mullion)obj, includeChildren);
			}
			else if (obj is Sash)
			{
				val = GetSashes((Sash)obj, includeChildren);
			}
			else if (obj is Filling)
			{
				val = GetSashes((Filling)obj, includeChildren);
			}
			else if (obj is Surface)
			{
				val = GetSashes((Surface)obj, includeChildren);
			}
			else if (obj is IFrame)
			{
				val = GetSashes(((IFrame)obj).ToFrame(), includeChildren);
			}
			else if (obj is IMullion)
			{
				val = GetSashes(((IMullion)obj).ToMullion(), includeChildren);
			}
			else if (obj is ISash)
			{
				val = GetSashes(((ISash)obj).ToSash(), includeChildren);
			}
			else if (obj is IFilling)
			{
				val = GetSashes(((IFilling)obj).ToFilling(), includeChildren);
			}
			else if (obj is ISurface)
			{
				val = GetSashes(((ISurface)obj).ToSurface(), includeChildren);
			}

			return val;
		}
		#endregion

		#region Surface
		public static IList<Sash> GetSashes(this Surface surface)
		{
			IList<Sash> val = new List<Sash>();

			return val;
		}

		public static IList<Sash> GetSashes(this Surface surface, bool includeChildren)
		{
			IList<Sash> val = new List<Sash>(), childVal = new List<Sash>();
			val = GetSashes(surface);
			if (surface.Children != null) foreach (object item in surface.Children)
				{
					IList<Sash> tmp = new List<Sash>();
					tmp = GetSashes(item, includeChildren);
					if (tmp != null) foreach (Sash tmpItem in tmp) val.Add(tmpItem);
				}

			return val;
		}
		#endregion

		#region Mullion
		public static IList<Sash> GetSashes(this Mullion mullion)
		{
			IList<Sash> val = new List<Sash>();

			return val;
		}

		public static IList<Sash> GetSashes(this Mullion mullion, bool includeChildren)
		{
			IList<Sash> val = new List<Sash>();

			val = GetSashes(mullion);

			return val;
		}
		#endregion

		#region Sash
		public static IList<Sash> GetSashes(this Sash sash)
		{
			IList<Sash> val = new List<Sash>();

			val.Add(sash);

			return val;
		}

		public static IList<Sash> GetSashes(this Sash sash, bool includeChildren)
		{
			IList<Sash> val = new List<Sash>(), childVal = new List<Sash>();

			val = GetSashes(sash);
			childVal = sash.Child.GetSashes(includeChildren);

			if (childVal != null) foreach (Sash item in childVal) val.Add(item);

			return val;
		}
		#endregion

		#region Filling
		public static IList<Sash> GetSashes(this Filling filling)
		{
			IList<Sash> val = new List<Sash>();

			return val;
		}

		public static IList<Sash> GetSashes(this Filling filling, bool includeChildren)
		{
			IList<Sash> val = new List<Sash>();

			val = GetSashes(filling);

			return val;
		}
		#endregion
		#endregion

		#region GetMullions
		#region Frame
		public static IList<Mullion> GetMullions(this Frame frame)
		{
			IList<Mullion> val = new List<Mullion>();

			return val;
		}

		public static IList<Mullion> GetMullions(this Frame frame, bool includeChildren)
		{
			IList<Mullion> val = new List<Mullion>(), childVal = new List<Mullion>();

			val = GetMullions(frame);
			childVal = frame.Child.GetMullions(includeChildren);

			if (childVal != null) foreach (Mullion item in childVal) val.Add(item);

			return val;
		}
		#endregion

		#region object
		public static IList<Mullion> GetMullions(this object obj)
		{
			IList<Mullion> val = new List<Mullion>();

			if (obj is Frame)
			{
				val = GetMullions((Frame)obj);
			}
			else if (obj is Mullion)
			{
				val = GetMullions((Mullion)obj);
			}
			else if (obj is Sash)
			{
				val = GetMullions((Sash)obj);
			}
			else if (obj is Filling)
			{
				val = GetMullions((Filling)obj);
			}
			else if (obj is Surface)
			{
				val = GetMullions((Surface)obj);
			}
			else if (obj is IFrame)
			{
				val = GetMullions(((IFrame)obj).ToFrame());
			}
			else if (obj is IMullion)
			{
				val = GetMullions(((IMullion)obj).ToMullion());
			}
			else if (obj is ISash)
			{
				val = GetMullions(((ISash)obj).ToSash());
			}
			else if (obj is IFilling)
			{
				val = GetMullions(((IFilling)obj).ToFilling());
			}
			else if (obj is ISurface)
			{
				val = GetMullions(((ISurface)obj).ToSurface());
			}

			return val;
		}

		public static IList<Mullion> GetMullions(this object obj, bool includeChildren)
		{
			IList<Mullion> val = new List<Mullion>();

			if (obj is Frame)
			{
				val = GetMullions((Frame)obj, includeChildren);
			}
			else if (obj is Mullion)
			{
				val = GetMullions((Mullion)obj, includeChildren);
			}
			else if (obj is Sash)
			{
				val = GetMullions((Sash)obj, includeChildren);
			}
			else if (obj is Filling)
			{
				val = GetMullions((Filling)obj, includeChildren);
			}
			else if (obj is Surface)
			{
				val = GetMullions((Surface)obj, includeChildren);
			}
			else if (obj is IFrame)
			{
				val = GetMullions(((IFrame)obj).ToFrame(), includeChildren);
			}
			else if (obj is IMullion)
			{
				val = GetMullions(((IMullion)obj).ToMullion(), includeChildren);
			}
			else if (obj is ISash)
			{
				val = GetMullions(((ISash)obj).ToSash(), includeChildren);
			}
			else if (obj is IFilling)
			{
				val = GetMullions(((IFilling)obj).ToFilling(), includeChildren);
			}
			else if (obj is ISurface)
			{
				val = GetMullions(((ISurface)obj).ToSurface(), includeChildren);
			}

			return val;
		}
		#endregion

		#region Surface
		public static IList<Mullion> GetMullions(this Surface surface)
		{
			IList<Mullion> val = new List<Mullion>();

			return val;
		}

		public static IList<Mullion> GetMullions(this Surface surface, bool includeChildren)
		{
			IList<Mullion> val = new List<Mullion>(), childVal = new List<Mullion>();
			val = GetMullions(surface);
			if (surface.Children != null) foreach (object item in surface.Children)
				{
					IList<Mullion> tmp = new List<Mullion>();
					tmp = GetMullions(item, includeChildren);
					if (tmp != null) foreach (Mullion tmpItem in tmp) val.Add(tmpItem);
				}

			return val;
		}
		#endregion

		#region Mullion
		public static IList<Mullion> GetMullions(this Mullion mullion)
		{
			IList<Mullion> val = new List<Mullion>();

			val.Add(mullion);

			return val;
		}

		public static IList<Mullion> GetMullions(this Mullion mullion, bool includeChildren)
		{
			IList<Mullion> val = new List<Mullion>();

			val = GetMullions(mullion);

			return val;
		}
		#endregion

		#region Sash
		public static IList<Mullion> GetMullions(this Sash sash)
		{
			IList<Mullion> val = new List<Mullion>();

			return val;
		}

		public static IList<Mullion> GetMullions(this Sash sash, bool includeChildren)
		{
			IList<Mullion> val = new List<Mullion>(), childVal = new List<Mullion>();

			val = GetMullions(sash);
			childVal = sash.Child.GetMullions(includeChildren);

			if (childVal != null) foreach (Mullion item in childVal) val.Add(item);

			return val;
		}
		#endregion

		#region Filling
		public static IList<Mullion> GetMullions(this Filling filling)
		{
			IList<Mullion> val = new List<Mullion>();

			return val;
		}

		public static IList<Mullion> GetMullions(this Filling filling, bool includeChildren)
		{
			IList<Mullion> val = new List<Mullion>();

			val = GetMullions(filling);

			return val;
		}
		#endregion
		#endregion

		#region GetSurfaces
		#region Frame
		public static IList<Surface> GetSurfaces(this Frame frame)
		{
			IList<Surface> val = new List<Surface>();

			return val;
		}

		public static IList<Surface> GetSurfaces(this Frame frame, bool includeChildren)
		{
			IList<Surface> val = new List<Surface>(), childVal = new List<Surface>();

			val = GetSurfaces(frame);
			childVal = frame.Child.GetSurfaces(includeChildren);

			if (childVal != null) foreach (Surface item in childVal) val.Add(item);

			return val;
		}
		#endregion

		#region object
		public static IList<Surface> GetSurfaces(this object obj)
		{
			IList<Surface> val = new List<Surface>();

			if (obj is Sash)
			{
				val = GetSurfaces((Sash)obj);
			}
			else if (obj is Surface)
			{
				val = GetSurfaces((Surface)obj);
			}
			else if (obj is ISash)
			{
				val = GetSurfaces(((ISash)obj).ToSash());
			}
			else if (obj is ISurface)
			{
				val = GetSurfaces(((ISurface)obj).ToSurface());
			}

			return val;
		}

		public static IList<Surface> GetSurfaces(this object obj, bool includeChildren)
		{
			IList<Surface> val = new List<Surface>();

			if (obj is Sash)
			{
				val = GetSurfaces((Sash)obj, includeChildren);
			}
			else if (obj is Surface)
			{
				val = GetSurfaces((Surface)obj, includeChildren);
			}
			else if (obj is ISash)
			{
				val = GetSurfaces(((ISash)obj).ToSash(), includeChildren);
			}
			else if (obj is ISurface)
			{
				val = GetSurfaces(((ISurface)obj).ToSurface(), includeChildren);
			}

			return val;
		}
		#endregion

		#region Surface
		public static IList<Surface> GetSurfaces(this Surface surface)
		{
			IList<Surface> val = new List<Surface>();

			val.Add(surface);

			return val;
		}

		public static IList<Surface> GetSurfaces(this Surface surface, bool includeChildren)
		{
			IList<Surface> val = new List<Surface>(), childVal = new List<Surface>();
			val = GetSurfaces(surface);
			if (surface.Children != null) foreach (object item in surface.Children)
				{
					IList<Surface> tmp = new List<Surface>();
					tmp = GetSurfaces(item, includeChildren);
					if (tmp != null) foreach (Surface tmpItem in tmp) val.Add(tmpItem);
				}

			return val;
		}
		#endregion

		#region Sash
		public static IList<Surface> GetSurfaces(this Sash sash)
		{
			IList<Surface> val = new List<Surface>();

			return val;
		}

		public static IList<Surface> GetSurfaces(this Sash sash, bool includeChildren)
		{
			IList<Surface> val = new List<Surface>(), childVal = new List<Surface>();

			val = GetSurfaces(sash);
			childVal = sash.Child.GetSurfaces(includeChildren);

			if (childVal != null) foreach (Surface item in childVal) val.Add(item);

			return val;
		}
		#endregion
		#endregion

		#region GetMuntins
		#region Frame
		public static IList<Muntin> GetMuntins(this Frame frame)
		{
			IList<Muntin> val = new List<Muntin>();

			if (frame.Muntins != null)
				foreach (var item in frame.Muntins)
					val.Add((Muntin)item);

			return val;
		}
		public static IList<Muntin> GetMuntins(this IFrame frame)
		{
			IList<Muntin> val = new List<Muntin>();
			frame = frame.ToFrame();
			if (frame.Muntins != null)
				foreach (var item in frame.Muntins)
					val.Add((Muntin)item);

			return val;
		}
		#endregion
		#endregion

		#region GetFrame
		#region Frame
		public static Frame GetFrame(this Frame frame)
		{
			Frame val = frame;

			return val;
		}
		#endregion

		#region object
		public static Frame GetFrame(this object obj)
		{
			Frame val = new Frame();

			if (obj is Frame)
			{
				val = GetFrame((Frame)obj);
			}
			else if (obj is Mullion)
			{
				val = GetFrame((Mullion)obj);
			}
			else if (obj is Sash)
			{
				val = GetFrame((Sash)obj);
			}
			else if (obj is Filling)
			{
				val = GetFrame((Filling)obj);
			}
			else if (obj is Surface)
			{
				val = GetFrame((Surface)obj);
			}
			else if (obj is IFrame)
			{
				val = GetFrame(((IFrame)obj).ToFrame());
			}
			else if (obj is IMullion)
			{
				val = GetFrame(((IMullion)obj).ToMullion());
			}
			else if (obj is ISash)
			{
				val = GetFrame(((ISash)obj).ToSash());
			}
			else if (obj is IFilling)
			{
				val = GetFrame(((IFilling)obj).ToFilling());
			}
			else if (obj is ISurface)
			{
				val = GetFrame(((ISurface)obj).ToSurface());
			}

			return val;
		}
		#endregion

		#region Surface
		public static Frame GetFrame(this Surface surface)
		{
			Frame val = surface.Parent.GetFrame();

			return val;
		}
		#endregion

		#region Mullion
		public static Frame GetFrame(this Mullion mullion)
		{
			Frame val = mullion.Parent.GetFrame();

			return val;
		}
		#endregion

		#region Sash
		public static Frame GetFrame(this Sash sash)
		{
			Frame val = sash.Parent.GetFrame();

			return val;
		}
		#endregion

		#region Filling
		public static Frame GetFrame(this Filling filling)
		{
			Frame val = filling.Parent.GetFrame();

			return val;
		}
		#endregion
		#endregion

		#endregion

		#region GetPrice, Square, Weight, etc
		#region Profiles

		public static Price GetPrice(this Profile item)
		{
			Price val = item.ProfileType.PricePerLength * item.Length;
			return val;
		}

		public static Price GetPrice(this IList<Profile> items)
		{
			Price val = new Price();
			foreach (var item in items)
				val += item.ProfileType.PricePerLength * item.Length;

			return val;
		}

		public static float GetWeight(this Profile item)
		{
			float val = item.ProfileType.WeightPerLength * item.Length;
			return val;
		}

		public static float GetWeight(this  IList<Profile> items)
		{
			float val = 0;
			foreach (var item in items) val += item.ProfileType.WeightPerLength * item.Length;
			return val;
		}
		#endregion

		#region ProfileTypes

		public static Price GetPrice(this ProfileType item, float qty, PricingType pricingType = PricingType.PerLength)
		{
			Price val = new Price();
			switch (pricingType)
			{
				case PricingType.PerLength:
					val = item.PricePerLength * qty;
					break;
				case PricingType.PerWeight:
					val = item.PricePerWeight * qty;
					break;
				case PricingType.PerSquare:
					break;
				case PricingType.PerVolume:
					break;
				case PricingType.PerItem:
					val = item.Price * qty;
					break;
				default:
					break;
			}
			return val;
		}

		public static Price GetPrice(this IList<ProfileType> items, float qty, PricingType pricingType = PricingType.PerLength)
		{
			Price val = new Price();
			foreach (var item in items)
				val += GetPrice(item, qty, pricingType);

			return val;
		}

		#endregion

		#region Accessories

		public static Price GetPrice(this Accessory item)
		{
			Price val = item.AccessoryType.Price * item.Quantity;
			return val;
		}

		public static Price GetPrice(this IList<Accessory> items)
		{
			Price val = new Price();
			foreach (var item in items)
				if (item.AccessoryType != null && item.AccessoryType.Price != null)
					val += item.AccessoryType.Price * item.Quantity;

			return val;
		}

		#endregion

		#region AccessoryTypes

		public static Price GetPrice(this AccessoryType item, float qty, PricingType pricingType = PricingType.PerItem)
		{
			Price val = new Price();
			switch (pricingType)
			{
				case PricingType.PerItem:
					val = item.Price * qty;
					break;
				default:
					break;
			}
			return val;
		}

		public static Price GetPrice(this IList<AccessoryType> items, float qty, PricingType pricingType = PricingType.PerLength)
		{
			Price val = new Price();
			foreach (var item in items)
				val += GetPrice(item, qty, pricingType);

			return val;
		}

		#endregion

		#region Fillings
		public static Price GetPrice(this Filling item, bool includeTolerance = true, bool includeChildren = true, bool includePeripherals = false)
		{
			Price val = new Price();
			val = item.FillingType.PricePerSquare * GetSquare(item, includeTolerance);
			if (includePeripherals)
			{
				val += GetPrice(item.GetProfiles(includeTolerance));
				val += GetPrice(item.GetAccessories());
			}

			return val;
		}

		public static Price GetPrice(this IList<Filling> items, bool includeTolerance = true, bool includeChildren = true, bool includePeripherals = false)
		{
			Price val = new Price();
			foreach (var item in items)
				val += GetPrice(item, includeTolerance, includeChildren, includePeripherals);

			return val;
		}

		public static float GetSquare(this Filling item, bool includeTolerance = true)
		{
			return GetWidth(item, includeTolerance) * GetHeight(item, includeTolerance);
		}
		public static float GetWidth(this Filling item, bool includeTolerance = true)
		{
			float x = includeTolerance ? item.Parent.Width + item.FillingType.Tolerance : item.Parent.Width;
			return x;
		}
		public static float GetHeight(this Filling item, bool includeTolerance = true)
		{
			float x = includeTolerance ? item.Parent.Height + item.FillingType.Tolerance : item.Parent.Height;
			return x;
		}
		public static float GetWeight(this Filling item, bool includeTolerance, bool includePeripherals)
		{
			float w = 0;
			if (includePeripherals)
			{
				w = GetProfiles(item, includeTolerance).GetWeight();
			}

			return w + item.FillingType.WeightPerSquare * GetSquare(item, includeTolerance);
		}
		public static float GetWeight(this IList<Filling> items, bool includeTolerance, bool includePeripherals)
		{
			float w = 0;
			foreach (var item in items)
			{
				w += GetWeight(item, includeTolerance, includePeripherals);
			}
			return w;
		}
		#endregion

		#region Sashes
		public static Price GetPrice(this Sash item, bool includeTolerance = true, bool includeChildren = true, bool includePeripherals = false)
		{
			Price val = new Price();

			val = GetPrice(item.GetProfiles(includeTolerance));
			val += GetPrice(item.GetAccessories());
			if (includeChildren)
				val += GetPrice(item.Child, includeTolerance, includeChildren, includePeripherals);

			return val;
		}

		public static Price GetPrice(this IList<Sash> items, bool includeTolerance = true, bool includeChildren = true, bool includePeripherals = false)
		{
			Price val = new Price();
			foreach (var item in items)
				val += GetPrice(item, includeTolerance, includeChildren, includePeripherals);

			return val;
		}

		public static float GetSquare(this Sash item, bool includeTolerance = true)
		{
			return GetWidth(item, includeTolerance) * GetHeight(item, includeTolerance);
		}
		public static float GetWidth(this Sash item, bool includeTolerance = true)
		{
			return includeTolerance ? item.Parent.Width + item.SashType.Tolerance : item.Parent.Width;
		}
		public static float GetHeight(this Sash item, bool includeTolerance = true)
		{
			return includeTolerance ? item.Parent.Height + item.SashType.Tolerance : item.Parent.Height;
		}
		public static float GetWeight(this Sash item, bool includeTolerance, bool includeChildren)
		{
			float w;
			w = GetProfiles(item, includeTolerance, includeChildren).GetWeight();
			if (includeChildren)
			{
				w += GetWeight(item.Child, includeTolerance, includeChildren);
			}

			return w;
		}
		public static float GetWeight(this IList<Sash> items, bool includeTolerance, bool includeChildren)
		{
			float w = 0;
			foreach (var item in items)
			{
				w += GetWeight(item, includeTolerance, includeChildren);
			}

			return w;
		}

		#endregion

		#region Surfaces
		public static Price GetPrice(this Surface item, bool includeTolerance = true, bool includeChildren = true, bool includePeripherals = false)
		{
			Price val = new Price();

			val = GetPrice(item.GetProfiles(includeTolerance));
			val += GetPrice(item.GetAccessories());
			if (includeChildren)
				foreach (var itm in item.Children)
					val += GetPrice(itm, includeTolerance, includeChildren, includePeripherals);

			return val;
		}

		public static Price GetPrice(this IList<Surface> items, bool includeTolerance = true, bool includeChildren = true, bool includePeripherals = false)
		{
			Price val = new Price();
			foreach (var item in items)
				val += GetPrice(item, includeTolerance, includeChildren, includePeripherals);

			return val;
		}

		public static float GetSquare(this Surface item)
		{
			return item.Width * item.Height;
		}
		public static float GetWeight(this Surface item, bool includeTolerance, bool includeChildren)
		{
			float w = 0;
			if (includeChildren)
			{
				foreach (var itm in item.Children)
				{
					w += GetWeight(itm, includeTolerance, includeChildren);
				}
			}

			return w;
		}
		public static float GetWeight(this IList<Surface> items, bool includeTolerance, bool includeChildren)
		{
			float w = 0;
			foreach (var item in items)
			{
				w += GetWeight(item, includeTolerance, includeChildren);
			}

			return w;
		}
		#endregion

		#region Mullions

		public static Price GetPrice(this Mullion item, bool includeTolerance = true)
		{
			Price val = GetProfiles(item, includeTolerance).GetPrice();
			return val;
		}

		public static Price GetPrice(this IList<Mullion> items, bool includeTolerance = true)
		{
			Price val = new Price();
			foreach (var item in items)
				val += GetPrice(item, includeTolerance);

			return val;
		}

		public static float GetWeight(this Mullion item, bool includeTolerance = true)
		{
			float val = GetProfiles(item, includeTolerance).GetWeight();
			return val;
		}

		public static float GetWeight(this  IList<Mullion> items, bool includeTolerance = true)
		{
			float val = 0;
			foreach (var item in items) val += GetWeight(item, includeTolerance);
			return val;
		}
		#endregion

		#region Muntins

		public static Price GetPrice(this Muntin item)
		{
			Price val = item.MuntinType.PricePerSquare * item.GetSquare();
			return val;
		}

		public static Price GetPrice(this IList<Muntin> items)
		{
			Price val = new Price();
			foreach (var item in items)
				val += GetPrice(item);

			return val;
		}

		public static float GetSquare(this Muntin item)
		{
			return item.Width * item.Height;
		}

		public static float GetWeight(this Muntin item)
		{
			return item.MuntinType.WeightPerSquare * item.GetSquare();
		}

		public static float GetWeight(this IList<Muntin> items)
		{
			float val = 0;
			foreach (var item in items)
			{
				val += GetWeight(item);
			}

			return val;
		}

		#endregion

		#region Objects
		public static Price GetPrice(this Object obj, bool includeTolerance = true, bool includeChildren = true, bool includePeripherals = false)
		{
			Price val = new Price();

			if (obj is Frame)
			{
				val = GetPrice((Frame)obj, includeTolerance, includeChildren, includePeripherals);
			}
			else if (obj is Mullion)
			{
				val = GetPrice((Mullion)obj, includeTolerance);
			}
			else if (obj is Sash)
			{
				val = GetPrice((Sash)obj, includeTolerance, includeChildren, includePeripherals);
			}
			else if (obj is Filling)
			{
				val = GetPrice((Filling)obj, includeTolerance, includeChildren, includePeripherals);
			}
			else if (obj is Surface)
			{
				val = GetPrice((Surface)obj, includeTolerance, includeChildren, includePeripherals);
			}
			else if (obj is IFrame)
			{
				val = GetPrice(((IFrame)obj).ToFrame(), includeTolerance, includeChildren, includePeripherals);
			}
			else if (obj is IMullion)
			{
				val = GetPrice(((IMullion)obj).ToMullion(), includeTolerance);
			}
			else if (obj is ISash)
			{
				val = GetPrice(((ISash)obj).ToSash(), includeTolerance, includeChildren, includePeripherals);
			}
			else if (obj is IFilling)
			{
				val = GetPrice(((IFilling)obj).ToFilling(), includeTolerance, includeChildren, includePeripherals);
			}
			else if (obj is ISurface)
			{
				val = GetPrice(((ISurface)obj).ToSurface(), includeTolerance, includeChildren, includePeripherals);
			}

			return val;
		}

		public static Price GetPrice(this IList<Object> items, bool includeTolerance = true, bool includeChildren = true, bool includePeripherals = false)
		{
			Price val = new Price();
			foreach (var item in items)
				val += GetPrice(item, includeTolerance, includeChildren, includePeripherals);

			return val;
		}

		public static float GetSquare(this Object obj)
		{
			float val = 0;

			if (obj is Frame)
			{
				val = GetSquare((Frame)obj);
			}
			else if (obj is Mullion)
			{
			}
			else if (obj is Sash)
			{
				val = GetSquare((Sash)obj);
			}
			else if (obj is Filling)
			{
				val = GetSquare((Filling)obj);
			}
			else if (obj is Surface)
			{
				val = GetSquare((Surface)obj);
			}
			else if (obj is IFrame)
			{
				val = GetSquare(((IFrame)obj).ToFrame());
			}
			else if (obj is IMullion)
			{
			}
			else if (obj is ISash)
			{
				val = GetSquare(((ISash)obj).ToSash());
			}
			else if (obj is IFilling)
			{
				val = GetSquare(((IFilling)obj).ToFilling());
			}
			else if (obj is ISurface)
			{
				val = GetSquare(((ISurface)obj).ToSurface());
			}

			return val;
		}
		public static float GetWeight(this Object obj, bool includeTolerance, bool includeChildren)
		{
			float val = 0;

			if (obj is Frame)
			{
				val = GetWeight((Frame)obj, includeTolerance, includeChildren);
			}
			else if (obj is Mullion)
			{
				val = GetWeight((Mullion)obj, includeTolerance);
			}
			else if (obj is Sash)
			{
				val = GetWeight((Sash)obj, includeTolerance, includeChildren);
			}
			else if (obj is Filling)
			{
				val = GetWeight((Filling)obj, includeTolerance, true);
			}
			else if (obj is Surface)
			{
				val = GetWeight((Surface)obj, includeTolerance, includeChildren);
			}
			else if (obj is Muntin)
			{
				val = GetWeight((Muntin)obj);
			}
			else if (obj is IFrame)
			{
				val = GetWeight(((IFrame)obj).ToFrame(), includeTolerance, includeChildren);
			}
			else if (obj is IMullion)
			{
				val = GetWeight(((IMullion)obj).ToMullion(), includeTolerance);
			}
			else if (obj is ISash)
			{
				val = GetWeight(((ISash)obj).ToSash(), includeTolerance, includeChildren);
			}
			else if (obj is IFilling)
			{
				val = GetWeight(((IFilling)obj).ToFilling(), includeTolerance, true);
			}
			else if (obj is ISurface)
			{
				val = GetWeight(((ISurface)obj).ToSurface(), includeTolerance, includeChildren);
			}
			else if (obj is IMuntin)
			{
				val = GetWeight(((IMuntin)obj).ToMuntin());
			}

			return val;
		}
		public static float GetWeight(this IList<Object> items, bool includeTolerance, bool includeChildren)
		{
			float w = 0;
			foreach (var item in items)
			{
				w += GetWeight(item, includeTolerance, includeChildren);
			}

			return w;
		}
		#endregion

		#region Frames
		public static Price GetPrice(this Frame item, bool includeTolerance = true, bool includeChildren = true, bool includePeripherals = true)
		{
			Price val = new Price();

			val = GetPrice(item.GetProfiles(includeTolerance));
			if (includePeripherals)
			{
				val += GetPrice(item.Accessories);
				val += GetPrice(item.Muntins);
			}
			if (includeChildren)
				val += GetPrice(item.Child, includeTolerance, includeChildren, includePeripherals);

			return val;
		}
		public static Price GetPrice(this IList<Frame> items, bool includeTolerance = true, bool includeChildren = true, bool includePeripherals = true)
		{
			Price val = new Price();
			foreach (var item in items)
				val += GetPrice(item, includeTolerance, includeChildren, includePeripherals);

			return val;
		}
		public static float GetSquare(this Frame item)
		{
			return item.Width * item.Height;
		}
		public static float GetWeight(this Frame item, bool includeTolerance = true, bool includeChildren = true, bool includePeripherals = true)
		{
			float w;
			w = GetProfiles(item, includeTolerance).GetWeight();
			if (includePeripherals)
			{
				w += GetWeight(item.Muntins.ToMuntins());
			}
			if (includeChildren)
				w += GetWeight(item.Child, includeTolerance, includeChildren);

			return w;
		}
		public static float GetWeight(this IList<Frame> items, bool includeTolerance = true, bool includeChildren = true, bool includePeripherals = true)
		{
			float w = 0;
			foreach (var item in items)
			{
				w += GetWeight(item, includeTolerance, includeChildren, includePeripherals);
			}

			return w;
		}
		#endregion

		public static float SquareMeter(this Unit item)
		{
			return (item.Width * item.Height).SquareMillimeterToMeter();
		}

		#endregion

		#region Convert Scaler Units
		#region Millimeter
		public static float MillimeterToCentimeter(this float number)
		{
			return number / 10;
		}
		public static float MillimeterToDecimeter(this float number)
		{
			return number / 100;
		}
		public static float MillimeterToMeter(this float number)
		{
			return number / 1000;
		}
		public static float MillimeterToInch(this float number)
		{
			return number / 25.4F;
		}
		public static float MillimeterToFoot(this float number)
		{
			return number / 304.8F;
		}
		public static float CentimeterToMillimeter(this float number)
		{
			return number * 10;
		}
		public static float DecimeterToMillimeter(this float number)
		{
			return number * 100;
		}
		public static float MeterToMillimeter(this float number)
		{
			return number * 1000;
		}
		public static float InchToMillimeter(this float number)
		{
			return number * 25.4F;
		}
		public static float FootToMillimeter(this float number)
		{
			return number * 304.8F;
		}
		#endregion

		#region Square Millimeter
		public static float SquareMillimeterToCentimeter(this float number)
		{
			return number / 100;
		}
		public static float SquareMillimeterToDecimeter(this float number)
		{
			return number / 10000;
		}
		public static float SquareMillimeterToMeter(this float number)
		{
			return number / 1000000;
		}
		public static float SquareCentimeterToMillimeter(this float number)
		{
			return number * 100;
		}
		public static float SquareDecimeterToMillimeter(this float number)
		{
			return number * 10000;
		}
		public static float SquareMeterToMillimeter(this float number)
		{
			return number * 1000000;
		}
		#endregion

		#region Weight
		public static float KilogramToGram(this float number)
		{
			return number * 1000;
		}
		public static float KilogramToTon(this float number)
		{
			return number / 1000;
		}
		public static float GramToKilogram(this float number)
		{
			return number / 1000;
		}
		public static float TonToKilogram(this float number)
		{
			return number * 1000;
		}
		#endregion
		#endregion

		#region Bytes Conversions
		public static byte[] ToBytes(this Image image)
		{
			MemoryStream stream = new MemoryStream();
			try
			{
				BinaryFormatter bFormatter = new BinaryFormatter();

				bFormatter.Serialize(stream, image);
				return stream.ToArray();
			}
			catch (Exception e)
			{
				return null;
			}
			finally
			{
				stream.Close();
			}
		}

		public static Image ToImage(this byte[] bytes)
		{
			MemoryStream stream = new MemoryStream(bytes);
			try
			{
				BinaryFormatter bformatter = new BinaryFormatter();
				return (Image)bformatter.Deserialize(stream);
			}
			catch (Exception e)
			{
				return null;
			}
			finally
			{
				stream.Close();
			}
		}

		public static string GetMimeType(this Image image)
		{
			foreach (ImageCodecInfo codec in ImageCodecInfo.GetImageDecoders())
			{
				if (codec.FormatID == image.RawFormat.Guid)
					return codec.MimeType;
			}

			return "image/unknown";
		}

		#endregion
	}
}

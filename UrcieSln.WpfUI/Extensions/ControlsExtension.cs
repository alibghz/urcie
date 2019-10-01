using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace UrcieSln.WpfUI.Extensions
{
	public static class ControlsExtension
	{
		public static DependencyObject FindChildControl<T>(this DependencyObject control, string ctrlName)
		{
			int childNumber = VisualTreeHelper.GetChildrenCount(control);
			for (int i = 0; i < childNumber; i++)
			{
				DependencyObject child = VisualTreeHelper.GetChild(control, i);
				FrameworkElement fe = child as FrameworkElement;
				if (fe == null) return null;

				if (child is T && fe.Name == ctrlName)
				{
					return child;
				}
				else
				{
					DependencyObject nextLevel = FindChildControl<T>(child, ctrlName);
					if (nextLevel != null) return nextLevel;
				}
			}
			return null;
		}

		public static bool IsValid(this DependencyObject node)
		{
			if (node != null)
			{
				bool isValid = !Validation.GetHasError(node);
				if (!isValid)
				{
					if (node is IInputElement) Keyboard.Focus((IInputElement)node);
					return false;
				}
			}

			foreach (object subnode in LogicalTreeHelper.GetChildren(node))
			{
				if (subnode is DependencyObject)
				{
					if (IsValid((DependencyObject)subnode) == false) return false;
				}
			}

			return true;
		}
	}
}

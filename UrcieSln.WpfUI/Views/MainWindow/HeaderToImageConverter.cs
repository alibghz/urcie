﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using UrcieSln.WpfUI.ViewModels;

namespace UrcieSln.WpfUI.Views
{

	[ValueConversion(typeof(string), typeof(bool))]
	public class HeaderToImageConverter : IValueConverter
	{
		public static HeaderToImageConverter Instance = new HeaderToImageConverter();

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			//if (value is ProjectViewModel)
			//{
			//	Uri uri = new Uri
			//	("pack://application:,,,/Resources/Icons/Mini/project.png");
			//	BitmapImage source = new BitmapImage(uri);
			//	return source;
			//}
			//else
			//{
			//	Uri uri = new Uri("pack://application:,,,/Resources/Icons/Mini/unit.png");
			//	BitmapImage source = new BitmapImage(uri);
			//	return source;
			//}
			Uri uri = new Uri("pack://application:,,,/Resources/Icons/Mini/unit.png");
			BitmapImage source = new BitmapImage(uri);
			return source;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException("Cannot convert back");
		}
	}
}

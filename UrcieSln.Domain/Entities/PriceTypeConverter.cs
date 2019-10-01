using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;

namespace UrcieSln.Domain.Entities
{
	public class PriceTypeConverter : TypeConverter
	{
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			if (sourceType == typeof(string))
				return true;

			return base.CanConvertFrom(context, sourceType);
		}

		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			if (destinationType == typeof(string))
				return true;

			return base.CanConvertTo(context, destinationType);
		}

		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			string text = value as string;
			Price p = new Price();
			p.Sale = 0;
			if(text != null)
			{
				try
				{
					float floatVal = float.Parse(text);
					p.Sale = floatVal;
					return p;
				}
				catch
				{
				}
			}
			return p;
		}

		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}

			Price p = value as Price;
			if(p != null && this.CanConvertTo(context, destinationType))
			{
				return p.Sale.ToString();
			}

			return "0";
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace UrcieSln.WpfUI.Validations
{
	public class ThicknessRule : ValidationRule
	{
		public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
		{
			try
			{
				int thickness = int.Parse((string)value);
				if (thickness <= 0)
				{
					return new ValidationResult(false, "Invalid value.");
				}
				return ValidationResult.ValidResult;
			}
			catch 
			{
				return new ValidationResult(false, "Only numbers are valid.");
			}
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace UrcieSln.WpfUI.Validations
{
	public class NameRule : ValidationRule
	{
		public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
		{
			string name = (string) value;
			if (string.IsNullOrWhiteSpace(name))
			{
				return new ValidationResult(false, "Field is required.");
			}
			return ValidationResult.ValidResult;
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using UrcieSln.WpfUI.Views.Materials;

namespace UrcieSln.WpfUI.Validations
{
	public class ProfileRule : ValidationRule
	{
		public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
		{
			ProfileTypeViewModel profileTypeViewModel = value as ProfileTypeViewModel;
			if (profileTypeViewModel == null)
				return new ValidationResult(false, "Profile type is required.");

			return ValidationResult.ValidResult;
		}
	}
}

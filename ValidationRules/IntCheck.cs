using System.Globalization;
using System.Windows.Controls;

namespace SlideShowMaker.ValidationRules
{
    class IntCheck : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo) =>
            int.TryParse((string)value, out var _) ? ValidationResult.ValidResult : new ValidationResult(false, "Значение должно быть целым числом.");
    }
}

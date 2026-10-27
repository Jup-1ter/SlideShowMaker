using System.Globalization;
using System.Windows.Controls;

namespace SlideShowMaker.ValidationRules
{
    class NotEmptyRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (string.IsNullOrWhiteSpace(value?.ToString()))
            {
                return new ValidationResult(false, "Поле обязательно для заполнения.");
            }
            return ValidationResult.ValidResult;
        }
    }
}
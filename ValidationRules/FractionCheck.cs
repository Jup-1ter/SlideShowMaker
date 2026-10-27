using System.Globalization;
using System.Windows.Controls;

namespace SlideShowMaker.ValidationRules
{
    class FractionCheck : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var input = ((string)value).Trim().Replace('.', ',');

            if (input.Contains('/'))
            {
                var parts = input.Split('/');
                if (parts.Length == 2)
                {
                    if (long.TryParse(parts[0].Trim(), out var _) &&
                        long.TryParse(parts[1].Trim(), out var denominator))
                    {
                        if (denominator == 0) return new ValidationResult(false, "Делитель не может быть 0.");
                    }
                    else return new ValidationResult(false, "Делитель и/или знаменатель введен(ы) неверно");
                }
                else return new ValidationResult(false, "Дробь введена неверно");
            }
            else if (!double.TryParse(input, out var _)) return new ValidationResult(false, "Десятичная дробь введена неверно");
            return ValidationResult.ValidResult;
        }
    }
}

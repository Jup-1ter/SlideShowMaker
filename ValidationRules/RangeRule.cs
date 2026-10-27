using System.Globalization;
using System.Windows.Controls;

namespace SlideShowMaker.ValidationRules
{
    class RangeRule : ValidationRule
    {
        public double Min { get; set; } = double.MinValue;
        public double Max { get; set; } = double.MaxValue;

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            try
            {
                var val = Convert.ToDouble(value);
                if (val < Min || val > Max)
                    return new ValidationResult(false,
                        val.CompareTo(Min) < 0 && Max == float.MaxValue ? $"Значение должно быть больше {Min}." :
                        val.CompareTo(Max) > 0 && Min == float.MinValue ? $"Значение должно быть меньше {Max}." :
                        $"Значение должно быть в дипазоне {Min}-{Max}.");
                return ValidationResult.ValidResult;
            }
            catch 
            {
                throw new ArgumentException("Правило предназначено только для объектов сравнимых с double. Предоставлен " + value.GetType());
            }
        }
    }
}

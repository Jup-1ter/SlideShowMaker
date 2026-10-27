using System.Globalization;
using System.Windows.Data;

namespace SlideShowMaker.Converters
{
    class FractionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object? parameter, CultureInfo culture) =>
            value is Fraction f ?
                targetType.IsAssignableFrom(typeof(double)) ? f.Value :
                targetType.IsAssignableFrom(typeof(string)) ? f.ToString() :
                throw new NotImplementedException("Конвертер предназначен только для string и double. Требуется " + value.GetType()) :
            throw new ArgumentException($"Конвертер предназначен только для преобразования значений типа Fraction. Передано {value.GetType()}");

        public object ConvertBack(object value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is string s)
            {
                if (string.IsNullOrEmpty(s)) throw new FormatException("Строка пуста");
                s = s.Trim().Replace('.', ',');

                if (s.Contains('/'))
                {
                    var parts = s.Split('/');
                    return new Fraction(long.Parse(parts[0].Trim()), long.Parse(parts[1].Trim()));
                }
                else return Fraction.FromDouble(double.Parse(s));
            }
            else if (value is double d) return Fraction.FromDouble(parameter is string p && int.TryParse(p, out var r) ? double.Round(d, r) : d);
            else throw new FormatException("Конвертер предназначен только для string и double. Передан " + value.GetType());
        }
    }
}

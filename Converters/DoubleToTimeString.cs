using System.Globalization;
using System.Windows.Data;

namespace SlideShowMaker.Converters
{
    class DoubleToTimeString : IValueConverter
    {
        public object Convert(object value, Type targetType, object? parameter, CultureInfo culture) =>
            targetType.IsAssignableFrom(typeof(string)) ?
                TimeSpan.FromMilliseconds(value is double d ? d :
                    throw new ArgumentException($"Конвертер предназначен только для преобразования значений типа double. Передано {value.GetType()}")) :
            throw new NotImplementedException($"Конвертер предназначен только для преобразования значений в string. Требуется {targetType}");

        public object ConvertBack(object value, Type targetType, object? parameter, CultureInfo culture) =>
            targetType.IsAssignableFrom(typeof(double)) ?
                TimeSpan.Parse(value is string s ? s :
                    throw new ArgumentException($"Конвертер предназначен только для преобразования значений типа string. Передано {value.GetType()}"))
                .ToString().TrimEnd('0') :
            throw new NotImplementedException($"Конвертер предназначен только для преобразования значений в double. Требуется {targetType}");
    }
}

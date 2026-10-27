using System.Globalization;
using System.Windows.Data;
using System.Windows;

namespace SlideShowMaker.Converters
{
    class EqualityToBoolConverter : IValueConverter
    {
        public bool Inverse { get; set; }
        public object Convert(object value, Type targetType, object? compared, CultureInfo culture) =>
            targetType.IsAssignableFrom(typeof(bool)) ?
                compared != null ?
                    (Equals(value, Util.TryParse(compared)) ^ Inverse) :
                throw new ArgumentNullException(nameof(compared), "Для данного конвертера параметр обязателен") :
            throw new NotImplementedException($"Конвертер предназначен только для преобразования значений в bool. Требуется {targetType}");

        public object ConvertBack(object value, Type targetType, object? parameter, CultureInfo culture) =>
            throw new NotImplementedException("Конвертер не предназначен для возврата значений");
    }
}

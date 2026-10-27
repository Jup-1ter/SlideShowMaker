using System.Globalization;
using System.Windows.Data;
using System.Windows;

namespace SlideShowMaker.Converters
{
    class EqualityToVisibilityConverter : IValueConverter
    {
        public bool Inverse { get; set; }
        public object Convert(object value, Type targetType, object? compared, CultureInfo culture) =>
            targetType.IsAssignableFrom(typeof(Visibility)) ?
                compared != null ?
                    (Equals(value, Util.TryParse(compared)) ^ Inverse ? Visibility.Visible : Visibility.Collapsed) :
                throw new ArgumentNullException(nameof(compared), "Для данного конвертера параметр обязателен") :
            throw new NotImplementedException($"Конвертер предназначен только для преобразования значений в Visibity. Требуется {targetType}");

        public object ConvertBack(object value, Type targetType, object? parameter, CultureInfo culture) =>
            throw new NotImplementedException("Конвертер не предназначен для возврата значений");
    }
}

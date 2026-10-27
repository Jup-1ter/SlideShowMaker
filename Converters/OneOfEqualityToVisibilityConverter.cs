using System.Globalization;
using System.Windows.Data;
using System.Windows;
using System.Collections.ObjectModel;

namespace SlideShowMaker.Converters
{
    class OneOfEqualityToVisibilityConverter : IValueConverter
    {
        public bool Inverse { get; set; }
        public object Convert(object value, Type targetType, object? compared, CultureInfo culture) =>
            targetType.IsAssignableFrom(typeof(Visibility)) ?
                compared != null ?
                    compared is object[] variants ?
                        (variants.Any(val => Equals(value, Util.TryParse(val))) ^ Inverse ? Visibility.Visible : Visibility.Collapsed) :
                    throw new NotImplementedException($"Параметр конвертера может быть только коллекцией. Передан объект типа {compared.GetType()}") :
                throw new ArgumentNullException(nameof(compared), "Для данного конвертера параметр обязателен") :
            throw new NotImplementedException($"Конвертер предназначен только для преобразования значений в Visibity. Требуется {targetType}");

        public object ConvertBack(object value, Type targetType, object? parameter, CultureInfo culture) =>
            throw new NotImplementedException("Конвертер не предназначен для возврата значений");
    }
}

using System.Windows;
using System.Windows.Media.Imaging;

namespace SlideShowMaker
{
    static class Util
    {
        static object? Parse(string s) => s.ToLower() switch
        {
            "true" => true,
            "false" => false,
            "visible" => Visibility.Visible,
            "collapsed" => Visibility.Collapsed,
            "hidden" => Visibility.Hidden,
            "null" => null,
            _ => int.TryParse(s, out var i) ? i :
                double.TryParse(s, out var d) ? d : s
        };
        public static object? TryParse(object? o) => o is string s ? Parse(s) : o;
        public static bool ToBool(object o) {
            return o switch
            {
                bool b => b,
                Visibility.Visible => true,
                int i => i != 0,
                double d => d != 0,
                string s => !string.IsNullOrEmpty(s),
                _ => false,
            };
        }

        public static (int width, int height) GetImageResolution(string imagePath)
        {
            try
            {
                using var stream = System.IO.File.OpenRead(imagePath);
                var decoder = BitmapDecoder.Create(stream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                var frame = decoder.Frames[0];
                return (frame.PixelWidth, frame.PixelHeight);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при обработке {imagePath}: {ex.Message}");
                return (-1, -1);
            }
        }
    }
}

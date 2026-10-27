using Microsoft.Win32;
using System.Windows;
using static SlideShowMaker.Util;

namespace SlideShowMaker
{
    partial class MainWindowClass
    {
        private (int, int) _resolution;

        static readonly List<string> ImageExtensions =
        [ ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".tiff", ".tif",
            ".webp", ".heif", ".heic", ".svg", ".apng", ".avif" ];

        void ExplorerButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new()
            {
                Filter = $"Файлы Изображений|*{string.Join(";*", ImageExtensions)}",
                Multiselect = true
            };
            if (openFileDialog.ShowDialog() == true)
                foreach (var path in openFileDialog.FileNames) if (AddImage(path)) break;
        }

        void Window_Drop(object sender, DragEventArgs e)
        {
            
            if (e.Data.GetData(DataFormats.FileDrop) is string[] files)
                foreach (var path in files) if (ImageExtensions.Any(path.EndsWith))
                        if(AddImage(path)) break;
                    
        }

        void Window_DragEnter(object sender, DragEventArgs e)
        {
            var data = e.Data;
            e.Effects = (
                data != null &&
                data.GetDataPresent(DataFormats.FileDrop) &&
                ((string[])data.GetData(DataFormats.FileDrop)).
                Any(file => ImageExtensions.Any(file.EndsWith))
                ) ? DragDropEffects.Copy : DragDropEffects.None;
        }

        bool AddImage(string path)
        {
            if (ImagePaths.Count > 0)
            {
                if (!ImagePaths.Contains(path))
                    if (GetImageResolution(path) == _resolution) ImagePaths.Add(path);
                    else
                    {
                        MessageBox.Show("Изображения должны быть одинакового разрешения.", "Некорректное изображение", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return true;
                    }
            }
            else
            {
                ImagePaths.Add(path);
                _resolution = GetImageResolution(path);
            }
            return false;
        }
    }
}

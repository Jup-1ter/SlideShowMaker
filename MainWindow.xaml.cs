using System.Collections.Specialized;
using System.Windows;
using System.Windows.Media.Imaging;

namespace SlideShowMaker
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    partial class MainWindowClass : Window
    {
        internal MainWindowClass() {
            InitializeComponent();
            _ip.CollectionChanged += (_, changes) =>
            {
                switch (changes.Action)
                {
                    case NotifyCollectionChangedAction.Remove | NotifyCollectionChangedAction.Replace:
                        foreach (var path in _ip)
                        {
                            BitmapImage img = new(new(path));
                            var h = (short)img.PixelHeight;
                            var w = (short)img.PixelWidth;
                            if (h > _rh) _rh = h;
                            if (w > _rw) _rw = w;
                        }
                        OnPropertyChanged(nameof(ResultHeight));
                        OnPropertyChanged(nameof(ResultWidth));
                        break;
                    case NotifyCollectionChangedAction.Reset:
                        ResultHeight = null;
                        ResultWidth = null;
                        break;
                    case NotifyCollectionChangedAction.Add:
                        var newItems = changes.NewItems;
                        if (newItems == null) break;
                        foreach (string path in newItems)
                        {
                            BitmapImage img = new(new(path));
                            var h = (short)img.PixelHeight;
                            var w = (short)img.PixelWidth;
                            if (h > _rh) _rh = h;
                            if (w > _rw) _rw = w;
                        }
                        OnPropertyChanged(nameof(ResultHeight));
                        OnPropertyChanged(nameof(ResultWidth));
                        break;
                };
                OnPropertyChanged(nameof(Command));
            };
        }
    }
}

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace SlideShowMaker
{
    partial class MainWindowClass
    {
        Point _startPoint;

        void ClearButton_Click(object sender, RoutedEventArgs e) => ImagePaths.Clear();

        void ImageList_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e) => _startPoint = e.GetPosition(null);

        void ImageList_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            var diff = _startPoint - e.GetPosition(null);

            if (e.LeftButton == MouseButtonState.Pressed &&
                (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance) &&
                FindAncestor<ListViewItem>((DependencyObject)e.OriginalSource) is ListViewItem listViewItem)
                    DragDrop.DoDragDrop(listViewItem, new DataObject("myFormat",
                        (string)((ListView)sender).ItemContainerGenerator.ItemFromContainer(listViewItem)), DragDropEffects.Move);
        }

        void ImageList_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                var item = (string)e.Data.GetData("myFormat");
                var listView = (ListView)sender;
                var dropPosition = e.GetPosition(listView);
                var dropIndex = -1;

                for (var i = 0; i < listView.Items.Count; i++)
                {
                    var curItem = (ListViewItem)listView.ItemContainerGenerator.ContainerFromIndex(i);
                    if (curItem == null) continue;
                    
                    var bounds = VisualTreeHelper.GetDescendantBounds(curItem);
                    var itemPosition = curItem.TransformToAncestor(listView).Transform(new Point(0, 0));

                    if (dropPosition.Y < itemPosition.Y + bounds.Height / 2)
                    {
                        dropIndex = i;
                        break;
                    }
                    
                }

                if (dropIndex == -1) dropIndex = listView.Items.Count - 1;

                ImagePaths.Remove(item);
                ImagePaths.Insert(dropIndex, item);
            }
        }

        static T? FindAncestor<T>(DependencyObject current) where T : DependencyObject
        {
            while (current != null)
            {
                if (current is T cur) return cur;
                current = VisualTreeHelper.GetParent(current);
            }
            return null;
        }
    }
}

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace SpiderSolitaire.Converters
{
    public sealed class ImageResourceKeyToImageConverter : IValueConverter
    {
        private static ImageResourceKeyToImageConverter _instance;
        public static ImageResourceKeyToImageConverter Instance => _instance ??= new ImageResourceKeyToImageConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return DependencyProperty.UnsetValue;
            }

            return Application.Current.Resources[value.ToString()] as BitmapImage;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}

using System;
using System.Globalization;
using System.Windows.Data;

namespace SpiderSolitaire.Converters
{
    public sealed class CardImageConverter : IMultiValueConverter
    {
        private static CardImageConverter _instance;
        public static CardImageConverter Instance => _instance ??= new CardImageConverter();

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if(values != null && values.Length == 2
                && values[0] != null && values[1] != null
                && Enum.IsDefined(typeof(Model.Suit), values[0].ToString())
                && Enum.IsDefined(typeof(Model.CardNumber), values[1].ToString()))
            {
                return (System.Windows.Media.Imaging.BitmapImage)System.Windows.Application.Current.Resources[values[0].ToString() + values[1].ToString()];
            }

            return null;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}

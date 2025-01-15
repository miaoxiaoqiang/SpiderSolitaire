using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace SpiderSolitaire.Converters
{
    public sealed class ShowWinConverter : IValueConverter
    {
        private static ShowWinConverter _instance;
        public static ShowWinConverter Instance => _instance ??= new ShowWinConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return Visibility.Collapsed;
            }

            if(System.Text.RegularExpressions.Regex.IsMatch(value.ToString(), @"^(true|false)$", System.Text.RegularExpressions.RegexOptions.IgnoreCase | System.Text.RegularExpressions.RegexOptions.Compiled))
            {
                bool result = System.Convert.ToBoolean(value);

                return result ? Visibility.Visible : Visibility.Collapsed;
            }

            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

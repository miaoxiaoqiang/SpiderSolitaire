using System;
using System.Globalization;
using System.Windows.Data;

namespace SpiderSolitaire.Converters
{
    public sealed class SecondsToTimeConverter : IValueConverter
    {
        private static SecondsToTimeConverter _instance;
        public static SecondsToTimeConverter Instance => _instance ??= new SecondsToTimeConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return "00:00:00";
            }

            //^(0|([1-9][0-9]*))(\.[\d]{1})?$  匹配非负数
            long duration = System.Convert.ToInt64(value);
            TimeSpan timespan = TimeSpan.FromSeconds(duration);
            return $"{timespan.Hours.ToString("D2")}:{timespan.Minutes.ToString("D2")}:{timespan.Seconds.ToString("D2")}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

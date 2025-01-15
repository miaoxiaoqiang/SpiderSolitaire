using System;
using System.Globalization;
using System.Windows.Data;

namespace SpiderSolitaire.Converters
{
    public sealed class LevelToCheckConverter : IValueConverter
    {
        private static LevelToCheckConverter _instance;
        public static LevelToCheckConverter Instance => _instance ??= new LevelToCheckConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null
                || !Enum.IsDefined(typeof(Model.GameLevel), value)
                || !Enum.IsDefined(typeof(Model.GameLevel), parameter))
            {
                return false;
            }

            Model.GameLevel level1 = (Model.GameLevel)Enum.Parse(typeof(Model.GameLevel), value.ToString());
            Model.GameLevel level2 = (Model.GameLevel)Enum.Parse(typeof(Model.GameLevel), parameter.ToString());

            return level1 == level2;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value != null || parameter != null)
            {
                bool result = bool.Parse(value.ToString());
                if (result)
                {
                    return (Model.GameLevel)Enum.Parse(typeof(Model.GameLevel), parameter.ToString());
                }
            }

            return Model.GameLevel.OneSuit;
        }
    }
}

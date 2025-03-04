using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace hoso.Utils.Converter
{
    public class BooleanToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isTrue && parameter is string options)
            {
                var splitOptions = options.Split('|');
                return isTrue ? splitOptions[0] : splitOptions[1];
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
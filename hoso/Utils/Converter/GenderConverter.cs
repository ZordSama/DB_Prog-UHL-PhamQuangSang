using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace hoso.Utils.Converter;
public class GenderConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is int gender)
        {
            return gender == 0 ? "Nam" : "Nữ";
        }
        return "Không xác định";
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is string genderStr)
        {
            return genderStr == "Nam" ? 0 : 1;
        }
        return null;
    }
}
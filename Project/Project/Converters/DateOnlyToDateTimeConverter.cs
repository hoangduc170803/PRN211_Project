using System;
using System.Globalization;
using System.Windows.Data;

namespace Project.Converters
{
    public class DateOnlyToDateTimeConverter : IValueConverter
    {
        // Convert DateOnly -> DateTime
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateOnly dateOnly)
                return dateOnly.ToDateTime(new TimeOnly(0, 0));
            return Binding.DoNothing;
        }

        // Convert DateTime -> DateOnly
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime dt)
                return DateOnly.FromDateTime(dt);
            return Binding.DoNothing;
        }
    }
}

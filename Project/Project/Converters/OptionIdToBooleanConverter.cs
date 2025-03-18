using System;
using System.Globalization;
using System.Windows.Data;

namespace Project.Converters
{
    public class OptionIdToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int selected && parameter is string optionStr && int.TryParse(optionStr, out int optionId))
            {
                return selected == optionId;
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isChecked && isChecked && parameter is string optionStr && int.TryParse(optionStr, out int optionId))
            {
                return optionId;
            }
            return Binding.DoNothing;
        }
    }
}


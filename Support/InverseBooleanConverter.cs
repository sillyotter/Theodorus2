using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace Theodorus2.Support
{
    [ValueConversion(typeof(bool), typeof(bool))]
    public class InverseBooleanConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Boolean && targetType == typeof (Boolean))
            {
                return !((Boolean) value);
            }
            throw new InvalidOperationException("Incorrect type to provider");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Convert(value, targetType, parameter, culture);
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}

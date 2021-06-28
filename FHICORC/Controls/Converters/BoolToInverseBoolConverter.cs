using System;
using System.Globalization;
using Xamarin.Forms;

namespace FHICORC.Controls.Converters
{
    public class BoolToInverseBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(bool))
            {
                throw new InvalidOperationException("Can't convert non-boolean");
            }

            return !(bool) value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(bool))
            {
                throw new InvalidOperationException("Can't convert non-boolean");
            }

            return !(bool) value;
        }
    }
}
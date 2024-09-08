using Avalonia.Data;
using Avalonia.Data.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tc.Utils.Converters
{
    public class ScoreComboIndexConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            //if (targetType.Equals( typeof(System.Int32))) 
            //    return new BindingNotification(new InvalidCastException(),
            //                                        BindingErrorType.Error);
            if (value is null)
            {
                return -1;
            }
            return (long)value-1;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            //if (targetType.Equals(typeof(long?)))
            //    return new BindingNotification(new InvalidCastException(),
            //                                        BindingErrorType.Error);
            if (value is null || (int)value! == -1)
            {
                return null;
            }
            return (int)value + 1;
        }
    }
}

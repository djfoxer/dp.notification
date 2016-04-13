using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace djfoxer.dp.notification.App.Helpers
{
    public sealed class StringFormat : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string formatString = parameter as string;
            if (!string.IsNullOrEmpty(formatString))
            {
                if (value is DateTime)
                {
                    return ((DateTime)value).ToString(formatString, CultureInfo.InvariantCulture);
                }
            }
            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace djfoxer.dp.notification.App.Helpers
{
    public sealed class NegationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return value is bool ? !(bool)value : false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value is bool ? !(bool)value : false;
        }
    }
}

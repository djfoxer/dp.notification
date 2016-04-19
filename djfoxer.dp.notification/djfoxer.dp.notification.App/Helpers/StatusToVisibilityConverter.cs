using System;
using Windows.UI.Xaml.Data;
using static djfoxer.dp.notification.Core.Enum;

namespace djfoxer.dp.notification.App.Helpers
{
    public sealed class StatusToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            NotificationStatus status = (NotificationStatus)value;
            if (status == NotificationStatus.New)
            {
                return 1;
            }
            else
            {
                return 0.5;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}

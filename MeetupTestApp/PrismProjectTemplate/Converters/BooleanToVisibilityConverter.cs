using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace MeetupTestClient.Converters
{
    public class BooleanToVisibilityConverter : IValueConverter
    {
        private Boolean _falseIsVisible = false;

        public Boolean FalseIsVisible
        {
            get { return _falseIsVisible; }
            set { _falseIsVisible = value; }
        }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            bool? val = value as bool?;

            bool result = (val == null || val == false);

            if (FalseIsVisible)
                return result ? Visibility.Visible : Visibility.Collapsed;
            else
                return result ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}

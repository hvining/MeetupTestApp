using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace MeetupTestClient.Converters
{
    public class CollectionToVisibilityConverter : IValueConverter
    {
        private Boolean _nullOrEmptyIsVisible = false;

        public Boolean NullOrEmptyIsVisible
        {
            get { return _nullOrEmptyIsVisible; }
            set
            {
                _nullOrEmptyIsVisible = value;
            }
        }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            IEnumerable<object> collection = value as IEnumerable<object>;

            bool hasContent = (collection != null && collection.Count() > 0);

            if (NullOrEmptyIsVisible)
            {
                return hasContent ? Visibility.Collapsed : Visibility.Visible;
            }
            else
            {
                return hasContent ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}

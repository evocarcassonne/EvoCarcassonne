using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using EvoCarcassonne.Backend;

namespace EvoCarcassonne
{
    public class NullToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter == null)
            {
                return value != null ? Visibility.Visible : Visibility.Collapsed;
            }

            if (value is List<IDirection> directions)
            {
                if (directions.Count > 4)
                {
                    return directions[4].Figure != null ? Visibility.Visible : Visibility.Collapsed;
                }

                return Visibility.Collapsed;
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace EvoCarcassonne
{
    public class PropertyToVisibilityValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null ? Visibility.Visible : Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // We don't need this.
            throw new NotImplementedException();
        }
    }
}
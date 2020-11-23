using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace CG.Tools.CodeMap.Infrastructure
{
    /// <summary>
    /// This class is a bool to visibility value converter.
    /// </summary>
    [ValueConversion(typeof(bool), typeof(bool))]
    public class BooleanToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// This method converts the value from the source type to the target type.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => (bool)value ? Visibility.Visible : Visibility.Collapsed;

        /// <summary>
        /// This method converts the value from the taget type to the source type.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}

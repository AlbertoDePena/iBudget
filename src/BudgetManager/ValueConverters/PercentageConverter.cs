using System;
using System.Globalization;
using System.Windows.Data;

namespace BudgetManager.ValueConverters
{
    public class PercentageConverter : IValueConverter
    {
        public object Convert(
            object value, Type targetType, object parameter, CultureInfo culture)
        {
            return System.Convert.ToDouble(value).ToString("P");
        }

        public object ConvertBack(
            object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
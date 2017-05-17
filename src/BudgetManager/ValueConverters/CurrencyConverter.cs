using System;
using System.Globalization;
using System.Windows.Data;

namespace BudgetManager.ValueConverters
{
    public class CurrencyConverter : IValueConverter
    {
        public object Convert(
            object value, Type targetType, object parameter, CultureInfo culture)
        {
            return System.Convert.ToDouble(value).ToString("C");
        }

        public object ConvertBack(
            object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
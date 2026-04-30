using System;
using System.Globalization;
using System.Windows.Data;

namespace AdSR.Common
{
    public class TroncaMarsauConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string testo && !string.IsNullOrEmpty(testo))
            {
                if (testo.Length > 6)
                    return testo.Substring(0, testo.Length - 6);

                return string.Empty;
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
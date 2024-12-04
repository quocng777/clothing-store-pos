using Microsoft.UI.Xaml.Data;
using System;
using System.Diagnostics;
using System.Globalization;

namespace Clothing_Store_POS.Converters
{
    public class PriceToVNDConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is double price)
            {
                CultureInfo vietnameseCulture = new CultureInfo("vi-VN");
                string formattedPrice = price.ToString("N0", vietnameseCulture);
                return $"{formattedPrice} đ";
            }

            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException("ConvertBack is not supported.");
        }
    }
}

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
            return convertToVND(value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException("ConvertBack is not supported.");
        }

        public static string convertToVND(object price)
        {
            if(price is double number)
            {
                CultureInfo vietnameseCulture = new CultureInfo("vi-VN");
                string formattedPrice = number.ToString("N0", vietnameseCulture);
                return $"{formattedPrice} đ";
            }

            return "";
        }
    }
}

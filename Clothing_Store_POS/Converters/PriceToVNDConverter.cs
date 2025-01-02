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
            return ConvertToVND(value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is string price)
            {
                string cleandPrice = price.Replace("đ", "")
                                          .Replace(" ", "")
                                          .Replace(".", "")
                                          .Replace(",", ".");

                if (double.TryParse(cleandPrice, NumberStyles.Any, CultureInfo.InvariantCulture, out double result))
                {
                    return result;
                }
            }

            return 0;
        }

        public static string ConvertToVND(object price)
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

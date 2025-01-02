using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media.Imaging;
using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clothing_Store_POS.Converters
{
    public class ThumnailPathConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string imagePath = value as string;

            if (imagePath != null && File.Exists(imagePath))
            {
                try
                {
                    var bitmapImage = new BitmapImage(new Uri(imagePath, UriKind.RelativeOrAbsolute));
                    return bitmapImage;
                }
                catch (Exception)
                {
                    return new BitmapImage(new Uri("ms-appx:///Assets/product-default.png", UriKind.Absolute));
                }
            } 
            else
            {
                return new BitmapImage(new Uri("ms-appx:///Assets/product-default.png", UriKind.Absolute));
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}

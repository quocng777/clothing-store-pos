using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clothing_Store_POS.Converters
{
    public class UtcDateTimeConverter : DefaultTypeConverter
    {
        public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            if (DateTime.TryParse(text, out var date))
            {
                return DateTime.SpecifyKind(date, DateTimeKind.Utc);
            }

            return DateTime.UtcNow;
        }
    }
}

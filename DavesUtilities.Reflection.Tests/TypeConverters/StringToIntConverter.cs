using System;
using System.ComponentModel;
using System.Globalization;

namespace DavesUtilities.Reflection.Tests.TypeConverters
{
    internal class StringToLengthConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == typeof(int);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {            
            if(value is string str)
            {
                return str.Length;
            }

            return 0;
        }
    }
}
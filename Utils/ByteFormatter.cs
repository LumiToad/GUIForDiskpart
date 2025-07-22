using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Web.Services.Description;


namespace GUIForDiskpart.Utils
{
    public enum Unit
    {
        B = 0,
        KB = 1,
        MB = 2,
        GB = 3,
        TB = 4,
        PB = 5,
        MAX = 5
    }

    public static class ByteFormatter
    {
        private static Dictionary<Unit, string> suffix = new()
        {
            {Unit.B, "B"},
            {Unit.KB, "KB"},
            {Unit.MB, "MB"},
            {Unit.GB, "GB"},
            {Unit.TB, "TB"},
            {Unit.PB, "PB"},
        };

        public static string BytesToAsString(Int64 bytes, bool withSuffix = true, Unit unit = Unit.MAX, int decimalDigits = 2)
        {
            int i = 0;
            string[] strings = new string[suffix.Count];
            suffix.Values.CopyTo(strings, 0);
            double dblSByte = FormatBytes(bytes, ref i, unit);
            string format = CreateFormatString(decimalDigits);
            string suffixResult = (withSuffix ? strings[i + 1] : string.Empty);

            return string.Format(format, dblSByte, suffixResult).TrimEnd();
        }

        public static string BytesToAsString(UInt64 unsignedBytes, bool withSuffix = true, Unit unit = Unit.MAX, int decimalDigits = 2)
        {
            Int64 bytes = System.Convert.ToInt64(unsignedBytes);

            return BytesToAsString(bytes, withSuffix, unit, decimalDigits);
        }

        public static ReturnType BytesTo<TypeOfBytes, ReturnType>(TypeOfBytes bytes, Unit unit = Unit.MAX) 
            where TypeOfBytes : unmanaged, IComparable, IFormattable, IConvertible, IComparable<TypeOfBytes>, IEquatable<TypeOfBytes>
            where ReturnType : unmanaged, IComparable, IFormattable, IConvertible, IComparable<ReturnType>, IEquatable<ReturnType>
        {
            int i = 0;
            long bytesAsLong = System.Convert.ToInt64(bytes);
            var result = FormatBytes(bytesAsLong, ref i, unit);
            return (ReturnType)System.Convert.ChangeType(result, typeof(ReturnType));
        }

        public static string SizeFromToAsString<TypeOfSize>(
            TypeOfSize size, Unit fromUnit, Unit toUnit, bool withSuffix = true, int decimalDigits = 2)
            where TypeOfSize : unmanaged, IComparable, IFormattable, IConvertible, IComparable<TypeOfSize>, IEquatable<TypeOfSize>
        {
            long sizeAsLong = System.Convert.ToInt64(size);
            double dblSByte = SizeFromTo<long, double>(sizeAsLong, fromUnit, toUnit);
            string format = CreateFormatString(decimalDigits);
            string suffixResult = (withSuffix ? suffix[toUnit] : string.Empty);

            return string.Format(format, dblSByte, suffixResult).TrimEnd();
        }

        public static ReturnType SizeFromTo<TypeOfSize, ReturnType>(TypeOfSize bytes, Unit fromUnit, Unit toUnit)
            where TypeOfSize : unmanaged, IComparable, IFormattable, IConvertible, IComparable<TypeOfSize>, IEquatable<TypeOfSize>
            where ReturnType : unmanaged, IComparable, IFormattable, IConvertible, IComparable<ReturnType>, IEquatable<ReturnType>
        {
            long bytesAsLong = System.Convert.ToInt64(bytes);
            var result = SizeFromTo(bytesAsLong, fromUnit, toUnit);
            return (ReturnType)System.Convert.ChangeType(result, typeof(ReturnType));
        }

        public static double SizeFromTo(Int64 size, Unit fromUnit, Unit toUnit)
        {
            if (fromUnit == toUnit)
                return size;
            else if (toUnit > fromUnit)
            {
                int i = (int)fromUnit;
                return FormatBytes(size, ref i, toUnit);
            }

            double dblSByte = size;
            for
                (
                int i = (int)fromUnit;
                i > (int)toUnit;
                i--, size *= 1024
                )
            {
                dblSByte = size * 1024.0;
            }

            return dblSByte;
        }

        private static string CreateFormatString(int decimalDigits)
        {
            string formatString = "{0:0.";
            formatString = formatString.PadRight(formatString.Length + decimalDigits, '#');
            formatString += "} {1}";

            return formatString;
        }

        private static double FormatBytes(Int64 bytes, ref int iteration, Unit unit)
        {
            double dblSByte = bytes;
            for 
                (
                int i = iteration;
                i < (int)unit && bytes >= 1024;
                i++, bytes /= 1024
                )
            {
                dblSByte = bytes / 1024.0;
                iteration = i;
            }

            return dblSByte;
        }
    }

}

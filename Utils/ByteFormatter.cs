using System;
using System.Collections.Generic;


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

        public static string BytesToUnitAsString(Int64 bytes, bool withSuffix = true, Unit unit = Unit.MAX, int decimalDigits = 2)
        {
            int i = 0;
            string[] strings = new string[suffix.Count];
            suffix.Values.CopyTo(strings, 0);
            double dblSByte = FormatBytes(bytes, ref i, unit);
            string format = CreateFormatString(decimalDigits);
            string suffixResult = (withSuffix ? strings[i + 1] : string.Empty);

            return string.Format(format, dblSByte, suffixResult).TrimEnd();
        }

        public static string BytesToUnitAsString(UInt64 unsignedBytes, bool withSuffix = true, Unit unit = Unit.MAX, int decimalDigits = 2)
        {
            Int64 bytes = System.Convert.ToInt64(unsignedBytes);

            return BytesToUnitAsString(bytes, withSuffix, unit, decimalDigits);
        }

        public static double BytesToUnit(Int64 bytes, Unit unit = Unit.MAX)
        {
            int i = 0;
            return FormatBytes(bytes, ref i, unit);
        }

        public static double BytesToUnit(UInt64 bytes, Unit unit = Unit.MAX)
        {
            int i = 0;
            return FormatBytes(System.Convert.ToInt64(bytes), ref i, unit);
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

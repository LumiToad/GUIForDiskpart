using System;

namespace GUIForDiskpart.main
{
    public static class ByteFormatter
    {
        public static string FormatBytes(long bytes)
        {
            string[] Suffix = { "B", "KB", "MB", "GB", "TB" };
            int i;
            double dblSByte = bytes;
            for (i = 0; i < Suffix.Length && bytes >= 1024; i++, bytes /= 1024)
            {
                dblSByte = bytes / 1024.0;
            }

            return String.Format("{0:0.##} {1}", dblSByte, Suffix[i]);
        }

        public static string FormatBytes(ulong unsignedBytes)
        {
            long bytes = Convert.ToInt64(unsignedBytes);

            string[] Suffix = { "B", "KB", "MB", "GB", "TB" };
            int i;
            double dblSByte = bytes;
            for (i = 0; i < Suffix.Length && bytes >= 1024; i++, bytes /= 1024)
            {
                dblSByte = bytes / 1024.0;
            }

            return String.Format("{0:0.##} {1}", dblSByte, Suffix[i]);
        }

        public static string GetBytesAsStringAndUnit(long bytes) 
        {
            return bytes > 0 ? $"({bytes} B)" : "";
        }

        public static string GetBytesAsStringAndUnit(ulong unsignedBytes)
        {
            long bytes = Convert.ToInt64(unsignedBytes);

            return GetBytesAsStringAndUnit(bytes);
        }
    }
    
}

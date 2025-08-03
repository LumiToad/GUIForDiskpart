using System.Text.RegularExpressions;

namespace GUIForDiskpart.Utils
{
    public static class TextUtils
    {
        
        public static string RemoveAllButNumbers(this string text)
        {
            return Regex.Replace(text, "[^0-9]", "");
        }

        public static string UnifyWhiteSpace(this string text)
        {
            return Regex.Replace(text, @"[\r\n]+", "\n");
        }
        
    }
}

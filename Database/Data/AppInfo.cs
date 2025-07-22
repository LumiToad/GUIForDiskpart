using System.Reflection;

namespace GUIForDiskpart.Database.Data
{
    public static class AppInfo
    {
        public const string WEBSITE_URL = "https://github.com/LumiToad/GUIForDiskpart";
        public const string WIKI_URL = "https://github.com/LumiToad/GUIForDiskpart/wiki";
        public const string BUILD_STAGE = "Beta (MVP refactor)";
        public static string BuildString => GetBuildNumberString();

        private static string GetBuildNumberString()
        {
            string build = "";

            build += Assembly.GetExecutingAssembly().GetName().Version.ToString();
            build += " - " + BUILD_STAGE;

            return build;
        }
    }
}

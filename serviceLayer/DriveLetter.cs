using DLRetriever = GUIForDiskpart.Database.Retrievers.DriveLetter;

namespace GUIForDiskpart.Service
{
    public static class DriveLetter
    {
        private const string VIABLE_DRIVE_LETTERS = "CDEFGHIJKLMNOPQRSTUVWXYZ";
        private static DLRetriever dlRetriever = new();

        public static char[] GetAvailableDriveLetters()
        {
            string availableLetters = VIABLE_DRIVE_LETTERS;

            char[] usedLetters = dlRetriever.GetUsedDriveLetters();

            foreach (char usedLetter in usedLetters)
            {
                if (availableLetters.Contains(usedLetter))
                {
                    availableLetters = availableLetters.Replace(usedLetter.ToString(), "");
                    availableLetters.TrimEnd();
                }
            }

            return availableLetters.ToCharArray();
        }
    }
}


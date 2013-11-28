namespace Solvberget.Core.ViewModels
{
    public class Utils
    {
        public static string ConvertMediaTypeToNiceString(string type)
        {
            switch (type)
            {
                case "Document":
                case "Book":
                    return "Bok";
                case "Film":
                    return "Film";
                case "AudioBook":
                    return "Lydbok";
                case "Cd":
                    return "CD";
                case "Journal":
                    return "Tidsskrift";
                case "SheetMusic":
                    return "Noter";
                case "Game":
                    return "Spill";
                default:
                    return "Annet";
            }
        }
    }
}

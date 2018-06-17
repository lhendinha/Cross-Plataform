namespace CrossPlataform.Utilities
{
    public class Functions
    {
        public string ReplaceCharacteres(string textToReplace)
        {
            var stringsToReplace = new string[]
            {
                "<li>",
                "</li>",
                "<ul>",
                "</ul>",
                "<p>",
                "</p>",
                "<br>"
            };

            foreach (var replacement in stringsToReplace)
            {
                textToReplace = textToReplace.Replace(replacement, string.Empty);
            }

            return textToReplace;
        }
    }
}
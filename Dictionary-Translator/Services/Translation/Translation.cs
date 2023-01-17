using Dictionary_Translator.Models;
using System.Collections.Generic;
using System.Web;

namespace Dictionary_Translator.Services.Translation
{
    internal static class Translation
    {
        internal static IEnumerable<string> GetTranslationsString(Root translationModel)
        {
            return GetNestedTranslatedTextFromList(translationModel.def[0].tr);
        }

        internal static string GetTranslatedStringFromCollection(IEnumerable<string> collection)
        {
            string result = string.Empty;

            int counter = 0;

            foreach (string translatedText in collection)
            {
                if (counter == 0)
                {
                    result += translatedText;
                    counter += 1;
                }
                else
                {
                    result += ", " + translatedText;
                }

            }

            return result;
        }

        private static IEnumerable<string> GetNestedTranslatedTextFromList(List<Tr> list)
        {
            if (list.Count == 0)
            {
                return null;
            }

            string[] translatedText = new string[list.Count];
            for (int i = 0; i < translatedText.Length; i++)
            {
                translatedText[i] = list[i].text ?? "null";
            }

            return translatedText;
        }

    }
}

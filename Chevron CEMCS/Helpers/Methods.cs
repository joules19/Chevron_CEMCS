using System;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using static Chevron_CEMCS.DataAccess.PlayerModel;

namespace Chevron_CEMCS.Helpers
{
	public class Methods
    {
        public static string RemoveWord(string text, List<string> wordsToBeRemoved)
        {
            foreach (var word in wordsToBeRemoved)
            {
                text = text.Replace(word, "");
            }
            var oRegEx = new System.Text.RegularExpressions.Regex("<[^>]+>");
            return oRegEx.Replace(text, string.Empty);
        }

        public static bool checkNumberOccurence(string text)
        {
            return text.Any(letter => char.IsDigit(letter));
        }

        public static int nextPlayerId(List<Player> players)
        {
            return players.Count() + 1;
        }
    }
}


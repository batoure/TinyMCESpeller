using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using NHunspell;

namespace SpellChecker
{
    public class Dictionary : Hunspell
        {
        public Dictionary(string aff, string dic)
                : base(aff, dic)
            {
                //other initialization stuff
            }
        public string get_incorrect_words(string textBlock)
        {
            char[] delimiterChars = { ' ', ',', '.', ':', '\t' };
            string[] words = textBlock.Split(delimiterChars);
            string outcome = null;
            var incorrectWords = (from word in words let correct = Spell(word) where !correct select word).ToList();
            if (incorrectWords.Count > 0)
            {
                outcome = "success";
            }
            var returndata = JsonConvert.SerializeObject(incorrectWords);
            return "{\"outcome\":\"" + outcome + "\",\"data\":[" + returndata + "]}";
        }
        public string get_suggestions(string word)
        {
            var suggestions = Suggest(word);
            return JsonConvert.SerializeObject(suggestions);
        }

    }
}
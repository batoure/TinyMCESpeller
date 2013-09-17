using System;
using System.Collections.Generic;
using System.Web;
using Newtonsoft.Json;
using NHunspell;

namespace SpellChecker
{
    public partial class SpellChecker : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Clear();
            Response.ContentType = "application/json; charset=utf-8";

            using (Spellchecker spellchecker = new Spellchecker("en_us.aff", "en_us.dic"))
            {
                string spellingAction = Request.Form["action"];
                try
                {
                    string json = null;

                    if (!string.IsNullOrEmpty(Request.Form["action"]))
                    {

                        if (spellchecker.GetType().GetMethod(Request.Form["action"]) != null)
                        {
                            json = (typeof(Spellchecker).GetMethod(spellingAction)
                                .Invoke(spellchecker, parameters: new object[] {Request})).ToString();
                        }
                    }
                    Response.Write(json);
                    Response.End();
                }
                catch (Exception exception)
                {
                    throw (exception);
                }
            }
        }
    }

    public class Spellchecker : Hunspell
        {
            public Spellchecker(string aff, string dic)
                : base(aff, dic)
            {
                //other initialization stuff
            }

            public string get_incorrect_words(HttpRequest args)
            {

                var text = args.Form["text[]"];
                char[] delimiterChars = { ' ', ',', '.', ':', '\t' };
                string[] words = text.Split(delimiterChars);
                string outcome = null;
                List<string> incorrectWords = new List<string>();
                foreach (string word in words)
                {
                    var correct = Spell(word);
                    if (!correct)
                    {
                        incorrectWords.Add(word);
                    }
                }
                if (incorrectWords.Count > 0)
                {
                    outcome = "success";
                }
                var returndata = JsonConvert.SerializeObject(incorrectWords);
                return "{\"outcome\":\"" + outcome + "\",\"data\":[" + returndata + "]}";
            }
            public string get_suggestions(HttpRequest args)
            {
                var word = args.Form["word"];
                List<string> suggestions = Suggest(word);
                return JsonConvert.SerializeObject(suggestions);
            }
        }
    }

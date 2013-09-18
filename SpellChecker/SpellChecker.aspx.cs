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
            using (Dictionary spellchecker = new Dictionary("en_us.aff", "en_us.dic"))
            {
                string spellingAction = Request.Form["action"];
                try
                {
                    string json = null;
                    if (!string.IsNullOrEmpty(Request.Form["action"]))
                    {
                        if (spellchecker.GetType().GetMethod(Request.Form["action"]) != null)
                        {
                            json = (typeof(Dictionary).GetMethod(spellingAction)
                                .Invoke(spellchecker, new object[] {Request})).ToString();
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
}

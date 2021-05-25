using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MonkeyBot.Data
{
    public static class EatData
    {
        private static string json =
@"
{
    ""_comment"": ""used for eat command"",
    ""monke"": ""oo aa, monke not like eating monke"",
    ""crossmane"": ""yum monke like eating penguins"",
    ""rolety"": ""poopoo"",
    ""void"": ""monke not like eating void. no ping for you :D"",
    ""your mom"": ""your mom"",
    ""ping"": ""why"",
    ""skyblock"": ""we did it boys grind is no more"",
    ""me"": ""no why"",
    ""secret"": ""congrats you found a secret"",
    ""coconut"": ""green coconut is yum"",
    ""abc"": ""def"",
    ""c#"": ""is better than python for writing bots"",
    ""cum"": ""you sick idiot"",
    ""cock"": ""you sick idiot"",
    ""sex"": ""you sick idiot"",
    ""alejo"": ""crknge""
}
";

        private static string[] _randoms =
        {
            "amazing monke food, me like",
            "cool food, me like it",
            "epic monke food oo aa",
            "monke like it",
            "monke not like it",
            "ewww bad monke food",
            "worst monke food possible ew"
        };
            
        
        public static string Choose(string f)
        {
            
            JObject deserialized = JsonConvert.DeserializeObject<JObject>(json);
            string msg;
            if (deserialized.ContainsKey(f.ToLower()))
            {
                msg = (string)deserialized[f.ToLower()];
            }
            else
            {
                Random r = new Random();
                int ran = r.Next(0, _randoms.Length - 1);
                msg = _randoms[ran];
            }

            return msg;
        }
    }
}
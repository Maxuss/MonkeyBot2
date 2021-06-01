using MonkeyBot.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;

namespace MonkeyBot.Helpers
{
    public static class Inner
    {
        public static char GetPrefix()
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "botconfig.xml");
            XmlDocument doc = new XmlDocument();
            doc.Load(path);
            XmlNodeList nodes = doc.DocumentElement.ChildNodes;
            XmlNode tokenNode = nodes[1];
            return tokenNode.InnerText.ToCharArray()[0];
        }

        public static string GetDiscordToken()
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "botconfig.xml");
            XmlDocument doc = new XmlDocument();
            doc.Load(path);
            XmlNodeList nodes = doc.DocumentElement.ChildNodes;
            XmlNode tokenNode = nodes[0];
            return tokenNode.InnerText;
        }

        public static string GetHypixelKey()
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "botconfig.xml");
            XmlDocument doc = new XmlDocument();
            doc.Load(path);
            XmlNodeList nodes = doc.DocumentElement.ChildNodes;
            XmlNode tokenNode = nodes[2];
            return tokenNode.InnerText;
        }

        public async  static Task<string> GetSbProfileAsync(string uuid, string KEY)
        {
            JObject data = await GetPlayerDataAsync(uuid, KEY);
            JObject dat = (JObject)data["player"]["stats"]["SkyBlock"]["profiles"];
            IList<string> keys = dat.Properties().Select(p => p.Name).ToList();
            return keys[0];
        }

        public async static Task<string[]> GetUUIDAsync(string user)
        {
            string mojangURL = "https://api.mojang.com/users/profiles/minecraft/" + user;
            WebRequest request = WebRequest.Create(mojangURL);
            request.Method = "GET";
            using WebResponse webResponse = await request.GetResponseAsync();
            using Stream webStream = webResponse.GetResponseStream();
            using StreamReader reader = new StreamReader(webStream);
            string data = reader.ReadToEnd();
            dynamic json = JsonConvert.DeserializeObject<dynamic>(data);
            string uuid = (string)json.id;
            string icon = "https://crafatar.com/avatars/" + uuid;
            return new[] { uuid, icon };
        }

        public async static Task<JObject> GetPlayerDataAsync(string uuid, string key)
        {
            string hyUrl = "https://api.hypixel.net/player?key=" + key + "&uuid=" + uuid;
            WebRequest request = WebRequest.Create(hyUrl);
            request.Method = "GET";
            using WebResponse webResponse = await request.GetResponseAsync();
            using Stream webStream = webResponse.GetResponseStream();
            using StreamReader reader = new StreamReader(webStream);
            string data = reader.ReadToEnd();
            return JsonConvert.DeserializeObject<JObject>(data);
        }


        public async static Task<string> GetPlayerAliasesAsync(string uuid, string KEY)
        {
            JObject data = await GetPlayerDataAsync(uuid, KEY);
            if (!(bool)data["success"])
                throw new HttpRequestException("400. Bad Request", new Exception((string)data["cause"]), HttpStatusCode.BadRequest);
            JArray aliases = (JArray)data["player"]["knownAliases"];
            string appendable = "```";
            foreach (string alias in aliases)
            {
                appendable += $"\n{alias}";
            }
            return appendable + "```";
        }

        public static string GetSkin(string uuid, string t)
        {
            switch (t.ToLower())
            {
                case "skin":
                    return "https://craftar.com/skins/" + uuid;
                case "icon":
                    return "https://crafatar.com/avatars/" + uuid;
                case "head":
                    return "https://crafatar.com/renders/head/" + uuid;
                case "body":
                    return "https://crafatar.com/renders/body/" + uuid;
                case "cape":
                    return "https://crafatar.com/capes/" + uuid;
                default:
                    goto case "body";
            }
        }

        public async static Task<JObject> GetPlayerSBDataAsync(string profile, string KEY)
        {
            string hyUrl = $"https://api.hypixel.net/skyblock/profile?key=" + KEY + "&profile=" + profile;
            WebRequest request = WebRequest.Create(hyUrl);
            request.Method = "GET";
            using WebResponse webResponse = await request.GetResponseAsync();
            using Stream webStream = webResponse.GetResponseStream();
            using StreamReader reader = new StreamReader(webStream);
            string data = await reader.ReadToEndAsync();
            return JsonConvert.DeserializeObject<JObject>(data);
        }

        public async static Task<JObject> GetPlayerSkyCryptAsync(string playername)
        {
            string url = "https://sky.shiiyu.moe/api/v2/profile/" + playername;
            WebRequest request = WebRequest.Create(url);
            request.Method = "GET";
            using WebResponse webResponse = await request.GetResponseAsync();
            using Stream webStream = webResponse.GetResponseStream();
            using StreamReader reader = new StreamReader(webStream);
            string data = await reader.ReadToEndAsync();
            return JsonConvert.DeserializeObject<JObject>(data);
        }

        public static async Task<T> GetGuildObject<T>(string guildName, string KEY)
        {
            string url = "https://api.hypixel.net/guild?name=" + guildName + "&key=" + KEY;
            WebRequest request = WebRequest.Create(url);
            request.Method = "GET";
            using WebResponse webResponse = await request.GetResponseAsync();
            using Stream webStream = webResponse.GetResponseStream();
            using StreamReader reader = new StreamReader(webStream);
            string data = await reader.ReadToEndAsync();
            return JsonConvert.DeserializeObject<T>(data);
        }

        public static async Task<string> GetUsernameByUuid(string uuid)
        {
            string url = "https://api.mojang.com/user/profiles/" + uuid + "/names";
            WebRequest request = WebRequest.Create(url);
            request.Method = "GET";
            using WebResponse webResponse = await request.GetResponseAsync();
            using Stream webStream = webResponse.GetResponseStream();
            using StreamReader reader = new StreamReader(webStream);
            string data = await reader.ReadToEndAsync();
            var d = JsonConvert.DeserializeObject<JArray>(data);
            return (string)((JObject)d.Last())["name"];
        }
    }
}
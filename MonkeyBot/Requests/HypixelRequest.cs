using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Discord.Net.Udp;
using MonkeyBot.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static MonkeyBot.Data.Constants;
namespace MonkeyBot.Requests
{
    public static class HypixelRequest
    {
        public static async Task<string> GET(HypixelRequestType type, string extra)
        {
            string url = "";
            switch (type)
            {
                case HypixelRequestType.Bazaar:
                    url = $"{HY_API}skyblock/bazaar?item={extra}&key={Inner.GetHypixelKey()}";
                    break;
                default:
                    break;
            }
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.AutomaticDecompression = DecompressionMethods.GZip;
            string data;
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                data = await reader.ReadToEndAsync();
            }

            return data;
        }

        public static JObject SerializeData(string data)
        {
            return JsonConvert.DeserializeObject<JObject>(data);
        }

        public static async Task<JObject> GetSerializedData(HypixelRequestType type, string extra)
        {
            return SerializeData(await GET(type, extra));
        }
    }
}
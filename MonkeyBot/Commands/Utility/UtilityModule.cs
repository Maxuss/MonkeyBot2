using Discord.Commands;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using Discord;
using MonkeyBot.Data;
using MonkeyBot.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using EmbedField = MonkeyBot.Helpers.EmbedField;
using Embed = MonkeyBot.Helpers.Embed;
using static MonkeyBot.Data.Constants;
namespace MonkeyBot.Commands.Utility
{
    [Group("dox")]
    public class UtilityModule : ModuleBase<SocketCommandContext>
    {
        [Command("ip")]
        [Summary("Get ip address by host name or website address")]
        public async Task IPAsync([Remainder] [Summary("The address to get ip")] string address)
        {
            IUserMessage msg = await ReplyAsync("Please wait while im getting IP...");
            try
            {
                if (address.Equals("localhost"))
                    await msg.ModifyAsync(m => m.Content = "No. I will not ping this address.");
                else
                {
                    Ping ping = new Ping();
                    PingReply pr = ping.Send(address);
                    if (!pr.Status.Equals(IPStatus.Success))
                        await msg.ModifyAsync(m => m.Content = "Couldn't ping this address");
                    else
                    {
                        EmbedField f0 = new EmbedField("Success: ", "True");
                        EmbedField f1 = new("PING IP Address", $"{pr.Address.MapToIPv4()}");
                        IPAddress[] adrs = await Dns.GetHostAddressesAsync(address);
                        EmbedField f2 = new("DNS IP Address", $"{adrs[0].MapToIPv4()}");
                        EmbedField f3 =
                            new("How does it work",
                                "PING IP is ip gotten through sending requests to address.\nDNS IP is ip gotten through getting registered ips of dns address\nNote that `www.example.com` and `example.com` are different things!");
                        Embed e =
                            new("IP Address finder", EMBED_COLOR, new[] {f0, f1, f2, f3},
                                $"{Constants.EMBED_FOOTER} | IP Finder");
                        await msg.ModifyAsync(m =>
                        {
                            m.Content = "";
                            m.Embed = e.E;
                        });
                    }
                }
            }
            catch (Exception e)
            {
                ErrorEmbed er = new ErrorEmbed(e);
                await msg.ModifyAsync(m =>
                {
                    m.Content = "";
                    m.Embed = er.Embed;
                });
            }
        }

        [Command("geo")]
        [Summary("Get geolocation by IP address")]
        public async Task GeolocationAsync([Summary("The IP address to find geolocation")]
            string ip, [Summary("Parameter of address. [ip/host]")] string param)
        {
            IUserMessage msg = await ReplyAsync("Looking for geolocation, please wait...");
            try
            {
                await GeolocationHandler(ip, param, msg);
            }
            catch (HttpRequestException e)
            {
                ErrorEmbed er = new ErrorEmbed(e);
                await msg.ModifyAsync(m =>
                {
                    m.Content = "";
                    m.Embed = er.Embed;
                });
            }
        }

        private async Task GeolocationHandler(string ip, string param, IUserMessage msg)
        {
            switch (param.ToLower())
            {
                case "ip":
                    await GetGeolocationAsync(ip, param, msg);
                    break;
                case "host":
                    IPAddress adr = (await Dns.GetHostAddressesAsync(ip))[0];
                    await GetGeolocationAsync($"{adr.MapToIPv4()}", param, msg);
                    break;
                default:
                    goto case "host";
            }
        }

        private async Task GetGeolocationAsync(string ip, string param, IUserMessage msg)
            {
                string url = $"http://ip-api.com/json/{ip}";
                HttpWebRequest request = (HttpWebRequest) WebRequest.Create(url);
                request.AutomaticDecompression = DecompressionMethods.GZip;
                string data;
                using (HttpWebResponse response = (HttpWebResponse) request.GetResponse())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    data = await reader.ReadToEndAsync();
                }

                JObject json = JsonConvert.DeserializeObject<JObject>(data);
                if (json["status"].Equals("fail"))
                    await msg.ModifyAsync(m =>
                        m.Content = $"Couldn't find geolocation data, cause: \"{json["message"]}\"");
                else
                {
                    try
                    {
                        string country = (string) json["country"];
                        string cc = ((string) json["countryCode"]).ToLower();
                        string region = (string) json["regionName"];
                        string city = (string) json["city"];
                        string zip = (string) json["zip"];
                        float lat = (float) json["lat"];
                        float lon = (float) json["lon"];
                        string isp = (string) json["isp"];
                        EmbedField f0 = new("Success: ", "true", true);
                        EmbedField f1 = new("Country", $":flag_{cc}: {country}");
                        EmbedField f2 = new("Full Address", $"{region}, {city}");
                        EmbedField f3 = new("Provider Name", $"{isp}");
                        EmbedField f4 = new("Latitude & Longitude", $"Lat: {lat}\nLon: {lon}");
                        EmbedField f5 = new("Zip Code", $"{zip}");
                        Embed e = new Embed("Geolocation Finder", EMBED_COLOR, new[] {f0, f1, f2, f3, f4, f5},
                            $"{Constants.EMBED_FOOTER} | Adr Query: {ip} ; Param: {param}");
                        await msg.ModifyAsync(m =>
                        {
                            m.Content = "";
                            m.Embed = e.E;
                        });
                    }
                    catch (NullReferenceException)
                    {
                        throw new HttpRequestException(
                            $"Could not get geolocation of address {ip}! Cause: \"{json["message"]}\"");
                    }
                }
            }
        }
    }
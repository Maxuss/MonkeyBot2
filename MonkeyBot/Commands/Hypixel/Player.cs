using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using Discord.Commands;
using System.Threading.Tasks;
using static MonkeyBot.Data.Constants;
using Discord;
using MonkeyBot.Helpers;
using Newtonsoft.Json.Linq;
using MonkeyBot.Requests;
using Newtonsoft.Json;
using Embed = MonkeyBot.Helpers.Embed;
using EmbedField = MonkeyBot.Helpers.EmbedField;

namespace MonkeyBot.Commands.Hypixel
{
    [Group("mc")]
    public class Player : ModuleBase<SocketCommandContext>
    {
        private string KEY = Inner.GetHypixelKey();

        [Command("uuid")]
        [Summary("Gets minecraft uuid of player")]
        public async Task UuidAsync([Remainder] [Summary("The player to look up")]
            string username)
        {
            IUserMessage msg = await ReplyAsync("Please wait as I'm doing request...");
            string[] data = await GetUUIDAsync(username);
            string uuid = data[0];
            string icon = data[1];
            EmbedField ef = new EmbedField("Username", username);
            EmbedField ef1 = new EmbedField("Mojang UUID", uuid);
            Embed e = new Embed("MonkeyBotV2 UUID Getter", Color.Orange, new[] {ef, ef1}, $"{EMBED_FOOTER} | UUID Getter");
            EmbedBuilder eb = e.Builder;
            eb.ImageUrl = icon;
            Discord.Embed de = eb.Build();
            await msg.ModifyAsync(m =>
            {
                m.Content = "";
                m.Embed = de;
            });
        }

        [Command("aliases")]
        [Summary("Get all alias usernames of user")]
        public async Task AliasesAsync([Remainder] [Summary("The user name to look for")]
            string username)
        {
            IUserMessage msg = await ReplyAsync("Please wait as i'm doing request...");
            try
            {
                string[] tmp = await GetUUIDAsync(username);
                string aliases = await GetPlayerAliasesAsync(tmp[0]);
                EmbedField _1 = new("Success", "true", true);
                EmbedField _2 = new("Known aliases", aliases);
                Embed em = new Embed("MonkeyBotV2 Aliases", Color.Orange, new[]{_1, _2}, $"{EMBED_FOOTER} | Minecraft aliases finder");
                await msg.ModifyAsync(m =>
                {
                    m.Content = "";
                    m.Embed = em.E;
                });
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
        [Command("steal")]
        [Summary("Steals skin of player or makes render of it.")]
        public async Task SkinAsync([Summary("The username too look up")] string username,
            [Summary("Type of stealing. skin | icon | head | body | cape")]
            string param)
        {
            IUserMessage msg = await ReplyAsync("Please wait as I'm doing request...");
            try
            {
                string[] tmp = await GetUUIDAsync(username);
                string url = GetSkin(tmp[0], param);
                EmbedField _1 = new("Success", "true", true);
                EmbedField _2 = new("If image does not appear, there are some internal errors", "_ _");
                Embed emb = new Embed("MonkeyBotV2 Skin Stealer", Color.Orange, new[] {_1, _2},
                    $"{EMBED_FOOTER} | Skin Stealer");
                EmbedBuilder eb = emb.Builder;
                eb.ThumbnailUrl = url;
                Discord.Embed _e = eb.Build();
                await msg.ModifyAsync(m =>
                    {
                        m.Content = "";
                        m.Embed = _e;
                    }
                );
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
        
        private async Task<string[]> GetUUIDAsync(string user)
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
            return new[] {uuid, icon};
        }

        private async Task<JObject> GetPlayerDataAsync(string uuid)
        {
            string hyUrl = "https://api.hypixel.net/player?key=" + KEY + "&uuid=" + uuid;
            WebRequest request = WebRequest.Create(hyUrl);
            request.Method = "GET";
            using WebResponse webResponse = await request.GetResponseAsync();
            using Stream webStream = webResponse.GetResponseStream();
            using StreamReader reader = new StreamReader(webStream);
            string data = reader.ReadToEnd();
            return JsonConvert.DeserializeObject<JObject>(data);
        }

        private async Task<string> GetPlayerAliasesAsync(string uuid)
        {
            JObject data = await GetPlayerDataAsync(uuid);
            if (!(bool) data["success"])
                throw new HttpRequestException("400. Bad Request", new Exception((string)data["cause"]), HttpStatusCode.BadRequest);
            JArray aliases = (JArray) data["player"]["knownAliases"];
            string appendable = "```";
            foreach (string alias in aliases)
            {
                appendable += $"\n{alias}";
            }
            return appendable + "```";
        }

        private string GetSkin(string uuid, string t)
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
    }
    
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using Discord.Commands;
using System.Threading.Tasks;
using static MonkeyBot.Data.Constants;
using Discord;
using MonkeyBot.Helpers;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Embed = MonkeyBot.Helpers.Embed;
using EmbedField = MonkeyBot.Helpers.EmbedField;
using static MonkeyBot.Helpers.Inner;
using MonkeyBot.Data;
using System.Runtime.Caching;

namespace MonkeyBot.Commands.Hypixel
{
    public class Player : ModuleBase<SocketCommandContext>
    {
        private static string KEY = GetHypixelKey();

        private static CacheItemPolicy cache;

        [Group("mc")]
        public class Minecraft : ModuleBase<SocketCommandContext>
        {
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
                Embed e = new Embed("MonkeyBotV2 UUID Getter", EMBED_COLOR, new[] { ef, ef1 }, $"{EMBED_FOOTER} | UUID Getter");
                EmbedBuilder eb = e.Builder;
                eb.ImageUrl = icon;
                Discord.Embed de = eb.Build();

                ////
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
                    string aliases = await GetPlayerAliasesAsync(tmp[0], KEY);
                    EmbedField _1 = new("Success", "true", true);
                    EmbedField _2 = new("Known aliases", aliases);
                    Embed em = new Embed("MonkeyBotV2 Aliases", EMBED_COLOR, new[] { _1, _2 }, $"{EMBED_FOOTER} | Minecraft aliases finder");
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
                    Embed emb = new Embed("MonkeyBotV2 Skin Stealer", EMBED_COLOR, new[] { _1, _2 },
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
        }
        [Group("sb")]
        public class Skyblock : ModuleBase<SocketCommandContext>
        {
            [Command("reqs")]
            public async Task SbTestAsync(string playername, string profile="None")
            {
                var msg = await ReplyAsync("Please wait as I'm getting user's weight...");
                try
                {
                    float[] data = await GetRequirementsAsync(playername, profile);
#nullable enable
                    bool? con = data[0] >= WEIGHT_REQUIREMENT;
#nullable disable
                    string acc = string.Empty;
                    switch (con)
                    {
                        case true:
                            acc = "User is accepted to the guild!";
                            break;
                        case false:
                            acc = "User is not accepted to the guild!";
                            break;
                        case null:
                            throw new ArgumentNullException("An inner exception occurred! Either API errors or Armageddon began.");
                    }

                    var _1 = new EmbedField("Nickname", playername);
                    var _2 = new EmbedField("Weight", $"{data[0]}");
                    var _3 = new EmbedField("Skill Average", $"{data[1]}".Replace(",", "."));
                    var _4 = new EmbedField("Slayers", $"{data[2]} XP");
                    var _5 = new EmbedField("Conclusion", acc);
                    var _6 = new EmbedField("Current Requirements", $"Weight: {WEIGHT_REQUIREMENT}\nSkills: {SKILLS_REQUIREMENT}\nSlayers: {SLAYER_REQUIREMENT}");
                    Embed em = new Embed("Macaques Guild Acceptance", EMBED_COLOR, new[] { _1, _2, _3, _4, _5, _6 }, $"{EMBED_FOOTER} | Macaques guild requirement");
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

            [Command("guild-average")]
            public async Task GAAsync([Remainder][Summary("Name of the guild")]string guildname)
            {
                var msg = await ReplyAsync("Please wait as I'm getting guild data...");
                DateTime start = DateTime.Now;
                try
                {
                    int i = 0;
                    float tsk = 0;
                    float tsl = 0;
                    float tw = 0;
                    Guild guild = await GetGuildObject<Guild>(guildname, KEY);
                    GuildMember[] members = guild.Data.Members;

                    var loop = Parallel.ForEach(members, member =>
                        {
                            try
                            {
                                Task<string> namet = GetUsernameByUuid(member.UUID);
                                string name = namet.Result;
                                Task<float[]> dt = GetRequirementsAsync(name, "None");
                                float[] d = dt.Result;
                                tw += d[0];
                                tsk += d[1];
                                tsl += d[2];
                                Console.WriteLine($"{tw} {tsk} {tsl}");
                                i++;
                            }
                            catch(AggregateException) { };
                        });
                    Console.WriteLine("completed");
                    if (loop.IsCompleted) 
                    {
                        float weight = tw / i;
                        float skills = tsk / i;
                        float slayers = tsl / i;
                        var _1 = new EmbedField("Guild", guildname);
                        var _2 = new EmbedField("Average weight", $"{weight}".Replace(",", "."));
                        var _3 = new EmbedField("Average skill level", $"{skills}".Replace(",", "."));
                        var _4 = new EmbedField("Average slayer experience", $"{slayers} XP".Replace(",", "."));
                        DateTime end = DateTime.Now;
                        TimeSpan took = start - end;
                        double seconds = took.TotalSeconds;
                        var _5 = new EmbedField($"Time took: {-seconds}", "_ _");
                        Embed em = new Embed("Guild average parameters check", EMBED_COLOR, new[] { _1, _2, _3, _4, _5 }, $"{EMBED_FOOTER} | Guild: {guildname}");
                        await msg.ModifyAsync(m =>
                        {
                            m.Content = "";
                            m.Embed = em.E;
                        });
                    }
                }
                catch(Exception e)
                {
                    ErrorEmbed er = new ErrorEmbed(e);
                    await msg.ModifyAsync(m =>
                    {
                        m.Content = "";
                        m.Embed = er.Embed;
                    });
                }
            }
        }

        public static async Task<string> GetSbProfileAsync(string uuid)
        {
            JObject data = await GetPlayerDataAsync(uuid, KEY);
            JObject dat = (JObject)data["player"]["stats"]["SkyBlock"]["profiles"];
            IList<string> keys = dat.Properties().Select(p => p.Name).ToList();
            return keys[0];
        }

        public static string GetSbProfile(JObject sc, string prof)
        {
            JObject d = (JObject)sc["profiles"];
            IList<string> keys = d.Properties().Select(p => p.Name).ToList();
            foreach (string profile in keys)
            {
                if (((string)d[profile]["cute_name"]).ToLower().Equals(prof.ToLower())) return profile;
            }
            return null;
        }

        public static string GetSbProfile(JObject sc)
        {
            JObject d = (JObject)sc["profiles"];
            IList<string> keys = d.Properties().Select(p => p.Name).ToList();
            foreach (string profile in keys)
            {
                if ((bool)d[profile]["current"]) return profile;
            }
            return null;
        }

        public static async Task<float[]> GetRequirementsAsync(string playername, string profile)
        {
            JObject sc = await GetPlayerSkyCryptAsync(playername);
            string id;
            if (profile != "None")
            {
                id = GetSbProfile(sc, profile);
            }
            else
            {
                id = GetSbProfile(sc);
            }
            float w = (float)sc["profiles"][id]["data"]["weight"];
            float weight = MathF.Floor(w);
            float sa = (float)sc["profiles"][id]["data"]["average_level"];
            float skills = MathF.Round(sa, 1);
            int slayers = (int)sc["profiles"][id]["data"]["slayer_xp"];
            return new[] { weight, skills, slayers };
        }
    }
}
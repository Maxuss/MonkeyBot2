using Discord;
using MonkeyBot.Helpers;
using EmbedField = MonkeyBot.Helpers.EmbedField;
using Embed = MonkeyBot.Helpers.Embed;
using static MonkeyBot.Data.Constants;
namespace MonkeyBot.Helpers
{
    public class HelpEmbed
    {
        public int Current;
        public Discord.Embed[] Embeds;
        
        public Discord.Embed GetHelpEmbed()
        {
            char pr = Inner.GetPrefix();
            EmbedField info = new EmbedField("INFO", $"$Prefix: {pr}");
            EmbedField echo = new EmbedField("echo", $"Echoes what you say.\nUsage: `{pr}echo [message]`");
            EmbedField about = new EmbedField("about", $"Gives info about the bot.\nUsage: `{pr}about`");
            EmbedField help = new EmbedField("help", $"Shows this info.\nUsage: `{pr}help <page>`");
            EmbedField choose = new EmbedField("choose",
                $"Chooses between 2 or more variants, separated by comma.\nUsage: `{pr}choose [variant1,variant2...]`");
            EmbedField eat = new EmbedField("eat", $"Make monke eat something!\nUsage: `{pr}eat [food]`");
            EmbedField bz = new EmbedField("bz",
                $"Gives detailed info on specified item on bazaar.\nUsage: `{pr}bz [item]`");
            EmbedField ip = new EmbedField("ip",
                $"Gives DNS IP address of specified host.\nUsage: `{pr}ip [hostname]`");
            EmbedField geo = new EmbedField("geo",
                $"Gives detailed geolocation based by IP or hostname.\nUsage: `{pr}geo [address] [ip/host]`\nExample: \n`{pr}geo www.google.com host`\n`{pr}geo 104.53.13.4 ip`");
            EmbedField TF = new EmbedField("Total Pages: 3. Enter `{pr}help [page] for different page`", "", true);
            EmbedField[] pg1 = {info, echo, about, help, choose};
            EmbedField[] pg2 = {eat, bz, ip, geo};
            Embed _1 = new Embed("MonkeyBotV2 Help", Color.Orange, pg1, $"{EMBED_FOOTER} | {pr}help ");
            Embed _2 = new Embed("MonkeyBotV2 Help", Color.Orange, pg2, $"{EMBED_FOOTER} | {pr}help ");
            Embeds = new[] {_1.E, _2.E};
            return _1.E;
        }
    }
}
using static MonkeyBot.Data.Constants;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using MonkeyBot.Helpers;
using Embed = MonkeyBot.Helpers.Embed;
using EmbedField = MonkeyBot.Helpers.EmbedField;

namespace MonkeyBot.Commands
{
    public class InformationModule : ModuleBase<SocketCommandContext>
    {
        [Command("help")]
        [Summary("Gives you help on all commands")]
        public Task HelpAsync()
            => ReplyAsync("", false, GetHelpEmbed());

        [Command("about")]
        [Summary("Gives you info on bot and everything")]
        public Task AboutAsync()
            => ReplyAsync("", false, GetAboutEmbed());
        
        public Discord.Embed GetHelpEmbed()
        {
            char pr = Inner.GetPrefix();
            EmbedField info = new EmbedField("INFO", $"$Prefix: {pr}");
            EmbedField echo = new EmbedField("echo", $"Echoes what you say.\nUsage: `{pr}echo <message:string>`");
            EmbedField about = new EmbedField("about", $"Gives info about the bot.\nUsage: `{pr}about`");
            EmbedField help = new EmbedField("help", $"Shows this info.\nUsage: `{pr}help [page:int]`");
            EmbedField[] fs = new[] {info, echo, about, help};
            Embed embed = new Embed("Its a test", Color.Orange, fs, EMBED_FOOTER);
            return embed.E;
        }

        public Discord.Embed GetAboutEmbed()
        {
            EmbedField _1 = new EmbedField("Creator", 
                "MonkeyBot was created by Void Moment#8152 for Hypixel Skyblock guild Macaques.");
            EmbedField _2 = new EmbedField("Purpose of bot", 
                "MonkeyBot was created to help you with all your Hypixel Skyblock stuff.");
            EmbedField _3 = new EmbedField("Why is it V2",
                "I was originally going to make a huge update for previous version of bot, but then whole code broke, so I did this.");
            EmbedField _4 = new EmbedField("Contacts",
                $"Github Repository: {REPOSITORY}\nWebsite: {WEBSITE}\nDiscord Server Invite: {DISCORD_INVITE}");
            EmbedField _5 = new EmbedField("Developer Data",
                $"Version: v{VERSION}\nShort Version: v{SHORT_VERSION}\nBotName: {BOT_NAME}");
            EmbedField[] fs = new[] {_1, _2, _3, _4, _5};
            Embed e = new Embed("About MonkeyBotV2", Color.Orange, fs, EMBED_FOOTER);
            return e.E;
        }
    }
}
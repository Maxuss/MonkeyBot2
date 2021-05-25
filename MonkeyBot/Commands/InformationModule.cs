using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
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

        public Discord.Embed GetHelpEmbed()
        {
            char pr = Inner.GetPrefix();
            EmbedField info = new EmbedField("INFO", $"$Prefix: {pr}");
            EmbedField echo = new EmbedField("echo", $"Echoes what you say.\nUsage: `{pr}echo <message:string>`");
            EmbedField about = new EmbedField("about", $"Gives info about the bot.\nUsage: `{pr}about`");
            EmbedField help = new EmbedField("help", $"Shows this info.\nUsage: `{pr}help [page:int]`");
            EmbedField[] fs = new[] {info, echo, about, help};
            Embed embed = new Embed("Its a test", Color.Orange, fs, "Test footer");
            return embed.E;
        }
    }
}
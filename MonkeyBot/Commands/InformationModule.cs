using System;
using System.Collections.Generic;
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
        public async Task HelpAsync()
            => await ReplyAsync("", false, GetHelpEmbed());

        [Command("about")]
        [Summary("Gives you info on bot and everything")]
        public Task AboutAsync()
            => ReplyAsync("", false, GetAboutEmbed());

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
            EmbedField[] pg1 = {info, echo, about, help, choose, eat, bz, ip, geo};
            Embed _1 = new Embed("MonkeyBotV2 Help. Page 1", Color.Orange, pg1, $"{EMBED_FOOTER} | {pr}help ");
            return _1.E;
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
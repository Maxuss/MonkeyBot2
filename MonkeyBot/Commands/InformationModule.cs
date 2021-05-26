using System;
using System.Collections.Generic;
using static MonkeyBot.Data.Constants;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using MonkeyBot.Data;
using MonkeyBot.Helpers;
using Embed = MonkeyBot.Helpers.Embed;
using EmbedField = MonkeyBot.Helpers.EmbedField;

namespace MonkeyBot.Commands
{

    public class InformationModule : ModuleBase<SocketCommandContext>
    {
        public static HelpEmbed e;
        
        [Command("help")]
        [Summary("Gives you help on all commands")]
        public async Task HelpAsync()
        {
            e = new HelpEmbed();
            IUserMessage msg = await ReplyAsync("", false, e.GetHelpEmbed());
            await msg.AddReactionAsync(new Emoji(ARROW_BACKWARD));
            await msg.AddReactionAsync(new Emoji(ARROW_FORWARD));
        }

        [Command("about")]
        [Summary("Gives you info on bot and everything")]
        public Task AboutAsync()
            => ReplyAsync("", false, GetAboutEmbed());
        
        
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
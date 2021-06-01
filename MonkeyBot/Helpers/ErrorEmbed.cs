
using System;
using Discord;
using MonkeyBot.Data;


namespace MonkeyBot.Helpers
{
    public sealed class ErrorEmbed
    {
        public Discord.Embed Embed;
        public ErrorEmbed(Exception err)
        {
            string message = err.Message;
            string source = err.Source;
            string type = $"{err.GetType()}";
            
            string innerMessage;
            string innerSource;
            string innerType;
            Exception inn = err.InnerException;
            if (inn != null)
            {
                innerMessage = inn.Message;
                innerType = $"{inn.GetType()}";
                innerSource = inn.Source;
            }
            else
            {
                innerMessage = "None";
                innerType = "None";
                innerSource = "None";
            }

            string _ = "_ _";
            EmbedField _1 = new EmbedField("Oops! An error occurred!", _);
            EmbedField _2 = new EmbedField($"Exception: {type}", _);
            EmbedField _3 = new EmbedField($"Source: {source}", _);
            EmbedField _4 = new EmbedField("Message:", message);
            Embed e;
            if (innerMessage != "None")
            {
                EmbedField _5 = new EmbedField($"Inner exception: {innerType}", _);
                EmbedField _6 = new EmbedField($"Inner exception source: {innerSource}", _);
                EmbedField _7 = new EmbedField($"Inner exception message:", innerMessage);
                e = new Embed("MonkeyBotV2 Error report", Constants.ERROR_COLOR, new[] {_1, _2, _3, _4, _5, _6, _7},
                    $"{Constants.EMBED_FOOTER} | Please, create issue on GitHub");
            }
            else
            {
                e = new Embed("MonkeyBotV2 Error report", Constants.ERROR_COLOR, new[] {_1, _2, _3, _4},
                    $"{Constants.EMBED_FOOTER} | Please, create issue on GitHub");
            }

            Embed = e.E;
        }
    }
}
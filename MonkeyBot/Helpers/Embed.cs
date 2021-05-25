using Discord;

namespace MonkeyBot.Helpers
{
    public sealed class Embed
    {
        public Discord.Embed E;
        
        public Embed(string title, Color color, EmbedField[] fields)
        {
            EmbedBuilder builder = new EmbedBuilder();
            builder.WithTitle(title);
            foreach (EmbedField f in fields)
            {
                builder.AddField(f.Name, f.Value, f.Inline);
            }
            builder.WithColor(color);
            E = builder.Build();
        }

        public Embed(string title, Color color, EmbedField[] fields, string footer)
        {
            EmbedBuilder builder = new EmbedBuilder();
            builder.WithTitle(title);
            foreach (EmbedField f in fields)
            {
                builder.AddField(f.Name, f.Value, f.Inline);
            }
            builder.WithColor(color);
            builder.WithFooter(footer);
            E = builder.Build();
        }

        public Embed(string title, Color color, EmbedField[] fields, string footer, string url)
        {
            EmbedBuilder builder = new EmbedBuilder();
            builder.WithTitle(title);
            foreach (EmbedField f in fields)
            {
                builder.AddField(f.Name, f.Value, f.Inline);
            }
            builder.WithColor(color);
            builder.WithFooter(footer);
            builder.WithUrl(url);
            E = builder.Build();
        }
    }
}
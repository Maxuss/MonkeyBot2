namespace MonkeyBot.Helpers
{
    public sealed class EmbedField
    {
        public string Name { get; }
        public string Value { get; }
        public bool Inline { get; }

        public EmbedField(string name, string value, bool inline=false)
        {
            Name = name;
            Value = value;
            Inline = inline;
        }
    }
}
using System;
using Discord;
namespace MonkeyBot.Data
{
    public static class Constants
    {
        public const string VERSION = "2.01.3-stable";
        public const string SHORT_VERSION = "2.01";
        public const string REPOSITORY = "https://github.com/Maxuss/MonkeyBot2";
        public const string WEBSITE = "https://maxus.space/";
        public const string DISCORD_INVITE = "https://discord.gg/aGamC4jDup/";
        public const uint MAGIC_NUMBER = uint.MaxValue - 1;
        public const string SKYBLOCK_GUILD = "Macaques";
        public const string BOT_NAME = "MonkeyBot 2";
        public const string HY_API = "https://api.hypixel.net/";
        public const string EMBED_FOOTER = "MonkeyBotV2 by maxus | Void Moment#8152 | Maxuss";
        public const float WEIGHT_REQUIREMENT = 1500f;
        public const float SLAYER_REQUIREMENT = 0f;
        public const float SKILLS_REQUIREMENT = 0f;
        public static Color EMBED_COLOR { get; } = Color.Orange;
        public static Color ERROR_COLOR { get; } = Color.Red;

        public const string ARROW_FORWARD = "\u25B6";
        public const string ARROW_BACKWARD = "\u25C0";
    }
}
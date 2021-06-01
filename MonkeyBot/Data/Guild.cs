using System;
using Newtonsoft.Json;

namespace MonkeyBot.Data
{
    public class Guild
    {
        [JsonProperty("success")] public bool Success;
#nullable enable
        [JsonProperty("guild")] public GuildData? Data;
#nullable disable
    }

    public class GuildData
    {
        [JsonProperty("_id")] public string GuildID;
        [JsonProperty("name")] public string Name;
        [JsonProperty("members")] public GuildMember[] Members;
    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace MonkeyBot.Data
{
    public class GuildMember
    {
        [JsonProperty("uuid")] public string UUID;
        [JsonProperty("rank")] public string Rank;
        [JsonProperty("joined")] public ulong JoinTimestamp;
        [JsonProperty("questParticipation")] public uint QuestParticipated;
        [JsonProperty("expHistory")] public IDictionary<string, int> ExperienceHistory;
    }
}

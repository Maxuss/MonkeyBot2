using Discord.Commands;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Discord;

namespace MonkeyBot.Commands.Fun
{
    /// <summary>
    /// this class is used for a single dungeon command. yeah
    /// </summary>
    public class DungeonModule : ModuleBase<SocketCommandContext>
    {
        [Command("dungeon")]
        [Summary("Simulates a dungeon run")]
        public async Task DungeonAsync([Summary("Floor to run, e.g. F1, or M3")]
            string floor, [Summary("Will the run be frag run")] bool frag)
        {
            IUserMessage message = await ReplyAsync("Calculating dungeon run, please wait...");
            
            await message.ModifyAsync(m =>
            {
                m.Content = "TEST";
            });
        }

        private string[] _dungeonFloors =
        {
            "f0", "f1", "f2", "f3", "f4", "f5", "f6", "f7",
            "m1", "m2", "m3", "m4", "m5", "m6"
        };
        
        public async Task<DungeonRunData> CalculateDungeonAsync(string floor, bool frag)
        {
            string of = floor.ToLower();
            if (!_dungeonFloors.Contains(of)) return null;
            
        }
    }

    public sealed class DungeonRunData
    {
        public long Profit;
        public string[] ChestLoot;
        public string[] Drops;
        public string HighestProfitItem;
        public long TotalItemAmount;

        public DungeonRunData(long p, string[] cl, string[] d, string hpi, long tia)
        {
            Profit = p;
            ChestLoot = cl;
            Drops = d;
            HighestProfitItem = hpi;
            TotalItemAmount = tia;
        }
}
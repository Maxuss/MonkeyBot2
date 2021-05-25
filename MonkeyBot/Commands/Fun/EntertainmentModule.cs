using Discord.Commands;
using System;
using System.Threading.Tasks;
using Discord;
using Discord.Net;
using MonkeyBot.Data;

namespace MonkeyBot.Commands.Fun
{
    public class EntertainmentModule : ModuleBase<SocketCommandContext>
    {
        [Command("echo")]
        [Summary("Echoes whatever you say")]
        public Task EchoAsync([Remainder] [Summary("The text to say")]
            string echo) => ReplyAsync(echo);

        [Command("choose")]
        [Summary("Chooses between 2 or more variants you give, separated by commas")]
        public async Task ChooseAsync([Remainder] [Summary("Variants, separated by comma")] string vars)
        {
            if (!vars.Contains(","))
            {
                await Context.Channel.SendMessageAsync(
                    "You have to provide 2 or more options, separated by commas! Try again!");
            }
            else
            {
                string[] options = vars.Split(",");
                Random r = new Random();
                string chosen = options[r.Next(0, options.Length - 1)];
                await ReplyAsync($"I choose {chosen}!");
            }
        }

        [Command("eat")]
        [Summary("Make monke eat something oo aa")]
        public async Task EatAsync(
            [Remainder] [Summary("The thing to eat")]
            string food)
        {
            string chosen = EatData.Choose(food);
            await ReplyAsync(chosen);
        }
    }
}
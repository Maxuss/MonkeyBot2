using System;
using MonkeyBot.Helpers;
using Discord;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using MonkeyBot.Commands;

namespace MonkeyBot
{
    public class Program
    {
        private DiscordSocketClient _client;
	
        public static void Main(string[] args)
            => new Program().MainAsync().GetAwaiter().GetResult();

        public async Task MainAsync()
        {
            var _config = new DiscordSocketConfig { MessageCacheSize = 100 };
            _client = new DiscordSocketClient();
            _client.Log += Log;
            string token = Inner.GetDiscordToken();
            Console.WriteLine(token);
            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();
            _client.MessageUpdated += MessageUpdated;
            _client.Ready += () =>
            {
                Console.WriteLine("Connected successfully");
                return Task.CompletedTask;
            };
            CommandHandler handler = new CommandHandler(_client, new CommandService());
            await handler.InstallCommandsAsync();
            await Task.Delay(-1);
        }
        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }

        private async Task MessageUpdated(Cacheable<IMessage, ulong> before, SocketMessage after, ISocketMessageChannel channel)
        {
            // If the message was not in the cache, downloading it will result in getting a copy of `after`.
            var message = await before.GetOrDownloadAsync();
            Console.WriteLine($"{message} -> {after}");
        }
    }
}
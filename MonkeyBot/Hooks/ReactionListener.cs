using System.Linq;
using Discord;
using System.Threading.Tasks;
using Discord.WebSocket;
using MonkeyBot.Commands;
using MonkeyBot.Helpers;
using static MonkeyBot.Data.Constants;
namespace MonkeyBot.Hooks
{
    internal class ReactionListener
    {

        public void HookReactionAdded(BaseSocketClient client)
            => client.ReactionAdded += HandleReactionAddedAsync;

        public async Task HandleReactionAddedAsync(Cacheable<IUserMessage, ulong> cachedMessage,
            ISocketMessageChannel originChannel, SocketReaction reaction)
        {
            var message = await cachedMessage.GetOrDownloadAsync();
            if (message != null && reaction.User.IsSpecified && message.Author.Username.Equals("MonkeyBot 2") && !reaction.User.Value.IsBot)
            {
                switch (message.Embeds.First().Title)
                {
                    case "MonkeyBotV2 Help":
                        // IT WORKS FINALLY
                        await message.RemoveReactionAsync(
                            reaction.Emote,
                            reaction.UserId);
                        HelpEmbed e = InformationModule.e;
                        switch (reaction.Emote.Name)
                        {
                            case ARROW_FORWARD:
                                if (e.Current < e.Embeds.Length - 1) e.Current++;
                                else e.Current = e.Embeds.Length-1;
                                await message.ModifyAsync(m =>
                                {
                                    m.Embed = e.Embeds[e.Current];
                                });
                                break;
                            case ARROW_BACKWARD:
                                if (e.Current > 0) e.Current--;
                                else e.Current = 0;
                                await message.ModifyAsync(m =>
                                {
                                    m.Embed = e.Embeds[e.Current];
                                });
                                break;
                            default:
                                goto case ARROW_FORWARD;
                        }
                        break;
                }
            } 
        }
    }
}
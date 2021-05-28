using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.Net;
using MonkeyBot.Helpers;

namespace MonkeyBot.Commands.Tests
{
    [Group("test")]
    public class Testing : ModuleBase<SocketCommandContext>
    {
        private string KEY = Inner.GetHypixelKey();
    }
}

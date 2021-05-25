using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

public class LoggingService
{
    private TextWriter writer;
    
    public LoggingService(DiscordSocketClient client, CommandService command)
    {
        Directory.CreateDirectory($"{Directory.GetCurrentDirectory()}\\log\\");
        File.Create($"{Directory.GetCurrentDirectory()}\\log\\latest.txt");
        writer = new StreamWriter($"{Directory.GetCurrentDirectory()}\\log\\latest.txt", true, Encoding.UTF8);
        client.Log += LogAsync;
        command.Log += LogAsync;
    }
    private Task LogAsync(LogMessage message)
    {
        string msg;
        if (message.Exception is CommandException cmdException)
        {
            msg = $"[Command/{message.Severity}] {cmdException.Command.Aliases.First()}"
                  + $" failed to execute in {cmdException.Context.Channel}.";
            Console.WriteLine(msg);
            Console.WriteLine(cmdException);
            writer.WriteLine(msg);
            writer.WriteLine(cmdException);
        }
        else
        {
            msg = $"[General/{message.Severity}] {message}";
            Console.WriteLine(msg);
            writer.WriteLine(msg);
        }

        return Task.CompletedTask;
    }
}
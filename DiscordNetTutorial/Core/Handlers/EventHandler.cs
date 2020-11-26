using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DiscordNetTutorial.Core.Handlers
{
    public class EventHandler
    {
        private DiscordSocketClient Client { get; set; }
        private CommandService Commands { get; set; }
        private ILogger Logger { get; set; }
        private IServiceProvider _Services { get; set; }

        public EventHandler(IServiceProvider Services)
        {
            Client = Services.GetRequiredService<DiscordSocketClient>();
            Commands = Services.GetRequiredService<CommandService>();
            Logger = Services.GetRequiredService<ILogger>();
            _Services = Services;
        }

        public Task InitAsync()
        {
            Client.Log += Client_Log;
            Commands.Log += Commands_Log;
            Client.Ready += Ready_Event;
            Client.MessageReceived += Message_Event;
            return Task.CompletedTask;
        }

        private async Task Message_Event(SocketMessage arg)
        {
            var Message = arg as SocketUserMessage;
            var Context = new SocketCommandContext(Client, Message);

            if (Message.Author.IsBot || Message.Channel is IDMChannel) return;

            int ArgPos = 0;

            if (!(Message.HasStringPrefix(Config.Bot.Prefix, ref ArgPos) || Message.HasMentionPrefix(Client.CurrentUser, ref ArgPos))) return;

            var Result = await Commands.ExecuteAsync(Context, ArgPos, _Services);
            
            if(!Result.IsSuccess)
            {
                if (Result.Error == CommandError.UnknownCommand) return;
            }
        }

        private async Task Ready_Event()
        {
            Logger.Information($"{Client.CurrentUser.Username} is ready.");
            await Client.SetStatusAsync(UserStatus.Online);
            await Client.SetGameAsync($"{Config.Bot.Prefix}help | In {Client.Guilds.Count} Servers");
        }

        private Task Commands_Log(Discord.LogMessage arg)
        {
            Logger.Information($"Source: [{arg.Source}] => {arg.Message}");
            return Task.CompletedTask;
        }

        private Task Client_Log(Discord.LogMessage arg)
        {
            Logger.Information($"Source: [{arg.Source}] => {arg.Message}");
            return Task.CompletedTask;
        }
    }
}

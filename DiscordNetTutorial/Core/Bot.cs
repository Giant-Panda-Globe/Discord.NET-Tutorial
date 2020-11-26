using Discord;
using Discord.Addons.Interactive;
using Discord.Commands;
using Discord.WebSocket;
using DiscordNetTutorial.Core.Handlers;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DiscordNetTutorial.Core
{
    public class Bot
    {
        private DiscordSocketClient Client { get; set; }
        private CommandService Commands { get; set; }
        private InteractiveService Interactive { get; set; }
        private IServiceProvider Services { get; set; }

        public Bot()
        {
            Client = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Debug
            });

            Commands = new CommandService(new CommandServiceConfig
            {
                LogLevel = LogSeverity.Debug,
                CaseSensitiveCommands = true,
                DefaultRunMode = RunMode.Async,
                IgnoreExtraArgs = true
            });

            Interactive = new InteractiveService(Client, new InteractiveServiceConfig
            {
                DefaultTimeout = TimeSpan.FromMinutes(2)
            });

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Debug)
                .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Information)
                .WriteTo.Console(
                    outputTemplate: "{Timestamp:HH:mm:ss} - [{Level:u4}] => {Message:lj}{NewLine}{Exception}"
                )
                .CreateLogger();

            Services = BuildServiceProvder();
        }

        public async Task MainAsync()
        {
            if (string.IsNullOrWhiteSpace(Config.Bot.GetToken())) return;
            await new CommandHandler(Services).InitAsync();
            await new Handlers.EventHandler(Services).InitAsync();
            await Client.LoginAsync(TokenType.Bot, Config.Bot.GetToken());
            Config.Bot.DisposeToken();
            await Client.StartAsync();
            await Task.Delay(-1);
        }

        private ServiceProvider BuildServiceProvder()
            => new ServiceCollection()
            .AddSingleton(Client)
            .AddSingleton(Commands)
            .AddSingleton(Interactive)
            .AddSingleton(Log.Logger)
            .BuildServiceProvider();
    }
}

using Discord.Commands;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DiscordNetTutorial.Core.Handlers
{
    public class CommandHandler
    {
        private CommandService Commands { get; set; }
        private ILogger Logger { get; set; }
        private IServiceProvider Services { get; set; }

        public CommandHandler(IServiceProvider services)
        {
            Commands = services.GetRequiredService<CommandService>();
            Logger = services.GetRequiredService<ILogger>();
            Services = services;
        }

        public async Task InitAsync()
        {
            await Commands.AddModulesAsync(Assembly.GetEntryAssembly(), Services);
            foreach (var command in Commands.Commands)
                Logger.Information($"Command {command.Name} was loaded.");
        }
    }
}

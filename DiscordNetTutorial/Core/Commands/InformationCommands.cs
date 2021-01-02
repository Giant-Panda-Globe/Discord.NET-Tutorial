using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordNetTutorial.Core.Commands
{
    public class InformationCommands : ModuleBase<SocketCommandContext>
    {
        
        private CommandService CommandService { get; set; }

        public InformationCommands(IServiceProvider serviceProvider) =>
            CommandService = serviceProvider.GetRequiredService<CommandService>();

        [Command("latency")]
        [Alias("ping")]
        [Summary("Display my latency")]
        public async Task LatencyCommand()
        {
            var builder = new EmbedBuilder
            {
                Title = "Pong!",
                Description = $"My Latency is {Context.Client.Latency} ms.",
                Color = Context.Client.Latency < 100 ?
                    Color.Green : Context.Client.Latency < 200 ?
                    Color.Orange : Color.Red
            };

            await Context.Channel.SendMessageAsync(embed: builder.Build());
        }

        [Command("whois")]
        [Description("Displays information about a user in the server or, information about your account.")]
        public async Task WhoisCommand(string user = null)
        {
            var builder = new EmbedBuilder();
            builder.WithColor(Utilites.Random.GetRandomColor());
            if (user is null)
            {
                var member = Context.Guild.GetUser(Context.User.Id);
                builder.WithTitle($"Information on ${member.Username}");
                builder.WithDescription($"Status: {member.Status.ToString()}");
                builder.AddField("Created On", member.CreatedAt);
                builder.AddField("Joined On", member.JoinedAt);
            }
            else
            {
                SocketGuildUser member;
                if (Context.Message.MentionedUsers.Count == 0)
                    member = (SocketGuildUser) Context.Message.MentionedUsers.FirstOrDefault();
                else
                    member = Utilites.User.GetUser(user, Context);

                if (member is null)
                {
                    await Context.Channel.SendMessageAsync(
                        "I couldn't find that user. This could mean that the user you asked for is a bot, or they are not in the server.");
                    return;
                }
                
                builder.WithTitle($"Information on ${member.Username}");
                builder.WithDescription($"Status: {member.Status.ToString()}");
                builder.AddField("Created On", member.CreatedAt);
                builder.AddField("Joined On", member.JoinedAt);
            }

            await Context.Channel.SendMessageAsync(embed: builder.Build());
        }

        /// Whois Command Algorthim
        ///
        /// Description: Displays information about a member in the server, or information about the author's account.
        /// Usage: +whois | +whois @member | +whois id | +whois username
        /// Aliases: []
        /// 
        /// User <- The user that is executing the command.
        /// user : ? <- Will be used to search for the Member that the User asked for.
        /// Member: ? <- The member in the server that the User is asking for.
        ///
        /// BEGIN WHOIS_COMMAND
        ///
        /// 1. User sends message 
        /// 2. Check to see if the user wants information about a user, or not.
        /// 2.A. If false -> GO TO IF_USER_DOESNT_WANT_INFO_ON_A_USER
        /// 2.B. If true -> GO TO IF_USER_WANTS_INFO_ON_A_USER
        ///
        /// BEGIN IF_USER_DOESNT_WANT_INFO_ON_A_USER
        /// 
        /// Builder : EmbedBuilder <- The embed that will be sent with the User's information. 
        /// 1. Get the user's information 
        /// 2. Make the Builder and put the information of the User in Builder. 
        /// 3. Send Builder as a Embed. 
        ///
        /// END IF_USER_DOESNT_WANT_INFO_ON_A_USER 
        ///
        /// BEGIN IF_USER_WANTS_INFO_ON_A_USER
        /// 
        /// Builder : EmbedBuilder <- The embed that will be sent with the Member's information. 
        /// 1. Get the Member's information.
        /// 1.A. ???
        /// 2. Make the Builder and put the information of the Member in Builder.
        /// 3. Send Builder as a Embed. 
        /// 
        /// END IF_USER_WANTS_INFO_ON_A_USER
        /// 
        /// END WHOIS_COMMAND
        [Command("help")]
        [Description("Displays a list of commands, or information about a specific command.")]
        [Alias("command", "commands")]
        public async Task HelpCommand(string name = null)
        {
            var builder = new EmbedBuilder();
            builder.WithColor(Utilites.Random.GetRandomColor());
            if (name is null)
            {
                builder.WithTitle("List of commands");
                builder.WithDescription($"Prefix: ${Config.Bot.Prefix}\nTotal Commands: {CommandService.Commands.ToList().Count}");

                foreach (var module in CommandService.Modules)
                {
                    var desc = string.Empty;
                    foreach (var command in module.Commands)
                        desc += $"{Config.Bot.Prefix}${command.Name}\n";

                    builder.AddField($"{module.Name} [Total Commands: {module.Commands.Count}]", desc);
                }
            }
            else
            {
                var command = CommandService.Search(name).Commands.FirstOrDefault().Command;
                if (command is null)
                {
                    await Context.Channel.SendMessageAsync($"Couldn't find a command like ${name}");
                    return;
                }

                builder.WithTitle($"Information on {command.Name}");
                builder.WithDescription($"Description: {command.Summary}");
                foreach (var arg in command.Parameters)
                    builder.AddField($"Argument: {arg.Name} [Type: {Utilites.Other.GetType(arg.Type.ToString())}]", arg.Summary ?? "\u200b");
            }

            await Context.Channel.SendMessageAsync(embed: builder.Build());
        }
        /// Help Command Algorthim
        /// 
        /// Description: Displays a list of commands, or information about a specific command.
        /// Usage: +help | +help command_name
        /// Aliases: [command, commands]
        /// 
        /// User <- The user that is executing the command 
        /// param Name: string => null <- The command name of the command that User asked for. 
        ///
        /// BEGIN HELP_COMMAND
        /// 1. User sends message 
        /// 1.A. Name is string | null 
        /// 2. Check Name to see if the User wants the list or info. 
        /// 2.A. If Name is null -> IF_NAME_IS_NULL. 
        /// 2.B. If Name is string -> IF_NAME_IS_STRING. 
        ///
        /// BEGIN IF_NAME_IS_NULL 
        /// Sends a embed with a list of commands.
        /// Builder: EmbedBuilder <- The embed with the list of commands. 
        ///
        /// 1. Get all of the modules 
        /// 2. Get all commands in the modules 
        /// 3. Put in the the commands that are that module into a field of the embed. 
        /// 4. Send embed. 
        /// 
        /// END IF_NAME_IS_NULL
        ///
        /// BEGIN IF_NAME_IS_STRING
        /// Sends a embed with information about the command that the User asked for.
        /// Builder: EmbedBuilder <- The embed with the list of commands. 
        /// Command: CommandInfo <- The command that the User asked for. 
        ///
        /// 1. Get the Command from the CommandService, by Name. 
        /// 2. Check If Command is ? -> Return a message saying that the command doesn't exist. 
        /// 2.A. Check if Command is ? -> Send a Embed with the command Information. 
        /// END IF_NAME_IS_STRING
        ///
        /// END HELP_COMMAND

    }
}

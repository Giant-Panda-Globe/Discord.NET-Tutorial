using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DiscordNetTutorial.Core.Commands
{
    public class InformationCommands : ModuleBase<SocketCommandContext>
    {
        [Command("latency")]
        [Alias("ping")]
        [Summary("Display my latency")]
        [Remarks("+latency, +ping")]
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
    }
}

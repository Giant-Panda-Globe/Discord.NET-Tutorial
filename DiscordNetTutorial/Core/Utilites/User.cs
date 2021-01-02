using System.Linq;
using Discord.Commands;
using Discord.WebSocket;

namespace DiscordNetTutorial.Core.Utilites
{
    internal static class User
    {
        public static SocketGuildUser GetUser(string name, SocketCommandContext ctx)
        {
            if (ulong.TryParse(name, out var result))
                return ctx.Guild.GetUser(result);
            else 
                return ctx.Guild.Users.FirstOrDefault(x => x.Username.Trim().ToLower() == name.Trim().ToLower());
        }
    }
}
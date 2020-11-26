using DiscordNetTutorial.Core;
using System;

namespace DiscordNetTutorial
{
    class Program
    {
        static void Main(string[] args)
            => new Bot().MainAsync().GetAwaiter().GetResult();
    }
}

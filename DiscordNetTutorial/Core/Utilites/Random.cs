using Discord;

namespace DiscordNetTutorial.Core.Utilites
{
    internal static class Random
    {
        public static int RandomInt(int min, int max)
        {
            System.Random rand = new System.Random();
            return rand.Next(min, max);
        }

        public static Color GetRandomColor()
        {
            var r = RandomInt(0, 255);
            var b = RandomInt(0, 255);
            var g = RandomInt(0, 255);
            
            return new Color(r, g, b);
        }
    }
}
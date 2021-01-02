using System.Collections.Generic;

namespace DiscordNetTutorial.Core.Utilites
{
    internal static class Other
    {
        public static string GetType(string type)
        {
            var types = new Dictionary<string, string>()
            {
                {"System.String", "Text"},
                {"System.Int16", "Number"},
                {"System.Int32", "Number"},
                {"System.Int64", "Number"},
                {"System.UInt16", "Number"},
                {"System.UInt32", "Number"},
                {"System.UInt64", "ID"}
            };

            if (types.ContainsKey(type))
                return types[type];
            return type;
        }
    }
}
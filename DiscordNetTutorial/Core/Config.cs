using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DiscordNetTutorial.Core
{
    class Config
    {
        private static string ConfigFolder = "Resources";
        private static string ConfigFile = "config.json";
        private static string ConfigPath = ConfigFolder + "/" + ConfigFile;
        public static BotConfig Bot;

        static Config()
        {
            if (!Directory.Exists(ConfigFolder))
                Directory.CreateDirectory(ConfigFolder);
            if (!File.Exists(ConfigPath))
            {
                Bot = new BotConfig();
                var json = JsonConvert.SerializeObject(Bot, Formatting.Indented);
                File.WriteAllText(ConfigPath, json);
            }
            else
            {
                var json = File.ReadAllText(ConfigPath);
                Bot = JsonConvert.DeserializeObject<BotConfig>(json);
            }
        }
    }

    public struct BotConfig
    {
        [JsonProperty("token")]
        private string Token { get; set; }
        [JsonProperty("prefix")]
        public string Prefix { get; private set; }

        public string GetToken()
        {
            return Token;
        }

        public void DisposeToken()
        {
            Token = "";
        }
    }
}

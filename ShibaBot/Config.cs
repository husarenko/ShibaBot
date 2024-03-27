using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShibaBot
{
    class Config
    {

        [JsonProperty("TelegramBotToken")]
        public string TelegramBotToken { get; set; }

        public static Config LoadConfig(string path)
        {
            try
            {
                string json = File.ReadAllText(path);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("> Файл JSON (ваш ключ):");
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine(json);
                Console.WriteLine();
                return JsonConvert.DeserializeObject<Config>(json);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Не вдалось завантажити токен телеграм бота: " + ex.Message);
                return null;
            }
        }
    }
}

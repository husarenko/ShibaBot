using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using System.Net.Http;
using System.Text.Json;
using Newtonsoft.Json;
using ShibaBot;

class Program
{
    static void Main(string[] args)
    {
        var config = Config.LoadConfig("config.json");
        if (config != null)
        {
            var client = new TelegramBotClient(config.TelegramBotToken);

            client.StartReceiving(Update, Error);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"> Бота увiмкнено! ({DateTime.UtcNow} UTC)");

            while (true)
            {
                string exitKeyword = "exit";

                string input = Console.ReadLine();

                if (input.ToLower() == exitKeyword)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"> Бота вимкнено! ({DateTime.UtcNow} UTC)");
                    break;
                }
                else
                {
                    Console.ReadLine();
                }
            }
        }
        else
        {
            Console.WriteLine("Failed to load config. Exiting...");
        }
    }

    static async Task Error(ITelegramBotClient client, Exception exception, CancellationToken token)
    {
        
    }

    static async Task Update(ITelegramBotClient botClient, Update update, CancellationToken token)
    {
        //інформація від користувача
        var message = update.Message;

        //кнопки "швидка відповідь"
        ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
        {
            new KeyboardButton[] { "Дай сiбу!" },
            new KeyboardButton[] { "Про бота" },
        })
        {
            //під розмір екрану
            ResizeKeyboard = true
        };

        if (message != null)
        {
            //вивід інформації про користувача у консоль
            Console.WriteLine($"({message.Date.ToLocalTime()}) {message.Chat.Id}, {message.Chat.Username}: {message.Text}");

            switch (message.Text)
            {
                //запуск бота
                case "/start":
                    await BotActions.StartAsync(botClient, message.Chat.Id, replyKeyboardMarkup, token);
                    break;
                //про бота
                case "Про бота":
                    await BotActions.AboutAsync(botClient, message.Chat.Id, token);
                    break;
                //основна дія
                case "Дай сiбу!":
                    await BotActions.GetShibeImageAsync(botClient, message.Chat.Id, token);
                    break;
                //ігнорування не типічних повідомлень
                default:
                    await BotActions.DefaultAsync(botClient, message.Chat.Id, replyKeyboardMarkup, token);
                    break;
                }
            }
        }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types;
using Telegram.Bot;
using Newtonsoft.Json;

namespace ShibaBot
{
    public static class BotActions
    {
        public static async Task StartAsync(ITelegramBotClient botClient, long chatId, ReplyKeyboardMarkup keyboardMarkup, CancellationToken token)
        {
            await botClient.SendStickerAsync(chatId, 
                sticker: InputFile.FromUri("https://d.newsweek.com/en/full/1925994/shiba-inu.webp"), 
                cancellationToken: token);
            await botClient.SendTextMessageAsync(chatId, 
                text: "🐶<b>Вітаю!</b>\nЦе <b>ShibaBot</b>, створений щоб розповсюджувати <b>радість та сiбiків</b>!", 
                parseMode: ParseMode.Html);
            await botClient.SendTextMessageAsync(chatId, 
                text: "👇<b>Обери дію</b>", 
                replyMarkup: keyboardMarkup, 
                cancellationToken: token, 
                parseMode: ParseMode.Html);
        }

        public static async Task AboutAsync(ITelegramBotClient botClient, long chatId, CancellationToken token)
        {
            await botClient.SendTextMessageAsync(chatId, 
                text: "❓<b>Бот створений під час виконання практичної роботи №2 з АППЗ .Net</b>\n\nБот використовує:\n- Telegram.Bot,\n- Visual Studio 2022,\n- https://shibe.online/ у якості API.", 
                parseMode: ParseMode.Html);
        }

        public static async Task DefaultAsync(ITelegramBotClient botClient, long chatId, ReplyKeyboardMarkup keyboardMarkup, CancellationToken token)
        {
            await botClient.SendTextMessageAsync(chatId, 
                text: "Не розумію :( \n\n👇<b>Обери дію</b>", 
                replyMarkup: keyboardMarkup, 
                cancellationToken: token, 
                parseMode: ParseMode.Html);
        }

        public static async Task GetShibeImageAsync(ITelegramBotClient botClient, long chatId, CancellationToken token)
        {
            var root = "https://shibe.online/api/shibes?count=1&urls=true&httpsUrls=true";
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(root, token);

                    if (response.IsSuccessStatusCode)
                    {
                        string jsonResult = await response.Content.ReadAsStringAsync();

                        string[] shibeUrls = JsonConvert.DeserializeObject<string[]>(jsonResult);

                        string firstShibeUrl = shibeUrls.FirstOrDefault();
                        Console.WriteLine($"Завантажена свiтлина: {firstShibeUrl}");
                        if (firstShibeUrl != null)
                        {
                            await botClient.SendPhotoAsync(chatId, 
                                photo: InputFile.FromUri(firstShibeUrl), 
                                caption: $"🐶<b>Твiй сiбiк!</b>\n<a href = '{firstShibeUrl}'>Джерело</a>", 
                                parseMode: ParseMode.Html, 
                                cancellationToken: token);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Помилка при виконанні запиту: " + response.StatusCode);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Помилка: " + ex.Message);
                }
            }
        }
    }
}

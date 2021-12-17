using Bot.Entities;
using Bot.Entities.Enums;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Bot.Messages
{
    public static class ChooseRecordDetails
    {
        public static async Task SendMessage(ITelegramBotClient botClient, Chat chat, Record record)
        {
            await botClient.SendTextMessageAsync(chatId: chat.Id,
                                                text: $"Опишите более подробнее ваш Заказ/Услугу");
        }
    }
}

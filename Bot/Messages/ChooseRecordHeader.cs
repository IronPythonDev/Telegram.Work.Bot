using Bot.Entities;
using Bot.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Bot.Messages
{
    public static class ChooseRecordHeader
    {
        public static async Task SendMessage(ITelegramBotClient botClient, Chat chat, Record record)
        {
            await botClient.SendTextMessageAsync(chatId: chat.Id,
                                                text: $"Отправьте заголовок для вашего Заказа/Услуги\nНапример: C# Developer, Нужен Full-Stack в команду");
        }
    }
}

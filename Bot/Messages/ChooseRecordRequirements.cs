using Bot.Entities;
using Bot.Entities.Enums;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Bot.Messages
{
    public static class ChooseRecordRequirements
    {
        public static async Task SendMessage(ITelegramBotClient botClient, Chat chat, Record record)
        {
            await botClient.SendTextMessageAsync(chatId: chat.Id,
                                                text: $"Отправьте ваши требования к заказчику или исполнителю\nНапример: Знание SQL, C#, TS");
        }
    }
}

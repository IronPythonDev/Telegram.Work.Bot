using Bot.Entities.Enums;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Bot.Messages
{
    public static class ChooseSection
    {
        public static async Task SendMessage(ITelegramBotClient botClient, Chat chat)
        {
            InlineKeyboardMarkup inlineKeyboard = new(
            new[]
            {
                new []
                {
                    InlineKeyboardButton.WithCallbackData("🤵Заказы", $"send_records {RecordType.Order}"),
                    InlineKeyboardButton.WithCallbackData("👷‍♂️Услуги", $"send_records {RecordType.CV}"),
                },
                new []
                {
                    InlineKeyboardButton.WithCallbackData("🟢Создать заказ", $"create_record {RecordType.Order}"),
                    InlineKeyboardButton.WithCallbackData("➕Создать услугу", $"create_record {RecordType.CV}"),
                }
            });

            await botClient.SendTextMessageAsync(chatId: chat.Id,
                                                text: "Меню",
                                                replyMarkup: inlineKeyboard);
        }
    }
}

using Bot.Entities;
using Bot.Entities.Enums;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Bot.Messages
{
    public static class ChooseTargetMP
    {
        public static async Task SendMessage(ITelegramBotClient botClient, Chat chat, int recordId)
        {
            InlineKeyboardMarkup inlineKeyboard = new(
            new[]
            {
                new []
                {
                    InlineKeyboardButton.WithCallbackData("alt:V", $"update_record {recordId} {nameof(Record.MultiplayerType)} {(int)MultiplayerType.AltV}"),
                    InlineKeyboardButton.WithCallbackData("FiveM", $"update_record {recordId} {nameof(Record.MultiplayerType)} {(int)MultiplayerType.FiveM}"),
                    InlineKeyboardButton.WithCallbackData("RageMP", $"update_record {recordId} {nameof(Record.MultiplayerType)} {(int)MultiplayerType.RageMP}"),
                    InlineKeyboardButton.WithCallbackData("Другие/Все", $"update_record {recordId} {nameof(Record.MultiplayerType)} {(int)MultiplayerType.Other}"),
                }
            });

            await botClient.SendTextMessageAsync(chatId: chat.Id,
                                                text: "Если еще не знаете с каким мультиплеером связан ваш заказ выберете Другие/Все",
                                                replyMarkup: inlineKeyboard);
        }
    }
}

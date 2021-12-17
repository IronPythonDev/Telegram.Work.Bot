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
    public static class SendRecords
    {
        public static async Task SendRecordsMessage(ITelegramBotClient botClient, Chat chat, IList<Record> records)
        {

            var keyboards = new List<List<InlineKeyboardButton>>(); 

            InlineKeyboardMarkup inlineKeyboardMarkup = new(keyboards);

            foreach (var record in records)
            {
                keyboards.Add(new List<InlineKeyboardButton>()
                {
                    InlineKeyboardButton.WithCallbackData(await GetSmallRecordString(record), $"record_details {record.Id}")
                });
            }

            await botClient.SendTextMessageAsync(chatId: chat.Id,
                                                text: $"Было Найдено {records.Count} записей",
                                                replyMarkup: inlineKeyboardMarkup);
        }

        public static async Task SendRecordDetailsMessage(ITelegramBotClient botClient, Chat chat, Record record, List<List<InlineKeyboardButton>>? keyboardButtons = null)
        {
            await botClient.SendTextMessageAsync(chatId: chat.Id,
                                                text: await GetFullRecordString(record),
                                                parseMode: Telegram.Bot.Types.Enums.ParseMode.Html,
                                                replyMarkup: keyboardButtons != null ? new InlineKeyboardMarkup(keyboardButtons) : null);
        }

        public static Task<string> GetFullRecordString(Record record)
        {
            var @string = new StringBuilder();

            @string.AppendLine($"<b>{record.Header} -> {(MultiplayerType)record.MultiplayerType}</b>");
            @string.AppendLine($"");
            @string.AppendLine($"Требования:");
            @string.AppendLine($"{record.Requirements}");
            @string.AppendLine($"");
            @string.AppendLine($"Подробности:");
            @string.AppendLine($"{record.Details}");

            return Task.FromResult(@string.ToString());
        }

        public static Task<string> GetSmallRecordString(Record record)
        {
            var @string = new StringBuilder();

            @string.AppendLine($"{record.Header} -> {(MultiplayerType)record.MultiplayerType}");

            return Task.FromResult(@string.ToString());
        }
    }
}

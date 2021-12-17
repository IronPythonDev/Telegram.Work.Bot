using Bot.Entities.Enums;
using Bot.Infrastructure.Abstractions;
using Bot.Infrastructure.Attributes;
using Bot.Infrastructure.Repositories;
using IronPython.Infrastructure.Abstractions.Exceptions;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Bot.CallbackQueryHandlers
{
    [CallbackQueryCommand("record_details")]
    public class RecordDetails : ICallbackQueryHandler
    {
        RecordRepository RecordRepository { get; }
        UserRepository UserRepository { get; }

        public RecordDetails(RecordRepository recordRepository, UserRepository userRepository)
        {
            RecordRepository = recordRepository;
            UserRepository = userRepository;
        }

        public async Task Handler(ITelegramBotClient botClient, CallbackQuery callbackQuery, string[] args)
        {
            try
            {
                var recordId = int.Parse(args[1]);

                var user = await UserRepository.GetByColumn(nameof(Entities.User.TelegramId), $"{callbackQuery.From.Id}");

                var record = await RecordRepository.GetById(recordId);

                var recordOwner = await UserRepository.GetById(record.OwnerId);

                await botClient.AnswerCallbackQueryAsync(
                   callbackQueryId: callbackQuery.Id,
                   text: $"Подробности успешно сформулированы и скоро будут отправлены");

                var inlineKeyboard = new List<List<InlineKeyboardButton>>();

                if (record.OwnerId == user.Id)
                {
                    inlineKeyboard.Add(new List<InlineKeyboardButton>
                    {
                        InlineKeyboardButton.WithCallbackData($"❌Удалить", $"remove_record {record.Id}")
                    });
                }

                inlineKeyboard.Add(new List<InlineKeyboardButton>
                {
                    InlineKeyboardButton.WithUrl($"✍️Написать!", $"https://t.me/{recordOwner.TelegramUserName}")
                });

                await Messages.SendRecords.SendRecordDetailsMessage(botClient, callbackQuery.Message.Chat, record, inlineKeyboard);
            }
            catch (NotFoundException ex)
            {
                await botClient.AnswerCallbackQueryAsync(
                     callbackQueryId: callbackQuery.Id,
                     text: $"Заказ/Услуга ранее была удалена");

                //await botClient.SendTextMessageAsync(
                //    chatId: callbackQuery.Message.Chat.Id,
                //    text: ex.Message);
            }

        }
    }
}

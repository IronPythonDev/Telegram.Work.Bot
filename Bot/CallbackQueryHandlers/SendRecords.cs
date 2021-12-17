using Bot.Entities.Enums;
using Bot.Infrastructure.Abstractions;
using Bot.Infrastructure.Attributes;
using Bot.Infrastructure.Repositories;
using IronPython.Infrastructure.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Bot.CallbackQueryHandlers
{
    [CallbackQueryCommand("send_records")]
    public class SendRecords : ICallbackQueryHandler
    {
        RecordRepository RecordRepository { get; }
        UserRepository UserRepository { get; }

        public SendRecords(RecordRepository recordRepository, UserRepository userRepository)
        {
            RecordRepository = recordRepository;
            UserRepository = userRepository;
        }

        public async Task Handler(ITelegramBotClient botClient, CallbackQuery callbackQuery, string[] args)
        {
            try
            {
                var recordType = Enum.Parse<RecordType>(args[1]);

                var user = await UserRepository.GetByColumn(nameof(Entities.User.TelegramId), $"{callbackQuery.From.Id}");

                var records = await RecordRepository.GetRecordsByType(recordType);

                await botClient.AnswerCallbackQueryAsync(
                   callbackQueryId: callbackQuery.Id,
                   text: $"Успешно сформирован список Заказов/Услуг");

                await Messages.SendRecords.SendRecordsMessage(botClient, callbackQuery.Message.Chat, records);
            }
            catch (NotFoundException ex)
            {
                await botClient.SendTextMessageAsync(
                    chatId: callbackQuery.Message.Chat.Id,
                    text: ex.Message);
            }

        }
    }
}

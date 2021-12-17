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
    [CallbackQueryCommand("remove_record")]
    public class RemoveRecord : ICallbackQueryHandler
    {
        RecordRepository RecordRepository { get; }
        UserRepository UserRepository { get; }

        public RemoveRecord(RecordRepository recordRepository, UserRepository userRepository)
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

                if (record.OwnerId != user.Id) return;

                await RecordRepository.DeleteById(recordId);

                await botClient.AnswerCallbackQueryAsync(
                   callbackQueryId: callbackQuery.Id,
                   text: $"Заказ/Услуга была успешно удалена");

                await botClient.DeleteMessageAsync(callbackQuery.Message.Chat.Id, callbackQuery.Message.MessageId);
            }
            catch (NotFoundException)
            {
                await botClient.AnswerCallbackQueryAsync(
                   callbackQueryId: callbackQuery.Id,
                   text: $"Заказ/Услуга ранее была уже успешно удалена");
                //await botClient.SendTextMessageAsync(
                //    chatId: callbackQuery.Message.Chat.Id,
                //    text: ex.Message);
            }
        }
    }
}

using Bot.Entities.Enums;
using Bot.Infrastructure.Abstractions;
using Bot.Infrastructure.Attributes;
using Bot.Infrastructure.Repositories;
using Bot.Messages;
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
    [CallbackQueryCommand("create_record")]
    public class CreateRecord : ICallbackQueryHandler
    {
        RecordRepository RecordRepository { get; }
        UserRepository UserRepository { get; }

        public CreateRecord(RecordRepository recordRepository, UserRepository userRepository)
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

                var record = await RecordRepository.Add(new()
                {
                    OwnerId = user.Id,
                    Type = (int)recordType,
                });

                await botClient.AnswerCallbackQueryAsync(
                   callbackQueryId: callbackQuery.Id,
                   text: $"Заказ/Услуга была успешно создана");

                await ChooseTargetMP.SendMessage(botClient, callbackQuery.Message.Chat, record.Id);
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

using Bot.Entities.Enums;
using Bot.Infrastructure.Abstractions;
using Bot.Infrastructure.Attributes;
using Bot.Infrastructure.Repositories;
using Bot.Messages;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Bot.CallbackQueryHandlers
{
    [CallbackQueryCommand("update_record")]
    public class UpdateRecord : ICallbackQueryHandler
    {

        RecordRepository RecordRepository { get; }
        UserRepository UserRepository { get; }

        public UpdateRecord(RecordRepository recordRepository, UserRepository userRepository)
        {
            RecordRepository = recordRepository;
            UserRepository = userRepository;
        }

        public async Task Handler(ITelegramBotClient botClient, CallbackQuery callbackQuery, string[] args)
        {
            var user = await UserRepository.GetByColumn(nameof(Entities.User.TelegramId), $"{callbackQuery.From.Id}");

            var recordId = int.Parse(args[1]);
            var column = args[2];
            var value = args[3];

            Console.WriteLine($"Update Record -> Id:{recordId}, Column: {column}, Value: {value}");

            var record = await RecordRepository.UpdateColumnValueById(recordId, column, column == "MultiplayerType" ? int.Parse(value) : value);

            await botClient.AnswerCallbackQueryAsync(
              callbackQueryId: callbackQuery.Id,
              text: $"Заказ/Услуга была успешно обновлена");

            if (column.ToLower() == "MultiplayerType".ToLower())
            {
                await ChooseRecordHeader.SendMessage(botClient, callbackQuery.Message.Chat, record);

                await UserRepository.UpdateColumnValueById(user.Id, nameof(Entities.User.InputState), $"{InputState.WaitHeader.ToString()} {record.Id}");
            }
            else if (column.ToLower() == "Header".ToLower())
            {
                await ChooseRecordRequirements.SendMessage(botClient, callbackQuery.Message.Chat, record);

                await UserRepository.UpdateColumnValueById(user.Id, nameof(Entities.User.InputState), $"{InputState.WaitRequirements.ToString()} {record.Id}");
            }
            else if (column.ToLower() == "Requirements".ToLower())
            {
                await ChooseRecordDetails.SendMessage(botClient, callbackQuery.Message.Chat, record);

                await UserRepository.UpdateColumnValueById(user.Id, nameof(Entities.User.InputState), $"{InputState.WaitDetails.ToString()} {record.Id}");
            }
            else
            {
                await ChooseSection.SendMessage(botClient, callbackQuery.Message.Chat);

                await UserRepository.UpdateColumnValueById(user.Id, nameof(Entities.User.InputState), $"");
            }
        }
    }
}

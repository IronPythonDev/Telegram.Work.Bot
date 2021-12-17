using Bot.Entities;
using Bot.Entities.Enums;
using Bot.Infrastructure.Abstractions;
using Bot.Infrastructure.Attributes;
using Bot.Infrastructure.Repositories;
using Bot.Messages;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Bot.Comands
{
    [Command("", true)]
    internal class Usage : ICommandHandler
    {
        UserRepository UserRepository { get; set; }
        RecordRepository RecordRepository { get; set; }

        public Usage(UserRepository userRepository, RecordRepository recordRepository)
        {
            UserRepository = userRepository;
            RecordRepository = recordRepository;
        }

        public async Task Handler(ITelegramBotClient botClient, Message message)
        {
            var user = await UserRepository.GetByColumn(nameof(Entities.User.TelegramId), $"{message.From.Id}");

            var stateArgs = user.InputState.Split(' ');

            if (user.InputState.Contains($"{InputState.WaitHeader}"))
            {
                int recordId = int.Parse(stateArgs[1]);

                var record = await RecordRepository.GetById(recordId);

                await RecordRepository.UpdateColumnValueById(recordId, nameof(Record.Header), message.Text);

                await UserRepository.UpdateColumnValueById(user.Id, nameof(Entities.User.InputState), $"{InputState.WaitRequirements.ToString()} {record.Id}");

                await ChooseRecordRequirements.SendMessage(botClient, message.Chat, record);
            }
            else if (user.InputState.Contains($"{InputState.WaitRequirements}"))
            {
                int recordId = int.Parse(stateArgs[1]);

                var record = await RecordRepository.GetById(recordId);

                await RecordRepository.UpdateColumnValueById(recordId, nameof(Record.Requirements), message.Text);

                await UserRepository.UpdateColumnValueById(user.Id, nameof(Entities.User.InputState), $"{InputState.WaitDetails.ToString()} {record.Id}");

                await ChooseRecordDetails.SendMessage(botClient, message.Chat, record);
            }
            else if (user.InputState.Contains($"{InputState.WaitDetails}"))
            {
                int recordId = int.Parse(stateArgs[1]);

                await RecordRepository.UpdateColumnValueById(recordId, nameof(Record.Details), message.Text);

                await UserRepository.UpdateColumnValueById(user.Id, nameof(Entities.User.InputState), $"");

                await ChooseSection.SendMessage(botClient, message.Chat);
            }
        }
    }
}

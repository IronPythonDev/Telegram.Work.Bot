using Telegram.Bot;
using Telegram.Bot.Types;

namespace Bot.Infrastructure.Abstractions
{
    public interface ICommandHandler
    {
        Task Handler(ITelegramBotClient botClient, Message message);
    }
}

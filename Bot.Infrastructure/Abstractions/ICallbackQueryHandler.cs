using Telegram.Bot;
using Telegram.Bot.Types;

namespace Bot.Infrastructure.Abstractions
{
    public interface ICallbackQueryHandler

    {
        Task Handler(ITelegramBotClient botClient, CallbackQuery callbackQuery, string[] args);
    }
}

using Bot.Infrastructure.Abstractions;
using Bot.Infrastructure.Attributes;
using Bot.Infrastructure.Repositories;
using Bot.Messages;
using IronPython.Infrastructure.Abstractions.Exceptions;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Bot.CallbackQueryHandlers
{
    [CallbackQueryCommand("set_lang")]
    internal class Language : ICallbackQueryHandler
    {
        UserRepository UserRepository { get; }

        public Language(UserRepository userRepository)
        {
            UserRepository = userRepository;
        }

        public async Task Handler(ITelegramBotClient botClient, CallbackQuery callbackQuery, string[] args)
        {
            try
            {
                var lang = args[1] switch
                {
                    "ru" => "Русский",
                    _ => "English"
                };

                Entities.User? user = await UserRepository.GetByColumn(nameof(Entities.User.TelegramId), $"{callbackQuery.From.Id}");

                if (user == null) throw new NotFoundException("User not found");

                user = await UserRepository.UpdateColumnValueById(user.Id, nameof(Entities.User.LanguageCode), args[1]);

                await botClient.AnswerCallbackQueryAsync(
                  callbackQueryId: callbackQuery.Id,
                  text: $"Successfully changed to {lang} language");

                await ChooseSection.SendMessage(botClient, callbackQuery.Message.Chat);
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

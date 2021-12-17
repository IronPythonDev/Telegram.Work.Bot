using Bot.Infrastructure.Abstractions;
using Bot.Infrastructure.Attributes;
using Bot.Infrastructure.Repositories;
using Bot.Messages;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Bot.Comands
{
    [Command("/start")]
    public class Start : ICommandHandler
    {
        UserRepository UserRepository { get; }

        public Start(UserRepository userRepository)
        {
            UserRepository = userRepository;
        }

        public async Task Handler(ITelegramBotClient botClient, Message message)
        {
            Entities.User user = new()
            {
                TelegramId = $"{message.From.Id}",
                TelegramUserName = $"{message.From.Username}",
                LanguageCode = message.From.LanguageCode
            };

            if (!(await UserRepository.Exists(user, new List<string>() { nameof(Entities.User.TelegramId) })))
            {
                await UserRepository.Add(user);
            }

            await ChooseSection.SendMessage(botClient, message.Chat);
            //InlineKeyboardMarkup inlineKeyboard = new(
            //new[]
            //{
            //    new []
            //    {
            //        InlineKeyboardButton.WithCallbackData("🇷🇺Русский", "set_lang ru"),
            //    },
            //    new []
            //    {
            //        InlineKeyboardButton.WithCallbackData("🇬🇧English", "set_lang en"),
            //    }
            //});

            //await botClient.SendTextMessageAsync(chatId: message.Chat.Id,
            //                                            text: "Select language:",
            //                                            replyMarkup: inlineKeyboard);
        }
    }
}

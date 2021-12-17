using IronPython.Infrastructure.Entities;

namespace Bot.Entities
{
    public class User : Identity
    {
        public string? TelegramId { get; set; }
        public string? TelegramUserName { get; set; }
        public string? LanguageCode { get; set; }
        public string InputState { get; set; }
    }
}
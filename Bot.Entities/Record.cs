using IronPython.Infrastructure.Entities;

namespace Bot.Entities
{
    public class Record : Identity
    {
        public int Type { get; set; }
        public int MultiplayerType { get; set; }
        public string? Header { get; set; }
        public string? Requirements { get; set; }
        public string? Details { get; set; }
        public int OwnerId { get; set; }
    }
}

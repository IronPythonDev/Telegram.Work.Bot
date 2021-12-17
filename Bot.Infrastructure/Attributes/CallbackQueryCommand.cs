namespace Bot.Infrastructure.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CallbackQueryCommand : Attribute
    {
        public string Name { get; set; }

        public CallbackQueryCommand(string name)
        {
            Name = name;
        }
    }
}

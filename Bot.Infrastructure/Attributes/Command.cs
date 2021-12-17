namespace Bot.Infrastructure.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class Command : Attribute
    {
        public string Name { get; set; }
        public bool IsDefault { get; set; }

        public Command(string name, bool isDefault = false)
        {
            Name = name;
            IsDefault = isDefault;
        }
    }
}

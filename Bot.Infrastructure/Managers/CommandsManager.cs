using Bot.Infrastructure.Abstractions;
using Bot.Infrastructure.Attributes;
using System.Reflection;

namespace Bot.Infrastructure.Managers
{
    public static class CommandsManager
    {
        private static IDictionary<string, ICommandHandler> Handlers { get; } = new Dictionary<string, ICommandHandler>();
        private static IList<ICommandHandler> DefaultHandlers { get; } = new List<ICommandHandler>();

        public static Task LoadHandlers(Assembly assembly)
        {
            var types = assembly.GetTypes().Where(v => v.IsAssignableTo(typeof(ICommandHandler)));

            foreach (var type in types)
            {
                var instance = ActivatorExtensions.CreateInstanceByDependencyInjection(type) as ICommandHandler;

                if (instance == null) continue;

                var commandAttribute = type.GetCustomAttribute<Command>();

                if (commandAttribute == null) continue;

                if (!commandAttribute.IsDefault) Handlers.Add(commandAttribute.Name, instance);
                if (commandAttribute.IsDefault) DefaultHandlers.Add(instance);

                Console.WriteLine($"Loaded [Default: {commandAttribute.IsDefault}] {commandAttribute.Name} command");
            }

            return Task.CompletedTask;
        }

        public static Task<IEnumerable<ICommandHandler>> GetHandlersByName(string command)
        {
            var handlers = Handlers.Where(v => v.Key == command).Select(v => v.Value);

            if (!handlers.Any()) handlers = DefaultHandlers;

            return Task.FromResult(handlers);
        }
    }
}

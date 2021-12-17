using Bot.Infrastructure.Abstractions;
using Bot.Infrastructure.Attributes;
using System.Reflection;

namespace Bot.Infrastructure.Managers
{
    public static class CallbackQueryManager
    {
        public static IDictionary<string, ICallbackQueryHandler> Handlers = new Dictionary<string, ICallbackQueryHandler>();

        public static Task<IEnumerable<ICallbackQueryHandler>> GetHandlersByName(string name) =>
            Task.FromResult(Handlers.Where(v => v.Key == name).Select(v => v.Value));

        public static Task LoadHandlers(Assembly assembly)
        {
            var types = assembly.GetTypes().Where(v => v.IsAssignableTo(typeof(ICallbackQueryHandler)));

            foreach (var type in types)
            {
                var instance = ActivatorExtensions.CreateInstanceByDependencyInjection(type) as ICallbackQueryHandler;

                if (instance == null) continue;

                var commandAttribute = type.GetCustomAttribute<CallbackQueryCommand>();

                if (commandAttribute == null) continue;

                Handlers.Add(commandAttribute.Name, instance);

                Console.WriteLine($"Loaded {commandAttribute.Name} callback query event");
            }

            return Task.CompletedTask;
        }
    }
}

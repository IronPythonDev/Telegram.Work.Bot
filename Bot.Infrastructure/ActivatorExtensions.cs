using Bot.Infrastructure.Exceptions;
using Microsoft.Extensions.DependencyInjection;

namespace Bot.Infrastructure
{
    public static class ActivatorExtensions
    {
        public static ServiceProvider? ServiceProvider { get; set; }

        public static object? CreateInstanceByDependencyInjection(Type type)
        {
            var constructor = type.GetConstructors().FirstOrDefault();

            if (constructor == null) throw new NullReferenceException("Constructor isn't found");

            var parameters = constructor.GetParameters();

            var constructorArgs = new List<object>();

            foreach (var parameter in parameters)
            {
                var instance = ServiceProvider?.GetRequiredService(parameter.ParameterType);

                if (instance == null)
                    throw new NotFoundException($"{parameter.ParameterType.Name} service isn't registered ");

                constructorArgs.Add(instance);
            }

            if (parameters.Length > 0) return Activator.CreateInstance(type, constructorArgs.ToArray());
            else return Activator.CreateInstance(type);
        }
    }
}

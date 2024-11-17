using Microsoft.Extensions.DependencyInjection;

namespace Temperature.Monitoring.Configurations
{
    public static class DependenciesConfiguration
    {
        public static IServiceCollection ApplyDependenciesConfiguration(this IServiceCollection services, Action<IServiceCollection> action)
        {

            action(services);

            return services;
        }
    }
}
 
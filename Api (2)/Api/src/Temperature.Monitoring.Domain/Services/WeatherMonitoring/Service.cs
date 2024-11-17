using Microsoft.Extensions.DependencyInjection;
using Temperature.Monitoring.WeatherMonitoring.Data.Interfaces;
using Temperature.Monitoring.WeatherMonitoring.Domain.Interfaces;
using Temperature.Monitoring.WeatherMonitoring.Models.Common;

namespace Temperature.Monitoring.WeatherMonitoring.Domain.Services
{
    internal class Service : IService
    {
        private readonly IServiceProvider _serviceProvider;
        public Service(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<WeatherStatus> GetWeatherStatus(Key key, CancellationToken cancellationToken = default)
        {
            var repository = _serviceProvider.GetRequiredKeyedService<IRepository>(key.GetIdentifier());

            return await repository.GetWeatherStatus(cancellationToken);
        }
    }
}

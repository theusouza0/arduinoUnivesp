using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.ComponentModel.Design;

namespace Temperature.Monitoring.Domain.Configuration
{
    public static class LibraryConfiguration
    {
        public static IServiceCollection ApplyConfigurationsDomain(this IServiceCollection services)
        {
            #region WeatherMonitoring 

            services.AddSingleton<WeatherMonitoring.Domain.Interfaces.IService, WeatherMonitoring.Domain.Services.Service>();

            services.AddKeyedSingleton<WeatherMonitoring.Data.Interfaces.IRepository, WeatherMonitoring.Data.Repositories.Variations.Local.Repository>(
                WeatherMonitoring.Models.Common.Keys.Key.CreateKey(WeatherMonitoring.Models.Common.Keys.Key.SourceType.Local).GetIdentifier());

            services.AddKeyedSingleton<WeatherMonitoring.Data.Interfaces.IRepository, WeatherMonitoring.Data.Repositories.Variations.Arduino.Repository>(
                WeatherMonitoring.Models.Common.Keys.Key.CreateKey(WeatherMonitoring.Models.Common.Keys.Key.SourceType.Arduino).GetIdentifier());

            #endregion

            return services;
        }
    }
}

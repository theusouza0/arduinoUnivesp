using Temperature.Monitoring.WeatherMonitoring.Models.Common;

namespace Temperature.Monitoring.WeatherMonitoring.Domain.Interfaces
{
    public interface IService
    {
        Task<WeatherStatus> GetWeatherStatus(Key key, CancellationToken cancellationToken = default);
    }
}

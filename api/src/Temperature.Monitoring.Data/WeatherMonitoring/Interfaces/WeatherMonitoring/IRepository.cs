using Temperature.Monitoring.WeatherMonitoring.Models.Common;

namespace Temperature.Monitoring.WeatherMonitoring.Data.Interfaces
{
    public interface IRepository
    {
        Task<WeatherStatus> GetWeatherStatus(CancellationToken cancellationToken = default);
    }
}

using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Temperature.Monitoring.WeatherMonitoring.Domain.Interfaces;
using Temperature.Monitoring.WeatherMonitoring.Domain.Resources.Status;
using Temperature.Monitoring.WeatherMonitoring.Models.Common;

namespace Temperature.Monitoring.WeatherMonitoring.Api.Services
{
    public class Service : BackgroundService
    {
        private readonly ILogger<Service>            _logger;
        private readonly IStringLocalizer<Resources> _statusResources;
        public readonly  IServiceProvider            _serviceProvider;

        private readonly Key Key = Keys.Key.CreateKey(Keys.Key.SourceType.Local);

        public Service(
            IServiceProvider            serviceProvider,
            ILogger<Service>            logger,
            IStringLocalizer<Resources> statusResources)
        {
            _serviceProvider = serviceProvider;
            _logger          = logger;
            _statusResources = statusResources;
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation(
                "Weather Monitoring Service running.");

            await DoWork(stoppingToken);
        }

        private async Task DoWork(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Weather Monitoring Service is working.");

                using (var scope = _serviceProvider.CreateScope())
                {
                    var service    = scope.ServiceProvider.GetRequiredService<IService>();
                    var hubContext = scope.ServiceProvider.GetRequiredService<IHubContext<WeatherMonitoring.Api.Hubs.Hub>>();

                    var weatherStatus = await service.GetWeatherStatus(Key, stoppingToken);

                    switch (weatherStatus)
                    {
                        case WeatherMonitoring.Models.Variations.Arduino.WeatherStatus variation:

                            var variationArduino = new
                            {
                                Status = new
                                {
                                    Date = variation.Status.Date.ToString("G"),

                                    Status = _statusResources[Enum.GetName(variation.Status.Status) ?? string.Empty] ?? string.Empty,

                                    Temperature = new
                                    {
                                        Celsius    = variation.Status.Temperature.Celsius.ToString("F2"),
                                        Fahrenheit = variation.Status.Temperature.Fahrenheit.ToString("F2")
                                    },

                                    Humidity = new
                                    {
                                        Percentage = variation.Status.Humidity.Percentage.ToString("F2")
                                    },

                                    Wind = new
                                    {
                                        Speed = variation.Status.Wind.Speed.ToString("F2"),
                                        Unit  = variation.Status.Wind.Unit
                                    },

                                    UvIndex = new
                                    {
                                        Index = variation.Status.UvIndex.Index.ToString("F2")
                                    },

                                    Visibility = new 
                                    {
                                        Speed = variation.Status.Visibility.Distance.ToString("F2"),
                                        Unit  = variation.Status.Visibility.Unit
                                    },

                                    Precipitation = new
                                    {
                                        Speed = variation.Status.Precipitation.Amount.ToString("F2"),
                                        Unit  = variation.Status.Precipitation.Unit
                                    },

                                    AirPressure = new
                                    {
                                        Speed = variation.Status.AirPressure.Pressure.ToString("F2"),
                                        Unit  = variation.Status.AirPressure.Unit
                                    }
                                }
                            };

                            _logger.LogInformation("{@weatherStatus}", variationArduino);

                            await hubContext.Clients.All.SendAsync("weather-monitoring", variationArduino, stoppingToken);

                            break;

                        case WeatherMonitoring.Models.Variations.Local.WeatherStatus variation:

                            var variationLocal = new
                            {
                                Status = new
                                {
                                    Date = variation.Status.Date.ToString("G"),

                                    Status = _statusResources[Enum.GetName(variation.Status.Status) ?? string.Empty] ?? string.Empty,

                                    Temperature = new
                                    {
                                        Celsius    = variation.Status.Temperature.Celsius.ToString("F2"),
                                        Fahrenheit = variation.Status.Temperature.Fahrenheit.ToString("F2")
                                    },

                                    Humidity = new
                                    {
                                        Percentage = variation.Status.Humidity.Percentage.ToString("F2")
                                    },

                                    Wind = new
                                    {
                                        Speed = variation.Status.Wind.Speed.ToString("F2"),
                                        Unit  = variation.Status.Wind.Unit
                                    },

                                    UvIndex = new
                                    {
                                        Index = variation.Status.UvIndex.Index.ToString("F2")
                                    },

                                    Visibility = new 
                                    {
                                        Speed = variation.Status.Visibility.Distance.ToString("F2"),
                                        Unit  = variation.Status.Visibility.Unit
                                    },

                                    Precipitation = new
                                    {
                                        Speed = variation.Status.Precipitation.Amount.ToString("F2"),
                                        Unit  = variation.Status.Precipitation.Unit
                                    },

                                    AirPressure = new
                                    {
                                        Speed = variation.Status.AirPressure.Pressure.ToString("F2"),
                                        Unit  = variation.Status.AirPressure.Unit
                                    }
                                }
                            };

                            _logger.LogInformation("{@weatherStatus}", variationLocal);

                            await hubContext.Clients.All.SendAsync("weather-monitoring", variationLocal, stoppingToken);

                            break;

                        default:

                            _logger.LogInformation("{@weatherStatus}", weatherStatus);

                            await hubContext.Clients.All.SendAsync("weather-monitoring", weatherStatus, stoppingToken);

                            break;

                    }                  
                    await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
                }
            }
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation(
                "Weather Monitoring Service is stopping.");

            await base.StopAsync(stoppingToken);
        }
    }
}

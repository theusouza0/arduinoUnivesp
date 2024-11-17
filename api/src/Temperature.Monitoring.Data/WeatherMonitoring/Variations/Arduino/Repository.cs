using Temperature.Monitoring.WeatherMonitoring.Data.Interfaces;
using Temperature.Monitoring.WeatherMonitoring.Models.Common;

using Variation = Temperature.Monitoring.WeatherMonitoring.Models.Variations.Arduino;

namespace Temperature.Monitoring.WeatherMonitoring.Data.Repositories.Variations.Arduino
{
    public class Repository : IRepository
    {
        private readonly Random _random = new();

        private double _currentTemperature   = 0;
        private double _currentHumidity      = 0;
        private double _currentWindSpeed     = 0;
        private double _currentUVIndex       = 0;
        private double _currentAirPressure   = 0;
        private double _currentVisibility    = 0;
        private double _currentPrecipitation = 0;

        private StatusWeather _currentStatus = StatusWeather.Nothing;

        private bool _initialized = false;

        private enum StatusWeather
        {
            Nothing   = 1,
            HotAndDry = 2,
            Snow      = 3,
            Cloudy    = 4,
            Foggy     = 5,
            Windy     = 6,
            Rain      = 7,
            Clear     = 8
        }

        public Task<WeatherStatus> GetWeatherStatus(CancellationToken cancellationToken = default)
        {
            if (!_initialized)
            {
                InitializeWeather();
                _initialized = true;
            }

            cancellationToken.ThrowIfCancellationRequested();

            UpdateWeather();
            return Task.FromResult(GenerateWeatherStatus());
        }

        private void InitializeWeather()
        {
            _currentTemperature   = _random.NextDouble() * 50 - 10;
            _currentHumidity      = _random.NextDouble() * 100;
            _currentWindSpeed     = _random.NextDouble() * 20;
            _currentUVIndex       = _random.NextDouble() * 12;
            _currentAirPressure   = _random.NextDouble() * 40 + 950; // [Range: 950 to 990 hPa]
            _currentVisibility    = _random.NextDouble() * 10;       // [Range: 0 to 10 km]
            _currentPrecipitation = 0;

            _currentStatus = GenerateRandomStatus();
        }

        private void UpdateWeather()
        {
            var timeFactor = DateTime.Now.Hour < 12 ? 1 : -1;

            _currentTemperature += (_random.NextDouble() - 0.5) * 1.5 * timeFactor;
            _currentHumidity    += _currentStatus == StatusWeather.Rain
                ? (_random.NextDouble() - 0.3) * 5.0
                : (_random.NextDouble() - 0.5) * 3.0;

            _currentWindSpeed       += (_random.NextDouble() - 0.5) * 2.0;
            _currentUVIndex         += (_random.NextDouble() - 0.5) * 0.5;
            _currentAirPressure     += (_random.NextDouble() - 0.5) * 1.0;
            _currentVisibility      += (_random.NextDouble() - 0.5) * 0.2;
            _currentPrecipitation   = _currentStatus == StatusWeather.Rain
                ? _random.NextDouble() * 10 // [Rainfall in mm]
                : 0;

            _currentTemperature = Math.Clamp(_currentTemperature, -30.0, 50.0);
            _currentHumidity    = Math.Clamp(_currentHumidity, 0.0, 100.0);
            _currentWindSpeed   = Math.Clamp(_currentWindSpeed, 0.0, 50.0);
            _currentUVIndex     = Math.Clamp(_currentUVIndex, 0.0, 12.0);
            _currentAirPressure = Math.Clamp(_currentAirPressure, 950.0, 1050.0);
            _currentVisibility  = Math.Clamp(_currentVisibility, 0.0, 10.0);

            _currentStatus = DetermineStatus();
        }

        private WeatherStatus GenerateWeatherStatus()
        {
            return new Variation.WeatherStatus
            {
                Status = new()
                {
                    Date = DateTime.Now,

                    Status = (Variation.WeatherStatus.StatusWeather)Enum.Parse(typeof(Variation.WeatherStatus.StatusWeather), _currentStatus.ToString()),

                    Humidity = new()
                    {
                        Percentage = _currentHumidity
                    },

                    Temperature = new()
                    {
                        Celsius = _currentTemperature,
                        Fahrenheit = CelsiusToFahrenheit(_currentTemperature)
                    },

                    Wind = new()
                    {
                        Speed = _currentWindSpeed,
                        Unit  = Variation.WeatherStatus.StatusProperties.WindProperties.UnitType.KilometerPerHour
                    },

                    UvIndex = new()
                    {
                        Index = _currentUVIndex
                    },

                    AirPressure = new()
                    {
                        Pressure = _currentAirPressure,
                        Unit     = Variation.WeatherStatus.StatusProperties.AirPressureProperties.UnitType.hPa
                    },

                    Visibility = new()
                    {
                        Distance = _currentVisibility,
                        Unit     = Variation.WeatherStatus.StatusProperties.VisibilityProperties.UnitType.Kilometer
                    },

                    Precipitation = new()
                    {
                        Amount = _currentPrecipitation,
                        Unit   = Variation.WeatherStatus.StatusProperties.PrecipitationProperties.UnitType.mm
                    }
                }
            };
        }

        private StatusWeather DetermineStatus()
        {
            if (_currentHumidity > 80 && _random.NextDouble() < 0.5)
                return StatusWeather.Rain;

            if (_currentTemperature < 0 && _currentHumidity > 70)
                return StatusWeather.Snow;

            if (_currentTemperature > 35 && _currentHumidity < 40)
                return StatusWeather.HotAndDry;

            return StatusWeather.Cloudy;
        }

        private StatusWeather GenerateRandomStatus()
        {
            var conditions = new StatusWeather[5] { StatusWeather.Clear, StatusWeather.Rain, StatusWeather.Cloudy, StatusWeather.Foggy, StatusWeather.Windy };
            return conditions[_random.Next(conditions.Length)];
        }

        private static double CelsiusToFahrenheit(double celsius)
        {
            return celsius * 9 / 5 + 32;
        }
    }
}

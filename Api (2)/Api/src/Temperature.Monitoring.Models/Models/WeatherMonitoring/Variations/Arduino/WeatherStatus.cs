namespace Temperature.Monitoring.WeatherMonitoring.Models.Variations.Arduino
{
    public record WeatherStatus : Common.WeatherStatus
    {
        public enum StatusWeather
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


        public required StatusProperties Status { get; init; }
        public class StatusProperties
        {
            public required DateTime Date { get; init; }
            public required StatusWeather Status { get; init; }
            public required TemperatureProperties Temperature { get; init; }
            public required HumidityProperties Humidity { get; init; }
            public required WindProperties Wind { get; init; }
            public required UvIndexProperties UvIndex { get; init; }
            public required VisibilityProperties Visibility { get; init; }
            public required PrecipitationProperties Precipitation { get; init; }
            public required AirPressureProperties AirPressure { get; init; }

            public class TemperatureProperties
            {
                public required double Celsius { get; init; }
                public required double Fahrenheit { get; init; }

            }

            public class HumidityProperties
            {
                public required double Percentage { get; init; }

            }

            public class UvIndexProperties
            {
                public required double Index { get; init; }

            }

            public class AirPressureProperties
            {
                public enum UnitType
                {
                    hPa
                }

                public required double Pressure { get; init; }
                public required UnitType Unit { get; init; }
            }

            public class WindProperties
            {
                public enum UnitType
                {
                    KilometerPerHour
                }

                public required double Speed { get; init; }
                public required UnitType Unit { get; init; }

            }

            public class VisibilityProperties
            {
                public enum UnitType
                {
                    Kilometer
                }

                public required double Distance { get; init; }
                public required UnitType Unit { get; init; }
            }

            public class PrecipitationProperties
            {
                public enum UnitType
                {
                    mm
                }

                public required double Amount { get; init; }
                public required UnitType Unit { get; init; }

            }
        }
    }
}

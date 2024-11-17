namespace Temperature.Monitoring.WeatherMonitoring.Models.Common
{
    public class Key
    {
        public virtual string GetIdentifier()
        {
            return string.Empty;
        }
    }

    public class Keys
    {
        public class Key : Common.Key
        {
            public enum SourceType
            {
                Arduino,
                Local
            }

            public SourceType Source { get; private set; }

            public static Key CreateKey(SourceType sourceType)
            {
                return new Key()
                {
                    Source = sourceType
                };
            }

            public override string GetIdentifier()
            {
                return $"{typeof(Key).FullName}-{Enum.GetName(Source)}";
            }
        }
    }
}

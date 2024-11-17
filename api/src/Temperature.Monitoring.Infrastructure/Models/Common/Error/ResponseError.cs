namespace Temperature.Monitoring.Infrastructure.Models.Common
{
    public class ResponseError
    {
        public required string Status { get; init; }
        public required ErrorProperties Error { get; init; }
        public class ErrorProperties
        {
            public required string Message { get; init; }
            public required int StatusCode { get; init; }
        }
    }
}

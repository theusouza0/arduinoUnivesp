using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Temperature.Monitoring.Infrastructure.Models.Common;

namespace Temperature.Monitoring.Configuration
{
    public static class ExceptionConfiguration
    {
        public static IApplicationBuilder ApplyExceptionHandler(this IApplicationBuilder app)
        {

            app.UseExceptionHandler(new ExceptionHandlerOptions()
            {
                ExceptionHandler = async (context) =>
                {
                    // [Logger]
                    var logger = context.RequestServices.GetRequiredService<ILogger>();

                    // [Exception]
                    var exception = context.Features.GetRequiredFeature<IExceptionHandlerFeature>().Error;

                    var error = new ResponseError
                    {
                        Status = "ERROR",
                        Error  = new()
                        {
                            Message    = exception.Message,
                            StatusCode = 500
                        }
                    };

                    logger.LogError("{@error}", error);

                    await context.Response.WriteAsJsonAsync<ResponseError>(error);

                    return;
                }
            });

            return app;

        }
    }
}

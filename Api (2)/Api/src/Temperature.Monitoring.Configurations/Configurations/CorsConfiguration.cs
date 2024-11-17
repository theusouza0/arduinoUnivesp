using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Temperature.Monitoring.Configurations
{
    public static class CorsConfigurations
    {
        public static IServiceCollection ApplyCorsConfiguration(this IServiceCollection services)
        {

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins", policy =>
                {
                    policy.AllowAnyHeader()                
                          .AllowAnyMethod()                
                          .AllowCredentials()              
                          .SetIsOriginAllowed(origin => true);
                });
            });

            return services;
        }

        public static IApplicationBuilder UseCorsConfiguration(this IApplicationBuilder app)
        {
            app.UseCors("AllowAllOrigins");

            return app;
        }
    }
}

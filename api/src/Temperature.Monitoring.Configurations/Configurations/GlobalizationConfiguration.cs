using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Globalization;

namespace Temperature.Monitoring.Configurations
{
    public static class GLobalizationConfiguration
    {
        public static IServiceCollection ApplyGlobalizationConfiguration(this IServiceCollection services)
        {
            services.AddLocalization();

            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                {
                    new CultureInfo("en-US"),
                    new CultureInfo("pt-BR"),
                    new CultureInfo("zh-CN")
                };

                options.DefaultRequestCulture = new RequestCulture("pt-BR");
                options.SupportedCultures     = supportedCultures;
                options.SupportedUICultures   = supportedCultures;

                options.RequestCultureProviders.Insert(0, new CustomRequestCultureProvider(context =>
                {
                    var currentLanguage = context.Request.Headers["Accept-Language"].ToString().Split(',').FirstOrDefault();
                    var defaultLanguage = string.IsNullOrWhiteSpace(currentLanguage) ? "pt-BR" : currentLanguage;

                    if (!supportedCultures.Any(s => s.Name.Equals(defaultLanguage)))
                        defaultLanguage = "pt-BR";
                    return Task.FromResult<ProviderCultureResult?>(new ProviderCultureResult(defaultLanguage, defaultLanguage));
                }));
            });

            return services;


        }

        public static IApplicationBuilder UseGlobalizationConfiguration(this IApplicationBuilder app)
        {
            var options = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();

            if (options is not null)
                app.UseRequestLocalization(options.Value);

            return app;
        }

    }
}

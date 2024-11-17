using Temperature.Monitoring.Configuration;
using Temperature.Monitoring.Configurations;
using Temperature.Monitoring.Domain.Configuration;
using WeatherMonitoring = Temperature.Monitoring.WeatherMonitoring;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.



builder.Services.ApplyDependenciesConfiguration((services) =>
{
    services.AddHostedService<WeatherMonitoring.Api.Services.Service>();
});


// [Domain]
builder.Services.ApplyConfigurationsDomain();

builder.Services.AddSignalR();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.CustomSchemaIds(x => x.FullName?.Replace('+', '.'));
});

// [Cors]
builder.Services.ApplyCorsConfiguration();

// [Globalization]
builder.Services.ApplyGlobalizationConfiguration();

var app = builder.Build();

// [Cors]
app.UseCorsConfiguration();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI();
}


// [Exception Handler]
app.ApplyExceptionHandler();

// [Globalization]
app.UseGlobalizationConfiguration();

app.UseRouting();

app.MapHub<WeatherMonitoring.Api.Hubs.Hub>("/weather-monitoring");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();


app.Run();

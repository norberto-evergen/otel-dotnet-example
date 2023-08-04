using Config;
using OtelConfiguration;
using System.Diagnostics;
using OpenTelemetry;
using OpenTelemetry.Trace;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddOpenTelemetry()
    .WithTracing(tracerProviderBuilder =>
        tracerProviderBuilder
            .AddSource(OtelConfig.ActivitySource.Name)
            .ConfigureResource(resource => resource
                .AddService(OtelConfig.ServiceName))
            .AddAspNetCoreInstrumentation()
            .AddConsoleExporter())
    .WithMetrics(metricsProviderBuilder =>
        metricsProviderBuilder
            .AddMeter(OtelConfig.WeatherMeter.Name)
            .AddMeter(OtelConfig.ServiceTwoMeter.Name)
            .ConfigureResource(resource => resource
                .AddService(OtelConfig.ServiceName))
            //.AddAspNetCoreInstrumentation()
            .AddConsoleExporter());

builder.Services.AddSingleton(TracerProvider.Default.GetTracer(DiagnosticsConfig.ServiceName));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

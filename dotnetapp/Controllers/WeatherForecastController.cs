using Microsoft.AspNetCore.Mvc;
using OpenTelemetry.Trace;
using Config;
using ServiceTwo;
using OtelConfiguration;

namespace dotnetapp.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;
    private readonly Tracer _tracer;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, Tracer tracer)
    {
        _logger = logger;
        _tracer = tracer;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        //using var span = myTracer?.StartActiveSpan("WeatherForcast");
        using var span = _tracer?.StartActiveSpan("WeatherForcast"); 
        span?.SetAttribute("Method", "Get");

        // using var span = _tracer.StartActiveSpan("GetWeatherForecast");
        // span.SetAttribute("operation.value", 1);
        // span.SetAttribute("operation.name", "Saying hello!");
        // span.SetAttribute("operation.other-stuff", new int[] { 1, 2, 3 });

        span.AddEvent(
            "Some event or error",
            DateTimeOffset.Now
        );

        OtelConfig.WeatherRequestCounter.Add(1,
            new("Action", "Get"),
            new("Controller", "WeatherForecastController"));

        ServiceTwo.HelloMessage.DoMessage();

        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }
}

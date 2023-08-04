using System.Diagnostics;
using System.Diagnostics.Metrics;
using OpenTelemetry;
using OpenTelemetry.Trace;
using OpenTelemetry.Resources;

namespace OtelConfiguration
{
    public static class OtelConfig
    {
        public static string ServiceName = "dotnetapp";
        public static ActivitySource ActivitySource = new ActivitySource(ServiceName);

        public static Meter WeatherMeter = new(ServiceName);
        public static Counter<long> WeatherRequestCounter =
            WeatherMeter.CreateCounter<long>("app.request_counter");
        
        public static Meter ServiceTwoMeter = new(ServiceName);
        public static Counter<long> ServiceTwoCounter =
            ServiceTwoMeter.CreateCounter<long>("app.service_two_counter");
        
        public static TracerProvider? MyTraceProvider = Sdk.CreateTracerProviderBuilder()
        .AddSource(ServiceName)
            .SetResourceBuilder(
                ResourceBuilder.CreateDefault()
                    .AddService(serviceName: ServiceName)
                )
            .AddConsoleExporter()
            .Build();
    }
}

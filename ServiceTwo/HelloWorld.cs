using System;
using OtelConfiguration;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using OpenTelemetry.Trace;

namespace ServiceTwo;
public static class HelloMessage
{
    private const string message = "Hello from ServiceTwo";
    private static readonly TracerProvider? tracerProvider = OtelConfig.MyTraceProvider;
    private static readonly Tracer? myTracer = tracerProvider.GetTracer(OtelConfig.ServiceName);

    public static void DoMessage()
    {
        using var span = myTracer?.StartActiveSpan("HelloMessageSpan");
        span?.SetAttribute("Method", "DoMessage");

        OtelConfig.ServiceTwoCounter.Add(5,
            new("Service", "ServiceTwo"),
            new("Method", "DoMessage"));

        Console.WriteLine(message);

        span?.End();
    }
}

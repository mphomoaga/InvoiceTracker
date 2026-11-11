using InvoiceTracker.Application;
using InvoiceTracker.Infrastructure;
using InvoiceTracker.Worker;
using Serilog;

var builder = Host.CreateApplicationBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("InvoiceTrackerDatabase");
if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException("ConnectionStrings:InvoiceTrackerDatabase is not configured.");
}
builder.Services.AddSerilog();
builder.Services.AddInfrastructureDependency(connectionString);
builder.Services.AddApplicationDependency();
builder.Services.AddHostedService<InvoiceReminderWorker>();

try
{
    Log.Information("InvoiceTracker Worker is starting");
    await builder.Build().RunAsync();
}
catch (Exception exception)
{
    Log.Fatal(
        exception,
        "InvoiceTracker Worker stopped unexpectedly");
}
finally
{
    await Log.CloseAndFlushAsync();
}


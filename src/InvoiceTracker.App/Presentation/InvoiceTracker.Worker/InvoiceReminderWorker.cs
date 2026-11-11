using InvoiceTracker.Application.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace InvoiceTracker.Worker;

public sealed class InvoiceReminderWorker(
    IServiceScopeFactory scopeFactory,
    ILogger<InvoiceReminderWorker> logger) : BackgroundService
{
    private static readonly TimeSpan RunInterval = TimeSpan.FromHours(24);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Invoice reminder worker started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = scopeFactory.CreateScope();
                var reminderService = scope.ServiceProvider.GetRequiredService<IInvoiceReminderService>();
                await reminderService.ProcessAsync(stoppingToken);
            }
            catch (OperationCanceledException)             {
                logger.LogError("Invoice reminder worker run was cancelled.");
                break;
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "Invoice reminder worker run failed.");
            }

            logger.LogInformation("Invoice reminder worker completed.");

            await Task.Delay(RunInterval, stoppingToken);
        }
    }
}

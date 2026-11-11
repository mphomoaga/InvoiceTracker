namespace InvoiceTracker.Application.Interfaces.Services;

public interface IInvoiceReminderService
{
    Task ProcessAsync(CancellationToken cancellationToken);
}

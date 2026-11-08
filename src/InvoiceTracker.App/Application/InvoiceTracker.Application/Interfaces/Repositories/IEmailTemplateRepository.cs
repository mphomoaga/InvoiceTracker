namespace InvoiceTracker.Application.Interfaces.Repositories
{
    public interface IEmailTemplateRepository
    {
        Task<string> GetEmailTemplate(string templateName, CancellationToken token);
    }
}

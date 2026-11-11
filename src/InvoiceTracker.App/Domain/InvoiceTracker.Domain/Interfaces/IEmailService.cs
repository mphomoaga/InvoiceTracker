using InvoiceTracker.Domain.Models;

namespace InvoiceTracker.Domain.Interfaces
{
    public interface IEmailService
    {
        Task<bool> SendEmail(string to,
            string subject,
            string itemType,
            int itemTypeRef,
            string body,
            IEnumerable<EmailAttachment> attachments,
            string createdBy,
            CancellationToken cancellationToken = default,
            string? cc = null);

        Task<bool> SendReminderEmailAsync(
            string to,
            string subject,
            string htmlBody,
            IEnumerable<EmailAttachment> attachments,
            CancellationToken cancellationToken = default);
    }
}

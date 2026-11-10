using InvoiceTracker.Domain.Entities;
using InvoiceTracker.Domain.Interfaces;
using InvoiceTracker.Domain.Models;
using InvoiceTracker.Infrastructure.Persistence;
using Microsoft.Extensions.Logging;

namespace InvoiceTracker.Infrastructure.Email
{
    public class EmailService : IEmailService
    {
        private readonly InvoiceTrackerDbContext _dbContext;
        private readonly ILogger<EmailService> _logger;
        private const string from = "no-reply@invoiceTracker.com";
        public EmailService(
            InvoiceTrackerDbContext dbContext,
            ILogger<EmailService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }


        public async Task<bool> SendEmail(string to, string subject, string itemType,
    int itemTypeRef, string body, IEnumerable<EmailAttachment> attachments, string createdBy, CancellationToken cancellationToken = default, string? cc = null)
        {
            try
            {
                // Save Audit
                var audit = new EmailAudit
                {
                    ItemType = itemType,
                    ItemTypeRef = itemTypeRef,
                    From = from,
                    To = to,
                    CC = cc,
                    Subject = subject,
                    Body = body,
                    HasAttachments = attachments?.Any() ?? false,
                    IsSent = true,
                    SentDate = DateTime.UtcNow,
                    IsActive = true,
                    CreatedBy = createdBy,
                    CreatedDate = DateTime.UtcNow
                };

                _dbContext.EmailAudits.Add(audit);

                await _dbContext.SaveChangesAsync(
                    cancellationToken);
                return true;
            }
            catch (Exception ex)
            {
                var audit = new EmailAudit
                {
                    ItemType = itemType,
                    ItemTypeRef = itemTypeRef,
                    From = from,
                    To = to,
                    CC = cc,
                    Subject = subject,
                    Body = body,
                    HasAttachments = attachments?.Any() ?? false,
                    IsSent = false,
                    SentDate = DateTime.UtcNow,
                    IsActive = true,
                    CreatedBy = createdBy,
                    CreatedDate = DateTime.UtcNow
                };

                _dbContext.EmailAudits.Add(audit);

                await _dbContext.SaveChangesAsync(
                    cancellationToken);

                _logger.LogError(ex, "Email Error");
                return false;
            }
        }
    }
}

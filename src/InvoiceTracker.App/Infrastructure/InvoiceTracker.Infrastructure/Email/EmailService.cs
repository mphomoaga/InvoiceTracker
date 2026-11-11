using InvoiceTracker.Domain.Entities;
using InvoiceTracker.Domain.Interfaces;
using InvoiceTracker.Domain.Models;
using InvoiceTracker.Infrastructure.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Mail;

namespace InvoiceTracker.Infrastructure.Email
{
    public class EmailService : IEmailService
    {
        private readonly InvoiceTrackerDbContext _dbContext;
        private readonly ILogger<EmailService> _logger;
        private readonly IConfiguration _configuration;
        private const string from = "no-reply@invoiceTracker.com";
        public EmailService(
            InvoiceTrackerDbContext dbContext,
            ILogger<EmailService> logger,
            IConfiguration configuration)
        {
            _dbContext = dbContext;
            _logger = logger;
            _configuration = configuration;
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

        public async Task<bool> SendReminderEmailAsync(
            string to,
            string subject,
            string htmlBody,
            IEnumerable<EmailAttachment> attachments,
            CancellationToken cancellationToken = default)
        {
            var host = _configuration["Smtp:Host"];
            var fromAddress = _configuration["Smtp:FromAddress"];
            if (string.IsNullOrWhiteSpace(host) || string.IsNullOrWhiteSpace(fromAddress))
            {
                _logger.LogError("Reminder email was not sent because Smtp:Host or Smtp:FromAddress is not configured.");
                return false;
            }

            try
            {
                var port = int.TryParse(_configuration["Smtp:Port"], out var configuredPort)
                    ? configuredPort
                    : 25;
                using var message = new MailMessage
                {
                    From = new MailAddress(fromAddress),
                    Subject = subject,
                    Body = htmlBody,
                    IsBodyHtml = true
                };
                message.To.Add(to);

                foreach (var attachment in attachments)
                {
                    message.Attachments.Add(new Attachment(
                        new MemoryStream(attachment.Content, writable: false),
                        attachment.FileName,
                        attachment.ContentType));
                }

                using var client = new SmtpClient(host, port)
                {
                    EnableSsl = bool.TryParse(_configuration["Smtp:EnableSsl"], out var enableSsl) && enableSsl,
                    UseDefaultCredentials = false
                };

                var username = _configuration["Smtp:Username"];
                if (!string.IsNullOrWhiteSpace(username))
                {
                    client.Credentials = new NetworkCredential(username, _configuration["Smtp:Password"]);
                }

                await client.SendMailAsync(message, cancellationToken);
                return true;
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "SMTP failed while sending reminder email to {CustomerEmail}.", to);
                return false;
            }
        }
    }
}

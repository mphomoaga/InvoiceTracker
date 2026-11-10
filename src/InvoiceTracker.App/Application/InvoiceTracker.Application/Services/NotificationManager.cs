using InvoiceTracker.Application.DTOs;
using InvoiceTracker.Application.Interfaces.Repositories;
using InvoiceTracker.Application.Interfaces.Services;
using InvoiceTracker.Domain.Entities;
using InvoiceTracker.Domain.Interfaces;
using InvoiceTracker.Domain.Models;

namespace InvoiceTracker.Application.Services;

public sealed class NotificationManager(
    IEmailAuditRepository emailAuditRepository,
    IEmailService emailService,
    IInvoiceService invoiceService) : INotificationService
{
    public async Task<PagedResult<EmailAuditSummaryDto>> GetPage(
        int pageNumber,
        int pageSize,
        string? searchTerm,
        CancellationToken cancellationToken)
    {
        var result = await emailAuditRepository.GetPage(pageNumber, pageSize, searchTerm, cancellationToken);
        return new PagedResult<EmailAuditSummaryDto>(
            result.Items.Select(MapSummary).ToArray(),
            result.PageNumber,
            result.PageSize,
            result.TotalCount);
    }

    public async Task<EmailAuditDetailsDto?> GetById(int emailAuditId, CancellationToken cancellationToken)
    {
        var email = await emailAuditRepository.GetById(emailAuditId, cancellationToken);
        return email is null ? null : new EmailAuditDetailsDto(
            email.EmailAuditId,
            email.To,
            email.CC,
            email.Subject,
            email.Body,
            email.ItemType,
            email.ItemTypeRef,
            email.HasAttachments,
            email.IsSent,
            email.SentDate);
    }

    public async Task<bool> Resend(int emailAuditId, string createdBy, CancellationToken cancellationToken)
    {
        var email = await emailAuditRepository.GetById(emailAuditId, cancellationToken);
        if (email is null)
        {
            return false;
        }

        var attachments = new List<EmailAttachment>();
        if (email.HasAttachments)
        {
            if (!string.Equals(email.ItemType, "InvoiceStatement", StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException("The original attachment cannot be recovered from this email audit.");
            }

            var invoice = await invoiceService.GetById(email.ItemTypeRef, cancellationToken);
            var document = await invoiceService.GenerateInvoiceToDocument(email.ItemTypeRef, cancellationToken);
            if (invoice is null || document is null)
            {
                throw new InvalidOperationException("The invoice attachment could not be regenerated.");
            }

            attachments.Add(new EmailAttachment
            {
                Content = document,
                ContentType = "application/pdf",
                FileName = $"Invoice_{invoice.Reference}.pdf"
            });
        }

        return await emailService.SendEmail(
            email.To,
            email.Subject,
            email.ItemType,
            email.ItemTypeRef,
            email.Body,
            attachments,
            createdBy,
            cancellationToken,
            email.CC);
    }

    private static EmailAuditSummaryDto MapSummary(EmailAudit email) => new(
        email.EmailAuditId,
        email.To,
        email.CC,
        email.Subject,
        email.ItemType,
        email.ItemTypeRef,
        email.HasAttachments,
        email.IsSent,
        email.SentDate);
}

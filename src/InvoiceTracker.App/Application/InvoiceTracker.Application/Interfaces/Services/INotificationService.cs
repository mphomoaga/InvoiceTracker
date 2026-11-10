using InvoiceTracker.Application.DTOs;
using InvoiceTracker.Domain.Models;

namespace InvoiceTracker.Application.Interfaces.Services;

public interface INotificationService
{
    Task<PagedResult<EmailAuditSummaryDto>> GetPage(
        int pageNumber,
        int pageSize,
        string? searchTerm,
        CancellationToken cancellationToken);

    Task<EmailAuditDetailsDto?> GetById(int emailAuditId, CancellationToken cancellationToken);

    Task<bool> Resend(int emailAuditId, string createdBy, CancellationToken cancellationToken);
}

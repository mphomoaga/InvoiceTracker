using InvoiceTracker.Domain.Entities;
using InvoiceTracker.Domain.Models;

namespace InvoiceTracker.Application.Interfaces.Repositories;

public interface IEmailAuditRepository
{
    Task<PagedResult<EmailAudit>> GetPage(
        int pageNumber,
        int pageSize,
        string? searchTerm,
        CancellationToken cancellationToken);

    Task<EmailAudit?> GetById(int emailAuditId, CancellationToken cancellationToken);
}

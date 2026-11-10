using InvoiceTracker.Application.Interfaces.Repositories;
using InvoiceTracker.Domain.Entities;
using InvoiceTracker.Domain.Models;
using InvoiceTracker.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace InvoiceTracker.Infrastructure.Repository;

public sealed class EmailAuditRepository(InvoiceTrackerDbContext context) : IEmailAuditRepository
{
    public async Task<PagedResult<EmailAudit>> GetPage(
        int pageNumber,
        int pageSize,
        string? searchTerm,
        CancellationToken cancellationToken)
    {
        var emails = context.EmailAudits.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var search = searchTerm.Trim();
            var hasEmailAuditId = int.TryParse(search, out var emailAuditId);
            var hasItemTypeRef = int.TryParse(search, out var itemTypeRef);
            emails = emails.Where(email =>
                email.To.Contains(search)
                || (email.CC != null && email.CC.Contains(search))
                || email.Subject.Contains(search)
                || email.Body.Contains(search)
                || email.ItemType.Contains(search)
                || (hasEmailAuditId && email.EmailAuditId == emailAuditId)
                || (hasItemTypeRef && email.ItemTypeRef == itemTypeRef));
        }

        var totalCount = await emails.CountAsync(cancellationToken);
        var items = await emails
            .OrderByDescending(email => email.SentDate)
            .ThenByDescending(email => email.EmailAuditId)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<EmailAudit>(items, pageNumber, pageSize, totalCount);
    }

    public Task<EmailAudit?> GetById(int emailAuditId, CancellationToken cancellationToken)
    {
        return context.EmailAudits
            .AsNoTracking()
            .FirstOrDefaultAsync(email => email.EmailAuditId == emailAuditId, cancellationToken);
    }
}

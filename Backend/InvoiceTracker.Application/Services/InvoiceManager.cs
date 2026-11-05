using InvoiceTracker.Domain.Interfaces.Services;
using InvoiceTracker.Domain.Models.Invoice;
using InvoiceTracker.Infrastructure.Persistence;
using InvoiceTracker.Infrastructure.Persistence.DatabaseModels;
using Microsoft.EntityFrameworkCore;

namespace InvoiceTracker.Application.Services;

public sealed class InvoiceManager : IInvoiceService
{
    private readonly AppDbContext _context;

    public InvoiceManager(AppDbContext context)
    {
        _context = context;
    }

    public async Task<InvoiceDto?> GetByIdAsync(
        int invoiceId,
        CancellationToken cancellationToken)
    {
        var invoice = await _context.Invoices
            .AsNoTracking()
            .Include(item => item.Person)
            .Include(item => item.InvoiceStatus)
            .Include(item => item.InvoiceLineItems.Where(lineItem => lineItem.IsActive))
            .SingleOrDefaultAsync(
                item => item.InvoiceId == invoiceId && item.IsActive,
                cancellationToken);

        return invoice is null ? null : Map(invoice);
    }

    public async Task<PagedResult<InvoiceDto>> GetPagedAsync(
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken)
    {
        pageNumber = Math.Max(pageNumber, 1);
        pageSize = Math.Clamp(pageSize, 1, 100);

        var query = _context.Invoices
            .AsNoTracking()
            .Where(invoice => invoice.IsActive);

        var totalCount = await query.CountAsync(cancellationToken);
        var invoices = await query
            .Include(item => item.Person)
            .Include(item => item.InvoiceStatus)
            .Include(item => item.InvoiceLineItems.Where(lineItem => lineItem.IsActive))
            .OrderByDescending(item => item.InvoiceDate)
            .ThenByDescending(item => item.InvoiceId)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<InvoiceDto>(
            invoices.Select(Map).ToList(),
            pageNumber,
            pageSize,
            totalCount);
    }

    public async Task<InvoiceDto> AddAsync(
        CreateInvoiceRequest request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var personExists = await _context.People.AnyAsync(
            person => person.PersonId == request.PersonId && person.IsActive,
            cancellationToken);
        if (!personExists)
        {
            throw new InvalidOperationException("The specified person does not exist or is inactive.");
        }

        var statusExists = await _context.InvoiceStatuses.AnyAsync(
            status => status.InvoiceStatusId == request.InvoiceStatusId && status.IsActive,
            cancellationToken);
        if (!statusExists)
        {
            throw new InvalidOperationException("The specified invoice status does not exist or is inactive.");
        }

        var now = DateTime.UtcNow;
        var invoice = new Invoice
        {
            PersonId = request.PersonId,
            Reference = request.Reference,
            InvoiceDate = request.InvoiceDate,
            InvoiceDueDate = request.InvoiceDueDate,
            InvoiceStatusId = request.InvoiceStatusId,
            InvoiceAmount = request.InvoiceAmount,
            IsActive = true,
            CreatedBy = request.CreatedBy,
            CreatedDate = now,
            InvoiceLineItems = request.LineItems?
                .Select(lineItem => new InvoiceLineItem
                {
                    Reference = lineItem.Reference,
                    Amount = lineItem.Amount,
                    IsActive = true,
                    CreatedBy = request.CreatedBy,
                    CreatedDate = now
                })
                .ToList() ?? []
        };

        _context.Invoices.Add(invoice);
        await _context.SaveChangesAsync(cancellationToken);

        var savedInvoice = await _context.Invoices
            .AsNoTracking()
            .Include(item => item.Person)
            .Include(item => item.InvoiceStatus)
            .Include(item => item.InvoiceLineItems.Where(lineItem => lineItem.IsActive))
            .SingleAsync(item => item.InvoiceId == invoice.InvoiceId, cancellationToken);

        return Map(savedInvoice);
    }

    public async Task<bool> RemoveInvoice(
        int invoiceId,
        string modifiedBy,
        CancellationToken cancellationToken)
    {
        var invoice = await _context.Invoices
            .Include(item => item.InvoiceLineItems)
            .SingleOrDefaultAsync(
                item => item.InvoiceId == invoiceId && item.IsActive,
                cancellationToken);

        if (invoice is null)
        {
            return false;
        }

        var now = DateTime.UtcNow;
        invoice.IsActive = false;
        invoice.ModifiedBy = modifiedBy;
        invoice.ModifiedDate = now;

        foreach (var lineItem in invoice.InvoiceLineItems.Where(item => item.IsActive))
        {
            lineItem.IsActive = false;
            lineItem.ModifiedBy = modifiedBy;
            lineItem.ModifiedDate = now;
        }

        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    private static InvoiceDto Map(Invoice invoice)
    {
        return new InvoiceDto(
            invoice.InvoiceId,
            invoice.PersonId,
            invoice.Reference,
            invoice.InvoiceDate,
            invoice.InvoiceDueDate,
            invoice.InvoiceStatusId,
            invoice.InvoiceAmount,
            invoice.InvoiceStatus?.Value,
            invoice.Person is null
                ? null
                : $"{invoice.Person.FullNames} {invoice.Person.Surname}".Trim(),
            invoice.InvoiceLineItems
                .Where(item => item.IsActive)
                .Select(item => new InvoiceLineItemDto(
                    item.InvoiceLineItemId,
                    item.Reference,
                    item.Amount))
                .ToList());
    }
}

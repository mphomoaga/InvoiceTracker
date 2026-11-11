using InvoiceTracker.Application.Interfaces.Repositories;
using InvoiceTracker.Domain.Enums;
using InvoiceTracker.Domain.Models;
using InvoiceTracker.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace InvoiceTracker.Infrastructure.Repository
{
    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly InvoiceTrackerDbContext _context;

        public InvoiceRepository(InvoiceTrackerDbContext context) => _context = context;

        public async Task<Invoice> AddInvoice(Invoice request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);

            var now = DateTime.UtcNow;
            var invoiceEntity = new InvoiceTracker.Domain.Entities.Invoice
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
                InvoiceLineItems = request.LineItems.Select(lineItem => new InvoiceTracker.Domain.Entities.InvoiceLineItem
                {
                    Reference = lineItem.Reference,
                    Amount = lineItem.Amount,
                    IsActive = true,
                    CreatedBy = request.CreatedBy,
                    CreatedDate = now
                }).ToList()
            };

            _context.Invoices.Add(invoiceEntity);
            await _context.SaveChangesAsync(cancellationToken);

            return MapInvoiceEntityToModel(invoiceEntity);
        }

        public async Task<Invoice> GetById(int invoiceId, CancellationToken cancellationToken)
        {
            var invoiceEntity = await _context.Invoices
                .AsNoTracking()
                .Include(invoice => invoice.Person)
                .Include(invoice => invoice.InvoiceLineItems)
                .FirstOrDefaultAsync(
                    invoice => invoice.InvoiceId == invoiceId && invoice.IsActive,
                    cancellationToken);

            return invoiceEntity is null ? null : MapInvoiceEntityToModel(invoiceEntity);
        }

        public async Task<PagedResult<Invoice>> GetPaged(
            int pageNumber,
            int pageSize,
            string? searchTerm,
            CancellationToken cancellationToken)
        {
            var invoices = _context.Invoices
                .AsNoTracking()
                .Where(invoice => invoice.IsActive);

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var search = searchTerm.Trim();
                invoices = invoices.Where(invoice =>
                    invoice.Reference.Contains(search)
                    || invoice.Person.FullNames.Contains(search)
                    || invoice.Person.Surname.Contains(search));
            }

            var totalCount = await invoices.CountAsync(cancellationToken);
            var invoiceEntities = await invoices
                .Include(invoice => invoice.Person)
                .Include(invoice => invoice.InvoiceLineItems)
                .OrderByDescending(invoice => invoice.InvoiceDate)
                .ThenByDescending(invoice => invoice.InvoiceId)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return new PagedResult<Invoice>(
                invoiceEntities.Select(MapInvoiceEntityToModel).ToList(),
                pageNumber,
                pageSize,
                totalCount);
        }

        public async Task<List<Invoice>> GetOverdueInvoicesAsync(
            CancellationToken cancellationToken)
        {
            var overdueCutoff = DateTime.UtcNow.AddDays(-15);

            var excludedStatuses = new[]
            {
                (int)InvoiceStatusEnum.Paid,
                (int)InvoiceStatusEnum.Draft,
                (int)InvoiceStatusEnum.WrittenOff,
                (int)InvoiceStatusEnum.Refunded,
                (int)InvoiceStatusEnum.Overdue,
            };

            var overdueInvoices = await _context.Invoices
                .AsNoTracking()
                .Include(i => i.Person)
                .Where(i =>
                    i.IsActive &&
                    !excludedStatuses.Contains(i.InvoiceStatusId) &&
                    i.InvoiceDueDate <= overdueCutoff)
                .ToListAsync(cancellationToken);

            return overdueInvoices
                .Select(MapInvoiceEntityToModel)
                .ToList();
        }

        public async Task<bool> RemoveInvoice(
            int invoiceId,
            string modifiedBy,
            CancellationToken cancellationToken)
        {
            var invoiceEntity = await _context.Invoices
                .FirstOrDefaultAsync(
                    invoice => invoice.InvoiceId == invoiceId,
                    cancellationToken);

            if (invoiceEntity is null)
            {
                return false;
            }

            invoiceEntity.IsActive = false;
            invoiceEntity.ModifiedBy = modifiedBy;
            invoiceEntity.ModifiedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }

        public async Task<bool> UpdateInvoice(Invoice invoice, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(invoice);

            var invoiceEntity = await _context.Invoices
                .FirstOrDefaultAsync(
                    item => item.InvoiceId == invoice.InvoiceId && item.IsActive,
                    cancellationToken);

            if (invoiceEntity is null)
            {
                return false;
            }

            invoiceEntity.PersonId = invoice.PersonId;
            invoiceEntity.Reference = invoice.Reference;
            invoiceEntity.InvoiceDate = invoice.InvoiceDate;
            invoiceEntity.InvoiceDueDate = invoice.InvoiceDueDate;
            invoiceEntity.InvoiceStatusId = invoice.InvoiceStatusId;
            invoiceEntity.InvoiceAmount = invoice.InvoiceAmount;
            invoiceEntity.ModifiedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<bool> UpdateInvoiceStatus(int invoiceId, InvoiceStatusEnum invoiceStatusEnum, string modifiedBy, CancellationToken cancellationToken)
        {
            var invoiceEntity = await _context.Invoices
                .Include(i => i.InvoiceStatus)
                .FirstOrDefaultAsync(
                    invoice => invoice.InvoiceId == invoiceId && invoice.IsActive,
                    cancellationToken);

            if (invoiceEntity is null)
            {
                return false;
            }

            invoiceEntity.InvoiceStatusId = (int)invoiceStatusEnum;
            invoiceEntity.ModifiedBy = modifiedBy;
            invoiceEntity.ModifiedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }

        private static Invoice MapInvoiceEntityToModel(InvoiceTracker.Domain.Entities.Invoice invoice)
        {
            return new Invoice
            {
                InvoiceId = invoice.InvoiceId,
                PersonId = invoice.PersonId,
                Reference = invoice.Reference,
                InvoiceDate = invoice.InvoiceDate,
                InvoiceDueDate = invoice.InvoiceDueDate,
                InvoiceStatusId = invoice.InvoiceStatusId,
                InvoiceAmount = invoice.InvoiceAmount,
                CreatedBy = invoice.CreatedBy,
                PersonFullName = invoice.Person is null
                    ? null
                    : $"{invoice.Person.FullNames} {invoice.Person.Surname}".Trim(),
                LineItems = invoice.InvoiceLineItems
                    .Where(lineItem => lineItem.IsActive)
                    .Select(lineItem => new InvoiceLineItem(
                        lineItem.InvoiceLineItemId,
                        lineItem.Reference,
                        lineItem.Amount))
                    .ToList()
            };
        }
    }
}

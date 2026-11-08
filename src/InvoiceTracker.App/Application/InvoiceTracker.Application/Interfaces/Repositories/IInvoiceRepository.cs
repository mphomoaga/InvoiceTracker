using InvoiceTracker.Application.DTOs;
using InvoiceTracker.Domain.Enums;
using InvoiceTracker.Domain.Models;

namespace InvoiceTracker.Application.Interfaces.Repositories
{
    public interface IInvoiceRepository
    {
        Task<Invoice?> GetById(
        int invoiceId,
        CancellationToken cancellationToken);
        Task<PagedResult<Invoice>> GetPaged(
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken);

        Task<Invoice> AddInvoice(
        Invoice request,
        CancellationToken cancellationToken);

        Task<bool> RemoveInvoice(
        int invoiceId,
        string modifiedBy,
        CancellationToken cancellationToken);

        Task<bool> UpdateInvoice(
        Invoice invoice,
        CancellationToken cancellationToken);

        Task<bool> UpdateInvoiceStatus(
        int invoiceId,
        InvoiceStatusEnum invoiceStatusEnum,
        string modifiedBy,
        CancellationToken cancellationToken);
    }
}

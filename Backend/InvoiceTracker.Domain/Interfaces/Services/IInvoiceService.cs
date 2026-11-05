using InvoiceTracker.Domain.Models.Invoice;

namespace InvoiceTracker.Domain.Interfaces.Services;

public interface IInvoiceService
{
    Task<InvoiceDto?> GetByIdAsync(int invoiceId, CancellationToken cancellationToken);

    Task<PagedResult<InvoiceDto>> GetPagedAsync(
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken);

    Task<InvoiceDto> AddAsync(
        CreateInvoiceRequest request,
        CancellationToken cancellationToken);

    Task<bool> RemoveInvoice(
        int invoiceId,
        string modifiedBy,
        CancellationToken cancellationToken);
}

using InvoiceTracker.Application.DTOs;
using InvoiceTracker.Domain.Enums;
using InvoiceTracker.Domain.Models;

namespace InvoiceTracker.Application.Interfaces.Services
{
    public interface IInvoiceService
    {
        Task<Invoice?> GetById(int invoiceId, CancellationToken cancellationToken);

        Task<PagedResult<Invoice>> GetPaged(
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken);

        Task<Invoice> AddNewInvoice(
            CreateInvoiceRequest request,
            CancellationToken cancellationToken);

        Task<bool> RemoveInvoice(
            int invoiceId,
            string modifiedBy,
            CancellationToken cancellationToken);

        Task<bool> UpdateInvoiceStatus(
           int invoiceId,
           InvoiceStatusEnum newStatus,

           CancellationToken cancellationToken);

        Task<bool> EmailInvoice(
         int invoiceId,
         int personId,
        CancellationToken cancellationToken);

        Task<byte[]> GenerateInvoiceToDocument(
        int invoiceId,
        CancellationToken cancellationToken);
    }

}

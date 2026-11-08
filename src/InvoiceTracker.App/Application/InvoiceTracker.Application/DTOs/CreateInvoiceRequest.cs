using InvoiceTracker.Domain.Enums;

namespace InvoiceTracker.Application.DTOs
{
    public sealed record CreateInvoiceRequest(
        int PersonId,
        string Reference,
        DateTime InvoiceDate,
        DateTime InvoiceDueDate,
        InvoiceStatusEnum InvoiceStatusEnum,
        decimal InvoiceAmount,
        string CreatedBy,
        List<InvoiceLineItemRequest>? LineItems = null);
}

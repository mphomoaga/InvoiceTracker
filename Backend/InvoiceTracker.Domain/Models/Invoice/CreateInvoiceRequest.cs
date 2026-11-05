namespace InvoiceTracker.Domain.Models.Invoice;

public sealed record CreateInvoiceRequest(
    int PersonId,
    string Reference,
    DateTime InvoiceDate,
    DateTime InvoiceDueDate,
    int InvoiceStatusId,
    decimal InvoiceAmount,
    string CreatedBy,
    IReadOnlyCollection<InvoiceLineItemRequest>? LineItems = null);

namespace InvoiceTracker.Domain.Models.Invoice;

public sealed record InvoiceDto(
    int InvoiceId,
    int PersonId,
    string Reference,
    DateTime InvoiceDate,
    DateTime InvoiceDueDate,
    int InvoiceStatusId,
    decimal InvoiceAmount,
    string? Status,
    string? PersonName,
    IReadOnlyCollection<InvoiceLineItemDto> LineItems);

public sealed record InvoiceLineItemDto(
    int InvoiceLineItemId,
    string Reference,
    decimal Amount);

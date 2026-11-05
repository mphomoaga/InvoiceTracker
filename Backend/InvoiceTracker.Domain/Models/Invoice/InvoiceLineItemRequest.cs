namespace InvoiceTracker.Domain.Models.Invoice;

public sealed record InvoiceLineItemRequest(
    string Reference,
    decimal Amount);

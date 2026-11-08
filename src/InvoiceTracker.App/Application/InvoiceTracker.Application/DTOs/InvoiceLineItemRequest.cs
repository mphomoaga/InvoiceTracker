namespace InvoiceTracker.Application.DTOs
{
    public sealed record InvoiceLineItemRequest(
    string Reference,
    decimal Amount);

}

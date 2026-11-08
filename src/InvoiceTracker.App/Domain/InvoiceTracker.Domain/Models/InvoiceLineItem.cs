namespace InvoiceTracker.Domain.Models
{
    public sealed record InvoiceLineItem(
        int InvoiceLineItemId,
        string Reference,
        decimal Amount);

}

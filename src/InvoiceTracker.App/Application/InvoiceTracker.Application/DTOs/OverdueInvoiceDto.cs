namespace InvoiceTracker.Application.DTOs;

public sealed class OverdueInvoiceDto
{
    public int InvoiceId { get; init; }
    public string Reference { get; init; } = string.Empty;
    public DateTime InvoiceDueDate { get; init; }
    public decimal InvoiceAmount { get; init; }
    public string CustomerEmail { get; init; } = string.Empty;
    public string CustomerName { get; init; } = string.Empty;
}

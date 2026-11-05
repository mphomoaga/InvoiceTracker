namespace InvoiceTracker.Domain.Enums;

public enum InvoiceStatusType
{
    Draft = 1,
    Pending = 2,
    Sent = 3,
    Viewed = 4,
    PartiallyPaid = 5,
    Paid = 6,
    Overdue = 7,
    Cancelled = 8,
    Refunded = 9,
    WrittenOff = 10
}
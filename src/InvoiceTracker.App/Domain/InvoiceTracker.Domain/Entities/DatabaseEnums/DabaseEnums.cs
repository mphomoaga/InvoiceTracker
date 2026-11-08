using System.ComponentModel;

namespace InvoiceTracker.Domain.Entities.DatabaseEnums
{
    public enum InvoiceStatus
    {
        [Description("Draft")]
        Draft = 1,
        [Description("Pending")]
        Pending = 2,
        [Description("Sent")]
        Sent = 3,
        [Description("Viewed")]
        Viewed = 4,
        [Description("Partially Paid")]
        PartiallyPaid = 5,
        [Description("Paid")]
        Paid = 6,
        [Description("Overdue")]
        Overdue = 7,
        [Description("Cancelled")]
        Cancelled = 8,
        [Description("Refunded")]
        Refunded = 9,
        [Description("Written Off")]
        WrittenOff = 10
    }
}

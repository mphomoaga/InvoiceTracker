using System;
using System.Collections.Generic;

namespace InvoiceTracker.Infrastructure.Persistence.DatabaseModels;

public partial class Payment
{
    public int PaymentId { get; set; }

    public string Reference { get; set; } = null!;

    public decimal Amount { get; set; }

    public bool IsActive { get; set; }

    public DateTime PaymentDate { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedDate { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public virtual ICollection<InvoicePaymentAllocation> InvoicePaymentAllocations { get; set; } = new List<InvoicePaymentAllocation>();
}

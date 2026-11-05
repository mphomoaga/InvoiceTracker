using System;
using System.Collections.Generic;

namespace InvoiceTracker.Infrastructure.Persistence.DatabaseModels;

public partial class InvoiceLineItem
{
    public int InvoiceLineItemId { get; set; }

    public int InvoiceId { get; set; }

    public string Reference { get; set; } = null!;

    public decimal Amount { get; set; }

    public bool IsActive { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedDate { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public virtual Invoice Invoice { get; set; } = null!;
}

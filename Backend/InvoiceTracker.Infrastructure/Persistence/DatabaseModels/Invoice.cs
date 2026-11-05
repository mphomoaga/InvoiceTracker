using System;
using System.Collections.Generic;

namespace InvoiceTracker.Infrastructure.Persistence.DatabaseModels;

public partial class Invoice
{
    public int InvoiceId { get; set; }

    public int PersonId { get; set; }

    public string Reference { get; set; } = null!;

    public DateTime InvoiceDate { get; set; }

    public DateTime InvoiceDueDate { get; set; }

    public int InvoiceStatusId { get; set; }

    public decimal InvoiceAmount { get; set; }

    public bool IsActive { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedDate { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public virtual ICollection<InvoiceLineItem> InvoiceLineItems { get; set; } = new List<InvoiceLineItem>();

    public virtual ICollection<InvoicePaymentAllocation> InvoicePaymentAllocations { get; set; } = new List<InvoicePaymentAllocation>();

    public virtual InvoiceStatus InvoiceStatus { get; set; } = null!;

    public virtual Person Person { get; set; } = null!;
}

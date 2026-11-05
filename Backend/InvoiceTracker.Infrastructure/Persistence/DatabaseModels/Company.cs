using System;
using System.Collections.Generic;

namespace InvoiceTracker.Infrastructure.Persistence.DatabaseModels;

public partial class Company
{
    public int CompanyId { get; set; }

    public int PersonId { get; set; }

    public string? CompanyName { get; set; }

    public string? RegNumber { get; set; }

    public string VATNumber { get; set; } = null!;

    public bool IsActive { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedDate { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public virtual Person Person { get; set; } = null!;
}

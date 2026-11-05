using System;
using System.Collections.Generic;

namespace InvoiceTracker.Infrastructure.Persistence.DatabaseModels;

public partial class Audit
{
    public int AuditId { get; set; }

    public string AuditType { get; set; } = null!;

    public string? SourceData { get; set; }

    public string? TargetData { get; set; }

    public bool IsActive { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedDate { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }
}

using System;
using System.Collections.Generic;

namespace InvoiceTracker.Infrastructure.Persistence.DatabaseModels;

public partial class SmsAudit
{
    public int SmsAuditId { get; set; }

    public string ItemType { get; set; } = null!;

    public int ItemTypeRef { get; set; }

    public string Cellphone { get; set; } = null!;

    public string Message { get; set; } = null!;

    public bool IsSent { get; set; }

    public DateTime SentDate { get; set; }

    public bool IsActive { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedDate { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }
}

using System;
using System.Collections.Generic;

namespace InvoiceTracker.Infrastructure.Persistence.DatabaseModels;

public partial class EmailTemplate
{
    public int EmailTemplateId { get; set; }

    public string TemplateName { get; set; } = null!;

    public string Value { get; set; } = null!;

    public bool IsActive { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedDate { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }
}

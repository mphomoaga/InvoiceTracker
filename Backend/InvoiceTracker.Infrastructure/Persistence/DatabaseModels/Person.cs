using System;
using System.Collections.Generic;

namespace InvoiceTracker.Infrastructure.Persistence.DatabaseModels;

public partial class Person
{
    public int PersonId { get; set; }

    public string? Title { get; set; }

    public string? Intials { get; set; }

    public string FullNames { get; set; } = null!;

    public string Surname { get; set; } = null!;

    public string? IdNumber { get; set; }

    public string? PassportNumber { get; set; }

    public bool IsActive { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedDate { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public virtual ICollection<Company> Companies { get; set; } = new List<Company>();

    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
}

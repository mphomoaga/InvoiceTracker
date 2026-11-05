using System;
using System.Collections.Generic;

namespace InvoiceTracker.Infrastructure.Persistence.DatabaseModels;

public partial class User
{
    public int UserId { get; set; }

    public string LastName { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? ContactNumber { get; set; }

    public bool IsLocked { get; set; }

    public string PasswordHash { get; set; } = null!;

    public DateTime? LastLoginUtc { get; set; }

    public bool IsActive { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedDate { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public int RoleId { get; set; }

    public virtual Role Role { get; set; } = null!;
}

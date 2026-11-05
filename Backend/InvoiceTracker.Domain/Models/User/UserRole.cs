using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceTracker.Domain.Models.User
{
    public record UserRole(int RoleId, bool IsActive, DateTime CreatedDate, string? ModifiedBy, DateTime? ModifiedDate)
    {
        public string RoleName { get; set; } = null!;
        public string Email { get; set; } = null!;

        public string UserName { get; set; } = null!;

        public string CreatedBy { get; set; } = null!;
    }
}

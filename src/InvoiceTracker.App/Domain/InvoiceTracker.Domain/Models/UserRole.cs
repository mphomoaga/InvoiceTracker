namespace InvoiceTracker.Domain.Models
{
    namespace InvoiceTracker.Domain.Entities
    {
        public class UserRole
        {
            public int RoleId { get; set; }
            public string RoleName { get; set; } = null!;
            public string Description { get; set; } = null!;
            public bool IsActive { get; set; }
            public string CreatedBy { get; set; } = null!;
            public DateTime CreatedDate { get; set; } 
            public string? ModifiedBy { get; set; }
            public DateTime? ModifiedDate { get; set; }

        }
    }

}

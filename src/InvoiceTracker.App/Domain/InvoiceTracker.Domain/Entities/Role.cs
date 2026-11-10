using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvoiceTracker.Domain.Entities
{
    [Table("Roles", Schema = "security")]
    public class Role
    {
        [Key]
        public int RoleId { get; set; }
        
        [MaxLength(50)]
        public string RoleName { get; set; } = null!;
        [MaxLength(100)]
        public string Description { get; set; } = null!;

        public bool IsActive { get; set; } = true;

        [MaxLength(100)]
        public string CreatedBy { get; set; } = null!;

        public DateTime CreatedDate { get; set; }

        [MaxLength(100)]
        public string? ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public virtual ICollection<User> Users { get; set; } = new List<User>();
    }
}

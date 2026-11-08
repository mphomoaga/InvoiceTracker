using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InvoiceTracker.Domain.Entities
{
    [Table("Users", Schema = "security")]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        [MaxLength(100)]
        public string FirstName { get; set; } = null!;
        [MaxLength(100)]
        public string LastName { get; set; } = null!;

        [MaxLength(100)]
        public string Email { get; set; } = null!;

        public bool IsLocked { get; set; } = false;

        public string PasswordHash { get; set; } = null!;

        public DateTime? LastLoginUtc { get; set; }

        public bool IsActive { get; set; }

        public string CreatedBy { get; set; } = null!;

        public DateTime CreatedDate { get; set; }

        public string? ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }
        public int RoleId { get; set; }

        [ForeignKey("RoleId")]
        public Role Role { get; set; } = null!;
    }
}

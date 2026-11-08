using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvoiceTracker.Domain.Entities
{
    [Table("Audit", Schema = "audit")]
    public class Audit
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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
}

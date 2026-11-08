using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InvoiceTracker.Domain.Entities
{
    [Table("EmailAudit", Schema = "communication")]
    public class EmailAudit
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EmailAuditId { get; set; }

        public string ItemType { get; set; } = null!;

        public int ItemTypeRef { get; set; }

        public string From { get; set; } = null!;

        public string To { get; set; } = null!;

        public string? CC { get; set; }

        public string Subject { get; set; } = null!;

        public string Body { get; set; } = null!;

        public bool HasAttachments { get; set; }

        public bool IsSent { get; set; }

        public DateTime SentDate { get; set; }

        public bool IsActive { get; set; }

        public string CreatedBy { get; set; } = null!;

        public DateTime CreatedDate { get; set; }

        public string? ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }
    }
}

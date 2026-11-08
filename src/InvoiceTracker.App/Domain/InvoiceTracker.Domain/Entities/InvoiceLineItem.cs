using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvoiceTracker.Domain.Entities
{
    [Table("InvoiceLineItem", Schema = "billing")]
    public  class InvoiceLineItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int InvoiceLineItemId { get; set; }

        public int InvoiceId { get; set; }

        public string Reference { get; set; } = null!;

        public decimal Amount { get; set; }

        public bool IsActive { get; set; }

        public string CreatedBy { get; set; } = null!;

        public DateTime CreatedDate { get; set; }

        public string? ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [ForeignKey("InvoiceId")]
        public virtual Invoice Invoice { get; set; } = null!;
    }
}
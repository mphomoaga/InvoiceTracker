using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvoiceTracker.Domain.Entities
{
    [Table("Invoice", Schema = "billing")]
    public class Invoice
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int InvoiceId { get; set; }

        public int PersonId { get; set; }

        public string Reference { get; set; } = null!;

        public DateTime InvoiceDate { get; set; }

        public DateTime InvoiceDueDate { get; set; }

        public int InvoiceStatusId { get; set; }

        public decimal InvoiceAmount { get; set; }

        public bool IsActive { get; set; } = true;

        public string CreatedBy { get; set; } = null!;

        public DateTime CreatedDate { get; set; }

        public string? ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public virtual ICollection<InvoiceLineItem> InvoiceLineItems { get; set; } = new List<InvoiceLineItem>();

        [ForeignKey("PersonId")]
        public virtual Person Person { get; set; } = null!;

        [ForeignKey("InvoiceStatusId")]
        public virtual InvoiceStatus InvoiceStatus { get; set; } = null!;
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvoiceTracker.Domain.Entities
{

    [Table("InvoiceStatuses", Schema = "common")]
    public class InvoiceStatus
    {
        [Key]
        public int InvoiceStatusId { get; set; }

        public string Description { get; set; } = string.Empty;
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvoiceTracker.Domain.Entities
{
    [Table("EmailTemplate", Schema = "template")]
    public class EmailTemplate
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EmailTemplateId { get; set; }

        public string TemplateName { get; set; } = null!;

        public string Value { get; set; } = null!;

        public bool IsActive { get; set; }

        public string CreatedBy { get; set; } = null!;

        public DateTime CreatedDate { get; set; }

        public string? ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }
    }
}

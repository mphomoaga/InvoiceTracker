using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InvoiceTracker.Domain.Entities
{
    [Table("Company", Schema = "client")]
    public class Company
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CompanyId { get; set; }

        public int PersonId { get; set; }

        public string? CompanyName { get; set; }

        public string? RegNumber { get; set; }

        public string VATNumber { get; set; } = null!;

        public bool IsActive { get; set; }

        public string CreatedBy { get; set; } = null!;

        public DateTime CreatedDate { get; set; }

        public string? ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [ForeignKey("PersonId")]
        public virtual Person Person { get; set; } = null!;
    }

}

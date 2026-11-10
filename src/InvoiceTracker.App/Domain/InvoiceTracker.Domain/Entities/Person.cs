using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InvoiceTracker.Domain.Entities
{
    [Table("Person", Schema = "dbo")]
    public  class Person
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PersonId { get; set; }

        public string? Title { get; set; }

        public string? Intials { get; set; }

        public string FullNames { get; set; } = null!;

        public string Surname { get; set; } = null!;

        public string? IdNumber { get; set; }

        public string? PassportNumber { get; set; }

        public bool IsActive { get; set; }

        public string CreatedBy { get; set; } = null!;

        public DateTime CreatedDate { get; set; }

        public string? ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public virtual ICollection<Company> Companies { get; set; } = new List<Company>();

        public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
    }
}

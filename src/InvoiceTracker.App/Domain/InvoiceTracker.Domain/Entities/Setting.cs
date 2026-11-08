using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvoiceTracker.Domain.Entities
{
    [Table("Settings", Schema = "settings")]
    public class Setting
    {
        [Key]
        public int SettingId { get; set; }

        public string Key { get; set; } = null!;

        public string Value { get; set; } = null!;

        public bool IsActive { get; set; } = true;

        public string CreatedBy { get; set; } = null!;

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public string? ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; } = DateTime.UtcNow;
    }
}

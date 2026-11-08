using System.ComponentModel.DataAnnotations;

namespace InvoiceTracker.Application.DTOs
{
    public class RegisterUserRequest
    {
        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; } = null!;
        [Required]
        [MaxLength(100)]
        public string LastName { get; set; } = null!;
        [Required]

        [MaxLength(100)]
        public string Email { get; set; } = null!;
        [Required]
        [MaxLength(50)]
        public string Password { get; set; } = null!;
        [Range(1, int.MaxValue)]
        public int RoleId { get; set; }
    }
}

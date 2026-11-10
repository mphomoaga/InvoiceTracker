using System.ComponentModel.DataAnnotations;

namespace InvoiceTracker.Application.DTOs;

public sealed class EmailInvoiceRequest
{
    [Required]
    [EmailAddress]
    public string RecipientEmail { get; set; } = string.Empty;
}

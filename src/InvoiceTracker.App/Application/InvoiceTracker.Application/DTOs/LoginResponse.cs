using InvoiceTracker.Domain.Models.InvoiceTracker.Domain.Entities;

namespace InvoiceTracker.Application.DTOs
{
    public record LoginResponse(bool LoginSuccess, string AccessToken, UserRole Role, string Message, DateTime ExpiresAt)
   ;
}

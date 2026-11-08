namespace InvoiceTracker.Application.DTOs
{
    public record LoginRequest(string Email, string Password, string AuthType);
}

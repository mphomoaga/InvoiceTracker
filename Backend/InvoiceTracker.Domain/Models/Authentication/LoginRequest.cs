using MediatR;

namespace InvoiceTracker.Domain.Models.Authentication;

public record LoginRequest(string Username, string Email, string Password, string AuthType)
{
    public LoginRequest(string email, string password, string authType) : this(default, email, password, authType)
    {
    }
}

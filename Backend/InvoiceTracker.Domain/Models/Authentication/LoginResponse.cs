using InvoiceTracker.Domain.Models.User;

namespace InvoiceTracker.Domain.Models.Authentication;

public record LoginResponse(string AccessToken, string Token,List<UserRole> Roles, DateTime ExpiresAt);

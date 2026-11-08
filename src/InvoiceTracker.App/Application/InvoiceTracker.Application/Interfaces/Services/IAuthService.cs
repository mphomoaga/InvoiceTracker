using InvoiceTracker.Application.DTOs;
using InvoiceTracker.Domain.Models.InvoiceTracker.Domain.Entities;

namespace InvoiceTracker.Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task<LoginResponse> LoginRequest(LoginRequest loginRequest, CancellationToken cancellationToken);
        Task<RegisterUserResponse> Register(RegisterUserRequest registerUser, CancellationToken cancellationToken);
        Task<List<UserRole>> GetUserRoles(int userId, CancellationToken cancellationToken);
    }
}

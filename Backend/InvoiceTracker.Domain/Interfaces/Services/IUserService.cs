using InvoiceTracker.Domain.Models.Authentication;
using InvoiceTracker.Domain.Models.User;

namespace InvoiceTracker.Domain.Interfaces.Services
{
    public interface IUserService
    {
        Task<LoginResponse> LoginRequest(LoginRequest loginRequest, CancellationToken cancellationToken);
        Task<bool> Register(RegisterUserRequest registerUser, CancellationToken cancellationToken);
        Task<List<UserRole>> GetUserRoles(int userId, CancellationToken cancellationToken);
    }
}
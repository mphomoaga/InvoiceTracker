using InvoiceTracker.Domain.Entities;

namespace InvoiceTracker.Application.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<User> AddUser(
            User user,
            CancellationToken cancellationToken = default);
        Task<bool> UpdateUser(
            User user,
            CancellationToken cancellationToken = default);
        Task<User?> GetUserByEmail(
            string email,
            CancellationToken cancellationToken = default);
        Task<bool> UserExistsAsync(
            string email,
            CancellationToken cancellationToken = default);
        Task<User?> GetUserById(
            int userId,
            CancellationToken cancellationToken = default);
    }
}

using InvoiceTracker.Application.Interfaces.Repositories;
using InvoiceTracker.Domain.Entities;
using InvoiceTracker.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace InvoiceTracker.Infrastructure.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly InvoiceTrackerDbContext _context;

        public UserRepository(InvoiceTrackerDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<User> AddUser(
            User user,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(user);

            await _context.Users.AddAsync(user, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return user;
        }

        public async Task<bool> UpdateUser(
            User user,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(user);

            var existingUser = await _context.Users
                .FirstOrDefaultAsync(existing => existing.UserId == user.UserId, 
                cancellationToken);
            
            if (existingUser is null)
            {
                return false;
            }

            user.ModifiedDate = DateTime.UtcNow;
            _context.Entry(existingUser).CurrentValues.SetValues(user);
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }

        public Task<User?> GetUserByEmail(
            string email,
            CancellationToken cancellationToken = default)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(email);

            return _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    user => user.Email == email 
                    && user.IsActive == true
                    && user.IsLocked == false,
                    cancellationToken);
        }

        public Task<bool> UserExistsAsync(
            string email,
            CancellationToken cancellationToken = default)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(email);

            return _context.Users
                .AsNoTracking()
                .AnyAsync(user => user.Email == email, cancellationToken);
        }

        public Task<User?> GetUserById(
            int userId,
            CancellationToken cancellationToken = default)
        {
            return _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    user => user.UserId == userId,
                    cancellationToken);
        }
    }
}

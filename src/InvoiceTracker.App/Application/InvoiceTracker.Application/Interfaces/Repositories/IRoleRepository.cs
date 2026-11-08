using InvoiceTracker.Domain.Models.InvoiceTracker.Domain.Entities;

namespace InvoiceTracker.Application.Interfaces.Repositories
{
    public interface IRoleRepository
    {
        Task<UserRole> GetUserRole(int roleId);
        Task<List<UserRole>> GetAllRoles();
    }
}

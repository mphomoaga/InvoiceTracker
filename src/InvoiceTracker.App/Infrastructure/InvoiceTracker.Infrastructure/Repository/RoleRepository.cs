using InvoiceTracker.Application.Interfaces.Repositories;
using InvoiceTracker.Domain.Entities;
using InvoiceTracker.Domain.Models.InvoiceTracker.Domain.Entities;
using InvoiceTracker.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceTracker.Infrastructure.Repository
{
    public class RoleRepository : IRoleRepository
    {
        private readonly InvoiceTrackerDbContext _context;
        public RoleRepository(InvoiceTrackerDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public Task<List<UserRole>> GetAllRoles()
        {
            return _context.Roles
                .AsNoTracking()
                .Where(role => role.IsActive == true)
                .Select(role => new UserRole
                {
                    RoleId = role.RoleId,
                    RoleName = role.RoleName,
                    Description = role.Description,
                    IsActive = role.IsActive,
                    CreatedBy = role.CreatedBy,
                    CreatedDate = role.CreatedDate,
                    ModifiedBy = role.ModifiedBy,
                    ModifiedDate = role.ModifiedDate
                })
                .ToListAsync();
        }

        public async Task<UserRole> GetUserRole(int roleId)
        {          
                
                var entity = await _context.Roles
                 .AsNoTracking()
                 .Where(role => role.RoleId == roleId)
                 .Select(role => new UserRole
                 {
                     RoleId = role.RoleId,
                     RoleName = role.RoleName,
                     Description = role.Description,
                     IsActive = role.IsActive,
                     CreatedBy = role.CreatedBy,
                     CreatedDate = role.CreatedDate,
                     ModifiedBy = role.ModifiedBy,
                     ModifiedDate = role.ModifiedDate
                 })
                 .FirstOrDefaultAsync();
            return entity;
        }
    }
}

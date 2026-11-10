using InvoiceTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;

namespace InvoiceTracker.Infrastructure.Persistence.Configuration
{
    public class RolesConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            var adminCreatedDate = new DateTime(2026, 11, 8, 17, 49, 23, 980, DateTimeKind.Utc);
            var adminModifiedDate = new DateTime(2026, 11, 8, 17, 49, 23, 980, DateTimeKind.Utc);
            var userCreatedDate = new DateTime(2026, 11, 8, 17, 49, 23, 980, DateTimeKind.Utc);
            var userModifiedDate = new DateTime(2026, 11, 8, 17, 49, 23, 980, DateTimeKind.Utc);
            var invoiceManagerCreatedDate = new DateTime(2026, 11, 8, 17, 49, 23, 980, DateTimeKind.Utc);
            var invoiceManagerModifiedDate = new DateTime(2026, 11, 8, 17, 49, 23, 980, DateTimeKind.Utc);
            var invoiceAgentCreatedDate = new DateTime(2026, 11, 8, 17, 49, 23, 980, DateTimeKind.Utc);
            var invoiceAgentModifiedDate = new DateTime(2026, 11, 8, 17, 49, 23, 980, DateTimeKind.Utc);
            var settingCreatedDate = new DateTime(2026, 11, 8, 17, 49, 23, 981, DateTimeKind.Utc);

            builder.HasData(
           new Role { RoleId = 1, RoleName = "Admin", Description = "System Administrator", IsActive = true, CreatedDate = adminCreatedDate, CreatedBy = "System", ModifiedDate = adminModifiedDate, ModifiedBy = "System" },
           new Role { RoleId = 2, RoleName = "User", Description = "Regular user", IsActive = true, CreatedDate = userCreatedDate, CreatedBy = "System", ModifiedDate = userModifiedDate, ModifiedBy = "System" },
           new Role { RoleId = 3, RoleName = "InvoiceManager", Description = "Invoice Manager", IsActive = true, CreatedDate = invoiceManagerCreatedDate, CreatedBy = "System", ModifiedDate = invoiceManagerModifiedDate, ModifiedBy = "System" },
           new Role { RoleId = 4, RoleName = "Invoice Agent", Description = "Invoice Agent", IsActive = true, CreatedDate = invoiceAgentCreatedDate, CreatedBy = "System", ModifiedDate = invoiceAgentModifiedDate, ModifiedBy = "System" }
       );
        }
    }
}

using InvoiceTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InvoiceTracker.Infrastructure.Persistence
{
    public class InvoiceTrackerDbContext : DbContext
    {
        public InvoiceTrackerDbContext(DbContextOptions<InvoiceTrackerDbContext> options)
            : base(options)
        {

        }
        public DbSet<User> Users { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<Setting> Settings { get; set; }

        public DbSet<Audit> Audits { get; set; }

        public DbSet<Company> Companies { get; set; }

        public DbSet<EmailAudit> EmailAudits { get; set; }

        public DbSet<EmailTemplate> EmailTemplates { get; set; }

        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceLineItem> InvoiceLineItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            var adminCreatedDate = new DateTime(2026, 11, 8, 17, 49, 23, 980, DateTimeKind.Utc).AddTicks(4615);
            var adminModifiedDate = new DateTime(2026, 11, 8, 17, 49, 23, 980, DateTimeKind.Utc).AddTicks(4993);
            var userCreatedDate = new DateTime(2026, 11, 8, 17, 49, 23, 980, DateTimeKind.Utc).AddTicks(5503);
            var userModifiedDate = new DateTime(2026, 11, 8, 17, 49, 23, 980, DateTimeKind.Utc).AddTicks(5504);
            var invoiceManagerCreatedDate = new DateTime(2026, 11, 8, 17, 49, 23, 980, DateTimeKind.Utc).AddTicks(5507);
            var invoiceManagerModifiedDate = new DateTime(2026, 11, 8, 17, 49, 23, 980, DateTimeKind.Utc).AddTicks(5508);
            var invoiceAgentCreatedDate = new DateTime(2026, 11, 8, 17, 49, 23, 980, DateTimeKind.Utc).AddTicks(5510);
            var invoiceAgentModifiedDate = new DateTime(2026, 11, 8, 17, 49, 23, 980, DateTimeKind.Utc).AddTicks(5511);
            var settingCreatedDate = new DateTime(2026, 11, 8, 17, 49, 23, 981, DateTimeKind.Utc).AddTicks(531);
            var settingModifiedDate = new DateTime(2026, 11, 8, 17, 49, 23, 981, DateTimeKind.Utc).AddTicks(532);

            // Seed initial products
            modelBuilder.Entity<Role>().HasData(
                new Role { RoleId = 1, RoleName = "Admin", Description = "System Administrator", IsActive = true, CreatedDate = adminCreatedDate, CreatedBy = "System", ModifiedDate = adminModifiedDate, ModifiedBy = "System" },
                new Role { RoleId = 2, RoleName = "User", Description = "Regular user", IsActive = true, CreatedDate = userCreatedDate, CreatedBy = "System", ModifiedDate = userModifiedDate, ModifiedBy = "System" },
                new Role { RoleId = 3, RoleName = "InvoiceManager", Description = "Invoice Manager", IsActive = true, CreatedDate = invoiceManagerCreatedDate, CreatedBy = "System", ModifiedDate = invoiceManagerModifiedDate, ModifiedBy = "System" },
                new Role { RoleId = 4, RoleName = "Invoice Agent", Description = "Invoice Agent", IsActive = true, CreatedDate = invoiceAgentCreatedDate, CreatedBy = "System", ModifiedDate = invoiceAgentModifiedDate, ModifiedBy = "System" }
            );

            //FOR TESTING PERPOSED
            modelBuilder.Entity<Setting>().HasData(
               new Setting { SettingId = 1, Key = "EcryptionKey", Value = "X9#mP7@vL2!qR8$zN5&wK1*Tj6^Ys3%Hu9+Cg4=Db7?Fm2!Qr8@Wx5#Lp1$Zn6^Vk3&Te0", IsActive = true, CreatedBy = "System", CreatedDate = settingCreatedDate, ModifiedDate = settingModifiedDate }
            );
        }


    }
}

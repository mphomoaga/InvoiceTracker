using InvoiceTracker.Domain.Entities;
using InvoiceTracker.Infrastructure.Persistence.Configuration;
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

        public DbSet<Person> Persons { get; set; }

        public DbSet<InvoiceStatus> InvoiceStatuses { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var settingModifiedDate = new DateTime(2026, 11, 8, 17, 49, 23, 981, DateTimeKind.Utc);

            //FOR TESTING PERPOSED. Store the encryption key in a secure vault.
            modelBuilder.Entity<Setting>().HasData(
               new Setting { SettingId = 1, Key = "EcryptionKey", Value = "X9#mP7@vL2!qR8$zN5&wK1*Tj6^Ys3%Hu9+Cg4=Db7?Fm2!Qr8@Wx5#Lp1$Zn6^Vk3&Te0", IsActive = true, CreatedBy = "System", CreatedDate = settingModifiedDate, ModifiedDate = settingModifiedDate });

            modelBuilder.ApplyConfiguration(
                new InvoiceStatusConfiguration());
            modelBuilder.ApplyConfiguration(
                new RolesConfiguration());

        }
    }
}

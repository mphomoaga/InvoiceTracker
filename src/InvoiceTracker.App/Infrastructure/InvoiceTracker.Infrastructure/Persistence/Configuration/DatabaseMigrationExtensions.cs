using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace InvoiceTracker.Infrastructure.Persistence.Configuration
{
    public static class DatabaseMigrationExtensions
    {
        public static async Task ApplyDatabaseMigrationsAsync(
            this IServiceProvider serviceProvider,
            CancellationToken cancellationToken = default)
        {
            using var scope = serviceProvider.CreateScope();

            var dbContext = scope.ServiceProvider
                .GetRequiredService<InvoiceTrackerDbContext>();

            var logger = scope.ServiceProvider
                .GetRequiredService<ILogger<InvoiceTrackerDbContext>>();

            var pendingMigrations = await dbContext.Database
                .GetPendingMigrationsAsync(cancellationToken);

            if (!pendingMigrations.Any())
            {
                logger.LogInformation("No pending database migrations found.");
                return;
            }

            logger.LogInformation("Applying {MigrationCount} pending database migration(s): {Migrations}",
                pendingMigrations.Count(),
                string.Join(", ", pendingMigrations));

            await dbContext.Database.MigrateAsync(cancellationToken);

            logger.LogInformation("Database migrations completed successfully.");
        }
    }
}

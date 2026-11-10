using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace InvoiceTracker.Infrastructure.Persistence;

public sealed class InvoiceTrackerDbContextFactory
    : IDesignTimeDbContextFactory<InvoiceTrackerDbContext>
{
    public InvoiceTrackerDbContext CreateDbContext(string[] args)
    {
        var configuration = BuildConfiguration();
        var connectionString = configuration.GetConnectionString("InvoiceTrackerDatabase");

     
        var options = new DbContextOptionsBuilder<InvoiceTrackerDbContext>()
            .UseSqlServer(connectionString)
            .Options;

        return new InvoiceTrackerDbContext(options);
    }

    private static IConfiguration BuildConfiguration()
    {
        var currentDirectory = Directory.GetCurrentDirectory();
        var webApiDirectory = FindWebApiDirectory(currentDirectory);

        return new ConfigurationBuilder()
            .SetBasePath(webApiDirectory)
            .AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .Build();
    }

    private static string FindWebApiDirectory(string currentDirectory)
    {
        var candidates = new[]
        {
            currentDirectory,
            Path.Combine(currentDirectory, "..", "Presentation", "InvoiceTracker.WebApi"),
            Path.Combine(currentDirectory, "..", "..", "Presentation", "InvoiceTracker.WebApi"),
            Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "..", "Presentation", "InvoiceTracker.WebApi")
        };

        var webApiDirectory = candidates
            .Select(Path.GetFullPath)
            .FirstOrDefault(path => File.Exists(Path.Combine(path, "appsettings.json")));

        return webApiDirectory
            ?? throw new DirectoryNotFoundException(
                "Could not locate the Web API appsettings.json for design-time DbContext creation.");
    }
}

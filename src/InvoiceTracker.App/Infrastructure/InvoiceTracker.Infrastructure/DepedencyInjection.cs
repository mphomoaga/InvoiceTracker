using DinkToPdf;
using DinkToPdf.Contracts;
using InvoiceTracker.Application.Interfaces.Repositories;
using InvoiceTracker.Application.Interfaces.Services;
using InvoiceTracker.Domain.Interfaces;
using InvoiceTracker.Infrastructure.PDFGenerator;
using InvoiceTracker.Infrastructure.Persistence;
using InvoiceTracker.Infrastructure.Repository;
using InvoiceTracker.Infrastructure.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace InvoiceTracker.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureDependency(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<InvoiceTrackerDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });

            services.AddMemoryCache();

            //Repositories
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ISettingRepository, SettingRepository>();
            services.AddScoped<IEncryptionService, EncryptionService>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IEmailTemplateRepository, EmailTemplateRepository>();
            services.AddScoped<IInvoiceRepository, InvoiceRepository>();

            //PDF Generator

            services.AddSingleton<IConverter>(new SynchronizedConverter(new PdfTools()));
            services.AddScoped<IPdfGenerator, PdfGenerator>();

            return services;
        }
    }
}

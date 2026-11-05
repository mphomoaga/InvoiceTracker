using InvoiceTracker.Application.Services;
using InvoiceTracker.Domain.Interfaces.Services;
using InvoiceTracker.Infrastructure.Persistence.DatabaseModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace InvoiceTracker.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));
            services.AddScoped<IUserService, UserManager>();
            services.AddScoped<IInvoiceService, InvoiceManager>();
            services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
            return services;
        }
    }
}

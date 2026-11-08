using InvoiceTracker.Application.Interfaces.Services;
using InvoiceTracker.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace InvoiceTracker.Application
{
    public static class DependecyInjection
    {
        public static IServiceCollection AddApplicationDependency(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthManager>();
            services.AddScoped<IInvoiceService, InvoiceManager>();

            return services;
        }
    }
}
